# Deploy a Telegram bot and run it with Webhooks

In this tutorial, I discuss how to configure and deploy a Telegram bot to an **Ubuntu 17.04 64bit**. Project [Telegram.Bot.Sample](https://github.com/pouladpld/Telegram.Bot.Framework/tree/master/src/Telegram.Bot.Sample) is deployed and it contains two Telegram bots inside.

## TLS Certificate

If you don't have a TLS certificate, use the command below to generate a self-signed certificate.

```bash
openssl req -newkey rsa:2048 -sha256 -nodes -keyout samplebot.key -x509 -days 365 -out samplebot.pem -subj "/C=CA/ST=Ontario/L=Toronto/O=.NET Telegram Bot Framework Organization/CN=example.org"
```

> Replace the values such as `example.org` in the above command.

Note that the CN, `example.org` here, should exactly match the webhook URL you send to Telegram. For keeping this tutorial simple, I am using 1 certificate for both of the bots.

## .NET Core CLI

Install [.NET Core](https://www.microsoft.com/net/core#linuxubuntu)

> I installed version `dotnet-dev-2.0.0-preview1-005977` for this tutorial because I had problems with installing version `dotnet-dev-1.0.4`.

## Application Service

```bash
sudo vi /etc/systemd/system/samplebot.service
```

content:

```text
[Unit]
    Description=SampleBot - Telegram bot app

    [Service]
    ExecStart=/bin/bash -c "cd /var/www/aspnet/samplebot && ./Telegram.Bot.Sample"
    Restart=always
    RestartSec=10
    SyslogIdentifier=samplebot
    User=www-data
    Environment=ASPNETCORE_ENVIRONMENT=Production

    [Install]
    WantedBy=multi-user.target
```

```bash
sudo systemctl daemon-reload
```

## Nginx

```bash
sudo apt-get install nginx -y
```

Create directories to publish the app to.

```bash
sudo mkdir -p /var/www/aspnet/samplebot
sudo chown -R :www-data /var/www/aspnet/samplebot
sudo chmod -R g+s /var/www/aspnet/samplebot
```

Copy the bot certificates to Nginx configuration directory.

```bash
sudo mkdir /etc/nginx/certificates
sudo cp samplebot.{key,pem} /etc/nginx/certificates
sudo chown www-data:www-data /etc/nginx/certificates/samplebot.{key,pem}
sudo chmod 400 /etc/nginx/certificates/samplebot.key
```

### Site Configuration

```bash
# Make a backup of default site's configuration
sudo cp /etc/nginx/sites-available/default{,~}

sudo vi /etc/nginx/sites-available/default
```

Content:

```nginx
server {
        server_name samplebot.com localhost;
        listen  80;
        listen  [::]:80;

        root /var/www/html;

        index index.html index.htm index.nginx-debian.html;

        location / {
            try_files $uri $uri/ =404;
        }

        location /sample_greeter_bot {
            proxy_pass http://localhost:5000;
        }

        location /sample_echoer_bot {
            proxy_pass http://localhost:5000;
        }
}

server {
        server_name samplebot.com localhost;

        listen 443  ssl;
        listen 8080 ssl;
        listen 8443 ssl;

        ssl_certificate     /etc/nginx/certificates/samplebot.pem;
        ssl_certificate_key /etc/nginx/certificates/samplebot.key;

        ssl_protocols TLSv1.2 TLSv1.1 TLSv1;
        ssl_prefer_server_ciphers on;

        location /sample_greeter_bot {
            proxy_pass          http://localhost:5000;
            proxy_http_version  1.1;
            proxy_set_header    Upgrade $http_upgrade;
            proxy_set_header    Connection keep-alive;
            proxy_set_header    Host $host;
            proxy_cache_bypass  $http_upgrade;
            proxy_set_header    X-Forwarded-For $proxy_add_x_forwarded_for;
        }

        location /sample_echoer_bot {
            proxy_pass          http://localhost:5000;
            proxy_http_version  1.1;
            proxy_set_header    Upgrade $http_upgrade;
            proxy_set_header    Connection keep-alive;
            proxy_set_header    Host $host;
            proxy_cache_bypass  $http_upgrade;
            proxy_set_header    X-Forwarded-For $proxy_add_x_forwarded_for;
        }
}
```

```bash
sudo nginx -t
sudo systemctl restart nginx
```

## Publish the App

```bash
mkdir source-code && cd source-code
git clone https://github.com/pouladpld/Telegram.Bot.Framework.git .

# Build the project
dotnet restore -r ubuntu.16.10-x64

cd src/Telegram.Bot.Sample
# rm -rf bin/publish/*
dotnet publish -c Release -o bin/publish -r ubuntu.16.10-x64

# sudo rm -rf /var/www/aspnet/samplebot/*
sudo cp -r bin/publish/* /var/www/aspnet/samplebot/
sudo chmod ug+x /var/www/aspnet/samplebot/Telegram.Bot.Sample
```

In addition to _appsettings.json_ file, you can store the production secrets in a file like the one below:

```json
// appsettings.Production.json
{
  "EchoerBot": {
    "ApiToken": "",
    "PathToCertificate": "/etc/nginx/certificates/samplebot.pem",
    "WebhookUrl": "https://example.org/{botname}/{token}/webhook"
  },
  "GreeterBot": {
    "ApiToken": "",
    "PathToCertificate": "/etc/nginx/certificates/samplebot.pem",
    "WebhookUrl": "https://example.org/{botname}/{token}/webhook"
  }
}
```

### Run the app

```bash
sudo systemctl start samplebot.service
```
