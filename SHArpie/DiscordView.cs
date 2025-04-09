using DSharpPlus;
using DSharpPlus.EventArgs;

public class DiscordView
{
    private DiscordClient client;

    // Delegado e evento que a View emite para o Controller
    public delegate Task ComandoRecebidoHandler(string utilizador, string comando);
    public event ComandoRecebidoHandler ComandoRecebido;

    public async Task IniciarAsync()
    {
        client = new DiscordClient(new DiscordConfiguration()
        {
            Token = "<TOKEN_DO_BOT>", // ⚠️ Substituir pelo token real do bot
            TokenType = TokenType.Bot
        });

        client.MessageCreated += OnMessageCreatedAsync;

        await client.ConnectAsync();
        await Task.Delay(-1); // Manter o bot ligado
    }

    private async Task OnMessageCreatedAsync(DiscordClient sender, MessageCreateEventArgs e)
    {
        if (e.Author.IsBot) return;

        string comando = e.Message.Content;
        string utilizador = e.Author.Username;

        // A View não sabe quem vai tratar o evento
        // Apenas o emite para que o Controller o trate
        if (ComandoRecebido != null)
            await ComandoRecebido(utilizador, comando);
    }

    public async Task MostrarMensagemAsync(string mensagem)
    {
        // ⚠️ Este método deve ser adaptado para enviar a mensagem no canal correto
        var canais = await client.GetGuildAsync(1234567890); // ⚠️ Substituir com o ID do servidor real
        var canal = canais.GetDefaultChannel();
        await canal.SendMessageAsync(mensagem);
    }
}
