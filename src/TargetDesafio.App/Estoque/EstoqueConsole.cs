namespace TargetDesafio.App.Estoque;

public static class EstoqueConsole
{
    // Fica guardado entre uma entrada e outra no menu, para o ID ser único durante toda a execução do programa.
    private static ControleEstoque? _controle;

    public static void Executar()
    {
        try
        {
            if (_controle is null)
            {
                var arquivo = ArquivoJson.Ler<EstoqueArquivo>("Data/estoque.json");

                // Sem a chave "estoque" no JSON, o desserializador deixa a lista nula.
                if (arquivo.Estoque is null)
                {
                    Console.WriteLine("Erro: o arquivo estoque.json não tem a lista \"estoque\".");
                    return;
                }

                _controle = new ControleEstoque(arquivo.Estoque);
            }

            MostrarProdutos(_controle);

            bool continuar = true;

            while (continuar)
            {
                RegistrarMovimentacao(_controle);
                continuar = PerguntarSimOuNao("Fazer outra movimentação? (S/N): ");
            }

            MostrarMovimentacoes(_controle);
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

    private static void MostrarProdutos(ControleEstoque controle)
    {
        Console.WriteLine();
        Console.WriteLine($"{"Código",-8}{"Descrição",-28}{"Estoque",8}");

        foreach (var produto in controle.Produtos)
        {
            Console.WriteLine($"{produto.CodigoProduto,-8}{produto.DescricaoProduto,-28}{produto.Quantidade,8}");
        }
    }

    private static void RegistrarMovimentacao(ControleEstoque controle)
    {
        Console.WriteLine();
        int codigo = LerCodigo(controle);
        TipoMovimentacao tipo = LerTipo();
        int quantidade = LerQuantidade();
        string descricao = LerDescricao();

        // Erro de regra aqui só cancela esta movimentação. O usuário pode tentar outra em seguida.
        try
        {
            var movimentacao = controle.Movimentar(codigo, tipo, quantidade, descricao);
            Console.WriteLine(Formatar(controle, movimentacao));
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

    private static void MostrarMovimentacoes(ControleEstoque controle)
    {
        Console.WriteLine();
        Console.WriteLine("Movimentações:");

        if (controle.Movimentacoes.Count == 0)
        {
            Console.WriteLine("Nenhuma movimentação registrada.");
            return;
        }

        foreach (var movimentacao in controle.Movimentacoes)
        {
            Console.WriteLine($"{movimentacao.Data:dd/MM/yyyy HH:mm:ss} | {Formatar(controle, movimentacao)}");
        }
    }

    private static string Formatar(ControleEstoque controle, Movimentacao movimentacao)
    {
        string nomeProduto = controle.Produtos.First(p => p.CodigoProduto == movimentacao.CodigoProduto).DescricaoProduto;
        string tipo = movimentacao.Tipo == TipoMovimentacao.Entrada ? "Entrada" : "Saída";

        return $"Movimentação #{movimentacao.Id} | {tipo} | {movimentacao.Quantidade} un. | {nomeProduto} | " +
               $"{movimentacao.Descricao} | Estoque final: {movimentacao.EstoqueFinal}";
    }

    // Valida já na digitação para o usuário corrigir só esta resposta.
    // O ControleEstoque continua validando, porque pode ser chamado sem passar pelo console (ex.: testes).
    private static int LerCodigo(ControleEstoque controle)
    {
        while (true)
        {
            int codigo = EntradaConsole.LerNumero("Código do produto: ");

            if (controle.Produtos.Any(p => p.CodigoProduto == codigo))
            {
                return codigo;
            }

            Console.WriteLine("Produto não encontrado. Use um dos códigos da lista.");
        }
    }

    private static int LerQuantidade()
    {
        while (true)
        {
            int quantidade = EntradaConsole.LerNumero("Quantidade (maior que zero): ");

            if (quantidade > 0)
            {
                return quantidade;
            }

            Console.WriteLine("A quantidade deve ser maior que zero.");
        }
    }

    private static string LerDescricao()
    {
        while (true)
        {
            string descricao = EntradaConsole.LerTexto(
                "Descrição (ex.: Compra de fornecedor, Venda balcão, Devolução de cliente, Ajuste de inventário): ");

            if (!string.IsNullOrWhiteSpace(descricao))
            {
                return descricao;
            }

            Console.WriteLine("A descrição é obrigatória.");
        }
    }

    private static TipoMovimentacao LerTipo()
    {
        while (true)
        {
            string texto = EntradaConsole.LerTexto("Tipo (E = entrada, S = saída): ").Trim().ToUpper();

            if (texto == "E")
            {
                return TipoMovimentacao.Entrada;
            }

            if (texto == "S")
            {
                return TipoMovimentacao.Saida;
            }

            Console.WriteLine("Digite E ou S.");
        }
    }

    private static bool PerguntarSimOuNao(string pergunta)
    {
        while (true)
        {
            string texto = EntradaConsole.LerTexto(pergunta).Trim().ToUpper();

            if (texto == "S")
            {
                return true;
            }

            if (texto == "N")
            {
                return false;
            }

            Console.WriteLine("Digite S ou N.");
        }
    }
}
