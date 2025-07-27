using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.UI;
using DevExpress.Web;
using Newtonsoft.Json;
using StackExchange.Profiling.Helpers.Dapper;
using System.Data;
using System.Data.Common;

public class NarazhCalculationAll
{
	public void Run()
	{
		var sql = @"select
r.id, r.report_id
FROM reports1nf_arenda r 
LEFT JOIN arenda a ON r.id = a.id 
WHERE 1=1
and isnull(a.is_deleted, 0) = 0
and exists 
(
	select	
		* 
	from reports1nf_arenda_payments Q 
	where Q.arenda_id = r.id and Q.report_id = r.report_id and Q.rent_period_id = (SELECT QQ.id FROM dict_rent_period QQ where QQ.is_active = 1)
)
--and r.agreement_state = 1
";


		//var reportID = 1594;
		//var arenda_id = 84257;

		var tm1 = DateTime.Now;

		var connection = Utils.ConnectToDatabase();
		var table = Utils.GetSqlDataTable(sql, connection);

		{
			connection.Execute("exec update_inflation_prognoz");

			connection.Execute("delete from reports1nf_payment_narah_prognoz");

			for (int row = 0; row < table.Rows.Count; row++)
			{
				System.Diagnostics.Debug.WriteLine("row =" + row);

				var arenda_id = (int)table.Rows[row]["id"];
				var reportID = (int)table.Rows[row]["report_id"];

				var robject = new NarazhCalculationMain
				{
					report_id = reportID,
					arenda_id = arenda_id,
					IsDBMode = true,
					UseInflationPrognoz = true,
					UsePaymentDiscountsFuture = true,
					CalcYears = 4,
					connection = connection,
				};
				var result = robject.Main();

				var allValues = result.AllValues;

				if (allValues != null)
				{
					foreach (var year in allValues.Keys)
					{
						foreach (var month in allValues[year].Keys)
						{
							var infation = allValues[year][month];
							using (SqlCommand cmd = new SqlCommand(@"
	insert into reports1nf_payment_narah_prognoz(report_id,arenda_id,narah_date,narah_sum)
	values(@report_id,@arenda_id,@narah_date,@narah_sum)", connection))
							{
								cmd.Parameters.Add(new SqlParameter("report_id", reportID));
								cmd.Parameters.Add(new SqlParameter("arenda_id", arenda_id));
								cmd.Parameters.Add(new SqlParameter("narah_date", new DateTime(year, month, 1)));
								cmd.Parameters.Add(new SqlParameter("narah_sum", infation));
								cmd.ExecuteNonQuery();
							}
						}
					}
				}
			}

			var tm2 = DateTime.Now;
			var dd = (tm2 - tm1).TotalMilliseconds;
			System.Diagnostics.Debug.WriteLine("dd=" + dd);
		}
	}

}

public class NarazhCalculationMain
{
	public int arenda_id;
	public int report_id;
	public bool IsDBMode;
	public bool UseInflationPrognoz;
	public bool UsePaymentDiscountsFuture;

	public bool IsNextYear;
	public int? CalcYears;
	public Dictionary<string, Control> controls;
	public ASPxGridView GridViewNotes;
	public int LastYear;

	public SqlConnection connection;
	List<NarazhCalculationOne.DogchangeClass> Dogchanges = new List<NarazhCalculationOne.DogchangeClass>();

	public NarazhCalculationOne.ResultClass Main()
	{
		if (connection == null)
		{
			connection = Utils.ConnectToDatabase();
		}

		LoadDogchanges();

		NarazhCalculationOne.ResultClass result = new NarazhCalculationOne.ResultClass();

		for (int dogchangeNum = -1; dogchangeNum < Dogchanges.Count; dogchangeNum++)
		{
			var robject = new NarazhCalculationOne
			{
				report_id = report_id,
				arenda_id = arenda_id,
				IsDBMode = IsDBMode,
				UseInflationPrognoz = UseInflationPrognoz,
				UsePaymentDiscountsFuture = UsePaymentDiscountsFuture,
				CalcYears = CalcYears,

				controls = controls,
				GridViewNotes = GridViewNotes,
				Dogchanges = Dogchanges,
				DogchangeNum = dogchangeNum,
				IsNextYear = IsNextYear,
				Connection = connection,
			};

			var res = robject.Run();
			this.LastYear = robject.LastYear;

			result.Add(res);

			if (dogchangeNum == -1)
			{
				//return res;
			}
		}

		return result;
	}

	void LoadDogchanges()
	{
		var rows = NarazhCalculationOne.GetSqlDataTable("select * from reports1nf_arenda_dogchange where arenda_id = " + arenda_id + " and report_id = " + report_id, connection);
		for (var rownum = 0; rownum < rows.Rows.Count; rownum++)
		{
			Dogchanges.Add(new NarazhCalculationOne.DogchangeClass
			{
				rent_start_date = rows.GetDateTimeFromSqlDataTable(rownum, "rent_start_date"),
				rent_actual_finish_date = rows.GetDateTimeFromSqlDataTable(rownum, "rent_actual_finish_date"),
				base_month = rows.GetDateTimeFromSqlDataTable(rownum, "base_month"),
				rent_rate = rows.GetDecimalFromSqlDataTable(rownum, "rent_rate"),
				invnum_rent = rows.GetStringFromSqlDataTable(rownum, "invnum_rent"),
			});
		}
	}


}

public class NarazhCalculationOne
{
	public int arenda_id;
	public int report_id;
	public bool IsDBMode;
	public bool UseInflationPrognoz;
	public bool UsePaymentDiscountsFuture;

	public int DogchangeNum;
	public bool IsNextYear;
	public int? CalcYears;
	public List<DogchangeClass> Dogchanges;
	public Dictionary<string, Control> controls;
	public ASPxGridView GridViewNotes;
	public SqlConnection Connection;

	public int LastYear;
	public int FirstYear;
	int LastMonth;
	Dictionary<int, decimal> InflationYearData = new Dictionary<int, decimal>();
	Dictionary<DateTime, decimal> InflationMonthData = new Dictionary<DateTime, decimal>();
	Dictionary<int, NotesDataClass> NotesData = new Dictionary<int, NotesDataClass>();
	List<ZnizhkaClass> ZnizhkaData = new List<ZnizhkaClass>();
	string methodCalc;
	DateTime baseMonth;
	DateTime rentStart;
	DateTime rentFinish;

	int ActiveRentPeriodId;


	bool IsDogchange
	{
		get
		{
			return DogchangeNum >= 0;
		}
	}

	DogchangeClass Dogchange
	{
		get
		{
			return IsDogchange ? Dogchanges[DogchangeNum] : null;
		}
	}

	public class NotesDataClass
	{
		public int id;
		public string invent_no;
		public decimal cost_agreement;
		public Dictionary<DateTime, decimal> MonthPlataBase = new Dictionary<DateTime, decimal>();
	}

	public class ZnizhkaClass
	{
		public decimal percent;
		public DateTime date1;
		public DateTime date2;
		public string invnum;
		public bool furure;
	}

	public class DogchangeClass
	{
		public DateTime? rent_start_date;
		public DateTime? rent_actual_finish_date;
		public DateTime? base_month;
		public Decimal? rent_rate;
		public string invnum_rent;
	}


	public ResultClass Run()
	{
		DateTime? baseMonthControl, rentStartControl, rentFinishControl;
		int? rent_period_id;
		string paymentType;

		SetupActiveRentPeriodId();

		if (IsDBMode)
		{
			var rows = NarazhCalculationOne.GetSqlDataTable(@"select 
(SELECT Q.name FROM dict_method_calc Q where Q.id = A.method_calc_id) as method_calc, 
base_month, rent_start_date, rent_actual_finish_date, 
(SELECT Q.name FROM dict_arenda_payment_type Q where Q.id = A.payment_type_id) as payment_type 
 from reports1nf_arenda A where id = " + arenda_id + " and report_id = " + report_id, Connection);

			methodCalc = rows.GetStringFromSqlDataTable(0, "method_calc");
			baseMonthControl = rows.GetDateTimeFromSqlDataTable(0, "base_month");
			rentStartControl = rows.GetDateTimeFromSqlDataTable(0, "rent_start_date");
			rentFinishControl = rows.GetDateTimeFromSqlDataTable(0, "rent_actual_finish_date");
			rent_period_id = ActiveRentPeriodId;
			paymentType = rows.GetStringFromSqlDataTable(0, "payment_type");
		}
		else
		{
			methodCalc = Reports1NFUtils.GetDropDownText(controls, "EditMethodCalc");
			baseMonthControl = Reports1NFUtils.GetDateValue(controls, "EditBaseMonth") as DateTime?;
			rentStartControl = Reports1NFUtils.GetDateValue(controls, "EditStartDate") as DateTime?;
			rentFinishControl = Reports1NFUtils.GetDateValue(controls, "EditActualFinishDate") as DateTime?;
			rent_period_id = Reports1NFUtils.GetDropDownValue(controls, "ReportingPeriodCombo") as int?;
			paymentType = Reports1NFUtils.GetDropDownText(controls, "ComboPaymentType");
		}

		if (!(paymentType == "ГРОШОВА ОПЛАТА" || paymentType == "ПОГОДИННО"))
		{
			return ReturnEmpty();
		}
		if (string.IsNullOrEmpty(methodCalc))
		{
			return ReturnEmpty();
		}
		if (baseMonthControl == null)
		{
			return ReturnEmpty();
		}
		if (rentStartControl == null)
		{
			return ReturnEmpty();
		}
		if (rent_period_id == null)
		{
			return ReturnEmpty();
		}

		BuildRentPeriodInfo(rent_period_id.Value);
		baseMonth = new DateTime(baseMonthControl.Value.Year, baseMonthControl.Value.Month, 1);
		rentStart = rentStartControl.Value;
		rentFinish = rentFinishControl != null ? rentFinishControl.Value : new DateTime(LastYear + 1, 1, 1).AddDays(-1);
		CorrectByDogchange();
		CreateInflationData();
		BuildNotes();
		BuildZnizhka();
		CalcMonthPlata();

		var allplata = CalcRealPlata();
		var plata = allplata[LastYear];

		var total = plata.Where(x => x.Key >= 1 && x.Key <= LastMonth).Sum(x => x.Value);
		var result = new ResultClass
		{
			NarazhCalculation_1 = plata[1],
			NarazhCalculation_2 = plata[2],
			NarazhCalculation_3 = plata[3],
			NarazhCalculation_4 = plata[4],
			NarazhCalculation_5 = plata[5],
			NarazhCalculation_6 = plata[6],
			NarazhCalculation_7 = plata[7],
			NarazhCalculation_8 = plata[8],
			NarazhCalculation_9 = plata[9],
			NarazhCalculation_10 = plata[10],
			NarazhCalculation_11 = plata[11],
			NarazhCalculation_12 = plata[12],
			NarazhCalculation_all = total,
			AllValues = allplata,
		};

		return result;
	}

	void SetupActiveRentPeriodId()
	{
		var rows = GetDataTable("SELECT QQ.id FROM dict_rent_period QQ where QQ.is_active = 1");
		ActiveRentPeriodId = (int)rows.GetIntFromSqlDataTable(0, "id");
	}

	void CorrectByDogchange()
	{
		if (!IsDogchange) return;

		if (Dogchange.base_month == null) throw new Exception("Не заповнений базовий місяць");
		if (Dogchange.base_month.Value.Day != 1) throw new Exception("Базовий місяць не перше число місяця");
		if (Dogchange.rent_start_date == null) throw new Exception("Не заповнена дата початку використання приміщення");

		baseMonth = Dogchange.base_month.Value;
		rentStart = Dogchange.rent_start_date.Value;
		rentFinish = Dogchange.rent_actual_finish_date != null ? Dogchange.rent_actual_finish_date.Value : rentFinish;
	}



	public class ResultClass
	{
		public decimal? NarazhCalculation_1 { get; set; }
		public decimal? NarazhCalculation_2 { get; set; }
		public decimal? NarazhCalculation_3 { get; set; }
		public decimal? NarazhCalculation_4 { get; set; }
		public decimal? NarazhCalculation_5 { get; set; }
		public decimal? NarazhCalculation_6 { get; set; }
		public decimal? NarazhCalculation_7 { get; set; }
		public decimal? NarazhCalculation_8 { get; set; }
		public decimal? NarazhCalculation_9 { get; set; }
		public decimal? NarazhCalculation_10 { get; set; }
		public decimal? NarazhCalculation_11 { get; set; }
		public decimal? NarazhCalculation_12 { get; set; }
		public decimal? NarazhCalculation_all { get; set; }

		[JsonIgnore]
		public Dictionary<int, Dictionary<int, decimal>> AllValues { get; set; }

		public void Add(ResultClass addvalue)
		{
			if (addvalue.NarazhCalculation_1.HasValue) NarazhCalculation_1 = (NarazhCalculation_1 ?? 0) + addvalue.NarazhCalculation_1.Value;
			if (addvalue.NarazhCalculation_2.HasValue) NarazhCalculation_2 = (NarazhCalculation_2 ?? 0) + addvalue.NarazhCalculation_2.Value;
			if (addvalue.NarazhCalculation_3.HasValue) NarazhCalculation_3 = (NarazhCalculation_3 ?? 0) + addvalue.NarazhCalculation_3.Value;
			if (addvalue.NarazhCalculation_4.HasValue) NarazhCalculation_4 = (NarazhCalculation_4 ?? 0) + addvalue.NarazhCalculation_4.Value;
			if (addvalue.NarazhCalculation_5.HasValue) NarazhCalculation_5 = (NarazhCalculation_5 ?? 0) + addvalue.NarazhCalculation_5.Value;
			if (addvalue.NarazhCalculation_6.HasValue) NarazhCalculation_6 = (NarazhCalculation_6 ?? 0) + addvalue.NarazhCalculation_6.Value;
			if (addvalue.NarazhCalculation_7.HasValue) NarazhCalculation_7 = (NarazhCalculation_7 ?? 0) + addvalue.NarazhCalculation_7.Value;
			if (addvalue.NarazhCalculation_8.HasValue) NarazhCalculation_8 = (NarazhCalculation_8 ?? 0) + addvalue.NarazhCalculation_8.Value;
			if (addvalue.NarazhCalculation_9.HasValue) NarazhCalculation_9 = (NarazhCalculation_9 ?? 0) + addvalue.NarazhCalculation_9.Value;
			if (addvalue.NarazhCalculation_10.HasValue) NarazhCalculation_10 = (NarazhCalculation_10 ?? 0) + addvalue.NarazhCalculation_10.Value;
			if (addvalue.NarazhCalculation_11.HasValue) NarazhCalculation_11 = (NarazhCalculation_11 ?? 0) + addvalue.NarazhCalculation_11.Value;
			if (addvalue.NarazhCalculation_12.HasValue) NarazhCalculation_12 = (NarazhCalculation_12 ?? 0) + addvalue.NarazhCalculation_12.Value;
			if (addvalue.NarazhCalculation_all.HasValue) NarazhCalculation_all = (NarazhCalculation_all ?? 0) + addvalue.NarazhCalculation_all.Value;

			if (AllValues == null)
			{
				AllValues = addvalue.AllValues;
			}
			else
			{
				foreach (int year in addvalue.AllValues.Keys)
				{
					for (int month = 1; month <= 12; month++)
					{
						decimal value1 = 0;
						if (AllValues.ContainsKey(year))
						{
							AllValues[year].TryGetValue(month, out value1);
						}

						decimal value2 = 0;
						addvalue.AllValues[year].TryGetValue(month, out value2);

						var sum = value1 + value2;

						if (AllValues[year].ContainsKey(month))
						{
							AllValues[year][month] = sum;
						}
						else
						{
							AllValues[year].Add(month, sum);
						}
					}
				}
			}
		}
	}

	public class ResultTotalClass
	{
		public int Year { get; set; }
		public ResultClass CurrentYear { get; set; }
		public ResultClass NextYear { get; set; }
	}

	Dictionary<int, Dictionary<int, decimal>> CalcRealPlata()
	{
		var allresult = new Dictionary<int, Dictionary<int, decimal>>();

		for (int year = FirstYear; year <= LastYear; year++)
		{
			var finalPlataByMonth = new Dictionary<int, decimal>();
			for (int month = 1; month <= 12; month++)
			{
				finalPlataByMonth.Add(month, 0);
			}


			foreach (var note in NotesData)
			{
				var invnum = note.Value.invent_no;
				for (int month = 1; month <= 12; month++)
				{
					var plataInfo = new Dictionary<DateTime, decimal>();
					var daysInMonth = DateTime.DaysInMonth(year, month);
					var baseMonthPlata = default(decimal);
					var datekey = new DateTime(year, month, 1);
					note.Value.MonthPlataBase.TryGetValue(datekey, out baseMonthPlata);
					var baseDayPlata = baseMonthPlata / daysInMonth;
					for (int day = 1; day <= daysInMonth; day++)
					{
						var date = new DateTime(year, month, day);
						if (IsDateValid(date))
						{
							var plata = baseDayPlata;

							var znizhkaList = ZnizhkaData.Where(x => (x.invnum == "" || x.invnum == invnum) && date >= x.date1 && date <= x.date2).ToArray();
							if (znizhkaList.Any())
							{
								var znizhkaPercent = 1.0M;
								foreach (var znizhkaOne in znizhkaList)
								{
									if (znizhkaOne.percent > 100.0M) znizhkaOne.percent = 100.0M;
									znizhkaPercent *= (1 - znizhkaOne.percent / 100.0M);
								}
								plata = plata * znizhkaPercent;
							}

							plataInfo.Add(date, plata);
						}
					}

					var totalMonth = plataInfo.Sum(x => x.Value);
					finalPlataByMonth[month] = finalPlataByMonth[month] + totalMonth;
				}
			}

			var result = finalPlataByMonth.Select(x => new { month = x.Key, value = round(x.Value) }).ToDictionary(x => x.month, x => x.value);
			allresult.Add(year, result);
		}
		return allresult;
	}

	bool IsDateValid(DateTime date)
	{
		var dogchangeNum = FindDogchange(date);
		if (dogchangeNum >= 0)
		{
			return dogchangeNum == DogchangeNum;
		}
		else
		{
			return (date >= rentStart && date <= rentFinish);
		}
	}

	int FindDogchange(DateTime date)
	{
		for (int dogchangeNum = 0; dogchangeNum < Dogchanges.Count; dogchangeNum++)
		{
			var dogchange = Dogchanges[dogchangeNum];
			if (
					(dogchange.rent_start_date == null || date >= dogchange.rent_start_date.Value)
						&&
					(dogchange.rent_actual_finish_date == null || date <= dogchange.rent_actual_finish_date.Value)
				)
			{
				return dogchangeNum;
			}
		}
		return -1;
	}



	void CalcMonthPlata()
	{
		foreach (var note in NotesData)
		{
			var noteId = note.Key;

			if (methodCalc == "аукціон")
			{
				CalcMonthPlata__auction(note.Value);
			}
			else if (methodCalc == "без аукціону/нові")
			{
				CalcMonthPlata__noauction(note.Value, true);
			}
			else if (methodCalc == "без аукціону/старі")
			{
				CalcMonthPlata__noauction(note.Value, false);
			}
			else throw new Exception();
		}
	}

	void CalcMonthPlata__auction(NotesDataClass note)
	{
		var year = baseMonth.Year;
		var plata = NotesData[note.id].cost_agreement;
		var monthPlata = note.MonthPlataBase;
		while (year <= LastYear)
		{
			for (var mm = 1; mm <= 12; mm++)
			{
				var date = new DateTime(year, mm, 1);
				monthPlata.Add(date, plata);
			}

			var inflation = GetYearInflation(year);
			if (year == rentStart.Year - 1 && year == baseMonth.Year)
			{
				inflation = 100M; //МЕТОДИКА розрахунку орендної плати за комунальне майно. Пункт 18
			}
			plata = round_0(plata * inflation / 100M);
			year++;
		}
	}

	void CalcMonthPlata__noauction(NotesDataClass note, bool isnew)
	{
		var year = baseMonth.Year;
		var plata = NotesData[note.id].cost_agreement;
		var monthPlata = note.MonthPlataBase;
		while (year <= LastYear)
		{
			var start_month = (year == baseMonth.Year ? baseMonth.Month : 1);
			for (var mm = start_month; mm <= 12; mm++)
			{
				var date = new DateTime(year, mm, 1);
				var inflation = GetMonthInflation(date);

				if (isnew && date == baseMonth && rentStart < baseMonth.AddMonths(1))
				{
					inflation = 100M;
				}

				if (isnew)
				{
					plata = round_0(plata * inflation / 100M);
				}

				monthPlata.Add(date, plata);

				if (!isnew)
				{
					plata = round_0(plata * inflation / 100M);
				}
			}

			year++;
		}
	}

	decimal GetYearInflation(int year)
	{
		decimal value;
		if (InflationYearData.TryGetValue(year, out value))
		{
			return value;
		}
		else
		{
			return 100;
		}
	}

	decimal GetMonthInflation(DateTime month)
	{
		decimal value;
		if (InflationMonthData.TryGetValue(month, out value))
		{
			return value;
		}
		else
		{
			return 100;
		}
	}

	void CreateInflationData()
	{
		var sql1 = "select * from inflation_month" +
				(UseInflationPrognoz ? " union select * from inflation_month_prognoz A where not exists (select 1 from inflation_month Q where Q.[year] = A.[year] and Q.[month] = A.[month])" : "");
		var inflationMonthData = GetDataTable(sql1);
		for (var rownum = 0; rownum < inflationMonthData.Rows.Count; rownum++)
		{
			var year = (int)inflationMonthData.Rows[rownum]["year"];
			var month = (int)inflationMonthData.Rows[rownum]["month"];
			var inflation = (decimal)inflationMonthData.Rows[rownum]["inflation"];
			InflationMonthData.Add(new DateTime(year, month, 1), inflation);
		}

		var sql2 = "select * from inflation_year" +
			(UseInflationPrognoz ? " union select * from inflation_year_prognoz A where not exists (select 1 from inflation_month Q where Q.[year] = A.[year])" : "");
		var inflationYearData = GetDataTable(sql2);
		for (var rownum = 0; rownum < inflationYearData.Rows.Count; rownum++)
		{
			var year = (int)inflationYearData.Rows[rownum]["year"];
			var inflation = (decimal)inflationYearData.Rows[rownum]["inflation"];
			InflationYearData.Add(year, inflation);
		}
	}

	void BuildNotes()
	{
		if (IsDogchange)
		{
			BuildNotesForDogchange();
			return;
		}

		if (IsDBMode)
		{
			var table = GetDataTable(@"
SELECT id, invent_no, cost_agreement FROM reports1nf_arenda_notes 
WHERE (is_deleted IS NULL OR is_deleted = 0) AND report_id = 1594 AND arenda_id = 84257");

			for (var rownum = 0; rownum < table.Rows.Count; rownum++)
			{
				var id = (int)table.GetIntFromSqlDataTable(rownum, "id");
				var invent_no = table.GetStringFromSqlDataTable(rownum, "invent_no");
				var cost_agreement = table.GetDecimalFromSqlDataTable(rownum, "cost_agreement");
				NotesData.Add(id, new NotesDataClass { id = id, invent_no = invent_no, cost_agreement = cost_agreement ?? 0 });
			}
		}
		else
		{
			var table = GridViewNotes.DataSource as DataTable;

			for (var rownum = 0; rownum < table.Rows.Count; rownum++)
			{
				var id = (int)table.Rows[rownum]["id"];
				var invent_no = (string)table.Rows[rownum]["invent_no"];
				var cost_agreement = (decimal?)table.Rows[rownum]["cost_agreement"];
				NotesData.Add(id, new NotesDataClass { id = id, invent_no = invent_no, cost_agreement = cost_agreement ?? 0 });
			}
		}
	}

	void BuildNotesForDogchange()
	{
		var rent_rate = Dogchange.rent_rate ?? 0M;
		var invnum_rent = Dogchange.invnum_rent ?? "";
		var exception1 = new Exception("Невірно заполнено поле \"Орендна плата за інвентарними номерами\": " + invnum_rent);

		if (!string.IsNullOrEmpty(invnum_rent))
		{
			var id = 0;
			var invnum_data = invnum_rent.Split(',');
			foreach (var data in invnum_data)
			{
				var parts = data.Split(':');
				if (parts.Length != 2) throw exception1;

				var invnum = parts[0].Trim();
				if (string.IsNullOrEmpty(invnum)) throw exception1;
				var s2 = parts[1].Replace(",", ".");
				decimal plata;
				if (!Decimal.TryParse(s2, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out plata))
				{
					throw exception1;
				}

				id++;
				NotesData.Add(id, new NotesDataClass
				{
					id = id,
					invent_no = invnum,
					cost_agreement = plata,
				});

				rent_rate -= plata;
			}
		}

		if (rent_rate < 0)
		{
			new Exception("Невірно заполнено поле \"Орендна плата за інвентарними номерами\": " + invnum_rent + ". Місячна орендна плата менша за суму в полі \"Орендна плата за інвентарними номерами\"");
		}

		if (rent_rate > 0)
		{
			NotesData.Add(-1, new NotesDataClass
			{
				id = -1,
				invent_no = "",
				cost_agreement = rent_rate,
			});
		}
	}

	void BuildRentPeriodInfo(int rent_period_id)
	{
		var table = GetDataTable("SELECT period_year, period_quarter FROM dict_rent_period where id = " + rent_period_id);
		FirstYear = (int)table.Rows[0]["period_year"];
		LastMonth = (int)table.Rows[0]["period_quarter"];
		if (IsNextYear)
		{
			LastYear = FirstYear + 1;
		}
		else if (CalcYears.HasValue)
		{
			LastYear = FirstYear + CalcYears.Value - 1;
		}
		else
		{
			LastYear = FirstYear;
		}
	}

	void BuildZnizhka()
	{
		int fileldCount = 10;


		DataTable table = null;
		if (IsDBMode)
		{
			table = GetDataTable(@"select top 1
znizhka1_name, znizhka1_percent, znizhka1_date1, znizhka1_date2, znizhka1_invnums,
znizhka2_name, znizhka2_percent, znizhka2_date1, znizhka2_date2, znizhka2_invnums,
znizhka3_name, znizhka3_percent, znizhka3_date1, znizhka3_date2, znizhka3_invnums,
znizhka4_name, znizhka4_percent, znizhka4_date1, znizhka4_date2, znizhka4_invnums,
znizhka5_name, znizhka5_percent, znizhka5_date1, znizhka5_date2, znizhka5_invnums,
znizhka6_name, znizhka6_percent, znizhka6_date1, znizhka6_date2, znizhka6_invnums,
znizhka7_name, znizhka7_percent, znizhka7_date1, znizhka7_date2, znizhka7_invnums,
znizhka8_name, znizhka8_percent, znizhka8_date1, znizhka8_date2, znizhka8_invnums,
znizhka9_name, znizhka9_percent, znizhka9_date1, znizhka9_date2, znizhka9_invnums,
znizhka10_name, znizhka10_percent, znizhka10_date1, znizhka10_date2, znizhka10_invnums
from reports1nf_arenda_payments A
where A.arenda_id = " + arenda_id + " and A.report_id = " + report_id + " and A.rent_period_id = " + ActiveRentPeriodId);
		}

		DataTable tableFuture = null;
		if (UsePaymentDiscountsFuture)
		{
			tableFuture = GetDataTable("select * from PaymentDiscountsFuture");
		}

		for (int num = 1; num <= fileldCount; num++)
		{
			decimal? percent;
			DateTime? date1, date2;
			string invnums;
			string znizhkaname;

			if (IsDBMode)
			{
				znizhkaname = table.GetStringFromSqlDataTable(0, "znizhka" + num + "_name");
				percent = table.GetDecimalFromSqlDataTable(0, "znizhka" + num + "_percent");
				date1 = table.GetDateTimeFromSqlDataTable(0, "znizhka" + num + "_date1");
				date2 = table.GetDateTimeFromSqlDataTable(0, "znizhka" + num + "_date2");
				invnums = table.GetStringFromSqlDataTable(0, "znizhka" + num + "_invnums");
			}
			else
			{
				znizhkaname = Reports1NFUtils.GetEditText(controls, "znizhka" + num + "_name") as string;
				percent = Reports1NFUtils.GetEditNumeric(controls, "edit_znizhka" + num + "_percent") as decimal?;
				date1 = Reports1NFUtils.GetDateValue(controls, "edit_znizhka" + num + "_date1") as DateTime?;
				date2 = Reports1NFUtils.GetDateValue(controls, "edit_znizhka" + num + "_date2") as DateTime?;
				invnums = Reports1NFUtils.GetEditText(controls, "znizhka" + num + "_invnums") as string;
			}

			string[] invlist = null;
			if (!string.IsNullOrEmpty(invnums))
			{
				invlist = invnums.Split(';').Select(x => x.Trim()).Where(x => !string.IsNullOrEmpty(x)).Distinct().ToArray();
			}
			if (invlist == null)
			{
				invlist = new[] { "" };
			}

			foreach (var invnum in invlist)
			{
				if (percent != null)
				{
					ZnizhkaData.Add(new ZnizhkaClass
					{
						percent = percent.Value,
						date1 = date1 ?? new DateTime(1, 1, 1),
						date2 = date2 ?? new DateTime(4000, 1, 1),
						invnum = invnum,
					});

					if (UsePaymentDiscountsFuture)
					{
						if (!string.IsNullOrEmpty(znizhkaname))
						{
							for (var rownum = 0; rownum < tableFuture.Rows.Count; rownum++)
							{
								var existsName = tableFuture.GetStringFromSqlDataTable(rownum, "ExistsName");
								var fpercent = tableFuture.GetDecimalFromSqlDataTable(rownum, "percent");
								var fdate1 = tableFuture.GetDateTimeFromSqlDataTable(rownum, "date1");
								var fdate2 = tableFuture.GetDateTimeFromSqlDataTable(rownum, "date2");
								if (string.Compare(znizhkaname, existsName, true) == 0)
								{
									ZnizhkaData.Add(new ZnizhkaClass
									{
										percent = fpercent.Value,
										date1 = fdate1 ?? new DateTime(1, 1, 1),
										date2 = fdate2 ?? new DateTime(4000, 1, 1),
										invnum = invnum,
										furure = true,
									});

								}
							}
						}
					}
				}
			}

		}

	}

	ResultClass ReturnEmpty()
	{
		var result = new ResultClass();
		return result;
	}

	DataTable GetDataTable(string sql)
	{
		return GetSqlDataTable(sql, Connection);
	}

	public static DataTable GetSqlDataTable(string sql, SqlConnection connection)
	{
		var factory = DbProviderFactories.GetFactory(connection);
		var dataTable = new DataTable();
		using (var cmd = factory.CreateCommand())
		{
			cmd.CommandText = sql;
			cmd.CommandType = CommandType.Text;
			cmd.Connection = connection;
			using (var adapter = factory.CreateDataAdapter())
			{
				adapter.SelectCommand = cmd;
				adapter.Fill(dataTable);
			}
		}

		return dataTable;
	}

	static Decimal? GetDecimal(object arg)
	{
		if (arg is System.DBNull)
			return (Decimal?)null;
		else
			return (Decimal)arg;
	}

	static DateTime? GetDateTime(object arg)
	{
		if (arg is System.DBNull)
			return (DateTime?)null;
		else
			return (DateTime)arg;
	}

	static string GetString(object arg)
	{
		if (arg is System.DBNull)
			return (string)null;
		else
			return (string)arg;
	}

	static Decimal round(Decimal arg)
	{
		arg = Math.Round(arg, 6);
		var result = (Int64)(arg * 100M) / 100.0M;
		if (arg == result)
		{
			return result;
		}
		else
		{
			return result + 0.01M;
		}
	}

	static Decimal round_0(Decimal arg)
	{
		return arg;
	}

}


