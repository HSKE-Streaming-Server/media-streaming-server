using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json.Serialization;
using API.Login;
using API.Manager;
using API.Model;
using API.Model.Request;
using API.Model.Response;
using MediaInput;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly LoginDbHandler _authHandler;
        private readonly ILogger<MainController> _logger;

        public MainController(ServerManager serverManager, ILogger<MainController> logger, LoginDbHandler authHandler)
        {
            //This constructor is called for every request. Therefore, keep it slim
            _serverManager = serverManager;
            //Save injected logger
            _logger = logger;
            _authHandler = authHandler;
            _logger.LogTrace($"{nameof(MainController)} initialized");
        }


        //Return whether or not token is valid and if it is, return the username of the user it belongs to.
        [HttpPost("authenticate")]
        public ActionResult<AuthenticateResponse> PostAuthenticate(string token)
        {
            _logger.LogTrace($"{Request.HttpContext.Connection.RemoteIpAddress}: POST {Request.Host}{Request.Path}");
            var username = _authHandler.CheckToken(token);
            return new AuthenticateResponse(username);
        }

        [HttpPost("login")]
        public ActionResult<LoginResponse> PostLogin(Account account)
        {
            _logger.LogTrace($"{Request.HttpContext.Connection.RemoteIpAddress}: POST {Request.Host}{Request.Path}");
            var token = _authHandler.LoginUser(account);
            return new LoginResponse(token);
        }

        [HttpPost("logout")]
        public ActionResult<LogoutResponse> PostLogout(string token)
        {
            _logger.LogTrace($"{Request.HttpContext.Connection.RemoteIpAddress}: POST {Request.Host}{Request.Path}");
            _authHandler.LogoutUser(token);
            return new LogoutResponse(true);
        }

        [HttpPost("categories")]
        public ActionResult<IEnumerable<string>> PostCategories(string token)
        {
            _logger.LogTrace($"{Request.HttpContext.Connection.RemoteIpAddress}: POST {Request.Host}{Request.Path}");
            _authHandler.CheckToken(token);
            //ToList required because of an interface limitation of C#
            return _serverManager.GetSources().ToList();
        }

        [HttpPost("media")]
        public ActionResult<IEnumerable<ContentInformation>> PostMedia(MediaRequest request)
        {
            _logger.LogTrace($"{Request.HttpContext.Connection.RemoteIpAddress}: POST {Request.Host}{Request.Path}");
            _authHandler.CheckToken(request.Token);
            return _serverManager.GetMedia(request.Category).ToList();
        }

        [HttpPost("stream")]
        public ActionResult<StreamResponse> PostStream(StreamRequest streamRequest)
        {
            _logger.LogTrace($"{Request.HttpContext.Connection.RemoteIpAddress}: POST {Request.Host}{Request.Path}");
            _authHandler.CheckToken(streamRequest.Token);
            return _serverManager.GetStream(streamRequest.StreamId, streamRequest.Settings.VideoPresetId,
                streamRequest.Settings.AudioPresetId);
        }

        [HttpPost("presets")]
        public PresetResponse PostPresets(string token)
        {
            _logger.LogTrace($"{Request.HttpContext.Connection.RemoteIpAddress}: POST {Request.Host}{Request.Path}");
            _authHandler.CheckToken(token);
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
            _authHandler.CheckToken(request.Token);
            _serverManager.KeepAlive(request);
            return Ok();
            //Todo adjust return
        }
        
        [HttpPost("detail")]
        public ActionResult<ContentInformation> GetDetail(DetailRequest request)
        {
            _logger.LogTrace($"{Request.HttpContext.Connection.RemoteIpAddress}: POST {Request.Host}{Request.Path}");
            _authHandler.CheckToken(request.Token);
            return _serverManager.GetDetail(request);
        }
    }
}