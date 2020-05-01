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
    public class SundtekGrabber : IGrabber
    {
        private DateTime _lastCacheTimestamp = DateTime.UnixEpoch;
        private IConfiguration _config;

        //Singleton pattern
        private static SundtekGrabber _singleton = null;

        private SundtekGrabber()
        {
            _config = new ConfigurationBuilder().AddJsonFile("SundtekConfig.json", false, true).Build();
        }

        static SundtekGrabber()
        {
            _singleton = new SundtekGrabber();
        }

        public static SundtekGrabber GetSingleton()
        {
            //TODO: how do we pass the HTTPClient to this class? 
            //returns _singleton if its not null, otherwise creates new instance and assigns it to _singleton
            return _singleton ??= new SundtekGrabber();
        }


        public IEnumerable<string> GetAvailableCategories()
        {
            return new[] {"television", "radio"};
        }

        public IDictionary<string, IEnumerable<ContentInformation>> GetAvailableContentInformation()
        {
            return GetAvailableCategories().ToDictionary(category => category, GetAvailableContentInformation);
        }

        public IEnumerable<ContentInformation> GetAvailableContentInformation(string category)
        {
            
            using (var conn =
                new MySqlConnection(
                    $"Server={_config["MySqlServerAddress"]};Database={_config["MySqlServerDatabase"]};Uid={_config["MySqlServerUser"]};Pwd={_config["MySqlServerPassword"]};")
            )
            {
                var selectCommand = new MySqlCommand($"SELECT * FROM sundtekcontent WHERE Category=@category", conn);
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