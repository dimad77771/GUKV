﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data;
using System.Data.SqlClient;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Security.Cryptography;
using System.Text;
using DevExpress.Web;
using System.IO;
using Syncfusion.Pdf;
using System.Web.Configuration;
using Syncfusion.Pdf.Graphics;
using System.Drawing;
using Syncfusion.Pdf.Parsing;
using Syncfusion.DocToPDFConverter;
using Syncfusion.DocIO.DLS;
using ExtDataEntry.Models;
using DevExpress.Compression;

using Syncfusion.XlsIO;
using WP = DocumentFormat.OpenXml.Wordprocessing;
using System.Data.Common;

public partial class Reports1NF_Cabinet : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
		var id = Int32.Parse(Request.QueryString["id"]);

		new Report1NFFreeSquareZvit2()
		{
			Page = Page,
			ID = id
		}.Run();
	}


	public class Report1NFFreeSquareZvit2
	{
		public Page Page;
		public int ID;
		public bool IsOgoloshena;

		public void Run()
		{
			string templateFileName = Page.Server.MapPath("Templates/" + "Лист_щодо_продовження.docx");

			if (templateFileName.Length > 0)
			{
				using (TempFile tempFile = TempFile.FromExistingFile(templateFileName))
				{
					using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(tempFile.FileName, true))
					{
						MainDocumentPart mainPart = wordDocument.MainDocumentPart;

						if (mainPart != null)
						{
							UpdateTemplateFile(mainPart);
						}

						//SaltedHash sh = new SaltedHash("123");
						//DocumentSettingsPart docSett = wordDocument.MainDocumentPart.DocumentSettingsPart;
						//DocumentProtection documentProtection = new DocumentProtection();
						//documentProtection.Edit = DocumentProtectionValues.ReadOnly;
						//OnOffValue docProtection = new OnOffValue(true);
						//documentProtection.Enforcement = docProtection;

						//documentProtection.CryptographicAlgorithmClass = CryptAlgorithmClassValues.Hash;
						//documentProtection.CryptographicProviderType = CryptProviderValues.RsaFull;
						//documentProtection.CryptographicAlgorithmType = CryptAlgorithmValues.TypeAny;
						//documentProtection.CryptographicAlgorithmSid = 4; // SHA1
						//												  //    The iteration count is unsigned
						//												  //UInt32Value uintVal = new UInt32Value();
						//												  //uintVal.Value = (uint)123;
						//documentProtection.CryptographicSpinCount = 1;
						//documentProtection.Hash = sh.Hash;
						//documentProtection.Salt = sh.Salt;
						//wordDocument.MainDocumentPart.DocumentSettingsPart.Settings.AppendChild(documentProtection);
						//wordDocument.MainDocumentPart.DocumentSettingsPart.Settings.Save();

						wordDocument.Close();

						// Dump the document contents to the output stream
						System.IO.FileInfo info = new System.IO.FileInfo(tempFile.FileName);

						var outfile = "Лист_щодо_продовження.docx";
						Page.Response.Clear();
						Page.Response.ClearHeaders();
						Page.Response.ClearContent();
						Page.Response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
						Page.Response.AddHeader("content-disposition", "attachment; filename=" + outfile + "; size=" + info.Length.ToString());

						// Pipe the stream contents to the output stream
						using (var stream = File.Open(tempFile.FileName, FileMode.Open, FileAccess.ReadWrite))
						{
							stream.CopyTo(Page.Response.OutputStream);
						}
					}

					Page.Response.End();
				}
			}
		}

		void UpdateTemplateFile(MainDocumentPart mainPart)
		{
			Dictionary<string, object> properties = new Dictionary<string, object>();

			SqlConnection connection = Utils.ConnectToDatabase();

			if (connection != null)
			{
				GetData(connection, properties);
				connection.Close();
			}

			foreach (KeyValuePair<string, object> pair in properties)
			{
				var value = pair.Value == null ? "" : pair.Value.ToString();
				ReplaceDocTagInElements(mainPart, pair.Key, value);
			}
		}

		void GetData(SqlConnection connection, Dictionary<string, object> properties)
		{
			DateTime dtNow = DateTime.Now;
			string currentDate = "\xAB" + " " + dtNow.Day.ToString() + " " + "\xBB" + " " + GetDateMonthName(dtNow) + " " + dtNow.Year.ToString();
			properties.Add("{REPORT_PRINT_DATE}", currentDate);

			var factory = DbProviderFactories.GetFactory(connection);
			var dataTable = new DataTable();
			using (var cmd = factory.CreateCommand())
			{
				cmd.CommandText =
					@"
SELECT 
fs.id,
	fs.komis_protocol,
    fs.prozoro_number,
	fs.using_possible_id,
	fs.geodata_map_points,
	fs.include_in_perelik,
	fs.current_stage_id,
	fs.freecycle_step_dict_id,
	fs.current_stage_docdate,
	fs.current_stage_docnum,
	fs.modify_date2,
	fs.modified_by2,
    fs.zal_balans_vartist,
    fs.perv_balans_vartist,
    fs.punkt_metod_rozrahunok,
    fs.prop_srok_orands,
    fs.nomer_derzh_reestr_neruh,
    fs.reenum_derzh_reestr_neruh,
    fs.info_priznach_nouse,
    fs.info_rahunok_postach,
    fs.priznach_before,
    fs.orend_plat_borg,
    fs.period_nouse,
    fs.stanom_na,
    fs.osoba_use_before,
 row_number() over (order by org.short_name, b.street_full_name, b.addr_nomer, fs.total_free_sqr) as npp     
,fs.arenda_id
,org.short_name as org_name
,org.zkpo_code
,org.report_id
,org.director_title as vidpov_osoba
,(select Q.name from dict_streets Q where Q.id = org.addr_street_id) as balanutr_addr_street
,org.addr_nomer as balanutr_addr_nomer

,b.district
,b.street_full_name as street_name
,(COALESCE(LTRIM(RTRIM(b.addr_nomer1)) + ' ', '') + COALESCE(LTRIM(RTRIM(b.addr_nomer2)) + ' ', '') + COALESCE(LTRIM(RTRIM(b.addr_nomer3)), '')) as addr_nomer

,(select q.name from dict_1nf_tech_stane q where q.id = fs.free_sqr_condition_id) as condition 
,b.object_type 
,b.object_kind
,b.sqr_total
,b.sqr_for_rent

,fs.total_free_sqr 
--,null as free_sql_usefull
--,null as mzk
--,rfs.sqr_free_korysna as free_sql_usefull
--,rfs.sqr_free_mzk as mzk
,fs.free_sqr_korysna as free_sql_usefull

,fs.is_included
,fs.floor
,fs.water
,fs.heating
,(select q.name from dict_1nf_power_info q where q.id = fs.power_info_id) as power_text
,fs.gas

--,(select left(qq.full_name, 150) as name from view_dict_rental_rate qq where qq.id = fs.using_possible_id) as possible_using
,fs.possible_using

,(select qq.name from dict_free_object_type qq where qq.id = fs.free_object_type_id) as free_object_type_name


,fs.modify_date
,fs.modified_by
,fs.note

,invest_solution = (select qq.name from dict_1nf_invest_solution qq where qq.id = fs.invest_solution_id)
--, solution = fs.is_solution

, fs.initiator
, zg2.name as zgoda_control
, zg.name as zgoda_renter

,st.kind
,rep.form_of_ownership
,rep.old_organ

--,b.object_kind as vydbudynku
,history = case when isnull(b.history, 'НІ') = 'НІ' then '' else 'ТАК' end 
, isnull(ddd.name, 'Невизначені') as sf_upr
, case when exists (select 1 from reports1nf_arenda_dogcontinue_photos qq where qq.free_square_id = fs.id) then 1 else 0 end as isexistsphoto

,org_renter.zkpo_code as orendar_zkpo
,org_renter.full_name as orendar_name

,org_giver.zkpo_code as giver_zkpo
,org_giver.full_name as giver_name
,(select Q.name from dict_streets Q where Q.id = org_giver.addr_street_id) as giver_addr_street
,org_giver.addr_nomer as giver_addr_nomer

,bal.agreement_num
,bal.agreement_date
,bal.rent_finish_date
,cast(round(DATEDIFF ( month, bal.agreement_date, bal.rent_finish_date ) / 12.0, 0) as int) as srok_dog

,fs.orend_plat_last_month
,fs.has_perevazh_pravo
,fs.polipshanya_vartist
,fs.polipshanya_finish_date
,fs.primitki
,fs.zalbalansvartist_date
,fs.osoba_oznakoml
,fs.rozmir_vidshkoduv

FROM view_reports1nf rep
join reports1nf_arenda bal on bal.report_id = rep.report_id
JOIN view_reports1nf_buildings b ON b.unique_id = bal.building_1nf_unique_id
join dbo.reports1nf_arenda_dogcontinue fs on fs.arenda_id = bal.id and fs.report_id = rep.report_id
--left join (select * from dbo.reports1nf_arenda_dogcontinue where id = (select top 1 id from dbo.reports1nf_arenda_dogcontinue where arenda_id = bal.id)) fs on fs.arenda_id = bal.id
join reports1nf_org_info org on org.id = bal.org_balans_id
left join [dbo].[dict_streets] st on b.addr_street_id = st.id
left join dbo.dict_zgoda_renter zg on fs.zgoda_renter_id = zg.id
left join dbo.dict_zgoda_renter zg2 on fs.zgoda_control_id = zg2.id
left join organizations org_renter on org_renter.id = bal.org_renter_id
left outer join organizations org_giver ON org_giver.id = bal.org_giver_id and (org_giver.is_deleted is null or org_giver.is_deleted = 0)

--OUTER APPLY (SELECT TOP 1 * FROM rent_free_square rfs
--		WHERE rfs.building_id = bal.building_id AND
--		      rfs.organization_id = bal.organization_id order by rfs.rent_period_id DESC) rfs

LEFT JOIN (
			select obp.org_id
			, occ.name
			, occ.id
			, per.name as period 
			from org_by_period obp
			join dict_rent_period per on per.id = obp.period_id and per.is_active = 1
			join dict_rent_occupation occ on occ.id = obp.org_occupation_id
				) DDD ON DDD.org_id = rep.organization_id

        WHERE fs.id = " + ID;

				cmd.CommandType = CommandType.Text;
				cmd.Connection = connection;
				using (var adapter = factory.CreateDataAdapter())
				{
					adapter.SelectCommand = cmd;
					adapter.Fill(dataTable);
				}
			}
			var r = dataTable.Rows[0];

			properties.Add("{Дата укладання договору}", GetDate(r["agreement_date"]));
			properties.Add("{Номер договору}", r["agreement_num"]);
			properties.Add("{Загальна площа об’єкта}", "" + GetDecimal(r["total_free_sqr"]));
			properties.Add("{Найменування орендаря}", r["orendar_name"]);
			properties.Add("{Строк оренди (роки)}", StrokOrenda(r["srok_dog"]));
			properties.Add("{Адреса балансоутримувача (вулиця)}", r["balanutr_addr_street"]);
			properties.Add("{Адреса балансоутримувача (номер дому)}", r["balanutr_addr_nomer"]);
		}

		string StrokOrenda(object arg)
		{
			if (arg is DBNull) return "";

			var srok = (int)arg;

			if (srok == 1) return "" + srok + " рік";
			else if (srok == 2 || srok == 3 || srok == 4) return "" + srok + " ріки";
			else return "" + srok + " років";
		}

		string nayavn(object arg)
		{
			if (arg == null) return "";
			else if (arg.ToString().ToLower() == "Так") return "в наявності";
			else if (arg.ToString().ToLower() == "Ні") return "відсутнє";
			else return arg.ToString().Trim();
		}

		string p(string prefix, object arg)
		{
			var text = (arg == null ? "" : arg.ToString()).Trim();
			return (prefix + text).Trim();
		}

		string j(string del, params object[] arg)
		{
			return string.Join(del, arg.Select(q => q == null ? "" : q.ToString()).Where(q => !string.IsNullOrEmpty(q)));
		}

		string GetDecimal(object arg, string format = "0.00")
		{
			return arg is DBNull ? "" : ((decimal)arg).ToString(format);
		}

		string GetDate(object arg)
		{
			return arg is DBNull ? "" : ((DateTime)arg).ToString("dd.MM.yyyy");
		}

		void ReplaceDocTagInElements(MainDocumentPart mainPart, string tag, string replacement)
		{
			List<OpenXmlElement> elements = mainPart.Document.Body.OfType<OpenXmlElement>().ToList();

			foreach (OpenXmlElement element in elements)
			{
				if (element is WP.Paragraph)
				{
					ReplaceDocTag(mainPart, element as WP.Paragraph, tag, replacement);
				}
				else if (element is DocumentFormat.OpenXml.Wordprocessing.Table)
				{
					List<DocumentFormat.OpenXml.Wordprocessing.TableRow> rows = element.ChildElements.OfType<DocumentFormat.OpenXml.Wordprocessing.TableRow>().ToList();

					foreach (DocumentFormat.OpenXml.Wordprocessing.TableRow row in rows)
					{
						List<DocumentFormat.OpenXml.Wordprocessing.TableCell> cells = row.ChildElements.OfType<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ToList();

						foreach (DocumentFormat.OpenXml.Wordprocessing.TableCell cell in cells)
						{
							foreach (OpenXmlElement e in cell.ChildElements)
							{
								if (e is WP.Paragraph)
								{
									ReplaceDocTag(mainPart, e as WP.Paragraph, tag, replacement);
								}
							}
						}
					}
				}
			}
		}

		void ReplaceDocTag(MainDocumentPart mainPart, WP.Paragraph para, string tag, string replacement)
		{
			string paragraphText = GetParagraphText(para);

			// Replace the tag
			bool replaced = false;
			int pos = paragraphText.IndexOf(tag);

			while (pos >= 0)
			{
				replaced = true;

				paragraphText = paragraphText.Replace(tag, replacement);

				// Search once again
				pos = paragraphText.IndexOf(tag);
			}

			if (replaced)
			{
				WP.Run firstRun = para.OfType<WP.Run>().First<WP.Run>();

				if (firstRun == null)
				{
					firstRun = new WP.Run();
				}

				// Delete all paragraph sub-items
				para.RemoveAllChildren<WP.Run>();

				// Add the text to the first Run
				firstRun.RemoveAllChildren<WP.Text>();
				firstRun.AppendChild(new WP.Text(paragraphText));

				// Create a new run with the modified text
				para.AppendChild(firstRun);
			}
		}

		string GetParagraphText(WP.Paragraph para)
		{
			string paragraphText = "";

			List<WP.Run> runs = para.OfType<WP.Run>().ToList();

			foreach (WP.Run run in runs)
			{
				List<WP.Text> texts = run.OfType<WP.Text>().ToList();

				foreach (WP.Text text in texts)
				{
					paragraphText += text.Text;
				}
			}

			return paragraphText;
		}

		string GetDateMonthName(object date)
		{
			if (date is DateTime)
			{
				switch (((DateTime)date).Month)
				{
					case 1:
						return Resources.Strings.Month1;

					case 2:
						return Resources.Strings.Month2;

					case 3:
						return Resources.Strings.Month3;

					case 4:
						return Resources.Strings.Month4;

					case 5:
						return Resources.Strings.Month5;

					case 6:
						return Resources.Strings.Month6;

					case 7:
						return Resources.Strings.Month7;

					case 8:
						return Resources.Strings.Month8;

					case 9:
						return Resources.Strings.Month9;

					case 10:
						return Resources.Strings.Month10;

					case 11:
						return Resources.Strings.Month11;

					case 12:
						return Resources.Strings.Month12;
				}
			}

			return "";
		}

	}





}