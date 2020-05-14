namespace API.Model.Request
{
    public struct StreamRequest
    {
        public string Token { get; set; }
        public string StreamId { get; set; }
        public StreamSettings Settings { get; set; }
    }
}