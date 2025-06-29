FROM grafana/tempo:latest as base
EXPOSE 3200
EXPOSE 7778
COPY ["opentelemetry/tempo/tempo.yaml", "/etc/tempo.yaml"]
CMD [ "--config.file=/etc/tempo.yaml" ]