using Autofac;
using NotSoSimpleEcommerce.IdentityServer.Domain.Services.Contracts;
using NotSoSimpleEcommerce.IdentityServer.Domain.Services.Implementations;

namespace NotSoSimpleEcommerce.IdentityServer.Modules
{
    public class DomainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserService>()
                .As<IUserService>()
                .InstancePerLifetimeScope();
        }
    }
}
