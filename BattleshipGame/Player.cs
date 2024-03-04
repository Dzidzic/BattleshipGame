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
        Computer computerMoves = new Computer();
        Gameboard playerBoard = new Gameboard();
        List<Ship> playerShips = new List<Ship>();
        public List<string> enemyPlayerMoves = new List<string>();

        public Player(bool isFirstPlayerTurn, bool isThisComputer)
        {
            CreateShips(isFirstPlayerTurn, isThisComputer);   
        }
        public string GetPlayerBoard(int a, int b, bool isItMyTurn)
        {
            return Char.ToString(playerBoard.ShowGameboardField(a, b, isItMyTurn));
        }
        public void ShowPlayerBoard(bool isFirstPlayerTurn, bool isThisShipPlacing, int shipLenght)
        {
            int playerNumber = isFirstPlayerTurn ? 1 : 2;
            if(isThisShipPlacing) 
            {
                Console.Clear();
                Console.WriteLine($"\n                 | Player {playerNumber} Turn |\n");
                Console.WriteLine("\n                | Place your ships |");
                Console.WriteLine($"\n Now placing: {shipLenght} mast ship");
            }         
            Console.WriteLine("\n       0   1   2   3   4   5   6   7   8   9");
            for (int i = 1; i <= 21; i++)
            {
                string row = "";

                if (i == 1 || i == 21) row += "     +";
                else row += i % 2 != 0 ? "     |" : $"   {Program.ConvertToRowCharacter(i / 2)} |";

                for (int k = 0; k < 10; k++)
                {
                    string rowElement;
                    if (i % 2 != 0)
                    {
                        if (i == 1 || i == 21) rowElement = "---";
                        else rowElement = "   ";
                    }
                    else rowElement = $" {GetPlayerBoard((i / 2) - 1, k, true)} ";
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


                Console.WriteLine(row);
            }
        }
        void CreateShips(bool isFirstPlayerTurn, bool isThisComputer)
        {
            for (int i = 1; i <= 10; i++)
            {                           
                int n;

                if (i > 6) { n = 1; }
                else if (i > 3) { n = 2; }
                else if (i > 1) { n = 3; }
                else n = 4;

                List<int[]> curShipPartsCoordinates = new List<int[]>();

                playerShips.Add(new Ship(n));

                playerBoard.SetAllPlayingFields(playerShips);

                if (isThisComputer)
                {
                    bool isShipPlaced = false;
                    int h = 0;
                    do
                    {
                        List<int[]> tempShip = computerMoves.CreateShip(n);
                        for (int j = 0; j < n; j++)
                        {
                            curShipPartsCoordinates.Add(CheckCorrectnessOfShipConstruction(curShipPartsCoordinates, tempShip[j], true));
                            if (curShipPartsCoordinates[j][0] == -1) break;
                            playerShips[i - 1].SetShipPartCoordinates(curShipPartsCoordinates[j]);
                            playerBoard.SetAllPlayingFields(playerShips);
                            if (j == n - 1) isShipPlaced = true;
                        }
                        if (isShipPlaced) break;
                    } while (h++ < 15);

                    if (!isShipPlaced)
                    {
                        playerShips.Clear();
                        i = 0;
                    }
                }
                else
                {
                    int[] placeHolder = new int[2];
                    for (int j = 0; j < n; j++)
                    {
                        ShowPlayerBoard(isFirstPlayerTurn, true, n);
                        curShipPartsCoordinates.Add(CheckCorrectnessOfShipConstruction(curShipPartsCoordinates, placeHolder, false));
                        playerShips[i - 1].SetShipPartCoordinates(curShipPartsCoordinates[j]);
                        playerBoard.SetAllPlayingFields(playerShips);
                    }
                }
            }
        }
        public bool CheckIfShipsAreAlive()
        {
            if(playerShips.Count == 0) return true;
            else return false;
        }
        public bool EnemyPlayerAttack(bool isCopmuterAttacking, string computerDifficulty)
        {   
            bool isThisHit;

            int[] coordinates = CheckCorrectnessOfEnemyAttack(isCopmuterAttacking, computerDifficulty);          

            isThisHit = playerBoard.SetPlayingField(coordinates, playerShips, true);

            if (isThisHit)
            {
                computerMoves.curUnderFireShipParts.Add(coordinates);
                
                for (int i = 0; i < playerShips.Count; i++)
                {
                    playerShips[i].DeleteShipPart(coordinates);
                    if (playerShips[i].DeleteShip())
                    {
                        computerMoves.curUnderFireShipParts.Clear();
                        for (int j = 0; j < 4; j++)
                        {
                            computerMoves.usedDirectionsFor4Choices[j] = false;
                        }
                        for (int j = 0; j < 2; j++)
                        {
                            computerMoves.usedDirectionsFor2Choices[j] = false;
                        }
          

                        for (int j = 0; j < playerShips[i].parts.Count; j++)
                        {
                            List<int[]> fieldsAroundShipPart = playerShips[i].GetFieldsAroundShipPart(j);
                            for (int k = 0; k < fieldsAroundShipPart.Count; k++)
                            {
                                playerBoard.SetPlayingField(fieldsAroundShipPart[k], playerShips, true);
                            }
                        }
                        playerShips.RemoveAt(i);
                    }
                }
            }

            char chCoordinates = Convert.ToChar((coordinates[0] + 65));
            string resultOfEnemyMove = $"{Convert.ToString(chCoordinates)}{Convert.ToString(coordinates[1])} | ";
            resultOfEnemyMove += isThisHit ? "HIT" : "MISS";
            enemyPlayerMoves.Add(resultOfEnemyMove);

            return !isThisHit;
        }
        int[] TakeCoordinatesFromUser()
        {
            int[] coordinates = new int[2];
            char[] characters;

            bool endSecondLoop = false;
            do
            {
                bool endFirstLoop = false;
                do
                {
                    Console.Write("Provide the coordinates of the ship part: ");
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

            return coordinates;
        }
        int[] CheckCorrectnessOfEnemyAttack(bool isCopmuterAttacking, string computerDifficulty)
        {
            int[] coordinates = new int[2];

            bool endFirstLoop = false;
            do
            {
                coordinates = isCopmuterAttacking ? computerMoves.MakeMove(computerDifficulty) : TakeCoordinatesFromUser();

                if (playerBoard.CheckIfPlayingFieldIsOccupied(coordinates))
                {
                    Console.WriteLine("Error! You can't hit the same place twice! \n");
                }
                else endFirstLoop = true;

            } while (!endFirstLoop);

            return coordinates;
        }
        int[] CheckCorrectnessOfShipConstruction(List<int[]> curShipPartsCoordinates, int[] tempShipPartCoordinates, bool isThisComputer)
        {
            int[] coordinates;

            bool endThirdLoop = false;
            do
            {
                bool endSecondLoop = false;
                do
                {
                    bool endFirstLoop = false;
                    do
                    {
                        coordinates = isThisComputer ? tempShipPartCoordinates : TakeCoordinatesFromUser();

                        if (CheckIfFieldIsOccupied(coordinates))
                        {
                            if(isThisComputer)
                            {
                                coordinates[0] = -1;
                                return coordinates;
                            }
                            Console.WriteLine("Error! The given place is already occupied! \n");
                        }
                        else endFirstLoop = true;

                    } while (!endFirstLoop);
                
                    if(!CheckIfShipConnectionIsCorrect(coordinates, curShipPartsCoordinates))
                    {
                        if (isThisComputer)
                        {
                            coordinates[0] = -1;
                            return coordinates;
                        }
                        Console.WriteLine("Error! The given parts of the ship do not touch each other! \n");
                    }
                    else endSecondLoop = true;
                
                } while (!endSecondLoop);

                if (CheckIfShipsAreTouching(coordinates, curShipPartsCoordinates))
                {
                    if (isThisComputer)
                    {
                        coordinates[0] = -1;
                        return coordinates;
                    }
                    Console.WriteLine("Error! There is already a placed ship around the given coordinates! \n");
                }
                else endThirdLoop = true;
            
            } while (!endThirdLoop);


            return coordinates;
        }
        bool CheckIfShipConnectionIsCorrect(int[] coordinates, List<int[]> curShipPartsCoordinates)
        {
            if (curShipPartsCoordinates.Count == 1)
            {
                if (coordinates[0] != curShipPartsCoordinates[0][0] && coordinates[1] != curShipPartsCoordinates[0][1]) return false;

                for(int i = -1; i < 2; i+=2)
                {
                    if (coordinates[0] + i == curShipPartsCoordinates[0][0]) return true;
                    if (coordinates[1] + i == curShipPartsCoordinates[0][1]) return true;
                }

                return false;
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
                if(playerShips[i].CompareCoordinates(coordinates)) return true;
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
