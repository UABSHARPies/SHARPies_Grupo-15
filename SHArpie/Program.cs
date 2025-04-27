/// <summary>
/// Classe principal do programa. Ponto de entrada da aplicação.
/// </summary>
class Program
{
    /// <summary>
    /// Método Main. Arranca a aplicação e inicia o bot.
    /// </summary>
    /// <param name="args">Argumentos da linha de comandos (não usados neste projeto).</param>
    static async Task Main(string[] args)
    {
        // Cria uma instância do Controller (responsável pela lógica do bot)
        Controller controller = new Controller();

        // Inicia o bot de forma assíncrona
        await controller.IniciarBotAsync();
    }
}
