# Application
Up databases
```sh
docker-compose up -d
```

Create identity database and schema
```sh
dotnet ef database drop
dotnet ef migrations add InitialCreate &&
dotnet ef database update
```

Run
```sh
dotnet run
```
Run with hot reload
```sh
dotnet watch run
```