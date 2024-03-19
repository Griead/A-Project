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
    public void InitNode()
    {
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
            _tempAxis.X = i;
            for (int j = -1; j <= 1; j++)
            {
                int _curY = _beginAxis.Y + j;
            
                //处理边界
                if(_curY < 0 || _curY >= Map_Y)
                    continue;

                _tempAxis.Y = j;
                
                //是否等于终点
                if(_beginAxis == _endAxis)
                    return true;
                
                //是否已经计算过了
                if(OpenList.ContainsKey(_tempAxis) || CloseList.ContainsKey(_tempAxis))
                    continue;
                
                OpenList.Add(_tempAxis, NodeMaps[_tempAxis.X, _tempAxis.Y]);
                //开始计算权重
                OpenList[_tempAxis].CalculateDistance(_beginAxis, _endAxis);
            }
        }

        AStarNode _tempAStarNode = null;
        //找出距离最小的数据
        foreach (var _nodeAxis in OpenList.Keys)
        {
            if (_tempAStarNode == null || OpenList[_nodeAxis].TotalDis < _tempAStarNode.TotalDis)
            {
                _tempAStarNode = OpenList[_nodeAxis];
            }
        }

        //如果为空，则表示没有找到路径
        if (_tempAStarNode == null)
            return false;
        
        CloseList.Add(new NodeAxis(_tempAStarNode.NodeAxis), _tempAStarNode);
        return CalculateAStarPath(_tempAStarNode.NodeAxis, _endAxis);
    }
}