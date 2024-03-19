using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class AStarTest : MonoBehaviour
{
    public GameObject Node;
    
    public int Map_X;

    public int Map_Y;

    public Dictionary<NodeAxis, GameObject> AxisDict;

    public NodeAxis BeginAxis;

    public NodeAxis EndAxis;

    private void Awake()
    {
        AStarManager.Instance.InitNode(Map_X, Map_Y);
    }

    private void Start()
    {
        CreateMap();
    }

    /// <summary>
    /// 创建地图块
    /// </summary>
    private void CreateMap()
    {
        var _interval = 0.1f;

        AxisDict = new Dictionary<NodeAxis, GameObject>();
        for (int i = 0; i < Map_X; i++)
        {
            for (int j = 0; j < Map_Y; j++)
            {
                NodeAxis nodeAxis = new NodeAxis();
                nodeAxis.X = i;
                nodeAxis.Y = j;

                var _str = $"{i}_{j}";
                var _quadInstance = GameObject.Instantiate(Node);
                
                _quadInstance.GetComponent<NodeMono>().SetText(_str);
                _quadInstance.name = _str;
                
                _quadInstance.transform.localEulerAngles = new Vector3(90, 0, 0);
                AxisDict.Add(nodeAxis, _quadInstance);

                _quadInstance.transform.position = new Vector3((i - Map_X * 0.5f) * (1 + _interval), 1,
                    (j - Map_Y * 0.5f) * (1 + _interval));
            }
        }
    }

    public void FindEnd()
    {
        if (AStarManager.Instance.FirstCalculateAStarPath(BeginAxis, EndAxis))
        {
            Debug.Log("找到了结果");
            AStarNode AStarNode = AStarManager.Instance.CloseList[EndAxis];

            List<AStarNode> result = new List<AStarNode>();
            while (AStarNode != null)
            {
                result.Add(AStarNode);
                Debug.Log(AStarNode.NodeAxis.ToString());
                AStarNode = AStarNode.Parent;
            }
        }
        else
        {
            Debug.Log("未找到结果");
        }
    }
}