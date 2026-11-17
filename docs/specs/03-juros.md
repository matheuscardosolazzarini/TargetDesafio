# Spec 03: Juros por atraso

## Enunciado

A partir de um valor e de uma data de vencimento, calcular o valor dos juros na data de hoje, considerando multa de 2,5% ao dia.

## Regra

Juros simples de 2,5% ao dia sobre o valor original, contados a partir do dia seguinte ao vencimento. Não há juros compostos.

- Dias de atraso = data de referência menos vencimento, em dias corridos, nunca negativo.
- Sem ajuste de dia útil: vencimento em sábado, domingo ou feriado não é levado para o próximo dia útil.
- Juros = valor × 0,025 × dias de atraso, arredondado em 2 casas.
- Vencimento hoje ou no futuro: 0 dias, juros zero.
- Valor menor ou igual a zero: erro.

## Tipos

- `record ResultadoJuros(int DiasAtraso, decimal Juros, decimal ValorAtualizado)`
- `static class CalculadoraJuros`
  - `ResultadoJuros Calcular(decimal valor, DateOnly vencimento, DateOnly dataReferencia)`
- `static class JurosConsole` com `Executar()`.

O console passa `DateOnly.FromDateTime(DateTime.Today)` como data de referência. Os testes passam datas fixas.

## Fluxo no console

Pede valor e vencimento (`dd/MM/yyyy`), repete a pergunta se a entrada for inválida e mostra dias de atraso, juros e valor atualizado.

## Testes (`JurosTests.cs`)

| Valor  | Vencimento  | Referência  | Dias | Juros  |
|--------|-------------|-------------|------|--------|
| 100,00 | 20/11/2026  | 17/11/2026  | 0    | 0,00   |
| 100,00 | 17/11/2026  | 17/11/2026  | 0    | 0,00   |
| 100,00 | 07/11/2026  | 17/11/2026  | 10   | 25,00  |
| 333,33 | 16/11/2026  | 17/11/2026  | 1    | 8,33   |

Valor 0 dá erro.

## Premissas (para o README)

- Juros simples de 2,5% ao dia, em dias corridos, sem ajuste de dia útil.
- O enunciado usa "juros" e "multa" para a mesma taxa diária. Foi tratado como juros.
