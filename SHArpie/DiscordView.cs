using System.Linq;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;



// Interface que representa uma interação genérica (utilizador + data)
public interface IInteracao
{
    string Utilizador { get; }
    DateTime Timestamp { get; }
}

// Implementação concreta para interações do Discord
public class InteracaoDiscord : IInteracao
{
    public string Utilizador { get; }
    public DateTime Timestamp { get; }

    public InteracaoDiscord(string utilizador, DateTime timestamp)
    {
        Utilizador = utilizador;
        Timestamp = timestamp;
    }
}


/// <summary>
/// Camada de View responsável pela comunicação com o Discord.
/// </summary>
public class DiscordView
{
    private DiscordClient client;            // Cliente do Discord
    private DiscordChannel canalPadrao;       // Canal de texto onde o bot vai atuar

    // Definição do delegado (evento) para quando um comando é recebido
    public delegate Task ComandoRecebidoHandler(string utilizador, string comando);
    public event ComandoRecebidoHandler ComandoRecebido;

    /// <summary>
    /// Inicia o cliente do Discord e lê o histórico de mensagens desde a última contagem.
    /// </summary>
    /// <param name="dataUltimaContagem">Data da última contagem registada.</param>
    /// <param name="model">Instância do Model para registar interações do histórico.</param>
    public async Task IniciarAsync(DateTime dataUltimaContagem, Model model)
    {
        // Configuração do cliente Discord
        client = new DiscordClient(new DiscordConfiguration()
        {
            Token = "", // ⚠️ Substituir pelo token correto
            TokenType = TokenType.Bot,
            Intents = DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents
        });

        // Subscrição do evento de nova mensagem criada
        client.MessageCreated += OnMessageCreatedAsync;

        // Conectar o cliente ao Discord
        await client.ConnectAsync();

        // Obter o servidor (Guild) e os canais
        var guild = await client.GetGuildAsync(1345133246199107635);
        var canais = await guild.GetChannelsAsync();

        // Procurar o canal chamado "comandosbot"
        canalPadrao = canais.FirstOrDefault(c => c.Type == ChannelType.Text && c.Name == "comandosbot");

        // Se o canal existir, vamos processar as mensagens antigas
        if (canalPadrao != null)
        {
            // Buscar até 500 mensagens recentes
            var mensagens = await canalPadrao.GetMessagesAsync(500);

            foreach (var mensagem in mensagens)
            {
                // Só contabilizar mensagens:
                // - Que não sejam do próprio bot
                // - Que não sejam comandos (não comecem por "!")
                // - Que sejam posteriores à última contagem conhecida
                if (!mensagem.Author.IsBot &&
                    !mensagem.Content.StartsWith("!") &&
                    mensagem.Timestamp.UtcDateTime > dataUltimaContagem)
                {
                    var interacao = new InteracaoDiscord(mensagem.Author.Username, mensagem.Timestamp.UtcDateTime);
                    model.RegistarInteracao(interacao);
                    // Registar interação
                }
            }

            // Atualizar a data de última contagem após tratar o histórico
            model.AtualizarDataUltimaContagem();
        }

        // Manter o bot ativo indefinidamente
        await Task.Delay(-1);
    }

    /// <summary>
    /// Evento chamado sempre que uma nova mensagem é criada no Discord.
    /// </summary>
    private async Task OnMessageCreatedAsync(DiscordClient sender, MessageCreateEventArgs e)
    {
        // Ignorar mensagens de outros bots
        if (e.Author.IsBot) return;

        // Capturar o conteúdo da mensagem e o nome do utilizador
        string comando = e.Message.Content;
        string utilizador = e.Author.Username;

        // Disparar o evento de ComandoRecebido para o Controller
        if (ComandoRecebido != null)
            await ComandoRecebido(utilizador, comando);
    }

    /// <summary>
    /// Envia uma mensagem para o canal padrão do bot no Discord.
    /// </summary>
    public async Task MostrarMensagemAsync(string mensagem)
    {
        if (canalPadrao != null)
        {
            await canalPadrao.SendMessageAsync(mensagem);
        }
    }
}
