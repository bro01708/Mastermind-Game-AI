using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mastermind
{
    //abstract codebreaker to allow polymorphism
    class CodeBreaker
    {
        //all codebreakers will have to match the
        //respond method in which they generate the next guess
        public virtual void Respond()
        {

        }
        
    }
}
