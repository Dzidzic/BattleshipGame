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
        public bool SetPlayingField(int[] coordinates, List<Ship> ships, bool isThisAttack)
        {
            bool isItShipField = false;
            for (int i = 0; i < ships.Count; i++)
            {
                if (ships[i].CompareCoordinates(coordinates))
                {
                    isItShipField = true;
                    break;
                }
            }

            if (isItShipField)
            {
                if (isThisAttack)
                {
                    playingFields[coordinates[0], coordinates[1]] = '@';
                    return true;
                }
                else playingFields[coordinates[0], coordinates[1]] = 'O';
            }
            else if(isThisAttack) playingFields[coordinates[0], coordinates[1]] = 'X';
            else playingFields[coordinates[0], coordinates[1]] = '-';

            return false;
        }
        public bool CheckIfPlayingFieldIsOccupied(int[] coordinates)
        {
            if (playingFields[coordinates[0], coordinates[1]] == '@' || playingFields[coordinates[0], coordinates[1]] == 'X') return true;
            else return false;
        }
        public void SetAllPlayingFields(List<Ship> ships)
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    SetPlayingField(new int[] { i, j }, ships, false);
                }
            }
        }
        public char ShowGameboardField(int a, int b, bool isItMyTurn)
        {
            if (!isItMyTurn && playingFields[a, b] == 'O')
            {
                return '-';
            }

            return playingFields[a, b];
        }
    }
}
