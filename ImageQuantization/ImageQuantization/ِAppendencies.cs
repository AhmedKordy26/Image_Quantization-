using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageQuantization
{

    public class DSU
    {
        List<int> parent, Gsize; // O(1)
        int Forests, NumNodes; // O(1)
        public DSU(int n) // O (N)
        {
            parent = new List<int>(n); // O(1)
            Gsize = new List<int>(n); // O(1)
            Forests = n; // O(1)
            NumNodes = n; // O(1)
            for (int c = 0; c < n; ++c) // O(N)
            {
                parent.Add(c); // O(1)
                Gsize.Add(1); // O(1)
            }
        }
        public int findParent(int x) //Constant time ~ O(1)
        {
            if (parent[x] == x) return x;// O(1)
            return parent[x] = findParent(parent[x]); // O(1)
        }
        public int GroupSize(int x) // O(1)
        {
            return Gsize[findParent(x)];// O(1)
        }
        public void link(int x, int y) // O(1)
        {
            x = findParent(x);  // O(1)
            y = findParent(y); // O(1)
            if (x == y) return;
            if (Gsize[x] >= Gsize[y]) // O(1)
            {
                Gsize[x] += Gsize[y]; // O(1)
                parent[y] = x; // O(1)
            }
            else
            {
                Gsize[y] += Gsize[x];// O(1)
                parent[x] = y;// O(1)
            }
            Forests--; // O(1)
        }

        public bool IsSameGroup(int x, int y) // O(1) 
        {
            return findParent(x) == findParent(y);// O(1)
        }
        public int numGroubs() // O(1)
        {
            return Forests;// O(1)
        }
        public List<List<int>> ConectedComponents() // O(N)
        {
            List<int>[] components = new List<int>[NumNodes + 1]; // O(1)
            List<int> tmpList = new List<int>(); // O(1)
            List<List<int>> groubs = new List<List<int>>(); // O(1)
            for (int i = 0; i <= NumNodes; ++i) // O(N)
            {
                components[i] = new List<int>(); // O(1)
            }
            for (int i = 0; i < NumNodes; ++i) // O(N)
            {
                components[findParent(i)].Add(i); // O(1)
            }
            for (int i = 0; i < NumNodes; ++i) // O(N)
            {
                if (components[i].Count > 0) // O(1)
                {
                    tmpList = components[i]; // O(1)
                    groubs.Add(tmpList); // O(1)
                }
            }
            return groubs; // O(1)
        }
    }
}
