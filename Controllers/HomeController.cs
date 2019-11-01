using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DatadogTest.Models;
using Datadog.Trace;
using System.Collections;

namespace DatadogTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var prefixes = new[] { "COR_", "CORECLR_", "DD_", "DATADOG_" };

            var envVars = from envVar in Environment.GetEnvironmentVariables().Cast<DictionaryEntry>()
                          from prefix in prefixes
                          let key = (envVar.Key as string)?.ToUpperInvariant()
                          let value = envVar.Value as string
                          where key.StartsWith(prefix)
                          orderby key
                          select new KeyValuePair<string, string>(key, value);

            using (var scope = Tracer.Instance.StartActive("web.request"))
            {
                var span = scope.Span;
                span.Type = SpanTypes.Web;
                span.ResourceName = HttpContext.Request.Path;
                span.SetTag(Tags.HttpMethod, HttpContext.Request.Method);

                // do some work...
                await Task.Delay(200);

                Console.WriteLine($@"{span.ServiceName},
                        {span.OperationName},
                        {span.ResourceName},
                        {span.TraceId},
                        {span.SpanId}");
            }

            return View(envVars.ToList());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
