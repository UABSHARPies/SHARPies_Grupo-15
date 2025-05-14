using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

/// <summary>
/// Representa a camada Model do projeto, responsável por gerir e persistir os dados das interações dos utilizadores.
/// </summary>
public class Model
{
    private Dictionary<string, int> interacoes; // Guarda o número de mensagens enviadas por cada utilizador
    private static readonly string FicheiroDados = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "interacoes.json");// Nome do ficheiro onde os dados serão guardados

    public DateTime DataUltimaContagem { get; private set; } // Data da última contagem de mensagens registada

    /// <summary>
    /// Construtor da classe Model. Carrega os dados do ficheiro, caso existam.
    /// </summary>
    public Model()
    {
        if (File.Exists(FicheiroDados))
        {
            try
            {
                string json = File.ReadAllText(FicheiroDados);
                var dados = JsonSerializer.Deserialize<DadosGuardados>(json);

                interacoes = dados?.Interacoes ?? new Dictionary<string, int>();
                DataUltimaContagem = dados?.DataUltimaContagem ?? DateTime.MinValue;

                Console.WriteLine("[INFO] Dados carregados com sucesso.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERRO] Falha ao carregar dados: {ex.Message}");
                interacoes = new Dictionary<string, int>();
                DataUltimaContagem = DateTime.MinValue;
            }
        }
        else
        {
            Console.WriteLine("[WARNING] Nenhum ficheiro de dados encontrado. A começar novo.");
            interacoes = new Dictionary<string, int>();
            DataUltimaContagem = DateTime.MinValue;
        }
    }

    /// <summary>
    /// Processa um comando enviado pelo utilizador, validando-o.
    /// </summary>
    /// <param name="utilizador">Nome do utilizador</param>
    /// <param name="comando">Comando recebido</param>
    public void ProcessarComando(string utilizador, string comando)
    {
        if (!ValidarComando(comando))
            throw new ComandoInvalidoException("Comando não reconhecido.");

        if (comando == "!reset")
        {
            ResetDados();
        }
        // Comandos como !mensagens e !top são apenas consultas e não alteram o modelo
    }

    /// <summary>
    /// Regista uma nova mensagem enviada por um utilizador.
    /// </summary>
    public void RegistarMensagem(string utilizador)
    {
        if (interacoes.ContainsKey(utilizador))
            interacoes[utilizador]++;
        else
            interacoes[utilizador] = 1;

        GuardarDados();
    }

    // Recebe uma interação genérica (via interface) e regista-a
    public void RegistarInteracao(IInteracao interacao)
    {
        RegistarMensagem(interacao.Utilizador);
    }


    /// <summary>
    /// Atualiza as interações de um utilizador (alias para RegistarMensagem).
    /// </summary>
    public void AtualizarInteracoes(string utilizador)
    {
        RegistarMensagem(utilizador);
    }

    /// <summary>
    /// Obtém o número total de mensagens enviadas por um utilizador.
    /// </summary>
    public int ObterTotalInteracoes(string utilizador)
    {
        if (interacoes.ContainsKey(utilizador))
            return interacoes[utilizador];
        else
            return 0;
    }

    /// <summary>
    /// Lista os 3 utilizadores com mais mensagens enviadas.
    /// </summary>
    public List<string> ListarTopUtilizadores()
    {
        var top = interacoes
            .OrderByDescending(p => p.Value)
            .Take(3)
            .Select(p => $"{p.Key}: {p.Value} mensagens")
            .ToList();

        return top;
    }

    /// <summary>
    /// Valida se um comando recebido é reconhecido.
    /// </summary>
    public bool ValidarComando(string comando)
    {
        return comando == "!help" || comando == "!mensagens" || comando == "!top" || comando == "!reset";
    }

    /// <summary>
    /// Atualiza a DataUltimaContagem para o momento atual e guarda os dados.
    /// </summary>
    public void AtualizarDataUltimaContagem()
    {
        DataUltimaContagem = DateTime.UtcNow;
        GuardarDados();
    }

    /// <summary>
    /// Atualiza a DataUltimaContagem para uma data específica e guarda os dados.
    /// </summary>
    public void AtualizarDataUltimaContagemPara(DateTime novaData)
    {
        DataUltimaContagem = novaData;
        GuardarDados();
    }

    /// <summary>
    /// Apaga todas as interações e atualiza a data de contagem para agora.
    /// </summary>
    public void ResetDados()
    {
        interacoes.Clear();
        DataUltimaContagem = DateTime.UtcNow;
        GuardarDados();
    }

    /// <summary>
    /// Guarda os dados de interações e da data de última contagem no ficheiro JSON.
    /// </summary>
    private void GuardarDados()
    {
        var dados = new DadosGuardados
        {
            Interacoes = interacoes,
            DataUltimaContagem = DataUltimaContagem
        };

        string json = JsonSerializer.Serialize(dados, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FicheiroDados, json);
    }

    /// <summary>
    /// Classe interna usada apenas para guardar/ler dados no ficheiro JSON.
    /// </summary>
    private class DadosGuardados
    {
        public Dictionary<string, int> Interacoes { get; set; }
        public DateTime DataUltimaContagem { get; set; }
    }

}
   