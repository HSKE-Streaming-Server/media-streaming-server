/*using System;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NUnit.Framework;

namespace Transcoder.Test
{
    public class FFmpegAsProcessTest
    {
        private FFmpegAsProcess _ffmpeg;

        [SetUp]
        public void Setup()
        {
            throw new NotImplementedException();
        }


        //TODO: Mock config file for test and inject it, then test if these presets are parsed correctly
        [Test]
        public void GetAvailableVideoPresetsCorrect()
        {
            var presets = _ffmpeg.GetAvailableVideoPresets();
            Assert.AreEqual(1, presets.Count());
        }

        [Test]
        public void GetAvailableAudioPresetsCorrect()
        {
        }

        [Test]
        public void StartProcessCorrect()
        {
            _ffmpeg.StartProcess(
                new Uri(
                    "http://localhost:22000/stream/ARD%2Dalpha"),
                1, 1);
            Thread.Sleep(60000);
        }
    }
}*/