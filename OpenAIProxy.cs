﻿using Standard.AI.OpenAI.Clients.OpenAIs;
using Standard.AI.OpenAI.Models.Configurations;
using Standard.AI.OpenAI.Models.Services.Foundations.ChatCompletions;

namespace ConsoleAppOpenAI;

public class OpenAIProxy : IOpenAIProxy
{
    readonly OpenAIClient openAIClient;

    readonly List<ChatCompletionMessage> _messages;

    public OpenAIProxy(string apiKey)
    {
        var openAIConfigurations = new OpenAIConfigurations
        {
            ApiKey = apiKey,
        };

        openAIClient = new OpenAIClient(openAIConfigurations);

        _messages = new();
    }

    public void StackMessages(params ChatCompletionMessage[] message)
    {
        _messages.AddRange(message);
    }

    static ChatCompletionMessage[] ToCompletionMessage(
      ChatCompletionChoice[] choices)
      => choices.Select(x => x.Message).ToArray();

    public Task<ChatCompletionMessage[]> SendChatMessage(string message)
    {
        var chatMsg = new ChatCompletionMessage()
        {
            Content = message,
            Role = "user"
        };
        return SendChatMessage(chatMsg);
    }

    async Task<ChatCompletionMessage[]> SendChatMessage(
      ChatCompletionMessage message = null)
    {
        if (message != null)
        {
            StackMessages(message);
        }

        var chatCompletion = new ChatCompletion
        {
            Request = new ChatCompletionRequest
            {
                Model = "gpt-3.5-turbo-1106",
                Messages = _messages.ToArray(),
                Temperature = 0.2,
                MaxTokens = 100
            }
        };

        var result = await openAIClient
          .ChatCompletions
          .SendChatCompletionAsync(chatCompletion);

        var choices = result.Response.Choices;

        var messages = ToCompletionMessage(choices);

        StackMessages(messages);

        return messages;
    }
}