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
        var newObject = new GameObject("GeneratedMesh");
        var meshFilter = newObject.AddComponent<MeshFilter>();
        var meshRenderer = newObject.AddComponent<MeshRenderer>();
        var newMesh = new Mesh();

        var width = 1;
        var height = 1;
        
        // 頂点座標設定
        // 2--3
        // |  |
        // 0--1
        Vector3[] vertices = new Vector3[4];
        vertices[0] = new Vector3(0, 0, 0);
        vertices[1] = new Vector3(width, 0, 0);
        vertices[2] = new Vector3(0, height, 0);
        vertices[3] = new Vector3(width, height, 0);
        newMesh.SetVertices(vertices);
        
        // 三角形の頂点インデックス設定
        int[] triangles = new int[6]
        {
            0, 2, 1,
            2, 3, 1
        };
        newMesh.SetTriangles(triangles, 0);
        
        // UV設定
        Vector2[] uv = new Vector2[4];
        uv[0] = new Vector2(0, 0);
        uv[1] = new Vector2(1, 0);
        uv[2] = new Vector2(0, 1);
        uv[3] = new Vector2(1, 1);
        newMesh.uv = uv;
        
        // 変更の反映
        newMesh.RecalculateNormals();
        meshFilter.mesh = newMesh;
        meshRenderer.material = _material;
    }
}
