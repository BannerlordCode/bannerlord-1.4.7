using System;
using System.Collections.Generic;
using System.Linq;
using SandBox.BoardGames.MissionLogics;
using SandBox.BoardGames.Objects;
using SandBox.BoardGames.Pawns;
using SandBox.BoardGames.Tiles;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace SandBox.BoardGames
{
	// Token: 0x020000EE RID: 238
	public class BoardGameMuTorere : BoardGameBase
	{
		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000BDF RID: 3039 RVA: 0x0005804E File Offset: 0x0005624E
		public override int TileCount
		{
			get
			{
				return 9;
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x06000BE0 RID: 3040 RVA: 0x00058052 File Offset: 0x00056252
		protected override bool RotateBoard
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x06000BE1 RID: 3041 RVA: 0x00058055 File Offset: 0x00056255
		protected override bool PreMovementStagePresent
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06000BE2 RID: 3042 RVA: 0x00058058 File Offset: 0x00056258
		protected override bool DiceRollRequired
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000BE3 RID: 3043 RVA: 0x0005805B File Offset: 0x0005625B
		public BoardGameMuTorere(MissionBoardGameLogic mission, PlayerTurn startingPlayer)
			: base(mission, new TextObject("{=5siAbi69}Mu Torere", null), startingPlayer)
		{
			this.PawnUnselectedFactor = 4288711820U;
		}

		// Token: 0x06000BE4 RID: 3044 RVA: 0x0005807C File Offset: 0x0005627C
		public override void InitializeUnits()
		{
			base.PlayerOneUnits.Clear();
			base.PlayerTwoUnits.Clear();
			List<PawnBase> list = ((base.PlayerWhoStarted == PlayerTurn.PlayerOne) ? base.PlayerOneUnits : base.PlayerTwoUnits);
			for (int i = 0; i < 4; i++)
			{
				GameEntity gameEntity = Mission.Current.Scene.FindEntityWithTag("player_one_unit_" + i);
				list.Add(base.InitializeUnit(new PawnMuTorere(gameEntity, base.PlayerWhoStarted == PlayerTurn.PlayerOne)));
			}
			List<PawnBase> list2 = ((base.PlayerWhoStarted == PlayerTurn.PlayerOne) ? base.PlayerTwoUnits : base.PlayerOneUnits);
			for (int j = 0; j < 4; j++)
			{
				GameEntity gameEntity2 = Mission.Current.Scene.FindEntityWithTag("player_two_unit_" + j);
				list2.Add(base.InitializeUnit(new PawnMuTorere(gameEntity2, base.PlayerWhoStarted > PlayerTurn.PlayerOne)));
			}
		}

		// Token: 0x06000BE5 RID: 3045 RVA: 0x00058164 File Offset: 0x00056364
		public override void InitializeTiles()
		{
			if (base.Tiles == null)
			{
				base.Tiles = new TileBase[this.TileCount];
			}
			int x;
			IEnumerable<GameEntity> enumerable = from x in this.BoardEntity.GetChildren()
				where x.Tags.Any<string>((string t) => t.Contains("tile_"))
				select x;
			IEnumerable<GameEntity> enumerable2 = from x in this.BoardEntity.GetChildren()
				where x.Tags.Any<string>((string t) => t.Contains("decal_"))
				select x;
			int num;
			for (x = 0; x < this.TileCount; x = num)
			{
				GameEntity gameEntity = enumerable.Single<GameEntity>((GameEntity e) => e.HasTag("tile_" + x));
				BoardGameDecal firstScriptOfType = enumerable2.Single<GameEntity>((GameEntity e) => e.HasTag("decal_" + x)).GetFirstScriptOfType<BoardGameDecal>();
				num = x;
				int num2;
				int num3;
				if (num != 0)
				{
					if (num != 1)
					{
						if (num != 8)
						{
							num2 = x - 1;
							num3 = x + 1;
						}
						else
						{
							num2 = 7;
							num3 = 1;
						}
					}
					else
					{
						num2 = 8;
						num3 = 2;
					}
				}
				else
				{
					num3 = (num2 = -1);
				}
				base.Tiles[x] = new TileMuTorere(gameEntity, firstScriptOfType, x, num2, num3);
				gameEntity.CreateVariableRatePhysics(true);
				num = x + 1;
			}
		}

		// Token: 0x06000BE6 RID: 3046 RVA: 0x000582B5 File Offset: 0x000564B5
		public override void InitializeCapturedUnitsZones()
		{
		}

		// Token: 0x06000BE7 RID: 3047 RVA: 0x000582B7 File Offset: 0x000564B7
		public override void InitializeSound()
		{
			PawnBase.PawnMoveSoundCodeID = SoundEvent.GetEventIdFromString("event:/mission/movement/foley/minigame/move_stone");
			PawnBase.PawnSelectSoundCodeID = SoundEvent.GetEventIdFromString("event:/mission/movement/foley/minigame/pick_stone");
			PawnBase.PawnTapSoundCodeID = SoundEvent.GetEventIdFromString("event:/mission/movement/foley/minigame/drop_wood");
			PawnBase.PawnRemoveSoundCodeID = SoundEvent.GetEventIdFromString("event:/mission/movement/foley/minigame/out_stone");
		}

		// Token: 0x06000BE8 RID: 3048 RVA: 0x000582F5 File Offset: 0x000564F5
		public override void Reset()
		{
			base.Reset();
			this.PreplaceUnits();
		}

		// Token: 0x06000BE9 RID: 3049 RVA: 0x00058304 File Offset: 0x00056504
		public override List<Move> CalculateValidMoves(PawnBase pawn)
		{
			List<Move> list = new List<Move>();
			PawnMuTorere pawnMuTorere = pawn as PawnMuTorere;
			if (pawnMuTorere != null)
			{
				TileMuTorere tileMuTorere = this.FindAvailableTile() as TileMuTorere;
				if (pawnMuTorere.X == 0)
				{
					Move move;
					move.Unit = pawn;
					move.GoalTile = tileMuTorere;
					list.Add(move);
				}
				else if (tileMuTorere.X != 0)
				{
					if (pawnMuTorere.X == tileMuTorere.XLeftTile || pawnMuTorere.X == tileMuTorere.XRightTile)
					{
						Move move2;
						move2.Unit = pawn;
						move2.GoalTile = tileMuTorere;
						list.Add(move2);
					}
				}
				else
				{
					TileMuTorere tileMuTorere2 = this.FindTileByCoordinate(pawnMuTorere.X);
					PawnBase pawnOnTile = base.Tiles[tileMuTorere2.XLeftTile].PawnOnTile;
					PawnBase pawnOnTile2 = base.Tiles[tileMuTorere2.XRightTile].PawnOnTile;
					if (pawnOnTile.PlayerOne != pawnMuTorere.PlayerOne || pawnOnTile2.PlayerOne != pawnMuTorere.PlayerOne)
					{
						Move move3;
						move3.Unit = pawn;
						move3.GoalTile = tileMuTorere;
						list.Add(move3);
					}
				}
			}
			return list;
		}

		// Token: 0x06000BEA RID: 3050 RVA: 0x00058400 File Offset: 0x00056600
		protected override PawnBase SelectPawn(PawnBase pawn)
		{
			if (base.PlayerTurn == PlayerTurn.PlayerOne)
			{
				if (pawn.PlayerOne)
				{
					this.SelectedUnit = pawn;
				}
			}
			else if (base.AIOpponent == null && !pawn.PlayerOne)
			{
				this.SelectedUnit = pawn;
			}
			return pawn;
		}

		// Token: 0x06000BEB RID: 3051 RVA: 0x00058434 File Offset: 0x00056634
		protected override void MovePawnToTileDelayed(PawnBase pawn, TileBase tile, bool instantMove, bool displayMessage, float delay)
		{
			base.MovePawnToTileDelayed(pawn, tile, instantMove, displayMessage, delay);
			TileMuTorere tileMuTorere = tile as TileMuTorere;
			PawnMuTorere pawnMuTorere = pawn as PawnMuTorere;
			if (tileMuTorere.PawnOnTile == null && pawnMuTorere != null)
			{
				if (displayMessage)
				{
					if (base.PlayerTurn == PlayerTurn.PlayerOne)
					{
						InformationManager.DisplayMessage(new InformationMessage(GameTexts.FindText("str_boardgame_move_piece_player", null).ToString()));
					}
					else
					{
						InformationManager.DisplayMessage(new InformationMessage(GameTexts.FindText("str_boardgame_move_piece_opponent", null).ToString()));
					}
				}
				if (pawnMuTorere.X != -1)
				{
					base.Tiles[pawnMuTorere.X].PawnOnTile = null;
				}
				tileMuTorere.PawnOnTile = pawnMuTorere;
				pawnMuTorere.MovingToDifferentTile = pawnMuTorere.X != tileMuTorere.X;
				pawnMuTorere.X = tileMuTorere.X;
				Vec3 globalPosition = tileMuTorere.Entity.GlobalPosition;
				pawnMuTorere.AddGoalPosition(globalPosition);
				pawnMuTorere.MovePawnToGoalPositionsDelayed(instantMove, 0.6f, this.JustStoppedDraggingUnit, delay);
				if (pawnMuTorere == this.SelectedUnit)
				{
					this.SelectedUnit = null;
				}
			}
		}

		// Token: 0x06000BEC RID: 3052 RVA: 0x0005852C File Offset: 0x0005672C
		protected override void SwitchPlayerTurn()
		{
			if (base.PlayerTurn == PlayerTurn.PlayerOneWaiting)
			{
				base.PlayerTurn = PlayerTurn.PlayerTwo;
			}
			else if (base.PlayerTurn == PlayerTurn.PlayerTwoWaiting)
			{
				base.PlayerTurn = PlayerTurn.PlayerOne;
			}
			this.CheckGameEnded();
			base.SwitchPlayerTurn();
		}

		// Token: 0x06000BED RID: 3053 RVA: 0x00058560 File Offset: 0x00056760
		protected override bool CheckGameEnded()
		{
			bool flag = false;
			List<List<Move>> list = this.CalculateAllValidMoves((base.PlayerTurn == PlayerTurn.PlayerOne) ? BoardGameSide.Player : BoardGameSide.AI);
			if (base.GetTotalMovesAvailable(ref list) <= 0)
			{
				if (base.PlayerTurn == PlayerTurn.PlayerOne)
				{
					base.OnDefeat("str_boardgame_defeat_message");
					this.ReadyToPlay = false;
					flag = true;
				}
				else if (base.PlayerTurn == PlayerTurn.PlayerTwo)
				{
					base.OnVictory("str_boardgame_victory_message");
					this.ReadyToPlay = false;
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x06000BEE RID: 3054 RVA: 0x000585C9 File Offset: 0x000567C9
		protected override void OnAfterBoardSetUp()
		{
			this.ReadyToPlay = true;
		}

		// Token: 0x06000BEF RID: 3055 RVA: 0x000585D4 File Offset: 0x000567D4
		public TileMuTorere FindTileByCoordinate(int x)
		{
			TileMuTorere tileMuTorere = null;
			for (int i = 0; i < this.TileCount; i++)
			{
				TileMuTorere tileMuTorere2 = base.Tiles[i] as TileMuTorere;
				if (tileMuTorere2.X == x)
				{
					tileMuTorere = tileMuTorere2;
				}
			}
			return tileMuTorere;
		}

		// Token: 0x06000BF0 RID: 3056 RVA: 0x00058610 File Offset: 0x00056810
		public BoardGameMuTorere.BoardInformation TakePawnsSnapshot()
		{
			BoardGameMuTorere.PawnInformation[] array = new BoardGameMuTorere.PawnInformation[base.PlayerOneUnits.Count + base.PlayerTwoUnits.Count];
			TileBaseInformation[] array2 = new TileBaseInformation[this.TileCount];
			int num = 0;
			foreach (PawnBase pawnBase in ((base.PlayerWhoStarted == PlayerTurn.PlayerOne) ? base.PlayerOneUnits : base.PlayerTwoUnits))
			{
				PawnMuTorere pawnMuTorere = (PawnMuTorere)pawnBase;
				BoardGameMuTorere.PawnInformation pawnInformation = new BoardGameMuTorere.PawnInformation(pawnMuTorere.X);
				array[num++] = pawnInformation;
			}
			foreach (PawnBase pawnBase2 in ((base.PlayerWhoStarted == PlayerTurn.PlayerOne) ? base.PlayerTwoUnits : base.PlayerOneUnits))
			{
				PawnMuTorere pawnMuTorere2 = (PawnMuTorere)pawnBase2;
				BoardGameMuTorere.PawnInformation pawnInformation2 = new BoardGameMuTorere.PawnInformation(pawnMuTorere2.X);
				array[num++] = pawnInformation2;
			}
			for (int i = 0; i < this.TileCount; i++)
			{
				array2[i] = new TileBaseInformation(ref base.Tiles[i].PawnOnTile);
			}
			return new BoardGameMuTorere.BoardInformation(ref array, ref array2);
		}

		// Token: 0x06000BF1 RID: 3057 RVA: 0x00058760 File Offset: 0x00056960
		public void UndoMove(ref BoardGameMuTorere.BoardInformation board)
		{
			int num = 0;
			foreach (PawnBase pawnBase in ((base.PlayerWhoStarted == PlayerTurn.PlayerOne) ? base.PlayerOneUnits : base.PlayerTwoUnits))
			{
				((PawnMuTorere)pawnBase).X = board.PawnInformation[num++].X;
			}
			foreach (PawnBase pawnBase2 in ((base.PlayerWhoStarted == PlayerTurn.PlayerOne) ? base.PlayerTwoUnits : base.PlayerOneUnits))
			{
				((PawnMuTorere)pawnBase2).X = board.PawnInformation[num++].X;
			}
			for (int i = 0; i < this.TileCount; i++)
			{
				base.Tiles[i].PawnOnTile = board.TileInformation[i].PawnOnTile;
			}
		}

		// Token: 0x06000BF2 RID: 3058 RVA: 0x00058874 File Offset: 0x00056A74
		public void AIMakeMove(Move move)
		{
			TileMuTorere tileMuTorere = move.GoalTile as TileMuTorere;
			PawnMuTorere pawnMuTorere = move.Unit as PawnMuTorere;
			base.Tiles[pawnMuTorere.X].PawnOnTile = null;
			tileMuTorere.PawnOnTile = pawnMuTorere;
			pawnMuTorere.X = tileMuTorere.X;
		}

		// Token: 0x06000BF3 RID: 3059 RVA: 0x000588C0 File Offset: 0x00056AC0
		public TileBase FindAvailableTile()
		{
			foreach (TileBase tileBase in base.Tiles)
			{
				if (tileBase.PawnOnTile == null)
				{
					return tileBase;
				}
			}
			return null;
		}

		// Token: 0x06000BF4 RID: 3060 RVA: 0x000588F4 File Offset: 0x00056AF4
		private void PreplaceUnits()
		{
			List<PawnBase> list = ((base.PlayerWhoStarted == PlayerTurn.PlayerOne) ? base.PlayerOneUnits : base.PlayerTwoUnits);
			List<PawnBase> list2 = ((base.PlayerWhoStarted == PlayerTurn.PlayerOne) ? base.PlayerTwoUnits : base.PlayerOneUnits);
			for (int i = 0; i < 4; i++)
			{
				this.MovePawnToTileDelayed(list[i], base.Tiles[i + 1], false, false, 0.15f * (float)(i + 1) + 0.25f);
				this.MovePawnToTileDelayed(list2[i], base.Tiles[8 - i], false, false, 0.15f * (float)(i + 1) + 0.5f);
			}
		}

		// Token: 0x0400053E RID: 1342
		public const int WhitePawnCount = 4;

		// Token: 0x0400053F RID: 1343
		public const int BlackPawnCount = 4;

		// Token: 0x02000216 RID: 534
		public struct BoardInformation
		{
			// Token: 0x060013F9 RID: 5113 RVA: 0x00078DDD File Offset: 0x00076FDD
			public BoardInformation(ref BoardGameMuTorere.PawnInformation[] pawns, ref TileBaseInformation[] tiles)
			{
				this.PawnInformation = pawns;
				this.TileInformation = tiles;
			}

			// Token: 0x0400098E RID: 2446
			public readonly BoardGameMuTorere.PawnInformation[] PawnInformation;

			// Token: 0x0400098F RID: 2447
			public readonly TileBaseInformation[] TileInformation;
		}

		// Token: 0x02000217 RID: 535
		public struct PawnInformation
		{
			// Token: 0x060013FA RID: 5114 RVA: 0x00078DEF File Offset: 0x00076FEF
			public PawnInformation(int x)
			{
				this.X = x;
			}

			// Token: 0x04000990 RID: 2448
			public readonly int X;
		}
	}
}
