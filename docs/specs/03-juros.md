# Spec 03: Juros por atraso

## Enunciado

A partir de um valor e de uma data de vencimento, calcular o valor dos juros na data de hoje, considerando multa de 2,5% ao dia.

## Regra

Juros simples de 2,5% ao dia sobre o valor original, contados a partir do dia seguinte ao vencimento. Não há juros compostos.

- Dias de atraso = data de referência menos vencimento, em dias corridos, nunca negativo.
- Sem ajuste de dia útil: vencimento em sábado, domingo ou feriado não é levado para o próximo dia útil.
- Juros = valor × 0,025 × dias de atraso, arredondado em 2 casas.
- Vencimento hoje ou no futuro: 0 dias, juros zero.
- Valor fora da faixa de 0,01 a 1.000.000.000,00: `ArgumentException`.

## Tipos

- `record ResultadoJuros(int DiasAtraso, decimal Juros, decimal ValorAtualizado)`
- `static class CalculadoraJuros`
  - `ResultadoJuros Calcular(decimal valor, DateOnly vencimento, DateOnly dataReferencia)`
- `static class JurosConsole` com `Executar()`.

O console passa `DateOnly.FromDateTime(DateTime.Today)` como data de referência. Os testes passam datas fixas.

## Fluxo no console

Pede valor e vencimento, repete a pergunta se a entrada for inválida e mostra dias de atraso, juros e valor atualizado.

Valor:

- Só vírgula como separador decimal (`1234,56`), no máximo 2 casas, sem ponto de milhar nem `R$`. Assim `100.50` é recusado em vez de virar 10.050,00. Mensagem: "Valor inválido. Use vírgula e no máximo 2 casas decimais, sem ponto de milhar nem R$ (ex.: 1234,56)."
- Fora da faixa de 0,01 a 1.000.000.000,00: "O valor deve estar entre R$ 0,01 e R$ 1.000.000.000,00."

Vencimento no formato `d/M/yyyy`, que aceita `07/11/2026` e `7/11/2026`. O prompt é "Vencimento (dd/mm/aaaa): ". Mensagem: "Data inválida. Use dia/mês/ano (ex.: 07/11/2026)."

## Testes (`JurosTests.cs`)

| Valor  | Vencimento  | Referência  | Dias | Juros  |
|--------|-------------|-------------|------|--------|
| 100,00 | 20/11/2026  | 17/11/2026  | 0    | 0,00   |
| 100,00 | 17/11/2026  | 17/11/2026  | 0    | 0,00   |
| 100,00 | 07/11/2026  | 17/11/2026  | 10   | 25,00  |
| 333,33 | 16/11/2026  | 17/11/2026  | 1    | 8,33   |
| 100,00 | 28/02/2028  | 01/03/2028  | 2    | 5,00   |

Valor 0,20 com 1 dia de atraso dá juros 0,005, que arredonda para 0,01 (`AwayFromZero`).

O caso de 2028 confere o ano bissexto: de 28/02 a 01/03 passa pelo 29/02.

Valor 0 e valor acima de 1.000.000.000,00 dão erro.

## Premissas

- Juros simples de 2,5% ao dia, em dias corridos, sem ajuste de dia útil.
- O enunciado usa "juros" e "multa" para a mesma taxa diária. Foi tratado como juros.
- Valor aceito de R$ 0,01 a R$ 1.000.000.000,00, com no máximo 2 casas decimais.
- Vencimento aceita dia e mês com um ou dois dígitos (`7/11/2026`).
