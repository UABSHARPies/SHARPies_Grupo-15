using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class Model
{
    private Dictionary<string, int> interacoes;
    private const string FicheiroDados = "interacoes.json";

    public DateTime DataUltimaContagem { get; private set; }

    public Model()
    {
        if (File.Exists(FicheiroDados))
        {
            string json = File.ReadAllText(FicheiroDados);
            var dados = JsonSerializer.Deserialize<DadosGuardados>(json);
            interacoes = dados?.Interacoes ?? new Dictionary<string, int>();
            DataUltimaContagem = dados?.DataUltimaContagem ?? DateTime.MinValue;
        }
        else
        {
            interacoes = new Dictionary<string, int>();
            DataUltimaContagem = DateTime.MinValue;
        }
    }

    public void ProcessarComando(string utilizador, string comando)
    {
        // Não faz nada ainda
    }

    public void RegistarMensagem(string utilizador)
    {
        if (interacoes.ContainsKey(utilizador))
            interacoes[utilizador]++;
        else
            interacoes[utilizador] = 1;

        GuardarDados();
    }

    public void AtualizarInteracoes(string utilizador)
    {
        RegistarMensagem(utilizador);
    }

    public int ObterTotalInteracoes(string utilizador)
    {
        if (interacoes.ContainsKey(utilizador))
            return interacoes[utilizador];
        else
            return 0;
    }

    public List<string> ListarTopUtilizadores()
    {
        var top = new List<string>();
        foreach (var par in interacoes)
        {
            top.Add($"{par.Key}: {par.Value} mensagens");
        }

        top.Sort((a, b) =>
        {
            int mensagensA = int.Parse(a.Split(':')[1].Trim().Split(' ')[0]);
            int mensagensB = int.Parse(b.Split(':')[1].Trim().Split(' ')[0]);
            return mensagensB.CompareTo(mensagensA);
        });

        return top;
    }

    public bool ValidarComando(string comando)
    {
        return comando == "!help" || comando == "!mensagens" || comando == "!top" || comando == "!reset" || comando == "!sobre";
    }

    public void AtualizarDataUltimaContagem()
    {
        DataUltimaContagem = DateTime.UtcNow;
        GuardarDados();
    }

    public void AtualizarDataUltimaContagemPara(DateTime novaData)
    {
        DataUltimaContagem = novaData;
        GuardarDados();
    }

    public void ResetDados()
    {
        interacoes.Clear();
        DataUltimaContagem = DateTime.UtcNow;
        GuardarDados();
    }

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

    private class DadosGuardados
    {
        public Dictionary<string, int> Interacoes { get; set; }
        public DateTime DataUltimaContagem { get; set; }
    }
}
