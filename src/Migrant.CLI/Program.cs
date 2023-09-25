using System.CommandLine;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.AI.ChatCompletion;
using Microsoft.SemanticKernel.Orchestration;
using SKCLI;

var command = CommandFactory.BuildRootCommand();
await command.InvokeAsync(args).ConfigureAwait(false);
