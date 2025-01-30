# Developer Evaluation Project

`READ CAREFULLY`

## Instructions
**The test below will have up to 7 calendar days to be delivered from the date of receipt of this manual.**

- The code must be versioned in a public Github repository and a link must be sent for evaluation once completed
- Upload this template to your repository and start working from it
- Read the instructions carefully and make sure all requirements are being addressed
- The repository must provide instructions on how to configure, execute and test the project
- Documentation and overall organization will also be taken into consideration

## Use Case
**You are a developer on the DeveloperStore team. Now we need to implement the API prototypes.**

As we work with `DDD`, to reference entities from other domains, we use the `External Identities` pattern with denormalization of entity descriptions.

Therefore, you will write an API (complete CRUD) that handles sales records. The API needs to be able to inform:

* Sale number
* Date when the sale was made
* Customer
* Total sale amount
* Branch where the sale was made
* Products
* Quantities
* Unit prices
* Discounts
* Total amount for each item
* Cancelled/Not Cancelled

It's not mandatory, but it would be a differential to build code for publishing events of:
* SaleCreated
* SaleModified
* SaleCancelled
* ItemCancelled

If you write the code, **it's not required** to actually publish to any Message Broker. You can log a message in the application log or however you find most convenient.

### Business Rules

* Purchases above 4 identical items have a 10% discount
* Purchases between 10 and 20 identical items have a 20% discount
* It's not possible to sell above 20 identical items
* Purchases below 4 items cannot have a discount

These business rules define quantity-based discounting tiers and limitations:

1. Discount Tiers:
   - 4+ items: 10% discount
   - 10-20 items: 20% discount

2. Restrictions:
   - Maximum limit: 20 items per product
   - No discounts allowed for quantities below 4 items

## Overview
This section provides a high-level overview of the project and the various skills and competencies it aims to assess for developer candidates. 

See [Overview](/.doc/overview.md)

## Tech Stack
This section lists the key technologies used in the project, including the backend, testing, frontend, and database components. 

See [Tech Stack](/.doc/tech-stack.md)

## Frameworks
This section outlines the frameworks and libraries that are leveraged in the project to enhance development productivity and maintainability. 

See [Frameworks](/.doc/frameworks.md)

<!-- 
## API Structure
This section includes links to the detailed documentation for the different API resources:
- [API General](./docs/general-api.md)
- [Products API](/.doc/products-api.md)
- [Carts API](/.doc/carts-api.md)
- [Users API](/.doc/users-api.md)
- [Auth API](/.doc/auth-api.md)
-->

## Project Structure
This section describes the overall structure and organization of the project files and directories. 

See [Project Structure](/.doc/project-structure.md)

---
## Features and Improvements

- **Search Filters for Sales**: Implemented flexible search filters to retrieve sales based on different criteria, such as customer name, branch, sale date range, and cancellation status. These filters allow a refined search experience for the users.
  
- **Sale Details by ID**: The complete sale details, including all items, are only fetched when querying by ID to reduce payload size. When fetching multiple sales, item details are omitted to avoid overloading the response payload.

- **Pagination in Search Endpoint**: Integrated existing pagination features for the sales search endpoint to ensure efficient data retrieval and prevent performance bottlenecks, especially when dealing with large datasets.

- **Discount Business Logic with Strategy Pattern**: Applied business rules for discounts (10% for 4+ items, 20% for 10-20 items) using the Strategy pattern. This ensures compliance with SOLID principles, making the discount logic easy to modify or extend in the future.

- **Unit and Integration Tests**: Executed both unit and integration tests to ensure the application's correctness and stability, validating the key functionalities and interactions.

- **Optional RabbitMQ Setup for Messaging**: Though not mandatory, configured RabbitMQ to handle messaging events. Sale-related events, such as "SaleCreated", "SaleModified", and "SaleCancelled", are published, providing better insight into the application's operation and allowing further integrations.

- **Utilization of MediatR**: Leveraged MediatR's Command, Query, Publisher, and Notification features for better event handling and separation of concerns. This ensures a clean architecture and facilitates scalability by decoupling different parts of the application.

---
## How to Run the Application

### 1. Clone the Repository

```sh
git clone https://github.com/tibursocampos/Ambev.DeveloperEvaluation.git
cd Ambev.DeveloperEvaluation
```

### 2. Running with Docker Compose (Recommended)

Navigate to the backend directory:

```sh
cd template/backend
```

Run the following command to start the services:

```sh
docker compose up -d
```

### 3. Applying Database Migrations

After starting the services, apply the migrations:

```sh
dotnet ef database update --project src/Ambev.DeveloperEvaluation.ORM --startup-project src/Ambev.DeveloperEvaluation.WebApi --context DefaultContext --connection "Host=localhost;Port=5432;Database=developer_evaluation;Username=developer;Password=ev@luAt10n;Pooling=true;"
```

If you are using Visual Studio, open the **Package Manager Console** and run the same command above.

### 4. Running in Debug Mode (Optional)

If you want to run the application in debug mode:

- Open the project in **Visual Studio**.
- Select **Docker Compose** as the startup option.
- Run the application.
- Run command on Package Manager Console of preview section to apply migrations

### 5. Accessing the API Documentation

Once the application is running, you can access the API documentation via Swagger:

```
https://localhost:8081/swagger/index.html
```

### 6. Accessing RabbitMQ

If you want to access RabbitMQ to monitor messages arriving in the queue, visit the following link:
```
http://localhost:15672/#/queues/
```

Credentials for Access:
- Username: developer
- Password: ev@luAt10n

Queue Name:
- SalesEvent

### 7. API Documentation

For detailed API specifications, check the [Sales API Documentation](/.doc/sales-api.md).

### 8. Payload examples for create Sales

To help create sales for testing, examples in [Create Sales Payload](/.doc/sales-payload.md).
