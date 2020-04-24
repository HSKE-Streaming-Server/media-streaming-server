using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using hsk_media_server.Model;

namespace hsk_media_server.Controllers
{
    [Route("v1/default")]
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
    }
}