// Copyright (c) Microsoft. All rights reserved.

using Microsoft.Extensions.Configuration;
namespace SKCLI;

#pragma warning disable CA1812 // instantiated by AddUserSecrets
internal sealed class Env
#pragma warning restore CA1812
{
    /// <summary>
    /// Simple helper used to load env vars and secrets like credentials,
    /// to avoid hard coding them in the sample code
    /// </summary>
    /// <param name="name">Secret name / Env var name</param>
    /// <returns>Value found in Secret Manager or Environment Variable</returns>
    internal static string? Var(string name)
    {
        var configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .AddJsonFile("config/appsettings.json", optional: true)
            .AddUserSecrets<Env>()
            .Build();

        var value = configuration[name];
        return !string.IsNullOrEmpty(value)
            ? value : throw new ArgumentException($"Invalid service type value: {name}");
    }
}