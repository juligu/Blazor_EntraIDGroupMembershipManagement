using Azure.Identity;
using Microsoft.Graph;

namespace BlazorAppGraph
{
    public static class GraphConfiguration
    {
        public static IServiceCollection ConfigureGraphClient(this IServiceCollection services, IConfiguration configuration)
        {
            // The client credentials flow requires that you request the
            // /.default scope, and pre-configure your permissions on the
            // app registration in Azure. An administrator must grant consent
            // to those permissions beforehand.
            var scopes = new[] { "https://graph.microsoft.com/.default" };

            // Values from app registration
            var clientId = configuration.GetValue<String>("EntraID:ClientId");
            var tenantId = configuration.GetValue<String>("EntraID:TenantID");
            var clientSecret = configuration.GetValue<String>("EntraID:ClientSecret");

            // using Azure.Identity;
            var options = new ClientSecretCredentialOptions
            {
                AuthorityHost = AzureAuthorityHosts.AzurePublicCloud,
            };

            // https://learn.microsoft.com/dotnet/api/azure.identity.clientsecretcredential
            var clientSecretCredential = new ClientSecretCredential(
                tenantId, clientId, clientSecret, options);

            services.AddScoped(sp =>
            {
                return new GraphServiceClient(clientSecretCredential, scopes);
            });

            return services;
        }
    }
}
