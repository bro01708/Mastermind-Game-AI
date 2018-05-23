using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mastermind
{
    /// <summary>
    /// struct to handle a combination
    /// </summary>
    public struct Combination
    {
        public int a;
        public int b;
        public int c;
        public int d;

        public int fitness;

        public Combination(int _a, int _b, int _c, int _d, int _fitness)
        {
            a = _a;
            b = _b;
            c = _c;
            d = _d;
            fitness = _fitness;
        }

        //indexer added to allow for easier mutations
        public int this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return a;
                    case 1:
                        return b;
                    case 2:
                        return c;
                    case 3:
                        return d;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
            set
            {
                switch (index)
                {
                    case 0:
                         a = value;
                        break;
                    case 1:
                         b = value;
                        break;
                    case 2:
                         c = value;
                        break;
                    case 3:
                         d = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }



        //enable comparisons
        public static bool operator ==(Combination a, Combination b)
        {
            return a.a == b.a && a.b == b.b && a.c == b.c && a.d == b.d;
        }

        public static bool operator !=(Combination a, Combination b)
        {
            return !(a == b);
        }
    }
}
