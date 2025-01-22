# Melhor Rota

Projeto simples em .NET (C#) que registra rotas em um arquivo CSV (usado como “banco de dados”) e calcula a **melhor rota** (menor custo) entre dois pontos, independentemente do número de conexões.

## Sumário

1. [Visão Geral](#visão-geral)  
2. [Como Executar](#como-executar)  
3. [Estrutura de Pastas/Arquivos](#estrutura-de-pastasarquivos)  
4. [Decisões de Design](#decisões-de-design)  
5. [Testes Unitários](#testes-unitários)

---

## Visão Geral

- **Funcionalidades Principais**:
  1. **Registrar novas rotas** (persistindo no CSV).  
  2. **Consultar a melhor rota** entre dois pontos, mesmo com várias conexões.  

- **Tecnologias/Dependências**:
  - .NET 8.0 (pode rodar no .NET 6 ou 7, se desejar).
  - MSTest para testes unitários, dentro do projeto `MelhorRota.Tests`.

- **Entrada e Saída**:
  - Lemos/escrevemos um arquivo `rotas.csv` na pasta de execução do programa (Console).
  - O usuário interage via **console**: um menu com opções para **registrar** e **consultar** rota.

---

## Como Executar

### 1. Clonar / Baixar o Repositório
Faça o clone deste repositório ou baixe os arquivos ZIP.

### 2. Abrir no Visual Studio ou Terminal

- **Visual Studio**:
  1. Abra o arquivo `.sln`.
  2. Na **Solution Explorer**, clique com botão direito em `MelhorRota.App` e selecione **Set as Startup Project**.
  3. Pressione **F5** ou **Ctrl+F5** para rodar.
  
- **.NET CLI (Terminal)**:
  1. Abra um terminal na pasta raiz do projeto.
  2. Rode:
     ```bash
     dotnet restore
     dotnet build
     dotnet run --project .\MelhorRota.App\MelhorRota.App.csproj
     ```

### 3. Uso do Programa

Ao executar, verá um menu como:
=== Bem-vindo ao Melhor Rota ===

Consultar melhor rota
Registrar nova rota
Sair Opção:


1. **Opção 1**: Permite escolher **origem** e **destino** entre os aeroportos já cadastrados. O sistema retorna a rota de **menor custo**.  
2. **Opção 2**: Adiciona uma nova rota no formato `ORIGEM,DESTINO,CUSTO` (exemplo: `GRU,SCL,20`), que será salva no CSV.  
3. **Opção 3**: Encerra a aplicação.

### 4. Executar Testes

No terminal, dentro da pasta do projeto:
```bash
dotnet test
 ```

```
Estrutura de Pastas/Arquivos
MelhorRota
├── MelhorRota.Domain
│   ├── Models
│   │   └ Rota.cs
│   ├── Interfaces
│   │   ├ IBuscaStrategy.cs
│   │   ├ IRepository.cs
│   │   └ IRotaService.cs
│   ├── Repositories
│   │   └ CsvRotaRepository.cs
│   ├── Services
│   │   └ RotaService.cs
│   └── Strategies
│       └ DepthFirstSearchStrategy.cs
├── MelhorRota.App
│   ├── Program.cs
│   └── MenuService.cs
└── MelhorRota.Tests
    ├── CsvRotaRepositoryTests.cs
    ├── DepthFirstSearchStrategyTests.cs
    └── RotaServiceTests.cs
```

```
MelhorRota.Domain
  Models: classes de domínio (ex.: Rota.cs).
  Interfaces: contratos (ex.: IRotaService, IRepository, IBuscaStrategy).
  Repositories: ex.: CsvRotaRepository (persistência em CSV).
  Services: ex.: RotaService (orquestra as rotas, monta o grafo).
  Strategies: ex.: DepthFirstSearchStrategy (algoritmo de busca).
MelhorRota.App
  Program.cs: ponto de entrada (Main).
  MenuService.cs: gerencia a interação via console (menu principal, etc.).
MelhorRota.Tests
  CsvRotaRepositoryTests.cs: testa as operações de persistência no CSV.
  DepthFirstSearchStrategyTests.cs: valida a lógica DFS independentemente do resto.
  RotaServiceTests.cs: testa a orquestração do serviço (montagem do grafo, custo).
```
```
Decisões de Design

Repository Pattern
  CsvRotaRepository abstrai o acesso ao CSV. Podemos trocar por outro tipo de persistência (por exemplo, banco de dados) sem afetar o restante.

Strategy Pattern
  IBuscaStrategy define a interface para o algoritmo de menor rota. Atualmente, usamos DepthFirstSearchStrategy (busca exaustiva).

RotaService
  É responsável por montar o grafo (dicionário de adjacências) e delegar a busca à Strategy. Também expõe métodos para inserir rotas no repositório.

Clean Code / SOLID
  Separamos responsabilidades:
  Program.cs e MenuService.cs (UI de console)
  RotaService (regra de negócio)
  CsvRotaRepository (persistência em CSV)
  DepthFirstSearchStrategy (algoritmo de busca)
```
```
Testes
  CsvRotaRepositoryTests assegura que a leitura/escrita de CSV funciona.
  DepthFirstSearchStrategyTests confirma que a DFS encontra a rota mínima, mesmo em cenários de loop ou caminhos múltiplos.
  RotaServiceTests confirma cenários completos, como “GRU->CDG = custo 40”, registro de novas rotas.
```


