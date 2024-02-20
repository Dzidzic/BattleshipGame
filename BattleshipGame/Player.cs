using BattleshipGame;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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

                List<int[]> shipPartsCoordinates = new List<int[]>();

                for (int j = 0; j < n; j++)
                {
                    shipPartsCoordinates.Add(takeCoordinatesFromUser());
                }

                playerShips.Add(new Ship(shipPartsCoordinates));
            }
        }
        int[] takeCoordinatesFromUser()
        {
            int[] test = new int[2];
            
            Console.Write("Podaj a: ");
            test[0] = int.Parse(Console.ReadLine()) - 1;

            Console.Write("Podaj b: ");
            test[1] = int.Parse(Console.ReadLine()) - 1;

            return test;
        }
    }
}
