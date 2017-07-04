# Deploy to Ubuntu with Nginx and Self-Signed Certificate

This tutorial shows you how to deploy your bot to an **Ubuntu 17.04** server and setup **webhooks**
via **Nginx**. A self-signed certificate is generated and used in this process. ASP.NET Core application
is deployed to Ubuntu server in a Self-Contained Deployment manner.

[SampleGames project](../../../sample/SampleGames/) is used in this example because it needs additional
 configurations for running its games.

Before everything, make sure you have Nginx installed on the server.

> `www.example.com` represents our arbitrary domain name here.

## TLS Certificate

Telegram uses your bot's certificate to authorize and encrypt webhook messages to bot. Read more about it on
Telegram documentations [here](https://core.telegram.org/bots/api#setwebhook) and [here](https://core.telegram.org/bots/self-signed)

If you don't have a trusted TLS certificate, use the command below to generate a self-signed certificate.
This guides continues using the following self-signed certificate.

```bash
openssl req -newkey rsa:2048 -sha256 -nodes -keyout sample-games-bot.key -x509 -days 365 -out sample-games-bot.pem -subj "/C=CA/ST=Ontario/L=Toronto/O=Telegram Bot Framework Organization/CN=example.com"
```

> Note that the CN, `example.com` here, should exactly match the domain name in webhook URL you set
in bot's settings.

Copy bot certificate files to Nginx configuration directory.

```bash
sudo mkdir /etc/nginx/certificates
sudo cp sample-games-bot.{key,pem} /etc/nginx/certificates
sudo chown www-data:www-data /etc/nginx/certificates/sample-games-bot.{key,pem}
sudo chmod 400 /etc/nginx/certificates/sample-games-bot.key
```

## Publish

This is self-contained deployment so We need to build and publish the app for Ubuntu runtime.

On your development machine, with .NET Core SDK installed, run the following commands to get the
app ready for Ubuntu. It should output the app to `sample/SampleGames/bin/publish` directory.
Copy it to your Ubuntu server.

```bash
## On Your Development Machine
# cd sample/SampleGames/
dotnet restore -r ubuntu.16.10-x64
dotnet publish -c Release -o bin/publish -r ubuntu.16.10-x64
```

On Ubuntu server, create app's directory and copy the app to it.

```bash
## On Ubuntu Server
sudo mkdir -p /var/www/aspnet/sample-games-bot/
sudo chown -R :www-data /var/www/aspnet/sample-games-bot
sudo chmod -R g+s /var/www/aspnet/sample-games-bot

sudo cp -r bin/publish/* /var/www/aspnet/sample-games-bot/
sudo chmod ug+x /var/www/aspnet/sample-games-bot/SampleGames
```

## Application Service

Add a system service for bot. Create file `/etc/systemd/system/sample-games-bot.service` and
write the following configuration to it.

```text
[Unit]
    Description=Sample Games Bot

    [Service]
    ExecStart=/bin/bash -c "cd /var/www/aspnet/sample-games-bot && ./SampleGames"
    Restart=always
    RestartSec=10
    SyslogIdentifier=sample-games-bot
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
    server_name example.com localhost;

    listen 443  ssl;
    listen 8080 ssl;
    listen 8443 ssl;

    ssl_certificate     /etc/nginx/certificates/sample-games-bot.pem;
    ssl_certificate_key /etc/nginx/certificates/sample-games-bot.key;

    ssl_protocols TLSv1.2 TLSv1.1 TLSv1;
    ssl_prefer_server_ciphers on;

    location ~* ^/bots/sample-games-bot/webhook/.+$ {
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

Test new Nginx configurations and restart the web server.

```bash
sudo nginx -t
sudo systemctl restart nginx
```

### App Configurations

Before running the app, we need to provide it configurations. Create file `/var/www/aspnet/sample-games-bot/appsettings.Production.json`
and store the configurations there.

```json
{
  "CrazyCircleBot": {
    "ApiToken": "{your-api-token}",
    "PathToCertificate": "/etc/nginx/certificates/sample-games-bot.pem",
    "WebhookUrl": "https://example.com/bots/{bot}/webhook/{token}"
  }
}
```

That's all. Start the app. App sets webhook at startup and you should be able to chat with it.
Try `/start` command in chat.

```bash
sudo systemctl start sample-games-bot
```
