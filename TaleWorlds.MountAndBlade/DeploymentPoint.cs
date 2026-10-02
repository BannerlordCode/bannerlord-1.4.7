using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Missions;
using TaleWorlds.MountAndBlade.Objects.Siege;
using TaleWorlds.MountAndBlade.Objects.Usables;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000342 RID: 834
	public class DeploymentPoint : SynchedMissionObject
	{
		// Token: 0x1400009A RID: 154
		// (add) Token: 0x06002EEE RID: 12014 RVA: 0x000B6C74 File Offset: 0x000B4E74
		// (remove) Token: 0x06002EEF RID: 12015 RVA: 0x000B6CAC File Offset: 0x000B4EAC
		public event Action<DeploymentPoint, SynchedMissionObject> OnDeploymentStateChanged;

		// Token: 0x1400009B RID: 155
		// (add) Token: 0x06002EF0 RID: 12016 RVA: 0x000B6CE4 File Offset: 0x000B4EE4
		// (remove) Token: 0x06002EF1 RID: 12017 RVA: 0x000B6D1C File Offset: 0x000B4F1C
		public event Action<DeploymentPoint> OnDeploymentPointTypeDetermined;

		// Token: 0x1400009C RID: 156
		// (add) Token: 0x06002EF2 RID: 12018 RVA: 0x000B6D54 File Offset: 0x000B4F54
		// (remove) Token: 0x06002EF3 RID: 12019 RVA: 0x000B6D8C File Offset: 0x000B4F8C
		public event Action<DeploymentPoint> OnDeployOrDisband;

		// Token: 0x170008BC RID: 2236
		// (get) Token: 0x06002EF4 RID: 12020 RVA: 0x000B6DC1 File Offset: 0x000B4FC1
		// (set) Token: 0x06002EF5 RID: 12021 RVA: 0x000B6DC9 File Offset: 0x000B4FC9
		public Vec3 DeploymentTargetPosition { get; private set; }

		// Token: 0x170008BD RID: 2237
		// (get) Token: 0x06002EF6 RID: 12022 RVA: 0x000B6DD2 File Offset: 0x000B4FD2
		// (set) Token: 0x06002EF7 RID: 12023 RVA: 0x000B6DDA File Offset: 0x000B4FDA
		public WallSegment AssociatedWallSegment { get; private set; }

		// Token: 0x170008BE RID: 2238
		// (get) Token: 0x06002EF8 RID: 12024 RVA: 0x000B6DE3 File Offset: 0x000B4FE3
		public IEnumerable<SynchedMissionObject> DeployableWeapons
		{
			get
			{
				return this._weapons.Where<SynchedMissionObject>((SynchedMissionObject w) => !w.IsDisabled);
			}
		}

		// Token: 0x170008BF RID: 2239
		// (get) Token: 0x06002EF9 RID: 12025 RVA: 0x000B6E0F File Offset: 0x000B500F
		public bool IsDeployed
		{
			get
			{
				return this.DeployedWeapon != null;
			}
		}

		// Token: 0x170008C0 RID: 2240
		// (get) Token: 0x06002EFA RID: 12026 RVA: 0x000B6E1A File Offset: 0x000B501A
		// (set) Token: 0x06002EFB RID: 12027 RVA: 0x000B6E22 File Offset: 0x000B5022
		public SynchedMissionObject DeployedWeapon { get; private set; }

		// Token: 0x170008C1 RID: 2241
		// (get) Token: 0x06002EFC RID: 12028 RVA: 0x000B6E2B File Offset: 0x000B502B
		// (set) Token: 0x06002EFD RID: 12029 RVA: 0x000B6E33 File Offset: 0x000B5033
		public SynchedMissionObject DisbandedWeapon { get; private set; }

		// Token: 0x06002EFE RID: 12030 RVA: 0x000B6E3C File Offset: 0x000B503C
		protected internal override void OnInit()
		{
			this._weapons = new MBList<SynchedMissionObject>();
		}

		// Token: 0x06002EFF RID: 12031 RVA: 0x000B6E4C File Offset: 0x000B504C
		public override void AfterMissionStart()
		{
			base.OnInit();
			if (!GameNetwork.IsClientOrReplay)
			{
				this._weapons = this.GetWeaponsUnder();
				this._associatedSiegeLadders = new List<SiegeLadder>();
				if (this.DeployableWeapons.IsEmpty<SynchedMissionObject>())
				{
					this.SetVisibleSynched(false, false);
					this.SetBreachSideDeploymentPoint();
				}
				base.AfterMissionStart();
				if (!GameNetwork.IsClientOrReplay)
				{
					this.DetermineDeploymentPointType();
				}
				this.HideAllWeapons();
			}
		}

		// Token: 0x06002F00 RID: 12032 RVA: 0x000B6EB4 File Offset: 0x000B50B4
		private void SetBreachSideDeploymentPoint()
		{
			Debug.Print("Deployment point " + (base.GameEntity.IsValid ? ("upgrade level mask " + base.GameEntity.GetUpgradeLevelMask().ToString()) : "no game entity.") + "\n", 0, Debug.DebugColor.White, 17592186044416UL);
			this._isBreachSideDeploymentPoint = true;
			this._deploymentPointType = DeploymentPoint.DeploymentPointType.Breach;
			FormationAI.BehaviorSide deploymentPointSide = (this._weapons.FirstOrDefault<SynchedMissionObject>((SynchedMissionObject w) => w is SiegeTower) as IPrimarySiegeWeapon).WeaponSide;
			this.AssociatedWallSegment = Mission.Current.ActiveMissionObjects.FindAllWithType<WallSegment>().FirstOrDefault<WallSegment>((WallSegment ws) => ws.DefenseSide == deploymentPointSide);
			this.DeploymentTargetPosition = this.AssociatedWallSegment.GameEntity.GlobalPosition;
		}

		// Token: 0x06002F01 RID: 12033 RVA: 0x000B6FAC File Offset: 0x000B51AC
		public Vec3 GetDeploymentOrigin()
		{
			return base.GameEntity.GlobalPosition;
		}

		// Token: 0x06002F02 RID: 12034 RVA: 0x000B6FC8 File Offset: 0x000B51C8
		public DeploymentPoint.DeploymentPointState GetDeploymentPointState()
		{
			switch (this._deploymentPointType)
			{
			case DeploymentPoint.DeploymentPointType.BatteringRam:
				if (!this.IsDeployed)
				{
					return DeploymentPoint.DeploymentPointState.NotDeployed;
				}
				return DeploymentPoint.DeploymentPointState.BatteringRam;
			case DeploymentPoint.DeploymentPointType.TowerLadder:
				if (!this.IsDeployed)
				{
					return DeploymentPoint.DeploymentPointState.SiegeLadder;
				}
				return DeploymentPoint.DeploymentPointState.SiegeTower;
			case DeploymentPoint.DeploymentPointType.Breach:
				return DeploymentPoint.DeploymentPointState.Breach;
			case DeploymentPoint.DeploymentPointType.Ranged:
				if (!this.IsDeployed)
				{
					return DeploymentPoint.DeploymentPointState.NotDeployed;
				}
				return DeploymentPoint.DeploymentPointState.Ranged;
			default:
				MBDebug.ShowWarning("Undefined deployment point type fetched.");
				return DeploymentPoint.DeploymentPointState.NotDeployed;
			}
		}

		// Token: 0x06002F03 RID: 12035 RVA: 0x000B7025 File Offset: 0x000B5225
		public DeploymentPoint.DeploymentPointType GetDeploymentPointType()
		{
			return this._deploymentPointType;
		}

		// Token: 0x06002F04 RID: 12036 RVA: 0x000B702D File Offset: 0x000B522D
		public List<SiegeLadder> GetAssociatedSiegeLadders()
		{
			return this._associatedSiegeLadders;
		}

		// Token: 0x06002F05 RID: 12037 RVA: 0x000B7038 File Offset: 0x000B5238
		private void DetermineDeploymentPointType()
		{
			if (this._isBreachSideDeploymentPoint)
			{
				this._deploymentPointType = DeploymentPoint.DeploymentPointType.Breach;
			}
			else if (this._weapons.Any<SynchedMissionObject>((SynchedMissionObject w) => w is BatteringRam))
			{
				this._deploymentPointType = DeploymentPoint.DeploymentPointType.BatteringRam;
				this.DeploymentTargetPosition = (this._weapons.First<SynchedMissionObject>((SynchedMissionObject w) => w is BatteringRam) as IPrimarySiegeWeapon).TargetCastlePosition.GameEntity.GlobalPosition;
			}
			else if (this._weapons.Any<SynchedMissionObject>((SynchedMissionObject w) => w is SiegeTower))
			{
				SiegeTower tower = this._weapons.FirstOrDefault<SynchedMissionObject>((SynchedMissionObject w) => w is SiegeTower) as SiegeTower;
				this._deploymentPointType = DeploymentPoint.DeploymentPointType.TowerLadder;
				this.DeploymentTargetPosition = tower.TargetCastlePosition.GameEntity.GlobalPosition;
				this._associatedSiegeLadders = (from sl in Mission.Current.ActiveMissionObjects.FindAllWithType<SiegeLadder>()
					where sl.WeaponSide == tower.WeaponSide
					select sl).ToList<SiegeLadder>();
			}
			else
			{
				this._deploymentPointType = DeploymentPoint.DeploymentPointType.Ranged;
				this.DeploymentTargetPosition = Vec3.Invalid;
			}
			Action<DeploymentPoint> onDeploymentPointTypeDetermined = this.OnDeploymentPointTypeDetermined;
			if (onDeploymentPointTypeDetermined == null)
			{
				return;
			}
			onDeploymentPointTypeDetermined(this);
		}

		// Token: 0x06002F06 RID: 12038 RVA: 0x000B71B8 File Offset: 0x000B53B8
		public MBList<SynchedMissionObject> GetWeaponsUnder()
		{
			TeamAISiegeComponent teamAISiegeComponent;
			List<SiegeWeapon> list;
			if ((teamAISiegeComponent = Mission.Current.Teams[0].TeamAI as TeamAISiegeComponent) != null)
			{
				list = teamAISiegeComponent.SceneSiegeWeapons;
			}
			else
			{
				List<GameEntity> list2 = new List<GameEntity>();
				base.GameEntity.Scene.GetEntities(ref list2);
				list = (from se in list2
					where se.HasScriptOfType<SiegeWeapon>()
					select se.GetScriptComponents<SiegeWeapon>().FirstOrDefault<SiegeWeapon>()).ToList<SiegeWeapon>();
			}
			MBList<SynchedMissionObject> mblist = new MBList<SynchedMissionObject>();
			float num = this.Radius * this.Radius;
			foreach (SiegeWeapon siegeWeapon in list)
			{
				if (siegeWeapon.GameEntity.HasTag(this.SiegeWeaponTag) || (siegeWeapon.GameEntity.Parent.IsValid && siegeWeapon.GameEntity.Parent.HasTag(this.SiegeWeaponTag)) || (siegeWeapon.GameEntity != base.GameEntity && siegeWeapon.GameEntity.GlobalPosition.DistanceSquared(base.GameEntity.GlobalPosition) < num))
				{
					mblist.Add(siegeWeapon);
				}
			}
			return mblist;
		}

		// Token: 0x06002F07 RID: 12039 RVA: 0x000B734C File Offset: 0x000B554C
		public IEnumerable<SpawnerBase> GetSpawnersForEditor()
		{
			List<GameEntity> list = new List<GameEntity>();
			base.GameEntity.Scene.GetEntities(ref list);
			IEnumerable<SpawnerBase> enumerable = from se in list
				where se.HasScriptOfType<SpawnerBase>()
				select se.GetScriptComponents<SpawnerBase>().FirstOrDefault<SpawnerBase>();
			IEnumerable<SpawnerBase> enumerable2 = from ssw in enumerable
				where ssw.GameEntity.HasTag(this.SiegeWeaponTag)
				select (ssw);
			Vec3 globalPosition = base.GameEntity.GlobalPosition;
			float radiusSquared = this.Radius * this.Radius;
			IEnumerable<SpawnerBase> enumerable3 = from ssw in enumerable
				where ssw.GameEntity != this.GameEntity && ssw.GameEntity.GlobalPosition.DistanceSquared(globalPosition) < radiusSquared
				select (ssw);
			return enumerable2.Concat<SpawnerBase>(enumerable3).Distinct<SpawnerBase>();
		}

		// Token: 0x06002F08 RID: 12040 RVA: 0x000B746C File Offset: 0x000B566C
		protected internal override void OnEditorInit()
		{
			base.OnEditorInit();
			this._weapons = null;
		}

		// Token: 0x06002F09 RID: 12041 RVA: 0x000B747C File Offset: 0x000B567C
		protected internal override void OnEditorTick(float dt)
		{
			base.OnEditorTick(dt);
			foreach (GameEntity gameEntity in this._highlightedEntites)
			{
				gameEntity.SetContourColor(null, true);
			}
			this._highlightedEntites.Clear();
			if (MBEditor.IsEntitySelected(base.GameEntity))
			{
				uint num = 4294901760U;
				if (this.Radius > 0f)
				{
					DebugExtensions.RenderDebugCircleOnTerrain(base.Scene, base.GameEntity.GetGlobalFrame(), this.Radius, num, true, false);
				}
				foreach (SpawnerBase spawnerBase in this.GetSpawnersForEditor())
				{
					spawnerBase.GameEntity.SetContourColor(new uint?(num), true);
					this._highlightedEntites.Add(TaleWorlds.Engine.GameEntity.CreateFromWeakEntity(spawnerBase.GameEntity));
				}
			}
		}

		// Token: 0x06002F0A RID: 12042 RVA: 0x000B7598 File Offset: 0x000B5798
		private void OnDeploymentStateChangedAux(SynchedMissionObject targetObject)
		{
			if (this.IsDeployed)
			{
				targetObject.SetVisibleSynched(true, false);
				targetObject.SetPhysicsStateSynched(true, true);
			}
			else
			{
				targetObject.SetVisibleSynched(false, false);
				targetObject.SetPhysicsStateSynched(false, true);
			}
			Action<DeploymentPoint, SynchedMissionObject> onDeploymentStateChanged = this.OnDeploymentStateChanged;
			if (onDeploymentStateChanged != null)
			{
				onDeploymentStateChanged(this, targetObject);
			}
			SiegeWeapon siegeWeapon;
			if ((siegeWeapon = targetObject as SiegeWeapon) != null)
			{
				siegeWeapon.OnDeploymentStateChanged(this.IsDeployed);
			}
		}

		// Token: 0x06002F0B RID: 12043 RVA: 0x000B75F8 File Offset: 0x000B57F8
		public void Deploy(Type t)
		{
			this.DeployedWeapon = this._weapons.First<SynchedMissionObject>((SynchedMissionObject w) => MissionSiegeWeaponsController.GetWeaponType(w) == t);
			this.OnDeploymentStateChangedAux(this.DeployedWeapon);
			this.ToggleDeploymentPointVisibility(false);
			this.ToggleDeployedWeaponVisibility(true);
			Action<DeploymentPoint> onDeployOrDisband = this.OnDeployOrDisband;
			if (onDeployOrDisband == null)
			{
				return;
			}
			onDeployOrDisband(this);
		}

		// Token: 0x06002F0C RID: 12044 RVA: 0x000B765A File Offset: 0x000B585A
		public void Deploy(SiegeWeapon s)
		{
			this.DeployedWeapon = s;
			this.DisbandedWeapon = null;
			this.OnDeploymentStateChangedAux(s);
			this.ToggleDeploymentPointVisibility(false);
			this.ToggleDeployedWeaponVisibility(true);
			Action<DeploymentPoint> onDeployOrDisband = this.OnDeployOrDisband;
			if (onDeployOrDisband == null)
			{
				return;
			}
			onDeployOrDisband(this);
		}

		// Token: 0x06002F0D RID: 12045 RVA: 0x000B7690 File Offset: 0x000B5890
		public ScriptComponentBehavior Disband()
		{
			this.ToggleDeploymentPointVisibility(true);
			this.ToggleDeployedWeaponVisibility(false);
			this.DisbandedWeapon = this.DeployedWeapon;
			this.DeployedWeapon = null;
			this.OnDeploymentStateChangedAux(this.DisbandedWeapon);
			Action<DeploymentPoint> onDeployOrDisband = this.OnDeployOrDisband;
			if (onDeployOrDisband != null)
			{
				onDeployOrDisband(this);
			}
			return this.DisbandedWeapon;
		}

		// Token: 0x170008C2 RID: 2242
		// (get) Token: 0x06002F0E RID: 12046 RVA: 0x000B76E2 File Offset: 0x000B58E2
		public IEnumerable<Type> DeployableWeaponTypes
		{
			get
			{
				return this.DeployableWeapons.Select<SynchedMissionObject, Type>(new Func<SynchedMissionObject, Type>(MissionSiegeWeaponsController.GetWeaponType));
			}
		}

		// Token: 0x06002F0F RID: 12047 RVA: 0x000B76FC File Offset: 0x000B58FC
		public void Hide()
		{
			this.ToggleDeploymentPointVisibility(false);
			foreach (SynchedMissionObject synchedMissionObject in this.GetWeaponsUnder())
			{
				if (synchedMissionObject != null)
				{
					synchedMissionObject.SetVisibleSynched(false, false);
					synchedMissionObject.SetPhysicsStateSynched(false, true);
				}
			}
		}

		// Token: 0x06002F10 RID: 12048 RVA: 0x000B7764 File Offset: 0x000B5964
		public void Show()
		{
			this.ToggleDeploymentPointVisibility(!this.IsDeployed);
			if (this.IsDeployed)
			{
				this.ToggleDeployedWeaponVisibility(true);
			}
		}

		// Token: 0x06002F11 RID: 12049 RVA: 0x000B7784 File Offset: 0x000B5984
		private void ToggleDeploymentPointVisibility(bool visible)
		{
			this.SetVisibleSynched(visible, false);
			this.SetPhysicsStateSynched(visible, true);
		}

		// Token: 0x06002F12 RID: 12050 RVA: 0x000B7796 File Offset: 0x000B5996
		private void ToggleDeployedWeaponVisibility(bool visible)
		{
			this.ToggleWeaponVisibility(visible, this.DeployedWeapon);
		}

		// Token: 0x06002F13 RID: 12051 RVA: 0x000B77A8 File Offset: 0x000B59A8
		public void ToggleWeaponVisibility(bool visible, SynchedMissionObject weapon)
		{
			WeakGameEntity weakGameEntity = ((weapon != null) ? weapon.GameEntity.Parent : WeakGameEntity.Invalid);
			SynchedMissionObject synchedMissionObject = (weakGameEntity.IsValid ? weakGameEntity.GetFirstScriptOfType<SynchedMissionObject>() : null);
			if (synchedMissionObject != null)
			{
				synchedMissionObject.SetVisibleSynched(visible, false);
				synchedMissionObject.SetPhysicsStateSynched(visible, true);
			}
			else
			{
				if (weapon != null)
				{
					weapon.SetVisibleSynched(visible, false);
				}
				if (weapon != null)
				{
					weapon.SetPhysicsStateSynched(visible, true);
				}
			}
			if (weapon is SiegeWeapon && weapon.GameEntity.Parent.IsValid)
			{
				foreach (WeakGameEntity weakGameEntity2 in weapon.GameEntity.Parent.GetChildren())
				{
					SiegeMachineStonePile firstScriptOfType = weakGameEntity2.GetFirstScriptOfType<SiegeMachineStonePile>();
					if (firstScriptOfType != null)
					{
						firstScriptOfType.SetPhysicsStateSynched(visible, true);
						break;
					}
				}
			}
		}

		// Token: 0x06002F14 RID: 12052 RVA: 0x000B7890 File Offset: 0x000B5A90
		public void HideAllWeapons()
		{
			foreach (SynchedMissionObject synchedMissionObject in this.DeployableWeapons)
			{
				this.ToggleWeaponVisibility(false, synchedMissionObject);
			}
		}

		// Token: 0x040012FE RID: 4862
		public BattleSideEnum Side = BattleSideEnum.Attacker;

		// Token: 0x040012FF RID: 4863
		public float Radius = 3f;

		// Token: 0x04001300 RID: 4864
		public string SiegeWeaponTag = "dpWeapon";

		// Token: 0x04001301 RID: 4865
		private readonly List<GameEntity> _highlightedEntites = new List<GameEntity>();

		// Token: 0x04001302 RID: 4866
		private DeploymentPoint.DeploymentPointType _deploymentPointType;

		// Token: 0x04001303 RID: 4867
		private List<SiegeLadder> _associatedSiegeLadders;

		// Token: 0x04001304 RID: 4868
		private bool _isBreachSideDeploymentPoint;

		// Token: 0x04001305 RID: 4869
		private MBList<SynchedMissionObject> _weapons;

		// Token: 0x02000613 RID: 1555
		public enum DeploymentPointType
		{
			// Token: 0x0400205F RID: 8287
			BatteringRam,
			// Token: 0x04002060 RID: 8288
			TowerLadder,
			// Token: 0x04002061 RID: 8289
			Breach,
			// Token: 0x04002062 RID: 8290
			Ranged
		}

		// Token: 0x02000614 RID: 1556
		public enum DeploymentPointState
		{
			// Token: 0x04002064 RID: 8292
			NotDeployed,
			// Token: 0x04002065 RID: 8293
			BatteringRam,
			// Token: 0x04002066 RID: 8294
			SiegeLadder,
			// Token: 0x04002067 RID: 8295
			SiegeTower,
			// Token: 0x04002068 RID: 8296
			Breach,
			// Token: 0x04002069 RID: 8297
			Ranged
		}
	}
}
