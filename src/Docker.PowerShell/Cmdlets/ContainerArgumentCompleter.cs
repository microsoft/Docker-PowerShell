using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;
using System.Management.Automation.Language;
using System.Linq;
using Docker.DotNet.Models;
using System;

public class ContainerArgumentCompleter : IArgumentCompleter
{
    public IEnumerable<CompletionResult> CompleteArgument(string commandName,
                                                          string parameterName,
                                                          string wordToComplete,
                                                          CommandAst commandAst,
                                                          IDictionary fakeBoundParameters)
    {
        var client = DockerFactory.CreateClient(fakeBoundParameters);

        var task = client.Containers.ListContainersAsync(new ContainersListParameters
        {
            All = true
        });

        task.Wait();

        return task.Result.SelectMany(container =>
            {
                // If the user has already typed part of the name, then include IDs that start
                // with that portion. Otherwise, just let the user tab through the names.
                if (wordToComplete == "")
                {
                    return container.Names;
                }
                else
                {
                    return container.Names.Concat(new List<string> { container.ID });
                }
            })
            .Select(name => name.StartsWith("/") && !wordToComplete.StartsWith("/") ? name.Substring(1) : name)
            .Where(name => name.StartsWith(wordToComplete, StringComparison.CurrentCultureIgnoreCase))
            .OrderBy(name => name)
            .Select(name => new CompletionResult(name, name, CompletionResultType.Text, name));
    }
}