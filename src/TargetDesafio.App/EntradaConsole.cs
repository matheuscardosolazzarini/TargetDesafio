namespace TargetDesafio.App;

public static class EntradaConsole
{
    public static string LerTexto(string pergunta)
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

    public static int LerNumero(string pergunta)
    {
        while (true)
        {
            string texto = LerTexto(pergunta);

            if (int.TryParse(texto, out int numero))
            {
                return numero;
            }

            Console.WriteLine("Digite um número inteiro.");
        }
    }
}
