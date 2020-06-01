using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using APIExceptions;

namespace MediaInput
{
    //TODO: Figure out a mutex scheme together with the keepalive module of the API so we never try to open another tuner stream when we already have one running
    public class Grabber : IGrabber
    {
        private DateTime _lastCacheTimestamp = DateTime.UnixEpoch;
        private IConfiguration Config { get; }
        private string SqlConnectionString { get; }
        private ILogger<Grabber> _logger;
        public Grabber(ILogger<Grabber> logger)
        {
            _logger = logger;
            
            Config = new ConfigurationBuilder().AddJsonFile("GrabberConfig.json", false, true).Build();
            SqlConnectionString = $"Server={Config["MySqlServerAddress"]};" +
                $"Database={Config["MySqlServerDatabase"]};" +
                $"Uid={Config["MySqlServerUser"]};" +
                $"Pwd={Config["MySqlServerPassword"]};";
            
            _logger.LogInformation($"{nameof(Grabber)} initialized");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetAvailableCategories()
        {
            using (var dbConnection = new MySqlConnection(SqlConnectionString))
            {
                var selectCommand = new MySqlCommand($"SELECT DISTINCT Category FROM mediacontent", dbConnection);
                var adapter = new MySqlDataAdapter
                {
                    SelectCommand = selectCommand
                };

                var dataset = new DataSet();
                try
                {
                    adapter.Fill(dataset);
                }
                catch (MySqlException mySqlException)
                {
                    _logger.LogError(mySqlException, "Failed to fill dataset from MySQL database");
                    throw new Exception("Internal DataBase Error.");
                }

                var rowCollection = dataset.Tables[0].Rows;
                _logger.LogTrace($"Found {rowCollection.Count} categories");
                return (from DataRow entry in rowCollection
                        select (string)entry[0]);
            }
        }

        public IDictionary<string, IEnumerable<ContentInformation>> GetAvailableContentInformation()
        {
            return GetAvailableCategories().ToDictionary(category => category, GetAvailableContentInformation);
        }

        public IEnumerable<ContentInformation> GetAvailableContentInformation(string category)
        {

            using (var dbConnection = new MySqlConnection(SqlConnectionString))
            {
                var selectCommand = new MySqlCommand($"SELECT * FROM mediacontent WHERE Category=@category", dbConnection);
                selectCommand.Parameters.AddWithValue("@category", category);
                var adapter = new MySqlDataAdapter
                { SelectCommand = selectCommand };
                var dataset = new DataSet();

                adapter.Fill(dataset);

                var rowCollection = dataset.Tables[0].Rows;

                return (from DataRow entry in rowCollection select new ContentInformation((string)entry[0], (string)entry[1], (string)entry[2], Convert.ToBoolean(entry[3]), Convert.ToBoolean(entry[4]), entry[5]!=System.DBNull.Value ?new Uri((string)entry[5]):null, new Uri((string)entry[6]))).ToList();
            }
        }

        public Tuple<Uri, bool> GetMediaStream(string contentId)
        {
            var content = GetAvailableContentInformation().Values.SelectMany(item => item);
            var requestedContent = content.FirstOrDefault(item => item.Id == contentId);
            if(requestedContent==null)
                throw new APINotFoundException("Content with specified contentId does not exist in database");
            return new Tuple<Uri, bool>(requestedContent.ContentLocation, requestedContent.TunerIsSource);
        }
    }
}