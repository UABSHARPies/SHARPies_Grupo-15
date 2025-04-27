/// <summary>
/// Classe Controller que gere a interação entre a View e o Model, segundo o padrão MVC.
/// </summary>
public class Controller
{
    private Model model;          // Instância do Model (dados e lógica de negócio)
    private DiscordView view;     // Instância da View (interação com o Discord)

    /// <summary>
    /// Construtor do Controller. Inicializa Model e View e subscreve eventos.
    /// </summary>
    public Controller()
    {
        model = new Model();
        view = new DiscordView();

        // Subscrição do evento: quando a View detetar um comando, chama TratarComando
        view.ComandoRecebido += TratarComando;
    }

    /// <summary>
    /// Inicia o bot, passando a DataUltimaContagem e o Model para a View.
    /// </summary>
    public async Task IniciarBotAsync()
    {
        await view.IniciarAsync(model.DataUltimaContagem, model);
    }

    /// <summary>
    /// Trata a lógica quando um comando ou mensagem é recebido da View.
    /// </summary>
    /// <param name="utilizador">Utilizador que enviou a mensagem</param>
    /// <param name="comando">Conteúdo da mensagem recebida</param>
    private async Task TratarComando(string utilizador, string comando)
    {
        try
        {
            // Verifica se a mensagem começa por "!" (ou seja, se é um comando)
            if (comando.StartsWith("!"))
            {
                string resposta = "";

                // Verifica se o comando é válido
                if (model.ValidarComando(comando))
                {
                    // Processa o comando (ações como reset)
                    model.ProcessarComando(utilizador, comando);

                    // Define a resposta do bot conforme o comando
                    switch (comando.ToLower())
                    {
                        case "!help":
                            resposta = "Comandos disponíveis:\n" +
                                       "!help - Lista dos comandos disponíveis\n" +
                                       "!mensagens - Mostra quantas mensagens enviaste\n" +
                                       "!top - Mostra o top de utilizadores mais ativos\n" +
                                       "!reset - Apaga todos os dados de interação\n";
                            break;

                        case "!mensagens":
                            int total = model.ObterTotalInteracoes(utilizador);
                            resposta = $"{utilizador}, já enviaste {total} mensagens!";
                            break;

                        case "!top":
                            List<string> topUtilizadores = model.ListarTopUtilizadores();
                            resposta = "Top utilizadores:\n" + string.Join("\n", topUtilizadores);
                            break;

                        case "!reset":
                            resposta = "Todos os dados foram apagados com sucesso!";
                            break;
                    }
                }
                else
                {
                    // Comando não reconhecido
                    resposta = "Comando inválido. Escreve !help para veres os comandos disponíveis.";
                }

                // Enviar resposta para o canal do Discord
                await view.MostrarMensagemAsync(resposta);
            }
            else
            {
                // Se for uma mensagem normal (não comando):
                model.RegistarMensagem(utilizador); // Regista a interação
                model.AtualizarDataUltimaContagemPara(DateTime.UtcNow); // Atualiza a última contagem
            }
        }
        catch (ComandoInvalidoException ex)
        {
            // Tratamento específico para comandos inválidos
            await view.MostrarMensagemAsync($"❌ {ex.Message}");
        }
        catch (Exception)
        {
            // Tratamento para qualquer outro erro inesperado
            await view.MostrarMensagemAsync("❌ Ocorreu um erro inesperado.");
        }
    }
}
