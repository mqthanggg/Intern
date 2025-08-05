import threading
import time
import random
import json
import uuid
import psycopg
import dotenv
import os
import paho.mqtt.client as mqtt
from typing import Dict, List
import signal
import sys
from datetime import datetime

dotenv.load_dotenv()

class MQTTSimulatorManager:
    def __init__(self):
        self.client = None
        self.running_simulators: Dict[str, threading.Thread] = {}
        self.stop_events: Dict[str, threading.Event] = {}
        self.dispenser_id_list = []
        self.tank_id_list = []
        self.fuel_prices = {}
        self.tank_max_volumes = {}
        # Payment transaction counter (total across all dispensers)
        self.total_payment_count = 0
        self.stats_lock = threading.Lock()
        self.stats_file = "payment_stats.txt"
        self._setup_mqtt()
        self._load_device_data()
        
    def _setup_mqtt(self):
        """Initialize single MQTT connection for all simulators"""
        self.client = mqtt.Client(client_id=str(uuid.uuid4()), transport="tcp")
        self.client.username_pw_set("mqthanggg", "admin123")
        self.client.tls_set(
            ca_certs="certs/ca.crt",
            certfile="certs/client.crt",
            keyfile="certs/client.key"
        )
        
        def on_connect(client, userdata, flags, rc):
            if rc == 0:
                print("✅ Connected to MQTT broker")
            else:
                print(f"❌ Failed to connect to MQTT broker: {rc}")
                
        def on_disconnect(client, userdata, rc):
            print("📡 Disconnected from MQTT broker")
            
        self.client.on_connect = on_connect
        self.client.on_disconnect = on_disconnect
        
        try:
            self.client.connect(os.getenv("MOSQUITTO_HOST"), 8883, 60)
            self.client.loop_start()
        except Exception as e:
            print(f"❌ MQTT connection failed: {e}")
            
    def _load_device_data(self):
        """Load device data from database"""
        try:
            with psycopg.connect(os.getenv("DBREAD_CONNECTION_STRING")) as conn:
                with conn.cursor() as cursor:
                    # Load dispensers and their fuel prices
                    cursor.execute("""
                        SELECT 
                            d.dispenser_id,
                            f.price
                        FROM petro_application.dispenser as d
                        INNER JOIN petro_application.fuel as f
                        ON f.fuel_id = d.fuel_id
                    """)
                    dispenser_data = cursor.fetchall()
                    self.dispenser_id_list = [row[0] for row in dispenser_data]
                    self.fuel_prices = {row[0]: row[1] for row in dispenser_data}
                    
                    # Load tanks and their max volumes
                    cursor.execute("""
                        SELECT 
                            tank_id,
                            max_volume
                        FROM petro_application.tank
                    """)
                    tank_data = cursor.fetchall()
                    self.tank_id_list = [row[0] for row in tank_data]
                    self.tank_max_volumes = {row[0]: row[1] for row in tank_data}
                    
            print(f"📊 Loaded {len(self.dispenser_id_list)} dispensers and {len(self.tank_id_list)} tanks")
            
            # Initialize or load existing payment count
            self._load_payment_stats()
        except Exception as e:
            print(f"❌ Database connection failed: {e}")
    
    def _load_payment_stats(self):
        """Load existing payment statistics from file"""
        try:
            if os.path.exists(self.stats_file):
                with open(self.stats_file, 'r') as f:
                    content = f.read().strip()
                    if content:
                        # Extract the number from the last line
                        lines = content.split('\n')
                        for line in reversed(lines):
                            if 'Total Payments:' in line:
                                self.total_payment_count = int(line.split(':')[1].strip())
                                break
                print(f"📊 Loaded existing payment count: {self.total_payment_count}")
            else:
                # Create initial stats file
                self._write_payment_stats()
        except Exception as e:
            print(f"⚠️ Could not load payment stats: {e}")
            self.total_payment_count = 0
    
    def _write_payment_stats(self):
        """Write current payment statistics to file"""
        try:
            timestamp = datetime.now().strftime("%Y-%m-%d %H:%M:%S")
            with open(self.stats_file, 'w') as f:
                f.write("=== MQTT Dispenser Payment Statistics ===\n\n")
                f.write(f"Last Updated: {timestamp}\n")
                f.write(f"Active Dispensers: {len(self.dispenser_id_list)}\n")
                f.write(f"Active Tanks: {len(self.tank_id_list)}\n")
                f.write(f"Total Payments: {self.total_payment_count}\n")
                f.write("\n" + "="*50 + "\n")
                f.write("Session History:\n")
                
                # Add session entry if this is an update (not initial creation)
                if hasattr(self, '_session_start_time'):
                    session_duration = datetime.now() - self._session_start_time
                    f.write(f"[{timestamp}] Session Duration: {str(session_duration).split('.')[0]}\n")
                    
        except Exception as e:
            print(f"❌ Could not write payment stats: {e}")
    
    def _increment_payment_counter(self, device_id: int):
        """Thread-safe increment of payment counter"""
        with self.stats_lock:
            self.total_payment_count += 1
            # Write to file immediately for persistence
            self._write_payment_stats()
            print(f"💳 Payment completed on Dispenser {device_id} | Total Payments: {self.total_payment_count}")
            
    def _dispenser_simulator(self, device_id: int, stop_event: threading.Event):
        """Dispenser simulator thread function"""
        topic = f"devices/dispenser/{device_id}"
        fuel_price = self.fuel_prices.get(device_id, 1000)  # Default price if not found
        
        print(f"🚀 Starting dispenser simulator for device {device_id}")
        
        # Mark session start time
        if not hasattr(self, '_session_start_time'):
            self._session_start_time = datetime.now()
        
        try:
            while not stop_event.is_set():
                # Initial state
                liter = 0
                current_price = 0
                selected_price_limit = 10000
                
                payload = {
                    "liter": 0.00,
                    "price": 0,
                    "state": 0
                }
                
                # Wait before starting (random delay)
                if stop_event.wait(random.randint(1, 5)):
                    break
                    
                self.client.publish(topic, json.dumps(payload), qos=1, retain=True)
                
                # Fueling process
                while current_price < selected_price_limit and not stop_event.is_set():
                    liter = round(liter + 0.032, 3)
                    current_price = int(round(fuel_price * liter, 0))
                    
                    if current_price > selected_price_limit:
                        current_price = selected_price_limit
                        liter = round(current_price / fuel_price, 3)
                        
                    payload = {
                        "liter": liter,
                        "price": current_price,
                        "state": 1
                    }
                    
                    self.client.publish(topic, json.dumps(payload), qos=1, retain=True)
                    
                    if stop_event.wait(0.05):
                        break
                
                if stop_event.is_set():
                    break
                    
                # Payment process
                if stop_event.wait(2):
                    break
                    
                payload = {
                    "liter": 0.00,
                    "price": 0,
                    "state": 2,
                    "payment": random.randint(1, 4)
                }
                
                
                if stop_event.wait(1):
                    break
                    
                result = self.client.publish(topic, json.dumps(payload), qos=1, retain=True)
                if result.rc == mqtt.MQTT_ERR_SUCCESS:
                    self._increment_payment_counter(device_id)
                
        except Exception as e:
            print(f"❌ Error in dispenser {device_id}: {e}")
        finally:
            print(f"🛑 Dispenser simulator {device_id} stopped")
    
    def _tank_simulator(self, device_id: int, stop_event: threading.Event):
        """Tank simulator thread function"""
        topic = f"devices/tank/{device_id}"
        tank_max_volume = self.tank_max_volumes.get(device_id, 1000)  # Default volume if not found
        
        print(f"🚀 Starting tank simulator for device {device_id}")
        
        try:
            while not stop_event.is_set():
                current_volume = tank_max_volume
                
                payload = {
                    "current_volume": current_volume
                }
                self.client.publish(topic, json.dumps(payload), qos=1, retain=True)
                
                # Simulate fuel consumption
                while current_volume > 0 and not stop_event.is_set():
                    current_volume = max(0, current_volume - 12)
                    
                    payload = {
                        "current_volume": current_volume
                    }
                    self.client.publish(topic, json.dumps(payload), qos=1, retain=True)
                    
                    if stop_event.wait(random.randint(10, 20)):
                        break
                        
        except Exception as e:
            print(f"❌ Error in tank {device_id}: {e}")
        finally:
            print(f"🛑 Tank simulator {device_id} stopped")
    
    def start_dispenser(self, device_id: int):
        """Start a dispenser simulator thread"""
        simulator_key = f"dispenser:{device_id}"
        
        if simulator_key in self.running_simulators:
            print(f"⚠️ Dispenser '{device_id}' is already running.")
            return
            
        stop_event = threading.Event()
        thread = threading.Thread(
            target=self._dispenser_simulator,
            args=(device_id, stop_event),
            daemon=True,
            name=f"Dispenser-{device_id}"
        )
        
        self.stop_events[simulator_key] = stop_event
        self.running_simulators[simulator_key] = thread
        thread.start()
        
        print(f"✅ Started dispenser '{device_id}'.")
    
    def start_tank(self, device_id: int):
        """Start a tank simulator thread"""
        simulator_key = f"tank:{device_id}"
        
        if simulator_key in self.running_simulators:
            print(f"⚠️ Tank '{device_id}' is already running.")
            return
            
        stop_event = threading.Event()
        thread = threading.Thread(
            target=self._tank_simulator,
            args=(device_id, stop_event),
            daemon=True,
            name=f"Tank-{device_id}"
        )
        
        self.stop_events[simulator_key] = stop_event
        self.running_simulators[simulator_key] = thread
        thread.start()
        
        print(f"✅ Started tank '{device_id}'.")
    
    def start_all_devices(self):
        """Start all dispenser and tank simulators"""
        print("🚀 Starting all devices...")
        
        for dispenser_id in self.dispenser_id_list:
            self.start_dispenser(dispenser_id)
            
        for tank_id in self.tank_id_list:
            self.start_tank(tank_id)
            
        print(f"✅ Started {len(self.dispenser_id_list)} dispensers and {len(self.tank_id_list)} tanks")
    
    def stop_simulator(self, simulator_key: str):
        """Stop a specific simulator"""
        if simulator_key in self.stop_events:
            self.stop_events[simulator_key].set()
            
        if simulator_key in self.running_simulators:
            thread = self.running_simulators[simulator_key]
            thread.join(timeout=5)  # Wait up to 5 seconds for thread to finish
            del self.running_simulators[simulator_key]
            del self.stop_events[simulator_key]
    
    def stop_all_simulators(self):
        """Stop all running simulators"""
        if not self.running_simulators:
            print("ℹ️ No running simulators.")
            return
            
        print("🛑 Stopping all simulators...")
        
        # Signal all threads to stop
        for stop_event in self.stop_events.values():
            stop_event.set()
            
        # Wait for all threads to finish
        for simulator_key, thread in list(self.running_simulators.items()):
            thread.join(timeout=5)
            
        # Clear dictionaries
        self.running_simulators.clear()
        self.stop_events.clear()
        
        print("✅ All simulators stopped.")
    
    def reset_payment_counters(self):
        """Reset payment counter to zero"""
        with self.stats_lock:
            self.total_payment_count = 0
            self._write_payment_stats()
        print("🔄 Payment counter reset to zero.")
    
    def view_payment_file(self):
        """Display contents of payment statistics file"""
        try:
            if os.path.exists(self.stats_file):
                with open(self.stats_file, 'r') as f:
                    content = f.read()
                print(f"\n📄 Contents of {self.stats_file}:")
                print("-" * 50)
                print(content)
                print("-" * 50)
            else:
                print(f"📄 Payment statistics file '{self.stats_file}' not found.")
        except Exception as e:
            print(f"❌ Could not read payment file: {e}")
    
    def get_status(self):
        """Get status of all simulators"""
        if not self.running_simulators:
            print("ℹ️ No running simulators.")
            return
            
        print(f"📊 Running simulators ({len(self.running_simulators)}):")
        for simulator_key, thread in self.running_simulators.items():
            status = "🟢 Running" if thread.is_alive() else "🔴 Dead"
            print(f"  {simulator_key}: {status}")
            
        # Show payment statistics
        self._show_payment_stats()
    
    def _show_payment_stats(self):
        """Display current payment statistics"""
        with self.stats_lock:
            print(f"\n💳 Total Payment Transactions: {self.total_payment_count}")
            if os.path.exists(self.stats_file):
                print(f"📄 Stats saved to: {self.stats_file}")
            else:
                print("📄 Stats file will be created on first payment")
    
    def cleanup(self):
        """Cleanup resources"""
        # Write final payment statistics
        print(f"\n📈 Session Complete - Total Payments Processed: {self.total_payment_count}")
        self._write_payment_stats()
        
        self.stop_all_simulators()
        if self.client:
            self.client.loop_stop()
            self.client.disconnect()
            print("📡 MQTT client disconnected")
        
        if os.path.exists(self.stats_file):
            print(f"📄 Final statistics saved to: {self.stats_file}")

def signal_handler(signum, frame):
    """Handle Ctrl+C gracefully"""
    print("\n🛑 Received interrupt signal...")
    if 'manager' in globals():
        manager.cleanup()
    sys.exit(0)

def main():
    global manager
    
    # Set up signal handling
    signal.signal(signal.SIGINT, signal_handler)
    signal.signal(signal.SIGTERM, signal_handler)
    
    manager = MQTTSimulatorManager()
    
    try:
        while True:
            print("\n📡 MQTT Device Simulator Controller")
            print("1️⃣  Start all devices")
            print("2️⃣  Stop all devices") 
            print("3️⃣  Show status & payment stats")
            print("4️⃣  Start specific dispenser")
            print("5️⃣  Start specific tank")
            print("6️⃣  Reset payment counter")
            print("7️⃣  Show payment stats")
            print("8️⃣  View payment file")
            print("0️⃣  Exit")
            
            choice = input("Select an option: ").strip()
            
            if choice == "1":
                manager.start_all_devices()
            elif choice == "2":
                manager.stop_all_simulators()
            elif choice == "3":
                manager.get_status()
            elif choice == "4":
                try:
                    device_id = int(input("Enter dispenser ID: "))
                    if device_id in manager.dispenser_id_list:
                        manager.start_dispenser(device_id)
                    else:
                        print(f"❌ Dispenser ID {device_id} not found in database")
                except ValueError:
                    print("❌ Invalid device ID")
            elif choice == "5":
                try:
                    device_id = int(input("Enter tank ID: "))
                    if device_id in manager.tank_id_list:
                        manager.start_tank(device_id)
                    else:
                        print(f"❌ Tank ID {device_id} not found in database")
                except ValueError:
                    print("❌ Invalid device ID")
            elif choice == "6":
                manager.reset_payment_counters()
            elif choice == "7":
                manager._show_payment_stats()
            elif choice == "0":
                manager.cleanup()
                print("👋 Bye!")
                break
            else:
                print("❌ Invalid option.")
                
    except KeyboardInterrupt:
        manager.cleanup()
    except Exception as e:
        print(f"❌ Unexpected error: {e}")
        manager.cleanup()

if __name__ == "__main__":
    main()