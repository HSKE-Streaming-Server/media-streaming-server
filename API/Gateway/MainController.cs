using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Text.Json.Serialization;
using API.Manager;
using API.Model;
using API.Model.Request;
using APIExceptions;
using MediaInput;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using Transcoder;

namespace API.Gateway
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public struct PresetResponse
    {
        [JsonPropertyName("videoPresets")]
        public IEnumerable<VideoPreset> VideoPresets { get; set; }
        [JsonPropertyName("audioPresets")]
        public IEnumerable<AudioPreset> AudioPresets { get; set; }
    }


    [Route("")]
    [ApiController]
    public class MainController : Controller
    {
        private readonly ServerManager _serverManager;
        private readonly ILogger<MainController> _logger;

        public MainController(ServerManager serverManager, ILogger<MainController> logger)
        {
            //This constructor is called for every request. Therefore, keep it slim
            _serverManager = serverManager;
            //Save injected logger
            _logger = logger;
            _logger.LogTrace($"{nameof(MainController)} initialized");
        }


        //Return whether or not token is valid and if it is, return the username of the user it belongs to.
        [HttpPost("authenticate")]
        public JsonResult PostAuthenticate(string token)
        {
            _logger.LogTrace($"{Request.HttpContext.Connection.RemoteIpAddress}: POST {Request.Host}{Request.Path}");
            throw new NotImplementedException();
            //return Json(_serverManager.Authenticate(account));
        }

        [HttpPost("login")]
        public ActionResult<LoginResponse> PostLogin(Account account)
        {
            _logger.LogTrace($"{Request.HttpContext.Connection.RemoteIpAddress}: POST {Request.Host}{Request.Path}");
            _serverManager.Login(account);
            //TODO: check login credentials, if bad return 400
            //throw new NotImplementedException();
            //try
            //{
                return _serverManager.Login(account);
            //}
            //catch (APIBadRequestException ex)
            //{
                //return new string[] { "400" }; //TODO: richtig implementieren

            //}//TODO: wusste nicht genau

        }

        [HttpPost("logout")]
        public JsonResult PostLogout(string token)
        {
            _logger.LogTrace($"{Request.HttpContext.Connection.RemoteIpAddress}: POST {Request.Host}{Request.Path}");
            //When would you ever return false to this? Maybe when the token didn't exist in the first place?
            throw new NotImplementedException();
        }

        [HttpPost("categories")]
        public ActionResult<IEnumerable<string>> PostCategories(string token)
        {
            _logger.LogTrace($"{Request.HttpContext.Connection.RemoteIpAddress}: POST {Request.Host}{Request.Path}");
            //TODO: AccountManager.checkToken(token)
            //TODO: impl

            //ToList required because of an interface limitation of C#
            return _serverManager.GetSources().ToList();
        }

        [HttpPost("media")]
        public ActionResult<IEnumerable<ContentInformation>> PostMedia(MediaRequest request)
        {
            _logger.LogTrace($"{Request.HttpContext.Connection.RemoteIpAddress}: POST {Request.Host}{Request.Path}");

            //TODO: AccountManager.checkToken(token)
            //var tok = Request.Form["token"];

            return _serverManager.GetMedia(request.Category).ToList();
        }

        [HttpPost("stream")]
        public ActionResult<StreamResponse> PostStream(StreamRequest streamRequest)
        {
            _logger.LogTrace($"{Request.HttpContext.Connection.RemoteIpAddress}: POST {Request.Host}{Request.Path}");
            //TODO: AccountManager.checkToken(token)
            
            return _serverManager.GetStream(streamRequest.StreamId, streamRequest.Settings.VideoPresetId,
                streamRequest.Settings.AudioPresetId);
        }

        [HttpPost("presets")]
        public PresetResponse PostPresets(string token)
        {
            _logger.LogTrace($"{Request.HttpContext.Connection.RemoteIpAddress}: POST {Request.Host}{Request.Path}");
            //TODO: AccountManager.checkToken(token)
            return new PresetResponse
            {
                AudioPresets = _serverManager.GetAudioPresets(), VideoPresets = _serverManager.GetVideoPresets()
            };
            /*return new Dictionary<string, IEnumerable<Preset>>
            {
                {"video", _serverManager.GetVideoPresets()},
                {"audio", _serverManager.GetAudioPresets()}
            };*/
        }

        [HttpPost("keepalive")]
        public ActionResult KeepAlive(KeepAliveRequest request)
        {
            _logger.LogTrace($"{Request.HttpContext.Connection.RemoteIpAddress}: POST {Request.Host}{Request.Path}");
            _serverManager.KeepAlive(request);
            return Ok();
            //Todo adjust return
        }
    }
}