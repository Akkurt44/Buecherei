import sys
sys.stdout.flush()
from kafka import KafkaConsumer
import json
import os
import smtplib
import time
from email.mime.text import MIMEText

SMTP_HOST = "email-smtp.us-east-1.amazonaws.com"
SMTP_PORT = 587
SMTP_USER = "AKIAUEPEJ4IVAUID7PFH"
SMTP_PASS = os.getenv("SMTP_PASSWORD")
MAIL_FROM = "muratakkurt18@gmail.com"
MAIL_TO = "muratakkurt18@gmail.com"

def mail_gonder(konu, icerik):
    msg = MIMEText(icerik)
    msg["Subject"] = konu
    msg["From"] = MAIL_FROM
    msg["To"] = MAIL_TO
    with smtplib.SMTP(SMTP_HOST, SMTP_PORT) as server:
        server.starttls()
        server.login(SMTP_USER, SMTP_PASS)
        server.sendmail(MAIL_FROM, MAIL_TO, msg.as_string())
    print(f"Mail gönderildi: {konu}", flush=True)

def create_consumer():
    while True:
        try:
            c = KafkaConsumer(
                "ausleihe-events",
                bootstrap_servers=os.getenv("KAFKA_BOOTSTRAP_SERVERS", "kafka:9092"),
                value_deserializer=lambda v: json.loads(v.decode("utf-8")),
                auto_offset_reset="earliest",
                group_id="mail-consumer"
            )
            print("Kafka'ya bağlandı!")
            return c
        except Exception as e:
            print(f"Kafka hazır değil, 5 saniye bekleniyor... {e}", flush=True)
            time.sleep(5)

consumer = create_consumer()


print("Consumer başladı, dinleniyor...", flush=True)

for message in consumer:
    event = message.value
    print(f"Event geldi: {event}", flush=True)

    if event["event"] == "ausleihe":
        konu = "📚 Kitap ödünç alındı!"
        icerik = f"Kitap ID: {event['buch_id']}\nÜye ID: {event['mitglied_id']}\nTarih: {event['datum']}"
    elif event["event"] == "iade":
        konu = "✅ Kitap iade edildi!"
        icerik = f"Ausleihe ID: {event['ausleihe_id']}\nTarih: {event['datum']}"
    else:
        konu = "❓ Bilinmeyen event"
        icerik = str(event)

    mail_gonder(konu, icerik)
