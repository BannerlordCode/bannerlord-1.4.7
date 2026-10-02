using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Objects.Siege;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000360 RID: 864
	public class WallSegment : SynchedMissionObject, IPointDefendable, ICastleKeyPosition
	{
		// Token: 0x17000938 RID: 2360
		// (get) Token: 0x06003170 RID: 12656 RVA: 0x000C9984 File Offset: 0x000C7B84
		// (set) Token: 0x06003171 RID: 12657 RVA: 0x000C998C File Offset: 0x000C7B8C
		public TacticalPosition MiddlePosition { get; private set; }

		// Token: 0x17000939 RID: 2361
		// (get) Token: 0x06003172 RID: 12658 RVA: 0x000C9995 File Offset: 0x000C7B95
		// (set) Token: 0x06003173 RID: 12659 RVA: 0x000C999D File Offset: 0x000C7B9D
		public TacticalPosition WaitPosition { get; private set; }

		// Token: 0x1700093A RID: 2362
		// (get) Token: 0x06003174 RID: 12660 RVA: 0x000C99A6 File Offset: 0x000C7BA6
		// (set) Token: 0x06003175 RID: 12661 RVA: 0x000C99AE File Offset: 0x000C7BAE
		public TacticalPosition AttackerWaitPosition { get; private set; }

		// Token: 0x1700093B RID: 2363
		// (get) Token: 0x06003176 RID: 12662 RVA: 0x000C99B7 File Offset: 0x000C7BB7
		// (set) Token: 0x06003177 RID: 12663 RVA: 0x000C99BF File Offset: 0x000C7BBF
		public IPrimarySiegeWeapon AttackerSiegeWeapon { get; set; }

		// Token: 0x1700093C RID: 2364
		// (get) Token: 0x06003178 RID: 12664 RVA: 0x000C99C8 File Offset: 0x000C7BC8
		// (set) Token: 0x06003179 RID: 12665 RVA: 0x000C99D0 File Offset: 0x000C7BD0
		public IEnumerable<DefencePoint> DefencePoints { get; protected set; }

		// Token: 0x1700093D RID: 2365
		// (get) Token: 0x0600317A RID: 12666 RVA: 0x000C99D9 File Offset: 0x000C7BD9
		// (set) Token: 0x0600317B RID: 12667 RVA: 0x000C99E1 File Offset: 0x000C7BE1
		public bool IsBreachedWall { get; private set; }

		// Token: 0x1700093E RID: 2366
		// (get) Token: 0x0600317C RID: 12668 RVA: 0x000C99EA File Offset: 0x000C7BEA
		// (set) Token: 0x0600317D RID: 12669 RVA: 0x000C99F2 File Offset: 0x000C7BF2
		public WorldFrame MiddleFrame { get; private set; }

		// Token: 0x1700093F RID: 2367
		// (get) Token: 0x0600317E RID: 12670 RVA: 0x000C99FB File Offset: 0x000C7BFB
		// (set) Token: 0x0600317F RID: 12671 RVA: 0x000C9A03 File Offset: 0x000C7C03
		public WorldFrame DefenseWaitFrame { get; private set; }

		// Token: 0x17000940 RID: 2368
		// (get) Token: 0x06003180 RID: 12672 RVA: 0x000C9A0C File Offset: 0x000C7C0C
		// (set) Token: 0x06003181 RID: 12673 RVA: 0x000C9A14 File Offset: 0x000C7C14
		public WorldFrame AttackerWaitFrame { get; private set; } = WorldFrame.Invalid;

		// Token: 0x17000941 RID: 2369
		// (get) Token: 0x06003182 RID: 12674 RVA: 0x000C9A1D File Offset: 0x000C7C1D
		// (set) Token: 0x06003183 RID: 12675 RVA: 0x000C9A25 File Offset: 0x000C7C25
		public FormationAI.BehaviorSide DefenseSide { get; private set; }

		// Token: 0x06003184 RID: 12676 RVA: 0x000C9A30 File Offset: 0x000C7C30
		public Vec3 GetPosition()
		{
			return base.GameEntity.GlobalPosition;
		}

		// Token: 0x06003185 RID: 12677 RVA: 0x000C9A4C File Offset: 0x000C7C4C
		public WallSegment()
		{
			this.AttackerSiegeWeapon = null;
		}

		// Token: 0x06003186 RID: 12678 RVA: 0x000C9AB0 File Offset: 0x000C7CB0
		protected internal override void OnInit()
		{
			base.OnInit();
			string sideTag = this.SideTag;
			if (!(sideTag == "left"))
			{
				if (!(sideTag == "middle"))
				{
					if (!(sideTag == "right"))
					{
						this.DefenseSide = FormationAI.BehaviorSide.BehaviorSideNotSet;
					}
					else
					{
						this.DefenseSide = FormationAI.BehaviorSide.Right;
					}
				}
				else
				{
					this.DefenseSide = FormationAI.BehaviorSide.Middle;
				}
			}
			else
			{
				this.DefenseSide = FormationAI.BehaviorSide.Left;
			}
			WeakGameEntity weakGameEntity = base.GameEntity.GetChildren().FirstOrDefault<WeakGameEntity>((WeakGameEntity ce) => ce.HasTag("solid_child"));
			List<WeakGameEntity> list = new List<WeakGameEntity>();
			List<WeakGameEntity> list2 = new List<WeakGameEntity>();
			if (weakGameEntity.IsValid)
			{
				list = weakGameEntity.CollectChildrenEntitiesWithTag("middle_pos");
				list2 = weakGameEntity.CollectChildrenEntitiesWithTag("wait_pos");
			}
			else
			{
				list = base.GameEntity.CollectChildrenEntitiesWithTag("middle_pos");
				list2 = base.GameEntity.CollectChildrenEntitiesWithTag("wait_pos");
			}
			MatrixFrame matrixFrame;
			if (list.Count > 0)
			{
				WeakGameEntity weakGameEntity2 = list[0];
				this.MiddlePosition = weakGameEntity2.GetFirstScriptOfType<TacticalPosition>();
				matrixFrame = weakGameEntity2.GetGlobalFrame();
			}
			else
			{
				matrixFrame = base.GameEntity.GetGlobalFrame();
			}
			this.MiddleFrame = new WorldFrame(matrixFrame.rotation, matrixFrame.origin.ToWorldPosition());
			if (list2.Count > 0)
			{
				WeakGameEntity weakGameEntity3 = list2[0];
				this.WaitPosition = weakGameEntity3.GetFirstScriptOfType<TacticalPosition>();
				matrixFrame = weakGameEntity3.GetGlobalFrame();
				this.DefenseWaitFrame = new WorldFrame(matrixFrame.rotation, matrixFrame.origin.ToWorldPosition());
				return;
			}
			this.DefenseWaitFrame = this.MiddleFrame;
		}

		// Token: 0x06003187 RID: 12679 RVA: 0x000C9C4D File Offset: 0x000C7E4D
		protected internal override bool MovesEntity()
		{
			return false;
		}

		// Token: 0x06003188 RID: 12680 RVA: 0x000C9C50 File Offset: 0x000C7E50
		public void OnChooseUsedWallSegment(bool isBroken)
		{
			WeakGameEntity firstChildEntityWithTag = base.GameEntity.GetFirstChildEntityWithTag("solid_child");
			WeakGameEntity firstChildEntityWithTag2 = base.GameEntity.GetFirstChildEntityWithTag("broken_child");
			Scene scene = base.GameEntity.Scene;
			if (isBroken)
			{
				firstChildEntityWithTag.GetFirstScriptOfType<WallSegment>().SetDisabledSynched();
				firstChildEntityWithTag2.GetFirstScriptOfType<WallSegment>().SetVisibleSynched(true, false);
				if (!GameNetwork.IsClientOrReplay)
				{
					if (this._properGroundOutsideNavmeshID > 0 && this._underDebrisOutsideNavmeshID > 0)
					{
						scene.SeparateFacesWithId(this._properGroundOutsideNavmeshID, this._underDebrisOutsideNavmeshID);
					}
					if (this._properGroundInsideNavmeshID > 0 && this._underDebrisInsideNavmeshID > 0)
					{
						scene.SeparateFacesWithId(this._properGroundInsideNavmeshID, this._underDebrisInsideNavmeshID);
					}
					if (this._underDebrisOutsideNavmeshID > 0)
					{
						scene.SetAbilityOfFacesWithId(this._underDebrisOutsideNavmeshID, false);
					}
					if (this._underDebrisInsideNavmeshID > 0)
					{
						scene.SetAbilityOfFacesWithId(this._underDebrisInsideNavmeshID, false);
					}
					if (this._underDebrisGenericNavmeshID > 0)
					{
						scene.SetAbilityOfFacesWithId(this._underDebrisGenericNavmeshID, false);
					}
					if (this._overDebrisOutsideNavmeshID > 0)
					{
						scene.SetAbilityOfFacesWithId(this._overDebrisOutsideNavmeshID, true);
						if (this._properGroundOutsideNavmeshID > 0)
						{
							scene.MergeFacesWithId(this._overDebrisOutsideNavmeshID, this._properGroundOutsideNavmeshID, 0);
						}
					}
					if (this._overDebrisInsideNavmeshID > 0)
					{
						scene.SetAbilityOfFacesWithId(this._overDebrisInsideNavmeshID, true);
						if (this._properGroundInsideNavmeshID > 0)
						{
							scene.MergeFacesWithId(this._overDebrisInsideNavmeshID, this._properGroundInsideNavmeshID, 1);
						}
					}
					if (this._overDebrisGenericNavmeshID > 0)
					{
						scene.SetAbilityOfFacesWithId(this._overDebrisGenericNavmeshID, true);
					}
					if (this._onSolidWallGenericNavmeshID > 0)
					{
						scene.SetAbilityOfFacesWithId(this._onSolidWallGenericNavmeshID, false);
					}
					foreach (StrategicArea strategicArea in from c in firstChildEntityWithTag.GetChildren()
						where c.HasScriptOfType<StrategicArea>()
						select c.GetFirstScriptOfType<StrategicArea>())
					{
						strategicArea.OnParentGameEntityVisibilityChanged(false);
					}
					foreach (StrategicArea strategicArea2 in from c in firstChildEntityWithTag2.GetChildren()
						where c.HasScriptOfType<StrategicArea>()
						select c.GetFirstScriptOfType<StrategicArea>())
					{
						strategicArea2.OnParentGameEntityVisibilityChanged(true);
					}
				}
				this.IsBreachedWall = true;
				List<WeakGameEntity> list = firstChildEntityWithTag2.CollectChildrenEntitiesWithTag("middle_pos");
				if (list.Count > 0)
				{
					WeakGameEntity weakGameEntity = list.FirstOrDefault<WeakGameEntity>();
					this.MiddlePosition = weakGameEntity.GetFirstScriptOfType<TacticalPosition>();
					MatrixFrame globalFrame = weakGameEntity.GetGlobalFrame();
					this.MiddleFrame = new WorldFrame(globalFrame.rotation, globalFrame.origin.ToWorldPosition());
				}
				else
				{
					MBDebug.ShowWarning("Broken child of wall does not have middle position");
					MatrixFrame globalFrame2 = firstChildEntityWithTag2.GetGlobalFrame();
					this.MiddleFrame = new WorldFrame(globalFrame2.rotation, new WorldPosition(scene, UIntPtr.Zero, globalFrame2.origin, false));
				}
				List<WeakGameEntity> list2 = firstChildEntityWithTag2.CollectChildrenEntitiesWithTag("wait_pos");
				if (list2.Count > 0)
				{
					WeakGameEntity weakGameEntity2 = list2.FirstOrDefault<WeakGameEntity>();
					this.WaitPosition = weakGameEntity2.GetFirstScriptOfType<TacticalPosition>();
					MatrixFrame globalFrame3 = weakGameEntity2.GetGlobalFrame();
					this.DefenseWaitFrame = new WorldFrame(globalFrame3.rotation, globalFrame3.origin.ToWorldPosition());
				}
				else
				{
					this.DefenseWaitFrame = this.MiddleFrame;
				}
				WallSegment firstScriptOfType = firstChildEntityWithTag.GetFirstScriptOfType<WallSegment>();
				if (firstScriptOfType != null)
				{
					firstScriptOfType.SetDisabledAndMakeInvisible(true, false);
				}
				WeakGameEntity weakGameEntity3 = firstChildEntityWithTag2.CollectChildrenEntitiesWithTag("attacker_wait_pos").FirstOrDefault<WeakGameEntity>();
				if (weakGameEntity3.IsValid)
				{
					MatrixFrame globalFrame4 = weakGameEntity3.GetGlobalFrame();
					this.AttackerWaitFrame = new WorldFrame(globalFrame4.rotation, globalFrame4.origin.ToWorldPosition());
					this.AttackerWaitPosition = weakGameEntity3.GetFirstScriptOfType<TacticalPosition>();
					return;
				}
			}
			else if (!GameNetwork.IsClientOrReplay)
			{
				firstChildEntityWithTag.GetFirstScriptOfType<WallSegment>().SetVisibleSynched(true, false);
				firstChildEntityWithTag2.GetFirstScriptOfType<WallSegment>().SetDisabledSynched();
				if (this._overDebrisOutsideNavmeshID > 0)
				{
					scene.SetAbilityOfFacesWithId(this._overDebrisOutsideNavmeshID, false);
				}
				if (this._overDebrisInsideNavmeshID > 0)
				{
					scene.SetAbilityOfFacesWithId(this._overDebrisInsideNavmeshID, false);
				}
				if (this._overDebrisGenericNavmeshID > 0)
				{
					scene.SetAbilityOfFacesWithId(this._overDebrisGenericNavmeshID, false);
				}
				foreach (StrategicArea strategicArea3 in from c in firstChildEntityWithTag.GetChildren()
					where c.HasScriptOfType<StrategicArea>()
					select c.GetFirstScriptOfType<StrategicArea>())
				{
					strategicArea3.OnParentGameEntityVisibilityChanged(true);
				}
				foreach (StrategicArea strategicArea4 in from c in firstChildEntityWithTag2.GetChildren()
					where c.HasScriptOfType<StrategicArea>()
					select c.GetFirstScriptOfType<StrategicArea>())
				{
					strategicArea4.OnParentGameEntityVisibilityChanged(false);
				}
			}
		}

		// Token: 0x06003189 RID: 12681 RVA: 0x000CA1D0 File Offset: 0x000C83D0
		protected internal override void OnEditorValidate()
		{
			base.OnEditorValidate();
		}

		// Token: 0x0600318A RID: 12682 RVA: 0x000CA1D8 File Offset: 0x000C83D8
		protected internal override bool OnCheckForProblems()
		{
			bool flag = base.OnCheckForProblems();
			if (!base.Scene.IsMultiplayerScene() && this.SideTag == "left")
			{
				List<GameEntity> list = new List<GameEntity>();
				base.Scene.GetEntities(ref list);
				int num = 0;
				foreach (GameEntity gameEntity in list)
				{
					if (base.GameEntity.GetUpgradeLevelOfEntity() == gameEntity.GetUpgradeLevelOfEntity() && gameEntity.GetFirstScriptOfType<SiegeLadderSpawner>() != null)
					{
						num++;
					}
				}
				if (num != 4)
				{
					MBEditor.AddEntityWarning(base.GameEntity, "The siege ladder count in the scene is not 4, for upgrade level " + base.GameEntity.GetUpgradeLevelOfEntity().ToString() + ". Current siege ladder count: " + num.ToString());
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x040014D8 RID: 5336
		private const string WaitPositionTag = "wait_pos";

		// Token: 0x040014D9 RID: 5337
		private const string MiddlePositionTag = "middle_pos";

		// Token: 0x040014DA RID: 5338
		private const string AttackerWaitPositionTag = "attacker_wait_pos";

		// Token: 0x040014DB RID: 5339
		private const string SolidChildTag = "solid_child";

		// Token: 0x040014DC RID: 5340
		private const string BrokenChildTag = "broken_child";

		// Token: 0x040014DD RID: 5341
		[EditableScriptComponentVariable(true, "")]
		private int _properGroundOutsideNavmeshID = -1;

		// Token: 0x040014DE RID: 5342
		[EditableScriptComponentVariable(true, "")]
		private int _properGroundInsideNavmeshID = -1;

		// Token: 0x040014DF RID: 5343
		[EditableScriptComponentVariable(true, "")]
		private int _underDebrisOutsideNavmeshID = -1;

		// Token: 0x040014E0 RID: 5344
		[EditableScriptComponentVariable(true, "")]
		private int _underDebrisInsideNavmeshID = -1;

		// Token: 0x040014E1 RID: 5345
		[EditableScriptComponentVariable(true, "")]
		private int _overDebrisOutsideNavmeshID = -1;

		// Token: 0x040014E2 RID: 5346
		[EditableScriptComponentVariable(true, "")]
		private int _overDebrisInsideNavmeshID = -1;

		// Token: 0x040014E3 RID: 5347
		[EditableScriptComponentVariable(true, "")]
		private int _underDebrisGenericNavmeshID = -1;

		// Token: 0x040014E4 RID: 5348
		[EditableScriptComponentVariable(true, "")]
		private int _overDebrisGenericNavmeshID = -1;

		// Token: 0x040014E5 RID: 5349
		[EditableScriptComponentVariable(true, "")]
		private int _onSolidWallGenericNavmeshID = -1;

		// Token: 0x040014EB RID: 5355
		public string SideTag;
	}
}
