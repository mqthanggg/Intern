import subprocess
import platform
import sys, psycopg

simulator_processes = {}
dispenser_id_list = []
tank_id_list = []

with psycopg.connect("host=localhost port=5432 user=read_user password=read123 dbname=Intern") as conn:
    with conn.cursor() as cursor:
        cursor.execute("""
            SELECT 
                dispenser_id
            FROM petro_application.dispenser
        """,
        [])
        dispenser_id_list = cursor.fetchall()
        cursor.execute("""
            SELECT 
                tank_id
            FROM petro_application.tank
        """,
        [])
        tank_id_list = cursor.fetchall()

dispenser_id_list = [tup[0] for tup in dispenser_id_list]
tank_id_list = [tup[0] for tup in tank_id_list]

def start_dispenser(device_id):
    if device_id in simulator_processes:
        print(f"⚠️ Dispenser '{device_id}' is already running.")
        return

    if platform.system() == "Windows":
        # Use PowerShell to launch a new terminal
        proc = subprocess.Popen(
            [sys.executable, "dispenser_simulator.py", str(device_id)],
            creationflags=subprocess.CREATE_NO_WINDOW,
            shell=False  # 🔒 Explicitly disable shell usage
        )
    else:
        # Fallback for macOS/Linux — uses gnome-terminal by default
        proc = subprocess.Popen(["gnome-terminal", "--", "python3", "dispenser_simulator.py", device_id])

    simulator_processes[f"dispenser:{device_id}"] = proc
    print(f"✅ Started dispenser '{device_id}'.")

def start_tanks(device_id):
    if device_id in simulator_processes:
        print(f"⚠️ Tank '{device_id}' is already running.")
        return

    if platform.system() == "Windows":
        proc = subprocess.Popen(
            [sys.executable, "tank_simulator.py", str(device_id)],
            creationflags=subprocess.CREATE_NO_WINDOW,
            shell=False  # 🔒 Explicitly disable shell usage
        )
    else:
        # Fallback for macOS/Linux — uses gnome-terminal by default
        proc = subprocess.Popen(["gnome-terminal", "--", "python3", "tank_simulator.py", device_id])

    simulator_processes[f"tank:{device_id}"] = proc
    print(f"✅ Started tank '{device_id}'.")

def stop_all():
    if not simulator_processes:
        print("ℹ️ No running simulators.")
        return

    print("🛑 Stopping all simulators...")
    for device_id, proc in simulator_processes.items():
        try:
            if proc.poll() is None:
                if platform.system() == "Windows":
                    subprocess.Popen(f"taskkill /F /PID {proc.pid} /T", shell=True)
                else:
                    proc.terminate()
        except Exception as e:
            print(f"❌ Could not terminate '{device_id}': {e}")
    simulator_processes.clear()
    print("✅ All simulators stopped.")

try:
    while True:
        print("\n📡 MQTT Device Simulator Controller")
        print("1️⃣  Start devices")
        print("0️⃣  Exit")
        choice = input("Select an option: ").strip()

        if choice == "1":
            for dispenser_id in dispenser_id_list:
                start_dispenser(dispenser_id)
            for tank_id in tank_id_list:
                start_tanks(tank_id)
        elif choice == "0":
            stop_all()
            print("👋 Bye!")
            break
        else:
            print("❌ Invalid option.")
except KeyboardInterrupt:
    stop_all()