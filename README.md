# TargetDesafio

Solução do desafio técnico da Target Sistemas para a vaga de Desenvolvedor de Sistemas Jr.

Aplicação de console em C# / .NET 8 com os três exercícios do enunciado, escolhidos por menu, e testes em xUnit.

## Como rodar

Requisito: .NET 8 SDK.

```
dotnet run --project src/TargetDesafio.App
dotnet test
```

## Estrutura

```
src/TargetDesafio.App/
├── Comissoes/        exercício 1
├── Estoque/          exercício 2
├── Juros/            exercício 3
├── Data/             vendas.json e estoque.json do enunciado
├── ArquivoJson.cs    leitura dos JSON
├── EntradaConsole.cs leitura do teclado
└── Program.cs        menu
tests/TargetDesafio.Tests/
docs/specs/           specs de cada exercício
```

As classes de cálculo (`CalculadoraComissao`, `ControleEstoque`, `CalculadoraJuros`) não leem teclado nem arquivo. Isso fica nas classes `*Console` de cada pasta. Assim as regras são testadas sem depender da tela.

## Exercícios

### 1. Comissão por vendedor

Lê `Data/vendas.json`, calcula a comissão de cada venda e soma por vendedor.

| Valor da venda      | Comissão |
|---------------------|----------|
| menor que 100,00    | 0%       |
| de 100,00 a 499,99  | 1%       |
| 500,00 ou mais      | 5%       |

```
Vendedor          Vendas   Total vendido    Comissão
João Silva            10    R$ 10.754,70   R$ 495,68
Maria Souza            9     R$ 9.874,30   R$ 465,95
Ana Lima               9     R$ 8.763,95   R$ 404,98
Carlos Oliveira        8     R$ 7.928,35   R$ 379,37
```

### 2. Movimentação de estoque

Carrega os produtos de `Data/estoque.json` e registra entradas e saídas. Cada movimentação recebe um ID sequencial, um tipo (entrada ou saída) e uma descrição com o motivo. Depois de cada uma, o programa mostra o estoque final do produto.

```
Código  Descrição                    Estoque
101     Caneta Azul                      150
102     Caderno Universitário             75
103     Borracha Branca                  200
104     Lápis Preto HB                   320
105     Marcador de Texto Amarelo         90

Código do produto: 101
Tipo (E = entrada, S = saída): S
Quantidade: 10
Descrição (ex.: Compra de fornecedor, Venda balcão, Devolução de cliente, Ajuste de inventário): Venda balcão
Movimentação #1 | Saída | 10 un. | Caneta Azul | Venda balcão | Estoque final: 140
```

Validações: produto inexistente, quantidade menor ou igual a zero, descrição vazia e saída maior que o estoque. Quando uma movimentação é recusada, o estoque não muda e o ID não é consumido.

### 3. Juros por atraso

Recebe um valor e uma data de vencimento e calcula os juros até a data de hoje, a 2,5% ao dia.

```
Valor (ex.: 1234,56): 1500,00
Vencimento (dd/MM/yyyy): 07/11/2026

Valor original:   R$ 1.500,00
Vencimento:       07/11/2026
Data de hoje:     17/11/2026
Dias de atraso:   10
Juros:            R$ 375,00
Valor atualizado: R$ 1.875,00
```

## Premissas

O enunciado deixa alguns pontos em aberto. As decisões tomadas foram estas:

- Venda de exatamente R$ 500,00 entra na faixa de 5%.
- A comissão de cada venda não é arredondada. O arredondamento (2 casas, `AwayFromZero`) é feito só no total do vendedor. Arredondar venda a venda daria 1 centavo a mais para cada vendedor.
- Saída maior que o estoque é recusada. O estoque nunca fica negativo.
- As movimentações ficam em memória. O ID é único durante a execução e recomeça em 1 quando o programa reinicia.
- O tipo da movimentação indica o sentido (entrada ou saída). A descrição indica o motivo (compra, venda, devolução, ajuste).
- O enunciado chama a taxa de "juros" e de "multa". Foi tratada como juros simples de 2,5% ao dia sobre o valor original.
- Os dias de atraso são corridos, sem ajuste para dia útil. Vencimento hoje ou no futuro não gera juros.
- O valor é digitado com vírgula decimal (`1234,56`). Ponto é recusado, para `100.50` não ser lido como 10.050,00.

## Testes

29 testes em xUnit:

- Comissão: os limites de cada faixa (99,99, 100,00, 499,99 e 500,00), o total de cada vendedor com o JSON do enunciado e a ordem do resultado.
- Estoque: entrada, saída, saída igual ao estoque, saída maior que o estoque, produto inexistente, quantidade e descrição inválidas, IDs em sequência e ID que não pula após erro.
- Juros: vencimento futuro, no dia, com atraso, arredondamento e valor inválido.
- Leitura de JSON: arquivo inexistente e JSON mal formado.

## Processo

Antes do código, cada exercício ganhou uma spec em `docs/specs`, com regras, casos de teste e premissas. A implementação foi feita com o Claude Code como assistente, seguindo as instruções do `CLAUDE.md` e uma spec por vez, com revisão de cada exercício antes do commit.
