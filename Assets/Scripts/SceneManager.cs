using UnityEngine;

/// <summary>
/// Scene Manager
/// </summary>
public class SceneManager : MonoBehaviour
{
    private const float MOVE_LINE_SPEED = 5f;
    private const float ROTATE_LINE_SPEED = 5f;
    
    /// <summary> メッシュ生成機 </summary>
    [SerializeField] private MeshCreator _meshCreator;
    
    /// <summary> 分割線 </summary>
    [SerializeField] private GameObject _divideLine;
    
    /// <summary> メッシュ分割機 </summary>
    private MeshDivider _meshDivider;

    private void Start()
    {
        _meshCreator.CreateFirstMesh();
        
        _meshDivider = new MeshDivider();
    }
    
    private void Update()
    {
        // 分割線の回転
        if (Input.GetKey(KeyCode.LeftShift))
        {
            
            if (Input.GetKey(KeyCode.UpArrow))
            {
                _divideLine.transform.Rotate(0, 0, ROTATE_LINE_SPEED * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                _divideLine.transform.Rotate(0, 0, -ROTATE_LINE_SPEED * Time.deltaTime);
            }
        }
        // 分割線の移動
        else
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
               _divideLine.transform.Translate(0, MOVE_LINE_SPEED * Time.deltaTime, 0);
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                _divideLine.transform.Translate(0, -MOVE_LINE_SPEED * Time.deltaTime, 0);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                _divideLine.transform.Translate(MOVE_LINE_SPEED * Time.deltaTime, 0, 0);
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                _divideLine.transform.Translate(-MOVE_LINE_SPEED * Time.deltaTime, 0, 0);
            }
        }

        // 分割
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var lineSlope = Mathf.Tan(_divideLine.transform.rotation.eulerAngles.z * Mathf.Deg2Rad);
            var lineIntercept = _divideLine.transform.position.y - lineSlope * _divideLine.transform.position.x;
            _meshDivider.DivideMesh(_meshCreator.MeshDataList, lineSlope, lineIntercept, _meshCreator.ClearMesh, _meshCreator.CreateMesh);
        }

        // リセット
        if (Input.GetKeyDown(KeyCode.R))
        {
            _meshCreator.CreateFirstMesh();
        }
    }
}
