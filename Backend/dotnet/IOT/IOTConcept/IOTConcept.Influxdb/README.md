﻿# Influx DB
To save the timeserise recored we use influx db.

> Download the [docker-compose.yml](./docker-compose.yml) file and run below docker compose command from termninal.
## Docker Container Creation

> Run below command to create influxDb container on windows machine

```bash
docker compose -f '.\Path\To\docker-compose.yml' up 
```

> On Linux use forward slash for path like `/path/to/docker-compose.yml`
### Port Detail

Port `8086`

### Login crediential

> Default login crediential if we not specifiy during creation of docker container

<small style='color:green'>_Username_</small> `guest` and <small style='color:green'>_Password_</small> `guest`
 
