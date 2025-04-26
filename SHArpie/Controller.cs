public class Controller
{
    private Model model;
    private DiscordView view;

    public Controller()
    {
        model = new Model();
        view = new DiscordView();

        view.ComandoRecebido += TratarComando;
    }

    public async Task IniciarBotAsync()
    {
        await view.IniciarAsync();
    }

    private async Task TratarComando(string utilizador, string comando)
    {
        model.ProcessarComando(utilizador, comando);

        if (comando.StartsWith("!"))
        {
            string resposta = "";

            if (model.ValidarComando(comando))
            {
                switch (comando.ToLower())
                {
                    case "!help":
                        resposta = "Comandos disponíveis:\n" +
                                   "!help - Lista os comandos disponíveis\n" +
                                   "!mensagens - Mostra quantas mensagens enviaste\n" +
                                   "!top - Mostra o top de utilizadores mais ativos\n" +
                                   "!reset - Apaga todos os dados de interação\n" +
                                 
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
                        model.ResetDados();
                        resposta = "Todos os dados foram apagados com sucesso!";
                        break;

                }
            }
            else
            {
                resposta = "Comando inválido. Escreve !help para veres os comandos disponíveis.";
            }

            await view.MostrarMensagemAsync(resposta);
        }
        else
        {
            model.RegistarMensagem(utilizador);
            model.AtualizarDataUltimaContagemPara(DateTime.UtcNow);
        }
    }
}
