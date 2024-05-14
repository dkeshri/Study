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


## Project Summary

Generally in Message Queue system __Producer(Sender)__ and __Consumer(Reciver)__ are saperate application therefore 
best practice to implement RabbitMq is to have one connection per Application/Process and one channel per thread.

We follow this convention to have one connection per process and one channel per thread.

So in our current implimentation sender and reciver are on same application so we share one connection to both sender and reciver of message queue.
and also we have one channel through of application.

but we can create two channel one for sender and one for reciver.



> Note: If our sender and receiver have two application then we also create two connection. and each connection have there respective channels depending upon there need. 

