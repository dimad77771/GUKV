using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using GUKV;
using System.Threading;

/// <summary>
/// Summary description for SendAllBalansObjectsWorkItem
/// </summary>
public class SendAllBalansObjectsWorkItem : WorkItem
{
    public SendAllBalansObjectsWorkItem()
    {
        // Every pending object must be submitted before the batch can succeed.
        SkipInvalidObjects = false;
    }

    public override string GetTableName()
    {
        return "reports1nf_balans";
    }

    public override ValidatorBase GetValidator()
    {
        return new BalansObjectValidator();
    }

    public override void Send(SqlConnection connection, int reportID, int id, string userName)
    {
        Reports1NFUtils.SendBalansObject(connection, reportID, id, userName);

        // SendBalansObject returns without throwing when DB validation fails.
        // Confirm submission before counting this object as processed.
        using (SqlCommand cmd = new SqlCommand(@"SELECT CASE WHEN is_valid = 1
                AND submit_date IS NOT NULL AND (modify_date IS NULL OR modify_date <= submit_date)
                THEN 1 ELSE 0 END FROM reports1nf_balans WHERE report_id = @rid AND id = @bid", connection))
        {
            cmd.Parameters.Add(new SqlParameter("rid", reportID));
            cmd.Parameters.Add(new SqlParameter("bid", id));
            object result = cmd.ExecuteScalar();
            if (!(result is int) || (int)result != 1)
                throw new InvalidOperationException("Зміни об'єкта не були надіслані до ДКВ.");
        }
    }

    public override string GetFailureMessage(int? objectId, Exception error)
    {
        if (!objectId.HasValue)
            return base.GetFailureMessage(objectId, error);

        return string.Format("Масове надсилання зупинено через помилку під час надсилання об'єкта. ID об'єкту: {0}. " +
            "Відкрийте цей об'єкт і надішліть його натисканням кнопки «Надіслати», " +
            "а потім поверніться до масового надсилання.", objectId.Value);
    }

    public override string GetFailureUrl(int? objectId)
    {
        return objectId.HasValue
            ? string.Format("~/Reports1NF/OrgBalansObject.aspx?rid={0}&bid={1}", reportID, objectId.Value)
            : string.Empty;
    }
}
