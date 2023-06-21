
# Social Media

This Project has Microservice CQRS design pattern with strong consistency
acheive using kafka and mongo db as write database and event store
where as used SQL server for read.

Here in this Solution you'll get Command and Query seperate project, each project has
sepereate responsibility for respective tasks.





## Run Locally

Clone the project

```bash
  git clone https://github.com/atul-sirsode/SocialMedia_CQRS.git
```

Go to the project directory

```bash
  cd SocialMedia_CQRS/SocialMedia/docker-compose
```

run docker compose command

```bash
  docker-compose up -d
```

Make sure all containers run successfully