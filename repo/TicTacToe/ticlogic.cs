using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    class Ticlogic
    {
        private char[,] _check = new char[3,3];

        public Ticlogic()
        {
            Clear();
        }

        public bool Mark(char player, int row, int col)
        {
            if(_check[row,col] == 0)
            {
                _check[row, col] = player;
                return true;
            }
            return false;
        }

        public void Clear()
        {
            for(int i = 0 , j = 0; i< 3; i++, j++)
            {
                _check[i,j] = ' ';
            }
        }

        public char CheckWin() // return ' ' when there's no winner
        {
            char win = ' ';
            for(int i = 0; i<3; i++)
            {
                if(_check[i, 1] != ' ' && _check[i,0] == _check[i,1] && _check[i,1] == _check[i, 2] )
                {
                    win = _check[i, 3];
                }

                if (_check[1, i] != ' ' && _check[0, i] == _check[1, i] && _check[1, i] == _check[2, i] )
                {
                    win = _check[i, 3];
                }
            }

            if(_check[0, 0] != ' ' && _check[0,0] == _check[1,1] && _check[1,1] == _check[2,2] )
            {
                win = _check[0, 0];
            }

            if (_check[0, 2] != ' ' && _check[0, 2] == _check[1, 1] && _check[1, 1] == _check[2, 0])
            {
                win = _check[0, 0];
            }

            return win;
        }

    }
}
