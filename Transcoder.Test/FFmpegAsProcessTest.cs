using System;
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
            _ffmpeg = FFmpegAsProcess.GetSingleton();
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
            var returnval =
                _ffmpeg.StartProcess(
                    new Uri(
                        "http://zdf-hls-01.akamaized.net/hls/live/2002460/de/6225f4cab378772631347dd27372ea68/5/5.m3u8"),
                    1, 1);
            Thread.Sleep(60000);
        }
    }
}