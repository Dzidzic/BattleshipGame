using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipGame
{
    internal class Computer
    {
        Random rnd = new Random();
        string difficulty = "Easy";
        
        int shipDirection;
        public List<int[]> curUnderFireShipParts = new List<int[]>();  
        public bool[] usedDirectionsFor4Choices = { false, false, false, false };
        public bool[] usedDirectionsFor2Choices = { false, false };
        public int[] MakeMove()
        {            
            switch(difficulty)
            {
                case "Random":
                    return DrawCoordinates();

                case "Easy":
                    return EasyMove();

                default: 
                    return null;
            }
        }
        int [] DrawCoordinates()
        {
            int[] randomCoordinates = { rnd.Next(10), rnd.Next(10) };
            return randomCoordinates; 
        }
        int[] EasyMove()
        {
            int[] coordinates = new int[2];
            int curDirection;
            
            if (curUnderFireShipParts.Count == 1)
            {               
                do
                {
                    curDirection = rnd.Next(4);
                    if (!usedDirectionsFor4Choices[curDirection])
                    {
                        usedDirectionsFor4Choices[curDirection] = true;
                        if (curDirection == 0)
                        {
                            shipDirection = 0;
                            coordinates[0] = curUnderFireShipParts[0][0] - 1;
                            coordinates[1] = curUnderFireShipParts[0][1];
                        }
                        else if (curDirection == 1)
                        {
                            shipDirection = 0;
                            coordinates[0] = curUnderFireShipParts[0][0] + 1;
                            coordinates[1] = curUnderFireShipParts[0][1];
                        }
                        else if (curDirection == 2)
                        {
                            shipDirection = 1;
                            coordinates[0] = curUnderFireShipParts[0][0];
                            coordinates[1] = curUnderFireShipParts[0][1] - 1;
                        }
                        else
                        {
                            shipDirection = 1;
                            coordinates[0] = curUnderFireShipParts[0][0];
                            coordinates[1] = curUnderFireShipParts[0][1] + 1;
                        }
                    }
                    if (!(coordinates[0] < 0 || coordinates[1] < 0) && !(coordinates[0] > 9 || coordinates[1] > 9)) break;
                } while (true);
                return coordinates;
            }
            else if (curUnderFireShipParts.Count >= 2)
            {
                curUnderFireShipParts = sortList(curUnderFireShipParts, shipDirection);
                
                if (shipDirection == 0)
                {
                    do
                    {
                        curDirection = rnd.Next(0, 2);
                        if (!usedDirectionsFor2Choices[curDirection])
                        {

                            if (curDirection == 0)
                            {
                                coordinates[0] = curUnderFireShipParts[0][0] - 1;
                                coordinates[1] = curUnderFireShipParts[0][1];
                            }
                            else
                            {
                                coordinates[0] = curUnderFireShipParts[curUnderFireShipParts.Count - 1][0] + 1;
                                coordinates[1] = curUnderFireShipParts[curUnderFireShipParts.Count - 1][1];
                            }
                        }
                        if (!(coordinates[0] < 0 || coordinates[1] < 0) && !(coordinates[0] > 9 || coordinates[1] > 9)) break;
                    } while (true);
                }
                else
                {                   
                    do
                    {
                        curDirection = rnd.Next(2, 4);
                        if (!usedDirectionsFor2Choices[curDirection - 2])
                        {
                            if (curDirection == 2)
                            {
                                coordinates[0] = curUnderFireShipParts[0][0];
                                coordinates[1] = curUnderFireShipParts[0][1] - 1;
                            }
                            else
                            {
                                coordinates[0] = curUnderFireShipParts[curUnderFireShipParts.Count - 1][0];
                                coordinates[1] = curUnderFireShipParts[curUnderFireShipParts.Count - 1][1] + 1;
                            }
                        }
                        if (!(coordinates[0] < 0 || coordinates[1] < 0) && !(coordinates[0] > 9 || coordinates[1] > 9)) break;
                    } while (true);
                }
                
                return coordinates;
            }
            else return DrawCoordinates();         
        }
        public List<int[]> CreateShip(int lenght)
        {
            List<int[]> shipParts = new List<int[]>();
            
            shipDirection = rnd.Next(2);
            int shipPartsCoordinatesForDir = rnd.Next(11-lenght);
            int shipPositionInLine = rnd.Next(10);

            if(shipDirection == 0) 
            {
                for(int i = 0; i < lenght; i++)
                {
                    int[] tempCoordinates = { shipPositionInLine, shipPartsCoordinatesForDir + i };                   
                    shipParts.Add(tempCoordinates);
                }
            }
            else{
                for (int i = 0; i < lenght; i++)
                {
                    int[] tempCoordinates = { shipPartsCoordinatesForDir + i, shipPositionInLine };
                    shipParts.Add(tempCoordinates);
                }
            }

            return shipParts;
        }
        List<int[]> sortList(List<int[]> listToSort, int xORy)
        {
            List<int[]> list = listToSort;
            for (int i = 0; i < list.Count - 1; i++)
            {
                for (int j = 0; j < list.Count - 1; j++)
                {
                    if (list[j][xORy] > list[j + 1][xORy])
                    {
                        int temp = list[j][xORy];
                        list[j][xORy] = list[j + 1][xORy];
                        list[j + 1][xORy] = temp;
                    }
                }
            }
            return list;
        }
    }
}
