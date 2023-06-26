using Autofac;
using Autofac.Extensions.DependencyInjection;
using HealthChecks.UI.Client;
using MediatR.Extensions.Autofac.DependencyInjection;
using MediatR.Extensions.Autofac.DependencyInjection.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Refit;
using SimpleEcommerce.Order.HttpHandlers.Contracts;
using SimpleEcommerce.Order.Middlewares;
using SimpleEcommerce.Order.Repositories.Contexts;
using SimpleEcommerce.Order.Repositories.Contracts;
using SimpleEcommerce.Order.Repositories.Implementations;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(applicationBuilder =>
{
    applicationBuilder.Register(configuration =>
        {
            var options = new DbContextOptionsBuilder<OrderContext>();
            options.UseNpgsql(builder.Configuration.GetConnectionString("Default"));
            var dbContext = new OrderContext(options.Options);

            dbContext.Database.Migrate();
            return dbContext;
        })
        .InstancePerLifetimeScope();

    applicationBuilder.RegisterType<ErrorHandlerMiddleware>().SingleInstance();
    applicationBuilder.RegisterType<OrderWriteRepository>().As<IOrderWriteRepository>();
    
    var configuration = MediatRConfigurationBuilder
        .Create(typeof(Program).Assembly)
        .WithAllOpenGenericHandlerTypesRegistered()
        .Build();

    applicationBuilder.Register(_ => RestService.For<ISimpleHttpClient>(
        builder.Configuration.GetValue<string>("BaseAddress:Product"))
    );
    applicationBuilder.RegisterMediatR(configuration);
});
builder.Services.AddHealthChecks();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

app.Map("/order", applicationBuilder =>
{
    applicationBuilder.UseSwagger();
    applicationBuilder.UseSwaggerUI();
    applicationBuilder.UseRouting();
    applicationBuilder.UseMiddleware<ErrorHandlerMiddleware>();

    applicationBuilder.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
        endpoints.MapHealthChecks("/health",
            new HealthCheckOptions
            {
                Predicate = _ => true, ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
    });
});

app.Run();
