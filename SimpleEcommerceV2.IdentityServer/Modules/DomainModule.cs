using Autofac;
using SimpleEcommerceV2.IdentityServer.Domain.Services.Contracts;
using SimpleEcommerceV2.IdentityServer.Domain.Services.Implementations;

namespace SimpleEcommerceV2.IdentityServer.Modules
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
