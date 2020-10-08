using System;
using System.Collections.Generic;

namespace HoshihaLab.Reversister
{
	public class Reversi
	{
		private List<List<Cell>> m_Grid;
		private int m_CellMax;

		private GameStateType m_GameState = GameStateType.BrackTurn;


		public event Action<GameStateType> onChangeGameState = (state) => { };


		public GameStateType GetGameState()
		{
			return m_GameState;
		}

		public bool IsEnd
		{
			get { return (m_GameState == GameStateType.BrackWin || m_GameState == GameStateType.WhiteWin || m_GameState == GameStateType.Draw); }
		}



		public Reversi(int maxCell = 8)
		{
			m_CellMax = maxCell;
			m_Grid = new List<List<Cell>>();
			for (int h = 0; h < m_CellMax; h++)
			{
				m_Grid.Add(new List<Cell>());
				for (int w = 0; w < m_CellMax; w++)
				{
					m_Grid[h].Add(new Cell());
				}
			}
			Reset();
		}


		public void Reset()
		{
			ForEach((cell) => { cell.Type = CellType.None; });

			int center = (m_CellMax / 2);
			GetCell(center + 0, center + 0).Type = CellType.White;
			GetCell(center - 1, center - 1).Type = CellType.White;
			GetCell(center - 0, center - 1).Type = CellType.Brack;
			GetCell(center - 1, center - 0).Type = CellType.Brack;
		}


		public Cell GetCell(int x, int y)
		{
			if (y < 0 || y >= m_Grid.Count ||
				x < 0 || x >= m_Grid[y].Count)
			{
				return null;
			}
			return m_Grid[y][x];
		}


		public Cell GetCell(XY xy)
		{
			return GetCell(xy.X, xy.Y);
		}


		/// <summary>
		/// マスをカウント
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public int CountCell(CellType type)
		{
			int count = 0;
			ForEach((cell) => { if (cell.Type == type) { count++; } });
			return count;
		}


		/// <summary>
		/// 置ける場所を返す
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public List<XY> CountFlip(CellType type)
		{
			List<XY> flips = new List<XY>();
			ForEach((x, y) =>
			{
				var xy = new XY(x, y);
				if (CanFlip(xy, type)) { flips.Add(xy); }
			});
			return flips;
		}


		/// <summary>
		/// どこかに置けるかどうか
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public bool CanPlay(CellType type)
		{
			return (CountFlip(type).Count > 0);
		}


		/// <summary>
		/// ゲーム終了かどうか
		/// </summary>
		/// <returns></returns>
		public bool CheckEnd()
		{
			if (CountCell(CellType.None) <= 0)
			{
				return true;
			}

			return !(CanPlay(CellType.White) || CanPlay(CellType.Brack));
		}


		/// <summary>
		/// パスする
		/// </summary>
		public void Pass()
		{
			UpdateGameState();
		}


		/// <summary>
		/// 置く
		/// </summary>
		/// <param name="xy"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		public int Flip(XY xy, CellType type)
		{
			if (!CanFlip(xy, type))
			{
				return 0;
			}

			GetCell(xy).Type = type;

			int count = 0;
			foreach (var value in Enum.GetValues(typeof(LineDirType)))
			{
				var dir = (LineDirType)value;
				count += FlipLine(xy, type, dir);
			}

			UpdateGameState();
			return count;
		}


		/// <summary>
		/// そこに置いた時にめくれる数を計算
		/// </summary>
		/// <param name="xy"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		public int CountFlipCell(XY xy, CellType type)
		{
			//// このセルが空かチェック
			//if (GetCell(xy).Type != CellType.None)
			//{
			//	return 0;
			//}

			//// 全方位チェック
			//int count = 0;
			//foreach (var value in Enum.GetValues(typeof(LineDirType)))
			//{
			//	var dir = (LineDirType)value;
			//	count += CanFlipLine(xy, type, dir);
			//}
			//return count;
			return FindFlipCell(xy, type).Count;
		}


		/// <summary>
		/// そこに置いたときにめくれる場所を検索
		/// </summary>
		/// <param name="xy"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		public List<XY> FindFlipCell(XY xy, CellType type)
		{
			// このセルが空かチェック
			if (GetCell(xy).Type != CellType.None)
			{
				return new List<XY>();
			}

			// 全方位チェック
			List<XY> cells = new List<XY>();
			foreach (var value in Enum.GetValues(typeof(LineDirType)))
			{
				var dir = (LineDirType)value;
				var flips = FindFlipLine(xy, type, dir, new List<XY>());
				if (flips.Count > 0)
				{
					cells.AddRange(flips);
				}
			}

			return cells;
		}


		/// <summary>
		/// そこに置けるかどうか
		/// </summary>
		/// <returns></returns>
		public bool CanFlip(XY xy, CellType type)
		{
			// このセルが空かチェック
			if (GetCell(xy).Type != CellType.None)
			{
				return false;
			}

			// 全方位チェック
			foreach (var value in Enum.GetValues(typeof(LineDirType)))
			{
				var dir = (LineDirType)value;
				if (CanFlipLine(xy, type, dir) > 0)
				{
					return true;
				}
			}

			return false;
		}


		private int FlipLine(XY xy, CellType type, LineDirType dir)
		{
			var count = CanFlipLine(xy, type, dir);
			if (count <= 0)
			{
				return 0;
			}

			XY add = dir.GetIndexValue();
			for (int i = 0; i < count; i++)
			{
				XY target = new XY(xy.X + add.X + (add.X * i), xy.Y + add.Y + (add.Y * i));
				var cell = GetCell(target);
				cell.Type = cell.Type.GetOpponent();
			}
			return count;
		}


		private int CanFlipLine(XY xy, CellType type, LineDirType dir, int count = 0)
		{
			XY add = dir.GetIndexValue();
			XY target = new XY(xy.X + add.X, xy.Y + add.Y);
			var cell = GetCell(target);

			// 場外なら無し
			if (cell == null)
			{
				return 0;
			}

			// 空なら無し
			if (cell.Type == CellType.None)
			{
				return 0;
			}

			// 自分と同じ色だったら、そこまでのカウント
			if (cell.Type == type)
			{
				return count;
			}

			// それ以外の場合(相手の色だった場合)
			// カウントを追加して検索を続行
			return CanFlipLine(target, type, dir, ++count);
		}


		private List<XY> FindFlipLine(XY xy, CellType type, LineDirType dir, List<XY> cells)
		{
			XY add = dir.GetIndexValue();
			XY target = new XY(xy.X + add.X, xy.Y + add.Y);
			var cell = GetCell(target);

			// 場外なら無し
			if (cell == null)
			{
				return new List<XY>();
			}

			// 空なら無し
			if (cell.Type == CellType.None)
			{
				return new List<XY>();
			}

			// 自分と同じ色だったら、そこまでのカウント
			if (cell.Type == type)
			{
				return cells;
			}

			// それ以外の場合(相手の色だった場合)
			// カウントを追加して検索を続行
			if (cells == null) { cells = new List<XY>(); }
			cells.Add(target);
			return FindFlipLine(target, type, dir, cells);
		}


		private void UpdateGameState()
		{
			var state = m_GameState;

			if (CheckEnd())
			{
				var w = CountCell(CellType.White);
				var b = CountCell(CellType.Brack);

				state =
					(w > b) ? GameStateType.WhiteWin :
					(w < b) ? GameStateType.BrackWin :
					GameStateType.Draw;
			}
			else
			{
				state =
					(m_GameState == GameStateType.WhiteTurn) ? GameStateType.BrackTurn :
					(m_GameState == GameStateType.BrackTurn) ? GameStateType.WhiteTurn :
					m_GameState;

			}

			if (state != m_GameState)
			{
				m_GameState = state;
				onChangeGameState(m_GameState);
			}
		}


		private void ForEach(Action<Cell> action)
		{
			ForEach((x, y) => { action(GetCell(x, y)); });
		}

		private void ForEach(Action<int, int> action)
		{
			for (int y = 0; y < m_CellMax; y++)
			{
				for (int x = 0; x < m_CellMax; x++)
				{
					action(x, y);
				}
			}
		}

	}
}