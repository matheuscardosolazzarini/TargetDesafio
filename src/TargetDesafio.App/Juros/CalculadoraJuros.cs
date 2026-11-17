namespace TargetDesafio.App.Juros;

public static class CalculadoraJuros
{
    private const decimal TaxaDiaria = 0.025m;

    // Públicas para o JurosConsole usar a mesma faixa na validação e na mensagem.
    public const decimal ValorMinimo = 0.01m;
    public const decimal ValorMaximo = 1_000_000_000.00m;

    public static ResultadoJuros Calcular(decimal valor, DateOnly vencimento, DateOnly dataReferencia)
    {
        if (valor < ValorMinimo || valor > ValorMaximo)
        {
            throw new ArgumentException($"O valor deve estar entre {ValorMinimo:C} e {ValorMaximo:C}.");
        }

        // DayNumber é o número de dias desde 01/01/0001. A diferença dá os dias corridos entre as datas.
        int diasAtraso = dataReferencia.DayNumber - vencimento.DayNumber;

        // Vencimento hoje ou no futuro não tem atraso.
        if (diasAtraso < 0)
        {
            diasAtraso = 0;
        }

        decimal juros = Math.Round(valor * TaxaDiaria * diasAtraso, 2, MidpointRounding.AwayFromZero);
        decimal valorAtualizado = valor + juros;

        return new ResultadoJuros(diasAtraso, juros, valorAtualizado);
    }
}
