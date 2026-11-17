using System.Globalization;
using System.Text;
using TargetDesafio.App.Comissoes;
using TargetDesafio.App.Estoque;
using TargetDesafio.App.Juros;

var cultura = new CultureInfo("pt-BR");
CultureInfo.DefaultThreadCurrentCulture = cultura;
CultureInfo.CurrentCulture = cultura;
Console.OutputEncoding = Encoding.UTF8;

while (true)
{
    Console.WriteLine();
    Console.WriteLine("1 - Comissão por vendedor");
    Console.WriteLine("2 - Movimentação de estoque");
    Console.WriteLine("3 - Juros por atraso");
    Console.WriteLine("0 - Sair");
    Console.Write("Opção: ");

    string? opcao = Console.ReadLine();

    // ReadLine devolve null quando a entrada acaba (ex.: entrada redirecionada). Sai para não ficar em loop.
    if (opcao is null || opcao.Trim() == "0")
    {
        break;
    }

    switch (opcao.Trim())
    {
        case "1":
            ComissoesConsole.Executar();
            break;
        case "2":
            EstoqueConsole.Executar();
            break;
        case "3":
            JurosConsole.Executar();
            break;
        default:
            Console.WriteLine("Opção inválida. Tente novamente.");
            break;
    }
}
