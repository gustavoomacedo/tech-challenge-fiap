FROM grafana/grafana:latest
COPY  ./config/grafana.ini /etc/grafana
COPY  ./provisioning /etc/grafana/provisioning
COPY  ./dashboards /var/lib/grafana/dashboards
EXPOSE 3000
