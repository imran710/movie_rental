# 🎬 Movie Rental API

A simple and modular **.NET 8 Web API** for managing movie rentals,
returns, availability checks, and rental history.\
The project uses **EF Core**, **PostgreSQL**, and **Docker** for easy
development & deployment.

## 📌 API Endpoints

### Rental Operations

  Method   Endpoint                     Description
  -------- ---------------------------- ----------------------
  `POST`   `/v1/api/rentals/checkout`   Rent a movie
  `POST`   `/v1/api/rentals/return`     Return rented movies

### Movie Operations

  Method   Endpoint                     Description
  -------- ---------------------------- -----------------------
  `GET`    `/v1/api/movies/available`   View available movies

### Customer Operations

  -------------------------------------------------------------------------------------
  Method             Endpoint                             Description
  ------------------ ------------------------------------ -----------------------------
  `GET`              `/v1/api/customers/rental-history`   View customer rental history

  -------------------------------------------------------------------------------------

## 🏗️ Entity Framework Commands

### Create Migration

``` bash
dotnet ef migrations add model_property_changes --context AppDbContext --project src/Core --startup-project src/Api --output-dir Infrastructure/Migrations
```

### Apply Migration

``` bash
dotnet ef database update --context AppDbContext --project src/Core --startup-project src/Api
```

## 🗄️ Seed SQL for Movies Table

``` sql
INSERT INTO public."Movies"(
    "Id", "Title", "Genre", "ReleaseYear", "Stock", "RentalType", 
    "DeletedAt", "DeletedBy", "IsDeleted", "UpdateInfo_UpdatedAt", "UpdateInfo_UpdatedBy", "CreationInfo_CreatedBy")
VALUES
    (1, 'The Shawshank Redemption', 'Drama', 1994, 12, 0, NULL, NULL, FALSE, '2025-11-01 10:00:00', 1, 1),
    (2, 'The Godfather', 'Crime', 1972, 22, 1, NULL, NULL, FALSE, '2025-11-02 11:15:00', 1, 1),
    (3, 'The Dark Knight', 'Action', 2008, 12, 2, NULL, NULL, FALSE, '2025-11-03 12:30:00', 1, 1),
    (4, 'Pulp Fiction', 'Crime', 1994, 54, 0, NULL, NULL, FALSE, '2025-11-04 13:45:00', 1, 1),
    (5, 'Inception', 'Sci-Fi', 2010, 12, 1, NULL, NULL, FALSE, '2025-11-05 15:00:00', 1, 1);
```

## 🐳 Docker Build & Run

### Build Docker Image

``` bash
docker buildx build --platform linux/amd64 -t rental-api:1.0 .
```

### Run Using Docker Compose

``` bash
docker-compose up -d --build
```

### View Logs

``` bash
docker-compose logs -f
```

## 📂 Project Structure

    src/
     ├── Api/                  # Presentation layer (controllers, endpoints)
     ├── Core/                 # Domain + EF Core entities + DbContext
     ├── Infrastructure/       # Migrations, persistence implementation
     └── Dockerfile            # API container definition
    docker-compose.yml         # Full stack with PostgreSQL
    README.md

## ✔️ Features

-   Movie rental & return workflow\
-   Rental history tracking\
-   Soft-delete support\
-   Clean architecture\
-   Dockerized deployment\
-   PostgreSQL database
