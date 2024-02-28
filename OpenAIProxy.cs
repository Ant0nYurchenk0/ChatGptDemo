using Standard.AI.OpenAI.Clients.OpenAIs;
using Standard.AI.OpenAI.Models.Configurations;
using Standard.AI.OpenAI.Models.Services.Foundations.ChatCompletions;

namespace ConsoleAppOpenAI;

public class OpenAIProxy : IOpenAIProxy
{
    readonly OpenAIClient openAIClient;

    //all messages in the conversation
    readonly List<ChatCompletionMessage> _messages;

    public OpenAIProxy(string apiKey)
    {
        //initialize the configuration with api key and sub
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

    //Public method to Send messages to OpenAI
    public Task<ChatCompletionMessage[]> SendChatMessage(string message)
    {
        var chatMsg = new ChatCompletionMessage()
        {
            Content = message,
            Role = "user"
        };
        return SendChatMessage(chatMsg);
    }

    //Where business happens
    async Task<ChatCompletionMessage[]> SendChatMessage(
      ChatCompletionMessage message = null)
    {
        //we should send all the messages
        //so we can give Open AI context of conversation
        if(message != null) 
        { 
            StackMessages(message);
        }

        var chatCompletion = new ChatCompletion
        {
            Request = new ChatCompletionRequest
            {
                Model = "gpt-4",
                Messages = _messages.ToArray(),
                Temperature = 0.2,                
                MaxTokens = 1
            }
        };

        var result = await openAIClient
          .ChatCompletions
          .SendChatCompletionAsync(chatCompletion);

        var choices = result.Response.Choices;

        var messages = ToCompletionMessage(choices);

        //stack the response as well - everything is context to Open AI
        StackMessages(messages);

        return messages;
    }
}