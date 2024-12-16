# WebhookDemo

**Webhook Demo** is a lightweight application designed to demonstrate webhook subscription management and event simulation. The project provides a platform for developers to subscribe to events, simulate event triggers, and test webhook integrations seamlessly. With in-memory storage, it is ideal for educational purposes and small-scale testing.

## Table of Contents

- [**Features**](#features)
- [**Technologies Used**](#technologies-used)
- [**Getting Started**](#getting-started)
  - [**Prerequisites**](#prerequisites)
  - [**Installation**](#installation)

---

## Features

- **Webhook Subscriptions**: Manage subscriptions for different event types.
- **Event Simulation**: Simulate events to trigger webhook calls.
- **In-Memory Storage**: Store subscriptions and logs in memory for simplicity.
- **Unit Testing**: Comprehensive test coverage using xUnit and Moq.

---

## Technologies Used

- **ASP.NET Core 6.0**: Backend framework.
- **xUnit**: Unit testing framework.
- **Moq**: Mocking library for unit tests.
- **HttpClient**: For handling HTTP requests.
- **JsonSerializer**: For serializing and deserializing JSON data.

---

## Getting Started

### Prerequisites

- .NET 6.0 SDK
- Visual Studio or any C# IDE

### Installation

1. Clone the repository:

   ```bash
   git clone https://github.com/your-repo/webhook-demo.git
   cd webhook-demo
