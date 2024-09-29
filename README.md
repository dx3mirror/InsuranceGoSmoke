# Описание

Backend-сервисы предназначены для обработки запрос от UI и других сервисов.

Solution делится на следующий части:
1) `Common` - общие библиотеки для всех проектов. В последующем будет выноситься в отдельные nuget-пакеты.
2) `Notification` - микросервис для работы с уведомлениями. Получает шаблон уведомления от `Static`, параметризирует и отправляет на почту через `Exchange`, но если включено параметром, если выключено пишет в лог текст письма.
3) `Security` отвечает за идентификацию, аутентификацию, авторизацию пользователей. Для работы с пользователями интегрируется с `keycloak`.
4) `Static` содержит статичные файлы. Кэширует ответы.  

-------------  
- `Api` могут в зависимостях иметь только два типа проектов: `Contracts` и `Handlers`. Для обработки сервисов извне(например, ui) созданы проекты с постфиксом `Public.Api`, например: `InsuranceGoSmoke.Security.Public.Hosts.Api`. Для обработки внутренних запросов создаются проекты с постфиксом `Private.Api`, например: `InsuranceGoSmoke.Notification.Private.Hosts.Api`. При этом Public сервисы работают с авторизацией через `Keycloak`, а Private сервисы проверяют доверенные сети
- `Migrations` проекты с миграциями и Seed'ами.
- `Hanlders` имеют в зависимостях `AppSettings`, в которых находятся основные сервисы бизнес-логики. При этом следует заметить, что обработка команд происходит в транзакции и елси требуются вычисления или действия, которые должны выполняться за транзакцией, например отправка уведомления, то необходимо добавлять в `IEventMessageProvider` событие для обработки отправки уведомления(смотреть `UpdateUserRequestHandler`)
- `AppSettings` содержат сервисы для бизнес-логики, могут инжектить `IRepository` для работы с базой данных.
- `Domain` содержат классы сущности таблиц БД.
- `Contracts` содержат контракты для общения с внешними источниками.
- `Clients` содержит реализацию клиентов для доступа к сервису. Данный проект будет публиковаться nuget-пакетом в хранилище для других сервисов, чтобы они могли общаться с этим сервисов посредством обычного вызова метода.
- `Infrastructures.DataAccess` проекты для конфигураций работы с базой данных.
- `Tests` проект с тестами, от этого проекта никто не должен зависеть.

Возможные зависимости между проектами:
|                 | Api | Hanlders | AppSettings | Domain | Contracts | Clients | Infrastructures | Tests |
|-----------------|-----|----------|-------------|--------|-----------|---------|-----------------|-------|
| Api             |  \  |     +    |             |        |     +     |         |                 |       |
| Hanlders        |     |     \    |      +      |        |     +     |    +    |                 |       |
| AppSettings     |     |          |      \      |    +   |     +     |    +    |        +        |       |
| Domain          |     |          |             |    \   |           |         |                 |       |
| Contracts       |     |          |             |        |     \     |         |                 |       |
| Clients         |     |          |             |        |     +     |    \    |                 |       |
| Infrastructures |     |          |             |    +   |           |         |        \        |       |
| Tests           |  +  |     +    |      +      |    +   |     +     |    +    |        +        |   \   |


Проект `AppSettings` внутри себя делит бизнес-логику на контексты, например  в проекте `Security` следующие контексты: 
Внутри контекста логика делится на более мелкие структурные части:
- `Models`: сущности, используемые только в бизнес-логике. Содержит промежуточные классы. Например, партнер содержит большое количество свойств и `Handler` не может передать сущность команды или запроса в сервис, т.к. у `AppService` нет зависимости от `Handler`, поэтому данные мапятся в сущность модели и передаётся в сервис.
- `Services`: логика обработки запроса
- `Factories`: фабрики
- `StateMachines`: машины-состояний
- и т.д.

- redis: `localhost:6379 пароль: pa55w0rd`
- postgres: `postgres:5432 postgres/postgres`
- pgadmin: `localhos:5050`
- keycloak: `localhos:8090 admin/admin`
- opensearch-dashboards: `localhos:5601`
- grafana: `localhost:3050 admin/P@ssw0rd`
- jaeger: `localhost:16686`

### Настройка Keycloak
1. Перейти по адресу http://localhost:8090/
2. Нажать на `Administration Console`
3. Ввести логин/пароль
4. Перейти в `Clients`, выбрать `admin-cli`, убедиться что настройки аутентификации и авторизации включены ![Realm](./documents/readme/realm.png) Если нет - включить. 
5. Перейти на вкладку `Credentials`, скопировать `Client secret`.
6. Перейти на вкладку `Service accounts roles`, если в списке нету роли `admin`, то нажать `Assign role` и выбрать `admin`.
4. Выбрать realm: insuranceGoSmoke Если же он отсутствует, то необходимо создать realm и при создании импортировать файл из папки `./localhost/keycloak/import/realm-export.json`
7. Перейти в `Clients`, выбрать `insuranceGoSmoke`.
8. Перейти на вкладку `Credentials`, сгенерировать новый `Client secret` (если кнопка генерации заблочена, обновить страницу), скопировать `Client secret`.
9. Данные `Client secret` необходимо прописать во всех `appsettings.Development.json` проекта `backend`. Пример:
```
    "Keycloak": {
        "Authority": "http://localhost:8090/realms/insuranceGoSmoke",
        "Realm": "insuranceGoSmoke",
        "ClientId": "insuranceGoSmoke",
        "ClientUuid": "3ac3b9a6-c1ad-4d68-ae19-4071c1e5d48c",
        "ClientSecret": "CWexDUgylZQtRiqkGyd6SUUCgtcieiMV", // secret из insuranceGoSmoke realm insuranceGoSmoke client 
        "MetadataAddress": "http://localhost:8090/realms/insuranceGoSmoke/.well-known/openid-configuration",
        "ApiAdminBaseUrl": "http://localhost:8090",
        "ApiClientId": "admin-cli",
        "ApiClientSecret": "tDZUqDNxHIfJ3kt4jnbTvYEVa2T2vnTU" // secret из realm master client admin-cli
    }
```

