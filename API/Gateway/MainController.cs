using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using API.Manager;
using API.Model;
using API.Model.Request;
using MediaInput;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Ess;
using Transcoder;

namespace API.Gateway
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public struct PresetResponse
    {
        public IEnumerable<VideoPreset> videoPresets { get; set; }
        public IEnumerable<AudioPreset> audioPresets { get; set; }
    }


    [Route("")]
    [ApiController]
    public class MainController : Controller
    {
        private readonly ServerManager _serverManager;

        public MainController(ServerManager serverManager)
        {
            _serverManager = serverManager;
        }


        //Return whether or not token is valid and if it is, return the username of the user it belongs to.
        [HttpPost("authenticate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public JsonResult PostAuthenticate(string token)
        {
            throw new NotImplementedException();
            //return Json(_serverManager.Authenticate(account));
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public JsonResult PostLogin(Account account)
        {
            //TODO: check login credentials, if bad return 400
            throw new NotImplementedException();
        }

        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public JsonResult PostLogout(string token)
        {
            //When would you ever return false to this? Maybe when the token didn't exist in the first place?
            throw new NotImplementedException();
        }

        [HttpPost("categories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)] //if token invalid
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] //if mysql is misconfigured in grabberconf or database is incorrect
        public ActionResult<IEnumerable<string>> PostCategories(string token)
        {
            //TODO: AccountManager.checkToken(token)
            //TODO: impl
            try
            {
                //ToList required because of an interface limitation of C#
                return _serverManager.GetSources().ToList();
            }
            catch (MySqlException mySqlException)
            {
                return StatusCode(500);
            }
        }

        [HttpPost("media")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)] //if token invalid
        [ProducesResponseType(StatusCodes.Status400BadRequest)] //if request null
        public ActionResult<IEnumerable<ContentInformation>> PostMedia(MediaRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Category) ||
                string.IsNullOrWhiteSpace(request.Token))
                return BadRequest();
                
            //TODO: AccountManager.checkToken(token)
            //var tok = Request.Form["token"];

            return _serverManager.GetMedia(request.Category).ToList();
        }

        [HttpPost("stream")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] //if either video or audio or streamid are invalid or non-existent
        [ProducesResponseType(StatusCodes.Status401Unauthorized)] //if token invalid
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)] //if tuner is already in use and tuner content is requested 
        public ActionResult<Tuple<Uri, int, int>> PostStream(StreamRequest streamRequest)
        {
            if (string.IsNullOrWhiteSpace(streamRequest.Token) ||
                string.IsNullOrWhiteSpace(streamRequest.StreamId))
                return BadRequest();
            //TODO: AccountManager.checkToken(token)
            return _serverManager.GetStream(streamRequest.StreamId, streamRequest.Settings.VideoPresetId,
                streamRequest.Settings.AudioPresetId);
        }

        [HttpPost("presets")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public PresetResponse PostPresets(string token)
        {
            //TODO: AccountManager.checkToken(token)
            return new PresetResponse
            {
                audioPresets = _serverManager.GetAudioPresets(), videoPresets = _serverManager.GetVideoPresets()
            };
            /*return new Dictionary<string, IEnumerable<Preset>>
            {
                {"video", _serverManager.GetVideoPresets()},
                {"audio", _serverManager.GetAudioPresets()}
            };*/
        }
    }
}