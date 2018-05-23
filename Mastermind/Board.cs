using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mastermind
{
    /// <summary>
    /// main storage for all pegs, interected with by codemakers, breakers and renderer
    /// </summary>
    class Board
    {
        //initialise the 2D arrays for the guess(play) and response boards
        ConsoleColor[,] playBoard;
        ConsoleColor[,] responseBoard;
        //allocate the possible colors for use in thegame
        ConsoleColor[] possColors = new ConsoleColor[] { ConsoleColor.Red, ConsoleColor.Green, ConsoleColor.Blue, ConsoleColor.Cyan, ConsoleColor.Yellow, ConsoleColor.Magenta };
        //gamestate
        bool gameWon;
        //make singleton
        private static Board instance;
        int rows;
        int turn;
        private Board() { }
        public static Board Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Board();
                }
                return instance;
            }
        }

        //fill both arrays with black 'pegs' so they appear invisible
        public void setup(int _guesses)
        {
            rows = _guesses;
            turn = 1;

            playBoard = new ConsoleColor[_guesses, 4];
            responseBoard = new ConsoleColor[_guesses, 4];

            for (int i = 0; i < _guesses; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    playBoard[i, j] = ConsoleColor.Black;
                    responseBoard[i, j] = ConsoleColor.Black;

                }
            }
        }        
        //track which row is the current play row
        public int rowIndex()
        {
            return rows - (turn - 1) -1;
        }

        //returns playboard data
        public ConsoleColor[,] getPlayBoard
        {
            get { return playBoard; }
            set { playBoard = value; }
        }
        //returns response board data
        public ConsoleColor[,] getResponseBoard
        {
            get { return responseBoard; }
            set { responseBoard = value; }
        }
        //getters and setters
        public int getRows { get => rows; set => rows = value; }
        public bool isGameWon { get => gameWon; set => gameWon = value; }
        public int getTurn { get => turn; set => turn = value; }
        public ConsoleColor[] getPossColors { get => possColors; set => possColors = value; }

        //increments the turn counter
        public void incrementTurn()
        {
            turn++;
        }

        //takes in the string from user and interprets it to be console colors.
        public void addGuess(string code)
        {
            for (int i = 0; i < 4; i++)
            {
                switch (code[i])
                {
                    case 'R':
                    case 'r':
                        getPlayBoard[rows - turn, i] = ConsoleColor.Red;
                        break;

                    case 'G':
                    case 'g':
                        getPlayBoard[rows - turn, i] = ConsoleColor.Green;
                        break;

                    case 'B':
                    case 'b':
                        getPlayBoard[rows - turn, i] = ConsoleColor.Blue;
                        break;

                    case 'C':
                    case 'c':
                        getPlayBoard[rows - turn, i] = ConsoleColor.Cyan;
                        break;

                    case 'Y':
                    case 'y':
                        getPlayBoard[rows - turn, i] = ConsoleColor.Yellow;
                        break;

                    case 'M':
                    case 'm':
                        getPlayBoard[rows - turn, i] = ConsoleColor.Magenta;
                        break;
                }
            }
        }

    }
}
