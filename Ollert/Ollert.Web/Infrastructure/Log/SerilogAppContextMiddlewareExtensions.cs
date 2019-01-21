using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using System;

namespace Ollert.Web.Infrastructure.Log
{
    public static class SerilogAppContextMiddlewareExtensions
    {
        public static IApplicationBuilder UserSerilogLogContext(this IApplicationBuilder builder, Action<SerilogAppContextMiddlewareOptions> settings)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            if (settings == null) throw new ArgumentNullException(nameof(settings));

            var options = new SerilogAppContextMiddlewareOptions();

            settings(options);

            return builder.UseMiddleware<SerilogAppContextMiddleware>(Options.Create(options));
        }
    }
}
