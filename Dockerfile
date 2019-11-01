FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS runtime
WORKDIR /app

ENV CORECLR_ENABLE_PROFILING 1
ENV CORECLR_PROFILER {846F5F1C-F9AE-4B07-969E-05C26BC060D8}
ENV CORECLR_PROFILER_PATH /opt/datadog/Datadog.Trace.ClrProfiler.Native.so
ENV DD_INTEGRATIONS /opt/datadog/integrations.json
ENV DD_DOTNET_TRACER_HOME /opt/datadog
ENV DD_TRACE_ANALYTICS_ENABLED true
# ENV DD_SERVICE_NAME datadog-test
ENV DD_ADONET_ENABLED false

RUN curl -LO https://github.com/DataDog/dd-trace-dotnet/releases/download/v1.8.0/datadog-dotnet-apm_1.8.0_amd64.deb && \
    dpkg -i ./datadog-dotnet-apm_1.8.0_amd64.deb

COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "datadog-test.dll"]