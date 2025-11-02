LambdaFun.Time
==============
An implementation of high precision time in F# for the .NET platform (C#, F#, VB.net ,etc).

For examples, see [LambdaFun.Time README](./LambdaFun.Time/README.md)

### New in [1.2]
Modern Frameworks, F# 9, Improved Debugger Support
- For Paket users, run `> dotnet paket add LambdaFun.Time --project [MyProject]`
- For NuGet users, run `PM> Install-Package FSharp.Data.JsonPath`

Prior version [1.1.0](https://www.nuget.org/packages/LambdaFun.Time/1.1.0) is still functionally equivalent, if you are running older versions of the above.

### More Details
Swapped to cross-platform Paket tooling in dotnet, replacing paket.exe and paket bootstrappers.
- use `dotnet tool restore` and `dotnet paket restore` to load dependencies.

Targets newer Frameworks, including net481, netstandard2.0, netcoreapp3.1, net80, and net90.
- use `dotnet build` to compile all versions.

Updates min-requirement for F# to 9.0, which is also supported in each of these frameworks.
Enhances NuGet package to support debug symbols and source references automatically.
