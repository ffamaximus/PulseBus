# ⚡ PulseBus.Extensions

`PulseBus.Extensions` is an extension pack designed to enhance `PulseBus` with production-ready features.

---
## ✨ Features

This library provides a comprehensive set of extensions, including:

### Serialization
*   **System.Text.Json** (Default)
*   **Newtonsoft.Json** (Optional)
*   **Protobuf** (Optional)

### Middlewares
*   **Logging**: For comprehensive event logging.
*   **Retry**: Implements robust retry mechanisms.
*   **Metrics** (Optional): For performance monitoring.
*   **Circuit Breaker** (Optional): Enhances fault tolerance.

### Retry Policies
*   **Exponential Retry**: Retries with exponentially increasing delays.
*   **Linear Retry**: Retries with fixed delays.
*   **No Retry**: Disables retry attempts.

### Idempotency
*   **In-memory Idempotency Store**: Ensures message processing occurs only once.
---
## 📦 Installation

To integrate `PulseBus.Extensions` into your project, use the following .NET CLI command:

```bash
dotnet add package PulseBus.Extensions
```
---
## 🚀 Usage

### Adding PulseBus with Default Extensions

You can easily configure `PulseBus` with a standard set of extensions:

```csharp
services.AddPulseBus(builder =>
{
    builder.UseRabbitMq(options =>
    {
        options.Host = "localhost";
        options.Username = "guest";
        options.Password = "guest";
    });

    builder.AddDefaultExtensions();
});
```
---
### Manual Configuration

For more granular control, you can manually configure individual extensions:

```csharp
builder.UseJsonSerializer();
builder.UseExponentialRetry(5);
builder.UseRetryMiddleware();
builder.UseInMemoryIdempotency();
builder.UseLogging();
```
---
## 🧱 Architecture

`PulseBus.Extensions` is modularly designed, with features organized into distinct directories:

*   `Serialization/`
*   `Middlewares/`
*   `RetryPolicies/`
*   `Idempotency/`
*   `Extensions/`

Each module is optional and can be enabled independently, allowing you to include only the functionalities your application requires.

---

## 💖 Support

This project is developed and maintained by **Andrés Mariño**. If you find this library useful, consider supporting its continued development:

- **Bitcoin (BTC):** `bc1p9zqgxghkjhauruhsza9n382e6kp5tpj4xtzu2csv4mypsdtdc4tqvdyg86`
- **Ko-fi:** [![Support Me](https://img.shields.io/badge/Ko--fi-Support%20Me-red?style=flat-square&logo=ko-fi)](https://ko-fi.com/andresdev21)

---
## 📝 License

This project is licensed under the MIT License.
