using System;
using System.Globalization;

public class Converter
{
    public static int? ToInt32Nullable(object value)
    {
        if (value == null)
            return null;

        var result = 0;
        if (int.TryParse(value.ToString(), out result))
            return result;

        try
        {
            result = Convert.ToInt32(value);
        }
        catch
        {
            return null;
        }

        return result;
    }

    public static DateTime? ToDateNullable(object value, string format)
    {
        if (value == null)
            return null;

        var result = DateTime.MinValue;
        if (DateTime.TryParseExact(value.ToString(), format, CultureInfo.CurrentUICulture, DateTimeStyles.None, out result))
            return result;

        try
        {
            result = Convert.ToDateTime(value);
        }
        catch { }

        return result == DateTime.MinValue ? null : (DateTime?)result;
    }

	public static string JsonConvertSerializeObject(object arg)
	{
		var json = Newtonsoft.Json.JsonConvert.SerializeObject(arg);
		return json;
	}
}
