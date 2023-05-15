//-----------------------------------
//担当：二宮怜
//内容：メッシュ作成
//-----------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMesh : MonoBehaviour
{
    // 変数宣言
    //[SerializeField] private int _materialNumber;
    //[SerializeField] private Material _material;

    [SerializeField] MeshFilter _meshFilter;     //メッシュフィルター
    [SerializeField] MeshRenderer _meshRenderer; // メッシュレンダラー

    [Header("初期では四角形が描画されるよう設定済み")]
    // 初期値(z座標は適当)
    // 0 : -5, 5, 0
    // 1 :  5, 5, 0
    // 2 :  5,-5, 0
    // 3 : -5,-5, 0
    [SerializeField] Vector3[] Verts;
    
    // 初期値
    // 0 : 0
    // 1 : 1
    // 2 : 3
    // 3 : 1
    // 4 : 2
    // 5 : 3
    [SerializeField] int[] Triangles; // Verts（頂点配列）のインデックス

    // Start is called before the first frame update
    void Start()
    {
        // 特にいじる必要無し↓↓↓

        //-----------------------------------------------------------------------------
        // 作成したいメッシュの情報をセットする（頂点位置、三角形、法線）
        // 頂点、三角形はインスペクタービューで設定してね

        // Meshクラス
        Mesh mesh = new Mesh();
        // 頂点情報をセット
        mesh.vertices = Verts;
        // 三角形情報をセット
        mesh.triangles = Triangles;
        // 何かよくわからんけど多分法線
        mesh.RecalculateNormals();

        //---------------------------------------------------------------------------------
        //脳死で書いとけ↓↓↓

        // メッシュフィルターにさっきセットした情報を渡す
        _meshFilter.sharedMesh = mesh;
        // 与えたマテリアルをセットする
        //_meshRenderer.sharedMaterial = _material;
        // メッシュレンダラーにセットされたマテリアルの中から指定されたマテリアルをセットする
        //_meshRenderer.sharedMaterial = _meshRenderer.materials[_materialNumber]; // できなかった
    }
}
