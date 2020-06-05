using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Model;
using MediaInput;
using MySql.Data.MySqlClient;
using System.Data;

namespace API.Login
{
    public class LoginDbHandler
    {

        public LoginDbHandler()
        {
            Config = new ConfigurationBuilder().AddJsonFile("LoginConfig.json", false, true).Build();
            DefaultTimeSpan = TimeSpan.FromMinutes(int.Parse(Config["TokenLifespan"]));

        }
        private TimeSpan DefaultTimeSpan { get; set; } 
        private IConfiguration Config { get; set; }
        private string SqlConnectionString { get; set; }

        public string CreateToken(Account account)
        {

         SqlConnectionString =   $"Server={Config["MySqlServerAddress"]};" +
                                 $"Database={Config["MySqlServerDatabase"]};" +
                                 $"Uid={Config["MySqlServerUser"]};" +
                                 $"Pwd={Config["MySqlServerPassword"]};";

            try
            {
                using (var dbConnection = new MySqlConnection(SqlConnectionString))
                {
                    var selectCommand = new MySqlCommand($"SELECT count(1) FROM login WHERE BENUTZERNAME =@username AND PASSWORT =@password", dbConnection);
                    selectCommand.Prepare();
                    selectCommand.Parameters.AddWithValue("@username", account.Username);
                    selectCommand.Parameters.AddWithValue("@password", account.Password);

                    var reader = selectCommand.ExecuteReader();
                    if (!reader.HasRows) throw new IndexOutOfRangeException();
                }
            }
            catch (MySqlException mySqlException)
            {
                //_logger.LogError(mySqlException, "Failed to fill dataset from MySQL database");
                throw;
            }
            catch (IndexOutOfRangeException IndexoutRangeExcepion)
            {
                //_logger.LogError(mySqlException, "Failed to fill dataset from MySQL database");
                throw;
            }

    



                return "haltsmaul";
        }
    }
}
