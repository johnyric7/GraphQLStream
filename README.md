# Real-Time Location Notifications System with GraphQL Subscriptions and Redis Pub/Sub

This project demonstrates the implementation of a **real-time location notification system** using **GraphQL subscriptions** and **Redis Pub/Sub** to deliver updates to clients efficiently at scale. The system leverages **HotChocolate** for GraphQL server implementation and **Redis** for message broadcasting, enabling real-time communication across multiple clusters.

## Overview

This system allows clients to subscribe to specific updates and get real-time notifications when changes occur. Redis Pub/Sub ensures that the message is pushed to all subscribers as soon as the data changes, enabling efficient real-time communication across thousands of users.

## Key Features

- **GraphQL Subscriptions:** Real-time updates through subscriptions, allowing clients to listen to specific events.
- **Redis Pub/Sub:** Efficient message distribution to all subscribers via Redis channels.
- **Scalable Architecture:** Easily scales to handle thousands (or even millions) of concurrent subscribers.
- **High Availability:** Redis ensures that the system remains highly available, even if Redis nodes or clusters are added.
- **Real-Time Location Updates:** Demonstrates use cases like location updates, where users are notified about changes as soon as they happen.

## Technologies

- **GraphQL (HotChocolate):** For implementing GraphQL queries, mutations, and subscriptions.
- **Redis:** For Pub/Sub message distribution. Handles message broadcasting to multiple subscribers in real-time.
- **.NET (C#):** The backend is built using C# and .NET Core.
- **WebSocket:** Used for GraphQL subscriptions, ensuring real-time communication between clients and the server.

## System Architecture

1. **GraphQL Server (HotChocolate):** Handles GraphQL queries, mutations, and subscriptions. When a mutation (e.g., `UpdateUserLocation`) occurs, it broadcasts the changes to Redis.
2. **Redis Pub/Sub:** Acts as a message broker to ensure real-time updates are delivered to all interested clients. Redis channels are used to distribute messages.
3. **Client Subscriptions:** Clients subscribe to specific topics (e.g., location updates, user profile changes) using GraphQL subscriptions over WebSockets.

## Getting Started

### Prerequisites

To run the project locally, ensure you have the following installed:

- [.NET SDK](https://dotnet.microsoft.com/download) (5.0 or later)
- [Redis](https://redis.io/download) (for local development or use a cloud Redis service)
- [HotChocolate GraphQL Server](https://chillicream.com/docs/hotchocolate) (for GraphQL server integration)

### Running the Application

1. **Clone the repository:**

```bash
git clone https://github.com/johnyric7/GraphQLStream.git
cd GraphQLStream
```

2. **Install dependencies:**

If you are using .NET Core, restore the dependencies:

```bash
dotnet restore
```

Add these packages:

```bash
dotnet add package Bogus --version 33.0.2
dotnet add package HotChocolate.AspNetCore --version 15.0.3
dotnet add package HotChocolate.Subscriptions.Redis --version 15.0.3
dotnet add package MongoDB.Driver --version 3.2.1
dotnet add package StackExchange.Redis --version 2.8.31
```

3. **Configure Redis connection and MongoDB:**

Make sure Redis and MongoDB are running locally on localhost:6379, or configure the connection strings accordingly. Alternatively, use a managed Redis/MongoDB service. Redis can be scaled out in a clustered mode and can be configured with multiple nodes for high availability (if needed for production).

```bash
docker compose up -d
```

4. **Run the application:**

You can now start the application:

```bash
dotnet run
The application will start and be available on http://localhost:5000/graphql.
```

The GraphQL UI will be helpful in running the queries. It is super responsive and easier to debug.

5. **Test the GraphQL Queries**

Example GraphQL Queries, Mutations and Subscription

1.  **_createUser Mutation_**
    The createUser mutation is used to create a new user with location information.

    ```graphql
    mutation {
      createUser(
        userInput: { name: "Johny1", latitude: 37.7749, longitude: 122.4194 }
      ) {
        name
        latitude
        longitude
      }
    }
    ```

    **Description:**
    **userInput:** A set of fields (name, latitude, and longitude) used to create the user.
    **name:** The name of the user.
    **latitude:** The latitude of the user's location.
    **longitude:** The longitude of the user's location.
    **Response Fields:**
    **name:** The name of the newly created user.
    **latitude:** The latitude of the newly created user’s location.
    **longitude:** The longitude of the newly created user’s location.

2.  **_updateUserLocation Mutation_**
    The updateUserLocation mutation is used to update a user's location information.

    ```graphql
    mutation {
      updateUserLocation(
        userId: "e31e2b93-717d-443e-a342-a662539c9f2b"
        userInput: { name: "Johnyr1", latitude: 40.000, longitude: 152.4194 }
      ) {
        createdTimestamp
        latitude
        longitude
        name
        updatedTimestamp
        userId
      }
    }
    ```

    **Description:**
    **userId:** The ID of the user whose location is being updated.
    **userInput:** A set of fields (name, latitude, and longitude) that are used to update the user's location.
    **name:** The name of the user.
    **latitude:** The new latitude of the user's location.
    **longitude:** The new longitude of the user's location.
    **Response Fields:**
    **createdTimestamp:** The timestamp when the user's location was originally created.
    **latitude:** The updated latitude of the user.
    **longitude:** The updated longitude of the user.
    **name:** The updated name of the user.
    **updatedTimestamp:** The timestamp when the user's location was last updated.
    **userId:** The ID of the user whose location has been updated.

3.  **_userLocationUpdated Subscription_**
    The userLocationUpdated subscription is used to subscribe to updates on a specific user's location.

    ```graphql
    subscription {
      userLocationUpdated(userId: "e31e2b93-717d-443e-a342-a662539c9f2b") {
        userId
        name
        latitude
        longitude
        createdTimestamp
        updatedTimestamp
      }
    }
    ```

    **Description:**
    **userId:** The ID of the user to subscribe to. The client will receive location updates for this specific user.
    **Response Fields:**
    **userId:** The ID of the user whose location is being updated.
    name: The name of the user whose location has been updated.
    **latitude:** The updated latitude of the user.
    **longitude:** The updated longitude of the user.
    **createdTimestamp:** The timestamp when the user's location was created.
    **updatedTimestamp:** The timestamp when the user's location was last updated.

    Open Multiple tabs to test if all the subscribers get the updates.

4.  **_getUsers Query_**
    The getUsers query retrieves a list of all users.

    ```graphql
    query {
      getUsers {
        userId
        name
        latitude
        longitude
        createdTimestamp
        updatedTimestamp
      }
    }
    ```

    **Description:**
    Retrieves a list of all users and their associated location data.
    **Response Fields:**
    **userId:** The ID of the user.
    **name:** The name of the user.
    **latitude:** The latitude of the user's location.
    **longitude:** The longitude of the user's location.
    **createdTimestamp:** The timestamp when the user's location was created.
    **updatedTimestamp:** The timestamp when the user's location was last updated.

### 6. **Stop the Application and Docker Services**:

- When you are finished, stop the application by pressing Ctrl + C in the terminal where dotnet run is running.

- Also, bring down the Docker containers to stop Redis Local and MongoDB by running:

```bash
docker-compose down
```
