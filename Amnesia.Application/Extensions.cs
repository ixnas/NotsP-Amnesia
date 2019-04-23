using Amnesia.Application.Peers;
using Amnesia.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Amnesia.Application
{
    public static class Extensions
    {
        public static void UseApplication(this IServiceCollection services)
        {
            services.AddSingleton<Amnesia>();

            services.AddSingleton<PeerManager>();

            services.AddTransient<BlockService>();
            services.AddTransient<ContentService>();
            services.AddTransient<DefinitionService>();
            services.AddTransient<StateService>();
        }
    }
}