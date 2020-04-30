using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using hsk_media_server.Model;
using hsk_media_server.Manager;

namespace hsk_media_server.Controllers
{

    [Route("")]
    [ApiController]
    public class DefaultController : Controller
    {

        private readonly ServerManager _serverManager;

        public DefaultController(ServerManager serverManager){
            _serverManager = serverManager;
        }

        [HttpPost("authenticate")]
        public JsonResult PostAuthenticate(Account account){
            return Json(_serverManager.authenticate(account));           
        }

        [HttpPost("source")]
        public JsonResult PostSource(SourceRequest sourceRequest){
            //TODO: AccountManager.checkToken(token)
            return Json(_serverManager.getSources(sourceRequest.type));       
        }

        [HttpPost("media")]
        public JsonResult PostMedia(MediaRequest mediaRequest){
            //TODO: AccountManager.checkToken(token)
            return Json(_serverManager.getMedia(mediaRequest.source));       
        }

        [HttpPost("stream")]
        public JsonResult PostStream(StreamRequest streamRequest){
            //TODO: AccountManager.checkToken(token)
            return Json(_serverManager.getStream(streamRequest.streamId));     
        }

        [HttpGet("presets")]
        public JsonResult GetPresets()
        {
            //TODO: AccountManager.checkToken(token)
            return Json(_serverManager.getPresets());
        }

    }
}