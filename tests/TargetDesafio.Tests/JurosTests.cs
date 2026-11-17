using System.Globalization;
using TargetDesafio.App.Juros;

namespace TargetDesafio.Tests;

public class JurosTests
{
    private static DateOnly Data(string texto)
    {
        return DateOnly.ParseExact(texto, "dd/MM/yyyy", CultureInfo.InvariantCulture);
    }

    // decimal e DateOnly não podem ser usados direto em atributo, por isso os valores vêm como texto.
    [Theory]
    [InlineData("100.00", "20/11/2026", "17/11/2026", 0, "0.00")]
    [InlineData("100.00", "17/11/2026", "17/11/2026", 0, "0.00")]
    [InlineData("100.00", "07/11/2026", "17/11/2026", 10, "25.00")]
    [InlineData("333.33", "16/11/2026", "17/11/2026", 1, "8.33")]
    // 0,20 x 2,5% = 0,005: com AwayFromZero vira 0,01 (o arredondamento bancário daria 0,00).
    [InlineData("0.20", "16/11/2026", "17/11/2026", 1, "0.01")]
    public void Calcular_DiasEJuros(string valor, string vencimento, string referencia, int diasEsperados, string jurosEsperados)
    {
        decimal valorDecimal = decimal.Parse(valor, CultureInfo.InvariantCulture);
        decimal juros = decimal.Parse(jurosEsperados, CultureInfo.InvariantCulture);

        var resultado = CalculadoraJuros.Calcular(valorDecimal, Data(vencimento), Data(referencia));

        Assert.Equal(diasEsperados, resultado.DiasAtraso);
        Assert.Equal(juros, resultado.Juros);
        Assert.Equal(valorDecimal + juros, resultado.ValorAtualizado);
    }

    [Theory]
    [InlineData("0")]
    [InlineData("-10.00")]
    public void Calcular_ValorMenorOuIgualAZero_LancaErro(string valor)
    {
        decimal valorDecimal = decimal.Parse(valor, CultureInfo.InvariantCulture);

        Assert.Throws<ArgumentException>(
            () => CalculadoraJuros.Calcular(valorDecimal, Data("07/11/2026"), Data("17/11/2026")));
    }
}
