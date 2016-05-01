using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Route
{
    public class ArcFlagsAlgorithm
    {
        private RoadNetwork grapph;
        private List<Node> boundNodes;
        //region boundry coordinate
        private double latMin;
        private double latMax;
        private double lngMin;
        private double lngMax;
        

        public RoadNetwork Grapph
        {
            get
            {
                return grapph;
            }

            set
            {
                grapph = value;
            }
        }

        public List<Node> BoundNodes
        {
            get
            {
                return boundNodes;
            }

            set
            {
                boundNodes = value;
            }
        }

        public double LatMin
        {
            get
            {
                return latMin;
            }

            set
            {
                latMin = value;
            }
        }

        public double LatMax
        {
            get
            {
                return latMax;
            }

            set
            {
                latMax = value;
            }
        }

        public double LngMin
        {
            get
            {
                return lngMin;
            }

            set
            {
                lngMin = value;
            }
        }

        public double LngMax
        {
            get
            {
                return lngMax;
            }

            set
            {
                lngMax = value;
            }
        }

        public ArcFlagsAlgorithm(RoadNetwork grapph)
        {
            Grapph = grapph;
            DijkstraAlgorithm dijk = new DijkstraAlgorithm(Grapph);
            dijk.ConsiderArcFlag = true;
        }

        // if the node in the region
        public bool IsInRegin(double latMin, double latMax, double lngMin,double lngMax, Node node)
        {
            bool isInRegin = false;
            if(node.coordinate.Latitude<latMax && node.coordinate.Latitude>latMin && 
                node.Coordinate.Longitude<lngMax && node.Coordinate.Longitude>lngMin)
            {
                isInRegin = true;
            }
            return isInRegin;
        }

        // compute boundry nodes
        public void CompBoundNodes()
        {
            Collection<Arc> arcs;
            Node headNode;
            for(int i=0;i<Grapph.Nodes.Count;i++)
            {
                headNode = Grapph.Nodes[i];
                if(!BoundNodes.Contains(headNode))
                {
                    if (IsInRegin(LatMin, LatMax, LngMin, lngMax, headNode))
                    {
                        arcs = Grapph.AdjacentArcs[headNode.Id];
                        for (int k = 0; k < arcs.Count; k++)
                        {
                            Arc tempArc = arcs[k];
                            if (!IsInRegin(LatMin, LatMax, LngMin, lngMax, tempArc.TailNode))
                            {
                                BoundNodes.Add(headNode);
                                break;
                            }
                        }

                     } 
                }
            }
        }

        //compute Dijkstra for each of the boundary nodes
        public void PrecomputeArcFlags()
        {
            for(int i=0;i<BoundNodes.Count;i++)
            {
                Node boundNode = BoundNodes[i];
                DijkstraAlgorithm dij = new DijkstraAlgorithm(Grapph);
                dij.GetShortPath(boundNode.Id, "-1");

                //set flag and save the parents into a list
                Dictionary<string, string> parents = dij.Parents;
                IEnumerator<string> str = parents.Keys.GetEnumerator();

                while(str.MoveNext())
                {
                    string currentNodeId = str.Current;
                    string parentNodeId = parents[currentNodeId];
                    if(parentNodeId !="-1")
                    {
                        Collection<Arc> allarcs = Grapph.AdjacentArcs[currentNodeId];
                        for(int k=0;k<allarcs.Count;k++)
                        {
                            Arc arc = allarcs[k];
                            if(arc.HeadNode.Id == parentNodeId)
                            {
                                arc.ArcFlag = true;
                            }
                        }
                    }
                }
            }
        }

        public double ComputeShprtPath(string sourceId , string targetId)
        {
            CompBoundNodes();
            PrecomputeArcFlags();
            DijkstraAlgorithm dij = new DijkstraAlgorithm(Grapph);
            return dij.GetShortPath(sourceId,targetId);
        }
    }
}
