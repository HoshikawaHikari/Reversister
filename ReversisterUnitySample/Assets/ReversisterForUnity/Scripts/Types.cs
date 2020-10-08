
namespace HoshihaLab.Reversister
{
	public enum CellType
	{
		None,
		White,
		Brack
	}


	public enum GameStateType
	{
		BrackTurn,
		WhiteTurn,
		BrackWin,
		WhiteWin,
		Draw,
	}


	public enum LineDirType
	{
		Up,
		Down,
		Right,
		Left,
		UpRight,
		UpLeft,
		DownRight,
		DownLeft,
	}


	public static class CellTypeHelper
	{
		public static string GetCellText(this CellType type)
		{
			switch (type)
			{
				case CellType.None:
					return "";
				case CellType.White:
					return "○";
				case CellType.Brack:
					return "●";
			}
			return "";
		}

		/// <summary>
		/// 逆の色タイプを返す
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static CellType GetOpponent(this CellType type)
		{
			if (type == CellType.White) { return CellType.Brack; }
			if (type == CellType.Brack) { return CellType.White; }
			return type;
		}
	}


	public static class GameStateTypeHelper
	{
		public static string GetText(this GameStateType type)
		{
			switch (type)
			{
				case GameStateType.BrackTurn:
					return "黒の手番です";
				case GameStateType.WhiteTurn:
					return "白の手番です";
				case GameStateType.BrackWin:
					return "黒の勝利です";
				case GameStateType.WhiteWin:
					return "白の勝利です";
				case GameStateType.Draw:
					return "引き分けです";
			}
			return "";
		}

		public static CellType ToCellType(this GameStateType type)
		{
			switch (type)
			{
				case GameStateType.BrackTurn:
					return CellType.Brack;
				case GameStateType.WhiteTurn:
					return CellType.White;
				case GameStateType.BrackWin:
					return CellType.Brack;
				case GameStateType.WhiteWin:
					return CellType.White;
				case GameStateType.Draw:
					return CellType.None;
			}
			return CellType.None;
		}
	}


	public static class LineDirTypeHelper
	{
		public static XY GetIndexValue(this LineDirType type)
		{
			switch (type)
			{
				case LineDirType.Up:
					return new XY(0, -1);
				case LineDirType.Down:
					return new XY(0, 1);
				case LineDirType.Right:
					return new XY(1, 0);
				case LineDirType.Left:
					return new XY(-1, 0);
				case LineDirType.UpRight:
					return new XY(1, -1);
				case LineDirType.UpLeft:
					return new XY(-1, -1);
				case LineDirType.DownRight:
					return new XY(-1, 1);
				case LineDirType.DownLeft:
					return new XY(1, 1);
			}
			return null;
		}
	}
}