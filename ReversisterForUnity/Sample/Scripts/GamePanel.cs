using HoshihaLab.Reversister;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : MonoBehaviour
{
	[SerializeField]
	private CellList boardList = null;

	[SerializeField]
	private GameObject listCellPrefab = null;

	[SerializeField]
	private Text gameStateText = null;

	[SerializeField]
	private Dialog passPanel = null;

	[SerializeField]
	private Dialog finishPanel = null;


	private const int CellMax = 8;
	private readonly Color NormalCellColor = new Color32(0, 120, 20, 255);
	private readonly Color HighlightCellColor = new Color32(0, 150, 50, 255);

	private List<List<CellListItem>> grid = null;

	private Reversi reversi;
	private CellType turn = CellType.White;

	private SimpleAI ai;
	private CellType m_MyType;


	void Start()
	{
		reversi = new Reversi(CellMax);
		reversi.onChangeGameState += OnChangeGameState;

		gameStateText.text = reversi.GetGameState().GetText();
		turn = reversi.GetGameState().ToCellType();
		m_MyType = CellType.Brack;
		
		// AI
		ai = new GameObject("AI", typeof(SimpleAI)).GetComponent<SimpleAI>();
		ai.StartGame(reversi, m_MyType.GetOpponent());
		ai.OnFlip += OnAIFlip;
		reversi.onChangeGameState += ai.OnChangeGameState;

		// 盤面作成
		CreateBoard();
		boardList.OnSelectedChanged += OnSelectedCell;

		// 置ける場所をハイライト
		HightlightCell(turn);
		
		passPanel.OnClick += OnPassPanelClick;
		finishPanel.OnClick += OnFinishPanelClick;
	}


	/// <summary>
	/// ゲームに変化があった時の処理
	/// </summary>
	/// <param name="type"></param>
	private void OnChangeGameState(GameStateType type)
	{
		turn = type.ToCellType();
		gameStateText.text = type.GetText();

		// 盤面の更新
		BoardUpdate();

		// ゲーム終了なら結果表示
		if (reversi.IsEnd)
		{
			var w = reversi.CountCell(CellType.White);
			var b = reversi.CountCell(CellType.Brack);

			finishPanel.gameObject.SetActive(true);
			finishPanel.InfoText.text = "白:" + w + " vs " + "黒:" + b + "\n" + type.GetText();
			
			return;
		}

		// 自分のターンの時
		if (turn == m_MyType)
		{
			// 自分が置ける場所をハイライト
			var flips = HightlightCell(turn);

			// 置ける場所が無い場合パス確認ダイアログの表示
			if (flips.Count <= 0)
			{
				passPanel.gameObject.SetActive(true);
			}
		}
	}


	/// <summary>
	/// 置ける場所をハイライトする
	/// </summary>
	/// <param name="turn"></param>
	/// <returns>置ける場所の一覧</returns>
	private List<XY> HightlightCell(CellType turn)
	{
		var flips = reversi.CountFlip(turn);
		foreach (var flip in flips)
		{
			var cell = GetCell(flip);
			cell.Button.enabled = true;
			cell.Button.image.color = HighlightCellColor;
		}
		return flips;
	}


	/// <summary>
	/// パス確認ダイアログのボタンが押された時の処理
	/// </summary>
	private void OnPassPanelClick()
	{
		reversi.Pass();
		passPanel.gameObject.SetActive(false);
	}


	/// <summary>
	/// ゲーム終了ダイアログのボタンが押された時の処理
	/// </summary>
	private void OnFinishPanelClick()
	{
		//UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScene");
		finishPanel.gameObject.SetActive(false);
	}


	/// <summary>
	/// 置く場所を選択した時の処理
	/// </summary>
	/// <param name="item"></param>
	private void OnSelectedCell(CellListItem item)
	{
		XY xy = FindCellItem(item);
		reversi.Flip(xy, turn);
	}


	/// <summary>
	/// 盤面の作成
	/// </summary>
	private void CreateBoard()
	{
		grid = new List<List<CellListItem>>();
		for (int h = 0; h < CellMax; h++)
		{
			grid.Add(new List<CellListItem>());
			for (int w = 0; w < CellMax; w++)
			{
				var go = Instantiate(listCellPrefab);
				go.SetActive(true);
				var item = go.GetComponent<CellListItem>();

				item.Text.text = reversi.GetCell(w, h).Type.GetCellText();
				item.Button.image.color = NormalCellColor;
				item.Button.enabled = false;

				boardList.AddItem(item);
				grid[h].Add(item);
			}
		}
	}


	/// <summary>
	/// 盤面を更新
	/// </summary>
	private void BoardUpdate()
	{
		for (int y = 0; y < CellMax; y++)
		{
			for (int x = 0; x < CellMax; x++)
			{
				var item = GetCell(x, y);
				var cell = reversi.GetCell(x, y);
				item.Text.text = cell.Type.GetCellText();

				item.Button.enabled = false;
				item.Button.image.color = NormalCellColor;
			}
		}
	}


	private void OnAIFlip(int flipCount)
	{
		Debug.Log("AIが置いた");
	}


	private CellListItem GetCell(int x, int y)
	{
		return grid[y][x];
	}


	private CellListItem GetCell(XY xy)
	{
		return GetCell(xy.X, xy.Y);
	}


	private XY FindCellItem(CellListItem item)
	{
		for (int y = 0; y < CellMax; y++)
		{
			for (int x = 0; x < CellMax; x++)
			{
				if (GetCell(x, y) == item)
				{
					return new XY(x, y);
				}
			}
		}
		return null;
	}

}
