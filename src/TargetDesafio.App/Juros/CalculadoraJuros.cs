namespace TargetDesafio.App.Juros;

public static class CalculadoraJuros
{
    private const decimal TaxaDiaria = 0.025m;

    public static ResultadoJuros Calcular(decimal valor, DateOnly vencimento, DateOnly dataReferencia)
    {
        if (valor <= 0m)
        {
            throw new ArgumentException("O valor deve ser maior que zero.");
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
