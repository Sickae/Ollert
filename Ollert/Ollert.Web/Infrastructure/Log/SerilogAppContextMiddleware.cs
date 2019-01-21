using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Ollert.Logic.Interfaces;
using Serilog.Context;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ollert.Web.Infrastructure.Log
{
    public class SerilogAppContextMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly SerilogAppContextMiddlewareOptions _options;

        public SerilogAppContextMiddleware(RequestDelegate next, IOptions<SerilogAppContextMiddlewareOptions> options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _options = options.Value ?? throw new ArgumentNullException(nameof(options.Value));
        }

        public async Task InvokeAsync(HttpContext context, IAppContext appContext)
        {
            IEnumerable<ILogEventEnricher> enrichers = null;
            if (_options.EnrichersForContextFactory != null)
            {
                try
                {
                    enrichers = _options.EnrichersForContextFactory(appContext);
                }
                catch
                {
                    if (_options.ReThrowEnricherFactoryExceptions) throw;
                }
            }

            bool nextExecuted = false;
            if (enrichers != null)
            {
                var array = enrichers.ToArray();
                if (array.Any())
                {
                    using (LogContext.Push(array))
                    {
                        await _next(context);
                        nextExecuted = true;
                    }
                }
            }

            if (!nextExecuted)
            {
                await _next(context);
            }
        }
    }
}
