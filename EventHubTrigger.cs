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
using System.IO;

namespace aiskafka.Function
{
    public static class EventHubTrigger
    {
        
        [FunctionName("EventHubTrigger")]
        public static async Task Run(
            [EventHubTrigger("inkafka", Connection = "EventHubinKafka")] EventData[] events, 
            [Kafka("kafkaBroker", "kafkaInboundTopic")] IAsyncCollector<KafkaEventData<string>> outputEvents,
            ILogger log)
        {
            var exceptions = new List<Exception>();

            foreach (EventData eventData in events)
            {
                try
                {
                    string messageBody = Encoding.UTF8.GetString(eventData.Body.Array, eventData.Body.Offset, eventData.Body.Count);

                    // Replace these two lines with your processing logic.
                    // log.LogInformation($"C# Event Hub trigger function processed a message: {messageBody}");
                    // await Task.Yield();
                    try
                    {
                        var kafkaEvent = new KafkaEventData<string>()
                        {
                            Value = await new StreamReader(messageBody).ReadToEndAsync(),
                        };

                        await outputEvents.AddAsync(kafkaEvent);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Are you sure the topic exists? To create using Confluent Docker quickstart run this command: 'docker-compose exec broker kafka-topics --create --zookeeper zookeeper:2181 --replication-factor 1 --partitions 10 --topic stringTopicTenPartitions'", ex);
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
