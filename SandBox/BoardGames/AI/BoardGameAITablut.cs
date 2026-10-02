using System;
using Helpers;
using SandBox.BoardGames.MissionLogics;

namespace SandBox.BoardGames.AI
{
	// Token: 0x02000109 RID: 265
	public class BoardGameAITablut : BoardGameAIBase
	{
		// Token: 0x06000D41 RID: 3393 RVA: 0x000608A2 File Offset: 0x0005EAA2
		public BoardGameAITablut(BoardGameHelper.AIDifficulty difficulty, MissionBoardGameLogic boardGameHandler)
			: base(difficulty, boardGameHandler)
		{
		}

		// Token: 0x06000D42 RID: 3394 RVA: 0x000608AC File Offset: 0x0005EAAC
		public override void Initialize()
		{
			base.Initialize();
			BoardGameAITablut.Board = base.BoardGameHandler.Board as BoardGameTablut;
		}

		// Token: 0x06000D43 RID: 3395 RVA: 0x000608C9 File Offset: 0x0005EAC9
		public override void OnSetGameOver()
		{
			base.OnSetGameOver();
			BoardGameAITablut.Board = null;
		}

		// Token: 0x06000D44 RID: 3396 RVA: 0x000608D8 File Offset: 0x0005EAD8
		public override Move CalculateMovementStageMove()
		{
			Move openingMove;
			openingMove.GoalTile = null;
			openingMove.Unit = null;
			if (BoardGameAITablut.Board.IsReady)
			{
				BoardGameTablut.BoardInformation boardInformation = BoardGameAITablut.Board.TakeBoardSnapshot();
				TreeNodeTablut treeNodeTablut = TreeNodeTablut.CreateTreeAndReturnRootNode(boardInformation, this.MaxDepth);
				int num = 0;
				while (num < this._sampleCount && !base.AbortRequested)
				{
					treeNodeTablut.SelectAction();
					num++;
				}
				if (!base.AbortRequested)
				{
					BoardGameAITablut.Board.UndoMove(ref boardInformation);
					TreeNodeTablut childWithBestScore = treeNodeTablut.GetChildWithBestScore();
					if (childWithBestScore != null)
					{
						openingMove = childWithBestScore.OpeningMove;
					}
				}
			}
			if (!base.AbortRequested)
			{
				bool isValid = openingMove.IsValid;
			}
			return openingMove;
		}

		// Token: 0x06000D45 RID: 3397 RVA: 0x00060974 File Offset: 0x0005EB74
		protected override void InitializeDifficulty()
		{
			switch (base.Difficulty)
			{
			case BoardGameHelper.AIDifficulty.Easy:
				this.MaxDepth = 3;
				this._sampleCount = 30000;
				return;
			case BoardGameHelper.AIDifficulty.Normal:
				this.MaxDepth = 4;
				this._sampleCount = 47000;
				return;
			case BoardGameHelper.AIDifficulty.Hard:
				this.MaxDepth = 5;
				this._sampleCount = 64000;
				return;
			default:
				return;
			}
		}

		// Token: 0x040005B9 RID: 1465
		public static BoardGameTablut Board;

		// Token: 0x040005BA RID: 1466
		private int _sampleCount;
	}
}
