using System;
using UnityEngine;

public class CellList : MonoBehaviour
{
	public event Action<CellListItem> OnSelectedChanged;


	public CellListItem this[int index]
	{
		get { return GetItem(index); }
	}


	void Start()
	{
		var children = transform.GetComponentsInChildren<CellListItem>();
		foreach (var child in children)
		{
			child.OnClick += OnSelectedCellChanged;
		}
	}


	public void AddItem(CellListItem item)
	{
		var s = item.transform.localScale;
		item.transform.SetParent(transform);
		item.transform.localScale = s;
		item.OnClick += OnSelectedCellChanged;
	}


	public CellListItem GetItem(int index)
	{
		return transform.GetChild(index).GetComponent<CellListItem>();
	}


	private void OnSelectedCellChanged(CellListItem item)
	{
		OnSelectedChanged?.Invoke(item);
	}
}
