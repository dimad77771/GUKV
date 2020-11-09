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

public partial class Reports1NF_FreeCycle : System.Web.UI.Page
{
    private static readonly ILog log = LogManager.GetLogger("ReportWebSite");

	protected int freecycle_id;
	protected int free_square_id;
	protected int cycle_num;
	protected string cycle_reg_number;
	protected bool is_read_only;
	protected string free_square_name;
	protected List<freecycle_step_dict> list_step_dict;
	protected List<freecycle_orendar> list_freecycle_orendar;
	protected List<freecycle_step> list_freecycle_step;
	protected List<control_cell> list_control_cell = new List<control_cell>();

	protected void Page_Load(object sender, EventArgs e)
	{
		//if (IsCallback)
		//{
		//	return;
		//}

		is_read_only = false;

		if (LoadFromFreeSquareId())
		{
			return;
		}

		freecycle_id = Int32.Parse(Request.QueryString["freecycle_id"]);

		BuildAllData();

		ASPxLabelHeader.Text += (is_read_only ? "Архівна картка" : "Картка") + " процесу передачі в оренду вільного приміщення (реєстраційний номер " + cycle_reg_number + ")";
		ASPxLabelHeader2.Text = free_square_name;
		ASPxLabelHeader2.Style.Add("margin-left", "10px");
		ASPxLabelHeader2.Style.Add("color", "#AFAFAF");
		if (is_read_only)
		{
			CardPageControl.TabPages[1].Visible = false;
		}

		var ww0 = 500;
		var ww1 = 100;
		var ww2 = 100;
		var ww3 = 70;
		var ww9 = 60;

		var controls = ContentControl1.Controls;

		controls.Add(new LiteralControl(@"<div style="""">"));
		controls.Add(new LiteralControl(@"<table border=""0"" style=""border:0px solid black; border-spacing:0px"">"));
		controls.Add(new LiteralControl(@"<colgroup><col width=""" + ww0 + @"px""/>"));
		foreach (var row_orendar in list_freecycle_orendar)
		{
			controls.Add(new LiteralControl(@"<col width=""" + (ww1 + ww2 + ww3 - ww9) + @"px""/><col width=""" + (62) + @"px""/>"));
		}
		controls.Add(new LiteralControl(@"</colgroup>"));

		controls.Add(new LiteralControl(@"<tr>"));
		controls.Add(new LiteralControl(@"<td style=""text-align:center;padding-bottom:8px"">"));
		var lab2 = new ASPxLabel() { Text = "Этап" };
		lab2.Style.Add("font-weight", "bold");
		lab2.Style.Add("text-decoration", "underline");
		controls.Add(lab2);
		controls.Add(new LiteralControl(@"</td>"));
		foreach (var row_orendar in list_freecycle_orendar)
		{
			controls.Add(new LiteralControl(@"<td style=""text-align:left; border:0px solid black"">"));
			var lab_orend = new ASPxLabel() { Text = row_orendar.full_name };
			lab_orend.Style.Add("font-weight", "bold");
			lab_orend.Style.Add("text-decoration", "underline");
			controls.Add(lab_orend);

			//var but1_html = @"<img title=""Редагувати"" onclick=""editOrendar(" + row_orendar.freecycle_orendar_id + @")""" +
			//		@"src =""../Styles/EditIcon.png"" alt=""Редагувати"" style=""cursor:pointer;"">";
			//controls.Add(new LiteralControl(but1_html));


			controls.Add(new LiteralControl(@"</td>"));

			controls.Add(new LiteralControl(@"<td style=""text-align:left"">"));
			var but1_html = @"<img title=""Редагувати"" onclick=""editOrendar(" + row_orendar.freecycle_orendar_id + @")""" +
					@"src =""../Styles/EditIcon.png"" alt=""Редагувати"" style=""cursor:pointer;"">";
			controls.Add(new LiteralControl(but1_html));
			var but2_html = @"<img title=""Вилучити"" onclick=""deleteOrendar(" + row_orendar.freecycle_orendar_id + @")""" +
					@"src =""../Styles/DeleteIcon.png"" alt=""Вилучити"" style=""cursor:pointer;" + (is_read_only ? "display:none" : "") + @""">";
			controls.Add(new LiteralControl(but2_html));

			//var lab_orend2 = new ASPxLabel() { Text = "A" };
			//controls.Add(lab_orend2);
			controls.Add(new LiteralControl(@"</td>"));
		}
		controls.Add(new LiteralControl(@"</tr>"));
		controls.Add(new LiteralControl(@"</table>"));
		controls.Add(new LiteralControl(@"</div>"));



		controls.Add(new LiteralControl(@"<div id=""div_data_table"" style=""overflow-y:scroll; height:500px"">"));
		controls.Add(new LiteralControl(@"<table border=""0"" style=""border:0px solid black; border-spacing:0px"">"));
		controls.Add(new LiteralControl(@"<colgroup><col width=""" + ww0 + @"px""/>"));
		foreach (var row_orendar in list_freecycle_orendar)
		{
			controls.Add(new LiteralControl(@"<col width=""" + ww1 + @"px""/><col width=""" + ww2 + @"px""/><col width=""" + ww3 + @"px""/>"));
		}
		controls.Add(new LiteralControl(@"</colgroup>"));
		var step_row_num = 0;
		foreach (var step_row in list_step_dict.OrderBy(q => q.step_ord))
		{
			var backrowcolor = (step_row_num % 2 == 0 ? "#FFFFFF" : "#E5E5E5");
			//var backColumnColor = (is_read_only ? System.Drawing.Color.FromName("#E5E5E5") : System.Drawing.Color.Transparent);
			//var backColumnColor = System.Drawing.Color.Transparent;
			step_row_num++;

			var spec_row_mode = GetRowSpecMode(step_row);

			controls.Add(new LiteralControl(@"<tr style=""background-color:" + backrowcolor + @""">"));
			controls.Add(new LiteralControl(@"<td style=""text-align:right;padding-bottom:8px"">"));
			var lab = new ASPxLabel() { Text = step_row.step_name };
			if (step_row.istitle)
			{
				lab.Style.Add("font-weight", "bold");
			}
			controls.Add(lab);
			controls.Add(new LiteralControl(@"</td>"));
			foreach (var row_orendar in list_freecycle_orendar)
			{
				var steprow = list_freecycle_step.Single(q => q.freecycle_orendar_id == row_orendar.freecycle_orendar_id && q.step_id == step_row.step_id);

				controls.Add(new LiteralControl(@"<td style=""padding-bottom:8px"">"));

				var dateEdit = new ASPxDateEdit()
				{
					Width = 100,
					ToolTip = step_row.header_dat,
					Visible = !string.IsNullOrEmpty(step_row.header_dat),
					ReadOnly = is_read_only,
					//BackColor = backColumnColor,
				};
				if (steprow.step_date != null)
				{
					dateEdit.Date = steprow.step_date.Value;
				}
				dateEdit.ClientSideEvents.DateChanged = OnDateChangedClientSideEvent();
				controls.Add(dateEdit);
				controls.Add(new LiteralControl(@"</td>"));

				controls.Add(new LiteralControl(@"<td style=""padding-bottom:8px"">"));
				var textBox = new ASPxTextBox()
				{
					Width = 100,
					ToolTip = step_row.header_doc,
					Visible = !string.IsNullOrEmpty(step_row.header_doc),
					ReadOnly = is_read_only,
					//BackColor = backColumnColor,
				};
				if (steprow != null)
				{
					textBox.Text = steprow.step_docnum;
				}
				textBox.ClientSideEvents.ValueChanged = OnDateChangedClientSideEvent();
				controls.Add(textBox);
				controls.Add(new LiteralControl(@"</td>"));

				controls.Add(new LiteralControl(@"<td style=""padding-bottom:8px"">"));
				var but_html = @"<img title=""Зображення документу PDF"" onclick=""showDocuments(" + steprow.freecycle_step_id + @")""" +
						@"src =""../Styles/current_stage_pdf.png"" alt=""Зображення документу PDF"" style=""cursor:pointer;"">";
				controls.Add(new LiteralControl(but_html));
				controls.Add(new LiteralControl(@"</td>"));

				var cell = new control_cell
				{
					freecycle_orendar_id = row_orendar.freecycle_orendar_id,
					step_id = step_row.step_id,
					CellData = steprow,
					DateEdit = dateEdit,
					DocnumEdit = textBox,
				};
				list_control_cell.Add(cell);
			}
			controls.Add(new LiteralControl(@"</tr>"));



			if (!string.IsNullOrEmpty(step_row.header_c1) && spec_row_mode == 0)
			{
				controls.Add(new LiteralControl(@"<tr style=""background-color:" + backrowcolor + @""">"));
				controls.Add(new LiteralControl(@"<td style=""text-align:right;padding-bottom:8px"">"));
				var lab33 = new ASPxLabel() { Text = step_row.header_c1 };
				lab33.Style.Add("font-style", "italic");
				controls.Add(lab33);
				controls.Add(new LiteralControl(@"</td>"));
				foreach (var row_orendar in list_freecycle_orendar)
				{
					var steprow = list_freecycle_step.Single(q => q.freecycle_orendar_id == row_orendar.freecycle_orendar_id && q.step_id == step_row.step_id);

					controls.Add(new LiteralControl(@"<td style=""padding-bottom:8px"">"));
					var dateEdit = new ASPxDateEdit()
					{
						Width = 100,
						ToolTip = step_row.header_c1,
						ReadOnly = is_read_only,
						//BackColor = backColumnColor,
					};
					if (steprow.step_c1 != null)
					{
						dateEdit.Date = steprow.step_c1.Value;
					}
					dateEdit.ClientSideEvents.DateChanged = OnDateChangedClientSideEvent();
					controls.Add(dateEdit);
					controls.Add(new LiteralControl(@"</td>"));

					controls.Add(new LiteralControl(@"<td style=""padding-bottom:8px"">"));
					controls.Add(new LiteralControl(@"</td>"));
					controls.Add(new LiteralControl(@"<td style=""padding-bottom:8px"">"));
					controls.Add(new LiteralControl(@"</td>"));

					var cell = list_control_cell.Single(q => q.freecycle_orendar_id == row_orendar.freecycle_orendar_id && q.step_id == step_row.step_id);
					cell.Column1Edit = dateEdit;
				}
				controls.Add(new LiteralControl(@"</tr>"));
			}


			if (!string.IsNullOrEmpty(step_row.header_c2) && spec_row_mode == 0)
			{
				controls.Add(new LiteralControl(@"<tr style=""background-color:" + backrowcolor + @""">"));
				controls.Add(new LiteralControl(@"<td style=""text-align:right;padding-bottom:8px"">"));
				var lab33 = new ASPxLabel() { Text = step_row.header_c2 };
				lab33.Style.Add("font-style", "italic");
				controls.Add(lab33);
				controls.Add(new LiteralControl(@"</td>"));
				foreach (var row_orendar in list_freecycle_orendar)
				{
					var steprow = list_freecycle_step.Single(q => q.freecycle_orendar_id == row_orendar.freecycle_orendar_id && q.step_id == step_row.step_id);

					controls.Add(new LiteralControl(@"<td style=""padding-bottom:8px"">"));
					var spinEdit = new ASPxSpinEdit()
					{
						Width = 100,
						ToolTip = step_row.header_c2,
						NumberType = SpinEditNumberType.Float,
						ReadOnly = is_read_only,
						//BackColor = backColumnColor,
					};
					if (steprow.step_c2 != null)
					{
						spinEdit.Number = steprow.step_c2.Value;
					}
					spinEdit.ClientSideEvents.ValueChanged = OnDateChangedClientSideEvent();
					controls.Add(spinEdit);

					controls.Add(new LiteralControl(@"</td>"));

					controls.Add(new LiteralControl(@"<td style=""padding-bottom:8px"">"));
					controls.Add(new LiteralControl(@"</td>"));
					controls.Add(new LiteralControl(@"<td style=""padding-bottom:8px"">"));
					controls.Add(new LiteralControl(@"</td>"));

					var cell = list_control_cell.Single(q => q.freecycle_orendar_id == row_orendar.freecycle_orendar_id && q.step_id == step_row.step_id);
					cell.Column2Edit = spinEdit;
				}
				controls.Add(new LiteralControl(@"</tr>"));
			}


			if (!string.IsNullOrEmpty(step_row.header_c3) && spec_row_mode == 0)
			{
				controls.Add(new LiteralControl(@"<tr style=""background-color:" + backrowcolor + @""">"));
				controls.Add(new LiteralControl(@"<td style=""text-align:right;padding-bottom:8px"">"));
				var lab33 = new ASPxLabel() { Text = step_row.header_c3 };
				lab33.Style.Add("font-style", "italic");
				controls.Add(lab33);
				controls.Add(new LiteralControl(@"</td>"));
				foreach (var row_orendar in list_freecycle_orendar)
				{
					var steprow = list_freecycle_step.Single(q => q.freecycle_orendar_id == row_orendar.freecycle_orendar_id && q.step_id == step_row.step_id);

					controls.Add(new LiteralControl(@"<td style=""padding-bottom:8px"">"));
					var textBox = new ASPxTextBox()
					{
						Width = 100,
						ToolTip = step_row.header_c3,
						ReadOnly = is_read_only,
					};
					if (steprow.step_c3 != null)
					{
						textBox.Text = steprow.step_c3;
					}
					textBox.ClientSideEvents.ValueChanged = OnDateChangedClientSideEvent();
					controls.Add(textBox);

					controls.Add(new LiteralControl(@"</td>"));

					controls.Add(new LiteralControl(@"<td style=""padding-bottom:8px"">"));
					controls.Add(new LiteralControl(@"</td>"));
					controls.Add(new LiteralControl(@"<td style=""padding-bottom:8px"">"));
					controls.Add(new LiteralControl(@"</td>"));

					var cell = list_control_cell.Single(q => q.freecycle_orendar_id == row_orendar.freecycle_orendar_id && q.step_id == step_row.step_id);
					cell.Column3Edit = textBox;
				}
				controls.Add(new LiteralControl(@"</tr>"));
			}


			if (spec_row_mode == 10)
			{
				controls.Add(new LiteralControl(@"<tr style=""background-color:" + backrowcolor + @""">"));
				controls.Add(new LiteralControl(@"<td style=""text-align:right;padding-bottom:8px"">"));
				var lab33 = new ASPxLabel() { Text = @"Підстава та дата відмови" };
				lab33.Style.Add("font -style", "italic");
				controls.Add(lab33);
				controls.Add(new LiteralControl(@"</td>"));
				foreach (var row_orendar in list_freecycle_orendar)
				{
					var steprow = list_freecycle_step.Single(q => q.freecycle_orendar_id == row_orendar.freecycle_orendar_id && q.step_id == step_row.step_id);

					controls.Add(new LiteralControl(@"<td style=""padding-bottom:8px"">"));

					var textBox = new ASPxComboBox()
					{
						Width = 100,
						ToolTip = "Підстава відмови",
						ReadOnly = is_read_only,
						ValueType = typeof(Int32),
						TextField = "rejection_full_name",
						ValueField = "rejection_id",
						IncrementalFilteringMode = IncrementalFilteringMode.StartsWith,
						DataSourceID = "SqlDataSourceFreecycleRejectionDict",
					};
					if (steprow.step_c4 != null)
					{
						textBox.Value = steprow.step_c4;
					}
					textBox.ClientSideEvents.ValueChanged = OnDateChangedClientSideEvent();
					controls.Add(textBox);
					controls.Add(new LiteralControl(@"</td>"));

					controls.Add(new LiteralControl(@"<td style=""padding-bottom:8px"">"));
					var dateEdit = new ASPxDateEdit()
					{
						Width = 100,
						ToolTip = "Дата відмови",
						ReadOnly = is_read_only,
					};
					if (steprow.step_c1 != null)
					{
						dateEdit.Date = steprow.step_c1.Value;
					}
					dateEdit.ClientSideEvents.DateChanged = OnDateChangedClientSideEvent();
					controls.Add(dateEdit);
					controls.Add(new LiteralControl(@"</td>"));

					controls.Add(new LiteralControl(@"<td style=""padding-bottom:8px"">"));
					controls.Add(new LiteralControl(@"</td>"));

					var cell = list_control_cell.Single(q => q.freecycle_orendar_id == row_orendar.freecycle_orendar_id && q.step_id == step_row.step_id);
					cell.Column1Edit = dateEdit;
					cell.Column4Edit = textBox;
				}
				controls.Add(new LiteralControl(@"</tr>"));
			}

		}
		controls.Add(new LiteralControl(@"</table>"));
		controls.Add(new LiteralControl(@"</div>"));

		controls.Add(new LiteralControl(@"<div style=""overflow-y:scroll; height:100px; display:none"">"));
		foreach (var cell in list_control_cell)
		{
			if (cell.DateEdit != null)
			{
				var edit = new ASPxDateEdit
				{
					Date = cell.DateEdit.Date,
				};
				controls.Add(edit);
				cell.hidden_DateEdit = edit;
			}

			if (cell.DocnumEdit != null)
			{
				var edit = new ASPxTextBox
				{
					Text = cell.DocnumEdit.Text,
				};
				controls.Add(edit);
				cell.hidden_DocnumEdit = edit;
			}

			if (cell.Column1Edit != null)
			{
				var edit = new ASPxDateEdit
				{
					Date = cell.Column1Edit.Date,
				};
				controls.Add(edit);
				cell.hidden_Column1Edit = edit;
			}

			if (cell.Column2Edit != null)
			{
				var edit = new ASPxSpinEdit
				{
					Number = cell.Column2Edit.Number,
				};
				controls.Add(edit);
				cell.hidden_Column2Edit = edit;
			}

			if (cell.Column3Edit != null)
			{
				var edit = new ASPxTextBox
				{
					Text = cell.Column3Edit.Text,
				};
				controls.Add(edit);
				cell.hidden_Column3Edit = edit;
			}

			if (cell.Column4Edit != null)
			{
				var edit = new ASPxComboBox
				{
					Value = cell.Column4Edit.Value,
				};
				controls.Add(edit);
				cell.hidden_Column4Edit = edit;
			}
		}
		controls.Add(new LiteralControl(@"</div>"));
	}

	public class control_cell
	{
		public int freecycle_orendar_id { get; set; }
		public int step_id { get; set; }
		public freecycle_step CellData { get; set; }

		public ASPxDateEdit DateEdit { get; set; }
		public ASPxTextBox DocnumEdit { get; set; }
		public ASPxDateEdit Column1Edit { get; set; }
		public ASPxSpinEdit Column2Edit { get; set; }
		public ASPxTextBox Column3Edit { get; set; }
		public ASPxComboBox Column4Edit { get; set; }

		public ASPxDateEdit hidden_DateEdit { get; set; }
		public ASPxTextBox hidden_DocnumEdit { get; set; }
		public ASPxDateEdit hidden_Column1Edit { get; set; }
		public ASPxSpinEdit hidden_Column2Edit { get; set; }
		public ASPxTextBox hidden_Column3Edit { get; set; }
		public ASPxComboBox hidden_Column4Edit { get; set; }
	}
	public class freecycle_step_dict
	{
		public int step_id { get; set; }
		public string step_cod { get; set; }
		public string step_name { get; set; }
		public int step_ord { get; set; }
		public bool istitle { get; set; }
		public string header_dat { get; set; }
		public string header_doc { get; set; }
		public string header_c1 { get; set; }
		public string header_c2 { get; set; }
		public string header_c3 { get; set; }
		public string header_c4 { get; set; }
		public string header_c5 { get; set; }
	}
	public class freecycle_orendar
	{
		public int freecycle_orendar_id { get; set; }
		public int freecycle_id { get; set; }
		public int npp { get; set; }
		public int org_orendar_id { get; set; }
		public string full_name { get; set; }
		public string zkpo_code { get; set; }
		public DateTime? modify_date { get; set; }
		public string modified_by { get; set; }
	}
	public class freecycle_step
	{
		public int freecycle_step_id { get; set; }
		public int freecycle_id { get; set; }
		public int freecycle_orendar_id { get; set; }
		public int step_id { get; set; }
		public DateTime? step_date { get; set; }
		public string step_docnum { get; set; }
		public DateTime? step_c1 { get; set; }
		public Decimal? step_c2 { get; set; }
		public string step_c3 { get; set; }
		public Int32? step_c4 { get; set; }
		public DateTime? modify_date { get; set; }
		public string modified_by { get; set; }

		public bool IsEqv(freecycle_step arg)
		{
			return (
				(step_date ?? default(DateTime)) == (arg.step_date ?? default(DateTime)) &&
				(step_docnum ?? "") == (arg.step_docnum ?? "") &&
				(step_c1 ?? default(DateTime)) == (arg.step_c1 ?? default(DateTime)) &&
				(step_c2 ?? Decimal.MinValue) == (arg.step_c2 ?? Decimal.MinValue) &&
				(step_c3 ?? "") == (arg.step_c3 ?? "") &&
				(step_c4 ?? Int32.MaxValue) == (arg.step_c4 ?? Int32.MaxValue) &&
				true
			);
		}

		public void CopyData(freecycle_step arg)
		{
			step_date = arg.step_date;
			step_docnum = arg.step_docnum;
			step_c1 = arg.step_c1;
			step_c2 = arg.step_c2;
			step_c3 = arg.step_c3;
			step_c4 = arg.step_c4;
		}
	}

	bool LoadFromFreeSquareId()
	{
		int free_square_id;
		if (Int32.TryParse(Request.QueryString["free_square_id"], out free_square_id))
		{
			SqlConnection connection = Utils.ConnectToDatabase();
			SqlTransaction trans = connection.BeginTransaction();

			var freecycle_id = FreeCycleUtils.GetMaxFreeCycleId(connection, trans, free_square_id);
			if (freecycle_id == null)
			{
				FreeCycleUtils.AddCycle(connection, trans, free_square_id);
				freecycle_id = FreeCycleUtils.GetMaxFreeCycleId(connection, trans, free_square_id);
			}
			freecycle_id = (int)freecycle_id;

			trans.Commit();
			Response.Redirect(@"FreeCycle.aspx?freecycle_id=" + freecycle_id);
			return true;
		}

		return false;
	}

	void BuildAllData()
	{
		SqlConnection connection = Utils.ConnectToDatabase();

		var sql3 = "select [free_square_id] from [freecycle] where [freecycle_id] = @freecycle_id";
		SqlCommand cmd3 = new SqlCommand(sql3, connection);
		cmd3.Parameters.Add(new SqlParameter("freecycle_id", freecycle_id));
		free_square_id = (int)cmd3.ExecuteScalar();

		var sql4 = "select [reg_number] from [freecycle] where [freecycle_id] = @freecycle_id";
		SqlCommand cmd4 = new SqlCommand(sql4, connection);
		cmd4.Parameters.Add(new SqlParameter("freecycle_id", freecycle_id));
		cycle_reg_number = (string)cmd4.ExecuteScalar();

		var sql5 = "select [cycle_num] from [freecycle] where [freecycle_id] = @freecycle_id";
		SqlCommand cmd5 = new SqlCommand(sql5, connection);
		cmd5.Parameters.Add(new SqlParameter("freecycle_id", freecycle_id));
		cycle_num = (int)cmd5.ExecuteScalar();


		list_step_dict = new List<freecycle_step_dict>();
		using (var cmd = new SqlCommand("select * from [freecycle_step_dict] A where A.[is_deleted] = 0", connection))
		{
			using (var reader = cmd.ExecuteReader())
			{
				while (reader.Read())
				{
					var row = new freecycle_step_dict
					{
						step_id = (int)reader["step_id"],
						step_cod = (string)reader["step_cod"],
						step_name = (string)reader["step_name"],
						step_ord = (int)reader["step_ord"],
						istitle = (bool)reader["istitle"],
						header_dat = (string)reader["header_dat"],
						header_doc = (string)reader["header_doc"],
						header_c1 = (string)reader["header_c1"],
						header_c2 = (string)reader["header_c2"],
						header_c3 = (string)reader["header_c3"],
						header_c4 = (string)reader["header_c4"],
						header_c5 = (string)reader["header_c5"],
					};
					list_step_dict.Add(row);
				}
				reader.Close();
			}
		}

		list_freecycle_orendar = new List<freecycle_orendar>();
		var sql2 = "select A.*, B.[full_name], B.[zkpo_code] from [freecycle_orendar] A left join [organizations] B on B.[id] = A.[org_orendar_id] " +
						@"where A.[freecycle_id]=" + freecycle_id + " and A.[is_deleted] = 0 order by A.npp";
		using (var cmd = new SqlCommand(sql2, connection))
		{
			using (var reader = cmd.ExecuteReader())
			{
				while (reader.Read())
				{
					var row = new freecycle_orendar
					{
						freecycle_orendar_id = (int)reader["freecycle_orendar_id"],
						freecycle_id = (int)reader["freecycle_id"],
						npp = (int)reader["npp"],
						org_orendar_id = (int)reader["org_orendar_id"],
						full_name = (string)reader["full_name"],
						zkpo_code = (string)reader["zkpo_code"],
					};
					list_freecycle_orendar.Add(row);
				}
				reader.Close();
			}
		}

		list_freecycle_step = new List<freecycle_step>();
		using (var cmd = new SqlCommand("select * from [freecycle_step] A where A.[freecycle_id]=" + freecycle_id + " and A.[is_deleted] = 0", connection))
		{
			using (var reader = cmd.ExecuteReader())
			{
				while (reader.Read())
				{
					var row = new freecycle_step
					{
						freecycle_step_id = (int)reader["freecycle_step_id"],
						freecycle_id = (int)reader["freecycle_id"],
						freecycle_orendar_id = (int)reader["freecycle_orendar_id"],
						step_id = (int)reader["step_id"],
						step_date = GetDateTime(reader, "step_date"),
						step_docnum = GetString(reader, "step_docnum"),
						step_c1 = GetDateTime(reader, "step_c1"),
						step_c2 = GetDecimal(reader, "step_c2"),
						step_c3 = GetString(reader, "step_c3"),
						step_c4 = GetInteger(reader, "step_c4"),
						modify_date = GetDateTime(reader, "modify_date"),
						modified_by = GetString(reader, "modified_by"),
					};
					list_freecycle_step.Add(row);
				}
				reader.Close();
			}
		}

		var max_cycle_num = FreeCycleUtils.GetMaxCycleNum(connection, null, free_square_id);
		is_read_only = (cycle_num < max_cycle_num);


		var sql9 = @"select
isnull(street_name, '') + ', ' + isnull(addr_nomer, '') + ', ' + cast(total_free_sqr as varchar(100)) + ' кв.м.' as free_square_name
from
(
	select
	rtrim(ltrim(b.street_full_name)) as street_name,
	rtrim(ltrim((COALESCE(LTRIM(RTRIM(b.addr_nomer1)) + ' ', '') + COALESCE(LTRIM(RTRIM(b.addr_nomer2)) + ' ', '') + COALESCE(LTRIM(RTRIM(b.addr_nomer3)), '')))) as addr_nomer,
	fs.total_free_sqr
	FROM view_reports1nf rep
	join reports1nf_balans bal on bal.report_id = rep.report_id
	JOIN view_reports1nf_buildings b ON b.unique_id = bal.building_1nf_unique_id
	join dbo.reports1nf_balans_free_square fs on fs.balans_id = bal.id and fs.report_id = rep.report_id
	where fs.id = @free_square_id
) as t";
		SqlCommand cmd9 = new SqlCommand(sql9, connection);
		cmd9.Parameters.Add(new SqlParameter("free_square_id", free_square_id));
		free_square_name = (string)cmd9.ExecuteScalar();
	}



	protected void SaveChanges()
    {
		SqlConnection connection = null;
		SqlTransaction transaction = null;
		try
		{
			connection = Utils.ConnectToDatabase();
			transaction = connection.BeginTransaction();

			var username = Utils.GetUser();
			var modify_date = DateTime.Now;

			foreach (var icell in list_control_cell)
			{
				var nvalue = new freecycle_step
				{
					step_date = GetASPxDateEditValue(icell.DateEdit),
					step_docnum = GetASPxTextBoxValue(icell.DocnumEdit),
					step_c1 = GetASPxDateEditValue(icell.Column1Edit),
					step_c2 = GetASPxSpinEditValue(icell.Column2Edit),
					step_c3 = GetASPxTextBoxValue(icell.Column3Edit),
					step_c4 = GetASPxComboBoxValue(icell.Column4Edit),
				};
				var ovalue = new freecycle_step
				{
					step_date = GetASPxDateEditValue(icell.hidden_DateEdit),
					step_docnum = GetASPxTextBoxValue(icell.hidden_DocnumEdit),
					step_c1 = GetASPxDateEditValue(icell.hidden_Column1Edit),
					step_c2 = GetASPxSpinEditValue(icell.hidden_Column2Edit),
					step_c3 = GetASPxTextBoxValue(icell.hidden_Column3Edit),
					step_c4 = GetASPxComboBoxValue(icell.hidden_Column4Edit),
				};

				if (!ovalue.IsEqv(nvalue))
				{
					using (var cmd = new SqlCommand(
						@"update [freecycle_step] set
								[step_date]					= @step_date,
								[step_docnum]				= @step_docnum,
								[step_c1]					= @step_c1,
								[step_c2]					= @step_c2,
								[step_c3]					= @step_c3,
								[step_c4]					= @step_c4,
								[modify_date]				= @modify_date,
								[modified_by]				= @modified_by
							where 1=1
								and [freecycle_id]			= @freecycle_id
								and [freecycle_orendar_id]	= @freecycle_orendar_id
								and [step_id]		= @step_id", connection, transaction))
					{
						cmd.Parameters.Add(GetSqlParameter("freecycle_id", freecycle_id));
						cmd.Parameters.Add(GetSqlParameter("freecycle_orendar_id", icell.freecycle_orendar_id));
						cmd.Parameters.Add(GetSqlParameter("step_id", icell.step_id));
						cmd.Parameters.Add(GetSqlParameter("step_date", nvalue.step_date));
						cmd.Parameters.Add(GetSqlParameter("step_docnum", nvalue.step_docnum));
						cmd.Parameters.Add(GetSqlParameter("step_c1", nvalue.step_c1));
						cmd.Parameters.Add(GetSqlParameter("step_c2", nvalue.step_c2));
						cmd.Parameters.Add(GetSqlParameter("step_c3", nvalue.step_c3));
						cmd.Parameters.Add(GetSqlParameter("step_c4", nvalue.step_c4));
						cmd.Parameters.Add(GetSqlParameter("modify_date", modify_date));
						cmd.Parameters.Add(GetSqlParameter("modified_by", username));
						var rowUpdated = cmd.ExecuteNonQuery();
						if (rowUpdated != 1) throw new Exception("FreeCycle update error");
					}
					icell.CellData.CopyData(nvalue);
				}
			}

			FreeCycleUtils.UpdateStageInfo(connection, transaction, free_square_id);

			transaction.Commit();
			connection.Close();
		}
		catch(Exception ex)
		{
			if (transaction != null)
			{
				transaction.Rollback();
			}
			if (connection != null)
			{
				connection.Close();
			}

			var lognet = log4net.LogManager.GetLogger("ReportWebSite");
			lognet.Debug("--------------- FreeCycle SaveChanges ----------------", ex);
			throw ex;
		}
	}


	protected void CPMainPanel_Callback(object sender, CallbackEventArgsBase e)
	{
		if (e.Parameter.StartsWith("save:"))
		{
			SaveChanges();
		}
		else if (e.Parameter.StartsWith("refresh:"))
		{
		}
	}

	protected void CPDeleteOrendar_Callback(object sender, CallbackEventArgsBase e)
	{
		var freecycle_orendar_id = Int32.Parse(e.Parameter);

		SqlConnection connection = null;
		SqlTransaction transaction = null;
		try
		{
			connection = Utils.ConnectToDatabase();
			transaction = connection.BeginTransaction();

			var username = Utils.GetUser();
			var modify_date = DateTime.Now;

			{
				using (var cmd = new SqlCommand(
					@"update [freecycle_orendar] set
							[is_deleted]				= 1,
							[modify_date]				= @modify_date,
							[modified_by]				= @modified_by
						where 1=1
							and [freecycle_id]			= @freecycle_id
							and [freecycle_orendar_id]	= @freecycle_orendar_id", connection, transaction))
				{
					cmd.Parameters.Add(GetSqlParameter("freecycle_id", freecycle_id));
					cmd.Parameters.Add(GetSqlParameter("freecycle_orendar_id", freecycle_orendar_id));
					cmd.Parameters.Add(GetSqlParameter("modify_date", modify_date));
					cmd.Parameters.Add(GetSqlParameter("modified_by", username));
					var rowUpdated = cmd.ExecuteNonQuery();
					if (rowUpdated != 1) throw new Exception("FreeCycle update error");
				}
			}


			{
				using (var cmd = new SqlCommand(
					@"update [freecycle_step] set
							[is_deleted]				= 1,
							[modify_date]				= @modify_date,
							[modified_by]				= @modified_by
						where 1=1
							and [freecycle_id]			= @freecycle_id
							and [freecycle_orendar_id]	= @freecycle_orendar_id", connection, transaction))
				{
					cmd.Parameters.Add(GetSqlParameter("freecycle_id", freecycle_id));
					cmd.Parameters.Add(GetSqlParameter("freecycle_orendar_id", freecycle_orendar_id));
					cmd.Parameters.Add(GetSqlParameter("modify_date", modify_date));
					cmd.Parameters.Add(GetSqlParameter("modified_by", username));
					var rowUpdated = cmd.ExecuteNonQuery();
				}
			}

			FreeCycleUtils.UpdateStageInfo(connection, transaction, free_square_id);

			transaction.Commit();
			connection.Close();
		}
		catch (Exception ex)
		{
			if (transaction != null)
			{
				transaction.Rollback();
			}
			if (connection != null)
			{
				connection.Close();
			}

			var lognet = log4net.LogManager.GetLogger("ReportWebSite");
			lognet.Debug("--------------- FreeCycle CPDeleteOrendar_Callback ----------------", ex);
			throw ex;
		}


	}

	protected void CPDeleteArhivCard_Callback(object sender, CallbackEventArgsBase e)
	{
		var freecycle_id = Int32.Parse(e.Parameter);

		SqlConnection connection = null;
		SqlTransaction transaction = null;
		try
		{
			connection = Utils.ConnectToDatabase();
			transaction = connection.BeginTransaction();

			var username = Utils.GetUser();
			var modify_date = DateTime.Now;

			{
				using (var cmd = new SqlCommand(
					@"update [freecycle] set
							[is_deleted]				= 1,
							[modify_date]				= @modify_date,
							[modified_by]				= @modified_by
						where 1=1
							and [freecycle_id]			= @freecycle_id", connection, transaction))
				{
					cmd.Parameters.Add(GetSqlParameter("freecycle_id", freecycle_id));
					cmd.Parameters.Add(GetSqlParameter("modify_date", modify_date));
					cmd.Parameters.Add(GetSqlParameter("modified_by", username));
					var rowUpdated = cmd.ExecuteNonQuery();
					if (rowUpdated != 1) throw new Exception("FreeCycle update error");
				}
			}

			FreeCycleUtils.UpdateStageInfo(connection, transaction, free_square_id);

			transaction.Commit();
			connection.Close();
		}
		catch (Exception ex)
		{
			if (transaction != null)
			{
				transaction.Rollback();
			}
			if (connection != null)
			{
				connection.Close();
			}

			var lognet = log4net.LogManager.GetLogger("ReportWebSite");
			lognet.Debug("--------------- FreeCycle CPDeleteArhivCard_Callback ----------------", ex);
			throw ex;
		}


	}

	protected void CPAddExistingOrendar_Callback(object sender, CallbackEventArgsBase e)
	{
		var org_orendar_id = Int32.Parse(e.Parameter);

		SqlConnection connection = null;
		try
		{
			connection = Utils.ConnectToDatabase();
			FreeCycleUtils.AddOrendar(connection, freecycle_id, org_orendar_id);
			connection.Close();
		}
		catch (Exception ex)
		{
			if (connection != null)
			{
				connection.Close();
			}

			var lognet = log4net.LogManager.GetLogger("ReportWebSite");
			lognet.Debug("--------------- FreeCycle CPAddExistingOrendar_Callback ----------------", ex);
			throw ex;
		}
	}

	protected void CPMoveToArhiv_Callback(object sender, CallbackEventArgsBase e)
	{
		SqlConnection connection = null;
		SqlTransaction transaction = null;
		try
		{
			connection = Utils.ConnectToDatabase();
			transaction = connection.BeginTransaction();

			FreeCycleUtils.AddCycle(connection, transaction, free_square_id);

			FreeCycleUtils.UpdateStageInfo(connection, transaction, free_square_id);

			transaction.Commit();
			connection.Close();
		}
		catch (Exception ex)
		{
			if (transaction != null)
			{
				transaction.Rollback();
			}
			if (connection != null)
			{
				connection.Close();
			}

			var lognet = log4net.LogManager.GetLogger("ReportWebSite");
			lognet.Debug("--------------- FreeCycle CPMoveToArhiv_Callback ----------------", ex);
			throw ex;
		}


	}

	protected void ComboRenterOrg_Callback(object sender, CallbackEventArgsBase e)
	{
		if (e.Parameter.StartsWith("refresh"))
		{
			var bb = ((ASPxPopupControl)sender);
			bb.DataBind();
		}
	}


	string GetString(SqlDataReader reader, string column)
	{
		var value = reader[column];
		if (value is DBNull)
		{
			return null;
		}
		else
		{
			return (string)value;
		}
	}

	DateTime? GetDateTime(SqlDataReader reader, string column)
	{
		var value = reader[column];
		if (value is DBNull)
		{
			return null;
		}
		else
		{
			return (DateTime)value;
		}
	}

	Decimal? GetDecimal(SqlDataReader reader, string column)
	{
		var value = reader[column];
		if (value is DBNull)
		{
			return null;
		}
		else
		{
			return (Decimal)value;
		}
	}

	Int32? GetInteger(SqlDataReader reader, string column)
	{
		var value = reader[column];
		if (value is DBNull)
		{
			return null;
		}
		else
		{
			return (Int32)value;
		}
	}


	DateTime? GetASPxDateEditValue(ASPxDateEdit aspxDateEdit)
	{
		if (aspxDateEdit == null) return (DateTime?)null;
		return (aspxDateEdit.Date == default(DateTime) ? (DateTime?)null : aspxDateEdit.Date);
	}

	Decimal? GetASPxSpinEditValue(ASPxSpinEdit aspxSpinEdit)
	{
		if (aspxSpinEdit == null) return (Decimal?)null;
		if (aspxSpinEdit.Value == null) return (Decimal?)null;
		return aspxSpinEdit.Number;
	}

	String GetASPxTextBoxValue(ASPxTextBox aspxEdit)
	{
		if (aspxEdit == null) return "";
		return (aspxEdit.Text ?? "");
	}

	Int32? GetASPxComboBoxValue(ASPxComboBox aspxComboBox)
	{
		if (aspxComboBox == null) return (Int32?)null;
		if (aspxComboBox.Value == null) return (Int32?)null;
		return Int32.Parse(aspxComboBox.Value.ToString());
	}


	SqlParameter GetSqlParameter(string parameterName, object value)
	{
		if (value == null)
		{
			return new SqlParameter(parameterName, DBNull.Value);
		}
		else
		{
			return new SqlParameter(parameterName, value);
		}
	}

	protected void ObjectDataSourcePhotoFiles_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
	{
		if (Request.Cookies["RecordID"] != null)
		{
			e.InputParameters["RecordID"] = Request.Cookies["RecordID"].Value;
		}
	}

	protected void SqlDataSourceRenter_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
	{
		if (Request.Cookies["RecordID"] != null)
		{
			var freecycle_orendar_id = Int32.Parse(Request.Cookies["RecordID"].Value);
			e.Command.Parameters["@freecycle_orendar_id"].Value = freecycle_orendar_id;

			//PopupEditRenterOrg.ClientInstanceName

		}
	}


	protected void SqlDataSourceArhivCycles_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
	{
		e.Command.Parameters["@free_square_id"].Value = free_square_id;
	}

	protected void SqlDataSourceArhivCycles_Selected(object sender, SqlDataSourceStatusEventArgs e)
	{
		CardPageControl.TabPages[1].Text = "Архів (" + e.AffectedRows + ")";
	}


	protected void CPArhivCycles_Callback(object sender, CallbackEventArgsBase e)
	{
		if (e.Parameter.StartsWith("refresh:"))
		{
		}
	}

	int GetRowSpecMode(freecycle_step_dict row)
	{
		if ((new[] { "БК_00", "КК_00" }).Contains(row.step_cod))
		{
			return 10;
		}
		else
		{
			return 0;
		}
	}

	string OnDateChangedClientSideEvent()
	{
		return "function (s,e) { has_form_change = true; }";
	}

}