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

        public Ship(List<int[]> shipPartsCoordinates)
        {
            for (int i = 0; i < shipPartsCoordinates.Count; i++)
            {
                setShipPartCoordinates(shipPartsCoordinates[i]);
            }
        }
        void setShipPartCoordinates(int[] coordinates)
        {
            parts.Add(coordinates);
        }
        public bool compareCoordinates(int[] coordinates)
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
    }
}
