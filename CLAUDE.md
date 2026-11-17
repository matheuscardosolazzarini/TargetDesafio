# TargetDesafio

Desafio técnico da Target Sistemas para a vaga de Desenvolvedor de Sistemas Jr. Entrega até 22/11/2026, com o link do GitHub respondendo o e-mail da Gupy.

## Objetivo

Console .NET 8 com três exercícios independentes, escolhidos por menu. A Target avalia lógica, organização, criatividade e a forma de estruturar a solução.

Código simples e direto, fácil de ler e de explicar. Na dúvida entre uma forma curta e esperta e uma forma óbvia, use a óbvia.

## Estrutura

```
TargetDesafio/
├── CLAUDE.md
├── README.md                     último passo
├── docs/specs/                   uma spec por exercício
├── src/TargetDesafio.App/        console
│   ├── Comissoes/
│   ├── Estoque/
│   ├── Juros/
│   ├── Data/                     vendas.json, estoque.json
│   ├── ArquivoJson.cs
│   └── Program.cs
└── tests/TargetDesafio.Tests/    xUnit
```

## Specs

- `docs/specs/01-comissoes.md`
- `docs/specs/02-estoque.md`
- `docs/specs/03-juros.md`

Implementar uma por vez, nesta ordem. Só começar a próxima quando a anterior estiver com build e testes passando e revisada.

## Regras gerais

- Dois projetos: `src/TargetDesafio.App` e `tests/TargetDesafio.Tests`. Não criar outros.
- Cada exercício fica na sua pasta, com namespace igual à pasta (`TargetDesafio.App.Comissoes` etc.).
- Classes de cálculo não usam `Console` nem leem arquivo. Recebem dados e devolvem resultado.
- Entrada e saída de cada exercício ficam numa classe estática `XxxConsole` com um método `Executar()`, dentro da pasta do exercício.
- `Program.cs` só configura cultura e encoding e mostra o menu principal.
- Valores monetários em `decimal`. Arredondamento com `Math.Round(valor, 2, MidpointRounding.AwayFromZero)`.
- Cultura pt-BR para formatar e ler números e datas (`R$ 1.234,56`, `dd/MM/yyyy`). `Console.OutputEncoding = Encoding.UTF8`.
- Erros de regra de negócio viram exceção (`ArgumentException` ou `InvalidOperationException`) com mensagem em português. O `XxxConsole` captura e mostra a mensagem, sem derrubar o programa.

### Não adicionar

Injeção de dependência, interfaces, repositórios, camadas, banco de dados, pacotes NuGet além dos do template de teste, persistência em arquivo, logs.

### Não remover

Separação entre cálculo e console, enum de tipo de movimentação, validações do estoque, testes listados nas specs.

## Código compartilhado

### ArquivoJson

`ArquivoJson.Ler<T>(string caminho)` na raiz do App, com `System.Text.Json` e `PropertyNameCaseInsensitive = true`. Os JSON ficam em `Data/` e são copiados para a saída do build (já configurado no `.csproj`). Caminho base: `AppContext.BaseDirectory`. Os testes usam o mesmo método para ler os arquivos.

### Menu principal

```
1 - Comissão por vendedor
2 - Movimentação de estoque
3 - Juros por atraso
0 - Sair
```

Opção inválida mostra aviso e repete o menu. Ao terminar um exercício, volta para o menu. Cada opção entra no menu quando o exercício correspondente for implementado.

## Convenções

- Identificadores em português, para casar com o domínio do enunciado (`CalculadoraComissao`, `Movimentar`).
- Comentários em português, só quando o motivo não for óbvio pelo código.
- Um tipo por arquivo. Namespaces no formato file-scoped.
- Build sem warnings, com nullable habilitado.
- Testes: um arquivo por exercício, `[Theory]` quando a mesma regra tiver vários valores.

## Comandos

```
dotnet build
dotnet test
dotnet run --project src/TargetDesafio.App
```

## Como trabalhar

- Ler a spec do exercício inteira antes de escrever código.
- Se a spec for ambígua ou conflitar com o código, parar e perguntar. Não decidir sozinho.
- Ao terminar: `dotnet build` sem warnings e `dotnet test` verde.
- Não fazer commit. Os commits são feitos depois da revisão.
- No fim de cada exercício, responder com uma lista curta: arquivos criados, e cada decisão tomada que não estava escrita na spec, com o motivo.

## Texto (README e mensagens)

Curto, direto, em português. Evitar sinais de texto de IA: palavras infladas ("crucial", "robusto", "fundamental"), "não é só X, é Y", grupos de três forçados, gerúndio no fim da frase ("garantindo", "destacando"), negrito em excesso, travessão no lugar de vírgula, emoji.

## README

Último passo, depois dos três exercícios:

- Como rodar (comandos acima).
- Premissas: juntar a seção "Premissas" de cada spec.
- Exemplo de saída de cada exercício.
