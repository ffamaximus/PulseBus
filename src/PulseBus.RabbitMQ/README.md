# PulseBus.RabbitMQ

![PulseBus.RabbitMQ Banner](https://raw.githubusercontent.com/ffamaximus/PulseBus/refs/heads/main/Banner.PNG)

A RabbitMQ provider for PulseBus, the minimalistic, extensible, and provider-agnostic messaging abstraction for .NET.

## ✨ Features

*   **Asynchronous Producer and Consumer**: Efficient and asynchronous message handling.
*   **Resilient Connection Handling**: Robust connection management to ensure message delivery.
*   **Manual ACK/NACK**: Granular control over message acknowledgment.
*   **Middleware Pipeline**: Extend and customize message processing.
*   **Idempotency Support**: Prevents duplicate message processing.
*   **Retry Policies**: Configure automatic retries for failed messages.
*   **Metadata & Headers**: Full support for message metadata and custom headers.
*   **Fully Compatible with PulseBus Core**: Seamless integration with the PulseBus ecosystem.

## 📦 Installation

Install the `PulseBus.RabbitMQ` NuGet package:

```bash
dotnet add package PulseBus.RabbitMQ
```

## 🚀 Usage

### Configure PulseBus with RabbitMQ

Integrate RabbitMQ into your PulseBus configuration:

```csharp
services.AddPulseBus(builder =>
{
    builder.UseRabbitMq(options =>
    {
        options.Host = "localhost";
        options.Username = "guest";
        options.Password = "guest";
    });
});
```

### Publish a Message

Send messages using the configured bus:

```csharp
await bus.PublishAsync("user.created", new UserCreated { Id = 1 });
```

### Consume a Message

Subscribe to messages and process them:

```csharp
bus.Subscribe<UserCreated>("user.created", async (msg, ctx) =>
{
    Console.WriteLine($"User created: {msg.Id}");
    await ctx.AcknowledgeAsync();
});
```

## 💖 Support

This project is developed and maintained by **Andrés Mariño**. If you find this library useful, consider supporting its continued development:

- **Bitcoin (BTC):** `bc1p9zqgxghkjhauruhsza9n382e6kp5tpj4xtzu2csv4mypsdtdc4tqvdyg86`
- **Ko-fi:** [![Support Me](https://img.shields.io/badge/Ko--fi-Support%20Me-red?style=flat-square&logo=ko-fi)](https://ko-fi.com/andresdev21)

---

## 📄 License

This project is licensed under the MIT License.
