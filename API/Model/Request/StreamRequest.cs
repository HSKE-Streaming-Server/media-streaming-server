using System.ComponentModel.DataAnnotations;
namespace API.Model.Request
{
    public struct StreamRequest
    {
        //[RequiredAttribute]
        public string Token { get; set; }
        [RequiredAttribute]
        public string stream_id { get; set; }
        [RequiredAttribute]
        public StreamSettings Settings { get; set; }
    }
}