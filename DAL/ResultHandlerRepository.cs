using CloudDaemon.Common.Entities;
using CloudDaemon.Common.Interfaces;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace CloudDaemon.DAL
{
    public class ResultHandlerRepository : IResultHandlerManager
    {
        public IEnumerable<ResultHandlerEntity> GetResultHandlers(int IdMonitor)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["AWSDatabase"].ConnectionString))
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"
SELECT 
    rh.IdResultHandler, rh.ResultHandlerName, rh.IdMonitor, rh.IdProfile, p.IdProfile, p.Login, p.Password, p.Alias 
FROM 
    ResultHandler rh
    LEFT JOIN Profile p ON rh.IdProfile = p.IdProfile
WHERE 
    rh.IdMonitor = @IdMonitor AND
    rh.Activated = 1";
                    command.Parameters.AddParameter("@IdMonitor", SqlDbType.Int, IdMonitor);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return new ResultHandlerEntity(reader);
                        }
                    }
                }
            }
        }
    }
}
