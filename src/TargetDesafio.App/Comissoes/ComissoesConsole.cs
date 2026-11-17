namespace TargetDesafio.App.Comissoes;

public static class ComissoesConsole
{
    public static void Executar()
    {
        try
        {
            var arquivo = ArquivoJson.Ler<VendasArquivo>("Data/vendas.json");

            // Sem a chave "vendas" no JSON, o desserializador deixa a lista nula.
            if (arquivo.Vendas is null)
            {
                Console.WriteLine("Erro: o arquivo vendas.json não tem a lista \"vendas\".");
                return;
            }

            var comissoes = CalculadoraComissao.CalcularPorVendedor(arquivo.Vendas);

            Console.WriteLine();
            Console.WriteLine($"{"Vendedor",-18}{"Vendas",6}{"Total vendido",16}{"Comissão",12}");

            foreach (var item in comissoes)
            {
                Console.WriteLine(
                    $"{item.Vendedor,-18}{item.QuantidadeVendas,6}{item.TotalVendido,16:C}{item.TotalComissao,12:C}");
            }
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
}
