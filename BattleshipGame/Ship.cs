using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace BattleshipGame
{
    internal class Ship
    {
        public List<int[]> parts = new List<int[]>();
        int aliveParts;
        public Ship(int partsQuantity)
        {
            this.aliveParts = partsQuantity;
        }
        public void SetShipPartCoordinates(int[] coordinates)
        {
            parts.Add(coordinates);
        }
        public bool CompareCoordinates(int[] coordinates)
        {
            for (int i = 0; i < parts.Count; i++)
            {
                if (parts[i][0] == coordinates[0] && parts[i][1] == coordinates[1])
                {
                    return true;
                }
            }

            return false;
        }
        public void DeleteShipPart(int[] coordinates)
        {
            for (int i = 0; i < parts.Count; i++)
            {
                if (parts[i][0] == coordinates[0] && parts[i][1] == coordinates[1])
                {
                    aliveParts--;
                }
            }
        }
        public bool DeleteShip() 
        {
            if (aliveParts == 0) return true;
            else return false;
        }
        public List<int[]> GetFieldsAroundShipPart(int i)
        {
            List<int[]> fieldsAroundShipPart = new List<int[]>();
            for(int j = -1; j <= 1; j++)
            {
                for(int k = -1; k <=1; k++)
                {
                    int[] tempPart = { parts[i][0] + j, parts[i][1] + k };
                    if (tempPart[0] >= 0 && tempPart[0] <= 9 && tempPart[1] >= 0 && tempPart[1] <= 9) fieldsAroundShipPart.Add(tempPart);                
                }
            }
            return fieldsAroundShipPart;
        }
    }
}
