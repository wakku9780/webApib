using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ss
{
    public class ApiHealthCheck : IHealthCheck
    {
        private readonly HttpClient httpClient;

        public ApiHealthCheck(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var response = await httpClient.GetAsync("http://localhost:5119/api/Students");
            if(response.IsSuccessStatusCode)
            {
                return await Task.FromResult(new HealthCheckResult(
                    status: HealthStatus.Healthy,
                    description: "The API is up and running"));
            }
            return await Task.FromResult(new HealthCheckResult(
                    status: HealthStatus.Unhealthy,
                    description: "The API is down"));
        }
    }
}
