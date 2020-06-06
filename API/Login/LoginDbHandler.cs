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
            IConfiguration config = new ConfigurationBuilder().AddJsonFile("LoginConfig.json", false, true).Build();
            _defaultTimeSpan = TimeSpan.FromMinutes(int.Parse(config["TokenLifespan"]));
            _sqlConnectionString = $"Server={config["MySqlServerAddress"]};" +
                                   $"Database={config["MySqlServerDatabase"]};" +
                                   $"Uid={config["MySqlServerUser"]};" +
                                   $"Pwd={config["MySqlServerPassword"]};";
            _tokenDictionary = new Dictionary<string, AuthTokenInformation>();
        }

        private readonly TimeSpan _defaultTimeSpan;

        private readonly Dictionary<string, AuthTokenInformation> _tokenDictionary;

        private readonly ILogger<LoginDbHandler> _logger;
        private readonly string _sqlConnectionString;

        /// <summary>
        /// Logs in a user by creating a new token for them if the credentials are valid.
        /// </summary>
        /// <param name="account">The user supplied credentials</param>
        /// <returns>The newly created token.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="account"/> is <c>null</c>.</exception>
        /// <exception cref="APIBadRequestException"><paramref name="account"/> was not found in the database.</exception>
        public string LoginUser(Account account)
        {
            if (account == null)
            {
                _logger.LogError("Tried to login with account NULL");
                throw new ArgumentNullException();
            }
            
            try
            {
                using (var dbConnection = new MySqlConnection(_sqlConnectionString))
                {
                    dbConnection.Open();
                    var selectCommand =
                        new MySqlCommand(
                            $"SELECT count(1) FROM login WHERE BENUTZERNAME =@username AND PASSWORT =@password",
                            dbConnection);
                    //selectCommand.Prepare();
                    selectCommand.Parameters.AddWithValue("@username", account.Username);
                    selectCommand.Parameters.AddWithValue("@password", account.Password);

                    var reader = selectCommand.ExecuteReader();

                    if (!reader.HasRows)
                    {
                        var ex = new APIBadRequestException("Username or password invalid.");
                        _logger.LogTrace(ex, ex.Message);
                        throw ex;
                    }

                    var newToken = Guid.NewGuid().ToString();
                    _tokenDictionary.Add(newToken, new AuthTokenInformation(account.Username));
                    dbConnection.Close();
                    return newToken;
                }
            }
            catch (MySqlException mySqlException)
            {
                _logger.LogError(mySqlException, "Failed to fill dataset from MySQL database for login.");
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <exception cref="APIUnauthorizedException"></exception>
        public void LogoutUser(string token)
        {
            if(!_tokenDictionary.ContainsKey(token))
                throw new APIUnauthorizedException("The supplied token is unknown.");
            _tokenDictionary.Remove(token);
        }

        /// <summary>
        /// Checks a token for validity. Updates LastAccess and returns username if valid.
        /// </summary>
        /// <param name="token">The token to be checked.</param>
        /// <returns>The username that the <paramref name="token"/> belongs to.</returns>
        /// <exception cref="APIUnauthorizedException">The <paramref name="token"/> was either not found or stale.</exception>
        public string CheckToken(string token)
        {
            if (!_tokenDictionary.ContainsKey(token))
                throw new APIUnauthorizedException("The supplied token is unknown.");
            if (_tokenDictionary[token].LastAccess.Add(_defaultTimeSpan) > DateTime.Now)
            {
                var authTokenInformation = _tokenDictionary[token];
                authTokenInformation.LastAccess = DateTime.Now;
                return _tokenDictionary[token].Username;
            }

            _tokenDictionary.Remove(token);
            throw new APIUnauthorizedException("The supplied token was stale.");
        }
    }
}