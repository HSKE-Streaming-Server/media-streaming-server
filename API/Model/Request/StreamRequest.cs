namespace API.Model.Request
{
    public struct StreamRequest
    {
        public string Token { get; set; }
        public string stream_id { get; set; }
        public StreamSettings Settings { get; set; }
    }
}