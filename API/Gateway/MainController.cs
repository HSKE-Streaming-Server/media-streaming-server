using System;
using System.Collections.Generic;
using API.Manager;
using API.Model;
using API.Model.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Transcoder;

namespace API.Gateway
{

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
        public JsonResult PostCategories(string token)
        {
            //TODO: AccountManager.checkToken(token)
            //TODO: impl
            return Json(_serverManager.GetSources());
        }

        [HttpPost("media")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] //if category doesnt exist
        [ProducesResponseType(StatusCodes.Status401Unauthorized)] //if token invalid
        public JsonResult PostMedia(MediaRequest mediaRequest)
        {
            //TODO: AccountManager.checkToken(token)
            return Json(_serverManager.GetMedia(mediaRequest.Source));
        }

        [HttpPost("stream")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] //if either video or audio or streamid are invalid or non-existent
        [ProducesResponseType(StatusCodes.Status401Unauthorized)] //if token invalid
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)] //if tuner is already in use and tuner content is requested 
        public JsonResult PostStream(StreamRequest streamRequest)
        {
            //TODO: AccountManager.checkToken(token)
            return Json(_serverManager.GetStream(streamRequest.StreamId, streamRequest.Settings.VideoPresetId, streamRequest.Settings.AudioPresetId));
        }

        [HttpPost("presets")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public JsonResult PostPresets(string token)
        {
            //TODO: AccountManager.checkToken(token)
            var returnval = _serverManager.GetVideoPresets();
            var json = Json(returnval);
            return json;
            /*return new Dictionary<string, IEnumerable<Preset>>
            {
                {"video", _serverManager.GetVideoPresets()},
                {"audio", _serverManager.GetAudioPresets()}
            };*/
        }

    }
}