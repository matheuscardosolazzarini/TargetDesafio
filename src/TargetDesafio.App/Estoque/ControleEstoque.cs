namespace TargetDesafio.App.Estoque;

public class ControleEstoque
{
    private readonly List<Produto> _produtos;
    private readonly List<Movimentacao> _movimentacoes = new();

    public ControleEstoque(IEnumerable<Produto> produtos)
    {
        _produtos = produtos.ToList();
    }

    public IReadOnlyCollection<Produto> Produtos => _produtos;

    public IReadOnlyList<Movimentacao> Movimentacoes => _movimentacoes;

    public Movimentacao Movimentar(int codigoProduto, TipoMovimentacao tipo, int quantidade, string descricao)
    {
        // Todas as validações vêm antes de qualquer alteração: se der erro, nada muda e o ID não é consumido.
        Produto? produto = _produtos.FirstOrDefault(p => p.CodigoProduto == codigoProduto);

        if (produto is null)
        {
            throw new ArgumentException($"Produto {codigoProduto} não encontrado.");
        }

        if (quantidade <= 0)
        {
            throw new ArgumentException("A quantidade deve ser maior que zero.");
        }

        if (string.IsNullOrWhiteSpace(descricao))
        {
            throw new ArgumentException("A descrição da movimentação é obrigatória.");
        }

        int estoqueFinal;

        if (tipo == TipoMovimentacao.Entrada)
        {
            estoqueFinal = produto.Quantidade + quantidade;
        }
        else if (tipo == TipoMovimentacao.Saida)
        {
            if (quantidade > produto.Quantidade)
            {
                throw new InvalidOperationException(
                    $"Estoque insuficiente de {produto.DescricaoProduto}. Disponível: {produto.Quantidade}, solicitado: {quantidade}.");
            }

            estoqueFinal = produto.Quantidade - quantidade;
        }
        else
        {
            throw new ArgumentException("Tipo de movimentação inválido.");
        }

        produto.Quantidade = estoqueFinal;

        // O ID é a posição na lista. Como só entra na lista quem passou nas validações, não há ID pulado.
        int id = _movimentacoes.Count + 1;
        var movimentacao = new Movimentacao(id, codigoProduto, tipo, quantidade, descricao.Trim(), DateTime.Now, estoqueFinal);
        _movimentacoes.Add(movimentacao);

        return movimentacao;
    }
}
