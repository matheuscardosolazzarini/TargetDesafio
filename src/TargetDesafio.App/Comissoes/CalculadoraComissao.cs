namespace TargetDesafio.App.Comissoes;

public static class CalculadoraComissao
{
    public static decimal Calcular(decimal valorVenda)
    {
        if (valorVenda < 100m)
        {
            return 0m;
        }

        if (valorVenda < 500m)
        {
            return valorVenda * 0.01m;
        }

        return valorVenda * 0.05m;
    }

    public static IReadOnlyList<ComissaoVendedor> CalcularPorVendedor(IEnumerable<Venda> vendas)
    {
        var resultado = new List<ComissaoVendedor>();

        foreach (var grupo in vendas.GroupBy(venda => venda.Vendedor))
        {
            int quantidade = 0;
            decimal totalVendido = 0m;
            decimal totalComissao = 0m;

            foreach (var venda in grupo)
            {
                quantidade++;
                totalVendido += venda.Valor;
                totalComissao += Calcular(venda.Valor);
            }

            // Arredonda só o total: arredondar venda a venda acumula diferença de centavos.
            totalComissao = Math.Round(totalComissao, 2, MidpointRounding.AwayFromZero);

            resultado.Add(new ComissaoVendedor(grupo.Key, quantidade, totalVendido, totalComissao));
        }

        return resultado.OrderByDescending(item => item.TotalComissao).ToList();
    }
}
