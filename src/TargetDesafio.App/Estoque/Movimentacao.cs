namespace TargetDesafio.App.Estoque;

public record Movimentacao(
    int Id,
    int CodigoProduto,
    TipoMovimentacao Tipo,
    int Quantidade,
    string Descricao,
    DateTime Data,
    int EstoqueFinal);
