# Cryptotracker

## Project Overview

A fullstack cryptocurrency tracking application built with .NET 8 API, PostgreSQL, and React+TypeScript. The application provides real-time cryptocurrency price tracking, historical data visualization, and search functionality.

## Tech Stack

### Backend
- .NET 8 Web API
- Entity Framework Core
- PostgreSQL Database
- Quartz.NET for scheduled jobs

### Frontend
- React 18
- TypeScript
- Styled Components
- React Router
- Axios for API communication

## Getting Started

### Prerequisites
- .NET 8 SDK
- Node.js and npm
- PostgreSQL
- Git

### Installation

1. Clone the repository:
```bash
git clone https://github.com/yourusername/cryptotracker.git
cd cryptotracker
```

2. Set up the database:
```bash
# Update connection string in appsettings.json
cd Cryptotracker.Api
dotnet ef database update
```

3. Start the API:
```bash
cd Cryptotracker.Api
dotnet run
```

4. Start the frontend:
```bash
cd cryptotracker-ui
npm install
npm start
```

## Configuration

Update the following configuration files:
- `appsettings.json` - Database and API settings
- `src/services/api.ts` - API endpoint configuration

## Project Structure

```
Cryptotracker.sln
├── Cryptotracker.Api/          # REST API
│   ├── Controllers/            # API Controllers
│   ├── Program.cs             # Application Entry Point
│   └── appsettings.json       # Configuration
├── Cryptotracker.Worker/       # Background Worker
│   ├── Jobs/                  # Scheduled Jobs
│   ├── Program.cs             # Application Entry Point
│   └── appsettings.json       # Configuration
├── Cryptotracker.Shared/       # Shared Library
│   ├── Models/                # Database Models
│   ├── Dto/                   # Data Transfer Objects
│   ├── Data/                  # Database Context
│   └── Services/              # Interfaces and Implementations
└── cryptotracker-ui/          # Frontend Application
    ├── src/
    │   ├── components/        # React Components
    │   ├── services/          # API Services
    │   ├── styles/            # Styled Components
    │   └── types/            # TypeScript Types
    └── package.json
```

## Features
- Real-time cryptocurrency price tracking
- Historical price data visualization
- Search and filter functionality
- Responsive design for mobile and desktop
- Automatic data updates every 10 minutes

## Testing

Backend tests:
```bash
dotnet test
```

Frontend tests:
```bash
cd cryptotracker-ui
npm test
```

## Contributing

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request
