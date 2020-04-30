namespace hsk_media_server.Model
{
    public class StreamRequest{

        public string token {get; set;}

        public string streamId {get; set;}

        public class Settings{

            public int videoPresetId {get; set;}

            public int audioPresetId {get; set;}
        }

    }
}