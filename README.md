# Power Grid Outage Reporting and Detection System

This project is a web application for reporting, detecting, and monitoring outages in an electrical power distribution network.  
The system enables efficient collection of outage reports, automatic analysis of potential fault locations, and real-time visualization of the current network state.

The application supports multiple user roles (Administrator, Customer, and Guest), each with clearly defined permissions and functionalities.

---

## Technology Stack

### Backend
- **.NET 8**
- **ASP.NET Core Web API**
- **Entity Framework Core**
- **Microsoft SQL Server (MSSQL)**
- **JWT-based authentication and authorization**
- **RESTful API architecture**

### Frontend
- **Angular 19.1**
- **TypeScript**
- **Angular Router**
- **Angular HTTP Client**
- **Map integration (Google Maps API or Leaflet / OpenStreetMap)**
- **Data visualization libraries (e.g. Chart.js)**

---

## System Overview

The system is designed to improve transparency and efficiency in handling power outages by providing:
- A centralized platform for outage reporting
- Automatic estimation of fault locations based on user reports and network topology
- Real-time visualization of the power grid
- Historical analysis of outages for maintenance planning

The electrical network is modeled as a **radial structure**, meaning there is a unique path between any two nodes (substations).

---

## User Roles and Permissions

### Administrator
The administrator has full access to the system and is responsible for managing the power grid model and outage resolution process.

Main responsibilities:
- Managing substations and power lines
- Maintaining the logical network model
- Viewing and managing all reported outages
- Organizing and tracking outage resolution
- Monitoring outage duration and statistics
- Managing user accounts
- Accessing outage history per power line

---

### Customer
A customer is a registered user who can report outages and track their resolution status.

Main features:
- Reporting outages
- Automatic estimation of fault location based on geographic data
- Viewing all reported outages
- Tracking the status of reported outages
- Viewing outage history
- Receiving notifications about status changes
- Calendar-based view of notifications

Constraints:
- A customer can report multiple outages
- The same outage cannot be reported multiple times for the same object

---

### Guest
A guest is an unregistered user with read-only access.

Available features:
- Viewing the current state of the power grid
- Viewing reported outages and their status
- Viewing outage locations on the map

Guests cannot report outages or interact with the system data.

---

## Core Functionalities

### Outage Reporting
- Customers can submit outage reports
- The system automatically estimates the potential fault location
- Multiple reports are analyzed to narrow down the affected area

---

### Network Visualization
- Interactive map displaying:
  - Substations
  - Power lines
  - Active and resolved outages
- Different visual indicators based on outage status

---

### Automatic Fault Location Detection
- Uses:
  - Customer geographic location
  - Logical network topology
- Determines the most probable fault segment or substation
- Provides a fault zone rather than an exact location

---

### Outage History and Analysis
- Each power line maintains an outage history
- Stored data includes:
  - Date and time of outage
  - Reporting user
  - Outage duration
  - Resolution status
- Data is available in tabular and graphical formats
- Supports long-term reliability analysis

---

### Notification System
- Customers receive notifications when:
  - Outage status changes
  - Relevant network segments are affected
- Notifications are available:
  - In-application
  - In calendar view

---

### Security and Access Control
- Secure registration and login
- JWT-based authentication
- Role-based authorization:
  - Administrator
  - Customer
  - Guest

---

## Database

The database stores:
- Users and roles
- Substations
- Power lines
- Outage reports
- Outage history
- Notifications

Microsoft SQL Server is used to ensure:
- Reliable data storage
- Efficient querying
- Support for time-based and relational data

---

## API Integrations

- Map services (Google Maps API or OpenStreetMap/Leaflet)
- Optional notification services (email or internal notifications)

---

## Optional Features

- Export outage data to CSV format
- Statistical and graphical outage analysis
- Preventive maintenance insights based on historical data

---

## Project Goals

- Faster detection and localization of power outages
- Improved organization of outage resolution
- Increased transparency for customers and the public
- Better long-term maintenance planning of the power grid

---
