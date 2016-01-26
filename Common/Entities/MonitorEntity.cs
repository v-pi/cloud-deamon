using System;
using System.Data.SqlClient;

namespace CloudDaemon.Common.Entities
{
    public class MonitorEntity
    {
        public MonitorEntity(SqlDataReader reader)
        {
            IdMonitor = (int)reader["IdMonitor"];
            MonitorName = (string)reader["MonitorName"];
            Frequency = (TimeSpan)reader["Frequency"];
            LastRun = (DateTime)reader["LastRun"];
            if (reader["IdMonitor"] != DBNull.Value)
            {
                Profile = new Profile(reader);
            }
        }

        public MonitorEntity()
        {
        }

        public int IdMonitor { get; set; }

        public string MonitorName { get; set; }

        public TimeSpan Frequency { get; set; }

        public DateTime LastRun { get; set; }

        public Profile Profile { get; set; }

        public string MonitorAssembly
        {
            get
            {
                string[] namespaceParts = MonitorName.Split('.');
                return String.Join(".", namespaceParts[0], namespaceParts[1]);
            }
        }
    }
}
