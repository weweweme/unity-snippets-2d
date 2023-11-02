using System.Collections.Generic;
using UnityEngine;
using static AStarHelper;

public class AStar
{
    // 노드 클래스는 각 그리드의 셀을 표현한다
    public class Node
    {
        public int Index { get; private set; }
        public int G { get; set; }
        public int H { get; set; }
        public int F => G + H;
        public bool IsOpen { get; set; }
        public bool IsClose { get; set; }
        public bool IsObstacle { get; set; }
        public Node Parent { get; set; }

        public Node(int index)
        {
            Index = index;
            G = int.MaxValue;
            H = 0;
        }
    }

    // 모든 노드가 저장되는 리스트
    private List<Node> nodeList;
    // 진행할 수 있는 노드가 저장되는 리스트 
    private List<Node> openList;

    public AStar()
    {
        int totalNodes = GridRows * GridCols;
        nodeList = new List<Node>(totalNodes);
        openList = new List<Node>();

        for (int i = 0; i < totalNodes; i++)
        {
            nodeList.Add(new Node(i));
        }
    }

    // 모든 노드를 반환하는 리스트
    public List<Node> GetNodeList()
    {
        return nodeList;
    }

    // 알고리즘 실행 메서드
    public List<int> Execute(int startIndex, int targetIndex)
    {
        // 결과로 반환될 경로 리스트
        List<int> path = new List<int>();
        // 시작 노드와 목표 노드 저장
        Node startNode = nodeList[startIndex];
        Node targetNode = nodeList[targetIndex];

        // 탐색을 위한 초기화
        startNode.G = 0;
        startNode.H = 0;
        startNode.IsOpen = true;
        openList.Add(startNode);

        while (openList.Count > 0)
        {
            // 오픈 리스트에서 가장 F값이 낮은 노드를 반환
            Node currentNode = GetMinFOpenNode();

            // 현재 노드가 목표 노드라면 경로가 완성된다
            if (currentNode.Index == targetIndex)
            {
                Node temp = currentNode;
                while (temp != null)
                {
                    path.Add(temp.Index);
                    temp = temp.Parent;
                }
                path.Reverse();
                return path;
            }

            // 이웃 노드들 리스트 업
            List<Node> neighbors = FindNearNode(currentNode.Index);

            // 모든 이웃 노드들 순
            foreach (var neighbor in neighbors)
            {
                // 만약 이웃 노드가 닫혀있거나 장애물이라면 무시
                if (neighbor.IsClose || neighbor.IsObstacle)
                {
                    continue;
                }

                // 현재 노드부터 이웃노드까지의 비용 계산
                int newG = currentNode.G + NodeCost;

                // 비용이 더 적거나, 이웃 노드가 오픈 리스트에 없는 경우 업데이트 실행
                if (newG < neighbor.G || !neighbor.IsOpen)
                {
                    neighbor.H = CalculateH(neighbor.Index, targetIndex);
                    neighbor.G = newG;
                    neighbor.Parent = currentNode;

                    // 만약 이웃 노드가 오픈 리스트에 추가된적이 없다면 추가
                    if (!neighbor.IsOpen)
                    {
                        neighbor.IsOpen = true;
                        openList.Add(neighbor);
                    }
                }
            }

            // 이미 탐색한 노드는 청소
            currentNode.IsClose = true;
            openList.Remove(currentNode);
        }

        // 경로를 찾지 못했다면 null을 반환한다
        return null;
    }

    // F값이 제일 적은 값을 반환하는 메서드
    private Node GetMinFOpenNode()
    {
        Node minFNode = openList[0];
        foreach (var node in openList)
        {
            if (node.F < minFNode.F)
            {
                minFNode = node;
            }
        }
        openList.Remove(minFNode);
        return minFNode;
    }

    // 현재 노드의 이웃 노드를 찾는 메서드
    private List<Node> FindNearNode(int currentNodeIndex)
    {
        List<Node> neighbors = new List<Node>();
        
        int row = currentNodeIndex / GridCols;
        int col = currentNodeIndex % GridCols;

        foreach (var (dx, dy) in Directions)
        {
            int newRow = row + dy;
            int newCol = col + dx;

            if (newRow < 0 || newRow >= GridRows || newCol < 0 || newCol >= GridCols)
            {
                continue;
            }

            neighbors.Add(nodeList[newRow * GridCols + newCol]);
        }

        return neighbors;
    }

    // 타겟 노드까지의 휴리스틱을 계산하는 메서드
    private int CalculateH(int currentIndex, int targetIndex)
    {
        int currentRow = currentIndex / GridCols;
        int currentCol = currentIndex % GridCols;
        int targetRow = targetIndex / GridCols;
        int targetCol = targetIndex % GridCols;
        
        return (Mathf.Abs(targetRow - currentRow) + Mathf.Abs(targetCol - currentCol)) * NodeCost;
    }
    
    // 초기화 메서드
    public void Clear()
    {
        openList.Clear();
    
        foreach (var node in nodeList)
        {
            node.G = int.MaxValue;
            node.H = 0;
            node.Parent = null;
            node.IsOpen = false;
            node.IsClose = false;
        }
    }
}
