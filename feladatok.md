# Feladatok

1. Ismerjük meg a rendszer felépítését.

1. Nyissuk meg Visual Studion-ban a solutiont és ismerjük meg a forráskód felépítését.

1. Implementáljuk a `users` mikroszolgáltatásban a `GET /api/users` és `GET /api/users/<id>` címeket kiszolgáló kéréseket.

   - Indítsunk el egy mongodb-t a _compose_ fájlból ehhez.

   - Próbáljuk ki a lekérdezéseket Postmanból (a szolgáltatás a <http://localhost:5083/api/users> címen érhető el.).

1. Implementáljuk a `todos` mikroszolgáltatásban a `GET /api/todos` és `GET /api/todos/<id>` címeket kiszolgáló kéréseket.

   - A repository hiányzó kódját is írjuk meg.

   - Használjuk ki a Redis cache-t az utóbbi lekérdezéshez.

   - Próbáljuk ki a lekérdezéseket Postmanból (a szolgáltatás a <http://localhost:5081/api/todos> címen érhető el.).

1. Tegyünk egy Traefik API Gateway-t az összes mikroszolgáltatás elé.

   - Most már használható lesz a web frontend a <http://localhost:5080> címen.

1. Konfiguráljuk be a _forward authentication_-t.

   - Próbáljuk ki, hogyan működik a REST API, ha a `users` mikroszolgáltatás kódjában a `/api/auth` válaszát lecseréljük.
