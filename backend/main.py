from fastapi import FastAPI, Depends
from sqlalchemy.orm import Session
from database import engine, get_db
from models import Base, Buch

Base.metadata.create_all(bind=engine)

app = FastAPI(title="Bücherei API")

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
