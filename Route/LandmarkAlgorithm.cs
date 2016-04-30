using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Route
{
    public class LandmarkAlgorithm 
    {
        private RoadNetwork graph;
        private List<string> landmarksIds ;


        int numOfLandmarks;

        public List<string> LandmarksIds
        {
            get
            {
                return landmarksIds;
            }

            set
            {
                landmarksIds = value;
            }
        }

        public int NumOfLandmarks
        {
            get
            {
                return numOfLandmarks;
            }

            set
            {
                numOfLandmarks = value;
            }
        }

        public RoadNetwork Graph
        {
            get
            {
                return graph;
            }

            set
            {
                graph = value;
            }
        }

        

        public LandmarkAlgorithm() { }

        public LandmarkAlgorithm(RoadNetwork graph)
        {
            Graph = graph;
        }

        //dijkstra from a set of nodes
        

        public Dictionary<string, double> PreComputLandmarksDistances(string currentLandmarkId)
        {
            Dictionary<string, double> costDictionary = new Dictionary<string, double>();
            DijkstraAlgorithm dijkstra = new DijkstraAlgorithm(Graph);
            dijkstra.GetShortPath(currentLandmarkId, "-1");
            costDictionary = dijkstra.VisitedNodeMarks;
            return costDictionary;
        }

        //Start with a random node, then iteratively add more
        public void SelectLandmarks(int numOfLandmarks)
        {
            Random ran = new Random();
            int random = ran.Next(Graph.Nodes.Count);
            string currentLandmarkId = Graph.Nodes[random].Id;
            LandmarksIds = new List<string>();
            LandmarksIds.Add(currentLandmarkId);
            

            if (numOfLandmarks < Graph.Nodes.Count)
            {
                if (!LandmarksIds.Contains(currentLandmarkId) || LandmarksIds.Count< numOfLandmarks)
                {
                    for (int i = 1; i < numOfLandmarks; i++)
                    {
                        Dictionary<string, double> cost = PreComputLandmarksDistances(currentLandmarkId);
                        double maxDistance = 0;
                        foreach (var item in cost)
                        {
                            if (item.Value > maxDistance)
                            {
                                maxDistance = item.Value;
                                currentLandmarkId = item.Key;
                            }
                        }

                        if(!LandmarksIds.Contains(currentLandmarkId))
                        {
                            LandmarksIds.Add(currentLandmarkId);
                        }
                        else
                        {
                            i = i - 1;
                        }
                        
                    }
                }
            }   
        }

        



       

        public Dictionary<string,double> GetHeuristicDictionary(string targetNodeId)
        {
            Dictionary<string, double> heuristic = new Dictionary<string, double>();
            List<Node> node = Graph.Nodes;
            for (int i = 0; i < node.Count; i++)
            {
                string sourceNodeId = node[i].Id;
                double nodeHeuristic = ComputeHeuristic(sourceNodeId, targetNodeId);
                heuristic.Add(sourceNodeId,nodeHeuristic);
            }
            return heuristic;
        }

        //In each iteration, pick the node u which maximizes
        public double ComputeHeuristic(string sourceNodeId, string targetNodeId)
        {
            double heuristic = 0;
            
            for (int i = 0; i < LandmarksIds.Count; i++)
            {
                //string current = LandmarksIds[i];
                Dictionary<string, double> cost = PreComputLandmarksDistances(sourceNodeId);
                if (cost.ContainsKey(sourceNodeId))
                {
                    double distFromLtoU = cost[sourceNodeId];
                    double disFromLtoT = cost[targetNodeId];
                    double currentHeuristic = Math.Abs(distFromLtoU - disFromLtoT);
                    if (currentHeuristic > heuristic)
                    {
                        heuristic = currentHeuristic;
                    }
                }
            }
            return heuristic;
        }

        public double ComputeShortPath(string sourceNodeId, string targetNodeId)
        {
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
            DijkstraAlgorithm dij = new DijkstraAlgorithm(Graph);
            dij.Heuristics = GetHeuristicDictionary(targetNodeId); 
            double cost = dij.GetShortPath(sourceNodeId, targetNodeId);
            dij.ShortPathToString(sourceNodeId, targetNodeId);
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
            return cost;

        }
    }
}
