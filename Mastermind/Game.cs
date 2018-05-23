using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mastermind.CodeBreakers;

namespace Mastermind
{
    class Game
    {
        //add references to  necessary singletons
        Board b = Board.Instance;
        CodeMaker cm = CodeMaker.Instance;
        Renderer r = Renderer.Instance;
        //initialise abstract codebreaker
        CodeBreaker cb;

        //use menu to select which child codebreaker will be used.
        public Game(int _menuSelect)
        {
            switch (_menuSelect)
            {
                case 1:
                    cb = new Human();
                    break;
                case 2:
                    cb = new FGA();
                    break;
                case 3:
                    cb = new GA();
                    break;
                default:
                    break;
            }
            //setup board
            b.setup(13);
            //get codemaker to create its code
            cm.createCode();

        }

        //sets up game logic , tracks turns, winstate and triggers display refresh
        //also handles win/loss messages if turns expire or winstate changes.
        public void play()
        {
            b.isGameWon = false;
            bool won = false;

            r.Display(null);
            for (int i = 0; i < b.getRows; i++)
            {
                if (won == false)
                {
                    Console.Clear();
                    r.Display(null);
                    cb.Respond();
                    cm.Respond();
                    b.incrementTurn();
                    if (b.isGameWon == true)
                    {
                        won = true;
                    }
                }
                r.Display(null);
            }
            if (won == true)
            {
                r.WinScreen();
                Console.ReadKey();
            }
            if (won == false)
            {
                r.LoseScreen();
                Console.ReadKey();

            }
            won = false;
        }



    }

}

