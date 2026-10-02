using System;
using System.Collections.Generic;
using Helpers;
using SandBox.BoardGames.MissionLogics;
using SandBox.BoardGames.Pawns;

namespace SandBox.BoardGames.AI
{
	// Token: 0x02000107 RID: 263
	public class BoardGameAIPuluc : BoardGameAIBase
	{
		// Token: 0x06000D2F RID: 3375 RVA: 0x0005FD62 File Offset: 0x0005DF62
		public BoardGameAIPuluc(BoardGameHelper.AIDifficulty difficulty, MissionBoardGameLogic boardGameHandler)
			: base(difficulty, boardGameHandler)
		{
			this._board = base.BoardGameHandler.Board as BoardGamePuluc;
		}

		// Token: 0x06000D30 RID: 3376 RVA: 0x0005FD9C File Offset: 0x0005DF9C
		protected override void InitializeDifficulty()
		{
			switch (base.Difficulty)
			{
			case BoardGameHelper.AIDifficulty.Easy:
				this.MaxDepth = 3;
				return;
			case BoardGameHelper.AIDifficulty.Normal:
				this.MaxDepth = 5;
				return;
			case BoardGameHelper.AIDifficulty.Hard:
				this.MaxDepth = 7;
				return;
			default:
				return;
			}
		}

		// Token: 0x06000D31 RID: 3377 RVA: 0x0005FDDC File Offset: 0x0005DFDC
		public override Move CalculateMovementStageMove()
		{
			Move move;
			move.GoalTile = null;
			move.Unit = null;
			if (this._board.IsReady)
			{
				this.ExpectiMax(this.MaxDepth, BoardGameSide.AI, false, ref move);
			}
			if (!base.AbortRequested)
			{
				bool isValid = move.IsValid;
			}
			return move;
		}

		// Token: 0x06000D32 RID: 3378 RVA: 0x0005FE28 File Offset: 0x0005E028
		private float ExpectiMax(int depth, BoardGameSide side, bool chanceNode, ref Move bestMove)
		{
			float num;
			if (depth == 0)
			{
				num = (float)this.Evaluation();
				if (side == BoardGameSide.Player)
				{
					num = -num;
				}
			}
			else if (chanceNode)
			{
				num = 0f;
				for (int i = 0; i < 5; i++)
				{
					int lastDice = this._board.LastDice;
					this._board.ForceDice((i == 0) ? 5 : i);
					num += this._diceProbabilities[i] * this.ExpectiMax(depth - 1, side, false, ref bestMove);
					this._board.ForceDice(lastDice);
				}
			}
			else
			{
				BoardGamePuluc.BoardInformation boardInformation = this._board.TakeBoardSnapshot();
				List<List<Move>> list = this._board.CalculateAllValidMoves(side);
				if (this._board.HasMovesAvailable(ref list))
				{
					num = float.MinValue;
					using (List<List<Move>>.Enumerator enumerator = list.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							List<Move> list2 = enumerator.Current;
							if (list2 != null)
							{
								foreach (Move move in list2)
								{
									this._board.AIMakeMove(move);
									BoardGameSide boardGameSide = ((side == BoardGameSide.AI) ? BoardGameSide.Player : BoardGameSide.AI);
									float num2 = -this.ExpectiMax(depth - 1, boardGameSide, true, ref bestMove);
									this._board.UndoMove(ref boardInformation);
									if (num < num2)
									{
										num = num2;
										if (depth == this.MaxDepth)
										{
											bestMove = move;
										}
									}
								}
							}
						}
						return num;
					}
				}
				num = (float)this.Evaluation();
				if (side == BoardGameSide.Player)
				{
					num = -num;
				}
			}
			return num;
		}

		// Token: 0x06000D33 RID: 3379 RVA: 0x0005FFC0 File Offset: 0x0005E1C0
		private int Evaluation()
		{
			return 20 * (this._board.GetPlayerTwoUnitsAlive() - this._board.GetPlayerOneUnitsAlive()) + 5 * (this.GetUnitsBeingCaptured(true) - this.GetUnitsBeingCaptured(false)) + (this.GetUnitsInPlay(false) - this.GetUnitsInPlay(true));
		}

		// Token: 0x06000D34 RID: 3380 RVA: 0x00060000 File Offset: 0x0005E200
		private int GetUnitsInSpawn(bool playerOne)
		{
			int num = 0;
			using (List<PawnBase>.Enumerator enumerator = (playerOne ? this._board.PlayerOneUnits : this._board.PlayerTwoUnits).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (((PawnPuluc)enumerator.Current).IsInSpawn)
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x06000D35 RID: 3381 RVA: 0x00060074 File Offset: 0x0005E274
		private int GetUnitsBeingCaptured(bool playerOne)
		{
			int num = 0;
			using (List<PawnBase>.Enumerator enumerator = (playerOne ? this._board.PlayerOneUnits : this._board.PlayerTwoUnits).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!((PawnPuluc)enumerator.Current).IsTopPawn)
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x06000D36 RID: 3382 RVA: 0x000600E8 File Offset: 0x0005E2E8
		private int GetUnitsInPlay(bool playerOne)
		{
			int num = 0;
			foreach (PawnBase pawnBase in (playerOne ? this._board.PlayerOneUnits : this._board.PlayerTwoUnits))
			{
				PawnPuluc pawnPuluc = (PawnPuluc)pawnBase;
				if (pawnPuluc.InPlay && pawnPuluc.IsTopPawn)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x040005B5 RID: 1461
		private readonly BoardGamePuluc _board;

		// Token: 0x040005B6 RID: 1462
		private readonly float[] _diceProbabilities = new float[] { 0.0625f, 0.25f, 0.375f, 0.25f, 0.0625f };
	}
}
