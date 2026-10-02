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
	// Token: 0x020000E6 RID: 230
	public class BoardGameBaghChal : BoardGameBase
	{
		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000B42 RID: 2882 RVA: 0x0005364C File Offset: 0x0005184C
		public override int TileCount
		{
			get
			{
				return BoardGameBaghChal.BoardWidth * BoardGameBaghChal.BoardHeight;
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000B43 RID: 2883 RVA: 0x00053659 File Offset: 0x00051859
		protected override bool RotateBoard
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000B44 RID: 2884 RVA: 0x0005365C File Offset: 0x0005185C
		protected override bool PreMovementStagePresent
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000B45 RID: 2885 RVA: 0x0005365F File Offset: 0x0005185F
		protected override bool DiceRollRequired
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000B46 RID: 2886 RVA: 0x00053662 File Offset: 0x00051862
		public BoardGameBaghChal(MissionBoardGameLogic mission, PlayerTurn startingPlayer)
			: base(mission, new TextObject("{=zWoj91XY}BaghChal", null), startingPlayer)
		{
			if (base.Tiles == null)
			{
				base.Tiles = new TileBase[this.TileCount];
			}
			this.SelectedUnit = null;
			this.PawnUnselectedFactor = 4287395960U;
		}

		// Token: 0x06000B47 RID: 2887 RVA: 0x000536A4 File Offset: 0x000518A4
		public override void InitializeUnits()
		{
			bool flag = base.PlayerWhoStarted == PlayerTurn.PlayerOne;
			if (this._goatUnits == null && this._tigerUnits == null)
			{
				this._goatUnits = (flag ? base.PlayerOneUnits : base.PlayerTwoUnits);
				for (int i = 0; i < 20; i++)
				{
					GameEntity gameEntity = Mission.Current.Scene.FindEntityWithTag("player_one_unit_" + i);
					this._goatUnits.Add(base.InitializeUnit(new PawnBaghChal(gameEntity, flag, false)));
				}
				this._tigerUnits = (flag ? base.PlayerTwoUnits : base.PlayerOneUnits);
				for (int j = 0; j < 4; j++)
				{
					GameEntity gameEntity2 = Mission.Current.Scene.FindEntityWithTag("player_two_unit_" + j);
					this._tigerUnits.Add(base.InitializeUnit(new PawnBaghChal(gameEntity2, !flag, true)));
				}
				return;
			}
			if (this._goatUnits == base.PlayerOneUnits != flag)
			{
				List<PawnBase> playerOneUnits = base.PlayerOneUnits;
				base.PlayerOneUnits = base.PlayerTwoUnits;
				base.PlayerTwoUnits = playerOneUnits;
			}
			this._goatUnits = (flag ? base.PlayerOneUnits : base.PlayerTwoUnits);
			this._tigerUnits = (flag ? base.PlayerTwoUnits : base.PlayerOneUnits);
			foreach (PawnBase pawnBase in this._goatUnits)
			{
				pawnBase.Reset();
				pawnBase.SetPlayerOne(flag);
			}
			foreach (PawnBase pawnBase2 in this._tigerUnits)
			{
				pawnBase2.Reset();
				pawnBase2.SetPlayerOne(!flag);
			}
		}

		// Token: 0x06000B48 RID: 2888 RVA: 0x00053880 File Offset: 0x00051A80
		public override void InitializeTiles()
		{
			int x;
			IEnumerable<GameEntity> enumerable = from x in this.BoardEntity.GetChildren()
				where x.Tags.Any<string>((string t) => t.Contains("tile_"))
				select x;
			IEnumerable<GameEntity> enumerable2 = from x in this.BoardEntity.GetChildren()
				where x.Tags.Any<string>((string t) => t.Contains("decal_"))
				select x;
			int num;
			for (x = 0; x < BoardGameBaghChal.BoardWidth; x = num)
			{
				int y;
				for (y = 0; y < BoardGameBaghChal.BoardHeight; y = num)
				{
					GameEntity gameEntity = enumerable.Single<GameEntity>((GameEntity e) => e.HasTag(string.Concat(new object[] { "tile_", x, "_", y })));
					BoardGameDecal firstScriptOfType = enumerable2.Single<GameEntity>((GameEntity e) => e.HasTag(string.Concat(new object[] { "decal_", x, "_", y }))).GetFirstScriptOfType<BoardGameDecal>();
					Tile2D tile2D = new Tile2D(gameEntity, firstScriptOfType, x, y);
					gameEntity.CreateVariableRatePhysics(true);
					this.SetTile(tile2D, x, y);
					num = y + 1;
				}
				num = x + 1;
			}
		}

		// Token: 0x06000B49 RID: 2889 RVA: 0x000539BE File Offset: 0x00051BBE
		public override void InitializeSound()
		{
			PawnBase.PawnMoveSoundCodeID = SoundEvent.GetEventIdFromString("event:/mission/movement/foley/minigame/move_stone");
			PawnBase.PawnSelectSoundCodeID = SoundEvent.GetEventIdFromString("event:/mission/movement/foley/minigame/pick_stone");
			PawnBase.PawnTapSoundCodeID = SoundEvent.GetEventIdFromString("event:/mission/movement/foley/minigame/drop_stone");
			PawnBase.PawnRemoveSoundCodeID = SoundEvent.GetEventIdFromString("event:/mission/movement/foley/minigame/out_stone");
		}

		// Token: 0x06000B4A RID: 2890 RVA: 0x000539FC File Offset: 0x00051BFC
		public override void Reset()
		{
			base.Reset();
			base.InPreMovementStage = true;
		}

		// Token: 0x06000B4B RID: 2891 RVA: 0x00053A0C File Offset: 0x00051C0C
		public override List<List<Move>> CalculateAllValidMoves(BoardGameSide side)
		{
			List<List<Move>> list = new List<List<Move>>();
			bool flag = true;
			foreach (PawnBase pawnBase in ((side == BoardGameSide.AI) ? base.PlayerTwoUnits : base.PlayerOneUnits))
			{
				PawnBaghChal pawnBaghChal = (PawnBaghChal)pawnBase;
				if ((flag || pawnBaghChal.IsPlaced) && !pawnBaghChal.Captured)
				{
					List<Move> list2 = this.CalculateValidMoves(pawnBaghChal);
					if (list2.Count > 0)
					{
						list.Add(list2);
					}
					if (pawnBaghChal.IsGoat && !pawnBaghChal.IsPlaced)
					{
						flag = false;
					}
				}
			}
			return list;
		}

		// Token: 0x06000B4C RID: 2892 RVA: 0x00053AB4 File Offset: 0x00051CB4
		public override List<Move> CalculateValidMoves(PawnBase pawn)
		{
			List<Move> list = new List<Move>();
			PawnBaghChal pawnBaghChal = pawn as PawnBaghChal;
			if (pawn != null)
			{
				int x = pawnBaghChal.X;
				int y = pawnBaghChal.Y;
				bool isTiger = pawnBaghChal.IsTiger;
				if ((isTiger || !base.InPreMovementStage) && x >= 0 && x < BoardGameBaghChal.BoardWidth && y >= 0 && y < BoardGameBaghChal.BoardHeight)
				{
					if (x > 0 && this.GetTile(x - 1, y).PawnOnTile == null)
					{
						Move move;
						move.Unit = pawn;
						move.GoalTile = this.GetTile(x - 1, y);
						list.Add(move);
					}
					if (x < BoardGameBaghChal.BoardWidth - 1 && this.GetTile(x + 1, y).PawnOnTile == null)
					{
						Move move2;
						move2.Unit = pawn;
						move2.GoalTile = this.GetTile(x + 1, y);
						list.Add(move2);
					}
					if (y > 0 && this.GetTile(x, y - 1).PawnOnTile == null)
					{
						Move move3;
						move3.Unit = pawn;
						move3.GoalTile = this.GetTile(x, y - 1);
						list.Add(move3);
					}
					if (y < BoardGameBaghChal.BoardHeight - 1 && this.GetTile(x, y + 1).PawnOnTile == null)
					{
						Move move4;
						move4.Unit = pawn;
						move4.GoalTile = this.GetTile(x, y + 1);
						list.Add(move4);
					}
					if ((x + y) % 2 == 0)
					{
						Vec2i vec2i = new Vec2i(x + 1, y + 1);
						if (vec2i.X < BoardGameBaghChal.BoardWidth && vec2i.Y < BoardGameBaghChal.BoardHeight && this.GetTile(vec2i.X, vec2i.Y).PawnOnTile == null)
						{
							Move move5;
							move5.Unit = pawn;
							move5.GoalTile = this.GetTile(vec2i.X, vec2i.Y);
							list.Add(move5);
						}
						vec2i = new Vec2i(x - 1, y + 1);
						if (vec2i.X >= 0 && vec2i.Y < BoardGameBaghChal.BoardHeight && this.GetTile(vec2i.X, vec2i.Y).PawnOnTile == null)
						{
							Move move6;
							move6.Unit = pawn;
							move6.GoalTile = this.GetTile(vec2i.X, vec2i.Y);
							list.Add(move6);
						}
						vec2i = new Vec2i(x - 1, y - 1);
						if (vec2i.X >= 0 && vec2i.Y >= 0 && this.GetTile(vec2i.X, vec2i.Y).PawnOnTile == null)
						{
							Move move7;
							move7.Unit = pawn;
							move7.GoalTile = this.GetTile(vec2i.X, vec2i.Y);
							list.Add(move7);
						}
						vec2i = new Vec2i(x + 1, y - 1);
						if (vec2i.X < BoardGameBaghChal.BoardWidth && vec2i.Y >= 0 && this.GetTile(vec2i.X, vec2i.Y).PawnOnTile == null)
						{
							Move move8;
							move8.Unit = pawn;
							move8.GoalTile = this.GetTile(vec2i.X, vec2i.Y);
							list.Add(move8);
						}
					}
				}
				if (isTiger && x >= 0 && x < BoardGameBaghChal.BoardWidth && y >= 0 && y < BoardGameBaghChal.BoardHeight)
				{
					if (x > 1)
					{
						PawnBaghChal pawnBaghChal2 = this.GetTile(x - 1, y).PawnOnTile as PawnBaghChal;
						PawnBase pawnOnTile = this.GetTile(x - 2, y).PawnOnTile;
						if (pawnBaghChal2 != null && !pawnBaghChal2.IsTiger && pawnOnTile == null)
						{
							Move move9;
							move9.Unit = pawn;
							move9.GoalTile = this.GetTile(x - 2, y);
							list.Add(move9);
						}
					}
					if (x < BoardGameBaghChal.BoardWidth - 2)
					{
						PawnBaghChal pawnBaghChal3 = this.GetTile(x + 1, y).PawnOnTile as PawnBaghChal;
						PawnBase pawnOnTile2 = this.GetTile(x + 2, y).PawnOnTile;
						if (pawnBaghChal3 != null && !pawnBaghChal3.IsTiger && pawnOnTile2 == null)
						{
							Move move10;
							move10.Unit = pawn;
							move10.GoalTile = this.GetTile(x + 2, y);
							list.Add(move10);
						}
					}
					if (y > 1)
					{
						PawnBaghChal pawnBaghChal4 = this.GetTile(x, y - 1).PawnOnTile as PawnBaghChal;
						PawnBase pawnOnTile3 = this.GetTile(x, y - 2).PawnOnTile;
						if (pawnBaghChal4 != null && !pawnBaghChal4.IsTiger && pawnOnTile3 == null)
						{
							Move move11;
							move11.Unit = pawn;
							move11.GoalTile = this.GetTile(x, y - 2);
							list.Add(move11);
						}
					}
					if (y < BoardGameBaghChal.BoardHeight - 2)
					{
						PawnBaghChal pawnBaghChal5 = this.GetTile(x, y + 1).PawnOnTile as PawnBaghChal;
						PawnBase pawnOnTile4 = this.GetTile(x, y + 2).PawnOnTile;
						if (pawnBaghChal5 != null && !pawnBaghChal5.IsTiger && pawnOnTile4 == null)
						{
							Move move12;
							move12.Unit = pawn;
							move12.GoalTile = this.GetTile(x, y + 2);
							list.Add(move12);
						}
					}
					if ((x + y) % 2 == 0)
					{
						Vec2i vec2i2 = new Vec2i(x + 2, y + 2);
						if (vec2i2.X < BoardGameBaghChal.BoardWidth && vec2i2.Y < BoardGameBaghChal.BoardHeight)
						{
							PawnBaghChal pawnBaghChal6 = this.GetTile(x + 1, y + 1).PawnOnTile as PawnBaghChal;
							if (pawnBaghChal6 != null && !pawnBaghChal6.IsTiger && this.GetTile(vec2i2.X, vec2i2.Y).PawnOnTile == null)
							{
								Move move13;
								move13.Unit = pawn;
								move13.GoalTile = this.GetTile(vec2i2.X, vec2i2.Y);
								list.Add(move13);
							}
						}
						vec2i2 = new Vec2i(x - 2, y + 2);
						if (vec2i2.X >= 0 && vec2i2.Y < BoardGameBaghChal.BoardHeight)
						{
							PawnBaghChal pawnBaghChal7 = this.GetTile(x - 1, y + 1).PawnOnTile as PawnBaghChal;
							if (pawnBaghChal7 != null && !pawnBaghChal7.IsTiger && this.GetTile(vec2i2.X, vec2i2.Y).PawnOnTile == null)
							{
								Move move14;
								move14.Unit = pawn;
								move14.GoalTile = this.GetTile(vec2i2.X, vec2i2.Y);
								list.Add(move14);
							}
						}
						vec2i2 = new Vec2i(x - 2, y - 2);
						if (vec2i2.X >= 0 && vec2i2.Y >= 0)
						{
							PawnBaghChal pawnBaghChal8 = this.GetTile(x - 1, y - 1).PawnOnTile as PawnBaghChal;
							if (pawnBaghChal8 != null && !pawnBaghChal8.IsTiger && this.GetTile(vec2i2.X, vec2i2.Y).PawnOnTile == null)
							{
								Move move15;
								move15.Unit = pawn;
								move15.GoalTile = this.GetTile(vec2i2.X, vec2i2.Y);
								list.Add(move15);
							}
						}
						vec2i2 = new Vec2i(x + 2, y - 2);
						if (vec2i2.X < BoardGameBaghChal.BoardWidth && vec2i2.Y >= 0)
						{
							PawnBaghChal pawnBaghChal9 = this.GetTile(x + 1, y - 1).PawnOnTile as PawnBaghChal;
							if (pawnBaghChal9 != null && !pawnBaghChal9.IsTiger && this.GetTile(vec2i2.X, vec2i2.Y).PawnOnTile == null)
							{
								Move move16;
								move16.Unit = pawn;
								move16.GoalTile = this.GetTile(vec2i2.X, vec2i2.Y);
								list.Add(move16);
							}
						}
					}
				}
				if (!isTiger && base.InPreMovementStage && x == -1 && y == -1)
				{
					for (int i = 0; i < this.TileCount; i++)
					{
						if (base.Tiles[i].PawnOnTile == null)
						{
							Move move17;
							move17.Unit = pawn;
							move17.GoalTile = base.Tiles[i];
							list.Add(move17);
						}
					}
				}
			}
			return list;
		}

		// Token: 0x06000B4D RID: 2893 RVA: 0x00054200 File Offset: 0x00052400
		public override void SetPawnCaptured(PawnBase pawn, bool fake = false)
		{
			base.SetPawnCaptured(pawn, fake);
			PawnBaghChal pawnBaghChal = pawn as PawnBaghChal;
			this.GetTile(pawnBaghChal.X, pawnBaghChal.Y).PawnOnTile = null;
			pawnBaghChal.PrevX = pawnBaghChal.X;
			pawnBaghChal.PrevY = pawnBaghChal.Y;
			pawnBaghChal.X = -1;
			pawnBaghChal.Y = -1;
			if (!fake)
			{
				base.RemovePawnFromBoard(pawnBaghChal, 0.6f, false);
			}
		}

		// Token: 0x06000B4E RID: 2894 RVA: 0x0005426C File Offset: 0x0005246C
		protected override void HandlePreMovementStage(float dt)
		{
			Move move = base.HandlePlayerInput(dt);
			if (move.IsValid)
			{
				this.MovePawnToTile(move.Unit, move.GoalTile, false, true);
			}
		}

		// Token: 0x06000B4F RID: 2895 RVA: 0x0005429E File Offset: 0x0005249E
		protected override void HandlePreMovementStageAI(Move move)
		{
			this.MovePawnToTile(move.Unit, move.GoalTile, false, true);
		}

		// Token: 0x06000B50 RID: 2896 RVA: 0x000542B4 File Offset: 0x000524B4
		protected override PawnBase SelectPawn(PawnBase pawn)
		{
			if (pawn.PlayerOne == (base.PlayerTurn == PlayerTurn.PlayerOne))
			{
				if (base.PlayerTurn == base.PlayerWhoStarted)
				{
					if (base.InPreMovementStage)
					{
						if (!pawn.IsPlaced && !pawn.Captured)
						{
							this.SelectedUnit = pawn;
						}
					}
					else
					{
						this.SelectedUnit = pawn;
					}
				}
				else
				{
					this.SelectedUnit = pawn;
				}
			}
			return pawn;
		}

		// Token: 0x06000B51 RID: 2897 RVA: 0x00054314 File Offset: 0x00052514
		protected override void SwitchPlayerTurn()
		{
			if ((base.PlayerTurn == PlayerTurn.PlayerOneWaiting || base.PlayerTurn == PlayerTurn.PlayerTwoWaiting) && this.SelectedUnit != null)
			{
				this.CheckIfPawnCaptures(this.SelectedUnit as PawnBaghChal, false);
			}
			this.SelectedUnit = null;
			if (base.PlayerTurn == PlayerTurn.PlayerOneWaiting)
			{
				base.PlayerTurn = PlayerTurn.PlayerTwo;
			}
			else if (base.PlayerTurn == PlayerTurn.PlayerTwoWaiting)
			{
				base.PlayerTurn = PlayerTurn.PlayerOne;
			}
			if (base.InPreMovementStage)
			{
				base.InPreMovementStage = !this.CheckPlacementStageOver();
			}
			this.CheckGameEnded();
			base.SwitchPlayerTurn();
		}

		// Token: 0x06000B52 RID: 2898 RVA: 0x0005439C File Offset: 0x0005259C
		protected override bool CheckGameEnded()
		{
			bool flag = false;
			if (base.PlayerTurn == PlayerTurn.PlayerTwo || base.PlayerTurn == PlayerTurn.PlayerOne)
			{
				List<List<Move>> list = this.CalculateAllValidMoves((base.PlayerWhoStarted == PlayerTurn.PlayerOne) ? BoardGameSide.AI : BoardGameSide.Player);
				if (!base.HasMovesAvailable(ref list))
				{
					if (base.PlayerWhoStarted == PlayerTurn.PlayerOne)
					{
						base.OnVictory("str_boardgame_victory_message");
					}
					else
					{
						base.OnDefeat("str_boardgame_defeat_message");
					}
					this.ReadyToPlay = false;
					flag = true;
				}
				else
				{
					int num = 0;
					using (List<PawnBase>.Enumerator enumerator = this._goatUnits.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (((PawnBaghChal)enumerator.Current).Captured)
							{
								num++;
							}
						}
					}
					if (num >= 5)
					{
						if (base.PlayerWhoStarted == PlayerTurn.PlayerOne)
						{
							base.OnDefeat("str_boardgame_defeat_message");
						}
						else
						{
							base.OnVictory("str_boardgame_victory_message");
						}
						this.ReadyToPlay = false;
						flag = true;
					}
				}
			}
			return flag;
		}

		// Token: 0x06000B53 RID: 2899 RVA: 0x00054488 File Offset: 0x00052688
		protected override void OnAfterBoardRotated()
		{
			this.PreplaceUnits();
		}

		// Token: 0x06000B54 RID: 2900 RVA: 0x00054490 File Offset: 0x00052690
		protected override void OnAfterBoardSetUp()
		{
			this.ReadyToPlay = true;
		}

		// Token: 0x06000B55 RID: 2901 RVA: 0x0005449C File Offset: 0x0005269C
		protected override void MovePawnToTileDelayed(PawnBase pawn, TileBase tile, bool instantMove, bool displayMessage, float delay)
		{
			base.MovePawnToTileDelayed(pawn, tile, instantMove, displayMessage, delay);
			Tile2D tile2D = tile as Tile2D;
			PawnBaghChal pawnBaghChal = pawn as PawnBaghChal;
			if (tile2D.PawnOnTile == null && pawnBaghChal != null)
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
				Vec3 globalPosition = tile2D.Entity.GlobalPosition;
				float num = 0.5f;
				if (!base.InPreMovementStage)
				{
					num = 0.3f;
				}
				pawnBaghChal.MovingToDifferentTile = pawnBaghChal.X != tile2D.X || pawnBaghChal.Y != tile2D.Y;
				pawnBaghChal.PrevX = pawnBaghChal.X;
				pawnBaghChal.PrevY = pawnBaghChal.Y;
				pawnBaghChal.X = tile2D.X;
				pawnBaghChal.Y = tile2D.Y;
				if (pawnBaghChal.PrevX != -1 && pawnBaghChal.PrevY != -1)
				{
					this.GetTile(pawnBaghChal.PrevX, pawnBaghChal.PrevY).PawnOnTile = null;
				}
				tile2D.PawnOnTile = pawnBaghChal;
				if (pawnBaghChal.Entity.GlobalPosition.z < globalPosition.z)
				{
					Vec3 globalPosition2 = pawnBaghChal.Entity.GlobalPosition;
					globalPosition2.z = globalPosition.z;
					pawnBaghChal.AddGoalPosition(globalPosition2);
				}
				pawnBaghChal.AddGoalPosition(globalPosition);
				pawnBaghChal.MovePawnToGoalPositionsDelayed(instantMove, num, this.JustStoppedDraggingUnit, delay);
				if (instantMove && !base.InPreMovementStage)
				{
					this.CheckIfPawnCaptures(this.SelectedUnit as PawnBaghChal, false);
					return;
				}
				if (pawnBaghChal == this.SelectedUnit && instantMove)
				{
					this.SelectedUnit = null;
				}
			}
		}

		// Token: 0x06000B56 RID: 2902 RVA: 0x00054644 File Offset: 0x00052844
		public void AIMakeMove(Move move)
		{
			Tile2D tile2D = move.GoalTile as Tile2D;
			PawnBaghChal pawnBaghChal = move.Unit as PawnBaghChal;
			if (tile2D.PawnOnTile == null)
			{
				pawnBaghChal.PrevX = pawnBaghChal.X;
				pawnBaghChal.PrevY = pawnBaghChal.Y;
				pawnBaghChal.X = tile2D.X;
				pawnBaghChal.Y = tile2D.Y;
				if (pawnBaghChal.PrevX != -1 && pawnBaghChal.PrevY != -1)
				{
					this.GetTile(pawnBaghChal.PrevX, pawnBaghChal.PrevY).PawnOnTile = null;
				}
				tile2D.PawnOnTile = pawnBaghChal;
				this.CheckIfPawnCaptures(pawnBaghChal, true);
			}
		}

		// Token: 0x06000B57 RID: 2903 RVA: 0x000546DC File Offset: 0x000528DC
		public BoardGameBaghChal.BoardInformation TakeBoardSnapshot()
		{
			BoardGameBaghChal.PawnInformation[] array = new BoardGameBaghChal.PawnInformation[base.PlayerOneUnits.Count + base.PlayerTwoUnits.Count];
			TileBaseInformation[,] array2 = new TileBaseInformation[BoardGameBaghChal.BoardWidth, BoardGameBaghChal.BoardHeight];
			int num = 0;
			foreach (PawnBase pawnBase in this._goatUnits)
			{
				PawnBaghChal pawnBaghChal = (PawnBaghChal)pawnBase;
				array[num++] = new BoardGameBaghChal.PawnInformation(pawnBaghChal.X, pawnBaghChal.Y, pawnBaghChal.PrevX, pawnBaghChal.PrevY, pawnBaghChal.Captured, pawnBaghChal.Entity.GlobalPosition);
			}
			foreach (PawnBase pawnBase2 in this._tigerUnits)
			{
				PawnBaghChal pawnBaghChal2 = (PawnBaghChal)pawnBase2;
				array[num++] = new BoardGameBaghChal.PawnInformation(pawnBaghChal2.X, pawnBaghChal2.Y, pawnBaghChal2.PrevX, pawnBaghChal2.PrevY, pawnBaghChal2.Captured, pawnBaghChal2.Entity.GlobalPosition);
			}
			for (int i = 0; i < BoardGameBaghChal.BoardWidth; i++)
			{
				for (int j = 0; j < BoardGameBaghChal.BoardHeight; j++)
				{
					array2[i, j] = new TileBaseInformation(ref this.GetTile(i, j).PawnOnTile);
				}
			}
			return new BoardGameBaghChal.BoardInformation(ref array, ref array2);
		}

		// Token: 0x06000B58 RID: 2904 RVA: 0x0005486C File Offset: 0x00052A6C
		public void UndoMove(ref BoardGameBaghChal.BoardInformation board)
		{
			int num = 0;
			foreach (PawnBase pawnBase in this._goatUnits)
			{
				PawnBaghChal pawnBaghChal = (PawnBaghChal)pawnBase;
				pawnBaghChal.X = board.PawnInformation[num].X;
				pawnBaghChal.Y = board.PawnInformation[num].Y;
				pawnBaghChal.PrevX = board.PawnInformation[num].PrevX;
				pawnBaghChal.PrevY = board.PawnInformation[num].PrevY;
				pawnBaghChal.Captured = board.PawnInformation[num].Captured;
				num++;
			}
			foreach (PawnBase pawnBase2 in this._tigerUnits)
			{
				PawnBaghChal pawnBaghChal2 = (PawnBaghChal)pawnBase2;
				pawnBaghChal2.X = board.PawnInformation[num].X;
				pawnBaghChal2.Y = board.PawnInformation[num].Y;
				pawnBaghChal2.PrevX = board.PawnInformation[num].PrevX;
				pawnBaghChal2.PrevY = board.PawnInformation[num].PrevY;
				pawnBaghChal2.Captured = board.PawnInformation[num].Captured;
				num++;
			}
			for (int i = 0; i < BoardGameBaghChal.BoardWidth; i++)
			{
				for (int j = 0; j < BoardGameBaghChal.BoardHeight; j++)
				{
					this.GetTile(i, j).PawnOnTile = board.TileInformation[i, j].PawnOnTile;
				}
			}
		}

		// Token: 0x06000B59 RID: 2905 RVA: 0x00054A34 File Offset: 0x00052C34
		public PawnBaghChal GetANonePlacedGoat()
		{
			foreach (PawnBase pawnBase in this._goatUnits)
			{
				PawnBaghChal pawnBaghChal = (PawnBaghChal)pawnBase;
				if (!pawnBaghChal.Captured && !pawnBaghChal.IsPlaced)
				{
					return pawnBaghChal;
				}
			}
			return null;
		}

		// Token: 0x06000B5A RID: 2906 RVA: 0x00054A9C File Offset: 0x00052C9C
		protected void CheckIfPawnCaptures(PawnBaghChal pawn, bool fake = false)
		{
			if (!pawn.IsTiger)
			{
				return;
			}
			int x = pawn.X;
			int y = pawn.Y;
			int prevX = pawn.PrevX;
			int prevY = pawn.PrevY;
			if (x == -1 || y == -1 || prevX == -1 || prevY == -1)
			{
				Debug.FailedAssert("x == -1 || y == -1 || prevX == -1 || prevY == -1", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox\\BoardGames\\BoardGameBaghChal.cs", "CheckIfPawnCaptures", 819);
			}
			Vec2i vec2i = new Vec2i(x - prevX, y - prevY);
			Vec2i vec2i2 = new Vec2i(vec2i.X / 2, vec2i.Y / 2);
			int num = vec2i.X + vec2i.Y;
			if (x == prevX || y == prevY)
			{
				if (num == 1 || num == -1)
				{
					return;
				}
			}
			else if (vec2i.X == 1 || vec2i.X == -1)
			{
				return;
			}
			Vec2i vec2i3 = new Vec2i(x - vec2i2.X, y - vec2i2.Y);
			this.SetPawnCaptured(this.GetTile(vec2i3.X, vec2i3.Y).PawnOnTile, fake);
		}

		// Token: 0x06000B5B RID: 2907 RVA: 0x00054B90 File Offset: 0x00052D90
		private void PreplaceUnits()
		{
			this.MovePawnToTileDelayed(this._tigerUnits[0], this.GetTile(0, 0), false, false, 0.4f);
			this.MovePawnToTileDelayed(this._tigerUnits[1], this.GetTile(4, 0), false, false, 0.55f);
			this.MovePawnToTileDelayed(this._tigerUnits[2], this.GetTile(0, 4), false, false, 0.70000005f);
			this.MovePawnToTileDelayed(this._tigerUnits[3], this.GetTile(4, 4), false, false, 0.85f);
			for (int i = 0; i < 20; i++)
			{
				PawnBaghChal pawnBaghChal = this._goatUnits[i] as PawnBaghChal;
				MatrixFrame globalFrame = pawnBaghChal.Entity.GetGlobalFrame();
				MatrixFrame initialFrame = pawnBaghChal.InitialFrame;
				if (base.PlayerWhoStarted != PlayerTurn.PlayerOne)
				{
					initialFrame.rotation.RotateAboutUp(3.1415927f);
				}
				pawnBaghChal.Entity.SetFrame(ref initialFrame, true);
				Vec3 origin = pawnBaghChal.Entity.GetGlobalFrame().origin;
				pawnBaghChal.Entity.SetGlobalFrame(in globalFrame, true);
				if (!pawnBaghChal.Entity.GlobalPosition.NearlyEquals(in origin, 1E-05f))
				{
					Vec3 globalPosition = pawnBaghChal.Entity.GlobalPosition;
					globalPosition.z = this.BoardEntity.GlobalBoxMax.z;
					pawnBaghChal.AddGoalPosition(globalPosition);
					globalPosition.x = origin.x;
					globalPosition.y = origin.y;
					pawnBaghChal.AddGoalPosition(globalPosition);
					pawnBaghChal.AddGoalPosition(origin);
					pawnBaghChal.MovePawnToGoalPositions(false, 0.5f, false);
				}
			}
		}

		// Token: 0x06000B5C RID: 2908 RVA: 0x00054D24 File Offset: 0x00052F24
		private bool CheckPlacementStageOver()
		{
			bool flag = false;
			int num = 0;
			foreach (PawnBase pawnBase in this._goatUnits)
			{
				PawnBaghChal pawnBaghChal = (PawnBaghChal)pawnBase;
				if (pawnBaghChal.Captured || pawnBaghChal.IsPlaced)
				{
					num++;
				}
			}
			if (num == 20)
			{
				flag = true;
			}
			return flag;
		}

		// Token: 0x06000B5D RID: 2909 RVA: 0x00054D98 File Offset: 0x00052F98
		private void SetTile(TileBase tile, int x, int y)
		{
			base.Tiles[y * BoardGameBaghChal.BoardWidth + x] = tile;
		}

		// Token: 0x06000B5E RID: 2910 RVA: 0x00054DAB File Offset: 0x00052FAB
		private TileBase GetTile(int x, int y)
		{
			return base.Tiles[y * BoardGameBaghChal.BoardWidth + x];
		}

		// Token: 0x040004E0 RID: 1248
		public const int UnitCountTiger = 4;

		// Token: 0x040004E1 RID: 1249
		public const int UnitCountGoat = 20;

		// Token: 0x040004E2 RID: 1250
		public static readonly int BoardWidth = 5;

		// Token: 0x040004E3 RID: 1251
		public static readonly int BoardHeight = 5;

		// Token: 0x040004E4 RID: 1252
		private List<PawnBase> _goatUnits;

		// Token: 0x040004E5 RID: 1253
		private List<PawnBase> _tigerUnits;

		// Token: 0x0200020C RID: 524
		public struct BoardInformation
		{
			// Token: 0x060013E1 RID: 5089 RVA: 0x00078AEC File Offset: 0x00076CEC
			public BoardInformation(ref BoardGameBaghChal.PawnInformation[] pawns, ref TileBaseInformation[,] tiles)
			{
				this.PawnInformation = pawns;
				this.TileInformation = tiles;
			}

			// Token: 0x0400096E RID: 2414
			public readonly BoardGameBaghChal.PawnInformation[] PawnInformation;

			// Token: 0x0400096F RID: 2415
			public readonly TileBaseInformation[,] TileInformation;
		}

		// Token: 0x0200020D RID: 525
		public struct PawnInformation
		{
			// Token: 0x060013E2 RID: 5090 RVA: 0x00078AFE File Offset: 0x00076CFE
			public PawnInformation(int x, int y, int prevX, int prevY, bool captured, Vec3 position)
			{
				this.X = x;
				this.Y = y;
				this.PrevX = prevX;
				this.PrevY = prevY;
				this.Captured = captured;
				this.Position = position;
			}

			// Token: 0x04000970 RID: 2416
			public readonly int X;

			// Token: 0x04000971 RID: 2417
			public readonly int Y;

			// Token: 0x04000972 RID: 2418
			public readonly int PrevX;

			// Token: 0x04000973 RID: 2419
			public readonly int PrevY;

			// Token: 0x04000974 RID: 2420
			public readonly bool Captured;

			// Token: 0x04000975 RID: 2421
			public readonly Vec3 Position;
		}
	}
}
