using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// メッシュ分割機
/// </summary>
public class MeshDivider
{
    /// <summary>
    /// 分割結果
    /// </summary>
    public class DivideResult
    {
        /// <summary> 分割後の頂点リスト 上側 </summary>
        public List<Vector3> TopVertexList = new List<Vector3>();
        
        /// <summary> 分割後の頂点リスト 下側 </summary>
        public List<Vector3> BotVertexList = new List<Vector3>();
        
        /// <summary> 分割後の三角形リスト 上側 </summary>
        public List<int> TopTriangleList = new List<int>();
        
        /// <summary> 分割後の三角形リスト 下側 </summary>
        public List<int> BotTriangleList = new List<int>();
    }
    
    // 分割結果リスト
    private List<DivideResult> _resultList = new List<DivideResult>();
    
    /// <summary>
    /// メッシュ分割を行う
    /// </summary>
    public void DivideMesh(List<MeshData> meshDataList, float lineSlope, float lineIntercept, Action onCompleteCalc, Action<MeshData> createMeshFunc)
    {
        _resultList.Clear();
        
        for (int mdIdx = 0; mdIdx < meshDataList.Count; mdIdx++)
        {
            var result = new DivideResult();
            
            var meshData = meshDataList[mdIdx];
            for (int triIdx = 0; triIdx < meshData.TriangleList.Count; triIdx += 3)
            {
                int idx0 = meshData.TriangleList[triIdx];
                int idx1 = meshData.TriangleList[triIdx + 1];
                int idx2 = meshData.TriangleList[triIdx + 2];

                Vector3 v0 = meshData.VertexList[idx0];
                Vector3 v1 = meshData.VertexList[idx1];
                Vector3 v2 = meshData.VertexList[idx2];
                
                DivideSingleTriangle(v0, v1, v2, lineSlope, lineIntercept, result);
            }
            
            _resultList.Add(result);
        }
        
        // 最後に、分割したメッシュで再生成
        onCompleteCalc?.Invoke();
        foreach (var divideResult in _resultList)
        {
            var topMeshData = new MeshData(divideResult.TopVertexList, divideResult.TopTriangleList, new List<Vector2>());
            var botMeshData = new MeshData(divideResult.BotVertexList, divideResult.BotTriangleList, new List<Vector2>());
            createMeshFunc?.Invoke(topMeshData);
            createMeshFunc?.Invoke(botMeshData);
        }
    }
    
    /// <summary>
    /// 指定した三角形を直線で分割する
    /// </summary>
    /// <remarks>
    /// 直線: y = ax + b
    /// </remarks>
    private void DivideSingleTriangle(Vector3 v1, Vector3 v2, Vector3 v3, float a, float b, DivideResult result)
    {
        // 三角形の各辺と直線の交差判定
        var isCross1 = IsCrossing(v1, v2, a, b, out Vector3 crossPoint1);
        var isCross2 = IsCrossing(v2, v3, a, b, out Vector3 crossPoint2);
        var isCross3 = IsCrossing(v3, v1, a, b, out Vector3 crossPoint3);
        
        // 交差している辺の数に応じて処理を分岐
        int crossCount = 0;
        if (isCross1) crossCount++;
        if (isCross2) crossCount++;
        if (isCross3) crossCount++;
        switch (crossCount)
        {
            case 0:
            {
                // 交差なし: 三角形全体がどちらか一方に属する
                AddTriangleToList(v1, v2, v3);
                break;
            }

            case 1:
            {
                // 1辺が交差: 三角形が2つに分割される(1頂点を通る)
                if (isCross1)
                {
                    // 辺v1-v2が交差, v3を通る
                    AddTriangleToList(v1, crossPoint1, v3);
                    AddTriangleToList(v2, v3, crossPoint1);
                }
                else if (isCross2)
                {
                    // 辺v2-v3が交差, v1を通る
                    AddTriangleToList(v2, crossPoint2, v1);
                    AddTriangleToList(v3, v1, crossPoint2);
                }
                else if (isCross3)
                {
                    // 辺v3-v1が交差, v2を通る
                    AddTriangleToList(v3, crossPoint3, v2);
                    AddTriangleToList(v1, v2, crossPoint3);
                }
                break;
            }

            case 2:
            {
                // 2辺が交差: 三角形が3つに分割される
                if (!isCross1)
                {
                    // 辺v1-v2が交差していない
                    AddTriangleToList(v1, crossPoint2, crossPoint3);
                    AddTriangleToList(v1, v2, crossPoint2);
                    AddTriangleToList(v3, crossPoint3, crossPoint2);
                }
                else if (!isCross2)
                {
                    // 辺v2-v3が交差していない
                    AddTriangleToList(v2, crossPoint3, crossPoint1);
                    AddTriangleToList(v2, v3, crossPoint3);
                    AddTriangleToList(v1, crossPoint1, crossPoint3);
                }
                else
                {
                    // 辺v3-v1が交差していない
                    AddTriangleToList(v3, crossPoint1, crossPoint2);
                    AddTriangleToList(v3, v1, crossPoint1);
                    AddTriangleToList(v2, crossPoint2, crossPoint1);
                }
                break;
            }
            
            case 3:
            {
                // 3辺が交差: 理論上ありえない
                Debug.LogError("Unexpected case: all three edges are crossing.");
                break;
            }
        }
        
        // 三角形のList登録
        void AddTriangleToList(Vector3 vert1, Vector3 vert2, Vector3 vert3)
        {
            // 重心が直線の上側か下側かで振り分け
            float centerX = (vert1.x + vert2.x + vert3.x) / 3.0f;
            float centerY = (vert1.y + vert2.y + vert3.y) / 3.0f;
            float lineY = a * centerX + b;
            var targetVertexList = (centerY < lineY) ? result.BotVertexList : result.TopVertexList;
            var targetTriangleList = (centerY < lineY) ? result.BotTriangleList : result.TopTriangleList;
            
            targetVertexList.Add(vert1);
            targetVertexList.Add(vert2);
            targetVertexList.Add(vert3);
            int baseIdx = targetVertexList.Count - 3;
            targetTriangleList.Add(baseIdx);
            targetTriangleList.Add(baseIdx + 1);
            targetTriangleList.Add(baseIdx + 2);
        }
    }
    
    /// <summary>
    /// 2頂点で構成される線分が直線と交差しているか
    /// </summary>
    /// <remarks>
    /// 直線: y = ax + b
    /// 線分: v1,v2
    /// 交点: crossPoint
    /// </remarks>
    private bool IsCrossing(Vector3 v1, Vector3 v2, float a, float b, out Vector3 crossPoint)
    {
        crossPoint = Vector3.zero;
        
        // 2頂点が同x座標の場合は例外処理
        if (Mathf.Approximately(v1.x, v2.x))
        {
            // 直線は y = ax + b
            // 線分は x = v1.x
            float lineY = a * v1.x + b;
            if (lineY < Mathf.Min(v1.y, v2.y) || lineY > Mathf.Max(v1.y, v2.y))
            {
                // 線分の範囲外
                return false;
            }
            
            crossPoint = new Vector3(v1.x, lineY, 0);
            return true;
        }
        
        // 直線は y = ax + b
        // 線分は y = mx + c  (m=(y2-y1)/(x2-x1), c=y1-m*x1)
        float m = (v2.y - v1.y) / (v2.x - v1.x);
        float c = v1.y - m * v1.x;
        
        if (Mathf.Approximately(m, a))
        {
            // 平行
            return false;
        }
        
        // 交点情報
        float crossX = (b - c) / (m - a);
        if (crossX < Mathf.Min(v1.x, v2.x) || crossX > Mathf.Max(v1.x, v2.x))
        {
            // 線分の範囲外
            return false;
        }
        
        float crossY = a * crossX + b;
        crossPoint = new Vector3(crossX, crossY, 0);
        return true;
    }
}
