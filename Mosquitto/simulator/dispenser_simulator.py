import paho.mqtt.client as mqtt
import json, time, random, sys, uuid, psycopg

device_id = sys.argv[1] if len(sys.argv) > 1 else "0"
topic = f"devices/dispenser/{device_id}"

client = mqtt.Client(client_id=str(uuid.uuid4()), transport="tcp")
client.username_pw_set("mqthanggg", "admin123")
client.tls_set(
    ca_certs="../certs/ca.crt",
    certfile="../certs/client.crt",
    keyfile="../certs/client.key"
)
client.connect("localhost", 8883)
client.loop_start()

fuel_price = 0

with psycopg.connect("host=localhost port=5432 user=read_user password=read123 dbname=Intern") as conn:
    with conn.cursor() as cursor:
        cursor.execute("""
            SELECT 
                f.price
            FROM petro_application.dispenser as d
            INNER JOIN petro_application.fuel as f
            ON 
                d.dispenser_id = %s
            AND
                f.fuel_id = d.fuel_id
        """,
        [int(device_id)])
        fuel_price = cursor.fetchone()[0]

try:
    while True:
        liter = 0
        current_price = 0
        selected_price_limit = random.randint(1,10)*10000
        payload = {
            "liter": 0.00,
            "price": 0,
            "state": 0
        }
        time.sleep(random.randint(1,5))
        client.publish(topic, json.dumps(payload), retain=True)
        while current_price < selected_price_limit:
            liter = round(liter + 0.032,3)
            current_price = int(round(fuel_price * liter,0))
            if current_price > selected_price_limit:
                current_price = selected_price_limit
                liter = round(current_price / fuel_price,3)
            payload = {
                "liter": liter,
                "price": current_price,
                "state": 1
            }
            client.publish(topic, json.dumps(payload), retain=True)

            time.sleep(0.05)
        time.sleep(2)
        payload = {
            "liter": 0.00,
            "price": 0,
            "state": 2,
            "payment": random.randint(1,4)
        }
        time.sleep(1)
        client.publish(topic, json.dumps(payload), retain=True)
    
except KeyboardInterrupt:
    client.loop_stop()
    client.disconnect()