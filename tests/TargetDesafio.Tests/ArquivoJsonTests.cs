using TargetDesafio.App;
using TargetDesafio.App.Comissoes;

namespace TargetDesafio.Tests;

public class ArquivoJsonTests
{
    [Fact]
    public void Ler_ArquivoInexistente_LancaErro()
    {
        Assert.Throws<InvalidOperationException>(() => ArquivoJson.Ler<VendasArquivo>("Data/nao-existe.json"));
    }

    [Fact]
    public void Ler_JsonMalFormado_LancaErro()
    {
        // Path.Combine ignora o diretório base quando o caminho é absoluto, então o arquivo temporário funciona.
        string caminho = Path.GetTempFileName();
        File.WriteAllText(caminho, "{ \"vendas\": [ ");

        try
        {
            var erro = Assert.Throws<InvalidOperationException>(() => ArquivoJson.Ler<VendasArquivo>(caminho));
            Assert.Contains("não é um JSON válido", erro.Message);
        }
        finally
        {
            File.Delete(caminho);
        }
    }
}
