using System.Collections.Generic;
using UnityEngine;
using static AStarHelper;

public class AStarTester : MonoBehaviour
{
    public GameObject tilePrefab;
    public GameObject player;

    private AStar aStar;
    private List<AStar.Node> nodeList;

    void Start()
    {
        aStar = new AStar();
        nodeList = aStar.GetNodeList();

        for (int y = 0; y < GridRows; y++)
        {
            for (int x = 0; x < GridCols; x++)
            {
                Instantiate(tilePrefab, new Vector3(x * CellSize, y * CellSize, 0), Quaternion.identity);
            }
        }

        int randomIndex = Random.Range(0, nodeList.Count);
        player.transform.position = IndexToPosition(randomIndex);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int targetIndex = PositionToIndex(mouseWorldPos);

            if (!nodeList[targetIndex].IsObstacle)
            {
                int startIndex = PositionToIndex(player.transform.position);
                List<int> path = aStar.Execute(startIndex, targetIndex);

                if (path != null && path.Count > 0)
                {
                    Vector3 targetPosition = IndexToPosition(path[path.Count - 1]);
                    player.transform.position = targetPosition;

                    // 경로 로그로 출력
                    foreach (var index in path)
                    {
                        Debug.Log($"Path node: {index}");
                    }
                    
                    aStar.Clear();
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int index = PositionToIndex(mouseWorldPos);
            nodeList[index].IsObstacle = !nodeList[index].IsObstacle;
        }
    }

    private int PositionToIndex(Vector3 position)
    {
        int x = Mathf.FloorToInt(position.x / CellSize);
        int y = Mathf.FloorToInt(position.y / CellSize);
        return y * GridCols + x;
    }

    private Vector3 IndexToPosition(int index)
    {
        int x = index % GridCols;
        int y = index / GridCols;
        return new Vector3(x * CellSize, y * CellSize, 0);
    }
}
