using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Arenda_RestoreRentAgreements : Page
{
    private const string ProblemMessage = "Проблеми зі статусом записів. Зверніться до адміністратора.";
    private const string NotDeletedMessage = "Договір не видалено";
    private const string ChangedMessage = "Дані договору змінилися. Виконайте пошук ще раз.";

    // Read the source tables: the usual agreement views exclude deleted records.
    // Count every row for the ID, including rows belonging to different reports.
    private const string SearchSql = @"
WITH AgreementSources AS
(
    SELECT a.id, a.org_balans_id, a.org_renter_id, a.org_giver_id,
           a.agreement_num, a.agreement_date, 0 AS source_order, 0 AS report_id
    FROM dbo.arenda a {0}
    UNION ALL
    SELECT a.id, a.org_balans_id, a.org_renter_id, a.org_giver_id,
           a.agreement_num, a.agreement_date, 1 AS source_order, a.report_id
    FROM dbo.reports1nf_arenda a {0}
), MatchingAgreements AS
(
    SELECT a.id, holder.full_name AS org_balans_full_name,
           holder.zkpo_code AS org_balans_zkpo,
           renter.short_name AS org_renter_short_name,
           giver.short_name AS org_giver_short_name,
           a.agreement_num, a.agreement_date,
           ROW_NUMBER() OVER (PARTITION BY a.id ORDER BY a.source_order, a.report_id) AS row_num
    FROM AgreementSources a
    INNER JOIN dbo.organizations holder ON holder.id = a.org_balans_id
    LEFT JOIN dbo.organizations renter ON renter.id = a.org_renter_id
    LEFT JOIN dbo.organizations giver ON giver.id = a.org_giver_id
    WHERE holder.zkpo_code = @holder_code
      AND a.agreement_num = @agreement_num
      AND (@id IS NULL OR a.id = @id)
      AND
      (
          @rda_district_id = 0
          OR (holder.addr_distr_new_id = @rda_district_id AND holder.form_ownership_id IN
              (SELECT id FROM dbo.dict_org_ownership WHERE is_rda = 1))
          OR (renter.addr_distr_new_id = @rda_district_id AND renter.form_ownership_id IN
              (SELECT id FROM dbo.dict_org_ownership WHERE is_rda = 1))
          OR (giver.addr_distr_new_id = @rda_district_id AND giver.form_ownership_id IN
              (SELECT id FROM dbo.dict_org_ownership WHERE is_rda = 1))
      )
)
SELECT a.id, a.org_balans_full_name, a.org_balans_zkpo,
       a.org_renter_short_name, a.org_giver_short_name,
       a.agreement_num, a.agreement_date,
       reports.row_count AS reports_count, reports.is_deleted AS reports_is_deleted,
       rent.row_count AS arenda_count, rent.is_deleted AS arenda_is_deleted
FROM MatchingAgreements a
CROSS APPLY
(
    SELECT COUNT(*) AS row_count, MAX(r.is_deleted) AS is_deleted
    FROM dbo.reports1nf_arenda r {0} WHERE r.id = a.id
) reports
CROSS APPLY
(
    SELECT COUNT(*) AS row_count, MAX(r.is_deleted) AS is_deleted
    FROM dbo.arenda r {0} WHERE r.id = a.id
) rent
WHERE a.row_num = 1
ORDER BY a.agreement_date, a.id";

    protected override void OnInit(EventArgs e)
    {
        ViewStateUserKey = Session.SessionID;
        base.OnInit(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetNoStore();
    }

    protected void ButtonSearch_Click(object sender, EventArgs e)
    {
        ClearConfirmation();
        ClearResults();
        LabelMessage.Visible = false;

        string holderCode = TextHolderCode.Text.Trim();
        string agreementNumber = TextAgreementNumber.Text.Trim();
        if (holderCode.Length == 0 || agreementNumber.Length == 0)
        {
            ShowMessage("Введіть код ЄДРПОУ балансоутримувача та номер договору оренди.", false);
            return;
        }

        if (holderCode.Length > 16 || agreementNumber.Length > 18)
        {
            ShowMessage("Перевірте довжину коду ЄДРПОУ та номера договору оренди.", false);
            return;
        }

        ViewState["SearchHolderCode"] = holderCode;
        ViewState["SearchAgreementNumber"] = agreementNumber;
        BindSearchResults();
    }

    protected void GridAgreements_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName != "RestoreAgreement")
            return;

        ClearConfirmation();
        LabelMessage.Visible = false;

        int agreementId;
        if (!int.TryParse(Convert.ToString(e.CommandArgument, CultureInfo.InvariantCulture),
            NumberStyles.Integer, CultureInfo.InvariantCulture, out agreementId))
        {
            ShowMessage(ChangedMessage, false);
            return;
        }

        try
        {
            using (SqlConnection connection = OpenConnection())
            {
                DataTable agreements = LoadAgreements(connection, null, agreementId);
                if (agreements.Rows.Count != 1)
                {
                    BindSearchResults();
                    ShowMessage(ChangedMessage, false);
                    return;
                }

                DataRow agreement = agreements.Rows[0];
                string status = GetStatusMessage(agreement);
                if (status.Length != 0)
                {
                    BindSearchResults();
                    ShowMessage(status, false);
                    return;
                }

                ViewState["PendingAgreementId"] = agreementId;
                ViewState["PendingAgreementNumber"] = Convert.ToString(agreement["agreement_num"]);
                ViewState["PendingAgreementDate"] = GetAgreementDate(agreement);
                LiteralAgreementNumber.Text = Convert.ToString(agreement["agreement_num"]);
                LiteralAgreementDate.Text = agreement.IsNull("agreement_date")
                    ? "Не вказано"
                    : ((DateTime)agreement["agreement_date"]).ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
                PopupRestore.ShowOnPageLoad = true;
            }
        }
        catch (Exception exception)
        {
            ReportError(exception);
        }
    }

    protected void ButtonConfirmRestore_Click(object sender, EventArgs e)
    {
        PopupRestore.ShowOnPageLoad = false;
        object pendingId = ViewState["PendingAgreementId"];
        if (!(pendingId is int))
        {
            ClearConfirmation();
            ShowMessage("Оберіть договір для відновлення.", false);
            return;
        }

        string failureMessage = null;
        try
        {
            using (SqlConnection connection = OpenConnection())
            using (SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable))
            {
                // Keep both source rows and their ID ranges locked until all three updates finish.
                DataTable agreements = LoadAgreements(connection, transaction, (int)pendingId);
                if (agreements.Rows.Count != 1)
                {
                    failureMessage = ChangedMessage;
                }
                else
                {
                    DataRow agreement = agreements.Rows[0];
                    string status = GetStatusMessage(agreement);
                    if (status.Length != 0)
                        failureMessage = status;
                    else if (!object.Equals(ViewState["PendingAgreementNumber"], Convert.ToString(agreement["agreement_num"]))
                        || !object.Equals(ViewState["PendingAgreementDate"], GetAgreementDate(agreement)))
                        failureMessage = ChangedMessage;
                }

                if (failureMessage != null)
                {
                    transaction.Rollback();
                }
                else
                {
                    const string updateSql = @"
UPDATE dbo.reports1nf_arenda SET is_deleted = 0 WHERE id = @id;
UPDATE dbo.arenda SET is_deleted = 0 WHERE id = @id;
UPDATE dbo.arch_arenda SET is_deleted = 0 WHERE id = @id;";
                    using (SqlCommand command = new SqlCommand(updateSql, connection, transaction))
                    {
                        command.Parameters.Add("@id", SqlDbType.Int).Value = (int)pendingId;
                        command.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
            }

            ClearConfirmation();
            if (failureMessage != null)
            {
                BindSearchResults();
                ShowMessage(failureMessage, false);
                return;
            }

            TextHolderCode.Text = string.Empty;
            TextAgreementNumber.Text = string.Empty;
            ClearResults();
            ShowMessage("Договір оренди успішно відновлено.", true);
        }
        catch (Exception exception)
        {
            ClearConfirmation();
            ReportError(exception);
        }
    }

    protected void ButtonCancelRestore_Click(object sender, EventArgs e)
    {
        ClearConfirmation();
    }

    private DataTable LoadAgreements(SqlConnection connection, SqlTransaction transaction, int? agreementId)
    {
        string holderCode = ViewState["SearchHolderCode"] as string;
        string agreementNumber = ViewState["SearchAgreementNumber"] as string;
        if (string.IsNullOrEmpty(holderCode) || string.IsNullOrEmpty(agreementNumber))
            return new DataTable();

        string lockHint = transaction == null ? string.Empty : "WITH (UPDLOCK, HOLDLOCK)";
        using (SqlCommand command = new SqlCommand(string.Format(CultureInfo.InvariantCulture, SearchSql, lockHint), connection, transaction))
        {
            command.Parameters.Add("@holder_code", SqlDbType.VarChar, 16).Value = holderCode;
            command.Parameters.Add("@agreement_num", SqlDbType.VarChar, 18).Value = agreementNumber;
            command.Parameters.Add("@id", SqlDbType.Int).Value = agreementId.HasValue ? (object)agreementId.Value : DBNull.Value;
            command.Parameters.Add("@rda_district_id", SqlDbType.Int).Value = Utils.RdaDistrictID;

            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                DataTable agreements = new DataTable();
                adapter.Fill(agreements);
                return agreements;
            }
        }
    }

    private void BindSearchResults()
    {
        try
        {
            using (SqlConnection connection = OpenConnection())
            {
                DataTable agreements = LoadAgreements(connection, null, null);
                agreements.Columns.Add("can_restore", typeof(bool));
                agreements.Columns.Add("status_message", typeof(string));
                foreach (DataRow agreement in agreements.Rows)
                {
                    string status = GetStatusMessage(agreement);
                    agreement["can_restore"] = status.Length == 0;
                    agreement["status_message"] = status;
                }

                GridAgreements.DataSource = agreements;
                GridAgreements.Visible = true;
                GridAgreements.DataBind();
            }
        }
        catch (Exception exception)
        {
            GridAgreements.Visible = false;
            ReportError(exception);
        }
    }

    private static string GetStatusMessage(DataRow agreement)
    {
        if ((int)agreement["reports_count"] != 1 || (int)agreement["arenda_count"] != 1)
            return ProblemMessage;

        if (object.Equals(agreement["reports_is_deleted"], 1) || object.Equals(agreement["arenda_is_deleted"], 1))
            return string.Empty;

        if (object.Equals(agreement["reports_is_deleted"], 0) && object.Equals(agreement["arenda_is_deleted"], 0))
            return NotDeletedMessage;

        return ProblemMessage;
    }

    private static object GetAgreementDate(DataRow agreement)
    {
        return agreement.IsNull("agreement_date") ? null : agreement["agreement_date"];
    }

    private static SqlConnection OpenConnection()
    {
        SqlConnection connection = Utils.ConnectToDatabase();
        if (connection == null)
            throw new InvalidOperationException("Не вдалося підключитися до бази даних.");
        return connection;
    }

    private void ClearConfirmation()
    {
        PopupRestore.ShowOnPageLoad = false;
        ViewState.Remove("PendingAgreementId");
        ViewState.Remove("PendingAgreementNumber");
        ViewState.Remove("PendingAgreementDate");
        LiteralAgreementNumber.Text = string.Empty;
        LiteralAgreementDate.Text = string.Empty;
    }

    private void ClearResults()
    {
        ViewState.Remove("SearchHolderCode");
        ViewState.Remove("SearchAgreementNumber");
        GridAgreements.DataSource = null;
        GridAgreements.DataBind();
        GridAgreements.Visible = false;
    }

    private void ShowMessage(string message, bool success)
    {
        LabelMessage.Text = message;
        LabelMessage.ForeColor = success ? System.Drawing.Color.Green : System.Drawing.Color.Red;
        LabelMessage.Visible = true;
    }

    private void ReportError(Exception exception)
    {
        System.Diagnostics.Trace.TraceError("Rent agreement restoration failed: {0}", exception);
        ShowMessage("Не вдалося виконати операцію. Спробуйте ще раз або зверніться до адміністратора.", false);
    }
}
