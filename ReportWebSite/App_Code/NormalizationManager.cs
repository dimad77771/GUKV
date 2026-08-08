using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Caching;

public static class NormalizationManager
{
	private const string CacheKey = "GUKV.NormalizationTexts";

	/// <summary>
	/// Загружает нормализованные тексты из БД и полностью заменяет кеш.
	/// </summary>
	public static void Reload()
	{
		var dictionary = new Dictionary<string, string>(
			StringComparer.OrdinalIgnoreCase);

		string connectionString =
			ConfigurationManager.ConnectionStrings["GUKVConnectionString"]
				.ConnectionString;

		using (var connection = new SqlConnection(connectionString))
		using (var command = new SqlCommand(
			"SELECT txt FROM NormalizationTexts WHERE txt IS NOT NULL",
			connection))
		{
			connection.Open();

			using (SqlDataReader reader = command.ExecuteReader(
				CommandBehavior.SequentialAccess))
			{
				while (reader.Read())
				{
					string text = reader.GetString(0);

					if (string.IsNullOrWhiteSpace(text))
						continue;

					/*
                     * Убираем случайные пробелы только по краям.
                     * Внутренние пробелы не изменяем.
                     */
					text = text.Trim();

					/*
                     * При совпадении без учёта регистра оставляем первую
                     * встретившуюся запись.
                     */
					if (!dictionary.ContainsKey(text))
						dictionary.Add(text, text);
				}
			}
		}

		/*
         * Сначала полностью создаём словарь, затем одной операцией
         * заменяем объект в кеше.
         */
		HttpRuntime.Cache.Insert(
			CacheKey,
			dictionary,
			null,
			System.Web.Caching.Cache.NoAbsoluteExpiration,
			System.Web.Caching.Cache.NoSlidingExpiration,
			CacheItemPriority.NotRemovable,
			null);
	}

	/// <summary>
	/// Возвращает нормализованный текст.
	/// Если текст отсутствует в справочнике, возвращает исходное значение.
	/// </summary>
	public static string N(string arg)
	{
		if (string.IsNullOrEmpty(arg))
			return arg;

		var dictionary =
			HttpRuntime.Cache[CacheKey] as Dictionary<string, string>;

		/*
         * Кеш мог быть очищен IIS, например при нехватке памяти
         * или перезапуске приложения.
         */
		if (dictionary == null)
		{
			Reload();

			dictionary =
				HttpRuntime.Cache[CacheKey] as Dictionary<string, string>;

			if (dictionary == null)
				return arg;
		}

		string normalizedText;

		if (dictionary.TryGetValue(arg, out normalizedText))
			return normalizedText;

		return arg;
	}
}