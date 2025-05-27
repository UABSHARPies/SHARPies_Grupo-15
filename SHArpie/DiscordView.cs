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
        client = new DiscordClient(new DiscordConfiguration()
        {
            Token = "", // Insira o seu token do bot aqui
            TokenType = TokenType.Bot,
            Intents = DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents
        });

        client.MessageCreated += OnMessageCreatedAsync;
        await client.ConnectAsync();

        var guild = await client.GetGuildAsync(1345133246199107635);
        var canais = await guild.GetChannelsAsync();

        bool houveMensagens = false;

        foreach (var canal in canais)
        {
            // Verificar canais de texto normais ou canais de voz com chat embutido
            if (canal.Type == ChannelType.Text || canal.Type == ChannelType.Voice)
            {
                try
                {
                    var mensagens = await canal.GetMessagesAsync(500);

                    foreach (var mensagem in mensagens)
                    {
                        if (!mensagem.Author.IsBot &&
                            !mensagem.Content.StartsWith("!") &&
                            mensagem.Timestamp.UtcDateTime > dataUltimaContagem)
                        {
                            var interacao = new InteracaoDiscord(mensagem.Author.Username, mensagem.Timestamp.UtcDateTime);
                            model.RegistarInteracao(interacao);
                            houveMensagens = true;
                        }
                    }

                    // Salvar canal padrão se for comandosbot
                    if (canal.Type == ChannelType.Text && canal.Name == "comandosbot" && canalPadrao == null)
                    {
                        canalPadrao = canal;
                    }

                    // ⚡ Threads dentro de canais de texto
                    if (canal.Type == ChannelType.Text && canal.Threads != null)
                    {
                        foreach (var thread in canal.Threads)
                        {
                            var mensagensThread = await thread.GetMessagesAsync(500);

                            foreach (var mensagem in mensagensThread)
                            {
                                if (!mensagem.Author.IsBot &&
                                    !mensagem.Content.StartsWith("!") &&
                                    mensagem.Timestamp.UtcDateTime > dataUltimaContagem)
                                {
                                    var interacao = new InteracaoDiscord(mensagem.Author.Username, mensagem.Timestamp.UtcDateTime);
                                    model.RegistarInteracao(interacao);
                                    houveMensagens = true;
                                }
                            }
                        }
                    }


                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERRO] Falha ao processar canal {canal.Name}: {ex.Message}");
                }
            }
        }

        if (houveMensagens)
        {
            model.AtualizarDataUltimaContagem();
        }

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
