# Описание API
Данное API осуществляет общение с БД, находящейся на VDS. 
- [x] Позволяет просмотреть последние сообщения от лица гостя(тестовая опция). 
- [x] Запросить Jwt токен для пользователя, зарегестрированного в БД. 
- [x] Записать сообщение в базу данных от лица пользователя, используя jwt bearer. 
- [x] Посмотреть 10 последних сообщений в базе данных, используя jwt токен.

Для теста работоспосбности API используйте гостевого пользователя `{ "username":"Guest", "password":"Guest" }`

# Endpoints: 
:warning: Приложеине работает только по HTTP протоколу.  
## не использующие jwt token
### - /api/inside/login
**описание:**
Получаем jwt токен при успешной авторизации

***тело сообщения:*** 
`{
    name:       "имя отправителя",
    password:    "пароль"
}`

***тело ответа:*** `{ token: "string"}`

***curl запрос:***  Для удобства копирвоания токена, заворачиваем curl запрос в echo
```
echo "$(curl -X POST \
  'http://localhost:7117/api/inside/login' \
  -H 'accept: text/plain' \
  -H 'Content-Type: application/json' \
  -d '{
  "name": "Guest",
  "password": "Guest"
}')"  

```

## использующие jwt token

### - /api/inside/message [POST]
**описание:**

*запрос требует Bearer токен*, полученный от `/api/inside/login`
Записывает сообщеине в БД, если проходит проверка на имя пользователя и Jwt токена выданный ему.

**тело сообщения(<sub>Bearer token</sub>):** `{ name: "имя отправителя", message: "текст сообщение" }`

**curl запрос:**

:pen: по ТЗ надо было между Bearer и полученным токеном ставть нижнее подчеркивание  (покачто не нашёл эту настройку)
```
curl --request POST 'http://localhost:7117/api/inside/message' \
--header 'Authorization: Bearer {JwtTokenString}' \
--header 'Content-Type: application/json' \
--data-raw '{
  "name": "Guest",
  "message": "Guest with jwt token"
}'
```
**тело ответа:** `{ name: "имя отправителя", message: "текст сообщение" }
`

# Процесс запуска

### 1. Командная строка.

Запуск команды `dotnet run` в командной строке из папки с файлом проекта (Inside.csproj)  при наличии установленного `dotnet` на вашей системе [(инструкция MSDN по использованию dotnet run)](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-run). 
Приложение запускается на `https://localhost:7117`

### 2. Докер контейнер
**Образ докер-контейнера на докер хабе:** `dmise/inside`

запускаем контейнер командой `docker-compose up`. По умолчанию контейнер запускается на 7117 порт, пингуем его работоспособность командами:
`https://localhost:7117/health` или 
`https://localhost:7117/swagger/index.html` 

содержимое файла *docker-compose.yaml*
```
version: '3'
services: 
  inside:
    image: dmise/inside:http  
    ports:    
    - "7117:80"
    environment: 
      ASPNETCORE_URLS: "http://+"      
      ASPNETCORE_HTTP_PORT: "7117"      
      ASPTENCORE_ENVIRONMENT: "Development"     
    container_name: "dmise_inside-http"
```

### 3. Среда разработки

Да, проект можно ещё запустить из среды разработки на ваш выбор.

# Техническое задание по которому писался API

```В БД создать пару sql табличек со связями (foreign keys)
Сделать HTTP POST эндпоинт, который получает данные в json вида:
{
    name: "имя отправителя"
    password: "пароль" 
}
этот эндпоинт проверяет пароль по БД и создает jwt токен (срок действия токена и алгоритм подписи не принципиален, для генерации и работе с токеном можно использовать готовую библиотечку) в токен записывает данные: name: "имя отправителя" 
и отправляет токен в ответ, тоже json вида:
{
    token: "тут сгенерированный токен" 
}
Сервер слушает и отвечает в какой-нибудь эндпоинт, в него на вход поступают данные в формате json:
Сообщения клиента-пользователя:
{
    name:       "имя отправителя",
    message:    "текст сообщение"
}
В заголовках указан Bearer токен, полученный из эндпоинта выше (между Bearer и полученным токеном должно быть нижнее подчеркивание).
Проверить токен, в случае успешной проверки токена, полученное сообщение сохранить в БД.
Если пришло сообщение вида:
{
    name:       "имя отправителя",
    message:    "history 10"
}
проверить токен, в случае успешной проверки токена отправить отправителю 10 последних сообщений из БД
Добавить описание и инструкцию по запуску и комментарии в коде, если изменяете формат сообщений, то подробное описание ендпоинтов и их полей.
Завернуть все компоненты в докер, покрыть код тестами.
Проект необходимо выкладывать на github и docker hub. Обязательно наличие readme-файла. 
При отсутствии полноценного readme-файла проверка тестового задания производиться не будет!
Порт 8080 НЕ УКАЗЫВАТЬ!!!
Составить запросы (curl) через терминал для проверки работоспособности вашей программы (приложить файл с запросами). ```
