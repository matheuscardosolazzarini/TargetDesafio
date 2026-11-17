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
            DateOnly vencimento = LerData("Vencimento (dd/MM/yyyy): ");
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
            if (!decimal.TryParse(texto, NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out decimal valor))
            {
                Console.WriteLine("Valor inválido. Use só números e vírgula para os centavos, ex.: 1234,56.");
                continue;
            }

            if (valor <= 0m)
            {
                Console.WriteLine("O valor deve ser maior que zero.");
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

            if (DateOnly.TryParseExact(texto, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out DateOnly data))
            {
                return data;
            }

            Console.WriteLine("Data inválida. Use o formato dd/MM/yyyy.");
        }
    }
}
