using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Bookview.Data
{
    public static class HostExtensions
    {
        /// <summary>
        /// Metoda rozszerzająca dla IHost, która inicjalizuje dane początkowe (seed).
        /// </summary>
        public static async Task SeedData(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                await InfoSeeder.Initialize(services);
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "Wystąpił błąd podczas seedowania bazy danych.");
            }
        }
    }
}
