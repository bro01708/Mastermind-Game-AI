using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mastermind
{
    /// <summary>
    /// struct to contain details of white and green pegs of an outcome.
    /// </summary>
    public struct Outcome
    {
        public int whites;
        public int greens;

        public Outcome(int _greens, int _whites)
        {
            greens = _greens;
            whites = _whites;
        }
        //enable comparison
        public static bool operator ==(Outcome a, Outcome b)
        {
            return a.whites == b.whites && a.greens == b.greens;
        }
        //enable comparison
        public static bool operator !=(Outcome a, Outcome b)
        {
            return !(a == b);
        }
    }
}
