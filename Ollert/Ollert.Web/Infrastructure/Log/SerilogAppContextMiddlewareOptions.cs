using Ollert.Logic.Interfaces;
using Serilog.Core;
using System;
using System.Collections.Generic;

namespace Ollert.Web.Infrastructure.Log
{
    public class SerilogAppContextMiddlewareOptions
    {
        public SerilogAppContextMiddlewareOptions Value => this;

        public Func<IAppContext, IEnumerable<ILogEventEnricher>> EnrichersForContextFactory { get; set; }

        public bool ReThrowEnricherFactoryExceptions { get; set; } = true;
    }
}
