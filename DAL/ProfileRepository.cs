using CloudDaemon.Common.Entities;
using CloudDaemon.Common.Interfaces;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace CloudDaemon.DAL
{
    public class ProfileRepository : IProfileManager
    {
        public Profile GetProfileById(int profileId)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["AWSDatabase"].ConnectionString))
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT IdProfile, Login, Password, Alias FROM Profile WHERE IdProfile= @IdProfile";
                    command.Parameters.AddParameter("@IdProfile", SqlDbType.Int, profileId);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        reader.Read();
                        return new Profile(reader);
                    }
                }
            }
        }

        public void InsertProfile(Profile profile)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["AWSDatabase"].ConnectionString))
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"
INSERT INTO 
    Profile (Login, Password, Alias)
VALUES 
    (@Login, @Password, @Alias)";
                    command.Parameters.AddParameter("@Login", SqlDbType.VarBinary, profile.EncryptedLogin);
                    command.Parameters.AddParameter("@Password", SqlDbType.VarBinary, profile.EncryptedPassword);
                    command.Parameters.AddParameter("@Alias", SqlDbType.VarChar, profile.Alias);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
