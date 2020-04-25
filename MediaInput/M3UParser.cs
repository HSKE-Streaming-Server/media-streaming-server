using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Data;


namespace MediaInput
{
    internal class M3UParser
    {
        //TODO: This is very very dumb code and I need someone smart to replace it for me -n.st
        
        
        /// <summary>
        /// Returns a triplet of preview picture URI, channel name and location.
        /// </summary>
        /// <param name="playlist">The stream that contains the playlist file, which was grabbed from the Sundtek server.</param>
        /// <returns>Preview picture URI, channel name and location, in that order.</returns>
        /// <exception cref="Exception"><paramref name="playlist"/> does not contain a proper playlist.</exception>
        /// <exception cref="NullReferenceException"><paramref name="playlist"/> is null.</exception>
        public static IEnumerable<Tuple<string, string, string>> ParsePlaylist(Stream playlist)
        {
            if(playlist==null)
                throw new ArgumentNullException(nameof(playlist), "Parameter playlist can't be null");
            
            StreamReader reader = new StreamReader(playlist);
            
            //EXTM3U is required as first line
            var firstLine = reader.ReadLine();
            if (firstLine != "#EXTM3U")
                throw new Exception("Not a valid M3U file");
            
            //Every entry consists of 3 lines, and we can ignore the second line
            while (!reader.EndOfStream)
            {
                string metadata = reader.ReadLine();
                //ignore second line
                reader.ReadLine();
                string link = reader.ReadLine();

                //Form of first line is: #EXTINF:-1 tvg-logo="http://sundtek.de/picons/?g=arte",arte
                //First remove the "#EXTINF:-1 "
                if (metadata != null && link != null)
                {
                    metadata = metadata.Remove(0, 11);
                    string[] splitMetadata = metadata.Split(',', 3);
                    //We have to find the first and second '"' so we can extract the link to the preview picture
                    IEnumerable<int> apposPos = splitMetadata[0].AllIndicesOf("\"");
                    var enumerable = apposPos as int[] ?? (apposPos ?? throw new Exception("Playlist didn't contain a link to a preview picture.")).ToArray();
                    //splitMetadata[2] should just be the channel name in clear
                    yield return new Tuple<string, string, string>(splitMetadata[0].Substring(enumerable[0]+1, enumerable[1]-enumerable[0]-1),
                        splitMetadata[1], link);
                }
                else
                {
                    throw new Exception("M3U file was in a different format than expected");
                }
            }
        }
    }
}