using UnityEngine;

/// <summary>
/// Scene Manager
/// </summary>
public class SceneManager : MonoBehaviour
{
    /// <summary> メッシュ生成機 </summary>
    [SerializeField] private MeshCreator _meshCreator;
    
    /// <summary> メッシュ分割機 </summary>
    private MeshDivider _meshDivider;

    private void Start()
    {
        _meshCreator.CreateFirstMesh();
        
        _meshDivider = new MeshDivider();
        _meshDivider.DivideMesh(_meshCreator.MeshDataList, _meshCreator.ClearMesh, _meshCreator.CreateMesh);
    }
}
