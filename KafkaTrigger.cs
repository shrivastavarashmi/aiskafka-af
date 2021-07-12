using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

// added
using Microsoft.Azure.WebJobs.Extensions.Kafka;
using Newtonsoft.Json;

namespace aiskafka.Function
{
    public static class KafkaTrigger
    {

        [FunctionName("KafkaTrigger")]
        public static async Task Run(
            [KafkaTrigger("kafkaBroker", "kafkaOutboundTopic", ConsumerGroup = "$Default", 
            AuthenticationMode = BrokerAuthenticationMode.Plain)] KafkaEventData<string>[] events,
            [EventHub("outkafka", Connection = "EventHuboutKafka")]IAsyncCollector<string> outputEvents,
            ILogger log)
        {
            var exceptions = new List<Exception>();

            foreach (var kafkaEvent in events)
            {
                try
                {
                    string messageBody = kafkaEvent.Value.ToString();

                    log.LogInformation($"C# Kafka processed a message: {messageBody}");

                    try{
                    
                        await outputEvents.AddAsync(JsonConvert.SerializeObject(messageBody));
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Are you sure the Event Hub exists?", ex);
                    }
                }
                catch (Exception e)
                {
                    // We need to keep processing the rest of the batch - capture this exception and continue.
                    // Also, consider capturing details of the message that failed processing so it can be processed again later.
                    exceptions.Add(e);
                }
            }

            // Once processing of the batch is complete, if any messages in the batch failed processing throw an exception so that there is a record of the failure.

            if (exceptions.Count > 1)
                throw new AggregateException(exceptions);

            if (exceptions.Count == 1)
                throw exceptions.Single();
        }
    }
}
