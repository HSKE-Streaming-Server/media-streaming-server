using System.IO;
using System.Linq;
using MediaInput;
using NUnit.Framework;

namespace MediaInput.Test
{
    public class M3UParserTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CorrectForRadioM3U()
        {
            Stream input = File.OpenRead("./Artefacts/radio.m3u");
            var parsedOutput = M3UParser.ParsePlaylist(input);
            
            Assert.IsNotNull(parsedOutput);

            var enumerable = parsedOutput.ToList();
            Assert.AreEqual(78, enumerable.Count());
            
            var firstElement = enumerable.ElementAt(0);

            Assert.AreEqual("http://sundtek.de/picons/?g=1LIVE", firstElement.Item1);
            Assert.AreEqual("1LIVE", firstElement.Item2);
            Assert.AreEqual("http://localhost:22000/stream/1LIVE", firstElement.Item3);

            var secondElement = enumerable.ElementAt(1);
            
            Assert.AreEqual("http://sundtek.de/picons/?g=B5%20aktuell", secondElement.Item1);
            Assert.AreEqual("B5 aktuell", secondElement.Item2);
            Assert.AreEqual("http://localhost:22000/stream/B5%20aktuell", secondElement.Item3);
        }
        //TODO: write same test for television playlists
    }
}