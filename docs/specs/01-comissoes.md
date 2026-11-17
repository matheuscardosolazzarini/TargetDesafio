# Spec 01: Comissão por vendedor

## Enunciado

Ler o JSON de vendas de um time comercial e calcular a comissão de cada vendedor, aplicando a regra a cada venda.

## Regra

| Valor da venda      | Comissão |
|---------------------|----------|
| menor que 100,00    | 0%       |
| de 100,00 a 499,99  | 1%       |
| 500,00 ou mais      | 5%       |

A comissão é calculada venda a venda e somada por vendedor. Não aplicar o percentual sobre o total vendido.

A comissão de cada venda não é arredondada. O arredondamento acontece só no total por vendedor. Arredondar venda a venda dá 1 centavo a mais para todos os vendedores neste conjunto de dados.

## Escopo extra deste exercício

Como é o primeiro, também cria `ArquivoJson` e o menu principal descritos no CLAUDE.md, só com as opções 1 e 0 por enquanto.

## Tipos

- `record Venda(string Vendedor, decimal Valor)`
- `record VendasArquivo(List<Venda> Vendas)`
- `record ComissaoVendedor(string Vendedor, int QuantidadeVendas, decimal TotalVendido, decimal TotalComissao)`
- `static class CalculadoraComissao`
  - `decimal Calcular(decimal valorVenda)`: comissão de uma venda, sem arredondar.
  - `IReadOnlyList<ComissaoVendedor> CalcularPorVendedor(IEnumerable<Venda> vendas)`: agrupa por vendedor, arredonda o total, ordena por comissão decrescente.
- `static class ComissoesConsole` com `Executar()`: lê `Data/vendas.json` e mostra a tabela.

## Saída esperada

```
Vendedor          Vendas   Total vendido   Comissão
João Silva            10    R$ 10.754,70   R$ 495,68
Maria Souza            9     R$ 9.874,30   R$ 465,95
Ana Lima               9     R$ 8.763,95   R$ 404,98
Carlos Oliveira        8     R$ 7.928,35   R$ 379,37
```

## Testes (`ComissaoTests.cs`)

Comissão por venda:

| Venda    | Comissão |
|----------|----------|
| 99,99    | 0        |
| 100,00   | 1,00     |
| 499,99   | 4,9999   |
| 500,00   | 25,00    |

Com `Data/vendas.json`:

- Totais: João 495,68; Maria 465,95; Ana 404,98; Carlos 379,37.
- Ordem do resultado: João, Maria, Ana, Carlos.

## Premissas (para o README)

- Venda de exatamente R$ 500,00 entra nos 5%.
- Arredondamento só no total do vendedor.
