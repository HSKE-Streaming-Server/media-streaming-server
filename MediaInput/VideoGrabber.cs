using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace MediaInput
{
    public class VideoGrabber : IGrabber
    {
        //Singleton
        private static VideoGrabber _singleton = null;
        //DB Config
        private IConfiguration Config { get; }

        private VideoGrabber()
        {
            this.Config = new ConfigurationBuilder().AddJsonFile("VideoStreamDBConfig.json", false, true).Build();
        }

        public static VideoGrabber GetSingletonInstance()
        {
            return _singleton ??= new VideoGrabber();
        }

        public IEnumerable<string> GetAvailableCategories()
        {
            using (var dbConnection = new MySqlConnection(
                $"Server={Config["MySqlServerAddress"]};" +
                $"Database={Config["MySqlServerDatabase"]};" +
                $"Uid={Config["MySqlServerUser"]};" +
                $"Pwd={Config["MySqlServerPassword"]};")
                )
            {
                var selectCommand = new MySqlCommand($"SELECT DISTINCT Category FROM content", dbConnection);
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
                var selectCommand = new MySqlCommand($"SELECT * FROM content WHERE Category=@category", dbConnection);
                selectCommand.Parameters.AddWithValue("@category", category);
                var adapter = new MySqlDataAdapter
                {
                    SelectCommand = selectCommand
                };

                var dataset = new DataSet();
                adapter.Fill(dataset);
                var rowCollection = dataset.Tables[0].Rows;

                return (from DataRow entry in rowCollection
                        select new ContentInformation(
                            (string)entry[0], (string)entry[1], (string)entry[2],
                            Convert.ToBoolean(entry[3]), Convert.ToBoolean(entry[4]),
                            new Uri((string)entry[5]), new Uri((string)entry[6]))).ToList();
            }
        }

        public Tuple<Uri, bool> GetMediaStream(string contentId)
        {
            var content = GetAvailableContentInformation().Values.SelectMany(item => item);
            var requestedContent = content.FirstOrDefault(item => item.Id == contentId);
            if (requestedContent == null)
                throw new Exception("Content with specified contentId does not exist in database");
            return new Tuple<Uri, bool>(requestedContent.ContentLocation, requestedContent.TunerIsSource);
        }


    }
}
