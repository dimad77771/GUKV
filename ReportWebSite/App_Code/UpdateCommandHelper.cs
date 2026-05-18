using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;

public static class UpdateCommandHelper
{
	public static void PrepareUpdateCommand(Page page, SqlDataSource sqlDataSource, DbCommand command)
	{
		if (page == null)
			throw new ArgumentNullException("page");

		if (sqlDataSource == null)
			throw new ArgumentNullException("sqlDataSource");

		if (command == null)
			throw new ArgumentNullException("command");

		var sessionKey = "__UpdateCommandHelper_OriginalUpdateCommand_" + page.AppRelativeVirtualPath + "_" + sqlDataSource.ID;

		if (page.Session[sessionKey] == null)
		{
			page.Session[sessionKey] = sqlDataSource.UpdateCommand;
		}

		var originalUpdateCommand = (string)page.Session[sessionKey];

		var existingParameters = command.Parameters
			.Cast<DbParameter>()
			.Select(q => q.ParameterName)
			.ToList();

		var newUpdateCommand = BuildUpdateCommand(originalUpdateCommand, existingParameters);

		sqlDataSource.UpdateCommand = newUpdateCommand;
		command.CommandText = newUpdateCommand;
	}

	private static string BuildUpdateCommand(string sql, List<string> existingParameters)
	{
		var match = Regex.Match(
			sql,
			@"(?<beforeSet>.*?\bSET\b)(?<set>.*?)(?<where>\bWHERE\b.*)$",
			RegexOptions.IgnoreCase | RegexOptions.Singleline);

		if (!match.Success)
		{
			return sql;
		}

		var setPart = match.Groups["set"].Value;
		var assignments = SplitAssignments(setPart);
		var filteredAssignments = new List<string>();

		foreach (var assignment in assignments)
		{
			var parameterName = GetAssignmentParameterName(assignment);

			if (string.IsNullOrWhiteSpace(parameterName))
			{
				filteredAssignments.Add(assignment);
				continue;
			}

			if (existingParameters.Any(q => String.Equals(q, parameterName, StringComparison.OrdinalIgnoreCase)))
			{
				filteredAssignments.Add(assignment);
			}
		}

		return match.Groups["beforeSet"].Value +
			Environment.NewLine +
			string.Join("," + Environment.NewLine, filteredAssignments) +
			Environment.NewLine +
			match.Groups["where"].Value;
	}

	private static string GetAssignmentParameterName(string assignment)
	{
		var match = Regex.Match(
			assignment,
			@"=\s*(?<param>@[A-Za-z0-9_]+)\s*$",
			RegexOptions.IgnoreCase);

		if (!match.Success)
		{
			return null;
		}

		return match.Groups["param"].Value;
	}

	private static List<string> SplitAssignments(string setPart)
	{
		return setPart
			.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
			.Select(q => q.Trim())
			.Where(q => !string.IsNullOrWhiteSpace(q))
			.ToList();
	}
}