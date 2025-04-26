public class Controller
{
    private Model model;
    private DiscordView view;

    public Controller()
    {
        model = new Model();
        view = new DiscordView();

        // Ligar os eventos da View aos métodos do Controller
        view.ComandoRecebido += TratarComando;
    }

    public async Task IniciarBotAsync()
    {
        await view.IniciarAsync();
    }

    private async Task TratarComando(string utilizador, string comando)
    {
        // Aqui o Controller trata o comando de forma genérica
        // e pede ao Model para processar os dados
        model.ProcessarComando(utilizador, comando);
        // Depois pede à View para mostrar uma resposta
        await view.MostrarMensagemAsync("(resposta gerada pelo Model)");
    }
}
