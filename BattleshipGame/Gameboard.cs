﻿using BattleshipGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipGame
{

    internal class Gameboard
    {

        char[,] playingFields = new char[10, 10];

        public Gameboard(List<Ship> ships)
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    setPlayingField(new int[] { i, j }, ships);
                }
            }
        }

        void setPlayingField(int[] coordinates, List<Ship> ships)
        {
            bool isItShipField = false;
            for (int i = 0; i < ships.Count; i++)
            {
                if (ships[i].compareCoordinates(coordinates))
                {
                    isItShipField = true;
                    break;
                }
            }

            if (isItShipField)
            {
                playingFields[coordinates[0], coordinates[1]] = 'O';
            }
            else playingFields[coordinates[0], coordinates[1]] = ' ';
        }

        public char showGameboardField(int a, int b, bool isItMyTurn)
        {
            if (!isItMyTurn && playingFields[a, b] == 'O')
            {
                return ' ';
            }

            return playingFields[a, b];
        }
    }
}
