using System;
using Amnesia.Domain.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Amnesia.Domain
{
    public static class Extensions
    {
        public static void UseDomain(this IServiceCollection services,
            Action<DbContextOptionsBuilder> dbOptions)
        {
            services.AddDbContext<BlockchainContext>(dbOptions);
        }
    }
}