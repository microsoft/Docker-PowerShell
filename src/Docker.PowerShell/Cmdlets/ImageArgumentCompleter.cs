using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;
using System.Management.Automation.Language;
using System.Linq;
using Docker.DotNet.Models;
using System;

public class ImageArgumentCompleter : IArgumentCompleter
{
    private const string LatestSuffix = ":latest";

    public IEnumerable<CompletionResult> CompleteArgument(string commandName,
                                                          string parameterName,
                                                          string wordToComplete,
                                                          CommandAst commandAst,
                                                          IDictionary fakeBoundParameters)
    {
        var client = DockerFactory.CreateClient(fakeBoundParameters);

        var task = client.Images.ListImagesAsync(new ImagesListParameters(){ All = true });
        task.Wait();

        return task.Result.SelectMany(image =>
            {
                var repoTags = image.RepoTags.Where(repoTag => repoTag != "<none>:<none>");

                // If the user has already typed part of the name and this isn't for push, then include IDs that start
                // with that portion. Otherwise, just let the user tab through the names.
                if (wordToComplete == "" || commandName == "Submit-ContainerImage")
                {
                    return repoTags;
                }
                else
                {
                    // Most of the time users will want to write IDs without the sha256: prefix.
                    // Autocomplete based on this unless they have typed sha256:.
                    var id = image.ID;
                    var idParts = id.Split(':');
                    if (idParts.Length >= 2 && !wordToComplete.StartsWith(idParts[0] + ":"))
                    {
                        id = id.Substring(idParts[0].Length + 1);
                    }
                    return repoTags.Concat(new List<string> { id });
                }
            })
            .Where(name => name.StartsWith(wordToComplete, StringComparison.CurrentCultureIgnoreCase))
            .Select(name =>
            {
                // Hide ":latest" unless the user has started typing it.
                if (name.EndsWith(LatestSuffix) && wordToComplete.Length <= name.Length - LatestSuffix.Length)
                {
                    name = name.Remove(name.Length - LatestSuffix.Length);
                }

                return name;
            })
            .OrderBy(name => name)
            .Select(repoTag => new CompletionResult(repoTag, repoTag, CompletionResultType.Text, repoTag));
    }
}