# Design Patterns
Pokemon & standaard memory game met de volgende 3 design patterns

- Observer pattern
- Memento pattern
- Factory method

Voor meer details, zie het startdocument in de map "documentatie"

## Installatie en uitvoering applicatie
De memory game kan je starten via Visual Studio of je kan in de project folder het commando `docker compose up` uitvoeren om de applicatie te laten draaien.

### Visual studio (native)
Het is van belang dat bij gebruik van Visual studio het systeem moet beschikken over `.NET 6`. Op basis van deze library is de app namelijk gemaakt. Vervolgens kan de app opgestart worden door:

1. Het te starten vanuit Visual Studio
2. Te navigeren naar de projectmap (MemoryGame) en het volgende commando uit te voeren `dotnet run`
3. Navigeer naar `https://127.0.0.1:443`

![Dotnet run](documentatie/afbeeldingen/readme/dotnet_run.png "dotnet run")

### Docker
Om de applicatie in docker op te bouwen kan gebruik gemaakt worden van het docker-compose bestand. Hiervoor dient de applicatie "Docker Desktop" geinstalleerd te zijn.

1. Navigeer naar de projectmap (MemoryGame)
2. Voer het commando `docker compose up`
3. Navigeer naar `https://127.0.0.1:443`

![Dotnet run](documentatie/afbeeldingen/readme/docker_compose_up.png "dotnet run")

