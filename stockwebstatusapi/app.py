from flask import Flask, jsonify
from flask_cors import CORS
import threading
import time
import requests
import os
import json

#Read environment variable
services_env = os.getenv("SERVICE_LIST", "[]")

data = [{
    "name": "StockAPI",
    "status":"Unhealthy",
    "url": "url"
}]
json_string = json.dumps(data)
print(json_string)
print(services_env)
try:
    SERVICES = json.loads(services_env)
except json.JSONDecodeError:
    SERVICES = []


# In-memory store of statuses
service_statuses = { svc["name"] : svc for svc in SERVICES}

app = Flask(__name__)
CORS(app)

@app.route('/api/health-status', methods=['GET'])
def get_health_status():
    list = []
    for k,v in service_statuses.items():
        list.append(v)
    return jsonify(list)

def check_services():
    while True:
        for svc in SERVICES:
            try:
                response = requests.get(svc["url"], timeout=3)
                if response.status_code == 200:
                    service_statuses[svc["name"]]["status"] = "Healthy"
                else:
                    service_statuses[svc["name"]]["status"] = "Unhealthy"
            except Exception:
                service_statuses[svc["name"]]["status"] = "Unknown"
        time.sleep(5)

threading.Thread(target=check_services, daemon=True).start()