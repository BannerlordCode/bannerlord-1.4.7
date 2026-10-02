using System;
using System.Collections.Generic;
using System.Linq;
using SandBox.BoardGames.Pawns;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace SandBox.BoardGames.AI
{
	// Token: 0x0200010A RID: 266
	public class TreeNodeTablut
	{
		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06000D46 RID: 3398 RVA: 0x000609D3 File Offset: 0x0005EBD3
		// (set) Token: 0x06000D47 RID: 3399 RVA: 0x000609DB File Offset: 0x0005EBDB
		public Move OpeningMove { get; private set; }

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06000D48 RID: 3400 RVA: 0x000609E4 File Offset: 0x0005EBE4
		private bool IsLeaf
		{
			get
			{
				return this._children == null;
			}
		}

		// Token: 0x06000D49 RID: 3401 RVA: 0x000609EF File Offset: 0x0005EBEF
		public TreeNodeTablut(BoardGameSide lastTurnIsPlayedBy, int depth)
		{
			this._lastTurnIsPlayedBy = lastTurnIsPlayedBy;
			this._depth = depth;
		}

		// Token: 0x06000D4A RID: 3402 RVA: 0x00060A05 File Offset: 0x0005EC05
		public static TreeNodeTablut CreateTreeAndReturnRootNode(BoardGameTablut.BoardInformation initialBoardState, int maxDepth)
		{
			TreeNodeTablut.MaxDepth = maxDepth;
			return new TreeNodeTablut(BoardGameSide.Player, 0)
			{
				_boardState = initialBoardState
			};
		}

		// Token: 0x06000D4B RID: 3403 RVA: 0x00060A1C File Offset: 0x0005EC1C
		public TreeNodeTablut GetChildWithBestScore()
		{
			TreeNodeTablut treeNodeTablut = null;
			if (!this.IsLeaf)
			{
				float num = float.MinValue;
				foreach (TreeNodeTablut treeNodeTablut2 in this._children)
				{
					if (treeNodeTablut2._visits > 0)
					{
						float num2 = (float)treeNodeTablut2._wins / (float)treeNodeTablut2._visits;
						if (!treeNodeTablut2.IsLeaf)
						{
							float num3 = 0f;
							foreach (TreeNodeTablut treeNodeTablut3 in treeNodeTablut2._children)
							{
								if (treeNodeTablut3._visits > 0)
								{
									float num4 = (float)treeNodeTablut3._wins / (float)treeNodeTablut3._visits;
									if (num4 > num3)
									{
										num3 = num4;
									}
								}
							}
							num2 *= 1f - num3;
						}
						if (num2 > num)
						{
							treeNodeTablut = treeNodeTablut2;
							num = num2;
						}
					}
				}
			}
			return treeNodeTablut;
		}

		// Token: 0x06000D4C RID: 3404 RVA: 0x00060B2C File Offset: 0x0005ED2C
		public void SelectAction()
		{
			TreeNodeTablut treeNodeTablut = this;
			while (!treeNodeTablut.IsLeaf)
			{
				treeNodeTablut = treeNodeTablut.Select();
			}
			TreeNodeTablut.ExpandResult expandResult = treeNodeTablut.Expand();
			BoardGameSide boardGameSide = BoardGameSide.None;
			bool flag = false;
			if (expandResult == TreeNodeTablut.ExpandResult.NeedsToBeSimulated)
			{
				if (!treeNodeTablut.IsLeaf)
				{
					treeNodeTablut = treeNodeTablut.Select();
				}
				TreeNodeTablut.SimulationResult simulationResult = treeNodeTablut.Simulate();
				if (simulationResult.EndState != BoardGameTablut.State.Aborted)
				{
					boardGameSide = ((simulationResult.EndState == BoardGameTablut.State.AIWon) ? BoardGameSide.AI : BoardGameSide.Player);
					treeNodeTablut.BackPropagate(boardGameSide);
					flag = simulationResult.TurnsNeededToReachEndState <= 1;
				}
			}
			else if (expandResult != TreeNodeTablut.ExpandResult.Aborted)
			{
				boardGameSide = ((expandResult == TreeNodeTablut.ExpandResult.AIWon) ? BoardGameSide.AI : BoardGameSide.Player);
				treeNodeTablut.BackPropagate(boardGameSide);
				flag = true;
			}
			if (flag)
			{
				this.PruneSiblings(treeNodeTablut, boardGameSide);
			}
		}

		// Token: 0x06000D4D RID: 3405 RVA: 0x00060BC4 File Offset: 0x0005EDC4
		private void PruneSiblings(TreeNodeTablut node, BoardGameSide winner)
		{
			if (node._parent != null && winner == node._lastTurnIsPlayedBy)
			{
				int count = node._parent._children.Count;
				if (count > 1)
				{
					int num = 0;
					int num2 = 0;
					for (int i = count - 1; i >= 0; i--)
					{
						if (node._parent._children[i] != node)
						{
							num += node._parent._children[i]._wins;
							num2 += node._parent._children[i]._visits;
							node._parent._children.RemoveAt(i);
						}
					}
					int num3 = num2 - num;
					for (TreeNodeTablut treeNodeTablut = node._parent; treeNodeTablut != null; treeNodeTablut = treeNodeTablut._parent)
					{
						if (treeNodeTablut._lastTurnIsPlayedBy == winner)
						{
							treeNodeTablut._wins -= num;
						}
						else
						{
							treeNodeTablut._wins -= num3;
						}
						treeNodeTablut._visits -= num2;
					}
				}
			}
		}

		// Token: 0x06000D4E RID: 3406 RVA: 0x00060CC8 File Offset: 0x0005EEC8
		private TreeNodeTablut Select()
		{
			double num = double.MinValue;
			TreeNodeTablut treeNodeTablut = null;
			foreach (TreeNodeTablut treeNodeTablut2 in this._children)
			{
				if (treeNodeTablut2._visits == 0)
				{
					treeNodeTablut = treeNodeTablut2;
					break;
				}
				double num2 = (double)treeNodeTablut2._wins / (double)treeNodeTablut2._visits + (double)(1.5f * MathF.Sqrt(MathF.Log((float)this._visits) / (float)treeNodeTablut2._visits));
				if (num2 > num)
				{
					treeNodeTablut = treeNodeTablut2;
					num = num2;
				}
			}
			if (treeNodeTablut._boardState.PawnInformation == null)
			{
				BoardGameAITablut.Board.UndoMove(ref treeNodeTablut._parent._boardState);
				BoardGameAITablut.Board.AIMakeMove(treeNodeTablut.OpeningMove);
				treeNodeTablut._boardState = BoardGameAITablut.Board.TakeBoardSnapshot();
			}
			return treeNodeTablut;
		}

		// Token: 0x06000D4F RID: 3407 RVA: 0x00060DAC File Offset: 0x0005EFAC
		private TreeNodeTablut.ExpandResult Expand()
		{
			TreeNodeTablut.ExpandResult expandResult = TreeNodeTablut.ExpandResult.NeedsToBeSimulated;
			if (this._depth < TreeNodeTablut.MaxDepth)
			{
				BoardGameAITablut.Board.UndoMove(ref this._boardState);
				BoardGameTablut.State state = BoardGameAITablut.Board.CheckGameState();
				if (state == BoardGameTablut.State.InProgress)
				{
					BoardGameSide boardGameSide = ((this._lastTurnIsPlayedBy == BoardGameSide.Player) ? BoardGameSide.AI : BoardGameSide.Player);
					Move winningMoveIfPresent = BoardGameAITablut.Board.GetWinningMoveIfPresent(boardGameSide);
					if (winningMoveIfPresent.IsValid)
					{
						TreeNodeTablut treeNodeTablut = new TreeNodeTablut(boardGameSide, this._depth + 1);
						treeNodeTablut.OpeningMove = winningMoveIfPresent;
						treeNodeTablut._parent = this;
						this._children = new List<TreeNodeTablut>(1);
						this._children.Add(treeNodeTablut);
					}
					else
					{
						List<List<Move>> list = BoardGameAITablut.Board.CalculateAllValidMoves(boardGameSide);
						int totalMovesAvailable = BoardGameAITablut.Board.GetTotalMovesAvailable(ref list);
						if (totalMovesAvailable > 0)
						{
							this._children = new List<TreeNodeTablut>(totalMovesAvailable);
							using (List<List<Move>>.Enumerator enumerator = list.GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									List<Move> list2 = enumerator.Current;
									foreach (Move move in list2)
									{
										TreeNodeTablut treeNodeTablut2 = new TreeNodeTablut(boardGameSide, this._depth + 1);
										treeNodeTablut2.OpeningMove = move;
										treeNodeTablut2._parent = this;
										this._children.Add(treeNodeTablut2);
									}
								}
								return expandResult;
							}
						}
						Debug.FailedAssert("No available moves left but the game is in progress", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox\\BoardGames\\AI\\TreeNodeTablut.cs", "Expand", 396);
					}
				}
				else if (state == BoardGameTablut.State.Aborted)
				{
					expandResult = TreeNodeTablut.ExpandResult.Aborted;
				}
				else if (state == BoardGameTablut.State.AIWon)
				{
					expandResult = TreeNodeTablut.ExpandResult.AIWon;
				}
				else
				{
					expandResult = TreeNodeTablut.ExpandResult.PlayerWon;
				}
			}
			return expandResult;
		}

		// Token: 0x06000D50 RID: 3408 RVA: 0x00060F4C File Offset: 0x0005F14C
		private TreeNodeTablut.SimulationResult Simulate()
		{
			BoardGameAITablut.Board.UndoMove(ref this._boardState);
			BoardGameTablut.State state = BoardGameAITablut.Board.CheckGameState();
			BoardGameSide boardGameSide = ((this._lastTurnIsPlayedBy == BoardGameSide.Player) ? BoardGameSide.AI : BoardGameSide.Player);
			int num = 0;
			while (state == BoardGameTablut.State.InProgress)
			{
				Move move = BoardGameAITablut.Board.GetWinningMoveIfPresent(boardGameSide);
				if (!move.IsValid)
				{
					List<PawnBase> list = ((boardGameSide == BoardGameSide.Player) ? BoardGameAITablut.Board.PlayerOneUnits : BoardGameAITablut.Board.PlayerTwoUnits);
					int count = list.Count;
					int num2 = 3;
					PawnBase pawnBase;
					bool flag;
					do
					{
						pawnBase = list[MBRandom.RandomInt(count)];
						flag = BoardGameAITablut.Board.HasAvailableMoves(pawnBase as PawnTablut);
						num2--;
					}
					while (!flag && num2 > 0);
					if (!flag)
					{
						pawnBase = list.OrderBy<PawnBase, int>((PawnBase x) => MBRandom.RandomInt()).FirstOrDefault<PawnBase>((PawnBase x) => BoardGameAITablut.Board.HasAvailableMoves(x as PawnTablut));
						flag = pawnBase != null;
					}
					if (flag)
					{
						move = BoardGameAITablut.Board.GetRandomAvailableMove(pawnBase as PawnTablut);
					}
				}
				if (move.IsValid)
				{
					BoardGameAITablut.Board.AIMakeMove(move);
					state = BoardGameAITablut.Board.CheckGameState();
				}
				else if (boardGameSide == BoardGameSide.Player)
				{
					state = BoardGameTablut.State.AIWon;
				}
				else
				{
					state = BoardGameTablut.State.PlayerWon;
				}
				boardGameSide = ((boardGameSide == BoardGameSide.Player) ? BoardGameSide.AI : BoardGameSide.Player);
				num++;
			}
			return new TreeNodeTablut.SimulationResult(state, num);
		}

		// Token: 0x06000D51 RID: 3409 RVA: 0x000610B4 File Offset: 0x0005F2B4
		private void BackPropagate(BoardGameSide winner)
		{
			for (TreeNodeTablut treeNodeTablut = this; treeNodeTablut != null; treeNodeTablut = treeNodeTablut._parent)
			{
				treeNodeTablut._visits++;
				if (winner == treeNodeTablut._lastTurnIsPlayedBy)
				{
					treeNodeTablut._wins++;
				}
			}
		}

		// Token: 0x040005BB RID: 1467
		private const float UCTConstant = 1.5f;

		// Token: 0x040005BC RID: 1468
		private static int MaxDepth;

		// Token: 0x040005BD RID: 1469
		private readonly int _depth;

		// Token: 0x040005BE RID: 1470
		private BoardGameTablut.BoardInformation _boardState;

		// Token: 0x040005BF RID: 1471
		private TreeNodeTablut _parent;

		// Token: 0x040005C0 RID: 1472
		private List<TreeNodeTablut> _children;

		// Token: 0x040005C1 RID: 1473
		private BoardGameSide _lastTurnIsPlayedBy;

		// Token: 0x040005C2 RID: 1474
		private int _visits;

		// Token: 0x040005C3 RID: 1475
		private int _wins;

		// Token: 0x0200022E RID: 558
		private struct SimulationResult
		{
			// Token: 0x0600142D RID: 5165 RVA: 0x000792FD File Offset: 0x000774FD
			public SimulationResult(BoardGameTablut.State s, int turns)
			{
				this.EndState = s;
				this.TurnsNeededToReachEndState = turns;
			}

			// Token: 0x040009DA RID: 2522
			public readonly BoardGameTablut.State EndState;

			// Token: 0x040009DB RID: 2523
			public readonly int TurnsNeededToReachEndState;
		}

		// Token: 0x0200022F RID: 559
		private enum ExpandResult
		{
			// Token: 0x040009DD RID: 2525
			NeedsToBeSimulated,
			// Token: 0x040009DE RID: 2526
			AIWon,
			// Token: 0x040009DF RID: 2527
			PlayerWon,
			// Token: 0x040009E0 RID: 2528
			Aborted
		}
	}
}
