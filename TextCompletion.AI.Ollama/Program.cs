

using Microsoft.Extensions.AI;
using OllamaSharp;

IChatClient client = 
    new OllamaApiClient(new Uri("http://127.0.0.1:1234") , "Qwen2.5-7B-Instruct-1M-Q4_K_M.gguf");


