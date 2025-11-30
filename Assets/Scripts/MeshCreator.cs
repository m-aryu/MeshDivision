using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// メッシュ生成機
/// </summary>
public class MeshCreator : MonoBehaviour
{
    /// <summary> Material </summary>
    [SerializeField] private Material _material;
    
    /// <summary> メッシュ情報リスト </summary>
    public List<MeshData> MeshDataList { get; private set; } = new List<MeshData>();
    
    // 生成済みオブジェクトリスト
    private List<GameObject> _generatedObjectList = new List<GameObject>();
    
    /// <summary>
    /// メッシュ生成
    /// </summary>
    public void CreateFirstMesh()
    {
        const float WIDTH = 1;
        const float HEIGHT = 1;
        // 1辺の頂点の数
        const int ROW_NUM = 8;
        const int COL_NUM = 8;
        
        ClearMesh();

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
        
        var meshData = new MeshData(vertexList, triangleList, uvList);
        CreateMesh(meshData);
    }
    
    /// <summary>
    /// 汎用メッシュ生成処理
    /// </summary>
    public void CreateMesh(MeshData meshData)
    {
        var newObject = new GameObject("GeneratedMesh");
        var meshFilter = newObject.AddComponent<MeshFilter>();
        var meshRenderer = newObject.AddComponent<MeshRenderer>();
        var newMesh = new Mesh();

        newMesh.SetVertices(meshData.VertexList);
        newMesh.SetTriangles(meshData.TriangleList, 0);
        newMesh.SetUVs(0, meshData.UvList);
        
        // 変更の反映
        newMesh.RecalculateNormals();
        meshFilter.mesh = newMesh;
        var newMaterial = new Material(_material);
        newMaterial.color = new Color(Random.Range(0,1f), Random.Range(0,1f), Random.Range(0,1f));
        meshRenderer.material = newMaterial;
        
        MeshDataList.Add(meshData);
        _generatedObjectList.Add(newObject);
    }
    
    /// <summary>
    /// メッシュ破棄
    /// </summary>
    public void ClearMesh()
    {
        foreach (var obj in _generatedObjectList)
        {
            Destroy(obj);
        }
        _generatedObjectList.Clear();
        
        MeshDataList.Clear();
    }
}
