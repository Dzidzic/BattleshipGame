﻿using System;
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

            // Player 1 Places His Ships
            bool isFirstPlayerTurn = true;
            
            Console.Clear();
            Console.Write("\nPress <Enter> to continue...");
            while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
            
            Player firstPlayer = new Player(isFirstPlayerTurn);


            // Player 2 Places His Ships
            isFirstPlayerTurn = false;
            
            Console.Clear();
            Console.Write("\nPress <Enter> to continue...");
            while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }       
            
            Player secondPlayer = new Player(isFirstPlayerTurn);

            // Game Loop
            bool didFirstPlayerWon = true;
            bool endOfGame = false;
            do
            {
                // Player 1 Turn
                isFirstPlayerTurn = true;
                
                Console.Clear();
                Console.Write("\nPress <Enter> to continue...");
                while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
                

                ShowGameboard();
                bool endPlayerAttack = false;
                do
                {
                    endPlayerAttack = secondPlayer.enemyPlayerAttack();
                    ShowGameboard();
                    endOfGame = secondPlayer.checkIfShipsAreAlive();
                    if (endOfGame) break;
                        
                } while(!endPlayerAttack);
                
                if (endOfGame)
                {
                    didFirstPlayerWon = true;
                    break;
                }


                // Player 2 Turn
                isFirstPlayerTurn = false;
                
                Console.Clear();
                Console.Write("\nPress <Enter> to continue...");
                while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }

                ShowGameboard();
                endPlayerAttack = false;
                do
                {
                    endPlayerAttack = firstPlayer.enemyPlayerAttack();
                    ShowGameboard();
                    endOfGame = firstPlayer.checkIfShipsAreAlive();
                    if (endOfGame) break;

                } while (!endPlayerAttack);

                if (endOfGame)
                {
                    didFirstPlayerWon = false;
                    break;
                }

            } while(!endOfGame);


            int winningPlayerNumeber = didFirstPlayerWon ? 1 : 2;
            Console.WriteLine("\n\t\t\t\t\t +---------------+");
            Console.WriteLine($"\t\t\t\t\t | PLAYER {winningPlayerNumeber} WINS |");
            Console.WriteLine("\t\t\t\t\t +---------------+");


            Console.Write("\nPress <Enter> to exit...");
            while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }


            void ShowGameboard()
            {
                Console.Clear();
                int playerNumber = isFirstPlayerTurn ? 1 : 2;
                Console.WriteLine($"\n                                         | Player {playerNumber} Turn |\n");
                Console.WriteLine("\n                  | Enemy Board |             \t                  | Your Board |");
                Console.WriteLine("\n       0   1   2   3   4   5   6   7   8   9  \t       0   1   2   3   4   5   6   7   8   9  ");
                for (int i = 1; i <= 21; i++)
                {
                    string row = "";
                    for (int j = 0; j < 2; j++)
                    {

                        row += i % 2 != 0 ? "     +" : $"   {convertToRowCharacter(i / 2)} |";

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

        public static string convertToRowCharacter(int characterNumber)
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
