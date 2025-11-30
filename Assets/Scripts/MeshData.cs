using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// メッシュデータ
/// </summary>
public class MeshData
{
    public readonly List<Vector3> VertexList = new List<Vector3>();
    public readonly List<int> TriangleList = new List<int>();
    public readonly List<Vector2> UvList = new List<Vector2>();
    
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="vertexList"></param>
    /// <param name="triangleList"></param>
    /// <param name="uvList"></param>
    public MeshData(List<Vector3> vertexList, List<int> triangleList, List<Vector2> uvList)
    {
        VertexList = vertexList;
        TriangleList = triangleList;
        UvList = uvList;
    }
}