using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.Data.SqlClient;
using GUKV;

/// <summary>
/// Summary description for SendAllRentAgreementsWorkItem
/// </summary>
public class SendAllRentAgreementsWorkItem : WorkItem
{
    public SendAllRentAgreementsWorkItem()
    {
        IncludeDeleted = true;
    }

    public override string GetTableName()
    {
        return "reports1nf_arenda";
    }

    public override ValidatorBase GetValidator()
    {
        return new RentingAgreementValidator();
    }

    public override void Send(SqlConnection connection, int reportID, int id, string userName)
    {
        Reports1NFUtils.SendRentAgreement(connection, reportID, ref id);
    }
}

