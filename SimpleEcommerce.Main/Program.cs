using Autofac;
using Autofac.Extensions.DependencyInjection;
using HealthChecks.UI.Client;
using MediatR.Extensions.Autofac.DependencyInjection;
using MediatR.Extensions.Autofac.DependencyInjection.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using SimpleEcommerce.Main.Middlewares;
using SimpleEcommerce.Main.Repositories.Contexts;
using SimpleEcommerce.Main.Repositories.Contracts;
using SimpleEcommerce.Main.Repositories.Implementations;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(applicationBuilder =>
{
    applicationBuilder.Register(configuration =>
    {
        var options = new DbContextOptionsBuilder<ProductContext>();
        options.UseNpgsql(builder.Configuration.GetConnectionString("Default"));
        var dbContext = new ProductContext(options.Options);

        dbContext.Database.Migrate();
        return dbContext;
    })
    .InstancePerLifetimeScope();

    applicationBuilder.RegisterType<ErrorHandlerMiddleware>().SingleInstance();
    applicationBuilder.RegisterType<ProductReadRepository>().As<IProductReadRepository>();
    applicationBuilder.RegisterType<ProductWriteRepository>().As<IProductWriteRepository>();
    applicationBuilder.RegisterType<StockWriteRepository>().As<IStockWriteRepository>();
    applicationBuilder.RegisterType<StockReadRepository>().As<IStockReadRepository>();

    var configuration = MediatRConfigurationBuilder
          .Create(typeof(Program).Assembly)
          .WithAllOpenGenericHandlerTypesRegistered()
          .Build();

    applicationBuilder.RegisterMediatR(configuration);
});
builder.Services.AddHealthChecks();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.Map("/main", applicationBuilder =>
{
    applicationBuilder.UseSwagger();
    applicationBuilder.UseSwaggerUI();
    applicationBuilder.UseRouting();
    applicationBuilder.UseMiddleware<ErrorHandlerMiddleware>();

    applicationBuilder.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
        endpoints.MapHealthChecks("/health", new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
    });
});

app.Run();
