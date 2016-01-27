using CloudDaemon.Common.Entities;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace CloudDaemon.DAL
{
    public class FreeMobileRepository
    {
        public void SaveConsumption(FreeMobileConsumption consumption)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["AWSDatabase"].ConnectionString))
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"
INSERT INTO 
    FreeConsumptionHistory (IdUser, CheckDate, VoiceConsumption, DataConsumption)
VALUES
    (@IdUser, @CheckDate, @VoiceConsumption, @DataConsumption)";
                    command.Parameters.AddParameter("@IdUser", SqlDbType.Int, consumption.User.IdProfile);
                    command.Parameters.AddParameter("@CheckDate", SqlDbType.DateTime2, DateTime.Now);
                    command.Parameters.AddParameter("@VoiceConsumption", SqlDbType.Time, consumption.ConsumedVoice);
                    command.Parameters.AddParameter("@DataConsumption", SqlDbType.Decimal, consumption.ConsumedData);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public FreeMobileConsumption GetLastConsumption(int IdUser)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["AWSDatabase"].ConnectionString))
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"
SELECT
    TOP 1 VoiceConsumption, DataConsumption
FROM 
    FreeConsumptionHistory
WHERE
    IdUser = @IdUser
ORDER BY
    IdHistory DESC";
                    command.Parameters.AddParameter("@IdUser", SqlDbType.Int, IdUser);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        reader.Read();
                        return new FreeMobileConsumption()
                        {
                            ConsumedData = (decimal)reader["DataConsumption"],
                            ConsumedVoice = (TimeSpan)reader["VoiceConsumption"]
                        };
                    }
                }
            }
        }
    }
}
