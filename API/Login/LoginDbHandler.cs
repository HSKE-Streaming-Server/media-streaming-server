using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Model;
using MediaInput;
using MySql.Data.MySqlClient;
using System.Data;
using APIExceptions;
using Microsoft.Extensions.Logging;

namespace API.Login
{
    public class LoginDbHandler
    {

        public LoginDbHandler(ILogger<LoginDbHandler> logger)
        {
            _logger = logger;
            Config = new ConfigurationBuilder().AddJsonFile("LoginConfig.json", false, true).Build();
            DefaultTimeSpan = TimeSpan.FromMinutes(int.Parse(Config["TokenLifespan"]));


        }
        private TimeSpan DefaultTimeSpan { get; set; }

        private ILogger<LoginDbHandler> _logger;

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
                    dbConnection.Open();
                    var selectCommand = new MySqlCommand($"SELECT count(1) FROM login WHERE BENUTZERNAME =@username AND PASSWORT =@password", dbConnection);
                    //selectCommand.Prepare();
                    selectCommand.Parameters.AddWithValue("@username", account.Username);
                    selectCommand.Parameters.AddWithValue("@password", account.Password);

                    var reader = selectCommand.ExecuteReader();

                    if (!reader.HasRows)
                    {
                        var ex = new APIBadRequestException("Username or passwort invalid.");
                        _logger.LogTrace(ex, ex.Message);
                        throw ex;
                    }

                    return Guid.NewGuid().ToString();


                }
            }
            catch (MySqlException mySqlException)
            {
                _logger.LogError(mySqlException, "Failed to fill dataset from MySQL database");
                throw;
            }  
        }
    }
}
