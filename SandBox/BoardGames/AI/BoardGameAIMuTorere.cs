using System;
using System.Collections.Generic;
using Helpers;
using SandBox.BoardGames.MissionLogics;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace SandBox.BoardGames.AI
{
	// Token: 0x02000106 RID: 262
	public class BoardGameAIMuTorere : BoardGameAIBase
	{
		// Token: 0x06000D29 RID: 3369 RVA: 0x0005FA22 File Offset: 0x0005DC22
		public BoardGameAIMuTorere(BoardGameHelper.AIDifficulty difficulty, MissionBoardGameLogic boardGameHandler)
			: base(difficulty, boardGameHandler)
		{
			this._board = base.BoardGameHandler.Board as BoardGameMuTorere;
		}

		// Token: 0x06000D2A RID: 3370 RVA: 0x0005FA44 File Offset: 0x0005DC44
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

		// Token: 0x06000D2B RID: 3371 RVA: 0x0005FA84 File Offset: 0x0005DC84
		public override Move CalculateMovementStageMove()
		{
			Move move;
			move.GoalTile = null;
			move.Unit = null;
			if (this._board.IsReady)
			{
				List<List<Move>> list = this._board.CalculateAllValidMoves(BoardGameSide.AI);
				BoardGameMuTorere.BoardInformation boardInformation = this._board.TakePawnsSnapshot();
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
							int num2 = -this.NegaMax(this.MaxDepth, -1);
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

		// Token: 0x06000D2C RID: 3372 RVA: 0x0005FBB0 File Offset: 0x0005DDB0
		private int NegaMax(int depth, int color)
		{
			int num = int.MinValue;
			if (depth == 0)
			{
				return color * this.Evaluation() * ((this._board.PlayerWhoStarted == PlayerTurn.PlayerOne) ? 1 : (-1));
			}
			BoardGameMuTorere.BoardInformation boardInformation = this._board.TakePawnsSnapshot();
			List<List<Move>> list = this._board.CalculateAllValidMoves((color == 1) ? BoardGameSide.AI : BoardGameSide.Player);
			if (!this._board.HasMovesAvailable(ref list))
			{
				return color * this.Evaluation();
			}
			foreach (List<Move> list2 in list)
			{
				foreach (Move move in list2)
				{
					this._board.AIMakeMove(move);
					num = MathF.Max(num, -this.NegaMax(depth - 1, -color));
					this._board.UndoMove(ref boardInformation);
				}
			}
			return num;
		}

		// Token: 0x06000D2D RID: 3373 RVA: 0x0005FCB8 File Offset: 0x0005DEB8
		private int Evaluation()
		{
			float num = MBRandom.RandomFloat;
			switch (base.Difficulty)
			{
			case BoardGameHelper.AIDifficulty.Easy:
				num = num * 2f - 1f;
				break;
			case BoardGameHelper.AIDifficulty.Normal:
				num = num * 1.7f - 0.7f;
				break;
			case BoardGameHelper.AIDifficulty.Hard:
				num = num * 1.4f - 0.4f;
				break;
			}
			return (int)(num * 100f * (float)(this.CanMove(false) - this.CanMove(true)));
		}

		// Token: 0x06000D2E RID: 3374 RVA: 0x0005FD30 File Offset: 0x0005DF30
		private int CanMove(bool playerOne)
		{
			List<List<Move>> list = this._board.CalculateAllValidMoves(playerOne ? BoardGameSide.Player : BoardGameSide.AI);
			if (!this._board.HasMovesAvailable(ref list))
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x040005B4 RID: 1460
		private readonly BoardGameMuTorere _board;
	}
}
