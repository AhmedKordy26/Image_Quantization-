using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageQuantization
{
    
    public class DSU
    {
        List<int> parent, Gsize;
        int Forests, NumNodes;
        public DSU(int n) // O ( log V )
        {
            parent = new List<int>(n );
            Gsize = new List<int>(n );
            Forests = n;
            NumNodes = n;
            for (int c = 0; c < n; ++c)
            {
                parent.Add(c);
                Gsize.Add(1);
            }
        }
        public int findParent(int x)
        {
            if (parent[x] == x) return x;
            return parent[x] = findParent(parent[x]);
        }
        public int GroupSize(int x)
        {
            return Gsize[findParent(x)];
        }
        public void link(int x, int y)
        {
            x = findParent(x);
            y = findParent(y);
            if (x == y) return;
            if (Gsize[x] >= Gsize[y])
            {
                Gsize[x] += Gsize[y];
                parent[y] = x;
            }
            else
            {
                Gsize[y] += Gsize[x];
                parent[x] = y;
            }
            Forests--;
        }

        public bool IsSameGroup(int x, int y)
        {
            return findParent(x) == findParent(y);
        }
      /*  vector<vector<int>> ConectedComponents()
        {
            vector<vector<int>> List;
            List.resize(NumNodes + 5);
            for (int c = 0; c <= NumNodes; c++)
            {
                List[findParent(c)].push_back(c);
            }
            return List;
        }*/
    }
}
