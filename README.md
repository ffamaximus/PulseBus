![PulseBus Banner](https://github.com/ffamaximus/PulseBus/blob/main/Banner.PNG?raw=true)


# 🚌 PulseBus

[![NuGet Version](https://img.shields.io/nuget/v/PulseBus.svg)](https://www.nuget.org/packages/PulseBus/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
![.NET Core](https://img.shields.io/badge/.NET-%3E%3D%206.0-blue.svg)

**Fast. Minimal. Universal.**

PulseBus is a lightweight, provider-agnostic messaging abstraction for .NET, designed to unify message publishing and consuming across multiple providers with a focus on **Clean Architecture** and **High Performance**.

---

## 📑 Table of Contents
- [Features](#-features)
- [Installation](#-installation)
- [Quick Start](#-quick-start)
- [Usage](#-usage)
  - [Publishing](#publishing)
  - [Subscribing](#subscribing)
- [Architecture](#-architecture)
- [Supported Providers](#-supported-providers)
- [Support](#-support)
- [License](#-license)

---

## 🚀 Features

- **Unified API:** Single interface to interact with RabbitMQ, SQS, Kafka, etc.
- **Middleware Pipeline:** Extensible pipeline similar to ASP.NET Core.
- **Flexible Serialization:** Support for JSON, Protobuf, MessagePack, and custom serializers.
- **Resilience:** Built-in retry policies and idempotency support.
- **Lightweight:** Zero external dependencies in the core (`PulseBus` package).
- **Extensible:** Easily add new providers through a plugin-based design.

---

## 📦 Installation

Install the base package via NuGet:

```bash
dotnet add package PulseBus
```

Then, add the provider you need:

| Provider | NuGet Package |
| :--- | :--- |
| **RabbitMQ** | `dotnet add package PulseBus.RabbitMQ` |
| **AWS SQS** | `dotnet add package PulseBus.SQS` |
| **Azure Queue** | `dotnet add package PulseBus.AzureQueue` |
| **Kafka** | `dotnet add package PulseBus.Kafka` |

---

## 🧩 Quick Start

Register PulseBus in your dependency injection container (`Program.cs` or `Startup.cs`):

```csharp
using PulseBus.Extensions;

services.AddPulseBus(builder =>
{
    // Configure your provider
    builder.UseRabbitMq(options =>
    {
        options.Host = "localhost";
        options.Username = "guest";
        options.Password = "guest";
    });

    // Select your preferred serializer
    builder.UseJsonSerializer();
});
```

---

## 💻 Usage

### Publishing
Send messages asynchronously to any configured bus:

```csharp
await bus.PublishAsync("user.requested", new UserRequested
{
    Email = "andres@example.com",
    Name = "Andrés"
});
```

### Subscribing
Define message handlers easily:

```csharp
bus.Subscribe<UserRequested>("user.requested", async (msg, ctx) =>
{
    Console.WriteLine($"Processing: {msg.Email}");
    
    // Acknowledge the message
    await ctx.AcknowledgeAsync();
});
```

---

## 🧱 Architecture

PulseBus is built on solid interfaces that allow you to decouple your business logic from the messaging infrastructure:

*   `IMessageBus`: Main entry point.
*   `IMessageProducer` / `IMessageConsumer`: Transport abstractions.
*   `IMessageMiddleware`: For logging, validation, or telemetry.
*   `IIdempotencyStore`: Integrated duplicate control.

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
