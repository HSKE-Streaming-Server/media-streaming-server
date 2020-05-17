using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Model.Request
{
    public class KeepAliveRequest
    {
        public string Token { get; set; }
        public int AudioPreset { get; set; }
        public int VideoPreset { get; set; }
        public string VideoID { get; set; }
    }
}
