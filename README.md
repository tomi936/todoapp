# TODO alkalmazás mikroszolgáltatásokra építve

## Labor célja

A labor célja bemutatni a mikroszolgáltatásokra épülő rendszerek esetében gyakran használt _polyglot_ elvet: mikroszolgáltatásonként más-más platformot és nyelvet, valamint más adatbázis rendszer használunk. A szolgáltatásokat pedig egy _API gateway_ fogja össze.

## Előkövetelmények

Az alkalmazás teljes egészében platformfüggetlen. A kényelmes fejlesztéshez azonban a forráskód Microsoft Visual Studio-t feltételez.

- Microsoft Visual Studio 2017/2019
- Docker
  - [Volume sharing](https://docs.microsoft.com/en-us/visualstudio/containers/troubleshooting-docker-errors?view=vs-2019#volume-sharing-is-not-enabled-enable-volume-sharing-in-the-docker-ce-for-windows-settings--linux-containers-only) engedélyezve
  - Minimum 2 GB memória allokálva a Docker-nek
- NPM és YARN (path-ban elérhető)
  - Csak akkor szükséges, ha a fronted kódban fejleszteni szeretnénk
- Postman
- 8 GB RAM

## Feladatok

A `master` ág állapotából kiindulva kövessük a [feladatokat](feladatok.md).

A kész megoldás elérhető a `megoldas` nevű ágon.

## Rendszer felépítése

A rendszer az alábbi mikroszolgáltatásokból épül fel:

- _todos_: A teendőket kezelő alkalmazás, ASP.NET Core platformon, REST API-t biztosítva. Elasticsearch adatbázist és Redis cache-t használ.
- _web_: React-ra épülő SPA webalkalmazás TypeScript-ben, NGINX webszerverrel kiszolgálva.
- _users_: Felhasználókat kezelő alkalmazás Python-ban, MongoDB adatbázisra épülve REST API-t biztosít.

```
                   +----+     +-------+     +---------+
                   |    +---->+ todos +--+->+ elastic |
+-----------+      |API |     +-------+  |  +---------+
|  browser  +----->+gate|                |
+-----------+      |way |                |  +-------+
                   |    |     +------+   +->| redis |
                   |    +---->+ web  |      +-------+
                   |    |     +------+
                   |    |
                   |    |     +-------+      +---------+
                   |    +---->+ users +----->+ mongodb |
                   +----+     +-------+      +---------+

```

## Futtatás

#### Microsoft Visual Studio-ból

Az `src/todoapp.sln` solution fájlt megnyitva a `docker-compose` nevű projektet _startup project_-nek beállítva F5-tel indítható. A forráskódban történő bármely változtatás után (akár C# kód, akár más kód) ugyanígy fordítás és debug módú indítás szükséges.

#### Konzolból

Az `src/docker` könyvtárból az alábbi parancsokkal fordítható és indítható az alkalmazás:

```bash
docker-compose build
docker-compose up
```

#### URL-ek

Az egyes szolgáltatások az alábbi URL-eken érhetően el:

- Weboldal
  - <http://localhost:5080>
  - Visual Studio-ból futtatva közvetlenül <http://localhost:5082>
- Todos REST API
  - <http://localhost:5080/api/todos>
  - Visual Studio-ból futtatva közvetlenül <http://localhost:5081/api/todos>
- Users REST API
  - <http://localhost:5080/api/users>
  - Visual Studio-ból futtatva közvetlenül <http://localhost:5083/api/todos>
- Traefik Dashboard: <http://localhost:5088>
- Mongodb: <mongodb://localhost:27017>
- Elasticsearch: <http://localhost:9200>
