class Program
{
    static async Task Main(string[] args)
    {
        Controller controller = new Controller();
        await controller.IniciarBotAsync();
    }
}
