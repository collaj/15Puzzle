using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _15Puzzle
{
    static class Game
    {

        public static int LENGTH = 4;
        private static State currentState = goalState();
        private static State goal = goalState();
        private static MainForm mainForm = null;
        public static bool gameOn { get; set; }


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            mainForm = new MainForm();
            mainForm.buildGame();
            gameOn = false;
            Application.Run(mainForm);
            
        }

        private static State goalState()
        {
            int[,] goal = new int[LENGTH,LENGTH];
            int count = 1;
            int max = LENGTH * LENGTH;
            for (int i = 0; i < LENGTH; i++)
            {
                for (int j = 0; j < LENGTH; j++)
                {
                    goal[i, j] = count % max;
                    count++;
                }
            }
            return new State(goal);
        }

        public static void newGame()
        {
            Random rand = new Random();
            bool solvable;
            int[,] board = new int[LENGTH, LENGTH];

            do
            {
                for (int i = 0; i < 5; i++)
                {
                    ArrayList usedTiles = new ArrayList();
                    for (int j = 0; j < Game.LENGTH * Game.LENGTH; j++)
                    {
                        int random;
                        do
                        {
                            random = rand.Next(Game.LENGTH * Game.LENGTH);
                        } while (usedTiles.Contains(random));
                        usedTiles.Add(random);
                        board[j % Game.LENGTH, j / Game.LENGTH] = random; // this will build columns first, not rows....cause why not
                    }
                }

                solvable = isSolvable(board);
            } while (!solvable);

            currentState = new State(board);
            gameOn = true;
        }

        private static bool isSolvable(int[,] state)
        {
            int[] boardState = toIntArray(state);
            int allInversions = inversions(boardState);
            int blankRow = findBlankRow(boardState);
            if (Game.LENGTH % 2 == 1)
            {
                return allInversions % 2 == 0;
            }
            else
            {
                return (allInversions + Game.LENGTH - blankRow + 1) % 2 == 0;
            }
        }

        private static int inversions(int[] boardState)
        {
            int inversions = 0;
            for (int i = 0; i < boardState.Length - 1; i++)
            {
                if (boardState[i] == 0)
                    continue;

                int value = boardState[i];
                for (int j = i; j < boardState.Length; j++)
                {
                    if (boardState[j] != 0 && value > boardState[j])
                    {
                        inversions++;
                    }
                }
            }

                return inversions;
        }


        private static int findBlankRow(int[] boardState)
        {
            for (int i = 0; i < boardState.Length; i++)
            {
                if (boardState[i] == 0)
                    return i / Game.LENGTH;
            }
            return -1;
        }

        public static int[] findBlankLocation(int[,] doubleArray) // assumes width and height is the same
        {
            int[] loc = new int[2];
            int size = doubleArray.GetLength(0);
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (doubleArray[i, j] == 0)
                    {
                        loc[0] = i;
                        loc[1] = j;
                        return loc;
                    }
                }
            }

            return null;
        }

        public static int getValueAtPosition(int row, int column)
        {
            return currentState.getState()[row, column];
        }


        private static int[] toIntArray(int[,] doubleArray) // assumes width and height is the same
        {
            int size = doubleArray.GetLength(0);
            int[] array = new int[size * size];
            int count = 0;

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    array[count] = doubleArray[i, j];
                    count++;
                }
            }
            return array;
        }


        public static bool isGoalState()
        {
            return goal.Equals(currentState);
        }

        public static State getCurrentState()
        {
            return currentState;
        }

        public static void setCurrentState(State state)
        {
            currentState = state;
        }

        public static State getGoalState()
        {
            return goal;
        }

    }
}
