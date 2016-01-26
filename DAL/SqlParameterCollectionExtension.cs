using System.Data;
using System.Data.SqlClient;

namespace CloudDaemon.DAL
{
    public static class SqlParameterCollectionExtension
    {
        public static SqlParameter AddParameter(this SqlParameterCollection coll, string parameterName, SqlDbType sqlDbType, object value)
        {
            SqlParameter param = coll.Add(parameterName, sqlDbType);
            param.Value = value;
            return param;
        }
    }
}
