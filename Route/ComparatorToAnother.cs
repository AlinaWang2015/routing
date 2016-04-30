using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Route
{
    public class ComparatorToAnother : IComparer<ActiveNode>
    {
        public ComparatorToAnother() { }

        public int Compare(ActiveNode n1, ActiveNode n2)
        {
            double dis = (n1.dist + n1.heuristic) - (n2.dist + n2.heuristic);
            if (dis > 0)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }
    }
}
