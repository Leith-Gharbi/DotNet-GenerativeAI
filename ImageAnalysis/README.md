# ImageAnalysis Console Application

A .NET console application that analyzes images using AI vision models. Supports both GitHub Models and Ollama as inference providers, configurable via feature flags.

## Features

- Analyze images using AI vision models
- Switch between GitHub Models and Ollama using feature flags
- Configurable endpoints and models
- Supports PNG and other common image formats

## Prerequisites

- .NET 10.0 SDK
- For GitHub Models: A valid GitHub Models API token
- For Ollama: Ollama installed locally with a vision model (e.g., `llava`)

## Configuration

### appsettings.json

```json
{
  "FeatureFlags": {
    "UseOllama": false
  },
  "GitHubModels": {
    "Endpoint": "https://models.github.ai/inference",
    "Model": "openai/gpt-4o-mini"
  },
  "Ollama": {
    "Endpoint": "http://127.0.0.1:11434",
    "Model": "llava"
  }
}
```

| Setting | Description |
|---------|-------------|
| `FeatureFlags:UseOllama` | Set to `true` to use Ollama, `false` for GitHub Models |
| `GitHubModels:Endpoint` | GitHub Models API endpoint |
| `GitHubModels:Model` | Model to use with GitHub Models |
| `Ollama:Endpoint` | Local Ollama server endpoint |
| `Ollama:Model` | Ollama model name (must support vision) |

### User Secrets (for GitHub Models token)

Initialize user secrets and set your GitHub Models token:

```bash
dotnet user-secrets init
dotnet user-secrets set "GitHubModels:Token" "your-github-token-here"
```

## Usage

### Using GitHub Models

1. Set `FeatureFlags:UseOllama` to `false` in `appsettings.json`
2. Ensure your GitHub Models token is configured in user secrets
3. Run the application:

```bash
dotnet run
```

### Using Ollama

1. Install Ollama from https://ollama.ai
2. Pull a vision-capable model:

```bash
ollama pull llava
```

3. Set `FeatureFlags:UseOllama` to `true` in `appsettings.json`
4. Run the application:

```bash
dotnet run
```

## Project Structure

```
ImageAnalysis/
├── Program.cs           # Main application entry point
├── ImageAnalysis.csproj # Project file with dependencies
├── appsettings.json     # Configuration file
├── README.md            # This file
└── Images/
    └── cars.png         # Sample image for analysis
```

## Dependencies

- `Microsoft.Extensions.AI` - AI abstraction layer
- `Microsoft.Extensions.AI.OpenAI` - OpenAI/GitHub Models integration
- `Microsoft.Extensions.Configuration` - Configuration support
- `Microsoft.Extensions.Configuration.Binder` - Configuration binding
- `Microsoft.Extensions.Configuration.Json` - JSON configuration provider
- `Microsoft.Extensions.Configuration.UserSecrets` - User secrets support
- `OllamaSharp` - Ollama client library

## Example Output

```
Using GitHub Models with model: openai/gpt-4o-mini
Prompt: How many red cars are in the picture? and what other car colors are there?
Image: cars.png
Response: There are 3 red cars in the picture. Other car colors visible include...
```
