using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mastermind
{
    class FGA : CodeBreaker
    {   
        /// <summary>
        /// five guess algorithm inherits from codebreaker
        /// </summary>
        /// set possible colors
        char[] possColors = new char[6] {'R','G','B','C','Y','M' };
        //list use to map between index to console color
        List<ConsoleColor> translationList = new List<ConsoleColor> { ConsoleColor.Red, ConsoleColor.Green, ConsoleColor.Blue, ConsoleColor.Cyan, ConsoleColor.Yellow, ConsoleColor.Magenta };
        //indexes of posscolors
        List<Combination> setS = new List<Combination>(); 
        //list of ellegible combinations
        List<Combination> possibleCombo = new List<Combination>();
        //reference board
        Board b = Board.Instance;
        //track previous guess
        Combination prevCombo;
        //track outcome of previous guess
        Outcome prevOutcome;
        //track possible outcomes
        Outcome[] possOutcomes = new Outcome[16];
        //track mincombo result of minmax
        Combination minCombo;


        /// <summary>
        /// initialize by generating all possible combinations
        /// </summary>
        public FGA()
        {
            GenerateSetS();
            possibleCombo = setS;
            int count = 0;
            
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    possOutcomes[count] = new Outcome(i, j);
                }
            }
        }

        /// <summary>
        /// method called to sumbit next guess
        /// </summary>
        public override void Respond()
        {

            Console.WriteLine("\n                                 AI is processing...\n");
            //if first guess play default guess
            if (b.getTurn == 1)
            {
                string initGuess = string.Concat(possColors[0], possColors[0], possColors[1], possColors[1]);
                b.addGuess(initGuess);
                prevCombo = new Combination(0, 0, 1, 1,0);
            }
            //else use knuths algorithm
            else
            {
                AssignPreviousResponse();
                FilterPossibleCombos();
                int min = int.MaxValue;
                foreach (Combination g in possibleCombo)
                {
                    int max = 0;
                    foreach (Outcome o in possOutcomes)
                    {
                        int count = 0;
                        foreach (Combination s in setS)
                        {
                            if (GetResponse(g, s) == o)
                                count++;
                        }
                        if (count > max)
                            max = count;
                    }
                    if(max<min)
                    {
                        min = max;
                        minCombo = g;
                    }
                    }

                //submit most likely guess
                string finalguess = string.Concat(possColors[minCombo.a], possColors[minCombo.b], possColors[minCombo.c], possColors[minCombo.d]);
                b.addGuess(finalguess);
                prevCombo = new Combination(minCombo.a, minCombo.b,minCombo.c,minCombo.d,0);

            }
        }
        /// <summary>
        ///compares two combinations as the codemaker would, returns an outcome
        /// correct position = green
        /// correct color wrong pos = white
        /// </summary>
        /// <param name="_guess"></param>
        /// <param name="_solution"></param>
        /// <returns></returns>
        private Outcome GetResponse(Combination _guess, Combination _solution)
        {
            int rowIndex = b.rowIndex();
            List<int> tempCode = new List<int>{_guess.a,_guess.b, _guess.c, _guess.d};
            List<int> tempGuess = new List<int> { _solution.a, _solution.b, _solution.c, _solution.d };
            List<ConsoleColor> greens = new List<ConsoleColor>();
            List<ConsoleColor> whites = new List<ConsoleColor>();
            //check for greens
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
            //check for whites
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
            responseList.AddRange(greens);
            responseList.AddRange(whites);
            return new Outcome(greens.Count,whites.Count);
        }
    
        //generate every possible combination and add it to setS
        private void GenerateSetS()
        {
            for (int a = 0; a < 6; a++)
            {
                for (int b = 0; b < 6; b++)
                {
                    for (int x = 0; x < 6; x++)
                    {
                        for (int y = 0; y < 6; y++)
                        {
                            setS.Add(new Combination(a,b,x,y,0));
                        }

                    }
                }
            }

        }


        //track previous response from codemaker
        private void AssignPreviousResponse()
        {
            int prevWhites = 0;
            int prevGreens = 0;

            for (int i = 0; i < 4; i++)
            {
                if (b.getResponseBoard[b.rowIndex() + 1, i] == ConsoleColor.Green)
                {
                    prevGreens++;
                }
                else if (b.getResponseBoard[b.rowIndex() + 1, i] == ConsoleColor.White)
                {
                    prevWhites++;
                }
            }
            prevOutcome = new Outcome(prevGreens, prevWhites);
        }

        /// <summary>
        /// for every combination if it doest achieve the same result
        /// when compared to the previous guess as the outcome, then it is ignored
        /// else add to the list of possibilities
        /// </summary>
        private void FilterPossibleCombos()
        {
            List<Combination> tempList = new List<Combination>();

            foreach (Combination c in possibleCombo)
            {
                if (GetResponse(c, prevCombo) == prevOutcome)
                {
                    tempList.Add(c);
                }
            }
            possibleCombo = tempList;
        }
    }
}
