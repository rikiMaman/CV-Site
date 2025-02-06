CV Site - Portfolio API

📌 Overview

CV Site is a web application that allows developers to showcase their GitHub projects in a personalized portfolio. The application connects to the developer's GitHub account, retrieves repositories, and displays key project information.

🚀 Features

Fetches and displays a developer's GitHub repositories

Provides details such as:

Programming languages used

Last commit date

Star count

Number of pull requests

Repository website link (if available)

Supports searching public repositories on GitHub by:

Repository name

Programming language

GitHub username

Caching for improved performance

🛠️ Technologies Used

Backend: .NET Core Web API

GitHub API: Octokit.NET

Caching: In-memory caching

📂 Project Structure

The project consists of two main components:

Service Library: Manages the connection to GitHub and retrieves repository data.

Web API: Uses the service library and provides API endpoints for retrieving and searching repositories.

🔧 Installation & Setup

Clone the Repository

git clone https://github.com/rikiMaman/CV-Site.git
cd CV-Site

Set Up API Authentication

Create a GitHub personal access token (GitHub Guide)

Store the token securely in secrets.json:

{
  "GitHub": {
    "Token": "your_personal_access_token",
    "Username": "your_github_username"
  }
}

Run the Application

Open the project in Visual Studio

Restore dependencies:

dotnet restore

Build and run the API:

dotnet run

📡 API Endpoints

🔍 Get Portfolio Repositories

Endpoint: GET /portfolio

Response Example:

[
  {
    "name": "my-repo",
    "language": "C#",
    "lastCommit": "2024-08-10T12:30:00Z",
    "stars": 42,
    "pullRequests": 5,
    "repoUrl": "https://github.com/user/my-repo"
  }
]

🔎 Search Public Repositories

Endpoint: GET /search

Query Parameters:

name (optional) - Repository name

language (optional) - Programming language

user (optional) - GitHub username

🏗️ Caching Strategy

Portfolio data is cached to optimize performance.

Cache is refreshed every few minutes to keep data up to date.

🤝 Contributing

Contributions are welcome! If you have suggestions or improvements, feel free to fork the repository and submit a pull request.

📜 License

This project is licensed under the MIT License.

🚀 Built with .NET Core and GitHub API integration!

