using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using hsk_media_server.Model;

namespace hsk_media_server.Controllers
{
    [Route("")]
    [ApiController]
    public class DefaultController : Controller
    {

        [HttpGet("{alias}")]
        public JsonResult Get(string alias)
        {
            //logik -> Klasse
            JSONstringALSklasse EinObject = new JSONstringALSklasse();
            EinObject.Name = alias;
            return Json(EinObject.Name);
        }


        [HttpPost("authenticate")]
        public JsonResult PostAuthenticate(Account account){
            //TODO: check Account, create Token
            return Json("hier dein Token! username: " + account.username + " password: " + account.password);           
        }

        [HttpPost("source")]
        public JsonResult PostSource(SourceRequest sourceRequest){
            //TODO: get sources with same type
            return Json("sources mit type: " + sourceRequest.type);        
        }

        [HttpGet("presets")]
        public JsonResult GetPresets()
        {
            //TODO: get all presets
            return Json("presets");
        }

    }
}