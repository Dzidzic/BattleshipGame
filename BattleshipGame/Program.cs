using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BattleshipGame
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool isFirstPlayerTurn;
            bool endLoop = false;
            int firstPlayerWins = 0;
            int secondPlayerWins = 0;
            // ------------------------------------------------------------------------------------
            // ----------------------------| Game info and inscrution |----------------------------
            // ------------------------------------------------------------------------------------

            Console.Clear();
            Console.Write("\n\t Tu bedzie instrukcja gry");    

            Console.WriteLine("\n\t\t\t\tSelect a game mode");


            bool isItPVP = true;
            do
            {
                if(isItPVP) Console.WriteLine("\n\t\t   > Player vs Player < |   Player vs Computer  ");
                else Console.WriteLine("\n\t\t     Player vs Player   | > Player vs Computer <");

                ConsoleKey key = Console.ReadKey(true).Key;

                if ((key == ConsoleKey.RightArrow || key == ConsoleKey.D) && isItPVP) isItPVP = false;
                else if ((key == ConsoleKey.LeftArrow || key == ConsoleKey.A) && !isItPVP) isItPVP = true;
                else if (key == ConsoleKey.Enter) endLoop = true;

                Console.SetCursorPosition(0, Console.CursorTop - 2);
            } while (!endLoop);

            bool programShutdown = false;
            do
            {
                // -------------------------------------------------------------------------------------
                // ----------------------------| Player 1 Places His Ships |----------------------------
                // -------------------------------------------------------------------------------------
                isFirstPlayerTurn = true;

                Console.Clear();
                Console.Write("\n\t| Player 1 Round. Please swap places |");
                Console.Write("\n\nPress <Enter> to continue...");
                while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }

                Player firstPlayer = new Player(isFirstPlayerTurn, false);


                // -------------------------------------------------------------------------------------
                // ----------------------------| Player 2 Places His Ships |----------------------------
                // -------------------------------------------------------------------------------------
                isFirstPlayerTurn = false;

                Console.Clear();
                Console.Write("\n\t| Player 2 Round. Please swap places |");
                Console.Write("\n\nPress <Enter> to continue...");
                while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }

                Player secondPlayer = new Player(isFirstPlayerTurn, true);

                // --------------------------------------------------------------------
                // ----------------------------| Gameloop |----------------------------
                // --------------------------------------------------------------------

                bool didFirstPlayerWon = true;
                bool endOfGame = false;
                bool showEnemyMoves = false;
                do
                {
                    // -------------------------------------------------------------------------
                    // ----------------------------| Player 1 Turn |----------------------------
                    // -------------------------------------------------------------------------
                    isFirstPlayerTurn = true;

                    Console.Clear();
                    Console.Write("\n\t| Player 1 Turn. Please swap places |");
                    Console.Write("\n\nPress <Enter> to continue...");
                    while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
                    Console.Clear();

                    Console.WriteLine("\n                                         | Player 1 Turn |\n");

                    if (showEnemyMoves)
                    {
                        Console.Write("\n Moves made by Player 2: \n");
                        for (int i = 0; i < firstPlayer.enemyPlayerMoves.Count; i++)
                        {
                            Console.WriteLine("- " + firstPlayer.enemyPlayerMoves[i]);
                        }
                        firstPlayer.enemyPlayerMoves.Clear();
                    }

                    ShowGameboard();
                    bool endPlayerAttack = false;
                    do
                    {
                        endPlayerAttack = secondPlayer.EnemyPlayerAttack(false);
                        Console.Clear();
                        Console.WriteLine("\n                                         | Player 1 Turn |\n");
                        ShowGameboard();
                        endOfGame = secondPlayer.CheckIfShipsAreAlive();
                        if (endOfGame) break;

                        string messageForEnemyMove = !endPlayerAttack ? "HIT" : "MISS";
                        Console.WriteLine($"\n Thats {messageForEnemyMove} !\n");

                    } while (!endPlayerAttack);

                    if (endOfGame)
                    {
                        didFirstPlayerWon = true;
                        break;
                    }
                    Console.Write("Press <Enter> to continue...");
                    while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }

                    // --------------------------------------------------------------------------
                    // ----------------------------| Player 2 Turn | ----------------------------
                    // --------------------------------------------------------------------------
                    isFirstPlayerTurn = false;

                    Console.Clear();
                    Console.Write("\n\t| Player 2 Turn. Please swap places |");
                    Console.Write("\n\nPress <Enter> to continue...");
                    while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
                    Console.Clear();

                    Console.WriteLine("\n                                         | Player 2 Turn |\n");

                    Console.Write("\n Moves made by Player 1:");
                    for (int i = 0; i < secondPlayer.enemyPlayerMoves.Count; i++)
                    {
                        Console.Write("\n- " + secondPlayer.enemyPlayerMoves[i]);
                    }
                    secondPlayer.enemyPlayerMoves.Clear();

                    ShowGameboard();
                    endPlayerAttack = false;

                    do
                    {
                        endPlayerAttack = firstPlayer.EnemyPlayerAttack(true);
                        Console.Clear();
                        Console.WriteLine("\n                                         | Player 2 Turn |\n");
                        ShowGameboard();
                        endOfGame = firstPlayer.CheckIfShipsAreAlive();
                        if (endOfGame) break;

                        string messageForEnemyMove = !endPlayerAttack ? "HIT" : "MISS";
                        Console.WriteLine($"\n Thats {messageForEnemyMove} !\n");
                      
                    } while (!endPlayerAttack);

                    if (endOfGame)
                    {
                        didFirstPlayerWon = false;
                        break;
                    }
                    Console.WriteLine("Press <Enter> to continue...");
                    while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }

                    showEnemyMoves = true;

                } while (!endOfGame);


                // ------------------------------------------------------------------------
                // ----------------------------| Win message | ----------------------------
                // ------------------------------------------------------------------------
                if (didFirstPlayerWon) firstPlayerWins++;
                else secondPlayerWins++;
                
                int winningPlayerNumeber = didFirstPlayerWon ? 1 : 2;
                Console.WriteLine("\n\t\t\t\t\t +---------------+");
                Console.WriteLine($"\t\t\t\t\t | PLAYER {winningPlayerNumeber} WINS |");
                Console.WriteLine("\t\t\t\t\t +---------------+");
                      
                Console.WriteLine($"\n\n\t\t         Total Player 1 Wins | {firstPlayerWins} : {secondPlayerWins} | Total Player 2 Wins      ");


                endLoop = false;
                bool rematch = true;
                do
                {
                    if (rematch) Console.WriteLine("\n\t\t\t\t     > Rematch < |   End Game  ");
                    else Console.WriteLine("\n\t\t\t\t       Rematch   | > End Game <");
                    
                    ConsoleKey key = Console.ReadKey(true).Key;

                    if ((key == ConsoleKey.RightArrow || key == ConsoleKey.D) && rematch) rematch = false;
                    else if ((key == ConsoleKey.LeftArrow || key == ConsoleKey.A) && !rematch) rematch = true;
                    else if (key == ConsoleKey.Enter) endLoop = true;

                    Console.SetCursorPosition(0, Console.CursorTop - 2);
                } while(!endLoop);

                if (!rematch) programShutdown = true;

                // ------------------------------------------------------------------------------
                // ----------------------------| Program Functions | ----------------------------
                // ------------------------------------------------------------------------------
                void ShowGameboard()
                {
                    Console.WriteLine("\n                  | Enemy Board |             \t                  | Your Board |");
                    Console.WriteLine("\n       0   1   2   3   4   5   6   7   8   9  \t       0   1   2   3   4   5   6   7   8   9  ");
                    for (int i = 1; i <= 21; i++)
                    {
                        string row = "";
                        for (int j = 0; j < 2; j++)
                        {
                            if (i == 1 || i == 21) row += "     +";
                            else row += i % 2 != 0 ? "     |" : $"   {Program.ConvertToRowCharacter(i / 2)} |";

                            for (int k = 0; k < 10; k++)
                            {
                                string rowElement;
                                if (i % 2 != 0)
                                {
                                    if(i == 1 || i == 21) rowElement = "---";
                                    else rowElement = "   ";
                                }
                                else if (j == 1 && isFirstPlayerTurn || j == 0 && !isFirstPlayerTurn)
                                {
                                    rowElement = $" {firstPlayer.GetPlayerBoard((i / 2) - 1, k, isFirstPlayerTurn ? true : false)} ";
                                }
                                else rowElement = $" {secondPlayer.GetPlayerBoard((i / 2) - 1, k, !isFirstPlayerTurn ? true : false)} ";
                                row += rowElement;
                                if (i != 1 && i != 21)
                                {
                                    if (k == 9) row += "|";
                                    else row += " ";
                                }
                                else
                                {
                                    if (k == 9) row += "+";
                                    else row += "-";
                                }
                            }

                            row += "\t";
                        }

                        Console.WriteLine(row);
                    }
                }
            } while (!programShutdown);
        }
        public static string ConvertToRowCharacter(int characterNumber)
        {
            switch (characterNumber)
            {
                case 1: return "A";
                case 2: return "B";
                case 3: return "C";
                case 4: return "D";
                case 5: return "E";
                case 6: return "F";
                case 7: return "G";
                case 8: return "H";
                case 9: return "I";
                case 10: return "J";
                default: return "!";
            }
        }
    }
}
