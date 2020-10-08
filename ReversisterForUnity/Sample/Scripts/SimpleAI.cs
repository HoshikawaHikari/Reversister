using System;
using System.Collections;
using UnityEngine;
using HoshihaLab.Reversister;

public class SimpleAI : MonoBehaviour
{
	SimpleReversiAI ai = new SimpleReversiAI();

	public event Action<int> OnFlip = (flipCount) => { };


	public void StartGame(Reversi reversi, CellType type)
	{
		ai.StartGame(reversi, type);
	}


	public void OnChangeGameState(GameStateType type)
	{
		if (ai.Reversi.IsEnd)
		{
			return;
		}

		if (type.ToCellType() != ai.Type)
		{
			return;
		}

		StartCoroutine(Calc());
	}


	IEnumerator Calc()
	{
		XY xy = ai.Calc();

		yield return new WaitForSeconds(1);

		if (xy != null)
		{
			int flipCount = ai.Reversi.Flip(xy, ai.Type);
			if (flipCount > 0)
			{
				OnFlip(flipCount);
			}
		}

		yield break;
	}
}
