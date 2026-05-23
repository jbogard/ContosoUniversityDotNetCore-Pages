using Microsoft.Data.SqlClient;

var builder = DistributedApplication.CreateBuilder(args);

var sql = builder.AddSqlServer("sql")
    .WithLifetime(ContainerLifetime.Persistent);

var db = sql.AddDatabase("db");
db.WithCommand(
        name: "migrate-contosouniversity-db",
        displayName: "Migrate ContosoUniversity Database",
        executeCommand: async cmd =>
        {
            var serverConnString = await sql.Resource.GetConnectionStringAsync();
            var sqlConnStringBuilder = new SqlConnectionStringBuilder(serverConnString)
            { 
                InitialCatalog = db.Resource.DatabaseName
            };
            var connString = sqlConnStringBuilder.ToString();

            var process = new System.Diagnostics.Process();
            process.StartInfo.WorkingDirectory = "../";
            process.StartInfo.FileName = "dotnet";
            process.StartInfo.Arguments = $"grate -c \"{connString}\" -f ContosoUniversity/App_Data --silent";
            process.StartInfo.UseShellExecute = true;

            process.Start();

            await process.WaitForExitAsync(cmd.CancellationToken);

            if (process.ExitCode != 0)
            {
                return CommandResults.Failure($"Grate exited with code {process.ExitCode}.");
            }

            return CommandResults.Success();
        },
        commandOptions: new CommandOptions { IconName = "ArrowSquareUpRight" }
    );
    ;

builder.AddProject<Projects.ContosoUniversity>("contosouniversity")
    .WithReference(db)
    .WaitFor(db);

builder.Build().Run();
