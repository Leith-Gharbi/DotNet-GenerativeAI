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

// Example: Send a message to the model
Console.WriteLine("Connecting to GitHub Models...\n");

var response = await chatClient.GetResponseAsync("What is generative AI? Explain 20 word.");

Console.WriteLine("Response from GitHub Models:");
Console.WriteLine(response.Text);
Console.WriteLine($"Tokens used: in={response.Usage?.InputTokenCount}, out={response.Usage?.OutputTokenCount}");
