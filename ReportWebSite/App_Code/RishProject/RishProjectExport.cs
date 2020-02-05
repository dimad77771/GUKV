using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FirebirdSql.Data.FirebirdClient;
using System.Data.SqlClient;
using GUKV.DataMigration;

/// <summary>
/// Performs export of the finalized Rishennya project to the 'Rozporadjennia' database
/// </summary>
public static class RishProjectExport
{
    #region Working with 1NF database

    public static int Update1NFOrganization(
        int orgId,
        string fullName,
        string shortName,
        string zkpo,
        int industryId,
        int occupationId,
        int formVlasnId,
        int statusId,
        int finFormId,
        int orgFormId,
        int vedomstvoId,
        string directorName,
        string directorPhone,
        string directorEmail,
        string buhgalterName,
        string buhgalterPhone,
        string fax,
        string kved,
        int addrDistrictId,
        string addrStreetName,
        string addrNomer,
        string addrKorpus,
        string addrZipCode,
        out string errorMessage)
    {
        bool duplicateInSqlServer = true;

        System.Web.Security.MembershipUser user = System.Web.Security.Membership.GetUser();
        string username = (user == null ? "System" : user.UserName);

        errorMessage = "";

        fullName = fullName.Trim().ToUpper();
        shortName = shortName.Trim().ToUpper();
        zkpo = zkpo.Trim();

        // Make all changes to the Firebird database in a transaction
        //FbTransaction transaction = null;

        try
        {
            // Verify parameters
            if (fullName.Length == 0)
            {
                throw new ArgumentException("Необхідно заповнити повну назву організації.");
            }

            if (zkpo.Length == 0)
            {
                throw new ArgumentException("Необхідно заповнити Код ЄДРПОУ.");
            }

            //transaction = connection.BeginTransaction();


            //using (FbCommand commandUpdate = new FbCommand("UPDATE SORG_1NF SET FULL_NAME_OBJ = @fname, " +
            //    (industryId == -1 ? "" : "KOD_GALUZ = @industry_id, ") +
            //    (occupationId == -1 ? "" : "KOD_VID_DIAL = @occupation_id, ") +
            //    (finFormId == -1 ? "" : "KOD_FORM_GOSP = @form_gosp_id, ") +
            //    (vedomstvoId == -1 ? "" : "KOD_VIDOM_NAL = @vedomstvo_id, ") +
            //    (orgFormId == -1 ? "" : "KOD_ORG_FORM = @orgf, ") +
            //    "NAME_UL = @strt, " + // street
            //    "NOMER_DOMA = @nom, " + // nom doma
            //    "NOMER_KORPUS = @korp, " + // korpus
            //    "POST_INDEX = @zcod, " +
            //    "TEL_BUH = @buhtel, " + 
            //    "FIO_BUH = @buhfio, " +
            //    "FIO_BOSS = @dirfio, " +
            //    "TEL_BOSS = @dirtel, " +
            //    "TELEFAX = @fax, " +
            //    "KOD_RAYON2 = @distr, " +
            //    "KOD_FORM_VLASN = @fvl, " +
            //    "KOD_STATUS = @sta, " +
            //    "USER_KOREG = @isp, DATE_KOREG = @dt, SHORT_NAME_OBJ = @sname, " +
            //    "KOD_STAN = 1, LAST_SOST = 1, DELETED = 0 " +
            //    " WHERE KOD_OBJ = @orgd", connection))
            //{
            //    commandUpdate.Parameters.Add(new FbParameter("orgd", orgId));
            //    commandUpdate.Parameters.Add(new FbParameter("isp", username.Left(18)));
            //    commandUpdate.Parameters.Add(new FbParameter("dt", DateTime.Now.Date));
            //    commandUpdate.Parameters.Add(new FbParameter("zkpo", zkpo.Length > 14 ? zkpo.Substring(0, 14) : zkpo));
            //    commandUpdate.Parameters.Add(new FbParameter("fname", fullName.Length > 252 ? fullName.Substring(0, 252) : fullName));
            //    commandUpdate.Parameters.Add(new FbParameter("strt", addrStreetName.Length > 100 ? addrStreetName.Substring(0, 100) : addrStreetName));
            //    commandUpdate.Parameters.Add(new FbParameter("nom", addrNomer.Length > 30 ? addrNomer.Substring(0, 30) : addrNomer));
            //    commandUpdate.Parameters.Add(new FbParameter("korp", addrKorpus.Length > 20 ? addrKorpus.Substring(0, 20) : addrKorpus));
            //    commandUpdate.Parameters.Add(new FbParameter("zcod", addrZipCode.Length > 18 ? addrZipCode.Substring(0, 18) : addrZipCode));
            //    commandUpdate.Parameters.Add(new FbParameter("buhtel", buhgalterPhone.Length > 23 ? buhgalterPhone.Substring(0, 23) : buhgalterPhone));
            //    commandUpdate.Parameters.Add(new FbParameter("buhfio", buhgalterName.Length > 70 ? buhgalterName.Substring(0, 70) : buhgalterName));
            //    commandUpdate.Parameters.Add(new FbParameter("dirfio", directorName.Length > 70 ? directorName.Substring(0, 70) : directorName));
            //    commandUpdate.Parameters.Add(new FbParameter("dirtel", directorPhone.Length > 23 ? directorPhone.Substring(0, 23) : directorPhone));
            //    commandUpdate.Parameters.Add(new FbParameter("fax", fax.Length > 23 ? fax.Substring(0, 23) : fax));

            //    commandUpdate.Parameters.Add(new FbParameter("distr", addrDistrictId));

            //    commandUpdate.Parameters.Add(new FbParameter("fvl", formVlasnId));
            //    commandUpdate.Parameters.Add(new FbParameter("sta", statusId));
            //    commandUpdate.Parameters.Add(new FbParameter("orgf", orgFormId));

            //    if (industryId != -1)
            //        commandUpdate.Parameters.Add(new FbParameter("industry_id", industryId));

            //    if (occupationId != -1)
            //        commandUpdate.Parameters.Add(new FbParameter("occupation_id", occupationId));
            //    if (finFormId != -1)
            //        commandUpdate.Parameters.Add(new FbParameter("form_gosp_id", finFormId));
            //    if (vedomstvoId != -1)
            //        commandUpdate.Parameters.Add(new FbParameter("vedomstvo_id", vedomstvoId));

            //    commandUpdate.Parameters.Add(new FbParameter("sname", shortName.Length > 100 ? shortName.Substring(0, 100) : shortName));


            //    commandUpdate.Transaction = transaction;
            //    commandUpdate.ExecuteNonQuery();
            //}

            // Commit the transaction
            //transaction.Commit();
            //transaction = null; // This will prevent an undesired Rollback() in the catch{} section

            // Duplicate the organization in SQL server, if requested
            if (duplicateInSqlServer)
            {

                SqlConnection connectionSql = Utils.ConnectToDatabase();

                if (connectionSql != null)
                {
                    ArchiverSql.CreateOrganizationArchiveRecord(connectionSql, orgId, username, null);

                    using (SqlCommand cmd = new SqlCommand("UPDATE organizations SET full_name = @fname, short_name = @sname, zkpo_code = @zkpo, modified_by = @isp, modify_date = @dt, is_deleted = 0, " +
                        (industryId == -1 ? "" : "industry_id = @industry_id, ") +
                        (occupationId == -1 ? "" : "occupation_id = @occupation_id, ") +
                        (finFormId == -1 ? "" : "form_gosp_id = @form_gosp_id, ") +
                        (vedomstvoId == -1 ? "" : "vedomstvo_id = @vedomstvo_id,") +
                        (orgFormId == -1 ? "" : "form_id = @form_id, ") +
                        "form_ownership_id = @form_ownership_id, status_id = @status_id, " +
                        "director_fio = @director_fio, director_phone = @director_phone, director_email = @director_email, buhgalter_fio = @buhgalter_fio, buhgalter_phone = @buhgalter_phone, fax = @fax, " +
                        "addr_distr_new_id = @addr_distr_new_id, addr_street_name = @addr_street_name, addr_nomer = @addr_nomer, addr_korpus = @addr_korpus" + 
                        " WHERE id = @orgd", connectionSql))
                    {
                        cmd.Parameters.Add(new SqlParameter("orgd", orgId));
                        cmd.Parameters.Add(new SqlParameter("isp", username));
                        cmd.Parameters.Add(new SqlParameter("dt", DateTime.Now));
                        cmd.Parameters.Add(new SqlParameter("zkpo", zkpo.Length > 16 ? zkpo.Substring(0, 16) : zkpo));
                        cmd.Parameters.Add(new SqlParameter("fname", fullName.Length > 255 ? fullName.Substring(0, 255) : fullName));
                        cmd.Parameters.Add(new SqlParameter("sname", shortName.Length > 255 ? shortName.Substring(0, 255) : shortName));
                        cmd.Parameters.Add(new SqlParameter("director_email", directorEmail.Length > 100 ? directorEmail.Substring(0, 100) : directorEmail));

                        if (industryId != -1)
                            cmd.Parameters.Add(new SqlParameter("industry_id", industryId));
                        if (occupationId != -1)
                            cmd.Parameters.Add(new SqlParameter("occupation_id", occupationId));
                        if (finFormId != -1)
                            cmd.Parameters.Add(new SqlParameter("form_gosp_id", finFormId));
                        if (vedomstvoId!= -1)
                            cmd.Parameters.Add(new SqlParameter("vedomstvo_id", vedomstvoId));
                        if (orgFormId != -1)
                            cmd.Parameters.Add(new SqlParameter("form_id", orgFormId));

                        cmd.Parameters.Add(new SqlParameter("form_ownership_id", formVlasnId));
                        cmd.Parameters.Add(new SqlParameter("status_id", statusId));
                        

                        cmd.Parameters.Add(new SqlParameter("director_fio", directorName.Length > 70 ? directorName.Substring(0, 70) : directorName));
                        cmd.Parameters.Add(new SqlParameter("director_phone", directorPhone.Length > 23 ? directorPhone.Substring(0, 23) : directorPhone));
                        // email
                        cmd.Parameters.Add(new SqlParameter("buhgalter_fio", buhgalterName.Length > 70 ? buhgalterName.Substring(0, 70) : buhgalterName));
                        cmd.Parameters.Add(new SqlParameter("buhgalter_phone", buhgalterPhone.Length > 23 ? buhgalterPhone.Substring(0, 23) : buhgalterPhone));
                        cmd.Parameters.Add(new SqlParameter("fax", fax.Length > 23 ? fax.Substring(0, 23) : fax));

                        cmd.Parameters.Add(new SqlParameter("addr_distr_new_id", addrDistrictId));
                        cmd.Parameters.Add(new SqlParameter("addr_street_name", addrStreetName.Length > 100 ? addrStreetName.Substring(0, 100) : addrStreetName));
                        cmd.Parameters.Add(new SqlParameter("addr_nomer", addrNomer.Length > 30 ? addrNomer.Substring(0, 30) : addrNomer));
                        cmd.Parameters.Add(new SqlParameter("addr_korpus", addrKorpus.Length > 20 ? addrKorpus.Substring(0, 20) : addrKorpus));
                        cmd.Parameters.Add(new SqlParameter("addr_zip_code", addrZipCode.Length > 18 ? addrZipCode.Substring(0, 18) : addrZipCode));

                        cmd.ExecuteNonQuery();
                    }

                    connectionSql.Close();
                }
            }
        }
        catch (Exception ex)
        {            
            errorMessage = ex.Message;

            // Roll back the transaction
            //if (transaction != null)
            //{
            //    transaction.Rollback();
            //}

            return -1;
        }

        return orgId;
    }

    public static int CreateNew1NFOrganization(
        string fullName,
        string shortName,
        string zkpo,
        int industryId,
        int occupationId,
        int formVlasnId,
        int statusId,
        int finFormId,
        int orgFormId,
        int vedomstvoId,
        string directorName,
        string directorPhone,
        string directorEmail,
        string buhgalterName,
        string buhgalterPhone,
        string fax,
        string kved,
        int addrDistrictId,
        string addrStreetName,
        string addrNomer,
        string addrKorpus,
        string addrZipCode,
        out string errorMessage)
    {
        bool duplicateInSqlServer = true;

        System.Web.Security.MembershipUser user = System.Web.Security.Membership.GetUser();
        string username = (user == null ? "System" : user.UserName);

        errorMessage = "";
        int newOrgId = -1;

        fullName = fullName.Trim().ToUpper();
        shortName = shortName.Trim().ToUpper();
        zkpo = zkpo.Trim();

        // Make all changes to the Firebird database in a transaction
        //FbTransaction transaction = null;

        try
        {
            // Verify parameters
            if (fullName.Length == 0)
            {
                throw new ArgumentException("Необхідно заповнити повну назву організації.");
            }

            if (zkpo.Length == 0)
            {
                throw new ArgumentException("Необхідно заповнити Код ЄДРПОУ.");
            }

            //transaction = connection.BeginTransaction();

            // Check if ZKPO code already exists
            //using (FbCommand cmdTest = new FbCommand("SELECT FIRST 1 KOD_OBJ FROM SORG_1NF WHERE KOD_ZKPO = @zkpo", connection))
            //{
            //    cmdTest.Transaction = transaction;
            //    cmdTest.Parameters.Add(new FbParameter("zkpo", zkpo.Length > 14 ? zkpo.Substring(0, 14) : zkpo));

            //    using (FbDataReader r = cmdTest.ExecuteReader())
            //    {
            //        if (r.Read())
            //        {
            //            throw new InvalidOperationException("В базі 1НФ вже існує організація з таким кодом ЄДРПОУ.");
            //        }

            //        r.Close();
            //    }
            //}

            // Generate new organization Id
            //newOrgId = Get1NFNewOrganizationId(connection, transaction);

            SqlConnection connectionSql = Utils.ConnectToDatabase();

            using (SqlCommand cmdNewOrgId = new SqlCommand("select MAX(id) + 1 from organizations", connectionSql))
            {
                using (SqlDataReader r = cmdNewOrgId.ExecuteReader())
                {
                    if (r.Read())
                        newOrgId = (int)r.GetValue(0);
                    r.Close();
                }
            }

            Dictionary<string, object> parameters = new Dictionary<string, object>();

            if (newOrgId < 0)
            {
                throw new InvalidOperationException("Помилка при створенні ідентифікатора нової організації в базі.");
            }

            // Prepare the INSERT statement
            //string fieldList = "KOD_OBJ, KOD_STAN, LAST_SOST, ISP, DT, USER_KOREG, DATE_KOREG, DELETED, KOD_ZKPO, FULL_NAME_OBJ, SHORT_NAME_OBJ";
            //string paramList = "@orgd, 1, 1, @isp, @dt, @isp, @dt, 0, @zkpo, @fname, @sname";

            //AddQueryParam("gal", "KOD_GALUZ", industryId, ref fieldList, ref paramList, parameters, -1);
            //AddQueryParam("viddial", "KOD_VID_DIAL", occupationId, ref fieldList, ref paramList, parameters, -1);
            //AddQueryParam("fvl", "KOD_FORM_VLASN", formVlasnId, ref fieldList, ref paramList, parameters, -1);
            //AddQueryParam("sta", "KOD_STATUS", statusId, ref fieldList, ref paramList, parameters, -1);
            //AddQueryParam("gosp", "KOD_FORM_GOSP", finFormId, ref fieldList, ref paramList, parameters, -1);
            //AddQueryParam("orgf", "KOD_ORG_FORM", orgFormId, ref fieldList, ref paramList, parameters, -1);
            //AddQueryParam("vedom", "KOD_VIDOM_NAL", vedomstvoId, ref fieldList, ref paramList, parameters, -1);
            //AddQueryParam("dirfio", "FIO_BOSS", directorName, ref fieldList, ref paramList, parameters, 70);
            //AddQueryParam("dirtel", "TEL_BOSS", directorPhone, ref fieldList, ref paramList, parameters, 23);
            //AddQueryParam("buhfio", "FIO_BUH", buhgalterName, ref fieldList, ref paramList, parameters, 70);
            //AddQueryParam("buhtel", "TEL_BUH", buhgalterPhone, ref fieldList, ref paramList, parameters, 23);
            //AddQueryParam("fax", "TELEFAX", fax, ref fieldList, ref paramList, parameters, 23);
            //AddQueryParam("kved", "KOD_KVED", kved, ref fieldList, ref paramList, parameters, 7);
            //AddQueryParam("distr", "KOD_RAYON2", addrDistrictId, ref fieldList, ref paramList, parameters, -1);
            //AddQueryParam("strt", "NAME_UL", addrStreetName, ref fieldList, ref paramList, parameters, 100);
            //AddQueryParam("nom", "NOMER_DOMA", addrNomer, ref fieldList, ref paramList, parameters, 30);
            //AddQueryParam("korp", "NOMER_KORPUS", addrKorpus, ref fieldList, ref paramList, parameters, 20);
            //AddQueryParam("zcod", "POST_INDEX", addrZipCode, ref fieldList, ref paramList, parameters, 18);

            //using (FbCommand commandInsert = new FbCommand("INSERT INTO SORG_1NF (" + fieldList + ") VALUES (" + paramList + ")", connection))
            //{
            //    commandInsert.Parameters.Add(new FbParameter("orgd", newOrgId));
            //    commandInsert.Parameters.Add(new FbParameter("isp", username.Length > 18 ? username.Substring(0, 18) : username));
            //    commandInsert.Parameters.Add(new FbParameter("dt", DateTime.Now.Date));
            //    commandInsert.Parameters.Add(new FbParameter("zkpo", zkpo.Length > 14 ? zkpo.Substring(0, 14) : zkpo));
            //    commandInsert.Parameters.Add(new FbParameter("fname", fullName.Length > 252 ? fullName.Substring(0, 252) : fullName));
            //    commandInsert.Parameters.Add(new FbParameter("sname", shortName.Length > 100 ? shortName.Substring(0, 100) : shortName));
                

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
                parameters.Clear();

                string fieldList = "id, modified_by, modify_date, is_deleted, zkpo_code, full_name, short_name, director_email";
                string paramList = "@orgd, @isp, @dt, 0, @zkpo, @fname, @sname, @director_email";

                AddQueryParam("gal", "industry_id", industryId, ref fieldList, ref paramList, parameters, -1);
                AddQueryParam("viddial", "occupation_id", occupationId, ref fieldList, ref paramList, parameters, -1);
                AddQueryParam("fvl", "form_ownership_id", formVlasnId, ref fieldList, ref paramList, parameters, -1);
                AddQueryParam("sta", "status_id", statusId, ref fieldList, ref paramList, parameters, -1);
                AddQueryParam("gosp", "form_gosp_id", finFormId, ref fieldList, ref paramList, parameters, -1);
                AddQueryParam("orgf", "form_id", orgFormId, ref fieldList, ref paramList, parameters, -1);
                AddQueryParam("vedom", "vedomstvo_id", vedomstvoId, ref fieldList, ref paramList, parameters, -1);
                AddQueryParam("dirfio", "director_fio", directorName, ref fieldList, ref paramList, parameters, 70);
                AddQueryParam("dirtel", "director_phone", directorPhone, ref fieldList, ref paramList, parameters, 23);
                AddQueryParam("buhfio", "buhgalter_fio", buhgalterName, ref fieldList, ref paramList, parameters, 70);
                AddQueryParam("buhtel", "buhgalter_phone", buhgalterPhone, ref fieldList, ref paramList, parameters, 23);
                AddQueryParam("fax", "fax", fax, ref fieldList, ref paramList, parameters, 23);
                AddQueryParam("kved", "kved_code", kved, ref fieldList, ref paramList, parameters, 7);
                AddQueryParam("distr", "addr_distr_new_id", addrDistrictId, ref fieldList, ref paramList, parameters, -1);
                AddQueryParam("strt", "addr_street_name", addrStreetName, ref fieldList, ref paramList, parameters, 100);
                AddQueryParam("nom", "addr_nomer", addrNomer, ref fieldList, ref paramList, parameters, 30);
                AddQueryParam("korp", "addr_korpus", addrKorpus, ref fieldList, ref paramList, parameters, 20);
                AddQueryParam("zcod", "addr_zip_code", addrZipCode, ref fieldList, ref paramList, parameters, 18);

                

                if (connectionSql != null)
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO organizations (" + fieldList + ") VALUES (" + paramList + ")", connectionSql))
                    {
                        cmd.Parameters.Add(new SqlParameter("orgd", newOrgId));
                        cmd.Parameters.Add(new SqlParameter("isp", username));
                        cmd.Parameters.Add(new SqlParameter("dt", DateTime.Now));
                        cmd.Parameters.Add(new SqlParameter("zkpo", zkpo.Length > 16 ? zkpo.Substring(0, 16) : zkpo));
                        cmd.Parameters.Add(new SqlParameter("fname", fullName.Length > 255 ? fullName.Substring(0, 255) : fullName));
                        cmd.Parameters.Add(new SqlParameter("sname", shortName.Length > 255 ? shortName.Substring(0, 255) : shortName));
                        cmd.Parameters.Add(new SqlParameter("director_email", directorEmail.Length > 100 ? directorEmail.Substring(0, 100) : directorEmail));

                        foreach (KeyValuePair<string, object> pair in parameters)
                        {
                            cmd.Parameters.Add(new SqlParameter(pair.Key, pair.Value));
                        }

                        cmd.ExecuteNonQuery();
                    }

                    connectionSql.Close();
                }
            }
        }
        catch (Exception ex)
        {
            newOrgId = -1;
            errorMessage = ex.Message;

            // Roll back the transaction
            //if (transaction != null)
            //{
            //    transaction.Rollback();
            //}
        }

        return newOrgId;
    }


	public static int CreateNewActive1NFOrganization(
			string fullName,
			string shortName,
			string zkpo,
			int industryId,
			int occupationId,
			int formVlasnId,
			int statusId,
			int finFormId,
			int orgFormId,
			int vedomstvoId,
			string directorName,
			string directorPhone,
			string directorEmail,
			string buhgalterName,
			string buhgalterPhone,
			string fax,
			string kved,
			int addrDistrictId,
			string addrStreetName,
			string addrNomer,
			string addrKorpus,
			string addrZipCode,
			out string errorMessage)
	{
		bool duplicateInSqlServer = true;

		System.Web.Security.MembershipUser user = System.Web.Security.Membership.GetUser();
		string username = (user == null ? "System" : user.UserName);

		errorMessage = "";
		int newOrgId = -1;

		fullName = fullName.Trim().ToUpper();
		shortName = shortName.Trim().ToUpper();
		zkpo = zkpo.Trim();

		// Make all changes to the Firebird database in a transaction
		//FbTransaction transaction = null;

		try
		{
			// Verify parameters
			if (fullName.Length == 0)
			{
				throw new ArgumentException("Необхідно заповнити повну назву організації.");
			}

			if (zkpo.Length == 0)
			{
				throw new ArgumentException("Необхідно заповнити ЄДРПОУ/ІНН");
			}


			//transaction = connection.BeginTransaction();

			// Check if ZKPO code already exists
			//using (FbCommand cmdTest = new FbCommand("SELECT FIRST 1 KOD_OBJ FROM SORG_1NF WHERE KOD_ZKPO = @zkpo", connection))
			//{
			//    cmdTest.Transaction = transaction;
			//    cmdTest.Parameters.Add(new FbParameter("zkpo", zkpo.Length > 14 ? zkpo.Substring(0, 14) : zkpo));

			//    using (FbDataReader r = cmdTest.ExecuteReader())
			//    {
			//        if (r.Read())
			//        {
			//            throw new InvalidOperationException("В базі 1НФ вже існує організація з таким кодом ЄДРПОУ.");
			//        }

			//        r.Close();
			//    }
			//}

			// Generate new organization Id
			//newOrgId = Get1NFNewOrganizationId(connection, transaction);

			SqlConnection connectionSql = Utils.ConnectToDatabase();

			//// Check if ZKPO code already exists
			//using (SqlCommand cmdTest = new SqlCommand("SELECT FIRST 1 KOD_OBJ FROM SORG_1NF WHERE KOD_ZKPO = @zkpo", connectionSql))
			//{
			//    //cmdTest.Transaction = transaction;
			//    cmdTest.Parameters.Add(new SqlParameter("zkpo", zkpo));

			//    using (var r = cmdTest.ExecuteReader())
			//    {
			//        if (r.Read())
			//        {
			//            throw new InvalidOperationException("В базі вже існує картка з таким ЄДРПОУ/ІНН");
			//        }

			//        r.Close();
			//    }
			//}


			using (SqlCommand cmdNewOrgId = new SqlCommand("select MAX(id) + 1 from organizations", connectionSql))
			{
				using (SqlDataReader r = cmdNewOrgId.ExecuteReader())
				{
					if (r.Read())
						newOrgId = (int)r.GetValue(0);
					r.Close();
				}
			}

			Dictionary<string, object> parameters = new Dictionary<string, object>();

			if (newOrgId < 0)
			{
				throw new InvalidOperationException("Помилка при створенні ідентифікатора нової організації в базі.");
			}

			// Prepare the INSERT statement
			//string fieldList = "KOD_OBJ, KOD_STAN, LAST_SOST, ISP, DT, USER_KOREG, DATE_KOREG, DELETED, KOD_ZKPO, FULL_NAME_OBJ, SHORT_NAME_OBJ";
			//string paramList = "@orgd, 1, 1, @isp, @dt, @isp, @dt, 0, @zkpo, @fname, @sname";

			//AddQueryParam("gal", "KOD_GALUZ", industryId, ref fieldList, ref paramList, parameters, -1);
			//AddQueryParam("viddial", "KOD_VID_DIAL", occupationId, ref fieldList, ref paramList, parameters, -1);
			//AddQueryParam("fvl", "KOD_FORM_VLASN", formVlasnId, ref fieldList, ref paramList, parameters, -1);
			//AddQueryParam("sta", "KOD_STATUS", statusId, ref fieldList, ref paramList, parameters, -1);
			//AddQueryParam("gosp", "KOD_FORM_GOSP", finFormId, ref fieldList, ref paramList, parameters, -1);
			//AddQueryParam("orgf", "KOD_ORG_FORM", orgFormId, ref fieldList, ref paramList, parameters, -1);
			//AddQueryParam("vedom", "KOD_VIDOM_NAL", vedomstvoId, ref fieldList, ref paramList, parameters, -1);
			//AddQueryParam("dirfio", "FIO_BOSS", directorName, ref fieldList, ref paramList, parameters, 70);
			//AddQueryParam("dirtel", "TEL_BOSS", directorPhone, ref fieldList, ref paramList, parameters, 23);
			//AddQueryParam("buhfio", "FIO_BUH", buhgalterName, ref fieldList, ref paramList, parameters, 70);
			//AddQueryParam("buhtel", "TEL_BUH", buhgalterPhone, ref fieldList, ref paramList, parameters, 23);
			//AddQueryParam("fax", "TELEFAX", fax, ref fieldList, ref paramList, parameters, 23);
			//AddQueryParam("kved", "KOD_KVED", kved, ref fieldList, ref paramList, parameters, 7);
			//AddQueryParam("distr", "KOD_RAYON2", addrDistrictId, ref fieldList, ref paramList, parameters, -1);
			//AddQueryParam("strt", "NAME_UL", addrStreetName, ref fieldList, ref paramList, parameters, 100);
			//AddQueryParam("nom", "NOMER_DOMA", addrNomer, ref fieldList, ref paramList, parameters, 30);
			//AddQueryParam("korp", "NOMER_KORPUS", addrKorpus, ref fieldList, ref paramList, parameters, 20);
			//AddQueryParam("zcod", "POST_INDEX", addrZipCode, ref fieldList, ref paramList, parameters, 18);

			//using (FbCommand commandInsert = new FbCommand("INSERT INTO SORG_1NF (" + fieldList + ") VALUES (" + paramList + ")", connection))
			//{
			//    commandInsert.Parameters.Add(new FbParameter("orgd", newOrgId));
			//    commandInsert.Parameters.Add(new FbParameter("isp", username.Length > 18 ? username.Substring(0, 18) : username));
			//    commandInsert.Parameters.Add(new FbParameter("dt", DateTime.Now.Date));
			//    commandInsert.Parameters.Add(new FbParameter("zkpo", zkpo.Length > 14 ? zkpo.Substring(0, 14) : zkpo));
			//    commandInsert.Parameters.Add(new FbParameter("fname", fullName.Length > 252 ? fullName.Substring(0, 252) : fullName));
			//    commandInsert.Parameters.Add(new FbParameter("sname", shortName.Length > 100 ? shortName.Substring(0, 100) : shortName));


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
				parameters.Clear();

				string fieldList = "id, modified_by, modify_date, is_deleted, zkpo_code, full_name, short_name, director_email, isactive";
				string paramList = "@orgd, @isp, @dt, 0, @zkpo, @fname, @sname, @director_email, 1";

				AddQueryParam("gal", "industry_id", industryId, ref fieldList, ref paramList, parameters, -1);
				AddQueryParam("viddial", "occupation_id", occupationId, ref fieldList, ref paramList, parameters, -1);
				AddQueryParam("fvl", "form_ownership_id", formVlasnId, ref fieldList, ref paramList, parameters, -1);
				AddQueryParam("sta", "status_id", statusId, ref fieldList, ref paramList, parameters, -1);
				AddQueryParam("gosp", "form_gosp_id", finFormId, ref fieldList, ref paramList, parameters, -1);
				AddQueryParam("orgf", "form_id", orgFormId, ref fieldList, ref paramList, parameters, -1);
				AddQueryParam("vedom", "vedomstvo_id", vedomstvoId, ref fieldList, ref paramList, parameters, -1);
				AddQueryParam("dirfio", "director_fio", directorName, ref fieldList, ref paramList, parameters, 70);
				AddQueryParam("dirtel", "director_phone", directorPhone, ref fieldList, ref paramList, parameters, 23);
				AddQueryParam("buhfio", "buhgalter_fio", buhgalterName, ref fieldList, ref paramList, parameters, 70);
				AddQueryParam("buhtel", "buhgalter_phone", buhgalterPhone, ref fieldList, ref paramList, parameters, 23);
				AddQueryParam("fax", "fax", fax, ref fieldList, ref paramList, parameters, 23);
				AddQueryParam("kved", "kved_code", kved, ref fieldList, ref paramList, parameters, 7);
				AddQueryParam("distr", "addr_distr_new_id", addrDistrictId, ref fieldList, ref paramList, parameters, -1);
				AddQueryParam("strt", "addr_street_name", addrStreetName, ref fieldList, ref paramList, parameters, 100);
				AddQueryParam("nom", "addr_nomer", addrNomer, ref fieldList, ref paramList, parameters, 30);
				AddQueryParam("korp", "addr_korpus", addrKorpus, ref fieldList, ref paramList, parameters, 20);
				AddQueryParam("zcod", "addr_zip_code", addrZipCode, ref fieldList, ref paramList, parameters, 18);



				if (connectionSql != null)
				{
					using (SqlCommand cmd = new SqlCommand("INSERT INTO organizations (" + fieldList + ") VALUES (" + paramList + ")", connectionSql))
					{
						cmd.Parameters.Add(new SqlParameter("orgd", newOrgId));
						cmd.Parameters.Add(new SqlParameter("isp", username));
						cmd.Parameters.Add(new SqlParameter("dt", DateTime.Now));
						cmd.Parameters.Add(new SqlParameter("zkpo", zkpo.Length > 16 ? zkpo.Substring(0, 16) : zkpo));
						cmd.Parameters.Add(new SqlParameter("fname", fullName.Length > 255 ? fullName.Substring(0, 255) : fullName));
						cmd.Parameters.Add(new SqlParameter("sname", shortName.Length > 255 ? shortName.Substring(0, 255) : shortName));
						cmd.Parameters.Add(new SqlParameter("director_email", directorEmail.Length > 100 ? directorEmail.Substring(0, 100) : directorEmail));

						foreach (KeyValuePair<string, object> pair in parameters)
						{
							cmd.Parameters.Add(new SqlParameter(pair.Key, pair.Value));
						}

						cmd.ExecuteNonQuery();
					}

					connectionSql.Close();
				}
			}
		}
		catch (Exception ex)
		{
			newOrgId = -1;
			errorMessage = ex.Message;

			// Roll back the transaction
			//if (transaction != null)
			//{
			//    transaction.Rollback();
			//}
		}

		return newOrgId;
	}

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

    public static int CreateNew1NFBuilding(
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

        SqlConnection connectionSql = Utils.ConnectToDatabase();

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
        FbTransaction transaction = null;

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

            using (SqlCommand newIdCommand = new SqlCommand("SELECT MAX(ID) + 1 FROM buildings", connectionSql))
            {
                using (SqlDataReader r = newIdCommand.ExecuteReader())
                {
                    if (r.Read())
                    {
                        newBuildingId = r.GetInt32(0);
                    }
                }
            }


            Dictionary<string, object> parameters = new Dictionary<string, object>();

            if (newBuildingId < 0)
            {
                throw new InvalidOperationException("Помилка при створенні ідентифікатора нового будинку в базі 1НФ.");
            }

            // Get the street name from 1NF
            string streetName = "";

            using (SqlCommand cmdSteetName = new SqlCommand("SELECT NAME FROM dict_streets WHERE ID = @strid", connectionSql))
            {
                using (SqlDataReader r = cmdSteetName.ExecuteReader())
                {
                    if (r.Read())
                    {
                        streetName = r.GetString(0);
                    }
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
            //string fieldList = "OBJECT_KOD, OBJECT_KODSTAN, ISP, DT, STAN_YEAR, REALSTAN, ULNAME, FULL_ULNAME, DELETED, KORPUS";
            //string paramList = "@oid, 1, @isp, @dt, @syear, 1, @sname, @sname, 0, 0";

            //AddQueryParam("scod", "ULKOD", streetId, ref fieldList, ref paramList, parameters, -1);
            //AddQueryParam("num1", "NOMER1", number1, ref fieldList, ref paramList, parameters, 9);
            //AddQueryParam("num2", "NOMER2", number2, ref fieldList, ref paramList, parameters, 18);
            //AddQueryParam("num3", "NOMER3", number3, ref fieldList, ref paramList, parameters, 10);
            //AddQueryParam("amsc", "ADRDOP", addrMisc, ref fieldList, ref paramList, parameters, 100);
            //AddQueryParam("distr", "NEWDISTR", districtId, ref fieldList, ref paramList, parameters, -1);
            //AddQueryParam("knd", "VIDOBJ", objKindId, ref fieldList, ref paramList, parameters, -1);
            //AddQueryParam("typ", "TYPOBJ", objTypeId, ref fieldList, ref paramList, parameters, -1);

            //using (FbCommand commandInsert = new FbCommand("INSERT INTO OBJECT_1NF (" + fieldList + ") VALUES (" + paramList + ")", connection))
            //{
            //    commandInsert.Parameters.Add(new FbParameter("oid", newBuildingId));
            //    commandInsert.Parameters.Add(new FbParameter("isp", username.Left(18)));
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
                parameters.Clear();

                string fieldList = "id, modified_by, modify_date, condition_year, addr_street_name, street_full_name, is_deleted, addr_korpus_flag";
                string paramList = "@oid, @isp, @dt, @syear, @sname, @sname, 0, 0";

                AddQueryParam("scod", "addr_street_id", streetId, ref fieldList, ref paramList, parameters, -1);
                AddQueryParam("num1", "addr_nomer1", number1, ref fieldList, ref paramList, parameters, 9);
                AddQueryParam("num2", "addr_nomer2", number2, ref fieldList, ref paramList, parameters, 18);
                AddQueryParam("num3", "addr_nomer3", number3, ref fieldList, ref paramList, parameters, 10);
                AddQueryParam("amsc", "addr_misc", addrMisc, ref fieldList, ref paramList, parameters, 100);
                AddQueryParam("distr", "addr_distr_new_id", districtId, ref fieldList, ref paramList, parameters, -1);
                AddQueryParam("knd", "object_kind_id", objKindId, ref fieldList, ref paramList, parameters, -1);
                AddQueryParam("typ", "object_type_id", objTypeId, ref fieldList, ref paramList, parameters, -1);

                if (connectionSql != null)
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO buildings (" + fieldList + ") VALUES (" + paramList + ")", connectionSql))
                    {
                        cmd.Parameters.Add(new SqlParameter("oid", newBuildingId));
                        cmd.Parameters.Add(new SqlParameter("isp", username));
                        cmd.Parameters.Add(new SqlParameter("dt", DateTime.Now.Date));
                        cmd.Parameters.Add(new SqlParameter("syear", DateTime.Now.Year));
                        cmd.Parameters.Add(new SqlParameter("sname", streetName));

                        foreach (KeyValuePair<string, object> pair in parameters)
                        {
                            cmd.Parameters.Add(new SqlParameter(pair.Key, pair.Value));
                        }

                        cmd.ExecuteNonQuery();
                    }

                    connectionSql.Close();
                }
            }
        }
        catch (Exception ex)
        {
            newBuildingId = -1;
            errorMessage = ex.Message;

            // Roll back the transaction
            if (transaction != null)
            {
                transaction.Rollback();
            }
        }

        return newBuildingId;
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

    public static int CreateNew1NFObject(int buildingId, int orgId, string square, 
        string cost, int ownershipId, int objKindId, int objTypeId, int purposeGroupId, int purposeId, 
        string use, out string errorMessage)
    {
        bool duplicateInSqlServer = true;

        SqlConnection connectionSql = Utils.ConnectToDatabase();

        //if (connection == null)
            //throw new ArgumentNullException("connection");

        errorMessage = string.Empty;
        int newBalansId = -1;

        System.Web.Security.MembershipUser user = System.Web.Security.Membership.GetUser();
        string username = (user == null ? "System" : user.UserName);

        // Make all changes to the Firebird database in a transaction
        //FbTransaction transaction = null;

        try
        {
            if (buildingId <= 0)
                throw new ArgumentException("Адреса не вказана");
            if (orgId <= 0)
                throw new ArgumentException("Організація не вказана");

            //transaction = connection.BeginTransaction();

            // Convert all numbers provided as string to actual numbers
            decimal objectSquare = Utils.ConvertStrToDecimal(square);
            decimal objectCost = Utils.ConvertStrToDecimal(cost);

            // Generate new balans Id
            //newBalansId = GenerateNewId(connection, "GEN_BALANS_1NF", transaction);


            using (SqlCommand cmdnewBalansId = new SqlCommand("select max(id) + 1 from balans", connectionSql))
            {
                using (SqlDataReader r = cmdnewBalansId.ExecuteReader())
                {
                    if (r.Read())
                        newBalansId = r.GetInt32(0);
                    r.Close();
                }
            }

            if (newBalansId < 0)
            {
                throw new InvalidOperationException("Помилка при створенні ідентифікатора нового об'єкту на балансі в базі 1НФ.");
            }

            // Get address properties from the 1NF database
            string objUlname, objNomer1, objNomer2, objNomer3, objKodbti;
            int objKodul;

            using (SqlCommand command = new SqlCommand("SELECT addr_street_name, addr_nomer1, addr_nomer2, addr_nomer3, bti_code, addr_street_id WHERE id = @buildingId", connectionSql))
            {
                command.Parameters.AddWithValue("buildingId", buildingId);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                        throw new InvalidOperationException("Вказаного будинку не знайдено в базі.");

                    objUlname = (reader["addr_street_name"] as string) ?? string.Empty;
                    objNomer1 = (reader["addr_nomer1"] as string) ?? string.Empty;
                    objNomer2 = (reader["addr_nomer2"] as string) ?? string.Empty;
                    objNomer3 = (reader["addr_nomer3"] as string) ?? string.Empty;
                    objKodbti = (reader["bti_code"] as string) ?? string.Empty;
                    objKodul = (reader["addr_street_id"] is int ? (int)reader["addr_street_id"] : -1);

                    reader.Close();
                }
            }

            //using (FbCommand command = connection.CreateCommand())
            //{
            //    command.Transaction = transaction;
            //    command.CommandType = System.Data.CommandType.Text;
            //    command.CommandTimeout = 600;
            //    command.CommandText = "SELECT ULNAME, NOMER1, NOMER2, NOMER3, KODBTI, ULKOD FROM OBJECT_1NF WHERE OBJECT_KOD = @buildingId AND OBJECT_KODSTAN = 1";

            //    command.Parameters.Add(new FbParameter("buildingId", buildingId));

            //    using (System.Data.IDataReader reader = command.ExecuteReader())
            //    {
            //        if (!reader.Read())
            //            throw new InvalidOperationException("Вказаного будинку не знайдено в базі 1НФ.");

            //        objUlname = (reader["ULNAME"] as string) ?? string.Empty;
            //        objNomer1 = (reader["NOMER1"] as string) ?? string.Empty;
            //        objNomer2 = (reader["NOMER2"] as string) ?? string.Empty;
            //        objNomer3 = (reader["NOMER3"] as string) ?? string.Empty;
            //        objKodbti = (reader["KODBTI"] as string) ?? string.Empty;
            //        objKodul = (reader["ULKOD"] is int ? (int)reader["ULKOD"] : -1);

            //        reader.Close();
            //    }
            //}

            // Make sure that all strings are 'upper case'
            use = use.Trim().ToUpper();
            objUlname = objUlname.Trim().ToUpper();
            objNomer1 = objNomer1.Trim().ToUpper();
            objNomer2 = objNomer2.Trim().ToUpper();
            objNomer3 = objNomer3.Trim().ToUpper();
            objKodbti = objKodbti.Trim().ToUpper();

            // Generate the INSERT statement
            string fieldList = string.Empty;
            string paramList = string.Empty;
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            //AddQueryParam("bid", "ID", newBalansId, ref fieldList, ref paramList, parameters, -1);
            //AddQueryParam("stn", "STAN", DateTime.Now, ref fieldList, ref paramList, parameters, -1);
            //AddQueryParam("objid", "OBJECT", buildingId, ref fieldList, ref paramList, parameters, -1);
            //AddQueryParam("objstn", "OBJECT_STAN", 1, ref fieldList, ref paramList, parameters, -1);
            //AddQueryParam("orgid", "ORG", orgId, ref fieldList, ref paramList, parameters, -1);
            //AddQueryParam("orgstn", "ORG_STAN", 1, ref fieldList, ref paramList, parameters, -1);
            //AddQueryParam("isp", "ISP", username, ref fieldList, ref paramList, parameters, 18);
            //AddQueryParam("dt", "DT", DateTime.Now.Date, ref fieldList, ref paramList, parameters, -1);
            //AddQueryParam("edt", "EDT", DateTime.Now.Date, ref fieldList, ref paramList, parameters, -1);
            //AddQueryParam("lyear", "LYEAR", DateTime.Now.Year, ref fieldList, ref paramList, parameters, -1);
            //AddQueryParam("pr1nf", "PRIZNAK1NF", 0, ref fieldList, ref paramList, parameters, -1); // 0 ~ NOT from 1NF
            //AddQueryParam("upds", "UPD_SOURCE", 10, ref fieldList, ref paramList, parameters, -1); // BAGRIY V.M.
            //AddQueryParam("delt", "DELETED", 0, ref fieldList, ref paramList, parameters, -1);

            //AddQueryParam("addrstr", "OBJ_ULNAME", objUlname, ref fieldList, ref paramList, parameters, 100);
            //AddQueryParam("addrnom1", "OBJ_NOMER1", objNomer1, ref fieldList, ref paramList, parameters, 9);
            //AddQueryParam("addrnom2", "OBJ_NOMER2", objNomer2, ref fieldList, ref paramList, parameters, 18);
            //AddQueryParam("addrnom3", "OBJ_NOMER3", objNomer3, ref fieldList, ref paramList, parameters, 10);
            //AddQueryParam("addrbti", "OBJ_KODBTI", objKodbti, ref fieldList, ref paramList, parameters, 18);
            //AddQueryParam("addrsid", "OBJ_KODUL", objKodul, ref fieldList, ref paramList, parameters, -1);

            //AddQueryParam("sqrtot", "SQR_ZAG", objectSquare, ref fieldList, ref paramList, parameters, -1);
            //AddQueryParam("balv", "BALANS_VARTIST", objectCost, ref fieldList, ref paramList, parameters, -1);
            //AddQueryParam("fvl", "FORM_VLASN", ownershipId, ref fieldList, ref paramList, parameters, -1);
            //AddQueryParam("knd", "KINDOBJ", objKindId, ref fieldList, ref paramList, parameters, -1);
            //AddQueryParam("typ", "TYPEOBJ", objTypeId, ref fieldList, ref paramList, parameters, -1);
            //AddQueryParam("pgr", "GRPURP", purposeGroupId, ref fieldList, ref paramList, parameters, -1);
            //AddQueryParam("prp", "PURPOSE", purposeId, ref fieldList, ref paramList, parameters, -1);
            //AddQueryParam("prpstr", "PURP_STR", use, ref fieldList, ref paramList, parameters, 255);

            //using (FbCommand command = connection.CreateCommand())
            //{
            //    command.Transaction = transaction;
            //    command.CommandType = System.Data.CommandType.Text;
            //    command.CommandTimeout = 600;
            //    command.CommandText = "INSERT INTO BALANS_1NF (" + fieldList.TrimStart(' ', ',')
            //        + ") VALUES (" + paramList.TrimStart(' ', ',') + ")";

            //    foreach (KeyValuePair<string, object> pair in parameters)
            //    {
            //        command.Parameters.Add(new FbParameter(pair.Key, pair.Value));
            //    }

            //    command.ExecuteNonQuery();
            //}

            // Commit the transaction
            //transaction.Commit();
            //transaction = null; // This will prevent an undesired Rollback() in the catch{} section

            // Duplicate the balans object in SQL server, if requested
            if (duplicateInSqlServer)
            {
                parameters.Clear();
                fieldList = "";
                paramList = "";

                AddQueryParam("bid", "id", newBalansId, ref fieldList, ref paramList, parameters, -1);
                AddQueryParam("objid", "building_id", buildingId, ref fieldList, ref paramList, parameters, -1);
                AddQueryParam("orgid", "organization_id", orgId, ref fieldList, ref paramList, parameters, -1);
                AddQueryParam("isp", "modified_by", username, ref fieldList, ref paramList, parameters, 18);
                AddQueryParam("dt", "modify_date", DateTime.Now, ref fieldList, ref paramList, parameters, -1);
                AddQueryParam("lyear", "year_balans", DateTime.Now.Year, ref fieldList, ref paramList, parameters, -1);
                AddQueryParam("pr1nf", "priznak_1nf", 0, ref fieldList, ref paramList, parameters, -1); // 0 ~ NOT from 1NF
                AddQueryParam("upds", "update_src_id", 10, ref fieldList, ref paramList, parameters, -1); // BAGRIY V.M.
                AddQueryParam("delt", "is_deleted", 0, ref fieldList, ref paramList, parameters, -1);

                AddQueryParam("addrstr", "obj_street_name", objUlname, ref fieldList, ref paramList, parameters, 100);
                AddQueryParam("addrnom1", "obj_nomer1", objNomer1, ref fieldList, ref paramList, parameters, 9);
                AddQueryParam("addrnom2", "obj_nomer2", objNomer2, ref fieldList, ref paramList, parameters, 18);
                AddQueryParam("addrnom3", "obj_nomer3", objNomer3, ref fieldList, ref paramList, parameters, 10);
                AddQueryParam("addrbti", "obj_bti_code", objKodbti, ref fieldList, ref paramList, parameters, 18);
                AddQueryParam("addrsid", "obj_street_id", objKodul, ref fieldList, ref paramList, parameters, -1);

                AddQueryParam("sqrtot", "sqr_total", objectSquare, ref fieldList, ref paramList, parameters, -1);
                AddQueryParam("balv", "cost_balans", objectCost, ref fieldList, ref paramList, parameters, -1);
                AddQueryParam("fvl", "form_ownership_id", ownershipId, ref fieldList, ref paramList, parameters, -1);
                AddQueryParam("knd", "object_kind_id", objKindId, ref fieldList, ref paramList, parameters, -1);
                AddQueryParam("typ", "object_type_id", objTypeId, ref fieldList, ref paramList, parameters, -1);
                AddQueryParam("pgr", "purpose_group_id", purposeGroupId, ref fieldList, ref paramList, parameters, -1);
                AddQueryParam("prp", "purpose_id", purposeId, ref fieldList, ref paramList, parameters, -1);
                AddQueryParam("prpstr", "purpose_str", use, ref fieldList, ref paramList, parameters, 255);

                

                if (connectionSql != null)
                {
                    using (SqlCommand cmd = connectionSql.CreateCommand())
                    {
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.CommandTimeout = 600;
                        cmd.CommandText = "INSERT INTO balans (" + fieldList.TrimStart(' ', ',') + ") VALUES (" + paramList.TrimStart(' ', ',') + ")";

                        foreach (KeyValuePair<string, object> pair in parameters)
                        {
                            cmd.Parameters.Add(new SqlParameter(pair.Key, pair.Value));
                        }

                        cmd.ExecuteNonQuery();
                    }

                    connectionSql.Close();
                }
            }
        }
        catch (Exception ex)
        {
            newBalansId = -1;
            errorMessage = ex.Message;

            // Roll back the transaction
            //if (transaction != null)
            {
                //transaction.Rollback();
            }
        }

        return newBalansId;
    }

    #endregion Working with 1NF database
}