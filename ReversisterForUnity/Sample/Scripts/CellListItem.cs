using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellListItem : MonoBehaviour
{
	[SerializeField]
	private Button button = null;

	[SerializeField]
	private Text text = null;

	[SerializeField]
	private Image image = null;


	public Button Button => button;
	public Text Text => text;
	public Image Image => image;


	public event Action<CellListItem> OnClick;


	void Awake()
	{
		button.onClick.AddListener(OnButtonClick);
	}


	private void OnButtonClick()
	{
		OnClick?.Invoke(this);
	}
}
