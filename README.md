# ____TfL.RoadStatus.ConsoleUI____

Returns the Road Status for one or more specified major roads using the [TfL open data](https://api.tfl.gov.uk) REST API.       

<img src=".\assets\img\logo.jpg" alt="logo" width="58%"/>

## ____Installation____

### Download and install.

- [.NET Core 3.1](https://dotnet.microsoft.com/download/dotnet/3.1) SDK and .NET Runtime. _(Required)_
- The latest version of [Visual Studio](https://visualstudio.microsoft.com/downloads/)  _(Optional, but recommended)_

### Build with Visual Studio

- In the menubar, using the dropdown, change _`Debug`_ to _`Release`_. Build following the instructions [here](https://docs.microsoft.com/en-us/visualstudio/get-started/csharp/run-program?view=vs-2019).
- Open _File Explorer (Windows) or Finder (MacOS)_, navigate to _and select_: _`
.\src\TfL.RoadStatus.ConsoleUI\release\netcoreapp3.1\TfL.RoadStatus.ConsoleUI.exe (Windows) or TfL.RoadStatus.ConsoleUI (MacOS)
`_ 
- Drag this executable into a Command Prompt _(Windows)_ or Terminal _(MacOS)._

### Alternatively to build without Visual Studio

- In a _Command Prompt (Windows) or Terminal (MacOS/ Linux)_ navigate to the same directory as README.md _(using the [`cd`](https://www.computerhope.com/issues/ch001310.htm) command)_. 
- Then execute the following commands:
```bash
dotnet build TfL.RoadStatus.sln --configuration Release &&
cd .\src\TfL.RoadStatus.ConsoleUI\release\netcoreapp3.1\
 ```
- _Optional:_ Use the [_`ls`_](https://www.freecodecamp.org/news/the-linux-ls-command-how-to-list-files-in-a-directory-with-options/) command to check _`TfL.RoadStatus.ConsoleUI.exe (Windows) or TfL.RoadStatus.ConsoleUI (MacOS)`_ has been created.

---
## Usage


```bash
PS C:\> .\TfL.RoadStatus.ConsoleUI.exe

TfL.RoadStatus.ConsoleUI 1.0.0

ERROR(S):
  A required value not bound to option name is missing.

  --apiurl            Required, but can be set as an environment variable
                      instead. E.g: https://api.tfl.gov.uk.

  --appid             Optional. (Included only to support legacy registrations)

  --apikey            Recommended. Register at https://api-portal.tfl.gov.uk

  RoadIds (pos. 0)    Required. Space-delimited. Note: RoadIds with spaces must
                      be escaped e.g: city%20route

PS C:\Users\AidanBunceWaters> $lastexitcode
2
```

```bash
PS C:\> .\TfL.RoadStatus.ConsoleUI.exe A1 A3 city%20route --apiurl https://api.tfl.gov.uk

The status of the A1 is as follows
         Road Status is Good
         Road Status Description is No Exceptional Delays

The status of the A3 is as follows
         Road Status is Good
         Road Status Description is No Exceptional Delays

The status of the City Route is as follows
         Road Status is Closure
         Road Status Description is Closure

PS C:\Users\AidanBunceWaters> $lastexitcode
0
```

```bash
PS C:\> .\TfL.RoadStatus.ConsoleUI.exe RoadWithoutTraffic --apiurl https://api.tfl.gov.uk

RoadWithoutTraffic is not a valid road

PS C:\Users\AidanBunceWaters> $lastexitcode
1
```


----

## Tests

<img src=".\assets\img\resharper-test-coverage.jpg" alt="resharper test window shows 92% total overall coverage, and 88% production code coverage" width="60%"/>

- __92% Test Coverage__ across all projects _(Total)_
- Consists of __Unit Tests__, developed with a __Test Driven Development (TDD)__ approach _(red -> green)_. Refactored after to include more comprehensive happy and unhappy path test data. 

- __ConsoleUI.AcceptanceTests__ end-to-end tests using the ___real___ TfL API, written using Specflow.
- __Application.IntegrationTests__ using ___moq'd___ HTTP responses to simulate the TfL API.
- Test names are as per __Behaviour Driven Development (BDD)__ _'Given, When Then'_ specification.
  

---
## To run the tests:

There are a few options for running the tests:
### Use the _Resharper (R#) [Unit Test Explorer](https://www.jetbrains.com/help/resharper/Reference__Windows__Unit_Test_Explorer.html)  (Recommended. Fast and reliable)_

- [dotCover](https://www.jetbrains.com/dotcover/) is a great tool for calculating Test Coverage. It includes coverage from ConsoleUI.AcceptanceTests.

### Visual Studio's built-in _[Test Explorer](https://docs.microsoft.com/en-us/visualstudio/test/run-unit-tests-with-test-explorer?view=vs-2019)     (Slower, but just as reliable)_

-  <img src=".\assets\img\vs-test-explorer.jpg" alt="visual studio test explorer shows all 136 tests passed" width="42%"/>

### Or alternatively _.Net Test  (Slowest, but Ideal for Azure Pipelines CI/CD)_ 
- Execute the following command. _(Warning: Specflow tests executed this way return false negatives, at times. To be investigated as tech debt.)_
```bash
dotnet test TfL.RoadStatus.sln
```

---
## Troubleshooting

You may encounter an _`ActivationException`_ when running the Specflow Tests. If so, you'll need to activate Specflow first. [Instructions here.](https://stackoverflow.com/questions/68388780/exception-specflow-plus-shared-services-activation-activationexception)


----
## Configuring the App ID and API key

Multiple options are supported. You can choose your preference, from the following:


- Console arguments _(When running in Visual Studio these are defined in `launchSettings.json`)_
- The `appSettings.json` file _(ConsoleUI.AcceptanceTests and Application.IntegrationTests have their own *.appSettings.json file)_
- User secrets _(Suggested for local development. Doesn't store securely, but does ensure you don't commit them into source control by mistake.)_
- Define as environment variables _(useful for CI/CD avoiding needing to parse args)_


The config gets built as per the following precedence _(lowest -> highest)_: 

```bash
app.AddEnvironmentVariables();
app.AddUserSecrets<Program>();
app.AddJsonFile("appsettings.json", false);
app.AddJsonStream(args
      .ToJsonStream()); //adds command line parameter (i.e: --appKey) and value args (i.e: roadIds)

```

___
## Assumptions and Goals

The assumption I've made is that TfL desires enterprise-ready code that has longevity. A console app that's ___extendable___, but built upon foundations of ___simplicity___ => to ensure ___ease in mantainability___.

I decided to ___'Keep it Simple'___ by implementing [___Clean Architecture___](https://www.youtube.com/watch?v=dK4Yb6-LxAk), meaning business logic is kept decoupled from the technology used to implement it. This adds the flexibility of new frontends and technologies being able to be swapped 'in' and 'out'.  
Find out more [here](https://www.youtube.com/watch?v=dK4Yb6-LxAk) about how Clean Architecture can be the difference between a system that lasts many years vs 'only a few years'.

<a href="https://www.ssw.com.au/rules/rules-to-better-clean-architecture">
    <img src=".\assets\img\clean-architecture.png" alt="clean architecture" width="24%"/> Source: https://jasontaylor.dev/clean-architecture-getting-started/ 
</a>


I decided to design my console application around the design principles an ASP.NET Web API Project implements. My goal being to ensure developers will feel comfortable and hopefully a sense of familiarity when maintaining this application.

---
## Design
Before I wrote any code, the approach I took was to spend some time designing what I was going to build. This is the _rough sketch_ I made:

<img src=".\assets\img\design.jpg" alt="diagram showing console application design" width="70%"/>

---

## Iteration 0

Normally I'd _never include_ unused and/ or incomplete code in a delivery. There's an NSwag OpenApi -> C# Code Generation alternative implementation that's been left in: _`TflSwaggerClient`_.

I decided to learn something new I thought would be of benefit to the project, so did a timeboxed spike into NSwag. Although I succeeded in configuring the tool, during the spike, I found the tool restrictive and challenging to work with using a TDD approach, so decided to abandon this, in favor of writing my own client.

I've left the code in however to demonstrate an ___agile way of working___.

---
##### _Refactorings, suggestions and feedback welcome! :-)_

TfL.RoadStatus.ConsoleUI © 2021 by Aidan Bunce-Waters is licensed under <a href="http://creativecommons.org/licenses/by-nc-sa/4.0/?ref=chooser-v1" target="_blank" rel="license noopener noreferrer" style="display:inline-block;">CC BY-NC-SA 4.0<img style="height:22px!important;margin-left:3px;vertical-align:text-bottom;" src="https://mirrors.creativecommons.org/presskit/icons/cc.svg?ref=chooser-v1"><img style="height:22px!important;margin-left:3px;vertical-align:text-bottom;" src="https://mirrors.creativecommons.org/presskit/icons/by.svg?ref=chooser-v1"><img style="height:22px!important;margin-left:3px;vertical-align:text-bottom;" src="https://mirrors.creativecommons.org/presskit/icons/nc.svg?ref=chooser-v1"><img style="height:22px!important;margin-left:3px;vertical-align:text-bottom;" src="https://mirrors.creativecommons.org/presskit/icons/sa.svg?ref=chooser-v1"></a></p>


---


<img src=".\assets\img\vms2.jpg" alt="logo" width="28%"/>
<img src=".\assets\img\vms.jpg" alt="logo" width="28%"/>
