// Copyright (c) Microsoft. All rights reserved.

using Microsoft.SemanticKernel;
namespace SKCLI;

internal static class KernelBuilderExtensions
{
    /// <summary>
    /// Adds a text completion service to the list. It can be either an OpenAI or Azure OpenAI backend service.
    /// </summary>
    /// <param name="kernelBuilder"></param>
    /// <exception cref="ArgumentException"></exception>
    internal static IKernelBuilder WithCompletionService(this IKernelBuilder kernelBuilder)
    {
        Console.WriteLine($"Using {Env.Var("Global:LlmService")!} service");
        switch (Env.Var("Global:LlmService")!)
        {
            case "AzureOpenAI":
                Console.WriteLine($"Using {Env.Var("AzureOpenAI:DeploymentType")!} deployment");
                if (Env.Var("AzureOpenAI:DeploymentType") == "text-completion")
                {
                    kernelBuilder.Services.AddAzureOpenAITextGeneration(
                        deploymentName: Env.Var("AzureOpenAI:TextCompletionDeploymentName")!,
                        modelId: Env.Var("AzureOpenAI:TextCompletionModelId")!,
                        endpoint: Env.Var("AzureOpenAI:Endpoint")!,
                        apiKey: Env.Var("AzureOpenAI:ApiKey")!
                    );
                }
                else if (Env.Var("AzureOpenAI:DeploymentType") == "chat-completion")
                {
                    kernelBuilder.Services.AddAzureOpenAIChatCompletion(
                        deploymentName: Env.Var("AzureOpenAI:ChatCompletionDeploymentName")!,
                        modelId: Env.Var("AzureOpenAI:ChatCompletionModelId")!,
                        endpoint: Env.Var("AzureOpenAI:Endpoint")!,
                        apiKey: Env.Var("AzureOpenAI:ApiKey")!
                    );
                }
                break;

            case "OpenAI":
                Console.WriteLine($"Using {Env.Var("OpenAI:Model:Type")!} model");
                if (Env.Var("OpenAI:Model:Type") == "text-completion")
                {
                    kernelBuilder.Services.AddOpenAITextGeneration(
                        modelId: Env.Var("OpenAI:Model:Id")!,
                        apiKey: Env.Var("OpenAI:ApiKey")!,
                        orgId: Env.Var("OpenAI:OrgId")
                    );
                }
                else if (Env.Var("OpenAI:Model:Type") == "chat-completion")
                {
                    kernelBuilder.Services.AddOpenAIChatCompletion(
                        modelId: Env.Var("OpenAI:Model:Id")!,
                        apiKey: Env.Var("OpenAI:ApiKey")!,
                        orgId: Env.Var("OpenAI:OrgId")
                    );
                }
                break;

            default:
                throw new ArgumentException($"Invalid service type value: {Env.Var("OpenAI:Model:Type")}");
        }

        return kernelBuilder;
    }
}