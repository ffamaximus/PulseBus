# PulseBus.Outbox

**Transactional Outbox for PulseBus**

Guarantees consistency between your database and your message broker (e.g., RabbitMQ, Kafka, SQS) using the Outbox Pattern.

PulseBus.Outbox ensures:
*   **Consistency**: Your domain state and events are always consistent.
*   **Reliability**: No messages are lost or duplicated.
*   **Resilience**: Publishing is retried safely, and event delivery is idempotent.
*   **Robustness**: Sagas and distributed workflows are reliable.

---

## 🚀 Features

*   **EF Core Outbox Store**: Seamless integration with Entity Framework Core.
*   **Background Outbox Processor**: Efficiently processes and dispatches messages.
*   **JSON Serializer**: Default serialization for message payloads.
*   **Middleware for automatic event registration**: Simplifies event handling.
*   **Publisher abstraction**: Works with various PulseBus implementations (e.g., PulseBus.RabbitMQ, PulseBus.Kafka).
*   **Configurable**: Customize batch size, interval, retries, and backoff strategies.
*   **High-Performance**: Fully asynchronous design for optimal performance.

---

## 📦 Installation

To install PulseBus.Outbox, run the following command in your project:

```bash
dotnet add package PulseBus.Outbox
```

---

## 🧩 How It Works

The Outbox Pattern ensures atomicity between your business logic and message publishing. Here's the typical flow:

1.  **Application Command Execution**: Your application executes a command that modifies the domain state.
2.  **Transactional Persistence**:
    *   Inside the same database transaction, the domain state is persisted.
    *   An `OutboxMessage` is inserted into the `OutboxMessages` table.
3.  **Outbox Processor**: A background process (the Outbox Processor) reads pending messages from the `OutboxMessages` table.
4.  **Event Publishing**: Events are published to the configured message broker.
5.  **Message Marking**: Once successfully published, messages are marked as processed in the `OutboxMessages` table.

This guarantees **at-least-once delivery** and **strong consistency** between your database and your message broker.

---

## 🛠️ Usage

Follow these steps to integrate PulseBus.Outbox into your application:

### 1. Register Outbox Services

Add the Outbox services to your `IServiceCollection`:

```csharp
services.AddPulseBusOutbox(options =>
{
    options.ProcessingIntervalMs = 500; // Interval for processing messages in milliseconds
    options.MaxBatchSize = 50;          // Maximum number of messages to process in a single batch
});
```

### 2. Add EF Core Outbox DbContext

Configure your `DbContext` to use the Outbox:

```csharp
services.AddDbContext<OutboxDbContext>(options =>
    options.UseSqlServer(connectionString)); // Or UsePostgreSQL, UseSqlite, etc.
```

### 3. Add Outbox Middleware to PulseBus

Integrate the `OutboxMiddleware` into your PulseBus configuration:

```csharp
services.AddPulseBus(bus =>
{
    bus.UseMiddleware<OutboxMiddleware>();
});
```

---

## 🧱 Outbox Table Schema

The `OutboxMessages` table stores the events before they are published. Here's an example schema:

```sql
CREATE TABLE OutboxMessages (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    Type NVARCHAR(500) NOT NULL,
    Payload NVARCHAR(MAX) NOT NULL,
    CreatedAt DATETIME2 NOT NULL,
    ProcessedAt DATETIME2 NULL
);
```

---

## 🧬 Architecture

```
+---------------------+
|       Command       |
+---------------------+
          ↓
+---------------------+
| Database Transaction|
| (Domain State +     |
|  OutboxMessage)     |
+---------------------+
          ↓
+---------------------+
|   Outbox Processor  |
+---------------------+
          ↓
+---------------------+
|    Message Broker   |
| (RabbitMQ, Kafka,   |
|        SQS)         |
+---------------------+
          ↓
+---------------------+
| Consumers / Sagas   |
+---------------------+
```

---

## 🧪 Example: Writing to Outbox

When you want to publish an event, you typically create an `OutboxMessageEntity` and save it within your existing database transaction:

```csharp
// Assuming 'evt' is your domain event and 'serializer' is an IMessageSerializer
var evt = new OrderCreated(order.Id, order.Total);

var msg = new OutboxMessageEntity
{
    Id = Guid.NewGuid(),
    Type = evt.GetType().FullName!,
    Payload = serializer.Serialize(evt) // Serialize your event object
};

db.OutboxMessages.Add(msg);
await db.SaveChangesAsync(); // Save changes within the same transaction
```

---

## 💖 Support

This project is developed and maintained by **Andrés Mariño**. If you find this library useful, consider supporting its continued development:

- **Bitcoin (BTC):** `bc1p9zqgxghkjhauruhsza9n382e6kp5tpj4xtzu2csv4mypsdtdc4tqvdyg86`
- **Ko-fi:** [![Support Me](https://img.shields.io/badge/Ko--fi-Support%20Me-red?style=flat-square&logo=ko-fi)](https://ko-fi.com/andresdev21)

---

## 📝 License

This project is licensed under the **MIT License**. See the [LICENSE](LICENSE) file for details.

---

Made with ❤️ for the .NET community
