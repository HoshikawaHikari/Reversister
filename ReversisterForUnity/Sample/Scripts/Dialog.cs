using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;


public class Dialog : MonoBehaviour
{
	[SerializeField]
	private Text titleText = null;

	[SerializeField]
	private Text infoText = null;

	[SerializeField]
	private Button button = null;


	public event Action OnClick;

	public Button Button => button;

	public Text TitleText => titleText;
	public Text InfoText => infoText;


	void Awake()
	{
		if (button == null)
		{
			button = GetComponentInChildren<Button>();
		}
		button.onClick.AddListener(OnButtonClick);
	}
	

	private void OnButtonClick()
	{
		OnClick?.Invoke();
	}
}
