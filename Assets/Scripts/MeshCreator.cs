using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// メッシュ生成機
/// </summary>
public class MeshCreator : MonoBehaviour
{
    /// <summary> Material </summary>
    [SerializeField] private Material _material;
    
    private void Start()
    {
        CreateMesh();
    }
    
    /// <summary>
    /// メッシュ生成
    /// </summary>
    private void CreateMesh()
    {
        const float WIDTH = 1;
        const float HEIGHT = 1;
        // 1辺の頂点の数
        const int ROW_NUM = 3;
        const int COL_NUM = 3;
        
        var newObject = new GameObject("GeneratedMesh");
        var meshFilter = newObject.AddComponent<MeshFilter>();
        var meshRenderer = newObject.AddComponent<MeshRenderer>();
        var newMesh = new Mesh();

        // 頂点座標設定
        var vertexList = new List<Vector3>();
        for (int rowIdx = 0; rowIdx < ROW_NUM; rowIdx++)
        {
            for (int colIdx = 0; colIdx < COL_NUM; colIdx++)
            {
                float x = colIdx % COL_NUM * WIDTH;
                float y = rowIdx * HEIGHT;
                vertexList.Add(new Vector3(x, y, 0));
            }
        }
        newMesh.SetVertices(vertexList);
        
        // 三角形の頂点インデックス設定
        var triangleList = new List<int>();
        for (int rowIdx = 0; rowIdx < ROW_NUM; rowIdx++)
        {
            for (int colIdx = 0; colIdx < COL_NUM; colIdx++)
            {
                int currentIdx = rowIdx * ROW_NUM + colIdx;
                if (colIdx < COL_NUM - 1 && rowIdx < ROW_NUM - 1)
                {
                    triangleList.Add(currentIdx);
                    triangleList.Add(currentIdx + COL_NUM);
                    triangleList.Add(currentIdx + 1);

                    triangleList.Add(currentIdx + 1);
                    triangleList.Add(currentIdx + COL_NUM);
                    triangleList.Add(currentIdx + COL_NUM + 1);
                }
            }
        }
        newMesh.SetTriangles(triangleList, 0);
        
        // UV設定
        var uvList = new List<Vector2>();
        for (int rowIdx = 0; rowIdx < ROW_NUM; rowIdx++)
        {
            for (int colIdx = 0; colIdx < COL_NUM; colIdx++)
            {
                float u = rowIdx / (ROW_NUM - 1);
                float v = colIdx / (COL_NUM - 1);
                uvList.Add(new Vector2(u, v));
            }
        }
        newMesh.SetUVs(0, uvList);
        
        // 変更の反映
        newMesh.RecalculateNormals();
        meshFilter.mesh = newMesh;
        meshRenderer.material = _material;
    }
}
