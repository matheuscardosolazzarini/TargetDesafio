using System.Globalization;
using TargetDesafio.App;
using TargetDesafio.App.Comissoes;

namespace TargetDesafio.Tests;

public class ComissaoTests
{
    [Theory]
    [InlineData("99.99", "0")]
    [InlineData("100.00", "1.00")]
    [InlineData("499.99", "4.9999")]
    [InlineData("500.00", "25.00")]
    public void Calcular_AplicaFaixaDeComissao(string venda, string comissaoEsperada)
    {
        // decimal não pode ser usado direto em atributo, por isso os valores vêm como texto.
        decimal valorVenda = decimal.Parse(venda, CultureInfo.InvariantCulture);
        decimal esperado = decimal.Parse(comissaoEsperada, CultureInfo.InvariantCulture);

        decimal comissao = CalculadoraComissao.Calcular(valorVenda);

        Assert.Equal(esperado, comissao);
    }

    [Theory]
    [InlineData("João Silva", "495.68")]
    [InlineData("Maria Souza", "465.95")]
    [InlineData("Ana Lima", "404.98")]
    [InlineData("Carlos Oliveira", "379.37")]
    public void CalcularPorVendedor_ComArquivoDeVendas_TotalPorVendedor(string vendedor, string totalEsperado)
    {
        var arquivo = ArquivoJson.Ler<VendasArquivo>("Data/vendas.json");
        decimal esperado = decimal.Parse(totalEsperado, CultureInfo.InvariantCulture);

        var resultado = CalculadoraComissao.CalcularPorVendedor(arquivo.Vendas);

        var item = resultado.Single(r => r.Vendedor == vendedor);
        Assert.Equal(esperado, item.TotalComissao);
    }

    [Fact]
    public void CalcularPorVendedor_ComArquivoDeVendas_OrdenaPorComissaoDecrescente()
    {
        var arquivo = ArquivoJson.Ler<VendasArquivo>("Data/vendas.json");

        var resultado = CalculadoraComissao.CalcularPorVendedor(arquivo.Vendas);

        var nomes = resultado.Select(r => r.Vendedor).ToList();
        Assert.Equal(new List<string> { "João Silva", "Maria Souza", "Ana Lima", "Carlos Oliveira" }, nomes);
    }
}
