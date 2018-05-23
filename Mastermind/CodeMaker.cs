using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mastermind
{
    public class CodeMaker
    {
        //reference board and random
        Random r = new Random();
        Board b = Board.Instance;
        private ConsoleColor[] code = new ConsoleColor[4];
        private CodeMaker() { createCode(); }

        // make singleston
        private static CodeMaker instance;
        public static CodeMaker Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CodeMaker();
                }
                return instance;
             }
        }

        //returns the secret code
        public ConsoleColor[] getCode
        {
            get
            {
                return Code;
            }
        }
        //returns Code
        public ConsoleColor[] Code { get => code; set => code = value; }

        //randomly assigns 4 colors to be the code based on randomly indexing the possible colors list
        public void createCode()
        {
            for (int i = 0; i < Code.Length; i++)
            {
               int index = r.Next(1, 7);
               Code[i] = b.getPossColors[index-1];
                
            }
        }

        //reads in the data from last guess the board , generates and returns a response of pegs
        public List<ConsoleColor> GetResponses()
        {

            int rowIndex = b.rowIndex();
            List<ConsoleColor> tempCode = new List<ConsoleColor> { Code[0], Code[1], Code[2], Code[3] };
            List<ConsoleColor> tempGuess = new List<ConsoleColor> { b.getPlayBoard[rowIndex, 0], b.getPlayBoard[rowIndex, 1], b.getPlayBoard[rowIndex, 2], b.getPlayBoard[rowIndex, 3] };
            List<ConsoleColor> greens = new List<ConsoleColor>();
            List<ConsoleColor> whites = new List<ConsoleColor>();

            //check for greens(matching position)
            for (int i = 0; i < tempCode.Count; i++)
            {

                if (tempCode[i] == tempGuess[i])
                {
                    greens.Add(ConsoleColor.Green);
                    tempGuess.RemoveAt(i);
                    tempCode.RemoveAt(i);
                    i--;
                }
            }
            //check for whites(correct color wrong pos)
            for (int i = 0; i < tempCode.Count; i++)
            {
                
                 if (tempCode.Exists(element => element == tempGuess[i]))
                {
                    whites.Add(ConsoleColor.White);
                    int index = tempCode.FindIndex(e => e == tempGuess[i]);
                    tempCode.RemoveAt(index);
                    tempGuess.RemoveAt(i);
                    i--;
                }

            }
            List<ConsoleColor> responseList = new List<ConsoleColor>();
            //reorganise so greens appear to the left.
            responseList.AddRange(greens);
            responseList.AddRange(whites);
            return responseList;
        }
        //method called to respond the guess by the codebreaker
        //sets the values on the board corresponding to its responselist
        public void Respond()
        {
            List<ConsoleColor> responseList = GetResponses();
            int greencount=0;
            
            for (int i = 0; i < responseList.Count(); i++)
            {
                b.getResponseBoard[b.rowIndex(), i] = responseList[i];
            }

            foreach (ConsoleColor c in responseList)
            {
                if (c == ConsoleColor.Green)
                {
                    greencount++;
                }
            }
            //acknowledge if code is correct and notify board.
            if (greencount ==4)
            {
                b.isGameWon = true;
            }
        }

    }
}
