using System.ComponentModel.DataAnnotations;
namespace API.Model.Request
{
    public struct MediaRequest{

        //[RequiredAttribute]
        public string Token {get; set;}

        [RequiredAttribute]
        public string Category {get; set;}

    }
}