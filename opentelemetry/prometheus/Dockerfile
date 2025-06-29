FROM prom/prometheus:main as base
EXPOSE 9090
COPY ["opentelemetry/prometheus/prometheus.yml", "/etc/prometheus/prometheus.yml"]
CMD ["--config.file=/etc/prometheus/prometheus.yml" ]