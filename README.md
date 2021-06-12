# MoneyLion-Tech-Assessment-ChaiMinghao
Solution to the technical assessment given by MoneyLion for the position of Senior Software Engineer (Backend).

## Solution details
This solution runs on the .NET 5.0 framework. Heres the link to the SDK which is required to run the solution - https://dotnet.microsoft.com/download 

**The technical assessment solution can be found within the 'Tech Assessment Feature Switches/Controllers' directory where the file is named 'FeatureAccessController.cs'.**

The Web API project is named 'Tech Assessment Feature Switches' where the technical assessment solution method names are 'GetFeatureAccess' (GET) and 'UpdateUserFeatureAccess' (POST). 

The Unit Test project is named 'Tech Assessment Feature Switches.Tests' and uses the xUnit testing tool (https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-dotnet-test).

The Web API and Unit Test project makes use of Entity Framework Core (https://docs.microsoft.com/en-us/ef/core/) as the InMemory database provider.

Upon running the solution (either locally or on the web), navigate to the '/swagger' (e.g. localhost:5001/swagger) to view and test the endpoints made available.
