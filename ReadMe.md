# DotNetNinja.AutoBoundConfiguration

Library to simplify the binding of classes to configuration sections in .Net Core applications.

## Build Status

[![Build status](https://dev.azure.com/chaosmonkey/DotNetNinja.AutoBoundConfiguration/_apis/build/status/DotNetNinja.AutoBoundConfiguration-ASP.NET%20Core-CI)](https://dev.azure.com/chaosmonkey/DotNetNinja.AutoBoundConfiguration/_build/latest?definitionId=25)

## Getting Started in ASP.NET Core Projects

* Add the NuGet Package DotNetNinja.AutoBoundConfiguration to you project
  * .NET CLI - dotnet add package DotNetNinja.AutoBoundConfiguration
  * Package Manager - Install-Package DotNetNinja.AutoBoundConfiguration
* Add the following in the Startup.cs
  * services.AddAutoBoundConfigurations(Configuration).FromAssembly(typeof(Program).Assembly);
  * If you need to bind classes in more than one assembly simply chain .FromAssembly(...)
    * services.AddAutoBoundConfigurations(Configuration).FromAssembly(typeof(Program).Assembly).FromAssembly(typeof(Foo).Assembly);
* Create your settings classes (Just like you would using the IOptions<T> pattern)
  * Mark your class with an [AutoBind("SECTIONNAME")] attribute, where SECTIONNAME is the name of the configuration section to bind to.
* Add your settings classes to whatever classes need them as constructor parameters.  They are already registered with the dependency injection system!

## More Reading & Examples
Check out my blog post on this package @ [https://dotnetninja.net/2020/02/automatic-binding-of-settings-classes-to-configuration/](https://dotnetninja.net/2020/02/automatic-binding-of-settings-classes-to-configuration/)
