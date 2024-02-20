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
            Player firstPlayer = new Player();
            Player secondPlayer = new Player();
            Player test = new Player();
            bool isFirstPlayerTurn = true;

            ShowGameboard();
            Console.ReadKey();


            void ShowGameboard()
            {
                Console.WriteLine("\n       1   2   3   4   5   6   7   8   9   10  \t       1   2   3   4   5   6   7   8   9   10  ");
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
                                rowElement = $" {firstPlayer.showPlayerBoard((i / 2) - 1, k, isFirstPlayerTurn ? true : false)} |";
                            }
                            else rowElement = $" {secondPlayer.showPlayerBoard((i / 2) - 1, k, !isFirstPlayerTurn ? true : false)} |";
                            row += rowElement;
                        }

                        row += "\t";
                    }

                    Console.WriteLine(row);
                }
            }
        }

        static string convertToRowCharacter(int characterNumber)
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
