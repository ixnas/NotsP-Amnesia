using Amnesia.Application.Peers;
using Amnesia.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Amnesia.Application
{
    public static class Extensions
    {
        public static void UseApplication(this IServiceCollection services, IConfiguration peerConfiguration)
        {
            services.AddTransient<Amnesia>();

            services.AddTransient<PeerManager>();

            services.AddTransient<BlockchainService>();
            services.AddTransient<StateService>();
            services.AddTransient<SeedService>();

            services.Configure<PeerConfiguration>(peerConfiguration);
            services.PostConfigure<PeerConfiguration>(config => config.Validate());
        }
    }
}