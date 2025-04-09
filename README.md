# Simon Says 🗣️​

**Simon Says** é um jogo de memória simples onde o objetivo é seguir uma sequência crescente de comandos (letras W, A, S, D) e repetí-la corretamente. A cada ronda a sequência aumenta, tornando-se cada vez mais difícil. O jogo termina quando o jogador falha em repetir a sequência corretamente.

## Classes do Projeto

### `Program.cs`
Esta classe contém o ponto de entrada principal para a aplicação. Ela cria uma instância do jogo e exibe o menu principal para o jogador.

### `Game.cs`
A classe `Game` contém a lógica principal do jogo. Ela gerencia o fluxo do jogo, incluindo:
- Exibição do menu principal (Start Game, View Game Stats, Quit).
- Início do jogo, onde a sequência é gerada e o jogador tenta reproduzi-la.
- Armazenamento e exibição dos resultados do jogo, incluindo a criação de um gráfico de eliminações por ronda.

### `CommandProvider.cs`
Esta classe é responsável por gerar as sequências aleatórias de comandos para o jogo. Ela cria uma sequência de comandos (`W`, `A`, `S`, `D`) com base no número da ronda, sendo mais longa a cada nova ronda.

### `GameResult.cs`
A classe `GameResult` representa o resultado de uma partida. Ela armazena:
- A sequência que causou a perda do jogador.
- O tempo que o jogador levou para completar o jogo.
- A ronda em que o jogador perdeu.

## Diagrama UML

```mermaid
classDiagram
    class Program {
        +static void Main(string[] args)
    }

    class Game {
        +CommandProvider commandProvider
        +GameResult[] gameStats
        +void ShowMenu()
        +void StartGame()
        +void ShowGameStats()
    }

    class GameResult {
        +string LosingPattern
        +double TimeTaken
        +int Round
    }

    class CommandProvider {
        +char[] commands
        +Random random
        +string GeneratePattern(int round)
    }

    Program --> Game
    Game --> GameResult
    Game --> CommandProvider
    GameResult --> CommandProvider