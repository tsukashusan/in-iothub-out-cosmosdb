using Newtonsoft.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public class EventHubTrigger1
    {
        private readonly ILogger _logger;

        public EventHubTrigger1(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<EventHubTrigger1>();
        }

        [Function("EventHubTrigger1")]
        [CosmosDBOutput(databaseName: "%OutputDatabase%", containerName: "%OutputContainerName%", Connection = "CosmosDBConnectionString", PartitionKey = "/deviceid", CreateIfNotExists = true)]
        public object Run([EventHubTrigger("samples-workitems", Connection = "EventHubConnectionAppSetting")] IReadOnlyList<MyDocument> input)
        {
            _logger.LogInformation($"First Event Hubs triggered message: {input[0]}");
            return input.Select(p => new
            {
                id = p.Id,
                messageId = p.messageId,
                deviceId = p.deviceId,
                temperature = p.temperature,
                humidity = p.humidity,
                send_datetime = p.send_datetime
            }); ;
        }
    }
    [JsonObject("Telemetry")]
    public class MyDocument
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("messageId")]
        public ulong messageId { get; set; }

        [JsonProperty("deviceId")]
        public string deviceId { get; set; }

        [JsonProperty("temperature")]
        public double temperature { get; set; }

        [JsonProperty("humidity")]
        public double humidity { get; set; }
        [JsonProperty("send_datetime")]
        public DateTime send_datetime { get; set; }
    }
}
