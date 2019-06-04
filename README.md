# All-Jokes Project

This project was presented at ISSoft Insights 2018 IT Conference and demonstrates how conventional ASP.NET Core services can be transformed into Azure Service Fabric projects without loosing the ability for self-hosted execution.

## Content

The following source code contains:
* JokesApi/ - contains ASP.NET Core Web API service
* JokesWeb/ - contains ASP.NET Core MVC application
* JokesApp/ - contains Azure Service Fabric application project
* JokesApiContracts/ - contains shared models used for communication between JokesApi & JokesWeb

## Building demo project

This project is created using Visual Studio 2017 (15.7) and requires Azure Service Fabric SDK (3.1.274) to run.

### Preparing the development machine

* Install .NET Core SDK (Windows). https://www.microsoft.com/net/download/windows
* Install Azure Service Fabric SDK (Windows). https://azure.microsoft.com/en-us/downloads

### Set up the development environment

* Clone the master branch. `git clone https://github.com/coherentsolutionsinc/issoft-insights-2018-aspnetcore-to-fabric-service <path to the local folder>`
* Start Service Fabric Local Cluster (Five Node Cluster configuration)
* Build & Run. `build-and-run.ps1`

The above listed step will:
* Get & Build the source code
* Run the application locally: `JokesWeb` & `JokesApi`, create `JokesDb` in `(localdb)\mssqllocaldb`

> **Developer's comment:**
>
> If you are planning to use database different from `(localdb)\mssqllocaldb` you should modify the connection string in `src/JokesApi/Program.cs[65]`

* Deploy the application to Service Fabric Local Cluster: `JokesWeb`, `JokesApi` (English, Русский - Народные, Русский - Советские)
* Navigate to **Service Fabric Explorer / JokesApp / JokesWebService / (partition) / (any node) / ServiceEndpoint**. 

> **Developer's comment:**
>
> The self-hosted `JokesWeb` is http://localhost:54036
>
> The base path to `JokesWebService` in Service Fabric Explorer is http://localhost:19080/Explorer/index.html#/apptype/JokesAppType/app/JokesApp/service/JokesApp%252FJokesWebService

### Application Usage

There are several `.json` files in `mock-data/` directory that contains demo jokes. Feel free to use application `Import` functionality.

> **Developer's comment:**
>
> The application doesn't implement any kind of error handling logic and therefore has a high chance to crash.

## See Also

* Service Fabric Explorer documentation. https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-visualizing-your-cluster

## Authors

This project is owned by [Coherent Solutions][1].

## License

This project is licensed under the MIT License - see the [LICENSE.md][2] for details.

[1]: https://www.coherentsolutions.com/ "Coherent Solutions Inc."
[2]: LICENCE.md "License"
