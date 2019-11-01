using System;
using Datadog.Trace;
using Datadog.Trace.Configuration;

namespace DatadogTest
{
    public class DatadogTracer
    {
        public static void Setup()
        {
            // read default configuration sources (env vars, web.config, datadog.json)
            var settings = TracerSettings.FromDefaultSources();

            // change some settings
            settings.ServiceName = "datadog-test";
            settings.AgentUri = new Uri("http://localhost:8126/");

            // disable the AdoNet integration
            settings.Integrations["AdoNet"].Enabled = false;

            // create a new Tracer using these settings
            var tracer = new Tracer(settings);

            // set the global tracer
            Tracer.Instance = tracer;            
        }
    }
}
