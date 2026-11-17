using System.Globalization;

namespace TargetDesafio.App.Juros;

public static class JurosConsole
{
    public static void Executar()
    {
        try
        {
            Console.WriteLine();
            decimal valor = LerValor();
            DateOnly vencimento = LerData("Vencimento (dd/mm/aaaa): ");
            DateOnly hoje = DateOnly.FromDateTime(DateTime.Today);

            var resultado = CalculadoraJuros.Calcular(valor, vencimento, hoje);

            Console.WriteLine();
            Console.WriteLine($"Valor original:   {valor:C}");
            Console.WriteLine($"Vencimento:       {vencimento:dd/MM/yyyy}");
            Console.WriteLine($"Data de hoje:     {hoje:dd/MM/yyyy}");
            Console.WriteLine($"Dias de atraso:   {resultado.DiasAtraso}");
            Console.WriteLine($"Juros:            {resultado.Juros:C}");
            Console.WriteLine($"Valor atualizado: {resultado.ValorAtualizado:C}");
        }
        catch (InvalidOperationException erro)
        {
            Console.WriteLine($"Erro: {erro.Message}");
        }
        catch (ArgumentException erro)
        {
            Console.WriteLine($"Erro: {erro.Message}");
        }
    }

    private static decimal LerValor()
    {
        while (true)
        {
            string texto = EntradaConsole.LerTexto("Valor (ex.: 1234,56): ").Trim();

            // Só aceita vírgula como separador decimal (cultura pt-BR do Program.cs).
            // Sem AllowThousands, o ponto é recusado: "100.50" não vira 10050 por engano.
            // AllowLeadingSign aceita o "-" para o negativo cair na mensagem da faixa, que explica melhor o erro.
            NumberStyles estilo = NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign;
            bool formatoValido = decimal.TryParse(texto, estilo, CultureInfo.CurrentCulture, out decimal valor);

            // Se arredondar para 2 casas muda o número, ele tinha mais de 2 casas decimais (ex.: 10,005).
            if (!formatoValido || Math.Round(valor, 2) != valor)
            {
                Console.WriteLine("Valor inválido. Use vírgula e no máximo 2 casas decimais, sem ponto de milhar nem R$ (ex.: 1234,56).");
                continue;
            }

            if (valor < CalculadoraJuros.ValorMinimo || valor > CalculadoraJuros.ValorMaximo)
            {
                Console.WriteLine($"O valor deve estar entre {CalculadoraJuros.ValorMinimo:C} e {CalculadoraJuros.ValorMaximo:C}.");
                continue;
            }

            return valor;
        }
    }

    private static DateOnly LerData(string pergunta)
    {
        while (true)
        {
            string texto = EntradaConsole.LerTexto(pergunta).Trim();

            // "d/M/yyyy" aceita dia e mês com um ou dois dígitos: 07/11/2026 e 7/11/2026.
            if (DateOnly.TryParseExact(texto, "d/M/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out DateOnly data))
            {
                return data;
            }

            Console.WriteLine("Data inválida. Use dia/mês/ano (ex.: 07/11/2026).");
        }
    }
}
