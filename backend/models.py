from sqlalchemy import Column, Integer, String, Date, ForeignKey
from sqlalchemy.orm import declarative_base

Base = declarative_base()

class Buch(Base):
    __tablename__ = "buecher"
    id = Column(Integer, primary_key=True)
    titel = Column(String, nullable=False)
    autor = Column(String, nullable=False)
    isbn = Column(String, unique=True)
    menge = Column(Integer, default=1)

class Mitglied(Base):
    __tablename__ = "mitglieder"
    id = Column(Integer, primary_key=True)
    name = Column(String, nullable=False)
    email = Column(String, unique=True)

class Ausleihe(Base):
    __tablename__ = "ausleihen"
    id = Column(Integer, primary_key=True)
    buch_id = Column(Integer, ForeignKey("buecher.id"))
    mitglied_id = Column(Integer, ForeignKey("mitglieder.id"))
    ausleihe_datum = Column(Date)
    rueckgabe_datum = Column(Date, nullable=True)
