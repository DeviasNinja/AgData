# Automated API and UI Test Suite

## Project Overview

This repository contains an automated test suite for API and UI testing using xUnit for C# and Selenium WebDriver. The purpose of this project is to demonstrate a comprehensive approach to test automation as part of a technical assessment.

The project consists of two parts:
1. **API Testing**: Tests for RESTful API endpoints using xUnit and HTTPClient.
2. **UI Testing**: Web-based UI tests using Selenium WebDriver and ChromeDriver.

## Table of Contents
- [Project Overview](#project-overview)
- [Features](#features)
- [Installation](#installation)
- [Usage](#usage)
- [API Test Suite](#api-test-suite)
- [UI Test Suite](#ui-test-suite)
- [Logging and Reporting](#logging-and-reporting)
- [Parallel Execution](#parallel-execution)
- [Contributing](#contributing)

## Features

- **Automated API tests** covers main CRUD operations for the Placeholder API.
- **UI tests** for validating workflows on the website.
- **Parallel test execution** for faster runs.
- **Detailed logging and reports** for easy troubleshooting.

## Installation

### Prerequisites

- .NET SDK (4.8 or higher)
- Visual Studio 2022
- Chrome browser
- ChromeDriver (managed via NuGet)
- NuGet packages: `Newtonsoft.Json`, `xunit`, `xunit.runner.visualstudio`, `Selenium.WebDriver`, `Selenium.WebDriver.ChromeDriver`, `Selenium.Support`, `DotNetSeleniumExtras.WaitHelpers`

### Steps

1. Clone the repository:
    ```bash
    git clone https://github.com/DeviasNinja/AgData
    ```
2. Open the solution in Visual Studio.
3. Restore the NuGet packages:
    ```bash
    dotnet restore
    ```
4. Build the solution:
    ```bash
    dotnet build
    ```

## Usage

### Running Tests via Visual Studio

1. Open **Test Explorer** in Visual Studio.
2. Build the solution to load the tests.
3. Run all tests or individual test categories (API or UI) from Test Explorer.

## API Test Suite

The API test suite covers both **happy path** and **negative path** scenarios for the following endpoints:

- **GET** `/posts`
- **POST** `/posts`
- **PUT** `/posts/{postId}`
- **DELETE** `/posts/{postId}`
- **POST** `/posts/{postId}/comments`
- **GET** `/comments?postId={postId}`

### Features

- Tests are data-driven using parameters to ensure wide coverage.
- Detailed logs are generated for both success and failure scenarios. (Will discuss failure and why they failed)

## UI Test Suite

The UI tests navigate the AGDATA website and verify key functionality, including:

- Navigating to the **Company Overview** page.
- Extracting the headings of the **Our Values** section.
- Clicking on **Let's Get Started** and verifying the **Contact** page loads.

### Features

- Utilizes the **Page Object Model (POM)** for cleaner test code.
- Incorporates explicit waits to handle dynamic elements and ensure reliability.

## Logging and Reporting

Detailed logs and reports are generated for both the API and UI tests. You can find the logs under the `/Reports` directory. These logs provide useful information about test runs, including failures and their reasons, making debugging easier.

## Parallel Execution

The tests can be executed in parallel to speed up the testing process. This is enabled in the xUnit configuration file.

## Contributing

Contributions are welcome! Feel free to fork this repository and submit pull requests for any improvements or additional features.
