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
    [InlineData("João Silva", 10, "10754.70", "495.68")]
    [InlineData("Maria Souza", 9, "9874.30", "465.95")]
    [InlineData("Ana Lima", 9, "8763.95", "404.98")]
    [InlineData("Carlos Oliveira", 8, "7928.35", "379.37")]
    public void CalcularPorVendedor_ComArquivoDeVendas_TotaisPorVendedor(
        string vendedor, int quantidadeEsperada, string totalVendidoEsperado, string comissaoEsperada)
    {
        var arquivo = ArquivoJson.Ler<VendasArquivo>("Data/vendas.json");
        decimal totalVendido = decimal.Parse(totalVendidoEsperado, CultureInfo.InvariantCulture);
        decimal comissao = decimal.Parse(comissaoEsperada, CultureInfo.InvariantCulture);

        var resultado = CalculadoraComissao.CalcularPorVendedor(arquivo.Vendas);

        var item = resultado.Single(r => r.Vendedor == vendedor);
        Assert.Equal(quantidadeEsperada, item.QuantidadeVendas);
        Assert.Equal(totalVendido, item.TotalVendido);
        Assert.Equal(comissao, item.TotalComissao);
    }

    [Fact]
    public void CalcularPorVendedor_AplicaFaixaVendaAVenda()
    {
        var vendas = new List<Venda>
        {
            new Venda("Teste", 99.99m),
            new Venda("Teste", 100.00m),
            new Venda("Teste", 500.00m)
        };

        var resultado = CalculadoraComissao.CalcularPorVendedor(vendas);

        // 0 + 1,00 + 25,00 = 26,00. Aplicar 5% sobre o total (699,99) daria 35,00.
        Assert.Equal(26.00m, resultado.Single().TotalComissao);
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
