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
            string texto = LerTexto("Valor (ex.: 1.234,56): ");

            // Lê no formato brasileiro, com vírgula decimal, pela cultura pt-BR configurada no Program.cs.
            if (decimal.TryParse(texto, NumberStyles.Number, CultureInfo.CurrentCulture, out decimal valor))
            {
                return valor;
            }

            Console.WriteLine("Valor inválido. Use o formato 1.234,56.");
        }
    }

    private static DateOnly LerData(string pergunta)
    {
        while (true)
        {
            string texto = LerTexto(pergunta).Trim();

            if (DateOnly.TryParseExact(texto, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out DateOnly data))
            {
                return data;
            }

            Console.WriteLine("Data inválida. Use o formato dd/MM/yyyy.");
        }
    }

    private static string LerTexto(string pergunta)
    {
        Console.Write(pergunta);
        string? texto = Console.ReadLine();

        // ReadLine devolve null quando a entrada acaba. Sem isso as perguntas repetiriam para sempre.
        if (texto is null)
        {
            throw new InvalidOperationException("A entrada de dados foi encerrada.");
        }

        return texto;
    }
}
