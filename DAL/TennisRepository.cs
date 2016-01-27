using CloudDaemon.Common.Entities;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace CloudDaemon.DAL
{
    public class TennisRepository
    {
        public bool IsAvailableSlotAlreadySent(DateTime availableDate)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["AWSDatabase"].ConnectionString))
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT COUNT(1) FROM AvailableTennisSlotsHistory WHERE AvailableDate = @AvailableDate";
                    command.Parameters.AddParameter("@AvailableDate", SqlDbType.DateTime2, availableDate);

                    connection.Open();
                    return (int)command.ExecuteScalar() >= 1;
                }
            }
        }

        public void SaveAvailableSlot(AvailableTennisSlot slot)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["AWSDatabase"].ConnectionString))
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"
INSERT INTO 
    AvailableTennisSlotsHistory (AvailableDate, CourtNumber, AddedDate)
VALUES
    (@AvailableDate, @CourtNumber, @AddedDate)";
                    command.Parameters.AddParameter("@AvailableDate", SqlDbType.DateTime2, slot.StartDateTime);
                    command.Parameters.AddParameter("@CourtNumber", SqlDbType.TinyInt, slot.CourtNumber);
                    command.Parameters.AddParameter("@AddedDate", SqlDbType.DateTime2, DateTime.Now);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
