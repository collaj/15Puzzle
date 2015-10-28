using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _15Puzzle
{
    // look into making this class indexable
    public class State
    {
        private int[,] state;

        public State(int[,] state)
        {
            this.state = state;
        }
        public State(State aState)
        {
            int[,] aStateBoard = aState.getState();
            int length = aStateBoard.GetLength(0);
            this.state = new int[length, length];
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    this.state[i, j] = aStateBoard[i, j];
                }
            }
        }


        public int[,] getState()
        {
            return this.state;
        }


        public override bool Equals(Object obj)
        {
            if (obj is State) {
                State aState = (State)obj;
                for (int i = 0; i < this.state.GetLength(0); i++)
                {
                    for (int j = 0; j < this.state.GetLength(0); j++)
                    {
                        if (this.state[i, j] != aState.state[i, j])
                            return false;
                    }
                }
                return true;
            }
            return false;
        }

        public State clone() // deep copy
        {
            return new State(this);
        }

    }
}
