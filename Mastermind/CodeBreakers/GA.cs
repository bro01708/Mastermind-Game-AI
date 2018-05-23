using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mastermind.CodeBreakers
{
    //inherit from codebreaker
    class GA : CodeBreaker
    {
        //set possible colors to choose from
        char[] possColors = new char[6] { 'R', 'G', 'B', 'C', 'Y', 'M' };
        //list use to map between index to console color
        List<ConsoleColor> translationList = new List<ConsoleColor> { ConsoleColor.Red, ConsoleColor.Green, ConsoleColor.Blue, ConsoleColor.Cyan, ConsoleColor.Yellow, ConsoleColor.Magenta };
        //initial population
        Combination[] population = new Combination[150];
        //reference the board
        Board b = Board.Instance;
        //track the previous guess that has been submitted
        Combination prevCombo;
        //keep track of the outcome of the previous guess
        Outcome prevOutcome;
        //set epoch limit
        int maxGen = 100;
        //set max population size
        int popSize = 100;
        //modifier used to scale up the weight of the fitness of a combo to increase selection chance for parenting
        int selectionModifier = 10;
        Random r = new Random();
        //setE is the set of codes at the end of each epoch
        List<Combination> setE = new List<Combination>();
        //track which guesses have been made to avoid duplicates
        List<Combination> playedGuesses = new List<Combination>();
        //the best combo so far to avoid straying from effective guesses
        Combination eliteCombo = new Combination();
        //fitess override of the elite
        int eliteScore = 0;



        public GA()
        {
            InitPop();
        }
        //fill the population with completely random codes
        public void InitPop()
        {
            Random r = new Random();
            Combination tempCombo;
            while (setE.Count <= popSize)
            {
                tempCombo = new Combination(r.Next(0, 6), r.Next(0, 6), r.Next(0, 6), r.Next(0, 6), 0);
                //check to avoid duplicates
                if (!setE.Exists(element => element == tempCombo))
                {
                    setE.Add(tempCombo);
                }
            }
        }

        //method called to actually create next guess
        public override void Respond()
        {
            Console.WriteLine("\n                                 AI is processing...\n");
            //if 1st guess then play default guess
            if (b.getTurn == 1)
            {
                string initGuess = string.Concat(possColors[0], possColors[0], possColors[1], possColors[2]);
                b.addGuess(initGuess);
                prevCombo = new Combination(0, 0, 1, 2, 0);
            }
            //else use GA
            else
            {
                //read and set the previous response
                AssignPreviousResponse();
                //check to see if the last guess was an elite, if so set its score and fitness
                if (prevOutcome.greens + prevOutcome.whites >= eliteScore)
                {
                    eliteCombo = prevCombo;
                    eliteCombo.fitness = 3;
                    eliteScore = prevOutcome.greens + prevOutcome.whites;
                }
                //Following berghmans algorithm
                int i = 1;
                int h = 1;
                while (h <= maxGen && i <= popSize)
                {
                    List<Combination> newPop = new List<Combination>();
                    newPop.Add(eliteCombo);


                    for (int j = 0; j < popSize;)
                    {
                        Combination temp = Crossover();
                        temp = Mutate(temp);
                        temp = Permutate(temp);
                        temp.fitness = GetFitness(temp);
                        if(!newPop.Exists(element => element == temp))
                        {
                            newPop.Add(temp);
                            i++;
                            j++;
                        }
                        
                    }
                    setE = newPop;
                    h++;
                }
                //sort by fitness order
                SortPopulation();
                //find fittest guess
                Combination finalCombo = BestFitness();
                playedGuesses.Add(finalCombo);
                //play guess
                string finalguess = string.Concat(possColors[finalCombo.a], possColors[finalCombo.b], possColors[finalCombo.c], possColors[finalCombo.d]);
                b.addGuess(finalguess);
                prevCombo = finalCombo;
            }

        }
        /// <summary>
        /// sort by fitness order
        /// </summary>
        private void SortPopulation()
        {
            List<Combination> temp = setE
            .OrderByDescending(x => (int)(x.fitness))
            .ToList();
            setE = temp;
        }

        /// <summary>
        /// returns the guess nearest the top of the list (fittest) that isnt a duplicate guess
        /// if not possible just returns the top.
        /// </summary>
        /// <returns></returns>
        private Combination BestFitness()
        {
            int i = 0;
            while (i < setE.Count)
            {
                if (!playedGuesses.Exists(element => element == setE[i]))
                {
                    return setE[i];
                }
                else { i++; }
            }
            return setE[0];


        }
        /// <summary>
        /// permutation swaps two pegs around in a combination
        /// </summary>
        /// <param name="_a"></param>
        /// <returns></returns>
        private Combination Permutate(Combination _a)
        {
            Combination newCombo = _a;

            if (r.Next(0, 99) <= 10)
            {
                int indexer = r.Next(0, 3);
                int indexer2 = r.Next(0, 3);
                while (indexer == indexer2)
                {
                    indexer2 = r.Next(0, 3);
                }
                newCombo[indexer] = _a[indexer2];
                newCombo[indexer2] = _a[indexer];
            }
            return newCombo;
        }

        /// <summary>
        /// reassigns a peg a new value in the combination
        /// </summary>
        /// <param name="_a"></param>
        /// <returns></returns>
        private Combination Mutate(Combination _a)
        {
            Combination newCombo = _a;
            if (r.Next(0, 99) <= 80)
            {
                int indexer = r.Next(0, 3);
                _a[indexer] = MutateInt(_a[indexer]);
            }
            return newCombo;
        }

        /// <summary>
        /// Sub method to randomly return another color
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        private int MutateInt(int color)
        {
            int newColor;
            newColor = color;

            while (newColor == color)
            {
                newColor = r.Next(0, 5);
            }
            return newColor;

        }

        /// <summary>
        /// uses fitness values to return a parent for crossover
        /// the higher the fitness the more likely it is to be selected
        /// </summary>
        /// <returns></returns>
        private Combination ParentSelection()
        {
            int roll = r.Next(0,100);
            for (int i = 0; i < setE.Count; i++)
            {
                if (roll <= setE[i].fitness*selectionModifier)
                {
                    return setE[i];
                }
            }
             return setE[r.Next(0,popSize)];
        }
        /// <summary>
        /// determines which type of crossover will be used
        /// </summary>
        /// <returns></returns>
        private Combination Crossover()
        {
            Combination a = ParentSelection();
            Combination b = ParentSelection();
            Combination tempCombo;

            if (r.Next(0, 99) <= 49)
            {
                tempCombo = SinglePointCrossover(a, b);
            }
            else
            {
                tempCombo = TwoPointCrossover(a, b);
            }
            return tempCombo;
        }

        /// <summary>
        /// splits and recombines parts from the other combination
        /// </summary>
        /// <param name="_a"></param>
        /// <param name="_b"></param>
        /// <returns></returns>
        private Combination SinglePointCrossover(Combination _a, Combination _b)
        {
            int[] a = new int[] { _a.a, _a.b, _a.c, _a.d };
            int[] b = new int[] { _b.a, _b.b, _b.c, _b.d };
            int[] c = new int[4];
            int crossoverPoint = r.Next(2, 3);

            for (int i = 0; i < 4; i++)
            {
                if (i < crossoverPoint)
                {
                    c[i] = a[i];
                }
            }
            return new Combination(c[0], c[1], c[2], c[3], 0);
        }
        /// <summary>
        /// splices the middle out of one combination replacing it with values from the other.
        /// </summary>
        /// <param name="_a"></param>
        /// <param name="_b"></param>
        /// <returns></returns>
        private Combination TwoPointCrossover(Combination _a, Combination _b)
        {
            int crossoverPoint1 = r.Next(2, 3);
            int crossoverPoint2 = r.Next(crossoverPoint1, 4);
            int[] a = new int[] { _a.a, _a.b, _a.c, _a.d };
            int[] b = new int[] { _b.a, _b.b, _b.c, _b.d };
            int[] c = new int[4];

            for (int i = 0; i < 4; i++)
            {
                if (i < crossoverPoint1 || i >= crossoverPoint2)
                {
                    c[i] = a[i];
                }
                else
                {
                    c[i] = b[i];
                }
            }
            return new Combination(c[0], c[1], c[2], c[3], 0);
        }

        /// <summary>
        /// returns the fitness value of a combination based on the response it would get if the previous guess was the answer and it was a guess
        /// </summary>
        /// <param name="_guess"></param>
        /// <returns></returns>
        private int GetFitness(Combination _guess)
        {
            int rowIndex = b.rowIndex();
            List<int> tempGuess = new List<int> { _guess.a, _guess.b, _guess.c, _guess.d };
            List<int> tempCode = new List<int> { prevCombo.a, prevCombo.b, prevCombo.c, prevCombo.d };
            List<ConsoleColor> greens = new List<ConsoleColor>();
            List<ConsoleColor> whites = new List<ConsoleColor>();

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
            return whites.Count + greens.Count;
        }
        /// <summary>
        /// takes the number of green and white pegs and creates an outcome from it.
        /// </summary>
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
    }
}
