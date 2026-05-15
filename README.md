# FixFlow - Field Service Management (SaaS)

> Note: This repository is currently under active development.

## Project Overview
FixFlow is a comprehensive Software as a Service (SaaS) platform engineered to orchestrate field operations, manage work orders, and establish a reliable technological bridge between centralized administration and deployed technicians. 

This repository serves as a public technical showcase of the core architecture and development patterns used in the project. Please note that sensitive business logic, proprietary algorithms, and client-specific integrations have been sanitized or removed for confidentiality.

## Core Architecture & Tech Stack

The system is built upon a distributed architecture, emphasizing scalability, data security, and resilience in low-connectivity environments.

### Backend & Cloud
* **Framework:** .NET 8 / ASP.NET Core Web API
* **ORM:** Entity Framework Core
* **Database:** Relational SQL Database (Multi-Tenant Architecture)
* **Authentication:** JWT (JSON Web Tokens) with Role-Based Access Control (RBAC)
* **Deployment:** Containerized via Docker, orchestrated on a Linux VPS with a Reverse Proxy.

### Mobile Client
* **Framework:** .NET MAUI (Multi-platform App UI)
* **Target Platforms:** Android
* **Local Storage:** SQLite
* **Features:** Native hardware integration (Camera API for evidence capture, touch events for digital signatures).

## Key Technical Features

### 1. Logical Multi-Tenancy
The backend is designed to support multiple contractor companies (Tenants) within a single database instance without data leakage. This is achieved through strict logical isolation using `TenantId` foreign keys and the implementation of **Global Query Filters** at the Entity Framework Core level, ensuring all database transactions inherently respect tenant boundaries.

### 2. Offline-First Synchronization
Field technicians frequently operate in areas with zero network coverage. The mobile application utilizes a local SQLite database to store assigned work orders and capture operational evidence (images, signatures). A custom synchronization algorithm, based on timestamps and atomic transactions, ensures data integrity and resolves conflicts once network connectivity is restored.

### 3. Inbound API Integration
The platform acts as a central hub and includes dedicated, secure RESTful endpoints designed to ingest work orders generated from external third-party CRM and ERP systems, allowing seamless operational continuity for enterprise clients.

## System Workflow
1. **Ingestion:** External systems or internal administrators generate Work Orders.
2. **Dispatch:** Orders are assigned and pushed to specific technicians.
3. **Execution (Offline/Online):** Technicians receive tasks, update statuses, and collect audit data via the mobile application.
4. **Synchronization:** Data is synced back to the central server, updating the administrative dashboard in real-time.

## Disclaimer
This is a showcase repository. For security reasons, environment variables, production deployment scripts, and specific external integration credentials are not included.
