using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using DevExpress.Web.Data;
using DevExpress.Web;
using DevExpress.Web;
using System.Data;
using System.Data.SqlClient;
using FirebirdSql.Data.FirebirdClient;
using log4net;
using GUKV;
using System.Web.Configuration;
using System.IO;
using DevExpress.Web;
using ExtDataEntry.Models;
using DevExpress.Web;

public partial class Reports1NF_FreeCyclePopupEditRenterOrg : System.Web.UI.Page
{
	protected int freecycle_orendar_id
	{
		get
		{
			return Int32.Parse(Request.QueryString["freecycle_orendar_id"]);
		}
	}

	protected int freecycle_id
	{
		get
		{
			return Int32.Parse(Request.QueryString["freecycle_id"]);
		}
	}

	protected string input_zkpo
	{
		get
		{
			return Request.QueryString["zkpo"].ToString() ?? "";
		}
	}

	protected void Page_Load(object sender, EventArgs e)
    {
		if (freecycle_orendar_id == -1)
		{
			SqlConnection connection = Utils.ConnectToDatabase();
			SqlDataAdapter adp = new SqlDataAdapter("SELECT org.* FROM organizations org where 1=2", connection);
			DataSet ds = new DataSet();
			adp.Fill(ds);
			var rows = ds.Tables[0].Rows;
			rows.Add(ds.Tables[0].NewRow());

			using (SqlCommand cmdTest = new SqlCommand("SELECT zkpo_code, full_name, short_name, status_id FROM organizations_from_derzh_reestr WHERE zkpo_code = @zkpo", connection))
			{
				var b = input_zkpo;
				cmdTest.Parameters.Add(new SqlParameter("zkpo", input_zkpo));
				using (var r = cmdTest.ExecuteReader())
				{
					if (r.Read())
					{
						var row = ds.Tables[0].Rows[0];
						row["zkpo_code"] = r["zkpo_code"];
						row["full_name"] = r["full_name"];
						row["short_name"] = r["short_name"];
						row["status_id"] = r["status_id"];
					}
					r.Close();
				}
			}

			FormView1.DataSourceID = "";
			FormView1.DataSource = ds;
			FormView1.DataBind();
		}
		else
		{
			var textBoxZkpoCodeOrg = (ASPxTextBox)PopupControlContentControl1.FindControlRecursive("TextBoxZkpoCodeOrg");
			textBoxZkpoCodeOrg.ReadOnly = true;
		}
	}

	protected void SqlDataSourceRenter_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
	{
		e.Command.Parameters["@freecycle_orendar_id"].Value = freecycle_orendar_id;
	}

	protected void CPPopUp_Callback(object sender, CallbackEventArgsBase e)
	{
		if (e.Parameter.StartsWith("create_org:") || e.Parameter.StartsWith("edit_org:"))
		{
			ASPxLabel label;
			Control ourPopUp;
			HiddenField HdnNewOrgId;
			if (e.Parameter.StartsWith("create_org:"))
			{
				ourPopUp = (Control)Utils.FindControlRecursive(PopupControlContentControl1, "PopupAddRenterOrg");
				HdnNewOrgId = (HiddenField)Utils.FindControlRecursive(ourPopUp, "NewOrgId");
				label = (ASPxLabel)Utils.FindControlRecursive(ourPopUp, "LabelOrgCreationError");
			}
			else
			{
				ourPopUp = PopupControlContentControl1;
				HdnNewOrgId = (HiddenField)Utils.FindControlRecursive(ourPopUp, "OldOrgId");
				label = (ASPxLabel)Utils.FindControlRecursive(ourPopUp, "LabelOrgEditError");
			}

			int newOrgId = 0;
			Int32.TryParse(HdnNewOrgId.Value, out newOrgId);

			//HdnNewOrgId.Value = "";

			string errorMessage = "";

			string fullName = ((ASPxTextBox)Utils.FindControlRecursive(ourPopUp, "TextBoxFullNameOrg")).Text.Trim();
			string zkpo = ((ASPxTextBox)Utils.FindControlRecursive(ourPopUp, "TextBoxZkpoCodeOrg")).Text.Trim();
			string shortName = ((ASPxTextBox)Utils.FindControlRecursive(ourPopUp, "TextBoxShortNameOrg")).Text.Trim();

			int district = ((ASPxComboBox)Utils.FindControlRecursive(ourPopUp, "ComboBoxDistrictOrg")).Value is int ? (int)((ASPxComboBox)Utils.FindControlRecursive(ourPopUp, "ComboBoxDistrictOrg")).Value : -1;
			string street = ((ASPxTextBox)Utils.FindControlRecursive(ourPopUp, "TextBoxStreetNameOrg")).Text; // Элемент в интерфейсе на самом деле не ASPxComboBox, а ASPxTextBox. Из-за этого сохранить данные невозможно.
			string numOfHouse = ((ASPxTextBox)Utils.FindControlRecursive(ourPopUp, "TextBoxAddrNomerOrg")).Text.Trim();
			string zip = ((ASPxTextBox)Utils.FindControlRecursive(ourPopUp, "TextBoxAddrZipCodeOrg")).Text.Trim();
			string korpus = ((ASPxTextBox)Utils.FindControlRecursive(ourPopUp, "TextBoxAddrKorpusFrom")).Text.Trim();

			int status = ((ASPxComboBox)Utils.FindControlRecursive(ourPopUp, "ComboBoxStatusOrg")).Value is int ? (int)((ASPxComboBox)Utils.FindControlRecursive(ourPopUp, "ComboBoxStatusOrg")).Value : -1;
			int formVlasn = ((ASPxComboBox)Utils.FindControlRecursive(ourPopUp, "ComboBoxFormVlasnOrg")).Value is int ? (int)((ASPxComboBox)Utils.FindControlRecursive(ourPopUp, "ComboBoxFormVlasnOrg")).Value : -1;
			int industry = ((ASPxComboBox)Utils.FindControlRecursive(ourPopUp, "ComboBoxIndustryOrg")).Value is int ? (int)((ASPxComboBox)Utils.FindControlRecursive(ourPopUp, "ComboBoxIndustryOrg")).Value : -1;
			int occupation = ((ASPxComboBox)Utils.FindControlRecursive(ourPopUp, "ComboBoxOccupationFrom")).Value is int ? (int)((ASPxComboBox)Utils.FindControlRecursive(ourPopUp, "ComboBoxOccupationFrom")).Value : -1;

			string directorFio = ((ASPxTextBox)Utils.FindControlRecursive(ourPopUp, "TextBoxDirectorFioOrg")).Text;
			string directorPhone = ((ASPxTextBox)Utils.FindControlRecursive(ourPopUp, "TextBoxDirectorPhoneOrg")).Text;
			string directorEmail = ((ASPxTextBox)Utils.FindControlRecursive(ourPopUp, "TextBoxDirectorEmailOrg")).Text;
			string buhgalterFio = ((ASPxTextBox)Utils.FindControlRecursive(ourPopUp, "TextBoxBuhgalterFioOrg")).Text;
			string buhgalterPhone = ((ASPxTextBox)Utils.FindControlRecursive(ourPopUp, "TextBoxBuhgalterPhoneOrg")).Text;
			string fax = ((ASPxTextBox)Utils.FindControlRecursive(ourPopUp, "TextBoxFax")).Text;

			if (fullName == "")
				errorMessage = "Необхідно заповнити повну назву організації.";
			else if (zkpo == "")
				errorMessage = "Необхідно заповнити Код ЄДРПОУ.";
			else if (shortName == "")
				errorMessage = "Необхідно заповнити коротку назву організації.";
			else if (district == -1)
				errorMessage = "Необхідно вибрати район.";
			else if (street == "")
				errorMessage = "Необхідно заповнити вулицю.";
			else if (numOfHouse == "")
				errorMessage = "Необхідно заповнити номер будинку.";
			else if (zip == "")
				errorMessage = "Необхідно заповнити поштовий індекс.";
			else if (status == -1)
				errorMessage = "Необхідно вибрати статус.";
			else if (status == 1 && zkpo.Length != 8)
				errorMessage = "Код ЄДРПОУ має бути 8 символів у довжину.";
			else if (status == 2 && zkpo.Length != 10)
				errorMessage = "Код ЄДРПОУ має бути 10 символів у довжину.";
			else if (zkpo.Count(q => q >= '0' && q <= '9') != zkpo.Length)
				errorMessage = "ЄДРПОУ/ІНН повинен містити тільки цифри.";
			else if (!(new[] { 8, 9, 10 }.Contains(zkpo.Length)))
				errorMessage = "ЄДРПОУ/ІНН повинен мати 8, 9 або 10 цифр.";
			else if (formVlasn == -1)
				errorMessage = "Необхідно вибрати форму власності.";
			else if (directorFio == "")
				errorMessage = "Необхідно заповнити ПІБ директора.";
			else if (directorPhone == "")
				errorMessage = "Необхідно заповнити телефон директора.";
			else if (directorEmail == "")
				errorMessage = "Необхідно заповнити Email директора.";


			if (errorMessage == "")
			{
				//var newOrgId = 0;

				//FbConnection connection = Utils.ConnectTo1NF();

				//if (connection != null)
				//{
				if (newOrgId != 0)
				{
					newOrgId = RishProjectExport.Update1NFOrganization(
						//connection,
						newOrgId,
						fullName,
						shortName,
						zkpo,
						industry,
						occupation,
						formVlasn,
						status,
						-1,
						-1,
						-1,
						directorFio,
						directorPhone,
						directorEmail,
						buhgalterFio,
						buhgalterPhone,
						fax,
						"", // kved
						district,
						street,
						numOfHouse,
						korpus,
						zip,
						//true,
						out errorMessage);
				}
				else
				{
					// Check if ZKPO code already exists
					using (SqlConnection validateConnectionSql = Utils.ConnectToDatabase())
					{
						using (SqlCommand cmdTest = new SqlCommand("SELECT zkpo_code FROM organizations WHERE isactive = 1 and zkpo_code = @zkpo", validateConnectionSql))
						{
							//cmdTest.Transaction = transaction;
							cmdTest.Parameters.Add(new SqlParameter("zkpo", zkpo));
							using (var r = cmdTest.ExecuteReader())
							{
								if (r.Read())
								{
									errorMessage = "В базі вже існує картка з таким ЄДРПОУ/ІНН";
								}
								r.Close();
							}
						}
					}

					if (errorMessage == "")
					{
						newOrgId = RishProjectExport.CreateNewActive1NFOrganization(
							//connection,
							fullName,
							shortName,
							zkpo,
							industry,
							occupation,
							formVlasn,
							status,
							-1,
							-1,
							-1,
							directorFio,
							directorPhone,
							directorEmail,
							buhgalterFio,
							buhgalterPhone,
							fax,
							"", // kved
							district,
							street,
							numOfHouse,
							korpus,
							zip,
							//true,
							out errorMessage);

						var connectionSql = Utils.ConnectToDatabase();
						FreeCycleUtils.AddOrendar(connectionSql, freecycle_id, newOrgId);
					}
				}

				//connection.Close();

				//if (newOrgId > 0)
				//{
				//	HdnNewOrgId.Value = newOrgId.ToString();

				//	ASPxTextBox EditRenterOrgZKPO = (ASPxTextBox)Utils.FindControlRecursive(OrganizationsForm, "EditRenterOrgZKPO");
				//	EditRenterOrgZKPO.Text = zkpo;
				//	RenterOrgZkpoPattern = zkpo;

				//	ASPxTextBox EditRenterOrgName = (ASPxTextBox)Utils.FindControlRecursive(OrganizationsForm, "EditRenterOrgName");
				//	EditRenterOrgName.Text = fullName;
				//	RenterOrgNamePattern = fullName;
				//}
				//}
			}

			if (errorMessage != "")
			{
				label.Text = errorMessage;
				label.ClientVisible = (errorMessage.Length > 0);
			}
		}
	}

}