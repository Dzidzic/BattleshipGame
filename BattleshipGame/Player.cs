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
            char[] characters;
            
            bool endFirstLoop = false;
            do
            {
                bool endSecondLoop = false;
                do
                {
                    Console.Write("Podaj koordynaty części statku: ");
                    characters = Console.ReadLine().ToCharArray();
                    
                    if(characters.Length != 2)
                    {
                        Console.WriteLine("Error! Koordynaty muszą składać się z dwóch znaków!\n");
                    }else endSecondLoop = true;

                } while(!endSecondLoop);

                if(!CheckIfCharactersAreCorrect(characters))
                {
                    Console.WriteLine("Error! Podano złe koordynaty!");
                }else endFirstLoop = true;

                

            } while(!endFirstLoop);

            test[0] = characters[0] - 65;
            test[1] = characters[1] - 48;

            Console.WriteLine($"Literka: {test[0]} | Liczba: {test[1]}");

            return test;
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
