using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using OllamaSharp;
using OpenAI;
using System.ClientModel;

// Load configuration from appsettings.json and user secrets
IConfigurationRoot config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddUserSecrets<Program>()
    .Build();

// Read feature flag
bool useOllama = config.GetValue<bool>("FeatureFlags:UseOllama");

// Create the appropriate chat client based on feature flag
IChatClient client;

if (useOllama)
{
    // Use Ollama local model
    var ollamaEndpoint = config["Ollama:Endpoint"] ?? "http://127.0.0.1:11434";
    var ollamaModel = config["Ollama:Model"] ?? "llava";

    Console.WriteLine($"Using Ollama with model: {ollamaModel}");
    client = new OllamaApiClient(new Uri(ollamaEndpoint), ollamaModel);
}
else
{
    // Use GitHub Models
    var credential = new ApiKeyCredential(
        config["GitHubModels:Token"] ?? throw new InvalidOperationException("Missing configuration: GitHubModels:Token."));

    var gitHubEndpoint = config["GitHubModels:Endpoint"] ?? "https://models.github.ai/inference";
    var gitHubModel = config["GitHubModels:Model"] ?? "openai/gpt-4o-mini";

    var options = new OpenAIClientOptions()
    {
        Endpoint = new Uri(gitHubEndpoint)
    };

    Console.WriteLine($"Using GitHub Models with model: {gitHubModel}");
    client = new OpenAIClient(credential, options).GetChatClient(gitHubModel).AsIChatClient();
}

// User prompts
var promptDescribe = "Describe the image";
var promptAnalyze = "How many red cars are in the picture? and what other car colors are there?";

// Prompts
string systemPrompt = "You are a useful assistant that describes images using a direct style.";
var userPrompt = promptAnalyze;

List<ChatMessage> messages =
[
    new ChatMessage(ChatRole.System, systemPrompt),
    new ChatMessage(ChatRole.User, userPrompt),
];

// Read the image bytes, create a new image content part and add it to the messages
var imageFileName = "cars.png";
string image = Path.Combine(Directory.GetCurrentDirectory(), "images", imageFileName);

AIContent aic = new DataContent(File.ReadAllBytes(image), "image/png");
var message = new ChatMessage(ChatRole.User, [aic]);
messages.Add(message);

// Send the messages to the assistant
var response = await client.GetResponseAsync(messages);
Console.WriteLine($"Prompt: {userPrompt}");
Console.WriteLine($"Image: {imageFileName}");
Console.WriteLine($"Response: {response.Messages[0]}");
