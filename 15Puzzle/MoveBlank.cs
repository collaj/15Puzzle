using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _15Puzzle
{
    public static class MoveBlank
    {

        private static readonly string[] moves = { "up", "left", "right", "down" };

        public static string[] getMoves() {
            return moves;
        }

        public static ArrayList getValidMoves(State currentState)
        {
            int[,] board = currentState.getState();
            ArrayList available = new ArrayList();

            bool found = false;
            for (int i = 0; i < Game.LENGTH; i++)
            {
                for (int j = 0; j < Game.LENGTH; j++)
                {
                    if (board[i, j] == 0)
                    {
                        if (i > 0)
                            available.Add(moves[0]);
                        if (j > 0)
                            available.Add(moves[1]);
                        if (j < Game.LENGTH - 1)
                            available.Add(moves[2]);
                        if (i < Game.LENGTH - 1)
                            available.Add(moves[3]);

                        found = true;
                        break;
                    }
                }

                if (found)
                    break;
            }
            return available;
        }

        public static State moveBlank(State currentState, string action)
        {
            State newState = new State(currentState);
            if (!moves.Contains(action))
                return newState;

            int[,] board = newState.getState();
            if (getValidMoves(currentState).Contains(action))
            {
                int[] blankLocation = new int[2];
                bool found = false;
                for (int i = 0; i < Game.LENGTH; i++)
                {
                    for (int j = 0; j < Game.LENGTH; j++)
                    {
                        if (board[i, j] == 0)
                        {
                            blankLocation[0] = i;
                            blankLocation[1] = j;
                            found = true;
                            break;
                        }
                    }

                    if (found)
                        break;
                }

                int temp;
                switch (action)
                {
                    case "up":
                        temp = board[blankLocation[0], blankLocation[1]];
                        board[blankLocation[0], blankLocation[1]] = board[blankLocation[0] - 1, blankLocation[1]];
                        board[blankLocation[0] - 1, blankLocation[1]] = temp;
                        break;
                    case "left":
                        temp = board[blankLocation[0], blankLocation[1]];
                        board[blankLocation[0], blankLocation[1]] = board[blankLocation[0], blankLocation[1] - 1];
                        board[blankLocation[0], blankLocation[1] - 1] = temp;
                        break;
                    case "right":
                        temp = board[blankLocation[0], blankLocation[1]];
                        board[blankLocation[0], blankLocation[1]] = board[blankLocation[0], blankLocation[1] + 1];
                        board[blankLocation[0], blankLocation[1] + 1] = temp;
                        break;
                    case "down":
                        temp = board[blankLocation[0], blankLocation[1]];
                        board[blankLocation[0], blankLocation[1]] = board[blankLocation[0] + 1, blankLocation[1]];
                        board[blankLocation[0] + 1, blankLocation[1]] = temp;
                        break;
                }
            }
            return newState;
        }

    }
}
