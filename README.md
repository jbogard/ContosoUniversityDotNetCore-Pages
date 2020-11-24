![CI](https://github.com/jbogard/ContosoUniversityDotNetCore-Pages/workflows/CI/badge.svg)

# ContosoUniversity on ASP.NET Core 5.0 on .NET Core and Razor Pages

Contoso University, the way I would write it.

This example requires some tools and PowerShell modules, you should run `setup.cmd` or `setup.ps1` to install them.

To prepare the database, execute the build script using [PSake](https://psake.readthedocs.io/): `psake migrate`. Open the solution and run!

## Things demonstrated

- CQRS and MediatR
- AutoMapper
- Vertical slice architecture
- Razor Pages
- Fluent Validation
- HtmlTags
- Entity Framework Core

## Migrating the Database

RoundHousE will automatically create or upgrade (migrate) the database to the latest schema version when you run it:

From PowerShell:
```
invoke-psake migrate
```

From CMD:
```
psake migrate
```

When running unit tests, you can recreate the unit test database using:

```
invoke-psake migratetest
```

## Versioning

Version numbers can be passed on the build script command line:

From PowerShell:
```
invoke-psake CI -properties ${'version':'1.2.3-dev.5'}
```

Because we're passing a PowerShell dictionary on the command line, the cmd script doesn't handle this very nicely.

Or generate a version using [GitVersion](https://gitversion.net/docs/) locally:
```
psake localversion
```
will generate a semantic version and output it.
