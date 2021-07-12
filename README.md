# aiskafka-af

Azure Function sample with 2 triggers:
- EventHubTrigger: will be triggered when a message is posted into an Event Hub, and will forward the message to a Kafka Topic
- KafkaTrigger: will be triggered when a message is posted into a Kafka Topic, and will forward the message to an Event Hub

This Azure Function uses the Kafka Extention: https://github.com/Azure/azure-functions-kafka-extension
