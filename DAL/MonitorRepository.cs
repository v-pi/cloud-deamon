using CloudDaemon.Common.Entities;
using CloudDaemon.Common.Interfaces;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;

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
    m.IdMonitor, m.MonitorName, m.Frequency, m.LastRun, p.IdProfile, p.Login, p.Password, p.Alias 
FROM 
    Monitor m
    LEFT JOIN Profile p ON m.IdProfile = p.IdProfile
WHERE
    m.Activated = 1";

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
    }
}
