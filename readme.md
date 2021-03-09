Relation mining in unstructured text: a solid approach with expert.ai
=====================================================================

This C# project is the implementation of the techniques described in the post **Relation mining in unstructured text** (find the full post here: <http://>).

To build and execute the project, you need to do the following:

- download and install .Net Core 5.0 SDK: <https://dotnet.microsoft.com/download/dotnet/5.0>
- suggested IDE: Visual Studio Code: <https://code.visualstudio.com/Download>
- suggested extension: C# for Visual Studio Code (powered by OmniSharp): <https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp>
- create a trial developer account for expert.ai NL API:
- download and install Neo4j Desktop: <https://neo4j.com/download/>
- open Neo4j and start the predefined database or create and start a new one
- adjust the content of the `appsettings.json` file:

```json
{
  "reader": {
    "InputDirectory": [the directory containing the documents you want to analyze],
    "DocSearchPattern": [the name pattern of the documents you want to analyze, e.g. *.txt]
  },
  "caller": {
    "TokenUrl": "https://developer.expert.ai/oauth2/token",
    "ApiUrl": "https://nlapi.expert.ai/v2/analyze/standard/en",
    "Username": [your expert.ai NL API developer account user name],
    "Password": [your expert.ai NL API developer account password]
  },
  "writer": {
    "ApiUrl": "http://localhost:7474/db/data/transaction/commit",
    "Username": [your local Neo4j database user name],
    "Password": [your local Neo4j database user password]
  }
}
```
- using the command line in the project root folder, execute the following commands:

```
dotnet build
dotnet run
```

Introduction
------------

> "What is the case — a fact — is the existence of states of affairs. A state of affairs is a combination of objects (things). It is essential to things that they should be possible constituents of states of affairs."
>
> L. Wittgenstein, *Tractatus Logico-Philosophicus*, 2-2.011

Relations are everywhere: in the world, in our  minds, in our languages. Relations between entities are perhaps more important than the entities themselves, since it's the existence of relations between entities that gives entities their meaning.

When we read some piece of text, when we decode language to turn it into meaning, we weave in our minds a web of relations connecting the things the text is talking about.

If you want to use technology to create knowledge from unstructured text, you need a clear and reliable way to extract both references to entities and to relations between entities. This is one of the many goals of *Natural Language Processing* (*NLP*), the branch of computer science that has progressed so much in the last years, but that often remains far from being usable in real life scenarios.

There are many tools that try to perform the task of extracting relations from unstructured text: in this post I want to show you how to use the expert.ai NL API to do that in a simple, solid way.
