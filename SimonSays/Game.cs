using System;
using System.Diagnostics;
using Spectre.Console;

namespace SimonSays
{
    public class Game
    {
        /// <summary>
        /// The provider responsible for generating the patterns used in the 
        /// game.
        /// </summary>
        private readonly CommandProvider commandProvider;

        /// <summary>
        /// A list to store the last 5 game results for the game stats board.
        /// </summary>
        private readonly GameResult[] gameStats;

        /// <summary>
        /// Initializes a new instance of the <see cref="Game"/> class.
        /// Sets up the command provider, and initializes the game
        /// stats.
        /// </summary>
        public Game()
        {
            commandProvider = new CommandProvider();
            gameStats = new GameResult[5];  // Um array para armazenar os últimos 5 resultados
        }

        /// <summary>
        /// Displays the main menu of the game and prompts the user to choose
        /// an option. The available choices are to start the game, view the 
        /// game stats board, or quit the game.
        /// </summary>
        /// <remarks>
        /// This method uses the <see cref="Spectre.Console"/> library to 
        /// display the main menu and handle user input for choosing different 
        /// options. The game will continue prompting 
        /// the user until they choose to quit.
        /// </remarks>
        public void ShowMenu()
        {
            while (true)
            {
                AnsiConsole.Clear();
                string choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[bold yellow]Simon Says[/]")
                        .AddChoices("Start Game", "View Game Stats", "Quit"));

                switch (choice)
                {
                    case "Start Game":
                        StartGame();
                        break;
                    case "View Game Stats":
                        ShowGameStats();
                        break;
                    case "Quit":
                        return; 
                }
            }
        }

        private void StartGame()
        {
            int round = 1;
            Stopwatch stopwatch = new Stopwatch();

            while (true)
            {
                string pattern = commandProvider.GeneratePattern(round);
                AnsiConsole.Clear();
                AnsiConsole.MarkupLine("[bold green]Simon Says Follow this" + 
                    " Pattern:[/]");
                AnsiConsole.MarkupLine($"[italic yellow]{pattern}[/]");

                stopwatch.Start();
                string userInput = AnsiConsole.Ask<string>("\n[bold cyan]Repeat"
                    + " the Pattern (use W, A, S, D):[/] ");
                stopwatch.Stop();

                bool isCorrect = userInput.Equals(pattern, StringComparison.OrdinalIgnoreCase);

                if (!isCorrect)
                {
                    double timeTaken = stopwatch.Elapsed.TotalSeconds;

                    AnsiConsole.MarkupLine("\n[bold red]You Lost![/]");
                    AnsiConsole.MarkupLine(
                        $"[bold]Time Taken:[/] {timeTaken:F2} Seconds");
                    AnsiConsole.MarkupLine(
                        $"[bold]You Lost at Round:[/] {round}");

                    // Shift existing entries
                    for (int i = gameStats.Length - 1; i > 0; i--)
                    {
                        gameStats[i] = gameStats[i - 1];
                    }

                    // Add new result at the beginning
                    gameStats[0] = new GameResult(pattern, timeTaken, round);

                    AnsiConsole.Markup("\n[bold green]Press Enter to Return to"
                        + " the Menu...[/]");
                    Console.ReadLine();
                    break;
                }

                round++;
            }
        }

        private void ShowGameStats()
        {
            AnsiConsole.Clear();
            Table table = new Table();
            table.AddColumn("#");
            table.AddColumn("Round");
            table.AddColumn("Time Taken (s)");
            table.AddColumn("Losing Pattern");
            
            int[] eliminationsByRound = new int[10];
            
            int roundCounter = 1;
            foreach (var result in gameStats)
            {
                if (result != null)
                {
                    table.AddRow(
                        roundCounter.ToString(),
                        result.Round.ToString(),
                        result.TimeTaken.ToString("F2"),  
                        result.LosingPattern 
                    );
                    if (result.Round <= eliminationsByRound.Length)
                    {
                        eliminationsByRound[result.Round - 1]++;
                    }
                    roundCounter++;
                }
            }

            AnsiConsole.Write(table);
            AnsiConsole.Markup("\n[bold yellow]Number of Eliminations per Round[/]");
            var chart = new BarChart()
                .Width(60)
                .Label("[bold cyan]Eliminations[/]");
            foreach (var eliminations in eliminationsByRound)
            {
                chart.AddItem(eliminations.ToString(), eliminations, Color.Green);
            }
            AnsiConsole.Write(chart);
            AnsiConsole.Markup("\n[bold green]Press Enter to Return to Menu...[/]");
            Console.ReadLine();
        }
    }
}