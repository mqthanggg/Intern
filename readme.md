# Gas station management software
A simple gas station management system designed for monitoring stations and its related devices such as dispensers, tanks.
## Hardware (simulator)
- Simulators are implemented using Python which automatically publish mock data of dispensers and tanks to the Mosquitto broker.
### ERD
![Screenshot of the software ERD](./Database/erd.png)
## Database
- Powered by PostgreSQL 17.
- Hosted on port 5432.
- There are 2 database users: 
    - **Read-only**: granted with SELECT statement 
    - **Write-only**: granted with INSERT, DELETE, UPDATE, SELECT (whenever required by the UPDATE statement).
- The data is stored under the **petro_application** schema, inside the **Intern** database.
## MQTT broker
- Powered by Mosquitto 2.0.22.
- Configured with SSL certificate and authentication.

## Backend
- Using .NET 9 with minimal APIs to deploy.
- Handles core features such as:
    - Authentication and authorization:
        - JWT with asymmetric key signatures.
        - CSRF (XSRF) protection.
        - Controllers are protected by RBAC.
    - Live data sync with broker via MQTT with TLS:
        - Live data streaming to the client's view.
        - Automatically inserting payments into the database.
## Frontend
- Users can perform _RUD operations on stations.
- Interactive dashboard for users:
    - Live dispensers, tanks metrics.
    - Live transaction logs.
    - Shift data visualizations.
- Interactive dashboard for administrators:
    - Managing staff's attendance.
    - Managing staff's accounts.
...
