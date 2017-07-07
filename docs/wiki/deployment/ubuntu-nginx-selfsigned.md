# Deploy to Ubuntu with Nginx and Self-Signed Certificate

This tutorial shows you how to deploy your bot to an **Ubuntu 16.04** server, use **Nginx** as its reverse proxy,
and setup **webhook**.

A self-signed certificate is generated and used in this process. ASP.NET Core application
is deployed to Ubuntu server in a Framework Dependant Deployment manner.

[SampleEchoBot project](../../../sample/SampleEchoBot/) is used in this tutorial for simplicity.

First of all, make sure you have installed [.NET Core SDK](https://www.microsoft.com/net/core#linuxubuntu) and
[Nginx](https://help.ubuntu.com/community/Nginx) on the server.

> `www.example.com` represents our arbitrary domain name here.

## TLS Certificate

Telegram uses your bot's certificate to authorize and encrypt webhook messages to bot. Read more about it on
Telegram documentations [here](https://core.telegram.org/bots/api#setwebhook) and [here](https://core.telegram.org/bots/self-signed)

If you don't have a trusted TLS certificate, use the command below to generate a self-signed certificate.

```bash
openssl req -newkey rsa:2048 -sha256 -nodes -keyout sample-echobot.key -x509 -days 365 -out sample-echobot.pem -subj "/C=CA/ST=Ontario/L=Toronto/O=Telegram Bot Framework Organization/CN=example.com"
```

> Note that the CN, `example.com` here, should exactly match the domain name in webhook URL you set
in bot's settings.

Copy bot certificate files to Nginx configuration directory.

```bash
sudo mkdir /etc/nginx/certificates
sudo cp sample-echobot.{key,pem} /etc/nginx/certificates
sudo chown www-data:www-data /etc/nginx/certificates/sample-echobot.{key,pem}
sudo chmod 400 /etc/nginx/certificates/sample-echobot.key
```

## Publish

Get the source code and with .NET Core SDK installed, build the app.

```bash
git clone "https://github.com/pouladpld/Telegram.Bot.Framework.git"

dotnet restore
dotnet build

cd sample/SampleEchoBot/
dotnet publish -c Release -o bin/publish
```

Create app's directory, give it necessary permissions and copy the app to it.

```bash
sudo mkdir -p /var/www/aspnet/sample-echobot/
sudo chown -R :www-data /var/www/aspnet/sample-echobot
sudo chmod -R g+s /var/www/aspnet/sample-echobot

sudo cp -r bin/publish/* /var/www/aspnet/sample-echobot/
```

## App Configurations

Create file `/var/www/aspnet/sample-echobot/appsettings.Production.json`
and store the configurations there.

```json
{
  "EchoBot": {
    "ApiToken": "{your-api-token}",
    "BotUserName": "{your-bot-username}",
    "PathToCertificate": "/etc/nginx/certificates/sample-echobot.pem",
    "WebhookUrl": "https://example.com/bots/{bot}/webhook/{token}"
  }
}
```

> Replace the values for _ApiToken_ and _BotUserName_, and domain name in _WebhookUrl_.

## App Service

Add a system service for bot. Create file `/etc/systemd/system/sample-echobot.service` and
write the following configuration to it.

```text
[Unit]
    Description=Sample Echo Bot

    [Service]
    ExecStart=/bin/bash -c "cd /var/www/aspnet/sample-echobot && dotnet ./SampleEchoBot.dll"
    Restart=always
    RestartSec=10
    SyslogIdentifier=sample-echobot
    User=www-data
    Environment=ASPNETCORE_ENVIRONMENT=Production

    [Install]
    WantedBy=multi-user.target
```

Reload the system settings so the new service will be available.

```bash
sudo systemctl daemon-reload
```

## Nginx

First of all, make a backup of default site's configuration.

```bash
sudo cp /etc/nginx/sites-available/default{,~}
```

Open file `/etc/nginx/sites-available/default` and edit Nginx configurations:

```nginx
server {
# Change domain name here
    server_name example.com localhost;

    listen  80;
    listen  [::]:80;

    root /var/www/html;
    index index.html index.htm index.nginx-debian.html;

    location / {
        try_files $uri $uri/ =404;
    }

    location ~* ^/bots/.+/webhook/.+$ {
        return 301 https://$host$request_uri;
    }
}

server {
# Change domain name here
    server_name example.com localhost;

    listen 443  ssl;
    listen 8080 ssl;
    listen 8443 ssl;

    ssl_certificate     /etc/nginx/certificates/sample-echobot.pem;
    ssl_certificate_key /etc/nginx/certificates/sample-echobot.key;

    ssl_protocols TLSv1.2 TLSv1.1 TLSv1;
    ssl_prefer_server_ciphers on;

# Change {your-bot-username} here
    location ~* ^/bots/{your-bot-username}/webhook/.+$ {
        proxy_pass          http://0.0.0.0:5000;
        proxy_http_version  1.1;
        proxy_set_header    Upgrade $http_upgrade;
        proxy_set_header    Connection keep-alive;
        proxy_set_header    Host $host;
        proxy_cache_bypass  $http_upgrade;
        proxy_set_header    X-Forwarded-For $proxy_add_x_forwarded_for;
    }
}
```

> Replace values for domain name in _server_name_ and _{your-bot-username}_ in location url.

Test new Nginx configurations and restart the web server.

```bash
sudo nginx -t && sudo systemctl restart nginx
```

## Start

That's all. Start the app. App sets webhook at startup and you should be able to chat with it.
Try `/echo` command in chat.

```bash
sudo systemctl start sample-echobot

# See app logs
sudo journalctl --identifier=sample-echobot --follow
```
