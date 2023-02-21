# ClientServer-App

Server app

spouštěcí soubor ve složce SERVER APP/Program

Na nakonfigurovatelné IP adrese a portu se spustí server, na který se může připojit více klientů,
server umožnuje obsluhovat zároveň všechny připojené klienty.
Server ukládá data(text) a odesílá ihned všem klientům.
Při každém zápisu dat se ukládá záloha.
Pokud se server vypne a nebo dojde k výpadku, při opětovném spuštění se načte poslední záloha dat.
Veškeré dění na serveru se vypisuje do serverlogu.


Client app

spouštecí soubor ve složce CLIENT APP/Program

Připojí se na nakonfigurovatelnou IP adresu a port. Může se spustit několik klientů zároveň.
Po připojení se zobrazí tabulka, která vypisuje všechny záznamy ve formátu ID,Name,Surname,Age.
Klient může přidat nový záznam, upravit, nebo smazat aktuálně vybraný záznam.
Pokud dojde k výpadku serveru, všichni připojení klienti se automaticky odpojí a odhlásí.

