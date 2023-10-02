# Parking Servis Server

Kreirati Parking Servis Server koji ima zadatak vođenja evidencije o uplatama korisnika za
parkiranje.
### Server u oviru svog interfejsa nudi sledeće funkcionalnosti:
* Dodavanje i/ili izmena zone parkiranja, ovu funkcionalnost može da izvršava korisnik koji ima permisiju **ManageZone**.
* Uplata parkiranja za određenu zonu i određeni auto, ovu funkcionalnost može da izvršavabilo koji 
korisnik.
* Informaciju o tome da li je korisnik uplatio parking, ovu funkcionalnost može da izvršava korisnik koji ima permisiju **Parking Worker**.
* Dodavanje doplatne karte korisniku koji nije uplatio parking,ovu funkcionalnost može da izvršava korisnik koji ima permisiju **Parking Worker**.
* Brisanje doplatne karte, ovu funkcionalnost može da izvršava korisnik koji ima permisiju **Parking Worker**.

Klijent i servis uspostavljaju komunikaciju preko Windows autentifikacije, a autorizacija se implementira po principu RBAC modela.
Neophodno je napraviti rezervni backup server koji se autentifikuje uz pomoć sertifikata sa primarnim serverom. Zadatak sekundarnog servera je da na unapred određeni vremenski interval zatraži podatke za repliciranje od primarnog servera. Podaci koji se šalju treba da budu kriptovani **DES algoritmom u CBC modu**, i **digitalno potpisani**.

Dodatno je neophodno realizovati failover, ukoliko se desi da primarni server padne klijent se preveže na sekundarni server i od tog momenta on postaje primarni dok ne dodje do njegovog pada i ponovnog prevezivanja na sekundarni server.

Sve akcije u sistemu počev od autentifikacije, autorizacije, kao i rad nad samom bazom podataka potrebno je logovati u okviru specifičnog log fajla u okviru **Windows Event Loga**
