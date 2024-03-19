using System.Collections.Generic;

public class AStarManager
{
    private static AStarManager _instance;

    public static AStarManager Instance
    {
        get
        {
            if (_instance is null)
                _instance = new AStarManager();

            return _instance;
        }
    }
    
    private int Map_X;

    private int Map_Y;

    public AStarNode[,] NodeMaps;

    public Dictionary<NodeAxis, AStarNode> OpenList;

    public Dictionary<NodeAxis, AStarNode> CloseList;

    /// <summary>
    /// 初始化格子
    /// </summary>
    public void InitNode(int _mapX, int _mapY)
    {
        Map_X = _mapX;
        Map_Y = _mapY;
        
        NodeMaps = new AStarNode[Map_X,Map_Y];

        for (int i = 0; i < Map_X; i++)
        {
            for (int j = 0; j < Map_Y; j++)
            {
                NodeMaps[i, j] = new AStarNode(i, j, NodeType.Walk);
            }
        }

        OpenList = new Dictionary<NodeAxis, AStarNode>();
        CloseList = new Dictionary<NodeAxis, AStarNode>();
    }

    public bool FirstCalculateAStarPath(NodeAxis _beginAxis,NodeAxis _endAxis)
    {
        CloseList.Add(_beginAxis, NodeMaps[_beginAxis.X, _beginAxis.Y]);
        return CalculateAStarPath(_beginAxis, _endAxis);
    }
    
    /// <summary>
    /// 计算A星路径
    /// </summary>
    /// <returns></returns>
    public bool CalculateAStarPath(NodeAxis _beginAxis,NodeAxis _endAxis)
    {
        //计算所有周围点位
        for (int i = -1; i <= 1; i++)
        {
            int _curX = _beginAxis.X + i;
            
            //处理边界
            if(_curX < 0 || _curX >= Map_X)
                continue;

            NodeAxis _tempAxis = new NodeAxis();
            _tempAxis.X = _curX;
            for (int j = -1; j <= 1; j++)
            {
                //不处理自身
                if(i == 0 && j == 0)
                    continue;
                
                int _curY = _beginAxis.Y + j;
            
                //处理边界
                if(_curY < 0 || _curY >= Map_Y)
                    continue;
                
                _tempAxis.Y = _curY;
                
                //是否等于终点
                if (_tempAxis.Equals(_endAxis))
                {
                    NodeMaps[_tempAxis.X, _tempAxis.Y].Parent = NodeMaps[_beginAxis.X, _beginAxis.Y];
                    CloseList.Add(_endAxis, NodeMaps[_tempAxis.X, _tempAxis.Y]);
                    return true;
                }
                
                //是否已经计算过了
                if(OpenList.ContainsKey(_tempAxis) || CloseList.ContainsKey(_tempAxis))
                    continue;
                
                //开始计算权重
                OpenList.Add(_tempAxis, NodeMaps[_tempAxis.X, _tempAxis.Y]);
                NodeMaps[_tempAxis.X, _tempAxis.Y].Parent = NodeMaps[_beginAxis.X, _beginAxis.Y];
                NodeMaps[_tempAxis.X, _tempAxis.Y].CalculateDistance(_beginAxis, _endAxis);
            }
        }


        AStarNode _minDisNode = null;
        foreach (var _nodeAxis in OpenList.Keys)
        {
            if (_minDisNode == null || _minDisNode.TotalDis > OpenList[_nodeAxis].TotalDis)
            {
                _minDisNode = OpenList[_nodeAxis];
            }
        }

        if (_minDisNode is null)
            return false;
        
        CloseList.Add(_minDisNode.NodeAxis, _minDisNode);
        return CalculateAStarPath(_minDisNode.NodeAxis, _endAxis);
    }
}