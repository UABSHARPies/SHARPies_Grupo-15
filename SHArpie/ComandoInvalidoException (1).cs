using System;

public class ComandoInvalidoException : Exception
{
    public ComandoInvalidoException(string mensagem) : base(mensagem) { }
}