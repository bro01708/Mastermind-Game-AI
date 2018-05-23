using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mastermind.CodeBreakers
{
    /// <summary>
    /// 'dumb' codebreaker just interprets user input
    /// </summary>
    class Human : CodeBreaker
    {
        Renderer r = Renderer.Instance;
        //list to compare input to
        string allowableChars = "RGBCYMrgbcym";
        //error message
        string message = null;
        //reference to board
        Board b = Board.Instance;
        // sanitizes input to only contain chars from the allowable chars, lenght must be 4
        public override void Respond()
        {
            string tempcode = null;
            bool valid = false;
            while (valid == false)
            {
                r.Display(message);
                message = null;
                Console.WriteLine("                                      ");
                Console.Write("                                ENTER YOUR CODE -\n                                      ");
                tempcode = Console.ReadLine();
                foreach (char c in tempcode)
                {
                    if (tempcode.Length == 4 && allowableChars.Contains(c.ToString()) && tempcode != null)
                    {
                        valid = true;
                        message = null;
                    }
                    else { message = "    Invalid input - Permitted code length = 4.Permitted chars = \"RrGgBbCcYyMm\""; valid = false; break; }
                }
                
            }
            //submit guess when it passes sanitation.
             b.addGuess(tempcode);
        }


        
    }
}
