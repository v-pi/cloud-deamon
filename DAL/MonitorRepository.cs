using CloudDaemon.Common.Entities;
using CloudDaemon.Common.Interfaces;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace CloudDaemon.DAL
{
    public class MonitorRepository : IMonitorManager
    {
        public IEnumerable<MonitorEntity> GetAllMonitors()
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["AWSDatabase"].ConnectionString))
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"
SELECT
    m.IdMonitor, m.MonitorName, m.Frequency, m.LastRun, m.Activated, p.IdProfile, p.Login, p.Password, p.Alias
FROM
    Monitor m LEFT JOIN
    Profile p ON m.IdProfile = p.IdProfile";

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return new MonitorEntity(reader);
                        }
                    }
                }
            }
        }

        public void UpdateLastRunTime(MonitorEntity monitor)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["AWSDatabase"].ConnectionString))
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"
UPDATE
    Monitor
SET
    LastRun = @LastRun
WHERE
    IdMonitor = @IdMonitor";

                    command.Parameters.AddParameter("@IdMonitor", SqlDbType.Int, monitor.IdMonitor);
                    command.Parameters.AddParameter("@LastRun", SqlDbType.DateTime2, monitor.LastRun);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
