
namespace HoshihaLab.Reversister
{
	public abstract class ReversiAIBase
	{
		public Reversi Reversi { get; private set; }
		public CellType Type { get; private set; }


		public void StartGame(Reversi reversi, CellType type)
		{
			Type = type;
			Reversi = reversi;
		}

		public abstract XY Calc();
	}

	public class SimpleReversiAI : ReversiAIBase
	{
		public override XY Calc()
		{
			var flips = Reversi.CountFlip(Type);

			if (flips.Count <= 0)
			{
				Reversi.Pass();
				return null;
			}

			// 一番多くめくれる場所
			int max = 0;
			XY maxXY = null;
			foreach (var flip in flips)
			{
				int flipCount = Reversi.CountFlipCell(flip, Type);
				if (flipCount > max)
				{
					maxXY = flip;
				}
			}

			return maxXY;
		}
	}

}
