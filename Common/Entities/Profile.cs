using CloudDaemon.Common.Impl;
using System;
using System.Data.SqlClient;

namespace CloudDaemon.Common.Entities
{
    public class Profile
    {
        /// <summary>
        /// Use this profile for websites where no login/password is required
        /// </summary>
        public static Profile EmptyProfile = new Profile(String.Empty, String.Empty);

        public string Login { get; set; }

        public byte[] EncryptedLogin
        {
            get
            {
                return CryptoUtils.EncryptStringAES(Login);
            }
            set
            {
                Login = CryptoUtils.DecryptStringAES(value);
            }
        }

        public string Password { get; set; }

        public byte[] EncryptedPassword
        {
            get
            {
                return CryptoUtils.EncryptStringAES(Password);
            }
            set
            {
                Password = CryptoUtils.DecryptStringAES(value);
            }
        }

        public string Alias { get; set; }

        public int IdProfile { get; set; }

        public Profile(string login, string password)
        {
            this.Login = login;
            this.Password = password;
        }

        public Profile(SqlDataReader reader)
        {
            EncryptedLogin = (byte[])reader["Login"];
            EncryptedPassword = (byte[])reader["Password"];
            IdProfile = (int)reader["IdProfile"];
            Alias = (string)reader["Alias"];
        }
    }
}
