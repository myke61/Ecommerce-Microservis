# 🏗️ Microservices E-Ticaret Platformu

Bu proje, modern yazılım mimarisi prensiplerini temel alan, ölçeklenebilir ve genişletilebilir bir **E-Ticaret Mikroservis Platformu** örneğidir. Proje, .NET 9 ve güncel teknoloji yığınları ile inşa edilmiştir.

---

## 🚀 Kullanılan Teknolojiler ve Mimariler

| Teknoloji                  | Açıklama                                                                 |
|----------------------------|--------------------------------------------------------------------------|
| **.NET 9**                 | Mikroservisler .NET 9 ile geliştirildi                                   |
| **DDD (Domain-Driven Design)** | Servislerde domain odaklı tasarım                                    |
| **Microservices Architecture** | Her iş parçası bağımsız bir servis olarak kurgulandı                 |
| **PostgreSQL / MSSQL**     | Veritabanı olarak kullanıldı                                             |
| **Redis**                 | Dağıtık cache yönetimi                                                    |
| **RabbitMQ**              | Servisler arası mesajlaşma altyapısı                                      |
| **ElasticSearch + Kibana** | Request loglama ve izleme çözümü                                         |
| **EventStoreDB**          | Event Sourcing için özel event veri tabanı                                |
| **Duende IdentityServer** | Kimlik doğrulama ve yetkilendirme                                         |
| **Docker + Docker Compose** | Tüm servisler konteynerleştirilmiştir                                   |

---

## 🧩 Mikroservisler ve URL'ler

| Servis            | Açıklama             | URL (Localhost)             |
|-------------------|----------------------|-----------------------------|
| 🛒 Basket Service  | Sepet işlemleri      | `https://localhost:7117`   |
| 📦 Product Service | Ürün işlemleri       | `https://localhost:7056`   |
| 📑 Order Service   | Sipariş işlemleri    | `https://localhost:7093`   |
| 💳 Fake Payment    | Ödeme simülasyonu    | `https://localhost:7055`   |

---

## 📊 İzleme & Loglama

| Bileşen         | URL                           |
|------------------|------------------------------|
| ElasticSearch    | `http://localhost:9200`      |
| Kibana UI        | `http://localhost:5601`      |
| EventStoreDB UI  | `http://localhost:2113`      |

---

## 🐳 Docker Komutları


### ElasticSearch
docker run -d --name elasticsearch -p 9200:9200 -e "discovery.type=single-node" -e "xpack.security.enabled=false" docker.elastic.co/elasticsearch/elasticsearch:8.13.0


### Kibana
docker run -d --name kibana --link elasticsearch:elasticsearch -p 5601:5601 -e "ELASTICSEARCH_HOSTS=http://elasticsearch:9200" docker.elastic.co/kibana/kibana:8.13.0


### EventStoreDB
docker run --name eventstore-node -d -p 2113:2113 -p 1113:1113 eventstore/eventstore:latest --insecure --run-projections=All --enable-atom-pub-over-http


### RabbitMQ
docker run -d --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:4-management


### PostgreSQL
docker run --name some-postgres -e POSTGRES_USER=postgres -e POSTGRES_PASSWORD=postgres -p 5432:5432 -e POSTGRES_DB=BasketDB -d postgres


### Redis
docker run -d --name redis-stack-server -p 6379:6379 redis/redis-stack-server:latest



🔐 Kimlik Doğrulama
  Duende IdentityServer ile servis bazlı authentication/authorization sağlanır.
  Her servisin yalnızca kendi yetkili endpointlerine erişimi sağlanır.


✅ Başlarken
  Docker servislerini çalıştırın.
  Her servisi https://localhost:[PORT] adresinden kontrol edin.
  Kibana üzerinden logları analiz edebilir, EventStoreDB üzerinden event akışlarını inceleyebilirsiniz.
