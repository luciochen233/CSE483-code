using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    class Ticlogic
    {
        public char[,] _check = new char[3,3];
        bool _end_of_game = false;
        int _counter = 0;
        
        public Ticlogic()
        {
            Clear();
        }

        public bool Mark(char player, int row, int col)
        {
            if (_end_of_game) return false;
            if(_check[row,col] == ' ')
            {
                _counter++;
                _check[row, col] = player;
                return true;
            }
            return false;
        }

        public void Clear()
        {
            for(int i = 0; i< 3; i++)
            {
                for(int j = 0; j<3; j++)
                {
                    _check[i, j] = ' ';
                }
            }
            _end_of_game = false;
            _counter = 0;
        }

        public char CheckWin() // return ' ' when there's no winner
        {
            char which_row = (char)0;
            char win = ' ';
            for(int i = 0; i<3; i++)
            {
                if(_check[i, 1] != ' ' && _check[i,0] == _check[i,1] && _check[i,1] == _check[i, 2] )
                {
                    which_row = (char)i;
                    win = _check[i, 1];
                }

                if (_check[1, i] != ' ' && _check[0, i] == _check[1, i] && _check[1, i] == _check[2, i] )
                {
                    which_row = (char)(3 + i);
                    win = _check[1, i];
                }
            }

            if(_check[0, 0] != ' ' && _check[0,0] == _check[1,1] && _check[1,1] == _check[2,2] )
            {
                which_row = (char)6;
                win = _check[0, 0];
            }

            if (_check[0, 2] != ' ' && _check[0, 2] == _check[1, 1] && _check[1, 1] == _check[2, 0])
            {
                which_row = (char)7;
                win = _check[0, 2];
            }

            if((_counter == 9) && (win == ' '))
            {
                win = 'D';
            }

            if(win != ' ')
            {
                _end_of_game = true;
                win += which_row;
            }

            return win;
        }
    }
}
