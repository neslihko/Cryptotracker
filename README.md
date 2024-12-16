<<<<<<< HEAD
# Getting Started with Create React App

This project was bootstrapped with [Create React App](https://github.com/facebook/create-react-app).

## Available Scripts

In the project directory, you can run:

### `npm start`

Runs the app in the development mode.\
Open [http://localhost:3000](http://localhost:3000) to view it in the browser.

The page will reload if you make edits.\
You will also see any lint errors in the console.

### `npm test`

Launches the test runner in the interactive watch mode.\
See the section about [running tests](https://facebook.github.io/create-react-app/docs/running-tests) for more information.

### `npm run build`

Builds the app for production to the `build` folder.\
It correctly bundles React in production mode and optimizes the build for the best performance.

The build is minified and the filenames include the hashes.\
Your app is ready to be deployed!

See the section about [deployment](https://facebook.github.io/create-react-app/docs/deployment) for more information.

### `npm run eject`

**Note: this is a one-way operation. Once you `eject`, you can’t go back!**

If you aren’t satisfied with the build tool and configuration choices, you can `eject` at any time. This command will remove the single build dependency from your project.

Instead, it will copy all the configuration files and the transitive dependencies (webpack, Babel, ESLint, etc) right into your project so you have full control over them. All of the commands except `eject` will still work, but they will point to the copied scripts so you can tweak them. At this point you’re on your own.

You don’t have to ever use `eject`. The curated feature set is suitable for small and middle deployments, and you shouldn’t feel obligated to use this feature. However we understand that this tool wouldn’t be useful if you couldn’t customize it when you are ready for it.

## Learn More

You can learn more in the [Create React App documentation](https://facebook.github.io/create-react-app/docs/getting-started).

To learn React, check out the [React documentation](https://reactjs.org/).
=======
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
>>>>>>> 212b241b72a63feea09c74d47e03ae490df0c270
