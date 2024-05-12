# RabbitMQ

## Docker Container Creation

> Run below command to create RabbitMq Docker container

```bash
docker run -d --hostname rmq --name RabbitMqServer -p 5672:5672 -p 8080:15672 rabbitmq:3.13-management
```
### Port Detail

Port `8080` is for management portal and access by below mention __Login credentials__.

Click on the link for <a href='http://localhost:8080/'>Admin Portal</a>

Port `5672` is use in communication during producing and consuming of message.

### Login crediential

> Default login crediential if we not specifiy during creation of docker container

<small style='color:green'>_Username_</small> `guest` and <small style='color:green'>_Password_</small> `guest`

