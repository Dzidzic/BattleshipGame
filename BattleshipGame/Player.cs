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
        Gameboard playerBoard;
        List<Ship> playerShips = new List<Ship>();

        public Player()
        {
            createShips();
            playerBoard = new Gameboard(playerShips);
        }

        public string showPlayerBoard(int a, int b, bool isItMyTurn)
        {
            return Char.ToString(playerBoard.showGameboardField(a, b, isItMyTurn));
        }

        void createShips()
        {
            for (int i = 1; i <= 1; i++)
            {
                int n;

                if (i > 6) { n = 1; }
                else if (i > 3) { n = 2; }
                else if (i > 1) { n = 3; }
                else n = 4;

                List<int[]> curShipPartsCoordinates = new List<int[]>();

                for (int j = 0; j < n; j++)
                {
                    curShipPartsCoordinates.Add(takeCoordinatesFromUser(curShipPartsCoordinates));
                }

                playerShips.Add(new Ship(curShipPartsCoordinates));
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
    }
}
