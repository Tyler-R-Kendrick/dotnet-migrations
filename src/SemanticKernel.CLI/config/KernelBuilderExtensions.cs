﻿using Microsoft.SemanticKernel;
using SKCLI;

internal static class KernelBuilderExtensions
{
    /// <summary>
    /// Adds a text completion service to the list. It can be either an OpenAI or Azure OpenAI backend service.
    /// </summary>
    /// <param name="kernelBuilder"></param>
    /// <param name="kernelSettings"></param>
    /// <exception cref="ArgumentException"></exception>
    internal static KernelBuilder WithCompletionService(this KernelBuilder kernelBuilder, KernelSettings kernelSettings)
    {
        switch (kernelSettings.ServiceType.ToUpperInvariant())
        {
            case ServiceTypes.AzureOpenAI when kernelSettings.EndpointType == "text-completion":
                kernelBuilder.WithAzureTextCompletionService(deploymentName: kernelSettings.DeploymentOrModelId, endpoint: kernelSettings.Endpoint, apiKey: kernelSettings.ApiKey, serviceId: kernelSettings.ServiceId);
                break;
            case ServiceTypes.OpenAI when kernelSettings.EndpointType == "text-completion":
                kernelBuilder.WithOpenAITextCompletionService(modelId: kernelSettings.DeploymentOrModelId, apiKey: kernelSettings.ApiKey, orgId: kernelSettings.OrgId, serviceId: kernelSettings.ServiceId);
                break;
            default:
                throw new ArgumentException($"Invalid service type value: {kernelSettings.ServiceType}");
        }

        return kernelBuilder;
    }
}
