using TargetDesafio.App;
using TargetDesafio.App.Estoque;

namespace TargetDesafio.Tests;

public class EstoqueTests
{
    private const int CanetaAzul = 101;

    // Cada teste lê o arquivo de novo, para começar com o estoque original e sem movimentações.
    private static ControleEstoque CriarControle()
    {
        var arquivo = ArquivoJson.Ler<EstoqueArquivo>("Data/estoque.json");
        return new ControleEstoque(arquivo.Estoque);
    }

    private static int EstoqueAtual(ControleEstoque controle, int codigo)
    {
        return controle.Produtos.Single(p => p.CodigoProduto == codigo).Quantidade;
    }

    [Fact]
    public void Movimentar_Entrada_SomaAoEstoque()
    {
        var controle = CriarControle();

        var movimentacao = controle.Movimentar(CanetaAzul, TipoMovimentacao.Entrada, 10, "Compra de fornecedor");

        Assert.Equal(160, movimentacao.EstoqueFinal);
        Assert.Equal(160, EstoqueAtual(controle, CanetaAzul));
    }

    [Fact]
    public void Movimentar_Saida_SubtraiDoEstoque()
    {
        var controle = CriarControle();

        var movimentacao = controle.Movimentar(CanetaAzul, TipoMovimentacao.Saida, 10, "Venda balcão");

        Assert.Equal(140, movimentacao.EstoqueFinal);
        Assert.Equal(140, EstoqueAtual(controle, CanetaAzul));
    }

    [Fact]
    public void Movimentar_SaidaIgualAoEstoque_ZeraOProduto()
    {
        var controle = CriarControle();

        var movimentacao = controle.Movimentar(CanetaAzul, TipoMovimentacao.Saida, 150, "Venda balcão");

        Assert.Equal(0, movimentacao.EstoqueFinal);
        Assert.Equal(0, EstoqueAtual(controle, CanetaAzul));
    }

    [Fact]
    public void Movimentar_SaidaMaiorQueEstoque_LancaErroENaoAlteraEstoque()
    {
        var controle = CriarControle();

        Assert.Throws<InvalidOperationException>(
            () => controle.Movimentar(CanetaAzul, TipoMovimentacao.Saida, 151, "Venda balcão"));

        Assert.Equal(150, EstoqueAtual(controle, CanetaAzul));
        Assert.Empty(controle.Movimentacoes);
    }

    [Fact]
    public void Movimentar_EntradaAteOLimiteDoInt_EhAceita()
    {
        var controle = CriarControle();

        // 150 + (int.MaxValue - 150) = int.MaxValue: chega no limite sem passar dele.
        var movimentacao = controle.Movimentar(CanetaAzul, TipoMovimentacao.Entrada, int.MaxValue - 150, "Compra de fornecedor");

        Assert.Equal(int.MaxValue, movimentacao.EstoqueFinal);
        Assert.Equal(int.MaxValue, EstoqueAtual(controle, CanetaAzul));
    }

    [Fact]
    public void Movimentar_EntradaQueUltrapassaLimiteDoInt_LancaErroENaoAlteraEstoque()
    {
        var controle = CriarControle();

        // 150 + int.MaxValue não cabe num int.
        Assert.Throws<InvalidOperationException>(
            () => controle.Movimentar(CanetaAzul, TipoMovimentacao.Entrada, int.MaxValue, "Compra de fornecedor"));

        Assert.Equal(150, EstoqueAtual(controle, CanetaAzul));
        Assert.Empty(controle.Movimentacoes);
    }

    [Fact]
    public void Movimentar_ProdutoInexistente_LancaErro()
    {
        var controle = CriarControle();

        Assert.Throws<ArgumentException>(
            () => controle.Movimentar(999, TipoMovimentacao.Entrada, 10, "Compra de fornecedor"));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public void Movimentar_QuantidadeMenorOuIgualAZero_LancaErro(int quantidade)
    {
        var controle = CriarControle();

        Assert.Throws<ArgumentException>(
            () => controle.Movimentar(CanetaAzul, TipoMovimentacao.Entrada, quantidade, "Compra de fornecedor"));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Movimentar_DescricaoVazia_LancaErro(string descricao)
    {
        var controle = CriarControle();

        Assert.Throws<ArgumentException>(
            () => controle.Movimentar(CanetaAzul, TipoMovimentacao.Entrada, 10, descricao));
    }

    [Fact]
    public void Movimentar_TresSeguidas_RecebemIdsSequenciais()
    {
        var controle = CriarControle();

        var primeira = controle.Movimentar(CanetaAzul, TipoMovimentacao.Entrada, 10, "Compra de fornecedor");
        var segunda = controle.Movimentar(CanetaAzul, TipoMovimentacao.Saida, 5, "Venda balcão");
        var terceira = controle.Movimentar(CanetaAzul, TipoMovimentacao.Entrada, 2, "Devolução de cliente");

        Assert.Equal(1, primeira.Id);
        Assert.Equal(2, segunda.Id);
        Assert.Equal(3, terceira.Id);
    }

    [Fact]
    public void Movimentar_ErroEntreDuasValidas_NaoPulaId()
    {
        var controle = CriarControle();

        var primeira = controle.Movimentar(CanetaAzul, TipoMovimentacao.Entrada, 10, "Compra de fornecedor");
        Assert.Throws<InvalidOperationException>(
            () => controle.Movimentar(CanetaAzul, TipoMovimentacao.Saida, 1000, "Venda balcão"));
        var segunda = controle.Movimentar(CanetaAzul, TipoMovimentacao.Saida, 5, "Venda balcão");

        Assert.Equal(1, primeira.Id);
        Assert.Equal(2, segunda.Id);
    }

    [Theory]
    [InlineData(999, 10, "Compra de fornecedor")]
    [InlineData(CanetaAzul, 0, "Compra de fornecedor")]
    [InlineData(CanetaAzul, 10, "")]
    public void Movimentar_ArgumentoInvalidoEntreDuasValidas_NaoPulaId(int codigo, int quantidade, string descricao)
    {
        var controle = CriarControle();

        var primeira = controle.Movimentar(CanetaAzul, TipoMovimentacao.Entrada, 10, "Compra de fornecedor");
        Assert.Throws<ArgumentException>(
            () => controle.Movimentar(codigo, TipoMovimentacao.Entrada, quantidade, descricao));
        var segunda = controle.Movimentar(CanetaAzul, TipoMovimentacao.Saida, 5, "Venda balcão");

        Assert.Equal(1, primeira.Id);
        Assert.Equal(2, segunda.Id);
    }
}
