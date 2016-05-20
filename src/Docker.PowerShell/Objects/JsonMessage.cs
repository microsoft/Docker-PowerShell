using Newtonsoft.Json;

namespace Docker.PowerShell.Objects
{
    internal class JsonMessage
    {
        [JsonProperty(PropertyName ="stream")]
        public string Stream { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "progressDetail")]
        public JsonProgress Progress { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string ID { get; set; }

        [JsonProperty(PropertyName = "from")]
        public string From { get; set; }

        [JsonProperty(PropertyName = "time")]
        public long Time { get; set; }

        [JsonProperty(PropertyName = "timeNano")]
        public long TimeNano { get; set; }

        [JsonProperty(PropertyName = "errorDetail")]
        public JsonError Error { get; set; }
    }

    internal class JsonProgress
    {
        [JsonProperty(PropertyName = "current")]
        public long Current { get; set; }

        [JsonProperty(PropertyName = "total")]
        public long Total { get; set; }

        [JsonProperty(PropertyName = "start")]
        public long Start { get; set; }
    }

    internal class JsonError
    {
        [JsonProperty(PropertyName = "code")]
        public long Code { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
    }
}