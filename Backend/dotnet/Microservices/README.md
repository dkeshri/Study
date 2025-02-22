# Microservice

Microservices architecture is a design pattern that structures an application as a collection of small, loosely coupled, independently deployable services. Each microservice is responsible for a specific business capability and communicates with other services via APIs.

## Key Characteristics
1. **Single Responsibility** – Each microservice handles a distinct function.
2. **Independence** – Services can be developed, deployed, and scaled independently.
3. **Decentralized Data Management** – Each service typically has its own database.
4. **API Communication** – Microservices interact via REST, gRPC, or message queues.
5. **Resilience** – Failures in one service do not bring down the whole system.
6. **Technology Agnostic** – Services can be built using different technologies.

## Common Patterns in Microservices

1. **API Gateway** – A single entry point that routes requests to appropriate microservices.
2. **Service Discovery** – Mechanism to dynamically find service instances (e.g., Consul, Eureka).
3. **Database Per Service** – Each microservice has its own database to avoid tight coupling.
4. **Event-Driven Communication** – Services communicate asynchronously via event brokers like RabbitMQ or Kafka.
5. **CQRS** (Command Query Responsibility Segregation) – Separates read and write operations to optimize performance.
6. [**Saga Pattern**](./SagaPattern/) – Manages distributed transactions across multiple microservices.

    [Saga_Choreography](./SagaPattern/Saga_Choreography/),
    [Saga_Orchestration](./SagaPattern/Saga_Orchestration/)

## [Kubernetes](./MergeAllConcept/k8s/)