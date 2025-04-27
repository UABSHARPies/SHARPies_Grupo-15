using System;

/// <summary>
/// Exce��o personalizada para representar um comando inv�lido enviado pelo utilizador.
/// </summary>
public class ComandoInvalidoException : Exception
{
    /// <summary>
    /// Construtor que cria uma nova exce��o de comando inv�lido, recebendo uma mensagem descritiva.
    /// </summary>
    /// <param name="mensagem">Mensagem de erro a associar � exce��o.</param>
    public ComandoInvalidoException(string mensagem) : base(mensagem)
    {
    }
}
