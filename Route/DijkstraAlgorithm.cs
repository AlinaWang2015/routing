using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Route
{
    public class DijkstraAlgorithm
    {
        private RoadNetwork graph;
        private Dictionary<string, double> visitedNodeMarks;
        private List<ActiveNode> activeNodes;
        private Dictionary<string, double> heuristics;
        private Dictionary<string, string> parents;

        public DijkstraAlgorithm() { }

        public DijkstraAlgorithm(RoadNetwork graph)
        {
            this.graph = graph;
        }

        public Dictionary<string, double> VisitedNodeMarks
        {
            get
            {
                if (visitedNodeMarks == null)
                {
                    visitedNodeMarks = new Dictionary<string, double>();
                }
                return visitedNodeMarks;
            }
        }

        public List<ActiveNode> ActiveNodes
        {
            get
            {
                if (activeNodes == null)
                {
                    activeNodes = new List<ActiveNode>();
                }
                return activeNodes;
            }
        }

        public Dictionary<string, string> Parents
        {
            get
            {
                if (parents == null)
                {
                    parents = new Dictionary<string, string>();
                }
                return parents;
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

        public Dictionary<string, double> Heuristics
        {
            get
            {
                return heuristics;
            }

            set
            {
                heuristics = value;
            }
        }

        public IComparer<ActiveNode> costComparer = new ComparatorToAnother();


        public double GetShortPath(string startNodeId, string targetNodeId)
        {
            parents = new Dictionary<string, string>();
            visitedNodeMarks = new Dictionary<string, double>();
            double shortestPathCost = 0;
            Collection<Arc> nodeAdjacentArc;
            int numSettledNodes = 0;
            double distToAdjNode = 0;
            ActiveNode startNode;
            ActiveNode activeNode;
            //ActiveNode currentNode;

            if (Heuristics == null)
            {
                startNode = new ActiveNode(startNodeId, 0, 0, "-1");
            }
            else
            {
                startNode = new ActiveNode(startNodeId, 0, Heuristics[startNodeId], "-1");
            }

            PriorityQueue<ActiveNode> pq = new PriorityQueue<ActiveNode>(1, costComparer);
            //activeNodes = new List<ActiveNode>();
            //parents = new Dictionary<string, string>();
            pq.Push(startNode);


            while (pq.Count != 0)
            {
                
                ActiveNode currentNode = pq.Pop();
                
               //prevent visited add to parents
                if (isvisited(currentNode.id))
                {
                    continue;
                }
                VisitedNodeMarks.Add(currentNode.id, currentNode.dist);
                parents.Add(currentNode.id, currentNode.parent);
                numSettledNodes++;
                if (currentNode.id == targetNodeId)
                {
                    shortestPathCost = currentNode.dist;
                    break;
                }
                //stop when all reachable nodes are settled
                if (numSettledNodes > graph.Nodes.Count()|| numSettledNodes== graph.Nodes.Count())
                {
                    //zhe li you wen ti ,bu neng dao dao zui hou yi ge jie dian 
                    shortestPathCost = currentNode.dist; 
                    break;
                }
                nodeAdjacentArc = this.graph.AdjacentArcs[currentNode.id];
                for (int i = 0; i < nodeAdjacentArc.Count(); i++)
                {
                    Arc arc = nodeAdjacentArc[i];
                    if (!isvisited(arc.TailNode.Id))
                    {
                        distToAdjNode = currentNode.dist + nodeAdjacentArc[i].Cost;
                        if (Heuristics == null)
                        {
                            activeNode = new ActiveNode(arc.TailNode.Id, distToAdjNode, 0, currentNode.id);
                        }
                        else
                        {
                            activeNode = new ActiveNode(arc.TailNode.Id, distToAdjNode, Heuristics[currentNode.id], currentNode.id);
                        }

                        pq.Push(activeNode);
                        
                    }
                }
            }
            return shortestPathCost;
        }

        public bool isvisited(string nodeId)
        {
            if (visitedNodeMarks.ContainsKey(nodeId))
            {
                return true;
            }
            return false;
        }

        public Dictionary<string, double> ComputeStraihtHeuristic(string targetNodeId)
        {
            heuristics = new Dictionary<string, double>();
            double distance;
            int maxSpeed = 110;
            double costTime;
            Node targetNode = graph.MapNodes[targetNodeId];
            for (int i = 0; i < graph.Nodes.Count(); i++)
            {
                distance = graph.ComputeDistance(graph.Nodes[i], targetNode);
                costTime = distance / maxSpeed;
                heuristics.Add(graph.Nodes[i].Id, costTime);
            }
            return heuristics;
        }

        public void ShortPathToString(string startNodeId, string targetNodeId)
        {
            string path = "";
            Node currentNode = new Node();
            string currentNodeId = "";
            currentNode = graph.MapNodes[targetNodeId];
            currentNodeId = targetNodeId;
            path = path + currentNode.Id + "->";
            while (currentNodeId != startNodeId)
            {
                currentNodeId = parents[currentNodeId];
                currentNode = graph.MapNodes[currentNodeId];
                path = currentNode.Id + "->" + path;
            }
            double cost = GetShortPath(startNodeId,targetNodeId);
            Console.WriteLine("short path cost is: " + cost);
            Console.WriteLine("short path is: " + path);
        }


    }
}
