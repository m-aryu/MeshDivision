using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 既成メッシュの読み込みを行うインポーター
/// </summary>
public class MeshImporter : MonoBehaviour
{
    /// <summary> 対象MeshFilter </summary>
    [SerializeField] private MeshFilter _targetMeshFilter;

    /// <summary>
    /// メッシュのインポートを行う
    /// </summary>
    public void ImportMesh(MeshCreator meshCreator)
    {
        if (_targetMeshFilter == null || meshCreator == null)
        {
            Debug.LogError("MeshImporter: Target MeshFilter or MeshCreator is not assigned.");
            return;
        }

        Mesh targetMesh = _targetMeshFilter.mesh;
        if (targetMesh == null)
        {
            Debug.LogError("MeshImporter: Target MeshFilter has no mesh.");
            return;
        }

        // GameObjectのTransformを考慮して頂点座標を変換
        var vertexList = new List<Vector3>(targetMesh.vertices.Select(v =>
            _targetMeshFilter.transform.rotation * v + _targetMeshFilter.transform.position));
        var triangleList = new List<int>(targetMesh.triangles);
        var uvList = new List<Vector2>(targetMesh.uv);

        var meshData = new MeshData(vertexList, triangleList, uvList);
        meshCreator.ImportMesh(meshData);
        
        _targetMeshFilter.gameObject.SetActive(false); // インポート後は非表示にする
    }
}
