using System;

/// <summary>
/// Exceção personalizada para representar um comando inválido enviado pelo utilizador.
/// </summary>
public class ComandoInvalidoException : Exception
{
    /// <summary>
    /// Construtor que cria uma nova exceção de comando inválido, recebendo uma mensagem descritiva.
    /// </summary>
    /// <param name="mensagem">Mensagem de erro a associar à exceção.</param>
    public ComandoInvalidoException(string mensagem) : base(mensagem)
    {
    }
}
