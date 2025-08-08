
# 🏥 HealthApp

A comprehensive **healthcare management system** built with ASP.NET 9.0, providing streamlined appointment scheduling, role-based user management, digital prescriptions, and real-time email notifications.

---

## 📁 Project Structure

This solution consists of the following projects:

### 1. **HealthApp.Domain**
Core business logic and domain models  
Contains:
- Entities: `User`, `Doctor`, `Patient`, `Appointment`, `Prescription`, etc.
- DTOs for data transfer
- Business rules and constants

### 2. **HealthApp.Razor**
Frontend built with **Razor Pages**  
Features:
- User interface for patients, doctors, and admins
- Role-based login and dashboards
- Appointment scheduling
- Responsive UI with Bootstrap
- Real-time notifications

### 3. **HealthApp.Tests**
Unit test project for backend services and business logic

---

## 🌟 Key Features

### ✅ Appointment Management
- Schedule, approve, reject or cancel medical appointments  
- Track appointment status  
- View upcoming and past consultations

### ✅ User Management
- Microsoft Identity integration  
- Login/register with **Admin**, **Doctor**, and **Patient** roles  
- Profile management and role-based dashboards

### ✅ Prescription Management
- Issue and store digital prescriptions  
- View prescription history by patient or doctor

### ✅ Notification System
- Real-time email alerts using RabbitMQ  
- Appointment status updates and reminders

---

## ⚙️ Technology Stack

### 🔧 Backend
- ASP.NET Core 9.0
- Entity Framework Core
- AutoMapper
- SQLite (dev) / SQL Server (prod)

### 🎨 Frontend
- Razor Pages
- Bootstrap 5
- JavaScript & jQuery

### 🧱 Infrastructure
- SMTP Email Service
- Swagger/OpenAPI
- CORS Support

---

## 🚀 Getting Started

### 🔐 Prerequisites
- [.NET 9.0 SDK](https://dotnet.microsoft.com/download)
- [SQLite](https://www.sqlite.org/download.html)
- [RabbitMQ Server](https://www.rabbitmq.com/download.html)

---

### 🛠️ Installation

```bash
git clone https://github.com/seu-usuario/HealthApp.git
cd HealthApp
dotnet restore
dotnet build

⚙️ Configuration
Update the following files as needed:

appsettings.json (Database connection strings)

HealthApp.API/appsettings.json (RabbitMQ & Email service configuration)

▶️ Running the App
Run the API

cd HealthApp.API
dotnet run

Run the Razor UI

cd HealthApp.Razor
dotnet run

The application will be available at:

API: https://localhost:7001

Razor UI: https://localhost:5101



Video Demo​

https://drive.google.com/file/d/1ptqHvKflwE26hEsNvVbiPSeFKcpoSsq4/view?usp=drive_link
