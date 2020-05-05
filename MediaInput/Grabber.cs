using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace MediaInput
{
    //TODO: Figure out a mutex scheme together with the keepalive module of the API so we never try to open another tuner stream when we already have one running
    public class Grabber : IGrabber
    {
        private DateTime _lastCacheTimestamp = DateTime.UnixEpoch;

        //Singleton pattern

        private Grabber()
        {
            Config = new ConfigurationBuilder().AddJsonFile("GrabberConfig.json", false, true).Build();
        }

        static Grabber()
        {
            _singleton = new Grabber();
        }

        public static Grabber GetSingleton()
        {
            //TODO: how do we pass the HTTPClient to this class? 
            //returns _singleton if its not null, otherwise creates new instance and assigns it to _singleton
            return _singleton ??= new Grabber();
        }

        private IConfiguration Config { get; }

        private static Grabber _singleton { get; set; } = null;

        public IEnumerable<string> GetAvailableCategories()
        {
            using (var dbConnection = new MySqlConnection(
                $"Server={Config["MySqlServerAddress"]};" +
                $"Database={Config["MySqlServerDatabase"]};" +
                $"Uid={Config["MySqlServerUser"]};" +
                $"Pwd={Config["MySqlServerPassword"]};")
                )
            {
                var selectCommand = new MySqlCommand($"SELECT DISTINCT Category FROM mediacontent", dbConnection);
                var adapter = new MySqlDataAdapter
                {
                    SelectCommand = selectCommand
                };

                var dataset = new DataSet();
                adapter.Fill(dataset);
                var rowCollection = dataset.Tables[0].Rows;
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

            using (var dbConnection = new MySqlConnection(
                $"Server={Config["MySqlServerAddress"]};" +
                $"Database={Config["MySqlServerDatabase"]};" +
                $"Uid={Config["MySqlServerUser"]};" +
                $"Pwd={Config["MySqlServerPassword"]};")
                )
            {
                var selectCommand = new MySqlCommand($"SELECT * FROM mediacontent WHERE Category=@category", dbConnection);
                selectCommand.Parameters.AddWithValue("@category", category);
                var adapter = new MySqlDataAdapter
                    {SelectCommand = selectCommand};
                var dataset = new DataSet();

                adapter.Fill(dataset);

                var rowCollection = dataset.Tables[0].Rows;

                return (from DataRow entry in rowCollection select new ContentInformation((string) entry[0], (string) entry[1], (string) entry[2], Convert.ToBoolean(entry[3]), Convert.ToBoolean(entry[4]), new Uri((string) entry[5]), new Uri((string) entry[6]))).ToList();
            }
        }

        public Tuple<Uri, bool> GetMediaStream(string contentId)
        {
            var content = GetAvailableContentInformation().Values.SelectMany(item => item);
            var requestedContent = content.FirstOrDefault(item => item.Id == contentId);
            if(requestedContent==null)
                throw new Exception("Content with specified contentId does not exist in database");
            return new Tuple<Uri, bool>(requestedContent.ContentLocation, requestedContent.TunerIsSource);
        }
    }
}