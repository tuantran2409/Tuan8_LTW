# Book Management System

A full-stack book management solution featuring an ASP.NET Core Web API backend and a Windows Forms desktop frontend.

## Features

- **Book API**: RESTful API for managing books and categories.
- **Windows Forms Client**: Desktop application for CRUD operations on books.
- **Image Management**: Seamless handling of book cover images.
- **Data Persistence**: SQLite database for efficient local storage.

## Project Structure

- `BookAPI/`: Backend service built with ASP.NET Core 8.0.
- `BookWinForms/`: Desktop client built with .NET Windows Forms.

## Prerequisites

- .NET 8.0 SDK or higher
- .NET 10.0 SDK for the WinForms client

## Getting Started

### 1. API Setup
1. Open terminal in `BookAPI/`
2. Run `dotnet restore`
3. Run `dotnet run`

The API will be available at `http://localhost:5033` (default configuration).

### 2. Desktop Client Setup
1. Open terminal in `BookWinForms/`
2. Run `dotnet restore`
3. Run `dotnet run`

## Built With

- **Backend**: ASP.NET Core, Entity Framework Core (SQLite), Swagger/OpenAPI.
- **Frontend**: Windows Forms (.NET), Newtonsoft.Json for API communication.
- **Database**: SQLite.
