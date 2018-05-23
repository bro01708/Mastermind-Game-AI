using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mastermind
{/// <summary>
/// renderer class handles display and drawing of the board, the pegs within the board and their respective colors
/// </summary>
    class Renderer
    {
        /// <summary>
        /// set as singleton , never going to need more than one
        /// </summary>
        private static Renderer instance;
        public static Renderer Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Renderer();
                }
                return instance;
            }
        }
        Board b = Board.Instance;
        CodeMaker cm = CodeMaker.Instance;


        /// <summary>
        /// draw the logo at the top of the board
        /// </summary>
        public void LogoDraw()
        {
            // Print out board header
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("                                  _                      _           _ ");
            Console.WriteLine("                                 | |                    (_)         | |");
            Console.WriteLine("              _ __ ___   __ _ ___| |_ ___ _ __ _ __ ___  _ _ __   __| |");
            Console.WriteLine("             | '_ ` _ \\ / _` / __| __/ _ \\ '__| '_ ` _ \\| | '_ \\ / _` |");
            Console.WriteLine("             | | | | | | (_| \\__ \\ ||  __/ |  | | | | | | | | | | (_| |");
            Console.WriteLine("             |_| |_| |_|\\__,_|___/\\__\\___|_|  |_| |_| |_|_|_| |_|\\__,_|");
            Console.ForegroundColor = ConsoleColor.White; //reset console text to white
            Console.WriteLine();
        }

        //displays board details and data
        public void Display(string message)
        {
            Console.Clear();
            LogoDraw();
            Console.WriteLine("                   =============================================");
            Console.WriteLine("                   |   #       #       #       #   |           |");
            Console.WriteLine("                   |===============================|===========|");


            //loop through and print each line with data+ formatting
            for (int i = 0; i < b.getRows; i++)
            {
                //Left hand side of the board
                Console.Write("                   |   "); // screen buffer
                printPeg(b.getPlayBoard[i, 0]);                   // first peg of row
                Console.Write("       ");                    // spacer                   
                printPeg(b.getPlayBoard[i, 1]);                   // 2nd peg of row
                Console.Write("       ");                    //spacer
                printPeg(b.getPlayBoard[i, 2]);
                Console.Write("       ");
                printPeg(b.getPlayBoard[i, 3]);
                Console.Write("   ");
                Console.ForegroundColor = ConsoleColor.White; //reset console text to white
                Console.Write("|");                         //print out the right hand border of code side and 1st spacer of response side



                //Right hand side of the board
                Console.Write("  ");
                printPeg(b.getResponseBoard[i, 0]);  //1st response peg of row
                Console.Write(" ");             //spacer
                printPeg(b.getResponseBoard[i, 1]);  //2nd response peg of row
                Console.Write(" ");             //spacer
                printPeg(b.getResponseBoard[i, 2]);
                Console.Write(" ");
                printPeg(b.getResponseBoard[i, 3]);
                Console.Write(" ");
                Console.ForegroundColor = ConsoleColor.White; //reset console text color
                Console.Write(" |");                          //right hand border
                Console.WriteLine();                          //carriage return
                Console.ForegroundColor = ConsoleColor.White; //reset console color for bottom divider
                Console.WriteLine("                   |-------------------------------|-----------|"); //bottom divider
            }
            Console.WriteLine("                   ============================================="); //board bottom
            Console.WriteLine();

            //Color Key
            Console.Write("              "); printPeg(ConsoleColor.Red); Console.ForegroundColor = ConsoleColor.White; Console.Write(" = R ");
            Console.Write("    "); printPeg(ConsoleColor.Green); Console.ForegroundColor = ConsoleColor.White; Console.Write(" = G ");
            Console.Write("    "); printPeg(ConsoleColor.Blue); Console.ForegroundColor = ConsoleColor.White; Console.Write(" = B ");
            Console.Write("    "); printPeg(ConsoleColor.Cyan); Console.ForegroundColor = ConsoleColor.White; Console.Write(" = C ");
            Console.Write("    "); printPeg(ConsoleColor.Yellow); Console.ForegroundColor = ConsoleColor.White; Console.Write(" = Y ");
            Console.Write("    "); printPeg(ConsoleColor.Magenta); Console.ForegroundColor = ConsoleColor.White; Console.WriteLine(" = M ");
            Console.Write(message);
        }

        //switches forground color to print the block char in the correct peg color
        public void printPeg(ConsoleColor c)
        {
            Console.ForegroundColor = c;
            Console.Write('■'.ToString());
        }
        //prints the game mode seletion menu
        public void MenuDraw()
        {
            Console.WriteLine("                                1 - Human CodeBreaker");
            Console.WriteLine("                       2 - AI Codebreaker (Five Guess Algorithm)");
            Console.WriteLine("                         3 - AI CodeBreaker (Genetic Algorithm)");
        }
        //displays the win message
        public void WinScreen()
        {
            Console.WriteLine("\n                                 Well Done , You Win!");
            Console.WriteLine("                          Press Any Key To Return To The Menu");
            Console.ReadKey();
        }
        //display the lose message
        public void LoseScreen()
        {
            Console.WriteLine("\n                                  Unlucky! You Lose");
            Console.WriteLine("                                The Winning Code was");
            Console.WriteLine("                             " + cm.Code[0].ToString() + " " + cm.Code[1].ToString() + " " + cm.Code[2].ToString() + " " + cm.Code[3].ToString());
            Console.WriteLine("                          Press Any Key To Return To The Menu");
            Console.ReadKey();

        }
    }
}
