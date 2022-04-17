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

Create ES database and schema
```sh
curl --request PUT \
  --url 'http://127.0.0.1:9200/products?pretty=' \
  --header 'Content-Type: application/json' \
  --data '{
    "settings" : {
        "number_of_shards" : 2,
        "number_of_replicas" : 1
    },
    "mappings" : {
        "properties" : {
            "tags" : { "type" : "keyword" },
            "updated_at" : { "type" : "date" }
        }
    }
}'
```
Get all records from ES products index
```sh
curl --request GET \
  --url http://127.0.0.1:9200/products/_search \
  --header 'Content-Type: application/json' \
  --data '{ }'
```

Run
```sh
dotnet run
```
Run with hot reload
```sh
dotnet watch run
```