using BattleshipGame;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipGame
{
    internal class Player
    {
        Gameboard playerBoard = new Gameboard();
        List<Ship> playerShips = new List<Ship>();

        public Player(bool isFirstPlayerTurn)
        {
            createShips(isFirstPlayerTurn);   
        }
        public string getPlayerBoard(int a, int b, bool isItMyTurn)
        {
            return Char.ToString(playerBoard.showGameboardField(a, b, isItMyTurn));
        }
        void createShips(bool isFirstPlayerTurn)
        {
            for (int i = 1; i <= 1; i++)
            {                           
                int n;

                if (i > 6) { n = 1; }
                else if (i > 3) { n = 2; }
                else if (i > 1) { n = 3; }
                else n = 4;

                List<int[]> curShipPartsCoordinates = new List<int[]>();

                playerShips.Add(new Ship());

                for (int j = 0; j < n; j++)
                {
                    showPlayerBoard(isFirstPlayerTurn);
                    curShipPartsCoordinates.Add(takeCoordinatesFromUser(curShipPartsCoordinates));
                    playerShips[i-1].setShipPartCoordinates(curShipPartsCoordinates[j]);
                    playerBoard.setAllPlayingFields(playerShips);
                }             
            }
        }
        int[] takeCoordinatesFromUser(List<int[]> curShipPartsCoordinates)
        {           
            int[] coordinates = new int[2];
            char[] characters;
            
            bool endThirdLoop = false;
            do
            {
                bool endSecondLoop = false;
                do
                {
                    bool endFirstLoop = false;
                    do
                    {
                        Console.Write("Podaj koordynaty części statku: ");
                        characters = Console.ReadLine().ToCharArray();

                        if (characters.Length != 2)
                        {
                            Console.WriteLine("Error! Coordinates must consist of two characters! \n");
                        }
                        else endFirstLoop = true;

                    } while (!endFirstLoop);

                    if (!CheckIfCharactersAreCorrect(characters))
                    {
                        Console.WriteLine("Error! Wrong coordinates entered! \n");
                    }
                    else endSecondLoop = true;

                } while (!endSecondLoop);

                coordinates[0] = characters[0] - 65;
                coordinates[1] = characters[1] - 48;
                
                if(CheckIfFieldIsOccupied(coordinates, curShipPartsCoordinates))
                {
                    Console.WriteLine("Error! The given place is already occupied! \n");
                }else endThirdLoop = true;

            } while (!endThirdLoop);


            return coordinates;
        }
        bool CheckIfFieldIsOccupied(int[] coordinates, List<int[]> curShipPartsCoordinates)
        {
            for (int i = 0; i < curShipPartsCoordinates.Count; i++)
            {
                if (curShipPartsCoordinates[i][0] == coordinates[0] && curShipPartsCoordinates[i][1] == coordinates[1]) return true;
            }          
            for(int i=0; i < playerShips.Count; i++) 
            {
                if(playerShips[i].compareCoordinates(coordinates)) return true;
            }

            return false;
        }
        bool CheckIfCharactersAreCorrect(char[] characters)
        {
            char[] correctCharacters = { 'A', 'B', 'C', 'D', 'F', 'G', 'H', 'I', 'J' };

            if (characters[1] < '0' || characters[1] > '9') return false;

            for(int i=0; i < correctCharacters.Length; i++) 
            {
                if (correctCharacters[i] == characters[0]) return true;
            }

            return false;
        }
        void showPlayerBoard(bool isFirstPlayerTurn)
        {
            Console.Clear();
            int playerNumber = isFirstPlayerTurn ? 1 : 2;
            Console.WriteLine($"\n                 | Player {playerNumber} Turn |\n");
            Console.WriteLine("\n                | Place your ships |");
            Console.WriteLine("\n       0   1   2   3   4   5   6   7   8   9");
            for (int i = 1; i <= 21; i++)
            {
                string row = "";
                
                row += i % 2 != 0 ? "     +" : $"   {Program.convertToRowCharacter(i / 2)} |";

                for (int k = 0; k < 10; k++)
                {
                    string rowElement;
                    if (i % 2 != 0)
                    {
                        rowElement = "---+";
                    }                    
                    else rowElement = $" {getPlayerBoard((i / 2) - 1, k, true)} |";
                    row += rowElement;
                }

                row += "\t";
                

                Console.WriteLine(row);
            }
        }
    }
}
