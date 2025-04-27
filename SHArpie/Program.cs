/// <summary>
/// Classe principal do programa. Ponto de entrada da aplica��o.
/// </summary>
class Program
{
    /// <summary>
    /// M�todo Main. Arranca a aplica��o e inicia o bot.
    /// </summary>
    /// <param name="args">Argumentos da linha de comandos (n�o usados neste projeto).</param>
    static async Task Main(string[] args)
    {
        // Cria uma inst�ncia do Controller (respons�vel pela l�gica do bot)
        Controller controller = new Controller();

        // Inicia o bot de forma ass�ncrona
        await controller.IniciarBotAsync();
    }
}
