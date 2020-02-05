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
    }
}