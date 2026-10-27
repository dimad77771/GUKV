using DevExpress.Web;
using DevExpress.Web.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Cards_AssessmentCard : Page
{
    private int _vid;
    private int? _addressStreetId;
    private readonly JavaScriptSerializer _json = new JavaScriptSerializer { MaxJsonLength = 262144 };
    private readonly Dictionary<ASPxGridView, AssessmentSnapshot> _startingEdits = new Dictionary<ASPxGridView, AssessmentSnapshot>();
    private static readonly Dictionary<string, string> ValueLookups = new Dictionary<string, string> {
        { "addr_district_id", "SELECT name FROM dict_districts2 WHERE id=@id" },
        { "expert_obj_type_id", "SELECT name FROM dict_expert_obj_type WHERE id=@id" },
        { "addr_street_id", "SELECT name FROM dict_streets WHERE id=@id" },
        { "org_balans_id", "SELECT CONCAT(zkpo_code,' (',id,') - ',full_name) FROM organizations WHERE id=@id" },
        { "org_renter_id", "SELECT CONCAT(zkpo_code,' (',id,') - ',full_name) FROM organizations WHERE id=@id" },
        { "expert_id", "SELECT full_name FROM dict_expert WHERE id=@id" },
        { "korrespondent_id", "SELECT name FROM dict_expert_korr WHERE id=@id" },
        { "rezenz_id", "SELECT name FROM dict_expert_rezenz WHERE id=@id" },
        { "rezenz_type_id", "SELECT name FROM dict_expert_rezenz_type WHERE id=@id" }
    };

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        if (Request.QueryString["isnew"] == "1") return;
        // Persist an otherwise empty session as well: snapshot MACs must keep the same
        // purpose on the first callback, before the user has visited any other page.
        if (Session["AssessmentEditorSession"] == null) Session["AssessmentEditorSession"] = true;
        if (!Int32.TryParse(Request.QueryString["vid"], out _vid)) throw new HttpException(400, "Невірний номер картки.");
        foreach (var source in new[] { SqlDataSourceAssessmentInputDoc, SqlDataSourceAssessmentOutputDoc, SqlDataSourceAssessmentDetails })
            source.SelectParameters["vid"].DefaultValue = _vid.ToString(CultureInfo.InvariantCulture);
        foreach (var grid in Grids())
        {
            grid.JSProperties["cpStateKey"] = grid.ID;
            grid.JSProperties["cpEditSnapshot"] = null;
            grid.JSProperties["cpSaveResult"] = null;
            grid.JSProperties["cpRowSnapshots"] = new Dictionary<string, string>();
        }
        using (var connection = Utils.ConnectToDatabase())
        using (var command = new SqlCommand("SELECT * FROM expert_note WHERE id=@id", connection))
        {
            command.Parameters.Add("@id", SqlDbType.Int).Value = _vid;
            var data = new DataTable();
            using (var adapter = new SqlDataAdapter(command)) adapter.Fill(data);
            bool initial = !IsPostBack && !IsCallback;
            if (initial && (data.Rows.Count == 0 || Convert.ToString(data.Rows[0]["is_deleted"]) == "1"))
                throw new HttpException(404, "Картку не знайдено.");
            if (data.Rows.Count != 0 && data.Rows[0]["addr_street_id"] != DBNull.Value)
                _addressStreetId = Convert.ToInt32(data.Rows[0]["addr_street_id"], CultureInfo.InvariantCulture);
            // Build editors before post data is loaded. Saves never re-render these forms.
            ObjDetails.DataSource = data; ObjDetails.DataBind();
            FormView1.DataSource = data; FormView1.DataBind();
            var signature = ObjDetails.FindControlRecursive("FormViewState") as FormView;
            if (signature != null) { signature.DataSource = data; signature.DataBind(); }
            if (initial)
            {
                var schema = AssessmentSchema.Get("expert_note");
                AssessmentEditState.Set("card", Protect(new AssessmentSnapshot {
                    CardId = _vid, RowId = _vid, Table = schema.Table,
                    Values = schema.Fields.ToDictionary(f => f.Name, f => f.Normalize(data.Rows[0][f.Name]))
                }));
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["isnew"] != "1") return;
        if (IsPostBack || IsCallback) throw new HttpException(400, "Невірний запит.");
        int id = checked((int)Utils.GetNextSequenceValue("sequence_expert_note"));
        using (var connection = Utils.ConnectToDatabase())
        using (var command = new SqlCommand("INSERT INTO expert_note(id) VALUES(@id)", connection))
        {
            command.Parameters.Add("@id", SqlDbType.Int).Value = id;
            command.ExecuteNonQuery();
        }
        Response.Redirect("AssessmentCard.aspx?vid=" + id.ToString(CultureInfo.InvariantCulture));
    }

    public sealed class SaveRequest
    {
        public string Action { get; set; }
        public string Snapshot { get; set; }
        public string Pending { get; set; }
        public Dictionary<string, string> Values { get; set; }
        public Dictionary<string, string> Choices { get; set; }
    }

    protected void AssessmentSave_Callback(object sender, CallbackEventArgs e)
    {
        try
        {
            var request = _json.Deserialize<SaveRequest>(e.Parameter);
            AssessmentOperation operation;
            if (request.Action == "save")
            {
                var snapshot = Unprotect<AssessmentSnapshot>(request.Snapshot);
                ValidateSnapshot(snapshot, "expert_note", _vid);
                operation = new AssessmentOperation { Action = "update", Original = snapshot, Values = request.Values };
            }
            else if (request.Action == "resolve")
            {
                var pending = Unprotect<AssessmentSaveResult>(request.Pending);
                ValidateSnapshot(pending.Operation.Original, pending.Operation.Original.Table, pending.Operation.Original.RowId);
                operation = AssessmentCardStore.Resolve(pending, request.Choices ?? new Dictionary<string, string>());
            }
            else throw new ArgumentException("Невірний запит збереження.");
            e.Result = Save(operation);
        }
        catch (Exception ex) { e.Result = SerializeError(ex); }
    }

    protected void AssessmentGrid_StartRowEditing(object sender, ASPxStartRowEditingEventArgs e)
    {
        var grid = (ASPxGridView)sender;
        var schema = Schema(grid);
        var data = (object[])grid.GetRowValuesByKeyValue(e.EditingKeyValue, schema.Fields.Select(f => f.Name).ToArray());
        var values = new Dictionary<string, string>();
        for (int i = 0; i < schema.Fields.Length; i++) values[schema.Fields[i].Name] = schema.Fields[i].Normalize(data[i]);
        var snapshot = new AssessmentSnapshot {
            CardId = _vid, RowId = Convert.ToInt32(e.EditingKeyValue, CultureInfo.InvariantCulture), Table = schema.Table, Values = values
        };
        _startingEdits[grid] = snapshot;
        grid.JSProperties["cpEditSnapshot"] = Protect(snapshot);
    }

    protected void AssessmentGrid_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
    {
        var grid = (ASPxGridView)sender;
        AssessmentSnapshot snapshot;
        if (!_startingEdits.TryGetValue(grid, out snapshot)) return;
        var field = Schema(grid).Fields.FirstOrDefault(f => f.Name == e.Column.FieldName);
        if (field == null) return;
        snapshot.Values[field.Name] = field.Normalize(e.Editor.Value);
        grid.JSProperties["cpEditSnapshot"] = Protect(snapshot);
    }

    protected void AssessmentGrid_InitNewRow(object sender, ASPxDataInitNewRowEventArgs e)
    {
        var grid = (ASPxGridView)sender;
        grid.JSProperties["cpEditSnapshot"] = Protect(new AssessmentSnapshot {
            CardId = _vid, Table = Schema(grid).Table, Values = new Dictionary<string, string>()
        });
    }

    protected void AssessmentGrid_DataBound(object sender, EventArgs e)
    {
        var grid = (ASPxGridView)sender;
        var schema = Schema(grid);
        var tokens = new Dictionary<string, string>();
        // HtmlRowPrepared runs too late for initial JSProperties serialization. Capture the
        // displayed page during DataBound so even a delete without first editing has a baseline.
        string[] names = new[] { "id" }.Concat(schema.Fields.Select(f => f.Name)).ToArray();
        int end = Math.Min(grid.VisibleRowCount, grid.VisibleStartIndex + grid.SettingsPager.PageSize);
        for (int index = grid.VisibleStartIndex; index < end; index++)
        {
            if (grid.IsGroupRow(index)) continue;
            var data = grid.GetRowValues(index, names) as object[];
            if (data == null || data[0] == null || data[0] == DBNull.Value) continue;
            var values = new Dictionary<string, string>();
            for (int i = 0; i < schema.Fields.Length; i++) values[schema.Fields[i].Name] = schema.Fields[i].Normalize(data[i + 1]);
            var snapshot = new AssessmentSnapshot {
                CardId = _vid, RowId = Convert.ToInt32(data[0], CultureInfo.InvariantCulture), Table = schema.Table, Values = values
            };
            tokens[snapshot.RowId.Value.ToString(CultureInfo.InvariantCulture)] = Protect(snapshot);
        }
        grid.JSProperties["cpRowSnapshots"] = tokens;
    }

    protected void AssessmentGrid_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
    {
        e.Cancel = true;
        SaveGrid((ASPxGridView)sender, "update", Convert.ToInt32(e.Keys["id"], CultureInfo.InvariantCulture), e.NewValues);
    }

    protected void AssessmentGrid_RowInserting(object sender, ASPxDataInsertingEventArgs e)
    {
        e.Cancel = true;
        SaveGrid((ASPxGridView)sender, "insert", null, e.NewValues);
    }

    protected void AssessmentGrid_RowDeleting(object sender, ASPxDataDeletingEventArgs e)
    {
        e.Cancel = true;
        SaveGrid((ASPxGridView)sender, "delete", Convert.ToInt32(e.Keys["id"], CultureInfo.InvariantCulture), null);
    }

    protected void AssessmentGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        if (e.Parameters != "resolved") return;
        var grid = (ASPxGridView)sender;
        grid.CancelEdit(); grid.DataBind();
    }

    private void SaveGrid(ASPxGridView grid, string action, int? rowId, IDictionary newValues)
    {
        try
        {
            string key = action == "delete" ? grid.ID + ":row:" + rowId.Value.ToString(CultureInfo.InvariantCulture) : grid.ID + ":edit";
            if (!AssessmentEditState.Contains(key)) throw new ArgumentException("Стан редагування втрачено. Відкрийте рядок повторно.");
            var snapshot = Unprotect<AssessmentSnapshot>(Convert.ToString(AssessmentEditState.Get(key)));
            var schema = Schema(grid);
            ValidateSnapshot(snapshot, schema.Table, rowId);
            var values = new Dictionary<string, string>();
            if (newValues != null)
                foreach (var field in schema.Fields)
                {
                    if (!newValues.Contains(field.Name)) throw new ArgumentException("Відсутнє поле: " + field.Caption);
                    values[field.Name] = field.Normalize(newValues[field.Name]);
                }
            string json = Save(new AssessmentOperation { Action = action, Original = snapshot, Values = values });
            grid.JSProperties["cpSaveResult"] = json;
            var response = _json.Deserialize<Dictionary<string, object>>(json);
            if (!response.ContainsKey("Pending")) { grid.CancelEdit(); grid.DataBind(); }
        }
        catch (Exception ex) { grid.JSProperties["cpSaveResult"] = SerializeError(ex); }
    }

    private string Save(AssessmentOperation operation)
    {
        string username = User != null && User.Identity != null && User.Identity.IsAuthenticated
            ? User.Identity.Name : "System";
        AssessmentSaveResult result;
        using (var connection = Utils.ConnectToDatabase()) result = AssessmentCardStore.Save(connection, operation, username);
        var schema = AssessmentSchema.Get(operation.Original.Table);
        var response = new Dictionary<string, object> { { "Table", schema.Table }, { "Changed", result.Changed } };
        if (operation.Action == "none") response["Message"] = "Залишено поточний стан у базі. Ваші зміни не застосовано.";
        if (result.Conflicts.Count != 0)
        {
            response["Pending"] = Protect(result);
            FormatConflicts(result, schema);
            response["Conflicts"] = result.Conflicts;
            response["Caption"] = schema.Caption + (operation.Original.RowId.HasValue ? " · рядок " + operation.Original.RowId.Value : " · новий документ");
        }
        else if (result.Snapshot != null)
        {
            response["Snapshot"] = Protect(result.Snapshot);
            response["Values"] = result.Snapshot.Values;
            response["DisplayValues"] = DisplayValues(result.Snapshot);
        }
        if (result.SignatureDate.HasValue) response["Signature"] = EvaluateSignature(result.SignatureUser, result.SignatureDate.Value);
        return _json.Serialize(response);
    }

    private Dictionary<string, string> DisplayValues(AssessmentSnapshot snapshot)
    {
        var result = new Dictionary<string, string>();
        if (snapshot.Table != "expert_note") return result;
        using (var connection = Utils.ConnectToDatabase())
            foreach (var lookup in ValueLookups.Where(x => snapshot.Values.ContainsKey(x.Key)))
            {
                string value = snapshot.Values[lookup.Key];
                if (value == null) { result[lookup.Key] = ""; continue; }
                using (var command = new SqlCommand(lookup.Value, connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = Int32.Parse(value, CultureInfo.InvariantCulture);
                    result[lookup.Key] = Convert.ToString(command.ExecuteScalar());
                }
            }
        return result;
    }

    private void FormatConflicts(AssessmentSaveResult result, AssessmentSchema schema)
    {
        using (var connection = Utils.ConnectToDatabase())
            foreach (var conflict in result.Conflicts.Where(c => c.Key != "__missing"))
            {
                if (conflict.Original != "Новий документ") conflict.Original = DescribeValues(connection, schema, conflict.Fields, result.Operation.Original.Values);
                conflict.Current = DescribeValues(connection, schema, conflict.Fields, result.Current);
                if (conflict.Key != "__delete") conflict.Mine = DescribeValues(connection, schema, conflict.Fields, result.Operation.Values);
            }
    }

    private string DescribeValues(SqlConnection connection, AssessmentSchema schema, string[] names, IDictionary<string, string> values)
    {
        return String.Join("\n", names.Select(name => {
            var field = schema.Fields.First(f => f.Name == name);
            string value = values[name], lookup;
            if (value == null) value = "(порожньо)";
            else if (ValueLookups.TryGetValue(name, out lookup))
            {
                using (var command = new SqlCommand(lookup, connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = Int32.Parse(value, CultureInfo.InvariantCulture);
                    string label = Convert.ToString(command.ExecuteScalar());
                    value = String.IsNullOrEmpty(label) ? "ID " + value : label + " (ID " + value + ")";
                }
            }
            else if (field.Kind == "date") value = DateTime.ParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd.MM.yyyy");
            else if (field.Kind == "decimal") value = value.Replace('.', ',');
            else if (field.Kind == "bool") value = value == "1" ? "Так" : "Ні";
            return names.Length == 1 ? value : field.Caption + ": " + value;
        }));
    }

    private void ValidateSnapshot(AssessmentSnapshot snapshot, string table, int? rowId)
    {
        if (snapshot == null || snapshot.CardId != _vid || snapshot.Table != table || snapshot.RowId != rowId)
            throw new ArgumentException("Стан редагування не відповідає рядку. Відкрийте його повторно.");
        AssessmentSchema.Get(table);
    }

    private string Protect(object value)
    {
        return Convert.ToBase64String(MachineKey.Protect(Encoding.UTF8.GetBytes(_json.Serialize(value)),
            "AssessmentCard.Edit.v1", Session.SessionID, _vid.ToString(CultureInfo.InvariantCulture)));
    }

    private T Unprotect<T>(string token)
    {
        try
        {
            byte[] data = MachineKey.Unprotect(Convert.FromBase64String(token),
                "AssessmentCard.Edit.v1", Session.SessionID, _vid.ToString(CultureInfo.InvariantCulture));
            if (data == null) throw new ArgumentException();
            return _json.Deserialize<T>(Encoding.UTF8.GetString(data));
        }
        catch (Exception) { throw new ArgumentException("Стан редагування втрачено. Відкрийте рядок або картку повторно."); }
    }

    private string SerializeError(Exception ex)
    {
        log4net.LogManager.GetLogger("ReportWebSite").Error("AssessmentCard save", ex);
        return _json.Serialize(new { Error = ex is ArgumentException ? ex.Message :
            "Не вдалося зберегти зміни. Перевірте значення та повторіть спробу. Введені дані збережено у формі." });
    }

    private ASPxGridView[] Grids() { return new[] { GridViewAssessmentInputDocs, GridViewAssessmentDetails, GridViewAssessmentOutputDocs }; }

    private AssessmentSchema Schema(ASPxGridView grid)
    {
        return AssessmentSchema.Get(grid == GridViewAssessmentInputDocs ? "expert_input_doc" :
            grid == GridViewAssessmentOutputDocs ? "expert_output_doc" : "expert_note_detail");
    }

    public string EvaluateSignature(object modifiedBy, object modifyDate)
    {
        string userName = modifiedBy as string ?? Resources.Strings.SignatureUnknownUser;
        string date = modifyDate is DateTime ? ((DateTime)modifyDate).ToShortDateString() + " " + ((DateTime)modifyDate).ToShortTimeString() : Resources.Strings.SignatureUnknownDate;
        return String.Format(Resources.Strings.SignatureObjCard, userName, date);
    }

    protected void SqlDataSourceDictBuildings_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters["@street_id"].Value = (object)_addressStreetId ?? DBNull.Value;
    }

    protected void SqlDataSourceDictExpertRezenz_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters["@vid"].Value = _vid;
    }

    protected void ComboBuilding_Callback(object source, CallbackEventArgsBase e)
    {
        int streetId;
        _addressStreetId = Int32.TryParse(e.Parameter, out streetId) ? (int?)streetId : null;
        ((ASPxComboBox)source).DataBind();
    }
}
