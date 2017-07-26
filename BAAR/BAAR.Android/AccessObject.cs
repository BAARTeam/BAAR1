using Newtonsoft.Json;


    public class AccessObject
    {
        [JsonProperty("access_token")]
        public string AccessToken;
        [JsonProperty("token_type")]
        public string Bearer;
        [JsonProperty("expires_in")]
        public string Expiration;
    }
