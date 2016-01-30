using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakesAndFoxes
{
    class RollTheDice
    {
        private Random random = new Random();
        int[] rollResults;

        public int[] roll(int rolls)
        {
            rollResults = new int[rolls];

            for (int i = 0; i < rolls; i++)
            {
                rollResults[i] = random.Next(6) + 1;
            }

            return rollResults;
        }
    }
}
