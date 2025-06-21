FROM python:3.11-slim

WORKDIR /app

COPY ["stockwebstatusapi/requirements.txt", "."]
RUN pip install -r requirements.txt

COPY ["stockwebstatusapi", "."]

ENV FLASK_APP=app.py

EXPOSE 5000
CMD ["flask", "run", "--host=0.0.0.0"]
