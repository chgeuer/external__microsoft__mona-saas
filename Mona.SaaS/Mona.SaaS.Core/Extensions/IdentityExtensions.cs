// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Mona.SaaS.Core.Models.Configuration
{
    using Microsoft.Azure.Management.ResourceManager.Fluent;
    using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;

    public static class IdentityExtensions
    {
        /// <summary>
        /// Create an AAD security token.
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static AzureCredentials CreateInfrastructureToken(this AppIdentityConfiguration config, MSIResourceType resourceType = MSIResourceType.AppService)
        {
            // Without a specified app id for a managed identity,
            // use the marketplace API service principal also for infrastructure access.
            return
                string.IsNullOrEmpty(config.ManagedIdentityAppId)
                    ? SdkContext.AzureCredentialsFactory.FromServicePrincipal(
                        clientId: config.AadClientId,
                        clientSecret: config.AadClientSecret,
                        tenantId: config.AadTenantId,
                        environment: AzureEnvironment.AzureGlobalCloud)
                    : SdkContext.AzureCredentialsFactory.FromMSI(
                        msiLoginInformation: new MSILoginInformation(
                            resourceType: resourceType,
                            clientId: config.ManagedIdentityAppId),
                        environment: AzureEnvironment.AzureGlobalCloud,
                        tenantId: config.AadTenantId);
        }
    }
}