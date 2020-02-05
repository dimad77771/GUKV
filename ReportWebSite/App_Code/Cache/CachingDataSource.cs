using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Data.Common;
using System.Web.UI;
using System.ComponentModel;

namespace Cache
{

    public class CachingDataSource : SqlDataSource
    {
        public CachingDataSource()
        {
            EnableCaching = false;
        }

        #region Property Overrides

        [DefaultValue(false)]
        public override bool EnableCaching
        {
            get
            {
                return base.EnableCaching;
            }
            set
            {
                base.EnableCaching = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        public override DataSourceCacheExpiry CacheExpirationPolicy
        {
            get
            {
                return base.CacheExpirationPolicy;
            }
            set
            {
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        public override int CacheDuration
        {
            get
            {
                return base.CacheDuration;
            }
            set
            {
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        public override string CacheKeyDependency
        {
            get
            {
                return base.CacheKeyDependency;
            }
            set
            {
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        public override string SqlCacheDependency
        {
            get
            {
                return base.SqlCacheDependency;
            }
            set
            {
            }
        }

        #endregion Property Overrides

        internal DbConnection CreateConnection(string connectionString)
        {
            DbConnection connection = this.GetDbProviderFactory().CreateConnection();
            connection.ConnectionString = connectionString;
            return connection;
        }

        internal DbCommand CreateCommand(string commandText, DbConnection connection)
        {
            DbCommand command = this.GetDbProviderFactory().CreateCommand();
            command.CommandText = commandText;
            command.Connection = connection;
            return command;
        }

        internal DbDataAdapter CreateDataAdapter(DbCommand command)
        {
            DbDataAdapter adapter = this.GetDbProviderFactory().CreateDataAdapter();
            adapter.SelectCommand = command;
            return adapter;
        }

        internal DbParameter CreateParameter(string parameterName, object parameterValue)
        {
            DbParameter parameter = this.GetDbProviderFactory().CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.Value = parameterValue;
            return parameter;
        }

        protected override DataSourceView GetView(string viewName)
        {
            SqlDataSourceView view = (SqlDataSourceView)base.GetView(viewName);
            if (EnableCaching)
                return new CachingDataSourceView(this, this.Context, view);
            return view;
        }
    }

}
