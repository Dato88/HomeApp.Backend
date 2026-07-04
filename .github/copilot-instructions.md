<!-- Quelle: Confluence „Doku-Standards", Stand: 2026-07, Version 1.0 -->
<!-- Bei Abweichung gilt Confluence. Änderungen NUR dort durch den Standard-Owner, dann hierher übernehmen. -->

# Dokumentations-Regeln für dieses Repository

- Neue oder geänderte Umgebungsvariable → `.env.example` **und** README-Abschnitt „Konfiguration" im selben Change aktualisieren.
- Neue Infrastruktur-Abhängigkeit (Datenbank, Message Broker, Identity Provider) → lauffähige Definition (Compose/Orchestrierung) ergänzen und im README unter „Infrastruktur" eintragen.
- Geänderte Test- oder Build-Befehle → README-Abschnitt „Tests & Build" anpassen.
- Neue oder geänderte Datenbank-Migration → zugehörige Doku in `docs/` im selben Change aktualisieren.
- Code-Kommentare erklären nur das **Warum** (Workarounds, nicht-offensichtliche Constraints), nie das Was.
- Keine Secrets in Code, Konfiguration oder Doku — ausschließlich Platzhalter.
- Bedeutende technische Richtungsentscheidungen, die nur dieses Repo betreffen → als Architecture Decision Record (ADR) unter `docs/adr/` festhalten (Muster: Kontext, Alternativen, Entscheidung, Konsequenzen).
- Fachliche Konzepte, teamübergreifende Architektur-Entscheidungen und Prozesse **nicht** in die README schreiben — stattdessen auf die Confluence-Projektseite verweisen: [Link — nachtragen].
- Die README knapp halten: Sie muss genügen, um die App lokal zu starten — nicht mehr.
