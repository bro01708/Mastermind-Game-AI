using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mastermind
{
    class Program
    {
        /// <summary>
        /// Main handles the initialisation of the program and the game itself
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            //shapes the window appropriate for vertical board    
            Console.SetWindowSize(80, 50);
            //initialise necessary objects
            Board b = Board.Instance;
            CodeMaker cm = CodeMaker.Instance;
            Renderer r = Renderer.Instance;
            //sanitize user input for the menu
            while (true)
            {
                Console.Clear();
                r.LogoDraw();
                r.MenuDraw();
                Console.WriteLine("\n                                  Select an Option");
                int menuSelect = 0; 

                ConsoleKeyInfo UserInput = Console.ReadKey(); 

                if (char.IsDigit(UserInput.KeyChar))
                {
                    menuSelect = int.Parse(UserInput.KeyChar.ToString()); 
                }

                if (menuSelect >= 1 && menuSelect <=3)
                {
                    Game game = new Game(menuSelect);
                    game.play();
                }
                
            }
        }
    }
}
