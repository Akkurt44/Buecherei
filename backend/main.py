from fastapi import FastAPI, Depends, HTTPException
from sqlalchemy.orm import Session
from database import engine, get_db
from models import Base, Buch, Mitglied, Ausleihe
import datetime
import os
import json

from kafka import KafkaProducer
from kafka.errors import NoBrokersAvailable

Base.metadata.create_all(bind=engine)

app = FastAPI(title="Bücherei API")

try:
    producer = KafkaProducer(
        bootstrap_servers=os.getenv("KAFKA_BOOTSTRAP_SERVERS", "kafka:9092"),
        value_serializer=lambda v: json.dumps(v).encode("utf-8")
    )
except NoBrokersAvailable:
    producer = None

@app.get("/")
def root():
    return {"message": "Bücherei API çalışıyor"}

@app.get("/health")
def health():
    return {"status": "ok"}

@app.get("/buecher")
def get_buecher(db: Session = Depends(get_db)):
    return db.query(Buch).all()

@app.post("/buecher")
def add_buch(titel: str, autor: str, isbn: str, menge: int = 1, db: Session = Depends(get_db)):
    buch = Buch(titel=titel, autor=autor, isbn=isbn, menge=menge)
    db.add(buch)
    db.commit()
    db.refresh(buch)
    return buch

@app.get("/mitglieder")
def get_mitglieder(db: Session = Depends(get_db)):
    return db.query(Mitglied).all()

@app.post("/mitglieder")
def add_mitglied(name: str, email: str, db: Session = Depends(get_db)):
    mitglied = Mitglied(name=name, email=email)
    db.add(mitglied)
    db.commit()
    db.refresh(mitglied)
    return mitglied

@app.get("/ausleihen")
def get_ausleihen(db: Session = Depends(get_db)):
    return db.query(Ausleihe).all()

@app.post("/ausleihen")
def ausleihe_buch(buch_id: int, mitglied_id: int, db: Session = Depends(get_db)):
    buch = db.query(Buch).filter(Buch.id == buch_id).first()
    if not buch:
        raise HTTPException(status_code=404, detail="Kitap bulunamadi")
    if buch.menge < 1:
        raise HTTPException(status_code=400, detail="Kitap stokta yok")
    buch.menge -= 1
    ausleihe = Ausleihe(buch_id=buch_id, mitglied_id=mitglied_id, ausleihe_datum=datetime.date.today())
    db.add(ausleihe)
    db.commit()
    db.refresh(ausleihe)
    if producer:
        producer.send("ausleihe-events", {
            "event": "ausleihe",
            "buch_id": buch_id,
            "mitglied_id": mitglied_id,
            "datum": str(datetime.date.today())
        })
    return ausleihe

@app.put("/ausleihen/{ausleihe_id}/iade")
def iade_buch(ausleihe_id: int, db: Session = Depends(get_db)):
    ausleihe = db.query(Ausleihe).filter(Ausleihe.id == ausleihe_id).first()
    if not ausleihe:
        raise HTTPException(status_code=404, detail="Kayit bulunamadi")
    if ausleihe.rueckgabe_datum:
        raise HTTPException(status_code=400, detail="Kitap zaten iade edildi")
    ausleihe.rueckgabe_datum = datetime.date.today()
    buch = db.query(Buch).filter(Buch.id == ausleihe.buch_id).first()
    buch.menge += 1
    db.commit()
    db.refresh(ausleihe)
    if producer:
        producer.send("ausleihe-events", {
            "event": "iade",
            "ausleihe_id": ausleihe_id,
            "datum": str(datetime.date.today())
        })
    return ausleihe
