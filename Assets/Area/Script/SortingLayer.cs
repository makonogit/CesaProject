//----------------------------------------------------------
// 担当者：中川直登
// 内容  ：MeshRendererにレイヤーを設定する
//----------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class SortingLayer : MonoBehaviour
{
	//-----------------------------------------------------------------
	//―秘匿変数―(私)
	[SerializeField,SortingLayer]
	private string layerName = "Default";
	[SerializeField]
	private int orderInLayer = 0;
	//-----------------------------------------------------------------
	//―初期化処理―
	void Awake()// インスタンス直後(Startより先に呼ばれる)
	{
		LayerName = layerName;
		OrderInLayer = orderInLayer;
	}
	//-----------------------------------------------------------------
	//―設定処理―
	void OnValidate()// Inspectorを触った時の処理
	{
		LayerName = layerName;
		OrderInLayer = orderInLayer;
	}

	public string LayerName
	{
		get
		{
			return layerName;
		}
		set
		{
			layerName = value;
			foreach (var renderer in GetComponents<Renderer>())
			{
				renderer.sortingLayerName = layerName;
			}
		}
	}

	public int OrderInLayer
	{
		get
		{
			return orderInLayer;
		}
		set
		{
			orderInLayer = value;
			foreach (var renderer in GetComponents<Renderer>())
			{
				renderer.sortingOrder = orderInLayer;
			}
		}
	}
}
//-----------------------------------------------------------------
public class SortingLayerAttribute: PropertyAttribute 
{

}
//-----------------------------------------------------------------
[CustomPropertyDrawer(typeof(SortingLayerAttribute))]
public class SortingLayerDrawer : PropertyDrawer 
{
	//-----------------------------------------------------------------
	private SerializedProperty sortinglayer = null;

	//-----------------------------------------------------------------
	public SerializedProperty SortingLayer
    {
        get 
		{
			if(sortinglayer == null) 
			{
				var tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
				sortinglayer = tagManager.FindProperty("m_SortingLayers");
			}
			return sortinglayer;
		}
	}
    //-----------------------------------------------------------------
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
		var List = AllSortingLayer;
		var selectedIndex = List.FindIndex(item => item.Equals(property.stringValue));
		if(selectedIndex == -1)
		{
			selectedIndex = List.FindIndex(item => item.Equals("Default"));
		}
		selectedIndex = EditorGUI.Popup(position, label.text, selectedIndex, List.ToArray());
		property.stringValue = List[selectedIndex];

	}
	//-----------------------------------------------------------------
	private List<string> AllSortingLayer 
	{
        get 
		{
			var layerNameList = new List<string>();
			for (int i = 0; i < SortingLayer.arraySize; i++) 
			{
				var tag = SortingLayer.GetArrayElementAtIndex(i);
				layerNameList.Add(tag.displayName);
			}
			return layerNameList;
		}
	}
}