import paho.mqtt.client as mqtt
import json, time, random, sys, uuid, psycopg

device_id = sys.argv[1] if len(sys.argv) > 1 else "0"
topic = f"devices/tank/{device_id}"

client = mqtt.Client(client_id=str(uuid.uuid4()), transport="tcp")
client.username_pw_set("mqthanggg", "admin123")
client.tls_set(
    ca_certs="../certs/ca.crt",
    certfile="../certs/client.crt",
    keyfile="../certs/client.key"
)
client.connect("localhost", 8883)
client.loop_start()

tank_max_volume = 0
with psycopg.connect("host=localhost port=5432 user=read_user password=read123 dbname=Intern") as conn:
    with conn.cursor() as cursor:
        cursor.execute("""
            SELECT 
                max_volume
            FROM petro_application.tank
            WHERE tank_id = %s
        """,
        [int(device_id)])
        tank_max_volume = cursor.fetchone()[0]

try:
    while True:
        liter = 0
        current_volume = tank_max_volume
        payload = {
            "current_volume": current_volume
        }
        client.publish(topic, json.dumps(payload), retain=True)
        
        while current_volume > 0:
            current_volume = current_volume - 12
            if current_volume < 0:
                current_volume = 0
            payload = {
                "current_volume": current_volume
            }
            client.publish(topic, json.dumps(payload), retain=True)
            
            time.sleep(random.randint(10,20))
    
except KeyboardInterrupt:
    client.loop_stop()
    client.disconnect()