echo "Start Zookeeper"
c:\Kafka\bin\windows\zookeeper-server-start.bat c:\Kafka\config\zookeeper.Properties

echo "Start Kafka Cluster"
c:\Kafka\bin\windows\kafka-server-start.bat c:\Kafka\config\server.Properties

echo "{ "hello":"world" }" | c:\Kafka\bin\windows\kafka-console-producer.bat --broker-list x.x.x.x:9092 --topic outbound