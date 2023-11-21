using POCELK;
using POCELK.Repositories;
using POCELK.Services;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddElasticSearch(hostContext.Configuration);

        services.AddSingleton<IExpensedRepository, ExpenseRepository>();

        services.AddSingleton<IExpensedService, ExpensedService>();

        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();