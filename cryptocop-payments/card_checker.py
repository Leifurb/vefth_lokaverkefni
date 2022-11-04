from credit_card_checker import CreditCardChecker
import pika
import json
from time import sleep

while True:
    try:
        connection = pika.BlockingConnection(pika.ConnectionParameters('rabbitmq', '5672'))
        break
    except:
        sleep(5)
        continue
channel = connection.channel()
exchange_name = 'order_exchange'
create_order_routing_key = 'create-order'
queue_name = 'card_checker_queue'

# Declare the exchange, if it doesn't exist
channel.exchange_declare(exchange=exchange_name, exchange_type='topic', durable=True)
# Declare the queue, if it doesn't exist
channel.queue_declare(queue=queue_name, durable=True)
# Bind the queue to a specific exchange with a routing key
channel.queue_bind(exchange=exchange_name, queue=queue_name, routing_key=create_order_routing_key)

def check_card(ch, method, properties, data):
    parsed_msg = json.loads(data)
    if CreditCardChecker(parsed_msg['CreditCard']).valid():
        print(f"Card Number {parsed_msg['CreditCard']} is valid")
    else:
        print(f"Card Number {parsed_msg['CreditCard']} is not valid")

channel.basic_consume(on_message_callback=check_card,
                      queue=queue_name,
                      auto_ack=True)

channel.start_consuming()
connection.close()

