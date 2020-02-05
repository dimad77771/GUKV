using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using FirebirdSql.Data.FirebirdClient;
using GUKV.Conveyancing;
using log4net;
using GUKV.ImportToolUtils;


/// <summary>
/// Summary description for ConveyancingUtils
/// </summary>
public static class ConveyancingUtils
{
    private static readonly ILog log = LogManager.GetLogger("ReportWebSite");

    public static int CreateUnverifiedBalansObject(int building_id, decimal sqr_total, decimal cost_balans, int form_ownership_id, int object_kind_id, int object_type_id, int purpose_group_id, int purpose_id, string purpose_str)
    {
        SqlConnection connectionSql = Utils.ConnectToDatabase();
        try
        {
            using (SqlCommand cmd = new SqlCommand("INSERT INTO unverified_balans (building_id, sqr_total, cost_balans, form_ownership_id, object_kind_id, object_type_id, purpose_group_id, purpose_id, purpose_str)"
                + "VALUES(@building_id, @sqr_total, @cost_balans, @form_ownership_id, @object_kind_id, @object_type_id, @purpose_group_id, @purpose_id, @purpose_str); SELECT CAST(SCOPE_IDENTITY() AS int)", connectionSql))
            {
                cmd.Parameters.Add(new SqlParameter("building_id", building_id));
                cmd.Parameters.Add(new SqlParameter("sqr_total", sqr_total));
                cmd.Parameters.Add(new SqlParameter("cost_balans", cost_balans));
                cmd.Parameters.Add(new SqlParameter("form_ownership_id", form_ownership_id));
                cmd.Parameters.Add(new SqlParameter("object_kind_id", object_kind_id));
                cmd.Parameters.Add(new SqlParameter("object_type_id", object_type_id));
                cmd.Parameters.Add(new SqlParameter("purpose_group_id", purpose_group_id));
                cmd.Parameters.Add(new SqlParameter("purpose_id", purpose_id));
                cmd.Parameters.Add(new SqlParameter("purpose_str", purpose_str));
                Int32 newId = (Int32)cmd.ExecuteScalar();
                return newId;
            }
        }
        catch (Exception ex)
        {
            return -1;
        }
    }

    // правильно ли указан балансоутримувач
    public static bool IsCorrectOwner(int balans_id, int organization_id)
    {
        var result = false;
        SqlConnection connectionSql = Utils.ConnectToDatabase();
        try
        {
            using (SqlCommand cmd = new SqlCommand("SELECT sqr_total FROM balans "
                + "WHERE id = @balans_id AND organization_id = @organization_id", connectionSql))
            {
                cmd.Parameters.Add(new SqlParameter("balans_id", balans_id));
                cmd.Parameters.Add(new SqlParameter("organization_id", organization_id));

                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        result = true;
                        break;
                    }
                }
            }
            connectionSql.Close();
        }
        catch (Exception ex)
        {
            log.Error(ex.Message);
        }

        return result;
    }

    //public static void TransferBetweenOrgs(int orgFromId, int orgToId, int objectId, int balansId, decimal sqr)
    //{
    //    Preferences preferences = new Preferences();
    //    var balansTransfer = new BalansTransfer();
    //    balansTransfer.transferType = ObjectTransferType.Transfer;
    //    balansTransfer.objectId_1NF = objectId;
    //    balansTransfer.sqr = sqr;
    //    balansTransfer.organizationFromId_1NF = orgFromId;
    //    balansTransfer.organizationToId_1NF = orgToId;
    //    balansTransfer.balansId_1NF = balansId;

    //    var actObject = new ActObject();
    //    actObject.makeChangesIn1NF = true;
    //    actObject.balansTransfers.Add(balansTransfer);

    //    var importedAct = new ImportedAct();
    //    importedAct.actObjects.Add(actObject);


    //    var rish = new DocumentNJF();
    //    rish.documentDate = DateTime.Now;

    //    //FbConnection connection1NF = new FbConnection(preferences.Get1NFConnectionString());
    //    //FbTransaction transaction1NF = connection1NF.BeginTransaction();
    //    SqlConnection connectionSql = new SqlConnection(preferences.GetSqlServerConnectionString());
    //    SqlTransaction transactionSql = connectionSql.BeginTransaction();

    //    GUKV.Conveyancing.DB.TransferBalansObjects(/*connection1NF, transaction1NF,*/ connectionSql, transactionSql, preferences, importedAct, rish, true);
    //}

    //public static void DestroyBalansObj(int balansId, int orgFromId, int objectId)
    //{
    //    Preferences preferences = new Preferences();
    //    var balansTransfer = new BalansTransfer();
    //    balansTransfer.transferType = ObjectTransferType.Destroy;
    //    balansTransfer.balansId_1NF = balansId; // 47106;
    //    balansTransfer.organizationFromId_1NF = orgFromId; // 26819;
    //    balansTransfer.objectId_1NF = objectId; // 12418;

    //    var actObject = new ActObject();
    //    actObject.makeChangesIn1NF = true;
    //    actObject.balansTransfers.Add(balansTransfer);

    //    var importedAct = new ImportedAct();
    //    importedAct.actObjects.Add(actObject);

    //    var rish = new DocumentNJF();
    //    rish.documentDate = DateTime.Now;

    //    //FbConnection connection1NF = new FbConnection(preferences.Get1NFConnectionString());
    //    //FbTransaction transaction1NF = connection1NF.BeginTransaction();
    //    SqlConnection connectionSql = new SqlConnection(preferences.GetSqlServerConnectionString());
    //    SqlTransaction transactionSql = connectionSql.BeginTransaction();

    //    GUKV.Conveyancing.DB.TransferBalansObjects(/*connection1NF, transaction1NF,*/ connectionSql, transactionSql, preferences, importedAct, rish, true);
    //}

    //public static void TestDB()
    //{
    //    Preferences preferences = new Preferences();

    //    Transfer transfer = new Transfer();
    //    transfer.organizationIdFrom = 1;
    //    transfer.rightId = 2;
    //    transfer.organizationIdTo = 25;

    //    AppendixObject appObj = new AppendixObject();
    //    appObj.objectId = 28575;
    //    appObj.transfers.Add(transfer);

    //    Appendix app = new Appendix();
    //    app.objects.Add(appObj);

    //    ImportedDoc doc = new ImportedDoc();
    //    doc.docTypeId = 1;
    //    doc.docDate = DateTime.Now;
    //    doc.appendices.Add(app);
    //    //GUKV.Conveyancing.DB.ExportDocument(preferences, doc);



    //    /* НЕ УДАЛЯТЬ! Передача от одного бал-ча другому
    //    Preferences preferences = new Preferences();
    //    var balansTransfer = new BalansTransfer();
    //    balansTransfer.transferType = ObjectTransferType.Transfer;
    //    balansTransfer.objectId_1NF = 39704;
    //    balansTransfer.sqr = 50;
    //    balansTransfer.organizationFromId_1NF = 49;
    //    balansTransfer.organizationToId_1NF = 51;
    //    balansTransfer.balansId_1NF = 47045;

    //    var actObject = new ActObject();
    //    actObject.makeChangesIn1NF = true;
    //    actObject.balansTransfers.Add(balansTransfer);

    //    var importedAct = new ImportedAct();
    //    importedAct.actObjects.Add(actObject);
        

    //    var rish = new DocumentNJF();
    //    rish.documentDate = DateTime.Now;

    //    GUKV.Conveyancing.DB.TransferBalansObjects(preferences, importedAct, rish, true);
    //    */


    //    /* НЕ УДАЛЯТЬ. отчуждение об-та */
    //    /*Preferences preferences = new Preferences();
        
    //    var balansTransfer = new BalansTransfer();
    //    balansTransfer.transferType = ObjectTransferType.Destroy;
    //    //balansTransfer.objectId_1NF = 47106;
    //    //balansTransfer.sqr = 50;
    //    //balansTransfer.organizationFromId_1NF = 49;
    //    //balansTransfer.organizationToId_1NF = 51;
    //    balansTransfer.balansId_1NF = 47106;
    //    balansTransfer.organizationFromId_1NF = 26819;
    //    balansTransfer.objectId_1NF = 12418;

    //    var actObject = new ActObject();
    //    actObject.makeChangesIn1NF = true;
    //    actObject.balansTransfers.Add(balansTransfer);

    //    var importedAct = new ImportedAct();
    //    importedAct.actObjects.Add(actObject);


    //    var rish = new DocumentNJF();
    //    rish.documentDate = DateTime.Now;

    //    GUKV.Conveyancing.DB.TransferBalansObjects(preferences, importedAct, rish, true);*/

    //    /*

    //    Preferences preferences = new Preferences();



    //    var balansTransfer = new BalansTransfer();
    //    balansTransfer.transferType = ObjectTransferType.Create;
    //    balansTransfer.objectId_1NF = 39704;

    //    balansTransfer.sqr = 50;
    //    balansTransfer.organizationFromId_1NF = 49;
    //    balansTransfer.organizationToId_1NF = 51;
    //    balansTransfer.balansId_1NF = 47045;

    //    var actObject = new ActObject();
    //    actObject.makeChangesIn1NF = true;
    //    actObject.balansTransfers.Add(balansTransfer);

    //    var importedAct = new ImportedAct();
    //    importedAct.actObjects.Add(actObject);


    //    var rish = new DocumentNJF();
    //    rish.documentDate = DateTime.Now;
    //    GUKV.Conveyancing.DB.ExportAct(preferences, importedAct, rish);
    //    GUKV.Conveyancing.DB.TransferBalansObjects(preferences, importedAct, rish, true);*/
    //    //GUKV.Conveyancing.DB.ExportAct(preferences, importedAct, rish);





    //    //GUKV.Conveyancing.DB.CreateBalansObjectNew(preferences, 39626, 10, 5870);
    //    //GUKV.Conveyancing.DB.CreateNew1NFObject(preferences, 1, "wqeqw", 1, "22", "www", "sas", "sd");

    //    //GUKV.Conveyancing.DB.LoadData(preferences);

    //}

    //public static int CreateNew1NFObject(FbConnection connection, int buildingId, int orgId, string square,
    //    string cost, int ownershipId, int objKindId, int objTypeId, int purposeGroupId, int purposeId,
    //    string use, out string errorMessage)
    //{
    //    bool duplicateInSqlServer = true;

    //    if (connection == null)
    //        throw new ArgumentNullException("connection");

    //    errorMessage = string.Empty;
    //    int newBalansId = -1;

    //    System.Web.Security.MembershipUser user = System.Web.Security.Membership.GetUser();
    //    string username = (user == null ? "System" : user.UserName);

    //    // Make all changes to the Firebird database in a transaction
    //    FbTransaction transaction = null;

    //    try
    //    {
    //        if (buildingId <= 0)
    //            throw new ArgumentException("Адреса не вказана");
    //        if (orgId <= 0)
    //            throw new ArgumentException("Організація не вказана");

    //        transaction = connection.BeginTransaction();

    //        // Convert all numbers provided as string to actual numbers
    //        decimal objectSquare = Utils.ConvertStrToDecimal(square);
    //        decimal objectCost = Utils.ConvertStrToDecimal(cost);

    //        // Generate new balans Id
    //        newBalansId = GenerateNewId(connection, "GEN_BALANS_1NF", transaction);

    //        if (newBalansId < 0)
    //        {
    //            throw new InvalidOperationException("Помилка при створенні ідентифікатора нового об'єкту на балансі в базі 1НФ.");
    //        }

    //        // Get address properties from the 1NF database
    //        string objUlname, objNomer1, objNomer2, objNomer3, objKodbti;
    //        int objKodul;

    //        using (FbCommand command = connection.CreateCommand())
    //        {
    //            command.Transaction = transaction;
    //            command.CommandType = System.Data.CommandType.Text;
    //            command.CommandTimeout = 600;
    //            command.CommandText = "SELECT ULNAME, NOMER1, NOMER2, NOMER3, KODBTI, ULKOD FROM OBJECT_1NF WHERE OBJECT_KOD = @buildingId AND OBJECT_KODSTAN = 1";

    //            command.Parameters.Add(new FbParameter("buildingId", buildingId));

    //            using (System.Data.IDataReader reader = command.ExecuteReader())
    //            {
    //                if (!reader.Read())
    //                    throw new InvalidOperationException("Вказаного будинку не знайдено в базі 1НФ.");

    //                objUlname = (reader["ULNAME"] as string) ?? string.Empty;
    //                objNomer1 = (reader["NOMER1"] as string) ?? string.Empty;
    //                objNomer2 = (reader["NOMER2"] as string) ?? string.Empty;
    //                objNomer3 = (reader["NOMER3"] as string) ?? string.Empty;
    //                objKodbti = (reader["KODBTI"] as string) ?? string.Empty;
    //                objKodul = (reader["ULKOD"] is int ? (int)reader["ULKOD"] : -1);

    //                reader.Close();
    //            }
    //        }

    //        // Make sure that all strings are 'upper case'
    //        use = use.Trim().ToUpper();
    //        objUlname = objUlname.Trim().ToUpper();
    //        objNomer1 = objNomer1.Trim().ToUpper();
    //        objNomer2 = objNomer2.Trim().ToUpper();
    //        objNomer3 = objNomer3.Trim().ToUpper();
    //        objKodbti = objKodbti.Trim().ToUpper();

    //        // Generate the INSERT statement
    //        string fieldList = string.Empty;
    //        string paramList = string.Empty;
    //        Dictionary<string, object> parameters = new Dictionary<string, object>();

    //        AddQueryParam("bid", "ID", newBalansId, ref fieldList, ref paramList, parameters, -1);
    //        AddQueryParam("stn", "STAN", DateTime.Now, ref fieldList, ref paramList, parameters, -1);
    //        AddQueryParam("objid", "OBJECT", buildingId, ref fieldList, ref paramList, parameters, -1);
    //        AddQueryParam("objstn", "OBJECT_STAN", 1, ref fieldList, ref paramList, parameters, -1);
    //        AddQueryParam("orgid", "ORG", orgId, ref fieldList, ref paramList, parameters, -1);
    //        AddQueryParam("orgstn", "ORG_STAN", 1, ref fieldList, ref paramList, parameters, -1);
    //        AddQueryParam("isp", "ISP", username, ref fieldList, ref paramList, parameters, 18);
    //        AddQueryParam("dt", "DT", DateTime.Now.Date, ref fieldList, ref paramList, parameters, -1);
    //        AddQueryParam("edt", "EDT", DateTime.Now.Date, ref fieldList, ref paramList, parameters, -1);
    //        AddQueryParam("lyear", "LYEAR", DateTime.Now.Year, ref fieldList, ref paramList, parameters, -1);
    //        AddQueryParam("pr1nf", "PRIZNAK1NF", 0, ref fieldList, ref paramList, parameters, -1); // 0 ~ NOT from 1NF
    //        AddQueryParam("upds", "UPD_SOURCE", 10, ref fieldList, ref paramList, parameters, -1); // BAGRIY V.M.
    //        AddQueryParam("delt", "DELETED", 0, ref fieldList, ref paramList, parameters, -1);

    //        AddQueryParam("addrstr", "OBJ_ULNAME", objUlname, ref fieldList, ref paramList, parameters, 100);
    //        AddQueryParam("addrnom1", "OBJ_NOMER1", objNomer1, ref fieldList, ref paramList, parameters, 9);
    //        AddQueryParam("addrnom2", "OBJ_NOMER2", objNomer2, ref fieldList, ref paramList, parameters, 18);
    //        AddQueryParam("addrnom3", "OBJ_NOMER3", objNomer3, ref fieldList, ref paramList, parameters, 10);
    //        AddQueryParam("addrbti", "OBJ_KODBTI", objKodbti, ref fieldList, ref paramList, parameters, 18);
    //        AddQueryParam("addrsid", "OBJ_KODUL", objKodul, ref fieldList, ref paramList, parameters, -1);

    //        AddQueryParam("sqrtot", "SQR_ZAG", objectSquare, ref fieldList, ref paramList, parameters, -1);
    //        AddQueryParam("balv", "BALANS_VARTIST", objectCost, ref fieldList, ref paramList, parameters, -1);
    //        AddQueryParam("fvl", "FORM_VLASN", ownershipId, ref fieldList, ref paramList, parameters, -1);
    //        AddQueryParam("knd", "KINDOBJ", objKindId, ref fieldList, ref paramList, parameters, -1);
    //        AddQueryParam("typ", "TYPEOBJ", objTypeId, ref fieldList, ref paramList, parameters, -1);
    //        AddQueryParam("pgr", "GRPURP", purposeGroupId, ref fieldList, ref paramList, parameters, -1);
    //        AddQueryParam("prp", "PURPOSE", purposeId, ref fieldList, ref paramList, parameters, -1);
    //        AddQueryParam("prpstr", "PURP_STR", use, ref fieldList, ref paramList, parameters, 255);

    //        using (FbCommand command = connection.CreateCommand())
    //        {
    //            command.Transaction = transaction;
    //            command.CommandType = System.Data.CommandType.Text;
    //            command.CommandTimeout = 600;
    //            command.CommandText = "INSERT INTO BALANS_1NF (" + fieldList.TrimStart(' ', ',')
    //                + ") VALUES (" + paramList.TrimStart(' ', ',') + ")";

    //            foreach (KeyValuePair<string, object> pair in parameters)
    //            {
    //                command.Parameters.Add(new FbParameter(pair.Key, pair.Value));
    //            }

    //            command.ExecuteNonQuery();
    //        }

    //        // Commit the transaction
    //        transaction.Commit();
    //        transaction = null; // This will prevent an undesired Rollback() in the catch{} section

    //        // Duplicate the balans object in SQL server, if requested
    //        if (duplicateInSqlServer)
    //        {
    //            parameters.Clear();
    //            fieldList = "";
    //            paramList = "";

    //            AddQueryParam("bid", "id", newBalansId, ref fieldList, ref paramList, parameters, -1);
    //            AddQueryParam("objid", "building_id", buildingId, ref fieldList, ref paramList, parameters, -1);
    //            AddQueryParam("orgid", "organization_id", orgId, ref fieldList, ref paramList, parameters, -1);
    //            AddQueryParam("isp", "modified_by", username, ref fieldList, ref paramList, parameters, 128);
    //            AddQueryParam("dt", "modify_date", DateTime.Now.Date, ref fieldList, ref paramList, parameters, -1);
    //            AddQueryParam("lyear", "year_balans", DateTime.Now.Year, ref fieldList, ref paramList, parameters, -1);
    //            AddQueryParam("pr1nf", "priznak_1nf", 0, ref fieldList, ref paramList, parameters, -1); // 0 ~ NOT from 1NF
    //            AddQueryParam("upds", "update_src_id", 10, ref fieldList, ref paramList, parameters, -1); // BAGRIY V.M.
    //            AddQueryParam("delt", "is_deleted", 0, ref fieldList, ref paramList, parameters, -1);

    //            AddQueryParam("addrstr", "obj_street_name", objUlname, ref fieldList, ref paramList, parameters, 100);
    //            AddQueryParam("addrnom1", "obj_nomer1", objNomer1, ref fieldList, ref paramList, parameters, 9);
    //            AddQueryParam("addrnom2", "obj_nomer2", objNomer2, ref fieldList, ref paramList, parameters, 18);
    //            AddQueryParam("addrnom3", "obj_nomer3", objNomer3, ref fieldList, ref paramList, parameters, 10);
    //            AddQueryParam("addrbti", "obj_bti_code", objKodbti, ref fieldList, ref paramList, parameters, 18);
    //            AddQueryParam("addrsid", "obj_street_id", objKodul, ref fieldList, ref paramList, parameters, -1);

    //            AddQueryParam("sqrtot", "sqr_total", objectSquare, ref fieldList, ref paramList, parameters, -1);
    //            AddQueryParam("balv", "cost_balans", objectCost, ref fieldList, ref paramList, parameters, -1);
    //            AddQueryParam("fvl", "form_ownership_id", ownershipId, ref fieldList, ref paramList, parameters, -1);
    //            AddQueryParam("knd", "object_kind_id", objKindId, ref fieldList, ref paramList, parameters, -1);
    //            AddQueryParam("typ", "object_type_id", objTypeId, ref fieldList, ref paramList, parameters, -1);
    //            AddQueryParam("pgr", "purpose_group_id", purposeGroupId, ref fieldList, ref paramList, parameters, -1);
    //            AddQueryParam("prp", "purpose_id", purposeId, ref fieldList, ref paramList, parameters, -1);
    //            AddQueryParam("prpstr", "purpose_str", use, ref fieldList, ref paramList, parameters, 255);

    //            SqlConnection connectionSql = Utils.ConnectToDatabase();

    //            if (connectionSql != null)
    //            {
    //                using (SqlCommand cmd = connectionSql.CreateCommand())
    //                {
    //                    cmd.CommandType = System.Data.CommandType.Text;
    //                    cmd.CommandTimeout = 600;
    //                    cmd.CommandText = "INSERT INTO balans (" + fieldList.TrimStart(' ', ',') + ") VALUES (" + paramList.TrimStart(' ', ',') + ")";

    //                    foreach (KeyValuePair<string, object> pair in parameters)
    //                    {
    //                        cmd.Parameters.Add(new SqlParameter(pair.Key, pair.Value));
    //                    }

    //                    cmd.ExecuteNonQuery();
    //                }

    //                connectionSql.Close();
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        newBalansId = -1;
    //        errorMessage = ex.Message;

    //        // Roll back the transaction
    //        if (transaction != null)
    //        {
    //            transaction.Rollback();
    //        }
    //    }

    //    return newBalansId;
    //}

    private static void AddQueryParam(string paramName, string fieldName, object value,
        ref string fieldList, ref string paramList, Dictionary<string, object> parameters, int textLimit)
    {
        bool valueIsValid = (value != null);

        if (value is int)
        {
            valueIsValid = ((int)value >= 0);
        }
        else if (value is string)
        {
            string s = (string)value;

            s = s.Trim();

            if (textLimit > 0 && s.Length > textLimit)
            {
                s = s.Substring(0, textLimit);
            }

            value = s;
            valueIsValid = s.Length > 0;
        }

        if (valueIsValid)
        {
            fieldList += ", " + fieldName;
            paramList += ", @" + paramName;
            parameters.Add(paramName, value);
        }
    }

    //private static int GenerateNewFirebirdId(FbConnection connection, string generatorName, FbTransaction transaction)
    //{
    //    int objectId = -1;

    //    string query = "SELECT GEN_ID(" + generatorName + ", 1) FROM RDB$DATABASE";

    //    using (FbCommand command = new FbCommand(query, connection))
    //    {
    //        command.Transaction = transaction;

    //        using (FbDataReader reader = command.ExecuteReader())
    //        {
    //            if (reader.Read())
    //            {
    //                if (!reader.IsDBNull(0))
    //                {
    //                    objectId = reader.GetInt32(0);
    //                }
    //            }

    //            reader.Close();
    //        }
    //    }

    //    return objectId;
    //}

    /// <summary>
    /// Generates a new ID for any Firebird table which has a GENERATOR
    /// </summary>
    /// <param name="connection">Connection to a Firebird database</param>
    /// <param name="generatorName">Name of the Generator to use</param>
    /// <param name="transaction">Transaction ('null' is not allowed)</param>
    /// <returns>The generated ID</returns>
    //private static int GenerateNewId(FbConnection connection, string generatorName, FbTransaction transaction)
    //{
    //    int objectId = -1;

    //    string query = "SELECT GEN_ID(" + generatorName + ", 1) FROM RDB$DATABASE";

    //    using (FbCommand command = new FbCommand(query, connection))
    //    {
    //        command.Transaction = transaction;

    //        using (FbDataReader reader = command.ExecuteReader())
    //        {
    //            if (reader.Read())
    //            {
    //                if (!reader.IsDBNull(0))
    //                {
    //                    objectId = reader.GetInt32(0);
    //                }
    //            }

    //            reader.Close();
    //        }
    //    }

    //    return objectId;
    //}

    public static List<object> GetUserAccountsOfOrganisation(int orgId)
    {
        var users = new List<object>();

        SqlConnection connection = Utils.ConnectToDatabase();

        if (connection != null)
        {
            string query = @"SELECT a.UserID, u.UserName, m.Email from reports1nf_accounts a
                            LEFT JOIN aspnet_Users u
                            ON a.UserID = u.UserID
                            LEFT JOIN aspnet_Membership m
                            ON a.UserID = m.UserID
                            WHERE organization_id = @organization_id";

            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.Add(new SqlParameter("organization_id", orgId));
                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        users.Add(new { UserID = reader["UserID"], UserName = reader["UserName"], Email = reader["Email"] });
                    }
                    reader.Close();
                }
            }
            connection.Close();
        }

        return users;
    }

    public static int CreateNew1NFBuilding(SqlConnection connectionSql, SqlTransaction transactionSql,
        int streetId,
        int districtId,
        int objKindId,
        int objTypeId,
        string number1,
        string number2,
        string number3,
        string addrMisc,
        out string errorMessage)
    {
        bool duplicateInSqlServer = true;

        ObjectFinder.Instance.BuildObjectCacheFromSqlServer(connectionSql);

        System.Web.Security.MembershipUser user = System.Web.Security.Membership.GetUser();
        string username = (user == null ? "System" : user.UserName);

        errorMessage = "";
        int newBuildingId = -1;

        number1 = number1.Trim().ToUpper();
        number2 = number2.Trim().ToUpper();
        number3 = number3.Trim().ToUpper();
        addrMisc = addrMisc.Trim().ToUpper();

        PreProcessBuildingNumbers(ref number1, ref number2, ref number3);

        // Make all changes to the Firebird database in a transaction
        //FbTransaction transaction = null;

        try
        {
            // Verify parameters
            if (number1.Length == 0)
            {
                throw new ArgumentException("Необхідно заповнити номер будинку.");
            }

            if (streetId < 0)
            {
                throw new ArgumentException("Необхідно вибрати вулицю.");
            }

            //transaction = connection.BeginTransaction();

            // Generate new building Id
            //newBuildingId = GenerateNewId(connection, "OBJECT_1NF_GEN", transaction);

            using (SqlCommand cmdBuildingId = new SqlCommand("select max(id) + 1 from buildings", connectionSql, transactionSql))
            {
                using (SqlDataReader r = cmdBuildingId.ExecuteReader())
                {
                    if (r.Read())
                        newBuildingId = r.GetInt32(0);
                    r.Close();
                }
            }


            Dictionary<string, object> parameters = new Dictionary<string, object>();

            if (newBuildingId < 0)
            {
                throw new InvalidOperationException("Помилка при створенні ідентифікатора нового будинку в базі.");
            }

            // Get the street name from 1NF
            string streetName = "";

            using (SqlCommand cmdStreetName = new SqlCommand("select name from dict_streets where id = @streetId", connectionSql, transactionSql))
            {
                cmdStreetName.Parameters.AddWithValue("streetId", streetId);
                using (SqlDataReader r = cmdStreetName.ExecuteReader())
                {
                    if (r.Read())
                        streetName = r.GetString(0);
                    r.Close();
                }
            }



            //using (FbCommand cmdTest = new FbCommand("SELECT FIRST 1 NAME FROM SUL WHERE KOD = @strid", connection))
            //{
            //    cmdTest.Transaction = transaction;
            //    cmdTest.Parameters.Add(new FbParameter("strid", streetId));

            //    using (FbDataReader r = cmdTest.ExecuteReader())
            //    {
            //        if (r.Read())
            //        {
            //            streetName = r.GetString(0);
            //        }

            //        r.Close();
            //    }
            //}

            streetName = streetName.Trim().ToUpper();

            // Prepare the INSERT statement
            //string fieldList = "OBJECT_KOD, OBJECT_KODSTAN, DT, STAN_YEAR, REALSTAN, ULNAME, FULL_ULNAME, DELETED, KORPUS";
            //string paramList = "@oid, 1, @dt, @syear, 1, @sname, @sname, 0, 0";

            //AddQueryParam("scod", "ULKOD", streetId, ref fieldList, ref paramList, parameters, -1);
            //AddQueryParam("num1", "NOMER1", number1, ref fieldList, ref paramList, parameters, 9);
            //AddQueryParam("num2", "NOMER2", number2, ref fieldList, ref paramList, parameters, 18);
            //AddQueryParam("num3", "NOMER3", number3, ref fieldList, ref paramList, parameters, 10);
            //AddQueryParam("amsc", "ADRDOP", addrMisc, ref fieldList, ref paramList, parameters, 100);
            //AddQueryParam("distr", "NEWDISTR", districtId, ref fieldList, ref paramList, parameters, -1);
            //AddQueryParam("knd", "VIDOBJ", objKindId, ref fieldList, ref paramList, parameters, -1);
            //AddQueryParam("typ", "TYPOBJ", objTypeId, ref fieldList, ref paramList, parameters, -1);
            //AddQueryParam("isp", "ISP", username, ref fieldList, ref paramList, parameters, 18);

            //using (FbCommand commandInsert = new FbCommand("INSERT INTO OBJECT_1NF (" + fieldList + ") VALUES (" + paramList + ")", connection))
            //{
            //    commandInsert.Parameters.Add(new FbParameter("oid", newBuildingId));
            //    commandInsert.Parameters.Add(new FbParameter("dt", DateTime.Now.Date));
            //    commandInsert.Parameters.Add(new FbParameter("syear", DateTime.Now.Year));
            //    commandInsert.Parameters.Add(new FbParameter("sname", streetName));

            //    foreach (KeyValuePair<string, object> pair in parameters)
            //    {
            //        commandInsert.Parameters.Add(new FbParameter(pair.Key, pair.Value));
            //    }

            //    commandInsert.Transaction = transaction;
            //    commandInsert.ExecuteNonQuery();
            //}

            // Commit the transaction
            //transaction.Commit();
            //transaction = null; // This will prevent an undesired Rollback() in the catch{} section

            // Duplicate the organization in SQL server, if requested
            if (duplicateInSqlServer)
            {
                bool addressIsSimple, similarAddressExists;
                List<int> existItems = ObjectFinder.Instance.FindObject(streetName, string.Empty, number1, number2, number3, addrMisc, districtId, null, out addressIsSimple, out similarAddressExists);
                if (existItems.Count > 0)
                    return existItems[0];

                parameters.Clear();

                string fieldList = "id, modify_date, condition_year, addr_street_name, street_full_name, is_deleted, addr_korpus_flag";
                string paramList = "@oid, @dt, @syear, @sname, @sname, 0, 0";

                AddQueryParam("scod", "addr_street_id", streetId, ref fieldList, ref paramList, parameters, -1);
                AddQueryParam("num1", "addr_nomer1", number1, ref fieldList, ref paramList, parameters, 9);
                AddQueryParam("num2", "addr_nomer2", number2, ref fieldList, ref paramList, parameters, 18);
                AddQueryParam("num3", "addr_nomer3", number3, ref fieldList, ref paramList, parameters, 10);
                AddQueryParam("amsc", "addr_misc", addrMisc, ref fieldList, ref paramList, parameters, 100);
                AddQueryParam("distr", "addr_distr_new_id", districtId, ref fieldList, ref paramList, parameters, -1);
                AddQueryParam("knd", "object_kind_id", objKindId, ref fieldList, ref paramList, parameters, -1);
                AddQueryParam("typ", "object_type_id", objTypeId, ref fieldList, ref paramList, parameters, -1);
                AddQueryParam("isp", "modified_by", username, ref fieldList, ref paramList, parameters, 128);

                if (connectionSql != null)
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO buildings (" + fieldList + ") VALUES (" + paramList + ")", connectionSql, transactionSql))
                    {
                        cmd.Parameters.Add(new SqlParameter("oid", newBuildingId));
                        cmd.Parameters.Add(new SqlParameter("dt", DateTime.Now.Date));
                        cmd.Parameters.Add(new SqlParameter("syear", DateTime.Now.Year));
                        cmd.Parameters.Add(new SqlParameter("sname", streetName));

                        foreach (KeyValuePair<string, object> pair in parameters)
                        {
                            cmd.Parameters.Add(new SqlParameter(pair.Key, pair.Value));
                        }

                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            newBuildingId = -1;
            errorMessage = ex.Message;

            // Roll back the transaction
            //if (transaction != null)
            //{
            //    transaction.Rollback();
            //}
        }

        return newBuildingId;
    }

    public static int CreateNew1NFBuilding(/*FbConnection connection,*/
        int streetId,
        int districtId,
        int objKindId,
        int objTypeId,
        string number1,
        string number2,
        string number3,
        string addrMisc,
        out string errorMessage)
    {
        using (SqlConnection connectionSql = Utils.ConnectToDatabase())
        {
            return CreateNew1NFBuilding(connectionSql, null, streetId, districtId, objKindId, objTypeId,
                number1, number2, number3, addrMisc, out errorMessage);
        }
    }

    private static void PreProcessBuildingNumbers(ref string number1, ref string number2, ref string number3)
    {
        if (number1.Length > 9 || number2.Length > 18)
        {
            number1 = number1.Trim().Substring(0, 9);
            number2 = number2.Trim().Substring(0, 18);
        }

        number1 = number1.Trim();
        number2 = number2.Trim();

        // Find any non-digit in the primary number
        for (int i = 0; i < number1.Length; i++)
        {
            if (!char.IsDigit(number1[i]))
            {
                string actualNomer1 = number1.Substring(0, i);
                string actualNomer2 = number1.Substring(i).Trim();

                string prevNomer2 = number2;

                if (prevNomer2.Length > 0)
                {
                    actualNomer2 += " " + prevNomer2;
                }

                if (actualNomer2.Length > 18)
                {
                    actualNomer2 = actualNomer2.Substring(0, 18);
                }

                number1 = actualNomer1;
                number2 = actualNomer2;
                break;
            }
        }
    }
}

