using System.Linq;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

public class DiscordView
{
    private DiscordClient client;
    private DiscordChannel canalPadrao;

    public delegate Task ComandoRecebidoHandler(string utilizador, string comando);
    public event ComandoRecebidoHandler ComandoRecebido;

    public async Task IniciarAsync()
    {
        client = new DiscordClient(new DiscordConfiguration()
        {
            Token = "", //Por o token, não pus por causa do commit
            TokenType = TokenType.Bot,
            Intents = DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents
        });

        client.MessageCreated += OnMessageCreatedAsync;

        await client.ConnectAsync();

        var guild = await client.GetGuildAsync(por id do servidor discord); // PREENCHER! 
        var canais = await guild.GetChannelsAsync();
        canalPadrao = canais.FirstOrDefault(c => c.Type == ChannelType.Text);

        if (canalPadrao != null)
        {
            // Buscar até 500 mensagens do histórico
            var mensagens = await canalPadrao.GetMessagesAsync(500);
            var model = new Model();

            foreach (var mensagem in mensagens)
            {
                if (!mensagem.Author.IsBot && !mensagem.Content.StartsWith("!") && mensagem.Timestamp.UtcDateTime > model.DataUltimaContagem)
                {
                    if (ComandoRecebido != null)
                        await ComandoRecebido(mensagem.Author.Username, mensagem.Content);
                }
            }

            model.AtualizarDataUltimaContagem();
        }

        await Task.Delay(-1); // Manter o bot ligado
    }

    private async Task OnMessageCreatedAsync(DiscordClient sender, MessageCreateEventArgs e)
    {
        if (e.Author.IsBot) return;

        string comando = e.Message.Content;
        string utilizador = e.Author.Username;

        if (ComandoRecebido != null)
            await ComandoRecebido(utilizador, comando);
    }

    public async Task MostrarMensagemAsync(string mensagem)
    {
        if (canalPadrao != null)
        {
            await canalPadrao.SendMessageAsync(mensagem);
        }
    }
}