using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.Data.SqlClient;
using GUKV;

/// <summary>
/// Summary description for SendAllBalansDeletedObjectsWorkItem
/// </summary>
public class SendAllBalansDeletedObjectsWorkItem : WorkItem
{
    public SendAllBalansDeletedObjectsWorkItem()
    {
        IncludeDeleted = true;
        ValidateDeleted = true;
    }

    public override string GetTableName()
    {
        return "reports1nf_balans_deleted";
    }

    public override ValidatorBase GetValidator()
    {
        return new DeletedObjectValidator();
    }

    public override void Send(SqlConnection connection, int reportID, int id, string userName)
    {
        Reports1NFUtils.SendBalansDeletedObject(connection, reportID, id);
    }
}