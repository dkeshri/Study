# Saga Pattern

The **Saga Pattern** is a design pattern used to manage distributed transactions in microservices. Since traditional ACID transactions are difficult to implement across multiple services, the Saga Pattern ensures data consistency by breaking a transaction into smaller, independent steps. Each step is a transaction in itself, and compensating transactions (rollback actions) are defined in case of failure.

## How Saga Works
A saga is a sequence of operations that ensures consistency across multiple microservices. It follows one of two coordination strategies:

1. **Choreography-Based Saga** (Event-Driven)
    * Each service listens for events and publishes events upon completion.
    * No central coordinator; services communicate through an event bus or message queue (e.g., Kafka, RabbitMQ).
    * Works well for simple workflows.
    * Example:
        * **Order Service** → Places an order → Emits `OrderCreated` event.
        * **Payment Service** → Processes payment → Emits `PaymentSuccessful` event.
        * **Inventory Service** → Reserves stock → Emits `StockReserved` event.
        * If inventory reservation fails, a `StockFailed` event triggers a rollback (compensating transactions).
    
    [Saga_Choreography_Project](./Saga_Choreography/)

2. **Orchestration-Based Saga**
    * A central Saga Orchestrator manages the transaction flow.
    * Services execute transactions only when instructed by the orchestrator.
    * Works well for complex workflows requiring strict control.
    * Example :
        * **Order Service** sends a request to **Orchestrator**.
        * Orchestrator calls **Payment Service** → Payment confirmed.
        * Orchestrator calls **Inventory Service** → Stock reserved.
        * If any step fails, the Orchestrator triggers compensating actions (e.g., refund payment, cancel order).
    
    [Saga_Orchestration_Project](./Saga_Orchestration/)