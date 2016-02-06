using CloudDaemon.Common.Interfaces;
using CloudDaemon.Common.Entities;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace CloudDaemon.DAL
{
    public class TaxesRepository : ITaxesManager
    {
        public void SaveTaxNotice(TaxNotice notice)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["AWSDatabase"].ConnectionString))
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"
INSERT INTO 
    TaxNotice (Id, Amount, PaymentDate)
VALUES
    (@Id, @Amount, @PaymentDate)";
                    command.Parameters.AddParameter("@IdUser", SqlDbType.BigInt, notice.Id);
                    command.Parameters.AddParameter("@Amount", SqlDbType.Decimal, notice.PaymentDate);
                    command.Parameters.AddParameter("@PaymentDate", SqlDbType.DateTime2, notice.PaymentDate);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public bool TaxNoticeExists(TaxNotice notice)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["AWSDatabase"].ConnectionString))
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT COUNT(1) FROM TaxNotice WHERE Id = @Id";
                    command.Parameters.AddParameter("@Id", SqlDbType.BigInt, notice.Id);

                    connection.Open();
                    return (int)command.ExecuteScalar() >= 1;
                }
            }
        }
    }
}
