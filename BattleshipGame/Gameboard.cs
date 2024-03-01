using BattleshipGame;
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

        public bool setPlayingField(int[] coordinates, List<Ship> ships, bool isThisAttack)
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
                if (isThisAttack) 
                {
                    playingFields[coordinates[0], coordinates[1]] = 'X';
                    return true;
                }
                else playingFields[coordinates[0], coordinates[1]] = '#';
            }
            else if(isThisAttack) playingFields[coordinates[0], coordinates[1]] = '*';
            else playingFields[coordinates[0], coordinates[1]] = ' ';

            return false;
        }
        public void setAllPlayingFields(List<Ship> ships)
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    setPlayingField(new int[] { i, j }, ships, false);
                }
            }
        }
        public char showGameboardField(int a, int b, bool isItMyTurn)
        {
            if (!isItMyTurn && playingFields[a, b] == '#')
            {
                return ' ';
            }

            return playingFields[a, b];
        }
    }
}
