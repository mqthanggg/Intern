FROM eclipse-mosquitto:2.0

COPY config/mosquitto.conf /mosquitto/config/
COPY config/passwd /mosquitto/config/
COPY certs/ /mosquitto/certs/

USER root
RUN chown root:root /mosquitto/config/passwd && \
    chmod 400 /mosquitto/config/passwd