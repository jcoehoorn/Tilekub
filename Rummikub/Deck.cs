using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rummikub
{
    public class Deck : ICollection<Tile>
    {
        private static Random prng = new Random();//todo: better random source

        private List<Tile> tiles = new List<Tile>();

        #region ICollection
        public void Add(Tile item)
        {
            tiles.Add(item);
        }

        public void Clear()
        {
            tiles.Clear();
        }

        public bool Contains(Tile item)
        {
            return tiles.Contains(item);
        }

        public void CopyTo(Tile[] array, int arrayIndex)
        {
            tiles.CopyTo(array, arrayIndex);
         }

        public int Count
        {
            get { return tiles.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(Tile item)
        {
            return tiles.Remove(item);
        }

        public IEnumerator<Tile> GetEnumerator()
        {
            return tiles.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

        public void Shuffle()
        {
            for (int i = 0; i < this.Count;i++)
            {
                int j = prng.Next(i, this.Count);
                Tile temp = tiles[i];
                tiles[i] = tiles[j];
                tiles[j] = temp;
            }
        }

        public Tile this[int index]
        {
            get
            {
                return tiles[index];
            }
            set
            {
                tiles[index] = value;
            }
        }
    }
}
