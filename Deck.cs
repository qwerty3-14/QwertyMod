using System.Collections.Generic;
using Terraria;

namespace QwertyMod
{
    public class Deck<F> : List<F>
    {
        //This class is inspired and tries to simulate drawing from a deck of cards

        public F[] Draw(int n)
        {
            F[] selection = new F[n];

            Deck<F> g = ShuffledCopy();

            for (int i = 0; i < n; i++)
            {
                selection[i] = g[i];
            }

            return selection;
        }

        public void Shuffle()
        {
            Deck<F> g = ShuffledCopy();
            Clear();
            foreach (F item in g)
            {
                Add(item);
            }
        }

        public Deck<F> ShuffledCopy()
        {
            Deck<F> g = new Deck<F>();
            Deck<F> c = new Deck<F>();
            foreach (F item in this)
            {
                c.Add(item);
            }
            while (c.Count > 0)
            {
                int r = Main.rand.Next(c.Count);
                g.Add(c[r]);
                c.RemoveAt(r);
            }
            return g;
        }
    }
}