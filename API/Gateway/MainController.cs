using API.Manager;
using API.Model;
using API.Model.Request;
using Microsoft.AspNetCore.Mvc;

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
        //
        [HttpPost("authenticate")]
        public JsonResult PostAuthenticate(Account account)
        {
            return Json(_serverManager.Authenticate(account));
        }

        [HttpPost("source")]
        public JsonResult PostSource(SourceRequest sourceRequest)
        {
            //TODO: AccountManager.checkToken(token)
            return Json(_serverManager.GetSources(sourceRequest.Type));
        }

        [HttpPost("media")]
        public JsonResult PostMedia(MediaRequest mediaRequest)
        {
            //TODO: AccountManager.checkToken(token)
            return Json(_serverManager.GetMedia(mediaRequest.Source));
        }

        [HttpPost("stream")]
        public JsonResult PostStream(StreamRequest streamRequest)
        {
            //TODO: AccountManager.checkToken(token)
            return Json(_serverManager.GetStream(streamRequest.StreamId, streamRequest.Settings.VideoPresetId, streamRequest.Settings.AudioPresetId));
        }

        [HttpGet("presets")]
        public JsonResult GetPresets()
        {
            //TODO: AccountManager.checkToken(token)
            return Json(ServerManager.GetPresets());
        }

    }
}