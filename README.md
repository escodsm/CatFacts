# CatFacts

## Overview

`CatFacts` is a simple C# console application that retrieves cat facts from a local API, categorizes them based on
specific rules, and displays the results grouped by status (`SUCCESS`, `WARNING`, `ERROR`). It also logs the results to
a file (`catfacts_log.txt`). The app is designed to process facts with the following rules:

-  **SUCCESS**: Facts that are unused (`used: false`) and created on or after February 1, 2018.
-  **WARNING**: Facts that are used (`used: true`) or created before February 1, 2018.
-  **ERROR**: Duplicate facts (based on the fact text), except for the special case below.

> This app is to help Sarah in accounting share cat facts with her colleagues.

# CatFacts Setup Script

This script provides step-by-step instructions to set up and run `CatFacts`, a C# console application that retrieves and
categorizes cat facts from an API.

## Prerequisites

Before starting, ensure you have:

-  **.NET 8.0 SDK**: Required to build and run the app.
-  **PowerShell**: For running commands (included on Windows; use PowerShell Core on macOS/Linux).
-  **Internet Access**: To download dependencies and access the API.
-  **CatFacts.zip**: The project archive containing `CatFacts.csproj`, `Program.cs`, `catfacts_log.txt` and this file.

# Setup Steps

1. Install .NET 8.0 SDK Visit https://dotnet.microsoft.com/download/dotnet/8.0 in a web browser. Download and install
   the .NET 8.0 SDK for your operating system (Windows, macOS, or Linux). Open PowerShell (Windows: search "PowerShell"
   in Start menu; macOS/Linux: use terminal with PowerShell Core) and verify:

> `dotnet --version`

Confirm the output shows 8.0.x (e.g., 8.0.100). If not, reinstall the SDK. macOS/Linux Note: Install PowerShell Core if
needed:

> `winget install --id Microsoft.PowerShell`

2. Extract the Project Locate CatFacts.zip (e.g., in your Downloads folder). Extract it to a directory, such as
   C:\Projects

Verify the CatFacts folder contains:

> -  CatFacts.csproj
> -  Program.cs
> -  README.md (this file)

3. Navigate to the Project Directory Open PowerShell. Change to the CatFacts directory:

> `cd C:\Projects\CatFacts`

Confirm you’re in the correct directory:

> `dir`

You should see:

> -  CatFacts.csproj
> -  Program.cs
> -  README.md (this file)

4. Install Dependencies Install the Newtonsoft.Json package for JSON parsing:

> `dotnet add package Newtonsoft.Json`

Wait for the command to complete (it downloads the package from NuGet).

5. Build the Application Compile the project:

> `dotnet build`

Look for "Build succeeded" in the output. If errors occur, check: **.NET 8.0 SDK is installed (dotnet --version).
**Newtonsoft.Json was installed (dotnet add package Newtonsoft.Json). \*\*CatFacts.csproj and Program.cs are present.

6. Run the Application Execute the app:

> `dotnet run`

##### The app will:

> Fetch cat facts from the API. Categorize them based on rules:

-  SUCCESS: Unused (used: false) and created on/after 2018-02-01.
-  WARNING: Used (used: true) or created before 2018-02-01.
-  ERROR: Duplicates (Fact text)

> Displays grouped results in the console. Log results to catfacts_log.txt in the project directory.

@2025 Steve Cordeiro
