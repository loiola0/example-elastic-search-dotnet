using Nest;
using POCELK.Entities;
using Serilog;

namespace POCELK
{
    public static class ElasticSearchExtension
    {
        public static void AddElasticSearch(this IServiceCollection services, IConfiguration configuration)
        {
            var baseUrl = configuration.GetValue<string>("ElasticSettings:BaseUrl");

            var user = configuration.GetValue<string>("ElasticSettings:ElasticUser");

            var password = configuration.GetValue<string>("ElasticSettings:ElasticPassword");

            var certificate = configuration.GetValue<string>("ElasticSettings:CertificateFingerprint");

            var settings = new ConnectionSettings(new Uri(baseUrl ?? ""))
                                                  .PrettyJson()
                                                  .CertificateFingerprint(certificate)
                                                  .BasicAuthentication(user, password);

            settings.EnableApiVersioningHeader();

            var indexGastos = configuration.GetValue<string>("ElasticSettings:IndexGastos") ?? "";

            var client = new ElasticClient(settings);

            services.AddSingleton<IElasticClient>(client);

            CreateIndex<Expense>(client, indexGastos);
        }

        private static void CreateIndex<T>(IElasticClient client, string indexName) where T : class
        {
            if (!client.Indices.Exists(indexName).Exists)
                client.Indices.Create(indexName, index => index.Map<T>(x => x.AutoMap()));
            else
                Log.Information($"Index '{indexName}' has existing.");
        }
    }
}
