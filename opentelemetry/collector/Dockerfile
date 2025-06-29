FROM otel/opentelemetry-collector-contrib:latest as base
COPY ["opentelemetry/collector/collector-config.yaml", "/etc/otel-collector-config.yaml"] 
EXPOSE 4317
EXPOSE 4318 
EXPOSE 7777
CMD [ "--config", "/etc/otel-collector-config.yaml" ]