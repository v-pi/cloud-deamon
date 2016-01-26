using System;
using System.Data.SqlClient;

namespace CloudDaemon.Common.Entities
{
    public class ResultHandlerEntity
    {
        public int IdResultHandler { get; set; }

        public string ResultHandlerName { get; set; }

        public string ResultHandlerAssembly
        {
            get
            {
                string[] namespaceParts = ResultHandlerName.Split('.');
                return String.Join(".", namespaceParts[0], namespaceParts[1]);
            }
        }

        public int IdMonitor { get; set; }

        public Profile Profile { get; set; }

        public ResultHandlerEntity()
        {
        }

        public ResultHandlerEntity(SqlDataReader reader)
        {
            IdResultHandler = (int)reader["IdResultHandler"];
            ResultHandlerName = (string)reader["ResultHandlerName"];
            IdMonitor = (int)reader["IdMonitor"];
            if (reader["IdProfile"] != DBNull.Value)
            {
                Profile = new Profile(reader);
            }
        }
    }
}
