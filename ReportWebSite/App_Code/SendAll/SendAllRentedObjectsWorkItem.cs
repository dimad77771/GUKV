using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.Data.SqlClient;
using GUKV;

/// <summary>
/// Summary description for SendAllRentedObjectsWorkItem
/// </summary>
public class SendAllRentedObjectsWorkItem : WorkItem
{
    public SendAllRentedObjectsWorkItem()
    {
        IncludeDeleted = true;
    }

    public override string GetTableName()
    {
        return "reports1nf_arenda_rented";
    }

    public override GUKV.ValidatorBase GetValidator()
    {
        return new RentedObjectValidator();
    }

    public override void Send(SqlConnection connection, int reportID, int id, string userName)
    {
        Reports1NFUtils.SendRentedObject(connection, reportID, id);
    }
}

