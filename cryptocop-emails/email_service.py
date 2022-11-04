import pika
import requests
import json
from time import sleep
import os

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
email_queue_name = 'email_queue'
email_info = '<h2>Thank you for ordering!</h2><p>We hope you will enjoy our lovely product</p><p3>%s</p3>'
email_table = '<table><thead><tr style="background-color: rgba(155, 155, 155, .2)"><th>Description</th><th> ID </th><th>Unit price (USD) </th><th> Quantity </th><th> Row price (USD) </th></tr></thead><tbody>%s</tbody></table>'

# Declare the exchange, if it doesn't exist
channel.exchange_declare(exchange=exchange_name, exchange_type='topic', durable=True)
# Declare the queue, if it doesn't exist
channel.queue_declare(queue=email_queue_name, durable=True)
# Bind the queue to a specific exchange with a routing key
channel.queue_bind(exchange=exchange_name, queue=email_queue_name, routing_key=create_order_routing_key)

def send_simple_message(to, subject, body):
    return requests.post(
        "https://api.mailgun.net/v3/sandboxe9858b796dea4d9ab336e605c5e4f132.mailgun.org/messages",
        auth=("api", "a275ae1e2a9493640091b87c3fd9efce-b0ed5083-1c7b74ba"),
        data={"from": "Mailgun Sandbox <postmaster@sandboxe9858b796dea4d9ab336e605c5e4f132.mailgun.org>",
              "to": to,
              "subject": subject,
              "html": body})

def send_order_email(ch, method, properties, data):
    parsed_msg = json.loads(data)
    header = ''.join("<p>Name: %s</p><p>StreetName: %s</p><p>HouseNumber: %s</p><p>City: %s</p><p>ZipCode: %s</p><p>Country: %s</p><p>Order Date: %s</p>" % (parsed_msg['FullName'], parsed_msg['StreetName'], parsed_msg['HouseNumber'], parsed_msg['City'], parsed_msg['ZipCode'],parsed_msg['Country'], parsed_msg['OrderDate']))
    items_html = ''.join(["<tr><td>%s</td><td>%d</td><td>%.2f</td><td>%.2f</td><td>%.2f</td></tr>" % (item['ProductIdentifier'], item['Id'], item['UnitPrice'], item['Quantity'],item['TotalPrice']) for item in parsed_msg['OrderItems']])
    representation = email_info % header
    representation += email_table % items_html + f"<p>Total: {parsed_msg['TotalPrice']} USD</p>"
    send_simple_message(parsed_msg['Email'], 'Successful order!', representation)

channel.basic_consume(on_message_callback=send_order_email,
                      queue=email_queue_name,
                      auto_ack=True)

channel.start_consuming()
connection.close()