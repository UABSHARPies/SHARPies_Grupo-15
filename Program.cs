using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.EventArgs;

class Program
{
    static async Task Main(string[] args)
    {
        var discord = new DiscordClient(new DiscordConfiguration()
        {
            Token = "",
            TokenType = TokenType.Bot,
            Intents = DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents
        });

        discord.Ready += async (client, e) =>
        {
            Console.WriteLine("Bot está online!");
        };

        discord.MessageCreated += async (client, e) =>
        {
            if (e.Message.Content.ToLower().Contains("boa tarde") && !e.Author.IsBot)
            {
                await e.Message.RespondAsync("Boa tarde!:wave:  ");
            }
            else if (e.Message.Content.ToLower().Contains("boa noite") && !e.Author.IsBot)
            {
                await e.Message.RespondAsync("Boa noite!:wave: ");
            }
            else if (e.Message.Content.ToLower().Contains("bom dia") && !e.Author.IsBot)
            {
                await e.Message.RespondAsync("Bom dia! :wave: ");
            }         
        };

        await discord.ConnectAsync();
        await Task.Delay(-1);
    }
}

Isto mete nojo