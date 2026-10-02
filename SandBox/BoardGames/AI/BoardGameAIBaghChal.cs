using System;
using System.Collections.Generic;
using Helpers;
using SandBox.BoardGames.MissionLogics;
using SandBox.BoardGames.Pawns;
using SandBox.BoardGames.Tiles;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace SandBox.BoardGames.AI
{
	// Token: 0x02000103 RID: 259
	public class BoardGameAIBaghChal : BoardGameAIBase
	{
		// Token: 0x06000D00 RID: 3328 RVA: 0x0005ECD0 File Offset: 0x0005CED0
		public BoardGameAIBaghChal(BoardGameHelper.AIDifficulty difficulty, MissionBoardGameLogic boardGameHandler)
			: base(difficulty, boardGameHandler)
		{
			this._board = base.BoardGameHandler.Board as BoardGameBaghChal;
		}

		// Token: 0x06000D01 RID: 3329 RVA: 0x0005ECF0 File Offset: 0x0005CEF0
		protected override void InitializeDifficulty()
		{
			switch (base.Difficulty)
			{
			case BoardGameHelper.AIDifficulty.Easy:
				this.MaxDepth = 3;
				return;
			case BoardGameHelper.AIDifficulty.Normal:
				this.MaxDepth = 4;
				return;
			case BoardGameHelper.AIDifficulty.Hard:
				this.MaxDepth = 5;
				return;
			default:
				return;
			}
		}

		// Token: 0x06000D02 RID: 3330 RVA: 0x0005ED30 File Offset: 0x0005CF30
		public override Move CalculateMovementStageMove()
		{
			Move move;
			move.GoalTile = null;
			move.Unit = null;
			if (this._board.IsReady)
			{
				List<List<Move>> list = this._board.CalculateAllValidMoves(BoardGameSide.AI);
				BoardGameBaghChal.BoardInformation boardInformation = this._board.TakeBoardSnapshot();
				if (this._board.HasMovesAvailable(ref list))
				{
					int num = int.MinValue;
					foreach (List<Move> list2 in list)
					{
						if (base.AbortRequested)
						{
							break;
						}
						foreach (Move move2 in list2)
						{
							if (base.AbortRequested)
							{
								break;
							}
							this._board.AIMakeMove(move2);
							int num2 = -this.NegaMax(this.MaxDepth, -1, -2147483647, int.MaxValue);
							this._board.UndoMove(ref boardInformation);
							if (num2 > num)
							{
								move = move2;
								num = num2;
							}
						}
					}
				}
			}
			if (!base.AbortRequested)
			{
				bool isValid = move.IsValid;
			}
			return move;
		}

		// Token: 0x06000D03 RID: 3331 RVA: 0x0005EE6C File Offset: 0x0005D06C
		public override Move CalculatePreMovementStageMove()
		{
			return this.CalculateMovementStageMove();
		}

		// Token: 0x06000D04 RID: 3332 RVA: 0x0005EE74 File Offset: 0x0005D074
		private int NegaMax(int depth, int color, int alpha, int beta)
		{
			if (depth == 0)
			{
				return color * this.Evaluation() * ((this._board.PlayerWhoStarted == PlayerTurn.PlayerOne) ? 1 : (-1));
			}
			BoardGameBaghChal.BoardInformation boardInformation = this._board.TakeBoardSnapshot();
			if (color == ((this._board.PlayerWhoStarted == PlayerTurn.PlayerOne) ? (-1) : 1) && this._board.GetANonePlacedGoat() != null)
			{
				for (int i = 0; i < this._board.TileCount; i++)
				{
					TileBase tileBase = this._board.Tiles[i];
					if (tileBase.PawnOnTile == null)
					{
						Move move = new Move(this._board.GetANonePlacedGoat(), tileBase);
						this._board.AIMakeMove(move);
						int num = -this.NegaMax(depth - 1, -color, -beta, -alpha);
						this._board.UndoMove(ref boardInformation);
						if (num >= beta)
						{
							return num;
						}
						alpha = MathF.Max(num, alpha);
					}
				}
			}
			else
			{
				List<List<Move>> list = this._board.CalculateAllValidMoves((color == 1) ? BoardGameSide.AI : BoardGameSide.Player);
				if (!this._board.HasMovesAvailable(ref list))
				{
					return color * this.Evaluation() * ((this._board.PlayerWhoStarted == PlayerTurn.PlayerOne) ? 1 : (-1));
				}
				foreach (List<Move> list2 in list)
				{
					foreach (Move move2 in list2)
					{
						this._board.AIMakeMove(move2);
						int num2 = -this.NegaMax(depth - 1, -color, -beta, -alpha);
						this._board.UndoMove(ref boardInformation);
						if (num2 >= beta)
						{
							return num2;
						}
						alpha = MathF.Max(num2, alpha);
					}
				}
				return alpha;
			}
			return alpha;
		}

		// Token: 0x06000D05 RID: 3333 RVA: 0x0005F050 File Offset: 0x0005D250
		private int Evaluation()
		{
			float num = MBRandom.RandomFloat;
			switch (base.Difficulty)
			{
			case BoardGameHelper.AIDifficulty.Easy:
				num = num * 0.7f + 0.5f;
				break;
			case BoardGameHelper.AIDifficulty.Normal:
				num = num * 0.5f + 0.65f;
				break;
			case BoardGameHelper.AIDifficulty.Hard:
				num = num * 0.35f + 0.75f;
				break;
			}
			List<List<Move>> list = this._board.CalculateAllValidMoves((this._board.PlayerWhoStarted == PlayerTurn.PlayerOne) ? BoardGameSide.AI : BoardGameSide.Player);
			int totalMovesAvailable = this._board.GetTotalMovesAvailable(ref list);
			return (int)((float)(100 * -(float)this.GetTigersStuck() + 50 * this.GetGoatsCaptured() + totalMovesAvailable + this.GetCombinedDistanceBetweenTigers()) * num);
		}

		// Token: 0x06000D06 RID: 3334 RVA: 0x0005F0F8 File Offset: 0x0005D2F8
		private int GetTigersStuck()
		{
			int num = 0;
			foreach (PawnBase pawnBase in ((this._board.PlayerWhoStarted == PlayerTurn.PlayerOne) ? this._board.PlayerTwoUnits : this._board.PlayerOneUnits))
			{
				PawnBaghChal pawnBaghChal = (PawnBaghChal)pawnBase;
				if (this._board.CalculateValidMoves(pawnBaghChal).Count == 0)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06000D07 RID: 3335 RVA: 0x0005F184 File Offset: 0x0005D384
		private int GetGoatsCaptured()
		{
			int num = 0;
			using (List<PawnBase>.Enumerator enumerator = ((this._board.PlayerWhoStarted == PlayerTurn.PlayerOne) ? this._board.PlayerOneUnits : this._board.PlayerTwoUnits).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (((PawnBaghChal)enumerator.Current).Captured)
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x06000D08 RID: 3336 RVA: 0x0005F204 File Offset: 0x0005D404
		private int GetCombinedDistanceBetweenTigers()
		{
			int num = 0;
			foreach (PawnBase pawnBase in ((this._board.PlayerWhoStarted == PlayerTurn.PlayerOne) ? this._board.PlayerTwoUnits : this._board.PlayerOneUnits))
			{
				PawnBaghChal pawnBaghChal = (PawnBaghChal)pawnBase;
				foreach (PawnBase pawnBase2 in ((this._board.PlayerWhoStarted == PlayerTurn.PlayerOne) ? this._board.PlayerTwoUnits : this._board.PlayerOneUnits))
				{
					PawnBaghChal pawnBaghChal2 = (PawnBaghChal)pawnBase2;
					if (pawnBaghChal != pawnBaghChal2)
					{
						num += MathF.Abs(pawnBaghChal.X - pawnBaghChal2.X) + MathF.Abs(pawnBaghChal.Y + pawnBaghChal2.Y);
					}
				}
			}
			return num;
		}

		// Token: 0x040005A8 RID: 1448
		private readonly BoardGameBaghChal _board;
	}
}
