using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipGame
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // ------------------------------------------------------------------------------------
            // ----------------------------| Game info and inscrution |----------------------------
            // ------------------------------------------------------------------------------------
            Console.Clear();
            Console.Write("\n\t Tu bedzie instrukcja gry");
            Console.Write("\n\nPress <Enter> to continue...");
            while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }


            // -------------------------------------------------------------------------------------
            // ----------------------------| Player 1 Places His Ships |----------------------------
            // -------------------------------------------------------------------------------------
            bool isFirstPlayerTurn = true;
            
            Console.Clear();
            Console.Write("\n\t| Player 1 Round. Please swap places |");
            Console.Write("\n\nPress <Enter> to continue...");
            while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
            
            Player firstPlayer = new Player(isFirstPlayerTurn);


            // -------------------------------------------------------------------------------------
            // ----------------------------| Player 2 Places His Ships |----------------------------
            // -------------------------------------------------------------------------------------
            isFirstPlayerTurn = false;
            
            Console.Clear();
            Console.Write("\n\t| Player 2 Round. Please swap places |");
            Console.Write("\n\nPress <Enter> to continue...");
            while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }       
            
            Player secondPlayer = new Player(isFirstPlayerTurn);

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

                if (showEnemyMoves) {                  
                    Console.Write("\n Moves made by Player 2:");
                    for (int i = 0; i < firstPlayer.enemyPlayerMoves.Count; i++) {
                        Console.WriteLine("- " + firstPlayer.enemyPlayerMoves[i]);
                    }
                    firstPlayer.enemyPlayerMoves.Clear();
                }

                ShowGameboard();
                bool endPlayerAttack = false;
                do
                {
                    endPlayerAttack = secondPlayer.enemyPlayerAttack();
                    Console.Clear();
                    Console.WriteLine("\n                                         | Player 1 Turn |\n");
                    ShowGameboard();
                    endOfGame = secondPlayer.checkIfShipsAreAlive();
                    if (endOfGame) break;

                    string messageForEnemyMove = !endPlayerAttack ? "HIT" : "MISS";
                    Console.WriteLine($"\n Thats {messageForEnemyMove} !\n");
                } while(!endPlayerAttack);               
                
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
                for (int i = 0; i < secondPlayer.enemyPlayerMoves.Count; i++) {
                    Console.Write("\n- " + secondPlayer.enemyPlayerMoves[i]);
                }
                secondPlayer.enemyPlayerMoves.Clear();        

                ShowGameboard();
                endPlayerAttack = false;

                do
                {                 
                    endPlayerAttack = firstPlayer.enemyPlayerAttack();
                    Console.Clear();
                    Console.WriteLine("\n                                         | Player 1 Turn |\n");
                    ShowGameboard();
                    endOfGame = firstPlayer.checkIfShipsAreAlive();
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

            } while(!endOfGame);


            // ------------------------------------------------------------------------
            // ----------------------------| Win message | ----------------------------
            // -------------------------------------------------------------------------    
            int winningPlayerNumeber = didFirstPlayerWon ? 1 : 2;
            Console.WriteLine("\n\t\t\t\t\t +---------------+");
            Console.WriteLine($"\t\t\t\t\t | PLAYER {winningPlayerNumeber} WINS |");
            Console.WriteLine("\t\t\t\t\t +---------------+");


            Console.Write("\n\nPress <Enter> to exit...");
            while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }


            void ShowGameboard()
            {
                int playerNumber = isFirstPlayerTurn ? 1 : 2;
                Console.WriteLine("\n                  | Enemy Board |             \t                  | Your Board |");
                Console.WriteLine("\n       0   1   2   3   4   5   6   7   8   9  \t       0   1   2   3   4   5   6   7   8   9  ");
                for (int i = 1; i <= 21; i++)
                {
                    string row = "";
                    for (int j = 0; j < 2; j++)
                    {

                        row += i % 2 != 0 ? "     +" : $"   {ConvertToRowCharacter(i / 2)} |";

                        for (int k = 0; k < 10; k++)
                        {
                            string rowElement;
                            if (i % 2 != 0)
                            {
                                rowElement = "---+";
                            }
                            else if (j == 1 && isFirstPlayerTurn || j == 0 && !isFirstPlayerTurn)
                            {
                                rowElement = $" {firstPlayer.getPlayerBoard((i / 2) - 1, k, isFirstPlayerTurn ? true : false)} |";
                            }
                            else rowElement = $" {secondPlayer.getPlayerBoard((i / 2) - 1, k, !isFirstPlayerTurn ? true : false)} |";
                            row += rowElement;
                        }

                        row += "\t";
                    }

                    Console.WriteLine(row);
                }
            }
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
