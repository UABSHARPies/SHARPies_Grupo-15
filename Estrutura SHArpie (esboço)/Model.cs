public class Model
{
    public Model() { }

    // Exemplos de funções 
    public void ProcessarComando(string utilizador, string comando) { }
    
    public void RegistarMensagem(string utilizador) { }

    public void AtualizarInteracoes(string utilizador) { }

    public int ObterTotalInteracoes(string utilizador) { return 0; }

    public List<string> ListarTopUtilizadores() { return new List<string>(); }

    public bool ValidarComando(string comando) { return true; }
}

