using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http.Features;
using Ollert.Logic.Interfaces;
using System;
using System.Net;
using System.Reflection;

namespace Ollert.Web.Infrastructure
{
    public class AppContext : IAppContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _hostName;
        private readonly string _appPath;
        private readonly string _correlationId;

        public AppContext(IHttpContextAccessor httpContextAccessor)
        {
            var codeBase = Assembly.GetExecutingAssembly().CodeBase;
            var uri = new UriBuilder(codeBase);

            _httpContextAccessor = httpContextAccessor;
            _hostName = Dns.GetHostName();
            _appPath = Uri.UnescapeDataString(uri.Path);
            _correlationId = $"Ollert-web-{Guid.NewGuid()}";
        }

        public static string Version => Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public string CorrelationId => string.IsNullOrEmpty(_httpContextAccessor.HttpContext?.Request?.Headers["CorrelationId"])
            ? _correlationId
            : _httpContextAccessor.HttpContext?.Request?.Headers["CorrelationId"].ToString();

        public object ClientInfo
        {
            get
            {
                var httpRequest = _httpContextAccessor.HttpContext?.Request;

                if (httpRequest == null)
                {
                    return new
                    {
                        Application = "Ollert.Web",
                        ServerHost = _hostName,
                        AppPath = _appPath,
                        AppVersion = Version
                    };
                }

                return new
                {
                    Application = "Ollert.Web",
                    ServerHost = _hostName,
                    AppPath = _appPath,
                    AppVersion = Version,
                    Request = new
                    {
                        ClientIP = httpRequest.HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress.ToString(),
                        Url = httpRequest.GetDisplayUrl(),
                        UserAgent = httpRequest.Headers["User-Agent"].ToString()
                    }
                };
            }
        }
    }
}
