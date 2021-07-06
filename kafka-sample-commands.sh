# Create Kafka Topic
~/kafka/bin/kafka-topics.sh --create --zookeeper localhost:2181 --replication-factor 1 --partitions 1 --topic inbound
~/kafka/bin/kafka-topics.sh --create --zookeeper localhost:2181 --replication-factor 1 --partitions 1 --topic outbound

# Send Message to Kafka Topic outbound
echo "Hello, World" | ~/kafka/bin/kafka-console-producer.sh --broker-list localhost:9092 --topic outbound > /dev/null

# Receive Message from Kafka Topic
~/kafka/bin/kafka-console-consumer.sh --bootstrap-server localhost:9092 --topic inbound --from-beginning

# KafkaT
kafkat partitions

# No. of Messages per Kafka Topic
~/kafka/bin/kafka-run-class.sh kafka.tools.GetOffsetShell --broker-list localhost:9092 --topic outbound --time -1
~/kafka/bin/kafka-run-class.sh kafka.tools.GetOffsetShell --broker-list localhost:9092 --topic inbound --time -1

# Check Kafka Connection
kafkacat -b x.x.x.x:9092 -t inbound