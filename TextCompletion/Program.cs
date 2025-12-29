using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using OpenAI;
using System.ClientModel;


// Build configuration with user secrets
var configuration = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

// Retrieve token from user secrets
var token = configuration["Githubmodels:Token"]
    ?? throw new InvalidOperationException("GitHub Models token not found. Please set 'Githubmodels:Token' in user secrets.");

Console.WriteLine($"Token = {token}");
// Configure GitHub Models endpoint (OpenAI-compatible API)
var credential = new ApiKeyCredential(token);
var openAIOptions = new OpenAIClientOptions
{
    Endpoint = new Uri("https://models.github.ai/inference")
};

// Create the OpenAI client pointing to GitHub Models
var openAIClient = new OpenAIClient(credential, openAIOptions);


// Create the chat client using Microsoft.Extensions.AI abstraction
// Using gpt-5-mini as the model (available on GitHub Models)
// Try: "gpt-5-mini", "openai/gpt-5-mini", or "gpt-4o-mini" if unavailable
IChatClient chatClient = openAIClient.GetChatClient("openai/gpt-4o-mini").AsIChatClient();


#region Basic Completion
//// Example: Send a message to the model
//Console.WriteLine("Connecting to GitHub Models...\n");

//var response = await chatClient.GetResponseAsync("What is generative AI? Explain 20 word.");

//Console.WriteLine("Response from GitHub Models:");
//Console.WriteLine(response.Text);
//Console.WriteLine($"Tokens used: in={response.Usage?.InputTokenCount}, out={response.Usage?.OutputTokenCount}");

#endregion

#region Streaming 
//var prompt = "What is generative AI? Explain in 200 words.";
//Console.WriteLine($"User Prompt >>>  {prompt} \n");

//var responseStream = chatClient.GetStreamingResponseAsync(prompt);
//await foreach (var chunk in responseStream)
//{
//    if (!string.IsNullOrEmpty(chunk.Text))
//    {
//        Console.Write(chunk.Text);
//    }
//}
#endregion

#region Classification

//var classificationPrompt = """
//Please classify the following sentences into categories: 
//- 'complaint' 
//- 'suggestion' 
//- 'praise' 
//- 'other'.

//1) "I love the new layout!"
//2) "You should add a night mode."
//3) "When I try to log in, it keeps failing."
//4) "This app is decent."
//""";

//Console.WriteLine($"user >>> {classificationPrompt}");

//ChatResponse classificationResponse = await client.GetResponseAsync(classificationPrompt);

//Console.WriteLine($"assistant >>>\n{classificationResponse}");

#endregion

#region Summarization

//var summaryPrompt = """
//Summarize the following blog in 1 concise sentences:

//"Microservices architecture is increasingly popular for building complex applications, but it comes with additional overhead. It's crucial to ensure each service is as small and focused as possible, and that the team invests in robust CI/CD pipelines to manage deployments and updates. Proper monitoring is also essential to maintain reliability as the system grows."
//""";

//Console.WriteLine($"user >>> {summaryPrompt}");

//ChatResponse summaryResponse = await client.GetResponseAsync(summaryPrompt);

//Console.WriteLine($"assistant >>> \n{summaryResponse}");

#endregion

