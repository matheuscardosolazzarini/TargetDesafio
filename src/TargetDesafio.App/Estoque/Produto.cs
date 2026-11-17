using System.Text.Json.Serialization;

namespace TargetDesafio.App.Estoque;

public class Produto
{
    public int CodigoProduto { get; set; }

    public string DescricaoProduto { get; set; } = string.Empty;

    [JsonPropertyName("estoque")]
    public int Quantidade { get; set; }
}
