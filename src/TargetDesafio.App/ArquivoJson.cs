using System.Text.Json;

namespace TargetDesafio.App;

public static class ArquivoJson
{
    private static readonly JsonSerializerOptions Opcoes = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static T Ler<T>(string caminho)
    {
        string caminhoCompleto = Path.Combine(AppContext.BaseDirectory, caminho);

        if (!File.Exists(caminhoCompleto))
        {
            throw new InvalidOperationException($"Arquivo não encontrado: {caminhoCompleto}");
        }

        string conteudo = File.ReadAllText(caminhoCompleto);
        T? resultado = JsonSerializer.Deserialize<T>(conteudo, Opcoes);

        if (resultado is null)
        {
            throw new InvalidOperationException($"O arquivo {caminho} está vazio.");
        }

        return resultado;
    }
}
