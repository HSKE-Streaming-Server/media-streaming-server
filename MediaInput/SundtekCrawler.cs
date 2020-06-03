using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading;
using Data;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace MediaInput
{
    public class SundtekCrawler
    {
        private readonly HttpClient _client;
        private Timer _crawlTimer;
        private IConfiguration _config;

        private static SundtekCrawler _singleton;

        private SundtekCrawler(HttpClient client)
        {
            _client = client;
            _config = new ConfigurationBuilder().AddJsonFile("SundtekConfig.json", false, true).Build();
        }

        public static SundtekCrawler GetSingleton(HttpClient client)
        {
            return _singleton ??= new SundtekCrawler(client);
        }

        /// <summary>
        /// Starts crawling task for this crawler with the specified interval.
        /// </summary>
        /// <param name="interval">The interval between crawls.</param>
        public void StartCrawling(TimeSpan interval)
        {
            _crawlTimer ??= new Timer(CrawlMethod, null, TimeSpan.Zero, interval);
        }

        public void StopCrawling()
        {
            _crawlTimer.Dispose();
        }

        private void CrawlMethod(Object stateInfo)
        {
            var radioUri =
                new Uri(
                    $"http://{_config["SundtekServerAddress"]}:{_config["SundtekServerPort"]}{_config["SundtekRadioUri"]}");
            var freeSdtvuri =
                new Uri(
                    $"http://{_config["SundtekServerAddress"]}:{_config["SundtekServerPort"]}{_config["SundtekSdtvUri"]}");
            var freeHdtvuri =
                new Uri(
                    $"http://{_config["SundtekServerAddress"]}:{_config["SundtekServerPort"]}{_config["SundtekHdtvUri"]}");
            var radioPlaylist = _client.GetStreamAsync(radioUri).Result;
            var freeSdtvPlaylist = _client.GetStreamAsync(freeSdtvuri).Result;
            var freeHdtvPlaylist = _client.GetStreamAsync(freeHdtvuri).Result;


            var radioList = M3UParser.ParsePlaylist(radioPlaylist).Select
            (item => new ContentInformation(item.contentUri.ToString().GetSha1HashAsHexString(),
                item.channelName, "radio", true, true, new Uri(item.pictureUri), new Uri(item.contentUri)));

            var sdtvList = M3UParser.ParsePlaylist(freeSdtvPlaylist).Select
            (item => new ContentInformation(item.contentUri.ToString().GetSha1HashAsHexString(),
                item.channelName, "television", true, true, new Uri(item.pictureUri), new Uri(item.contentUri)));

            var hdtvList = M3UParser.ParsePlaylist(freeHdtvPlaylist).Select
            (item => new ContentInformation(item.contentUri.ToString().GetSha1HashAsHexString(),
                item.channelName, "television", true, true, new Uri(item.pictureUri), new Uri(item.contentUri)));


            var completeList = radioList.ToDictionary(entry => entry.Id);

            foreach (var entry in sdtvList)
            {
                completeList.Add(entry.Id, entry);
            }

            foreach (var entry in hdtvList)
            {
                completeList.Add(entry.Id, entry);
            }


            //now write all of this bollocks into the database, but only if it doesn't exist yet
            //TODO: actual database writes instead of writing to a file?
            WriteCrawledContentToDatabase(completeList);
        }

        private void WriteCrawledContentToDatabase(IDictionary<string, ContentInformation> crawledContent)
        {
            var connectionString =
                $"Server={_config["MySqlServerAddress"]};Database={_config["MySqlServerDatabase"]};Uid={_config["MySqlServerUser"]};Pwd={_config["MySqlServerPassword"]};";
            using var conn = new MySqlConnection(connectionString);

            conn.Open();

            using (var checkForTableCommand =
                new MySqlCommand(
                    $"SELECT count(*) FROM information_schema.tables WHERE table_schema = '{_config["MySqlServerDatabase"]}' AND table_name = '{_config["MySqlServerTableName"]}'",
                    conn))
            {
                using (var reader = checkForTableCommand.ExecuteReader())
                {
                    reader.Read();
                    //If table doesn't exist
                    if (reader.GetInt32(0) != 1)
                    {
                        throw new Exception(
                            "There is no sundtekcontent table in the mediaserver database! Please run the create_sundtekcontent_table.sql script and then try again.");
                    }
                }
            }

            using (var clearTableCommand = new MySqlCommand($"TRUNCATE TABLE {_config["MySqlServerTableName"]}", conn))
            {
                clearTableCommand.ExecuteNonQuery();
            }

            var adapter = new MySqlDataAdapter {SelectCommand = new MySqlCommand("SELECT * FROM sundtekcontent", conn)};
            // ReSharper disable once ObjectCreationAsStatement
            //Has to stay like this so we can have the commands other than Select built
            new MySqlCommandBuilder(adapter);


            var dataSet = PrepareDataSet(crawledContent);

            //Add table mapping
            adapter.TableMappings.Add("sundtekcontent", "SundtekDataTable");
            //Add column mappings
            adapter.TableMappings[0].ColumnMappings.Add("ID", "ID");
            adapter.TableMappings[0].ColumnMappings.Add("Name", "Name");
            adapter.TableMappings[0].ColumnMappings.Add("Category", "Category");
            adapter.TableMappings[0].ColumnMappings.Add("Tunersource", "Tunersource");
            adapter.TableMappings[0].ColumnMappings.Add("LiveStream", "LiveStream");
            adapter.TableMappings[0].ColumnMappings.Add("Imagelocation", "Imagelocation");
            adapter.TableMappings[0].ColumnMappings.Add("Link", "Link");

            adapter.Update(dataSet, "SundtekDataTable");
        }

        private static DataSet PrepareDataSet(IDictionary<string, ContentInformation> crawledContent)
        {
            var dataSet = new DataSet();

            //Create the table structure
            var dataTable = new DataTable("SundtekDataTable");
            dataTable.Columns.Add(new DataColumn("ID", typeof(string)));
            dataTable.Columns.Add(new DataColumn("Name", typeof(string)));
            dataTable.Columns.Add(new DataColumn("Category", typeof(string)));
            dataTable.Columns.Add(new DataColumn("Tunersource", typeof(ulong)));
            dataTable.Columns.Add(new DataColumn("LiveStream", typeof(ulong)));
            dataTable.Columns.Add(new DataColumn("Imagelocation", typeof(string)));
            dataTable.Columns.Add(new DataColumn("Link", typeof(string)));

            //Add content
            foreach (var (key, value) in crawledContent)
            {
                dataTable.Rows.Add(key, value.Name, value.Category, value.TunerIsSource,
                    true,
                    value.ImageLocation.ToString(), value.ContentLocation.ToString());
            }

            dataSet.Tables.Add(dataTable);
            return dataSet;
        }
    }
}