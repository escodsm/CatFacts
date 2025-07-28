using System;
using System.Text.Json.Serialization;

namespace CatFacts.Models
{
    public class CatFact
    {
        [JsonPropertyName("fact")]
        public string Text { get; set; }

        [JsonPropertyName("used")]
        public bool Used { get; set; }

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }

        // Not from API, used internally
        public string LogStatus { get; set; }
    }
}
