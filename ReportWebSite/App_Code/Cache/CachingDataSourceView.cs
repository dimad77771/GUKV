using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Reflection;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Data.Common;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Text;

namespace Cache
{

    public class CachingDataSourceView : DataSourceView
    {
        private static PropertyInfo _parameterPrefixProperty;
        private static MethodInfo _onSelectingMethod;
        private static MethodInfo _onSelectedMethod;

        private readonly CachingDataSource _owner;
        private readonly HttpContext _context;
        private readonly SqlDataSourceView _view;
        private readonly DataTable _data;

        public CachingDataSourceView(CachingDataSource owner, HttpContext context, SqlDataSourceView view)
            : base(owner, view.Name)
        {
            _owner = owner;
            _context = context;
            _view = view;
        }

        public CachingDataSourceView(CachingDataSource owner, string viewName, DataTable data)
            : base(owner, viewName)
        {
            _owner = owner;
            _data = data;
        }

        private void InitializeParameters(DbCommand command, ParameterCollection parameters, IDictionary exclusionList)
        {
            if (CachingDataSourceView._parameterPrefixProperty == null)
            {
                lock (typeof(CachingDataSourceView))
                {
                    if (CachingDataSourceView._parameterPrefixProperty == null)
                    {
                        CachingDataSourceView._parameterPrefixProperty =
                            typeof(SqlDataSourceView).GetProperty("ParameterPrefix", BindingFlags.NonPublic | BindingFlags.Instance);
                    }
                }
            }

            string parameterPrefix = (string)_parameterPrefixProperty.GetValue(_view, new object[0]);
            IDictionary dictionary = null;
            if (exclusionList != null)
            {
                dictionary = new ListDictionary(StringComparer.OrdinalIgnoreCase);
                foreach (DictionaryEntry entry in exclusionList)
                {
                    dictionary.Add(entry.Key, entry.Value);
                }
            }
            IOrderedDictionary values = parameters.GetValues(_context, this._owner);
            for (int i = 0; i < parameters.Count; i++)
            {
                Parameter parameter = parameters[i];
                if ((dictionary != null) && dictionary.Contains(parameter.Name))
                {
                    continue;
                }
                DbParameter parameter2 = this._owner.CreateParameter(parameterPrefix + parameter.Name, values[i]);
                parameter2.Direction = parameter.Direction;
                parameter2.Size = parameter.Size;
                if ((parameter.DbType != DbType.Object) || ((parameter.Type != TypeCode.Empty) && (parameter.Type != TypeCode.DBNull)))
                {
                    SqlParameter parameter3 = parameter2 as SqlParameter;
                    if (parameter3 == null)
                    {
                        parameter2.DbType = parameter.GetDatabaseType();
                    }
                    else
                    {
                        DbType databaseType = parameter.GetDatabaseType();
                        if (databaseType != DbType.Date)
                        {
                            if (databaseType != DbType.Time)
                            {
                                goto Label_0143;
                            }
                            parameter3.SqlDbType = SqlDbType.Time;
                        }
                        else
                        {
                            parameter3.SqlDbType = SqlDbType.Date;
                        }
                    }
                }
                goto Label_0151;
            Label_0143:
                parameter2.DbType = parameter.GetDatabaseType();
            Label_0151:
                command.Parameters.Add(parameter2);
            }
        }

        private static CommandType GetCommandType(SqlDataSourceCommandType commandType)
        {
            if (commandType == SqlDataSourceCommandType.Text)
                return CommandType.Text;
            return CommandType.StoredProcedure;
        }

        private void ReplaceNullValues(DbCommand command)
        {
            int count = command.Parameters.Count;
            foreach (DbParameter parameter in command.Parameters)
            {
                if (parameter.Value == null)
                    parameter.Value = DBNull.Value;
            }
        }

        private Exception BuildCustomException(Exception ex, DataSourceOperation operation, DbCommand command, out bool isCustomException)
        {
            SqlException exception = ex as SqlException;
            if ((exception != null) && ((exception.Number == 0x89) || (exception.Number == 0xc9)))
            {
                string str;
                if (command.Parameters.Count > 0)
                {
                    StringBuilder builder = new StringBuilder();
                    bool flag = true;
                    foreach (DbParameter parameter in command.Parameters)
                    {
                        if (!flag)
                        {
                            builder.Append(", ");
                        }
                        builder.Append(parameter.ParameterName);
                        flag = false;
                    }
                    str = builder.ToString();
                }
                else
                {
                    str = "SqlDataSourceView_NoParameters";
                }
                isCustomException = true;
                return new InvalidOperationException("SqlDataSourceView_MissingParameters");
            }
            isCustomException = false;
            return ex;
        }

        private void OnSelected(SqlDataSourceStatusEventArgs args)
        {
            if (CachingDataSourceView._onSelectedMethod == null)
            {
                lock (typeof(CachingDataSourceView))
                {
                    if (CachingDataSourceView._onSelectedMethod == null)
                    {
                        CachingDataSourceView._onSelectedMethod =
                            typeof(SqlDataSourceView).GetMethod("OnSelected", BindingFlags.NonPublic | BindingFlags.Instance);
                    }
                }
            }

            CachingDataSourceView._onSelectedMethod.Invoke(_view, new object[] { args });
        }

        protected override IEnumerable ExecuteSelect(DataSourceSelectArguments arguments)
        {
            if (_view.SelectCommand.Length == 0)
                return null;

            DbConnection connection = this._owner.CreateConnection(this._owner.ConnectionString);
            if (connection == null)
                throw new InvalidOperationException("SqlDataSourceView_CouldNotCreateConnection");

            DbCommand command = this._owner.CreateCommand(_view.SelectCommand, connection);
            this.InitializeParameters(command, _view.SelectParameters, null);
            command.CommandType = GetCommandType(_view.SelectCommandType);
            SqlDataSourceSelectingEventArgs e = new SqlDataSourceSelectingEventArgs(command, arguments);

            if (CachingDataSourceView._onSelectingMethod == null)
            {
                lock (typeof(CachingDataSourceView))
                {
                    if (CachingDataSourceView._onSelectingMethod == null)
                    {
                        CachingDataSourceView._onSelectingMethod =
                            typeof(SqlDataSourceView).GetMethod("OnSelecting", BindingFlags.Instance | BindingFlags.NonPublic);
                    }
                }
            }

            _onSelectingMethod.Invoke(_view, new object[] { e });
            if (e.Cancel)
                return null;

            if (_view.CancelSelectOnNullParameter)
            {
                int count = command.Parameters.Count;
                for (int i = 0; i < count; i++)
                {
                    DbParameter parameter = command.Parameters[i];
                    if (((parameter != null) && (parameter.Value == null)) && ((parameter.Direction == ParameterDirection.Input) || (parameter.Direction == ParameterDirection.InputOutput)))
                    {
                        return null;
                    }
                }
            }
            this.ReplaceNullValues(command);
            switch (this._owner.DataSourceMode)
            {
                case SqlDataSourceMode.DataSet:
                    break;

                case SqlDataSourceMode.DataReader:
                default:
                    throw new NotSupportedException();
            }

            DataTable data;
            if (DataSourceCache.TryGet(command, out data))
                return new DataView(data);

            DbDataAdapter adapter = this._owner.CreateDataAdapter(command);
            DataSet dataSet = new DataSet();
            int affectedRows = 0;
            bool flag2 = false;
            try
            {
                affectedRows = adapter.Fill(dataSet, base.Name);
                flag2 = true;
                SqlDataSourceStatusEventArgs args3 = new SqlDataSourceStatusEventArgs(command, affectedRows, null);
                this.OnSelected(args3);
            }
            catch (Exception exception)
            {
                if (flag2)
                {
                    bool flag3;
                    exception = this.BuildCustomException(exception, DataSourceOperation.Select, command, out flag3);
                    if (flag3)
                    {
                        throw exception;
                    }
                    throw;
                }
                SqlDataSourceStatusEventArgs args4 = new SqlDataSourceStatusEventArgs(command, affectedRows, exception);
                this.OnSelected(args4);
                if (!args4.ExceptionHandled)
                {
                    throw;
                }
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            DataTable table = (dataSet.Tables.Count > 0) ? dataSet.Tables[0] : null;
            DataSourceCache.Put(command, table);
            return new DataView(table);
        }
    }

}
