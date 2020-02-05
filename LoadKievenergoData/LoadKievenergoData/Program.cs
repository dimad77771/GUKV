#define ENABLE_ADDRESS_EXTRACTION
#define ENABLE_EXCEL_LOAD

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Excel;
using System.IO;
using System.Data;
using System.Text.RegularExpressions;

namespace LoadKievenergoData
{
    class Program
    {
        public static decimal? ToDecimal(object value)
        {
            if (value is DBNull)
                return null;
            return Convert.ToDecimal(value);
        }

        public static DateTime? ToDateTime(object yearValue, object monthValue)
        {
            if (yearValue is DBNull || monthValue is DBNull)
                return null;

            return DateTime.Parse("1 " + monthValue + " " + yearValue, System.Globalization.CultureInfo.GetCultureInfo("uk-UA"));
        }

        public static DateTime? ToDateTime(object yearMonthDayValue)
        {
            if (yearMonthDayValue is DBNull)
                return null;

            return DateTime.Parse(yearMonthDayValue.ToString(), System.Globalization.CultureInfo.GetCultureInfo("uk-UA"));
        }

        public static string ToString(object value)
        {
            if (value is DBNull)
                return null;
            return value.ToString();
        }

        private static readonly string[] AddrLeadIn = {
                                                          "КТМ",
                                                          "КЕМ",
                                                          "СТМ",
                                                          "Апарат управління"
                                                      };
        private static readonly string[] InvalidAddress = {
                                                              "Київські електричні мережі",
                                                              "Київські теплові мережі",
                                                              "Район теплових розподільчих мереж \"Троєщина\"",
                                                              "м.Київ",
                                                              "м. Київ,"
                                                          };
        private static readonly string[] InvalidAddressPrefix = {
                                                                    "р/к",
                                                                    "рк",
                                                                    "к/к"
                                                                };

        public static string ExtractAddress(string location)
        {
            if (location == null)
                return null;

            Match match1 = Regex.Match(location, @"\bм\.Київ\b", RegexOptions.IgnoreCase);
            if (match1.Success)
                return location.Substring(match1.Index);

            Match match2 = Regex.Match(location, @"\b(вул\.?|вулиця|ул\.?|улица|пр\.?|просп\.?|проспект|пров\.?|провулок|бульв\.?|бул\.?|бв|бульвар|пл\.?|площа)\b", RegexOptions.IgnoreCase);
            if (match2.Success)
                return location.Substring(match2.Index);

            foreach (string leadIn in AddrLeadIn)
            {
                string searchSubstring = " " + leadIn + ",";

                int idx = location.IndexOf(searchSubstring, StringComparison.OrdinalIgnoreCase);
                if (idx > 0)
                    return location.Substring(idx + searchSubstring.Length);
            }

            return null;
        }

        static void Main(string[] args)
        {
            DateTime startDate = DateTime.Now;
            Console.WriteLine("Start date: {0}", startDate);

#if !DISABLE_TRY_CATCH
            try
            {
#endif
                //foreach (var culture in System.Globalization.CultureInfo.GetCultures(System.Globalization.CultureTypes.SpecificCultures))
                //{
                //    Console.WriteLine("{0}", culture.Name);
                //    if (culture.Name.ToLower().Contains("ua"))
                //        Console.WriteLine("**********");
                //}

                //return;

                //Console.WriteLine(ExtractAddress("Апарат управління ЕС, Енергосервіс"));

                var adapter = new DBTableAdapters.balans_kievenergoTableAdapter();

                using (var stream = File.Open(@"C:\Users\a\Documents\Перелік ОЗ ком власн КЕ на 30.06.2015.xlsx", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (var reader = ExcelReaderFactory.CreateOpenXmlReader(stream))
                    {
#if ENABLE_EXCEL_LOAD
                        DataSet dataset = reader.AsDataSet();

                        adapter.Connection.Open();
                        using (var command = adapter.Connection.CreateCommand())
                        {
                            command.CommandText = "TRUNCATE TABLE [balans_kievenergo]";
                            command.ExecuteNonQuery();
                        }

                        foreach (var row in dataset.Tables[0].Rows.Cast<DataRow>().Skip(6))
                        {
                            if (row[0] is DBNull)
                                break;

                            adapter.Insert(ToString(row[1]), ToString(row[2]), ToString(row[3]), ToString(row[4]),
                                ToDecimal(row[5]).Value, ToDecimal(row[6]).Value, ToDecimal(row[7]).Value, ToDecimal(row[8]),
                                ToDateTime(row[9], row[10]).Value, null, ToString(row[11]), ToString(row[12]), ToDateTime(row[13]), null);
                        }

                        foreach (var row in dataset.Tables[1].Rows.Cast<DataRow>().Skip(7))
                        {
                            if (row[0] is DBNull || ToString(row[0]) == "Всього")
                                break;

                            adapter.Insert(ToString(row[1]), ToString(row[2]), ToString(row[3]), ToString(row[4]),
                                ToDecimal(row[5]).Value, ToDecimal(row[6]).Value, ToDecimal(row[7]), ToDecimal(row[8]),
                                ToDateTime(row[9], row[10]).Value, null, null, null, null, null);
                        }

                        foreach (var row in dataset.Tables[2].Rows.Cast<DataRow>().Skip(7))
                        {
                            if (row[0] is DBNull || ToString(row[0]) == "Всього")
                                break;

                            adapter.Insert(ToString(row[1]), ToString(row[2]), ToString(row[3]), ToString(row[4]),
                                ToDecimal(row[5]).Value, ToDecimal(row[6]).Value, ToDecimal(row[7]), ToDecimal(row[8]),
                                ToDateTime(row[9], row[10]).Value, ToDateTime(row[11], row[12]) ?? new DateTime(2015, 6, 1), null, null, null, null);
                        }

                        using (var command = adapter.Connection.CreateCommand())
                        {
                            command.CommandText = "update balans_kievenergo set on_balance_date = case when commissioned_date < '2001-09-01' then '2001-09-01' else commissioned_date end";
                            command.ExecuteNonQuery();
                        }
#endif
#if ENABLE_ADDRESS_EXTRACTION
                        foreach (var row in adapter.GetData())
                        {
                            if (row.IslocationNull())
                                continue;

                            string address = (ExtractAddress(row.location) ?? string.Empty).Trim();
                            if (!string.IsNullOrWhiteSpace(address)
                                && !InvalidAddress.Any(x => x.Equals(address, StringComparison.OrdinalIgnoreCase))
                                && !InvalidAddressPrefix.Any(x => Regex.IsMatch(address, "^" + x + @"\b", RegexOptions.IgnoreCase)))
                            {
                                row.address = address;
                                adapter.Update(row);
                            }
                        }
#endif

                        //dataset.WriteXml(@"C:\Users\a\Documents\Перелік ОЗ ком власн КЕ на 30.06.2015.dataset.xml");
                        //do
                        //{
                        //    bool isFirstRow = true;

                        //    while (reader.Read())
                        //    {
                        //        if (isFirstRow)
                        //        {
                        //            isFirstRow = false;

                        //            for (int column = 0; column < reader.FieldCount; column++)
                        //            {
                        //                Console.WriteLine("[{0}] {1}", column, reader.GetName(column));
                        //            }
                        //        }
                        //    }
                        //} while (reader.NextResult());
                    }
                }
#if !DISABLE_TRY_CATCH
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
#endif

            DateTime endDate = DateTime.Now;
            Console.WriteLine("End date: {0} (elapsed: {1})", endDate, (endDate - startDate));
        }
    }
}
