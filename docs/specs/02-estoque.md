# Spec 02: Movimentação de estoque

## Enunciado

Lançar movimentações de entrada ou saída dos produtos do JSON. Cada movimentação tem um número identificador único e uma descrição que identifica o tipo da movimentação. Ao final de cada movimentação, mostrar a quantidade final em estoque do produto movimentado.

## Regra

- Produtos carregados de `Data/estoque.json`. Só esses produtos existem, não há cadastro de produto novo.
- Cada movimentação tem ID sequencial começando em 1, tipo, quantidade, descrição e data/hora.
- O tipo diz o sentido (entrada ou saída). A descrição diz o motivo: "Compra de fornecedor", "Venda balcão", "Devolução de cliente", "Ajuste de inventário".
- Movimentações ficam em memória. O ID é único dentro de uma execução e recomeça em 1 quando o programa reinicia.
- Cada movimentação devolve o estoque final do produto.

## Validações

- Produto inexistente: erro.
- Quantidade menor ou igual a zero: erro.
- Descrição vazia ou só espaços: erro.
- Saída maior que o estoque atual: erro, e o estoque não muda.
- Quando dá erro, o ID não é consumido.

## Tipos

- `enum TipoMovimentacao { Entrada, Saida }`
- `class Produto` com `CodigoProduto`, `DescricaoProduto` e `Quantidade` (mapeado de `"estoque"` no JSON via `[JsonPropertyName]`).
- `record EstoqueArquivo(List<Produto> Estoque)`
- `record Movimentacao(int Id, int CodigoProduto, TipoMovimentacao Tipo, int Quantidade, string Descricao, DateTime Data, int EstoqueFinal)`
- `class ControleEstoque`
  - construtor recebe `IEnumerable<Produto>`
  - `IReadOnlyCollection<Produto> Produtos`
  - `IReadOnlyList<Movimentacao> Movimentacoes`
  - `Movimentacao Movimentar(int codigoProduto, TipoMovimentacao tipo, int quantidade, string descricao)`
- `static class EstoqueConsole` com `Executar()`.

## Fluxo no console

1. Lista os produtos com código, descrição e quantidade.
2. Pede código do produto, tipo (E/S), quantidade e descrição. O pedido da descrição mostra exemplos de motivo.
3. Mostra a movimentação registrada:
   `Movimentação #1 | Saída | 10 un. | Caneta Azul | Venda balcão | Estoque final: 140`
4. Pergunta se quer fazer outra. Ao sair, lista todas as movimentações da sessão.

Entrada inválida (código não numérico, tipo diferente de E/S) repete a pergunta.

Se `estoque.json` vier sem a chave `"estoque"`, a lista chega nula. Tratar como no `ComissoesConsole`: mostrar o erro e voltar ao menu.

## Testes (`EstoqueTests.cs`)

- Entrada de 10 na Caneta Azul (150) devolve estoque final 160.
- Saída de 10 na Caneta Azul devolve 140.
- Saída de 151 na Caneta Azul dá erro e o estoque continua 150.
- Produto 999 dá erro.
- Quantidade 0 dá erro.
- Descrição vazia dá erro.
- Três movimentações seguidas recebem IDs 1, 2 e 3.
- Uma movimentação com erro entre duas válidas não pula ID.

## Premissas (para o README)

- Saída maior que o estoque é bloqueada.
- Movimentações só em memória, com ID único por execução.
- A descrição é o motivo da movimentação (compra, venda, devolução, ajuste), separada do sentido entrada/saída.
