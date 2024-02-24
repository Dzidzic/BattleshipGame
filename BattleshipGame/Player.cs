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
        void createShips(bool isFirstPlayerTurn)
        {
            for (int i = 1; i <= 2; i++)
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

            bool endFifthLoop = false;
            do
            {
                bool endFourthLoop = false;
                do
                {
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

                        if (CheckIfFieldIsOccupied(coordinates))
                        {
                            Console.WriteLine("Error! The given place is already occupied! \n");
                        }
                        else endThirdLoop = true;

                    } while (!endThirdLoop);
                
                    if(!CkeckIfShipConnectionIsCorrect(coordinates, curShipPartsCoordinates))
                    {
                        Console.WriteLine("Error! The given parts of the ship do not touch each other! \n");
                    }
                    else endFourthLoop = true;
                
                } while (!endFourthLoop);

                if (CheckIfShipsAreTouching(coordinates, curShipPartsCoordinates))
                {
                    Console.WriteLine("Error! There is already a placed ship around the given coordinates! \n");
                }
                else endFifthLoop = true;
            
            } while (!endFifthLoop);


            return coordinates;
        }
        bool CkeckIfShipConnectionIsCorrect(int[] coordinates, List<int[]> curShipPartsCoordinates)
        {
            if (curShipPartsCoordinates.Count == 1)
            {
                if (coordinates[0] != curShipPartsCoordinates[0][0] && coordinates[1] != curShipPartsCoordinates[0][1]) return false;
            }
            else if (curShipPartsCoordinates.Count >= 2)
            {
                int shipFacing = 0;
                if (curShipPartsCoordinates[0][0] - curShipPartsCoordinates[1][0] == -1 || curShipPartsCoordinates[0][0] - curShipPartsCoordinates[1][0] == 1) shipFacing = 1;

                if (coordinates[shipFacing] - curShipPartsCoordinates[0][shipFacing] != 0) return false;

                shipFacing = shipFacing == 1 ? 0 : 1;

                for(int i = -1; i < 2; i+=2)
                {
                    for(int j = 0; j < curShipPartsCoordinates.Count; j++)
                    {
                        if (coordinates[shipFacing] + i == curShipPartsCoordinates[j][shipFacing]) return true;
                    }
                }
                return false;
            }

            return true;

        }
        bool CheckIfShipsAreTouching(int[] coordinates, List<int[]> curShipPartsCoordinates)
        {
            for(int i = -1 ; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {                 
                    if (coordinates[0]+j >= 0 && coordinates[0]+j <= 9 && coordinates[1]+i >= 0 && coordinates[1]+i <= 9)
                    {
                        int[] tempCoordinates = { coordinates[0] + j, coordinates[1] + i };

                        if (curShipPartsCoordinates.Count == 0)
                        {
                            if (CheckIfFieldIsOccupied(tempCoordinates)) return true;
                        }
                        else
                        {
                            for (int k = 0; k < curShipPartsCoordinates.Count; k++)
                            {
                                if (curShipPartsCoordinates[k][0] != tempCoordinates[0] && curShipPartsCoordinates[k][1] != tempCoordinates[1])
                                {
                                    if (CheckIfFieldIsOccupied(tempCoordinates)) return true;
                                }
                            }
                        }
                    }             
                }
            }
            return false;
        }
        bool CheckIfFieldIsOccupied(int[] coordinates)
        {         
            for(int i=0; i < playerShips.Count; i++) 
            {
                if(playerShips[i].compareCoordinates(coordinates)) return true;
            }

            return false;
        }
        bool CheckIfCharactersAreCorrect(char[] characters)
        {
            char[] correctCharacters = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J' };

            if (characters[1] < '0' || characters[1] > '9') return false;

            for(int i=0; i < correctCharacters.Length; i++) 
            {
                if (correctCharacters[i] == characters[0]) return true;
            }

            return false;
        }        
    }
}
