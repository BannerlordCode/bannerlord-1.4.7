using System;
using System.Collections.Generic;
using System.Linq;
using NetworkMessages.FromClient;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200034E RID: 846
	public abstract class RangedSiegeWeapon : SiegeWeapon
	{
		// Token: 0x170008DF RID: 2271
		// (get) Token: 0x06002FA9 RID: 12201 RVA: 0x000BBC15 File Offset: 0x000B9E15
		public virtual string MultipleFireProjectileId
		{
			get
			{
				return "grapeshot_fire_stack";
			}
		}

		// Token: 0x170008E0 RID: 2272
		// (get) Token: 0x06002FAA RID: 12202 RVA: 0x000BBC1C File Offset: 0x000B9E1C
		public virtual string MultipleFireProjectileFlyingId
		{
			get
			{
				return "grapeshot_fire_projectile";
			}
		}

		// Token: 0x170008E1 RID: 2273
		// (get) Token: 0x06002FAB RID: 12203 RVA: 0x000BBC23 File Offset: 0x000B9E23
		public virtual string MultipleProjectileId
		{
			get
			{
				return "grapeshot_stack";
			}
		}

		// Token: 0x170008E2 RID: 2274
		// (get) Token: 0x06002FAC RID: 12204 RVA: 0x000BBC2A File Offset: 0x000B9E2A
		public virtual string MultipleProjectileFlyingId
		{
			get
			{
				return "grapeshot_projectile";
			}
		}

		// Token: 0x170008E3 RID: 2275
		// (get) Token: 0x06002FAD RID: 12205 RVA: 0x000BBC31 File Offset: 0x000B9E31
		public virtual string SingleFireProjectileId
		{
			get
			{
				return "pot";
			}
		}

		// Token: 0x170008E4 RID: 2276
		// (get) Token: 0x06002FAE RID: 12206 RVA: 0x000BBC38 File Offset: 0x000B9E38
		public virtual string SingleFireProjectileFlyingId
		{
			get
			{
				return "pot_projectile";
			}
		}

		// Token: 0x170008E5 RID: 2277
		// (get) Token: 0x06002FAF RID: 12207 RVA: 0x000BBC3F File Offset: 0x000B9E3F
		public virtual string SingleProjectileId
		{
			get
			{
				return "boulder";
			}
		}

		// Token: 0x170008E6 RID: 2278
		// (get) Token: 0x06002FB0 RID: 12208 RVA: 0x000BBC46 File Offset: 0x000B9E46
		public virtual string SingleProjectileFlyingId
		{
			get
			{
				return "boulder_projectile";
			}
		}

		// Token: 0x140000A0 RID: 160
		// (add) Token: 0x06002FB1 RID: 12209 RVA: 0x000BBC50 File Offset: 0x000B9E50
		// (remove) Token: 0x06002FB2 RID: 12210 RVA: 0x000BBC88 File Offset: 0x000B9E88
		public event Action<RangedSiegeWeapon, Agent> OnAgentLoadsMachine;

		// Token: 0x170008E7 RID: 2279
		// (get) Token: 0x06002FB3 RID: 12211 RVA: 0x000BBCBD File Offset: 0x000B9EBD
		// (set) Token: 0x06002FB4 RID: 12212 RVA: 0x000BBCC5 File Offset: 0x000B9EC5
		public RangedSiegeWeapon.WeaponState State
		{
			get
			{
				return this._state;
			}
			set
			{
				if (this._state != value)
				{
					if (GameNetwork.IsServerOrRecorder)
					{
						GameNetwork.BeginBroadcastModuleEvent();
						GameNetwork.WriteMessage(new SetRangedSiegeWeaponState(base.Id, value));
						GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
					}
					this._state = value;
					this.OnRangedSiegeWeaponStateChange();
				}
			}
		}

		// Token: 0x170008E8 RID: 2280
		// (get) Token: 0x06002FB5 RID: 12213 RVA: 0x000BBD02 File Offset: 0x000B9F02
		protected virtual float MaximumBallisticError
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x170008E9 RID: 2281
		// (get) Token: 0x06002FB6 RID: 12214
		protected abstract float ShootingSpeed { get; }

		// Token: 0x170008EA RID: 2282
		// (get) Token: 0x06002FB7 RID: 12215 RVA: 0x000BBD09 File Offset: 0x000B9F09
		public virtual Vec3 CanShootAtPointCheckingOffset
		{
			get
			{
				return Vec3.Zero;
			}
		}

		// Token: 0x170008EB RID: 2283
		// (get) Token: 0x06002FB8 RID: 12216 RVA: 0x000BBD10 File Offset: 0x000B9F10
		// (set) Token: 0x06002FB9 RID: 12217 RVA: 0x000BBD18 File Offset: 0x000B9F18
		public GameEntity CameraHolder { get; private set; }

		// Token: 0x170008EC RID: 2284
		// (get) Token: 0x06002FBA RID: 12218 RVA: 0x000BBD21 File Offset: 0x000B9F21
		// (set) Token: 0x06002FBB RID: 12219 RVA: 0x000BBD29 File Offset: 0x000B9F29
		private protected SynchedMissionObject Projectile { protected get; private set; }

		// Token: 0x170008ED RID: 2285
		// (get) Token: 0x06002FBC RID: 12220 RVA: 0x000BBD34 File Offset: 0x000B9F34
		protected Vec3 MissileStartingGlobalPositionForSimulation
		{
			get
			{
				if (this.MissileStartingPositionEntityForSimulation != null)
				{
					return this.MissileStartingPositionEntityForSimulation.GlobalPosition;
				}
				SynchedMissionObject projectile = this.Projectile;
				if (projectile == null)
				{
					return Vec3.Zero;
				}
				return projectile.GameEntity.GlobalPosition;
			}
		}

		// Token: 0x170008EE RID: 2286
		// (set) Token: 0x06002FBD RID: 12221 RVA: 0x000BBD78 File Offset: 0x000B9F78
		protected string SkeletonName
		{
			set
			{
				this.SkeletonNames = new string[] { value };
			}
		}

		// Token: 0x170008EF RID: 2287
		// (set) Token: 0x06002FBE RID: 12222 RVA: 0x000BBD8A File Offset: 0x000B9F8A
		protected string FireAnimation
		{
			set
			{
				this.FireAnimations = new string[] { value };
			}
		}

		// Token: 0x170008F0 RID: 2288
		// (set) Token: 0x06002FBF RID: 12223 RVA: 0x000BBD9C File Offset: 0x000B9F9C
		protected string SetUpAnimation
		{
			set
			{
				this.SetUpAnimations = new string[] { value };
			}
		}

		// Token: 0x170008F1 RID: 2289
		// (set) Token: 0x06002FC0 RID: 12224 RVA: 0x000BBDAE File Offset: 0x000B9FAE
		protected int FireAnimationIndex
		{
			set
			{
				this.FireAnimationIndices = new int[] { value };
			}
		}

		// Token: 0x170008F2 RID: 2290
		// (set) Token: 0x06002FC1 RID: 12225 RVA: 0x000BBDC0 File Offset: 0x000B9FC0
		protected int SetUpAnimationIndex
		{
			set
			{
				this.SetUpAnimationIndices = new int[] { value };
			}
		}

		// Token: 0x170008F3 RID: 2291
		// (get) Token: 0x06002FC2 RID: 12226 RVA: 0x000BBDD2 File Offset: 0x000B9FD2
		// (set) Token: 0x06002FC3 RID: 12227 RVA: 0x000BBDDA File Offset: 0x000B9FDA
		protected ItemObject LoadedMissileItem
		{
			get
			{
				return this._loadedMissileItem;
			}
			set
			{
				this._loadedMissileItem = value;
				this.OnLoadedMissileItemChanged();
			}
		}

		// Token: 0x140000A1 RID: 161
		// (add) Token: 0x06002FC4 RID: 12228 RVA: 0x000BBDEC File Offset: 0x000B9FEC
		// (remove) Token: 0x06002FC5 RID: 12229 RVA: 0x000BBE24 File Offset: 0x000BA024
		public event RangedSiegeWeapon.OnSiegeWeaponReloadDone OnReloadDone;

		// Token: 0x170008F4 RID: 2292
		// (get) Token: 0x06002FC6 RID: 12230 RVA: 0x000BBE59 File Offset: 0x000BA059
		protected virtual bool WeaponMovesDownToReload
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008F5 RID: 2293
		// (get) Token: 0x06002FC7 RID: 12231 RVA: 0x000BBE5C File Offset: 0x000BA05C
		// (set) Token: 0x06002FC8 RID: 12232 RVA: 0x000BBE64 File Offset: 0x000BA064
		public int AmmoCount
		{
			get
			{
				return this.CurrentAmmo;
			}
			protected set
			{
				this.CurrentAmmo = value;
			}
		}

		// Token: 0x170008F6 RID: 2294
		// (get) Token: 0x06002FC9 RID: 12233 RVA: 0x000BBE6D File Offset: 0x000BA06D
		// (set) Token: 0x06002FCA RID: 12234 RVA: 0x000BBE75 File Offset: 0x000BA075
		protected virtual bool HasAmmo { get; set; } = true;

		// Token: 0x170008F7 RID: 2295
		// (get) Token: 0x06002FCB RID: 12235 RVA: 0x000BBE7E File Offset: 0x000BA07E
		public virtual float DirectionRestriction
		{
			get
			{
				return 2.0943952f;
			}
		}

		// Token: 0x170008F8 RID: 2296
		// (get) Token: 0x06002FCC RID: 12236 RVA: 0x000BBE85 File Offset: 0x000BA085
		protected virtual float HorizontalAimSensitivity
		{
			get
			{
				return 0.2f;
			}
		}

		// Token: 0x170008F9 RID: 2297
		// (get) Token: 0x06002FCD RID: 12237 RVA: 0x000BBE8C File Offset: 0x000BA08C
		protected virtual float VerticalAimSensitivity
		{
			get
			{
				return 0.2f;
			}
		}

		// Token: 0x170008FA RID: 2298
		// (get) Token: 0x06002FCE RID: 12238 RVA: 0x000BBE93 File Offset: 0x000BA093
		protected virtual float ReloadSpeedMultiplier
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x170008FB RID: 2299
		// (get) Token: 0x06002FCF RID: 12239 RVA: 0x000BBE9A File Offset: 0x000BA09A
		// (set) Token: 0x06002FD0 RID: 12240 RVA: 0x000BBEA2 File Offset: 0x000BA0A2
		public bool PlayerForceUse { get; private set; }

		// Token: 0x06002FD1 RID: 12241
		protected abstract void RegisterAnimationParameters();

		// Token: 0x06002FD2 RID: 12242
		protected abstract void GetSoundEventIndices();

		// Token: 0x06002FD3 RID: 12243 RVA: 0x000BBEAC File Offset: 0x000BA0AC
		protected virtual void ConsumeAmmo()
		{
			int ammoCount = this.AmmoCount;
			this.AmmoCount = ammoCount - 1;
			if (GameNetwork.IsServerOrRecorder)
			{
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new SetRangedSiegeWeaponAmmo(base.Id, this.AmmoCount));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
			}
			this.UpdateAmmoMesh();
			this.CheckAmmo();
		}

		// Token: 0x06002FD4 RID: 12244 RVA: 0x000BBEFF File Offset: 0x000BA0FF
		public virtual void SetAmmo(int ammoLeft)
		{
			if (this.AmmoCount != ammoLeft)
			{
				this.AmmoCount = ammoLeft;
				this.UpdateAmmoMesh();
				this.CheckAmmo();
			}
		}

		// Token: 0x06002FD5 RID: 12245 RVA: 0x000BBF1D File Offset: 0x000BA11D
		public virtual void SetStartAmmo(int ammoLeft)
		{
			if (this.AmmoCount != ammoLeft)
			{
				this.AmmoCount = ammoLeft;
				this.UpdateAmmoMesh();
				this.CheckAmmo();
			}
		}

		// Token: 0x06002FD6 RID: 12246 RVA: 0x000BBF3C File Offset: 0x000BA13C
		protected virtual void CheckAmmo()
		{
			if (this.AmmoCount <= 0 && this.StartingAmmoCount > 0)
			{
				this.HasAmmo = false;
				base.SetForcedUse(false);
				foreach (StandingPoint standingPoint in base.AmmoPickUpPoints)
				{
					standingPoint.IsDeactivated = true;
				}
			}
		}

		// Token: 0x06002FD7 RID: 12247 RVA: 0x000BBFB0 File Offset: 0x000BA1B0
		protected void ChangeProjectileEntityServer(Agent loadingAgent, string missileItemID)
		{
			List<SynchedMissionObject> list = base.GameEntity.CollectScriptComponentsWithTagIncludingChildrenRecursive<SynchedMissionObject>("projectile");
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].GameEntity.HasTag(missileItemID))
				{
					this.Projectile = list[i];
					this._projectileIndex = i;
					break;
				}
			}
			this.LoadedMissileItem = Game.Current.ObjectManager.GetObject<ItemObject>(missileItemID);
			if (GameNetwork.IsServerOrRecorder)
			{
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new RangedSiegeWeaponChangeProjectile(base.Id, this._projectileIndex));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
			}
			Action<RangedSiegeWeapon, Agent> onAgentLoadsMachine = this.OnAgentLoadsMachine;
			if (onAgentLoadsMachine == null)
			{
				return;
			}
			onAgentLoadsMachine(this, loadingAgent);
		}

		// Token: 0x06002FD8 RID: 12248 RVA: 0x000BC060 File Offset: 0x000BA260
		public void ChangeProjectileEntityClient(int index)
		{
			List<SynchedMissionObject> list = base.GameEntity.CollectScriptComponentsWithTagIncludingChildrenRecursive<SynchedMissionObject>("projectile");
			this.Projectile = list[index];
			this._projectileIndex = index;
		}

		// Token: 0x06002FD9 RID: 12249 RVA: 0x000BC094 File Offset: 0x000BA294
		protected internal override void OnInit()
		{
			base.OnInit();
			this.DetermineDefaultBattleSide();
			this.ReleaseAngleRestrictionCenter = (this.TopReleaseAngleRestriction + this.BottomReleaseAngleRestriction) * 0.5f;
			this.ReleaseAngleRestrictionAngle = this.TopReleaseAngleRestriction - this.BottomReleaseAngleRestriction;
			this.CurrentReleaseAngle = (this._lastSyncedReleaseAngle = this.ReleaseAngleRestrictionCenter);
			this.OriginalMissileItem = Game.Current.ObjectManager.GetObject<ItemObject>(this.MissileItemID);
			this._projectileRadiusCached = -1f;
			this.LoadedMissileItem = this.OriginalMissileItem;
			this.OriginalMissileWeaponStatsDataForTargeting = new MissionWeapon(this.OriginalMissileItem, null, null).GetWeaponStatsDataForUsage(0);
			if (this.RotationObject == null)
			{
				this.RotationObject = this;
			}
			this._rotationObjectInitialFrame = this.RotationObject.GameEntity.GetFrame();
			this.CurrentDirection = (this._lastSyncedDirection = 0f);
			this._syncTimer = 0f;
			List<WeakGameEntity> list = base.GameEntity.CollectChildrenEntitiesWithTag("cameraHolder");
			if (list.Count > 0)
			{
				this.CameraHolder = TaleWorlds.Engine.GameEntity.CreateFromWeakEntity(list[0]);
				this._cameraHolderInitialFrame = this.CameraHolder.GetFrame();
				if (GameNetwork.IsClientOrReplay)
				{
					this.MakeVisibilityCheck = false;
				}
			}
			List<SynchedMissionObject> list2 = base.GameEntity.CollectScriptComponentsWithTagIncludingChildrenRecursive<SynchedMissionObject>("projectile");
			foreach (SynchedMissionObject synchedMissionObject in list2)
			{
				synchedMissionObject.GameEntity.SetVisibilityExcludeParents(false);
			}
			this.Projectile = list2.FirstOrDefault<SynchedMissionObject>((SynchedMissionObject x) => x.GameEntity.HasTag(this.MissileItemID));
			this._projectileIndex = list2.IndexOf(this.Projectile);
			this.Projectile.GameEntity.SetVisibilityExcludeParents(true);
			WeakGameEntity weakGameEntity = base.GameEntity.GetChildren().FirstOrDefault<WeakGameEntity>((WeakGameEntity x) => x.Name == "clean");
			if (weakGameEntity.IsValid)
			{
				weakGameEntity = weakGameEntity.GetChildren().FirstOrDefault<WeakGameEntity>((WeakGameEntity x) => x.Name == "projectile_leaving_position");
			}
			this.MissileStartingPositionEntityForSimulation = TaleWorlds.Engine.GameEntity.CreateFromWeakEntity(weakGameEntity);
			this.TargetDirection = this.CurrentDirection;
			this.TargetReleaseAngle = this.CurrentReleaseAngle;
			this.CanPickUpAmmoStandingPoints = new List<StandingPoint>();
			this.ReloadStandingPoints = new List<StandingPoint>();
			if (base.StandingPoints != null)
			{
				foreach (StandingPoint standingPoint in base.StandingPoints)
				{
					standingPoint.AddComponent(new ResetAnimationOnStopUsageComponent(ActionIndexCache.act_none, false));
					if (standingPoint.GameEntity.HasTag("reload"))
					{
						this.ReloadStandingPoints.Add(standingPoint);
					}
					if (standingPoint.GameEntity.HasTag("can_pick_up_ammo"))
					{
						this.CanPickUpAmmoStandingPoints.Add(standingPoint);
					}
				}
			}
			List<StandingPointWithWeaponRequirement> list3 = base.StandingPoints.OfType<StandingPointWithWeaponRequirement>().ToList<StandingPointWithWeaponRequirement>();
			List<StandingPointWithWeaponRequirement> list4 = new List<StandingPointWithWeaponRequirement>();
			foreach (StandingPointWithWeaponRequirement standingPointWithWeaponRequirement in list3)
			{
				if (standingPointWithWeaponRequirement.GameEntity.HasTag(this.AmmoPickUpTag))
				{
					standingPointWithWeaponRequirement.InitGivenWeapon(this.OriginalMissileItem);
					standingPointWithWeaponRequirement.SetupOnUsingStoppedBehavior(false, new Action<Agent, bool>(this.OnAmmoPickupUsingCancelled));
				}
				else
				{
					list4.Add(standingPointWithWeaponRequirement);
					standingPointWithWeaponRequirement.SetupOnUsingStoppedBehavior(false, new Action<Agent, bool>(this.OnLoadingAmmoPointUsingCancelled));
					standingPointWithWeaponRequirement.InitRequiredWeaponClasses(new WeaponClass[] { this.OriginalMissileItem.PrimaryWeapon.WeaponClass });
				}
			}
			if (base.AmmoPickUpPoints.Count > 1)
			{
				this._ammoPickupCenter = default(Vec3);
				foreach (StandingPoint standingPoint2 in base.AmmoPickUpPoints)
				{
					((StandingPointWithWeaponRequirement)standingPoint2).SetHasAlternative(true);
					this._ammoPickupCenter += standingPoint2.GameEntity.GlobalPosition;
				}
				this._ammoPickupCenter /= (float)base.AmmoPickUpPoints.Count;
			}
			else
			{
				this._ammoPickupCenter = base.GameEntity.GlobalPosition;
			}
			list4.Sort(delegate(StandingPointWithWeaponRequirement element1, StandingPointWithWeaponRequirement element2)
			{
				if (element1.GameEntity.GlobalPosition.DistanceSquared(this._ammoPickupCenter) > element2.GameEntity.GlobalPosition.DistanceSquared(this._ammoPickupCenter))
				{
					return 1;
				}
				if (element1.GameEntity.GlobalPosition.DistanceSquared(this._ammoPickupCenter) < element2.GameEntity.GlobalPosition.DistanceSquared(this._ammoPickupCenter))
				{
					return -1;
				}
				return 0;
			});
			this.LoadAmmoStandingPoint = list4.FirstOrDefault<StandingPointWithWeaponRequirement>();
			this.SortCanPickUpAmmoStandingPoints();
			Vec3 vec = base.PilotStandingPoint.GameEntity.GlobalPosition - base.GameEntity.GlobalPosition;
			foreach (StandingPoint standingPoint3 in this.CanPickUpAmmoStandingPoints)
			{
				if (standingPoint3 != base.PilotStandingPoint)
				{
					float length = (standingPoint3.GameEntity.GlobalPosition - base.GameEntity.GlobalPosition + vec).Length;
					this.PilotReservePriorityValues.Add(standingPoint3, length);
				}
			}
			this.AmmoCount = MathF.Max(0, this.StartingAmmoCount - 1);
			this.UpdateAmmoMesh();
			this.RegisterAnimationParameters();
			this.GetSoundEventIndices();
			this.InitAnimations();
			base.SetScriptComponentToTick(this.GetTickRequirement());
		}

		// Token: 0x06002FDA RID: 12250 RVA: 0x000BC650 File Offset: 0x000BA850
		protected virtual void DetermineDefaultBattleSide()
		{
			DestructableComponent firstScriptOfType = base.GameEntity.GetFirstScriptOfType<DestructableComponent>();
			this.DefaultSide = firstScriptOfType.BattleSide;
		}

		// Token: 0x06002FDB RID: 12251 RVA: 0x000BC678 File Offset: 0x000BA878
		private void SortCanPickUpAmmoStandingPoints()
		{
			if (MBMath.GetSmallestDifferenceBetweenTwoAngles(this._lastCanPickUpAmmoStandingPointsSortedAngle, this.CurrentDirection) > 0.18849556f)
			{
				this._lastCanPickUpAmmoStandingPointsSortedAngle = this.CurrentDirection;
				int signOfAmmoPile = Math.Sign(Vec3.DotProduct(base.GameEntity.GetGlobalFrame().rotation.s, this._ammoPickupCenter - base.GameEntity.GlobalPosition));
				this.CanPickUpAmmoStandingPoints.Sort(delegate(StandingPoint element1, StandingPoint element2)
				{
					Vec3 vec = this._ammoPickupCenter - element1.GameEntity.GlobalPosition;
					Vec3 vec2 = this._ammoPickupCenter - element2.GameEntity.GlobalPosition;
					float num = vec.LengthSquared;
					float num2 = vec2.LengthSquared;
					float num3 = Vec3.DotProduct(this.GameEntity.GetGlobalFrame().rotation.s, element1.GameEntity.GlobalPosition - this.GameEntity.GlobalPosition);
					float num4 = Vec3.DotProduct(this.GameEntity.GetGlobalFrame().rotation.s, element2.GameEntity.GlobalPosition - this.GameEntity.GlobalPosition);
					if (!element1.GameEntity.HasTag("no_ammo_pick_up_penalty") && signOfAmmoPile != Math.Sign(num3))
					{
						num += num3 * num3 * 64f;
					}
					if (!element2.GameEntity.HasTag("no_ammo_pick_up_penalty") && signOfAmmoPile != Math.Sign(num4))
					{
						num2 += num4 * num4 * 64f;
					}
					if (element1.GameEntity.HasTag(this.PilotStandingPointTag))
					{
						num += 25f;
					}
					else if (element2.GameEntity.HasTag(this.PilotStandingPointTag))
					{
						num2 += 25f;
					}
					if (num > num2)
					{
						return 1;
					}
					if (num < num2)
					{
						return -1;
					}
					return 0;
				});
			}
		}

		// Token: 0x06002FDC RID: 12252 RVA: 0x000BC710 File Offset: 0x000BA910
		protected internal override void OnEditorInit()
		{
			List<SynchedMissionObject> list = base.GameEntity.CollectScriptComponentsWithTagIncludingChildrenRecursive<SynchedMissionObject>("projectile");
			if (list.Count > 0)
			{
				this.Projectile = list[0];
			}
		}

		// Token: 0x06002FDD RID: 12253 RVA: 0x000BC744 File Offset: 0x000BA944
		private void InitAnimations()
		{
			for (int i = 0; i < this.Skeletons.Length; i++)
			{
				this.Skeletons[i].SetAnimationAtChannel(this.SetUpAnimations[i], 0, 1f, 0f, 0f);
				this.Skeletons[i].SetAnimationParameterAtChannel(0, 1f);
				this.Skeletons[i].TickAnimations(0.0001f, MatrixFrame.Identity, true);
			}
		}

		// Token: 0x06002FDE RID: 12254 RVA: 0x000BC7B4 File Offset: 0x000BA9B4
		protected internal override void OnMissionReset()
		{
			base.OnMissionReset();
			this.Projectile.GameEntity.SetVisibilityExcludeParents(true);
			foreach (StandingPoint standingPoint in base.StandingPoints)
			{
				Agent userAgent = standingPoint.UserAgent;
				if (userAgent != null)
				{
					userAgent.StopUsingGameObject(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
				}
				standingPoint.IsDeactivated = false;
			}
			this._state = RangedSiegeWeapon.WeaponState.Idle;
			this.CurrentDirection = (this._lastSyncedDirection = 0f);
			this._syncTimer = 0f;
			this.CurrentReleaseAngle = (this._lastSyncedReleaseAngle = this.ReleaseAngleRestrictionCenter);
			this.TargetDirection = this.CurrentDirection;
			this.TargetReleaseAngle = this.CurrentReleaseAngle;
			this.ApplyCurrentDirectionToEntity();
			this.AmmoCount = MathF.Max(0, this.StartingAmmoCount - 1);
			this.UpdateAmmoMesh();
			if (this.MoveSound != null)
			{
				this.MoveSound.Stop();
				this.MoveSound = null;
			}
			this._hasFrameChangedInPreviousFrame = false;
			Skeleton[] skeletons = this.Skeletons;
			for (int i = 0; i < skeletons.Length; i++)
			{
				skeletons[i].Freeze(false);
			}
			foreach (StandingPoint standingPoint2 in base.AmmoPickUpPoints)
			{
				standingPoint2.IsDeactivated = false;
			}
			this.InitAnimations();
			this.UpdateProjectilePosition();
			if (!GameNetwork.IsClientOrReplay)
			{
				this.SetActivationLoadAmmoPoint(false);
			}
		}

		// Token: 0x06002FDF RID: 12255 RVA: 0x000BC944 File Offset: 0x000BAB44
		public override void WriteToNetwork()
		{
			base.WriteToNetwork();
			GameNetworkMessage.WriteIntToPacket((int)this.State, CompressionMission.RangedSiegeWeaponStateCompressionInfo);
			GameNetworkMessage.WriteFloatToPacket(this.TargetDirection, CompressionBasic.RadianCompressionInfo);
			GameNetworkMessage.WriteFloatToPacket(this.TargetReleaseAngle, CompressionBasic.RadianCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.AmmoCount, CompressionMission.RangedSiegeWeaponAmmoCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this._projectileIndex, CompressionMission.RangedSiegeWeaponAmmoIndexCompressionInfo);
		}

		// Token: 0x06002FE0 RID: 12256 RVA: 0x000BC9A7 File Offset: 0x000BABA7
		protected virtual void UpdateProjectilePosition()
		{
		}

		// Token: 0x06002FE1 RID: 12257 RVA: 0x000BC9AC File Offset: 0x000BABAC
		public override bool IsInRangeToCheckAlternativePoints(Agent agent)
		{
			float num = ((base.AmmoPickUpPoints.Count > 0) ? (agent.GetInteractionDistanceToUsable(base.AmmoPickUpPoints[0]) + 2f) : 2f);
			return this._ammoPickupCenter.DistanceSquared(agent.Position) < num * num;
		}

		// Token: 0x06002FE2 RID: 12258 RVA: 0x000BCA00 File Offset: 0x000BAC00
		public override StandingPoint GetBestPointAlternativeTo(StandingPoint standingPoint, Agent agent)
		{
			if (base.AmmoPickUpPoints.Contains(standingPoint))
			{
				IEnumerable<StandingPoint> enumerable = base.AmmoPickUpPoints.Where<StandingPoint>((StandingPoint sp) => !sp.IsDeactivated && (sp.IsInstantUse || (!sp.HasUser && !sp.HasAIMovingTo)) && !sp.IsDisabledForAgent(agent));
				float num = standingPoint.GameEntity.GlobalPosition.DistanceSquared(agent.Position);
				StandingPoint standingPoint2 = standingPoint;
				foreach (StandingPoint standingPoint3 in enumerable)
				{
					float num2 = standingPoint3.GameEntity.GlobalPosition.DistanceSquared(agent.Position);
					if (num2 < num)
					{
						num = num2;
						standingPoint2 = standingPoint3;
					}
				}
				return standingPoint2;
			}
			return standingPoint;
		}

		// Token: 0x06002FE3 RID: 12259 RVA: 0x000BCAD4 File Offset: 0x000BACD4
		protected virtual void OnRangedSiegeWeaponStateChange()
		{
			switch (this.State)
			{
			case RangedSiegeWeapon.WeaponState.Idle:
			case RangedSiegeWeapon.WeaponState.WaitingBeforeIdle:
				this._cameraState = ((this._cameraState == RangedSiegeWeapon.CameraState.FreeMove) ? RangedSiegeWeapon.CameraState.ApproachToCamera : RangedSiegeWeapon.CameraState.StickToWeapon);
				break;
			case RangedSiegeWeapon.WeaponState.WaitingBeforeProjectileLeaving:
				this.AttackClickWillReload = this.WeaponNeedsClickToReload;
				if (!GameNetwork.IsDedicatedServer)
				{
					SoundManager.StartOneShotEventWithIndex(this.FireSoundIndex, in base.GameEntity.GetGlobalFrame().origin);
				}
				break;
			case RangedSiegeWeapon.WeaponState.Shooting:
				if (this.CameraHolder != null)
				{
					this._cameraState = RangedSiegeWeapon.CameraState.DoNotMove;
					this.DontMoveTimer = 0.35f;
				}
				break;
			case RangedSiegeWeapon.WeaponState.WaitingAfterShooting:
				this.AttackClickWillReload = this.WeaponNeedsClickToReload;
				this.CheckAmmo();
				break;
			case RangedSiegeWeapon.WeaponState.WaitingBeforeReloading:
				this.AttackClickWillReload = false;
				if (this.CameraHolder != null && this.WeaponMovesDownToReload)
				{
					this._cameraState = RangedSiegeWeapon.CameraState.MoveDownToReload;
				}
				this.CheckAmmo();
				break;
			case RangedSiegeWeapon.WeaponState.LoadingAmmo:
				if (this.ReloadSound != null && this.ReloadSound.IsValid)
				{
					this.ReloadSound.Stop();
				}
				this.ReloadSound = null;
				break;
			case RangedSiegeWeapon.WeaponState.Reloading:
				if (this.ReloadSound != null && this.ReloadSound.IsValid)
				{
					if (this.ReloadSound.IsPaused())
					{
						this.ReloadSound.Resume();
					}
					else
					{
						this.ReloadSound.PlayInPosition(base.GameEntity.GetGlobalFrame().origin);
					}
				}
				else
				{
					this.ReloadSound = SoundEvent.CreateEvent(this.ReloadSoundIndex, base.Scene);
					this.ReloadSound.PlayInPosition(base.GameEntity.GetGlobalFrame().origin);
				}
				break;
			case RangedSiegeWeapon.WeaponState.ReloadingPaused:
				if (this.ReloadSound != null && this.ReloadSound.IsValid)
				{
					this.ReloadSound.Pause();
				}
				break;
			default:
				Debug.FailedAssert("Invalid WeaponState.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Objects\\Siege\\RangedSiegeWeapon.cs", "OnRangedSiegeWeaponStateChange", 894);
				break;
			}
			if (!GameNetwork.IsClientOrReplay)
			{
				switch (this.State)
				{
				case RangedSiegeWeapon.WeaponState.Idle:
				case RangedSiegeWeapon.WeaponState.WaitingAfterShooting:
				case RangedSiegeWeapon.WeaponState.WaitingBeforeReloading:
					break;
				case RangedSiegeWeapon.WeaponState.WaitingBeforeProjectileLeaving:
				{
					for (int i = 0; i < this.SkeletonOwnerObjects.Length; i++)
					{
						this.SkeletonOwnerObjects[i].SetAnimationAtChannelSynched(this.FireAnimations[i], 0, 1f);
					}
					return;
				}
				case RangedSiegeWeapon.WeaponState.Shooting:
					this.ShootProjectile();
					return;
				case RangedSiegeWeapon.WeaponState.LoadingAmmo:
					this.SetActivationLoadAmmoPoint(true);
					this.ReloaderAgent = null;
					return;
				case RangedSiegeWeapon.WeaponState.WaitingBeforeIdle:
					this.SendReloaderAgentToOriginalPoint();
					this.SetActivationLoadAmmoPoint(false);
					return;
				case RangedSiegeWeapon.WeaponState.Reloading:
				{
					for (int j = 0; j < this.SkeletonOwnerObjects.Length; j++)
					{
						if (this.SkeletonOwnerObjects[j].GameEntity.IsSkeletonAnimationPaused())
						{
							this.SkeletonOwnerObjects[j].ResumeSkeletonAnimationSynched();
						}
						else
						{
							this.SkeletonOwnerObjects[j].SetAnimationAtChannelSynched(this.SetUpAnimations[j], 0, 1f);
						}
					}
					this._currentReloaderCount = 1;
					return;
				}
				case RangedSiegeWeapon.WeaponState.ReloadingPaused:
				{
					SynchedMissionObject[] skeletonOwnerObjects = this.SkeletonOwnerObjects;
					for (int k = 0; k < skeletonOwnerObjects.Length; k++)
					{
						skeletonOwnerObjects[k].PauseSkeletonAnimationSynched();
					}
					return;
				}
				default:
					Debug.FailedAssert("Invalid WeaponState.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Objects\\Siege\\RangedSiegeWeapon.cs", "OnRangedSiegeWeaponStateChange", 970);
					break;
				}
			}
		}

		// Token: 0x06002FE4 RID: 12260 RVA: 0x000BCE03 File Offset: 0x000BB003
		protected virtual void SetActivationLoadAmmoPoint(bool activate)
		{
		}

		// Token: 0x06002FE5 RID: 12261 RVA: 0x000BCE05 File Offset: 0x000BB005
		protected override float GetDetachmentWeightAux(BattleSideEnum side)
		{
			if (this.HasAmmo)
			{
				return base.GetDetachmentWeightAux(side);
			}
			return float.MinValue;
		}

		// Token: 0x06002FE6 RID: 12262 RVA: 0x000BCE1C File Offset: 0x000BB01C
		protected float GetDetachmentWeightAuxForExternalAmmoWeapons(BattleSideEnum side)
		{
			if (this.IsDisabledForBattleSideAI(side))
			{
				return float.MinValue;
			}
			this.UsableStandingPoints.Clear();
			bool flag = false;
			bool flag2 = false;
			bool flag3 = !base.PilotStandingPoint.HasUser && !base.PilotStandingPoint.HasAIMovingTo && (this.ReloaderAgent == null || this.ReloaderAgentOriginalPoint != base.PilotStandingPoint);
			int num = -1;
			StandingPoint standingPoint = null;
			bool flag4 = false;
			for (int i = 0; i < base.StandingPoints.Count; i++)
			{
				StandingPoint standingPoint2 = base.StandingPoints[i];
				if (standingPoint2.GameEntity.HasTag("can_pick_up_ammo"))
				{
					if (this.ReloaderAgent == null || standingPoint2 != this.ReloaderAgentOriginalPoint)
					{
						if (standingPoint2.IsUsableBySide(side))
						{
							if (!standingPoint2.HasAIMovingTo)
							{
								if (!flag2)
								{
									this.UsableStandingPoints.Clear();
									num = -1;
								}
								flag2 = true;
							}
							else if (flag2 || standingPoint2.MovingAgent.Formation.Team.Side != side)
							{
								goto IL_016A;
							}
							flag = true;
							this.UsableStandingPoints.Add(new ValueTuple<int, StandingPoint>(i, standingPoint2));
							if (flag3 && base.PilotStandingPoint == standingPoint2)
							{
								num = this.UsableStandingPoints.Count - 1;
							}
						}
						else if (flag3 && standingPoint2.HasAIUser && (standingPoint == null || this.PilotReservePriorityValues[standingPoint2] > this.PilotReservePriorityValues[standingPoint] || flag4))
						{
							standingPoint = standingPoint2;
							flag4 = false;
						}
					}
					else if (flag3 && standingPoint == null)
					{
						standingPoint = standingPoint2;
						flag4 = true;
					}
				}
				IL_016A:;
			}
			if (standingPoint != null)
			{
				if (flag4)
				{
					this.ReloaderAgentOriginalPoint = base.PilotStandingPoint;
				}
				else
				{
					Agent userAgent = standingPoint.UserAgent;
					userAgent.StopUsingGameObjectMT(true, Agent.StopUsingGameObjectFlags.DoNotWieldWeaponAfterStoppingUsingGameObject);
					userAgent.AIMoveToGameObjectEnable(base.PilotStandingPoint, this, base.Ai.GetScriptedFrameFlags(userAgent));
				}
				if (num != -1)
				{
					this.UsableStandingPoints.RemoveAt(num);
				}
			}
			this.AreUsableStandingPointsVacant = flag2;
			if (!flag)
			{
				return float.MinValue;
			}
			if (flag2)
			{
				return 1f;
			}
			if (base.IsDetachmentRecentlyEvaluated)
			{
				return 0.01f;
			}
			return 0.1f;
		}

		// Token: 0x06002FE7 RID: 12263 RVA: 0x000BD02C File Offset: 0x000BB22C
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			if (base.GameEntity.IsVisibleIncludeParents())
			{
				return ScriptComponentBehavior.TickRequirement.Tick | base.GetTickRequirement();
			}
			return base.GetTickRequirement();
		}

		// Token: 0x06002FE8 RID: 12264 RVA: 0x000BD058 File Offset: 0x000BB258
		protected internal override void OnTick(float dt)
		{
			base.OnTick(dt);
			if (!base.GameEntity.IsVisibleIncludeParents())
			{
				return;
			}
			if (!GameNetwork.IsClientOrReplay)
			{
				this.UpdateState(dt);
				if (base.PilotAgent != null && !base.PilotAgent.IsInBeingStruckAction)
				{
					if (base.PilotAgent.MovementFlags.HasAnyFlag(Agent.MovementControlFlag.AttackMask))
					{
						if (this.State == RangedSiegeWeapon.WeaponState.Idle)
						{
							this._aiRequestsShoot = false;
							this.Shoot();
						}
						else if (this.State == RangedSiegeWeapon.WeaponState.WaitingAfterShooting && this.AttackClickWillReload)
						{
							this._aiRequestsManualReload = false;
							this.ManualReload();
						}
					}
					if (this._aiRequestsManualReload)
					{
						this.ManualReload();
					}
					if (this._aiRequestsShoot)
					{
						this.Shoot();
					}
				}
				this._aiRequestsShoot = false;
				this._aiRequestsManualReload = false;
			}
			this.HandleUserAiming(dt);
		}

		// Token: 0x06002FE9 RID: 12265 RVA: 0x000BD124 File Offset: 0x000BB324
		protected static bool ApproachToAngle(ref float angle, float angleToApproach, bool isMouse, float speed_limit, float dt, float sensitivity)
		{
			speed_limit = MathF.Abs(speed_limit);
			if (angle != angleToApproach)
			{
				float num = sensitivity * dt;
				float num2 = MathF.Abs(angle - angleToApproach);
				if (isMouse)
				{
					num *= MathF.Max(num2 * 8f, 0.15f);
				}
				if (speed_limit > 0f)
				{
					num = MathF.Min(num, speed_limit * dt);
				}
				if (num2 <= num)
				{
					angle = angleToApproach;
				}
				else
				{
					angle += num * (float)MathF.Sign(angleToApproach - angle);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06002FEA RID: 12266 RVA: 0x000BD198 File Offset: 0x000BB398
		protected virtual void HandleUserAiming(float dt)
		{
			bool flag = false;
			float horizontalAimSensitivity = this.HorizontalAimSensitivity;
			float verticalAimSensitivity = this.VerticalAimSensitivity;
			bool flag2 = false;
			if (this._cameraState != RangedSiegeWeapon.CameraState.DoNotMove)
			{
				if (this._inputGiven)
				{
					flag2 = true;
					if (this.CanRotate())
					{
						if (this._inputX != 0f)
						{
							this.TargetDirection += horizontalAimSensitivity * dt * this._inputX;
							this.TargetDirection = MBMath.WrapAngle(this.TargetDirection);
							this.TargetDirection = MBMath.ClampAngle(this.TargetDirection, this.CurrentDirection, 0.7f);
							this.TargetDirection = MBMath.ClampAngle(this.TargetDirection, 0f, this.DirectionRestriction);
						}
						if (this._inputY != 0f)
						{
							this.TargetReleaseAngle += verticalAimSensitivity * dt * this._inputY;
							this.TargetReleaseAngle = MBMath.ClampAngle(this.TargetReleaseAngle, this.CurrentReleaseAngle + 0.049999997f, 0.6f);
							this.TargetReleaseAngle = MBMath.ClampAngle(this.TargetReleaseAngle, this.ReleaseAngleRestrictionCenter, this.ReleaseAngleRestrictionAngle);
						}
					}
					this._inputGiven = false;
					this._inputX = 0f;
					this._inputY = 0f;
				}
				else if (this._exactInputGiven)
				{
					bool flag3 = false;
					if (this.CanRotate())
					{
						if (this.TargetDirection != this._inputTargetX)
						{
							float num = horizontalAimSensitivity * dt;
							if (MathF.Abs(this.TargetDirection - this._inputTargetX) < num)
							{
								this.TargetDirection = this._inputTargetX;
							}
							else if (this.TargetDirection < this._inputTargetX)
							{
								this.TargetDirection += num;
								flag3 = true;
							}
							else
							{
								this.TargetDirection -= num;
								flag3 = true;
							}
							this.TargetDirection = MBMath.WrapAngle(this.TargetDirection);
							this.TargetDirection = MBMath.ClampAngle(this.TargetDirection, this.CurrentDirection, 0.7f);
							this.TargetDirection = MBMath.ClampAngle(this.TargetDirection, 0f, this.DirectionRestriction);
						}
						if (this.TargetReleaseAngle != this._inputTargetY)
						{
							float num2 = verticalAimSensitivity * dt;
							if (MathF.Abs(this.TargetReleaseAngle - this._inputTargetY) < num2)
							{
								this.TargetReleaseAngle = this._inputTargetY;
							}
							else if (this.TargetReleaseAngle < this._inputTargetY)
							{
								this.TargetReleaseAngle += num2;
								flag3 = true;
							}
							else
							{
								this.TargetReleaseAngle -= num2;
								flag3 = true;
							}
							this.TargetReleaseAngle = MBMath.ClampAngle(this.TargetReleaseAngle, this.CurrentReleaseAngle + 0.049999997f, 0.6f);
							this.TargetReleaseAngle = MBMath.ClampAngle(this.TargetReleaseAngle, this.ReleaseAngleRestrictionCenter, this.ReleaseAngleRestrictionAngle);
						}
					}
					else
					{
						flag3 = true;
					}
					if (!flag3)
					{
						this._exactInputGiven = false;
					}
				}
			}
			switch (this._cameraState)
			{
			case RangedSiegeWeapon.CameraState.StickToWeapon:
				flag = RangedSiegeWeapon.ApproachToAngle(ref this.CurrentDirection, this.TargetDirection, this.UsesMouseForAiming, -1f, dt, horizontalAimSensitivity);
				flag = RangedSiegeWeapon.ApproachToAngle(ref this.CurrentReleaseAngle, this.TargetReleaseAngle, this.UsesMouseForAiming, -1f, dt, verticalAimSensitivity) || flag;
				this.CameraDirection = this.CurrentDirection;
				this.CameraReleaseAngle = this.CurrentReleaseAngle;
				break;
			case RangedSiegeWeapon.CameraState.DoNotMove:
				this.DontMoveTimer -= dt;
				if (this.DontMoveTimer < 0f)
				{
					if (!this.AttackClickWillReload && this.WeaponMovesDownToReload)
					{
						this._cameraState = RangedSiegeWeapon.CameraState.MoveDownToReload;
						this.MaxRotateSpeed = 0f;
						this.ReloadTargetReleaseAngle = MBMath.ClampAngle((MathF.Abs(this.CurrentReleaseAngle) > 0.17453292f) ? 0f : this.CurrentReleaseAngle, this.CurrentReleaseAngle - 0.049999997f, 0.6f);
						this.TargetDirection = this.CameraDirection;
						this.CameraReleaseAngle = this.TargetReleaseAngle;
					}
					else
					{
						this._cameraState = RangedSiegeWeapon.CameraState.StickToWeapon;
					}
				}
				break;
			case RangedSiegeWeapon.CameraState.MoveDownToReload:
				this.MaxRotateSpeed += dt * 1.2f;
				this.MaxRotateSpeed = MathF.Min(this.MaxRotateSpeed, 1f);
				flag = RangedSiegeWeapon.ApproachToAngle(ref this.CurrentReleaseAngle, this.ReloadTargetReleaseAngle, this.UsesMouseForAiming, 0.4f + this.MaxRotateSpeed, dt, verticalAimSensitivity);
				flag = RangedSiegeWeapon.ApproachToAngle(ref this.CameraDirection, this.TargetDirection, this.UsesMouseForAiming, -1f, dt, horizontalAimSensitivity) || flag;
				flag = RangedSiegeWeapon.ApproachToAngle(ref this.CameraReleaseAngle, this.ReloadTargetReleaseAngle, this.UsesMouseForAiming, 0.5f + this.MaxRotateSpeed, dt, verticalAimSensitivity) || flag;
				if (!flag)
				{
					this._cameraState = RangedSiegeWeapon.CameraState.RememberLastShotDirection;
				}
				break;
			case RangedSiegeWeapon.CameraState.RememberLastShotDirection:
				if (this.State == RangedSiegeWeapon.WeaponState.Idle || flag2)
				{
					this._cameraState = RangedSiegeWeapon.CameraState.FreeMove;
					RangedSiegeWeapon.OnSiegeWeaponReloadDone onReloadDone = this.OnReloadDone;
					if (onReloadDone != null)
					{
						onReloadDone();
					}
				}
				break;
			case RangedSiegeWeapon.CameraState.FreeMove:
				flag = RangedSiegeWeapon.ApproachToAngle(ref this.CameraDirection, this.TargetDirection, this.UsesMouseForAiming, -1f, dt, horizontalAimSensitivity);
				flag = RangedSiegeWeapon.ApproachToAngle(ref this.CameraReleaseAngle, this.TargetReleaseAngle, this.UsesMouseForAiming, -1f, dt, verticalAimSensitivity) || flag;
				this.MaxRotateSpeed = 0f;
				break;
			case RangedSiegeWeapon.CameraState.ApproachToCamera:
				this.MaxRotateSpeed += 0.9f * dt + this.MaxRotateSpeed * 2f * dt;
				flag = RangedSiegeWeapon.ApproachToAngle(ref this.CameraDirection, this.TargetDirection, this.UsesMouseForAiming, -1f, dt, horizontalAimSensitivity);
				flag = RangedSiegeWeapon.ApproachToAngle(ref this.CameraReleaseAngle, this.TargetReleaseAngle, this.UsesMouseForAiming, -1f, dt, verticalAimSensitivity) || flag;
				flag = RangedSiegeWeapon.ApproachToAngle(ref this.CurrentDirection, this.TargetDirection, this.UsesMouseForAiming, this.MaxRotateSpeed, dt, horizontalAimSensitivity) || flag;
				flag = RangedSiegeWeapon.ApproachToAngle(ref this.CurrentReleaseAngle, this.TargetReleaseAngle, this.UsesMouseForAiming, this.MaxRotateSpeed, dt, verticalAimSensitivity) || flag;
				if (!flag)
				{
					this._cameraState = RangedSiegeWeapon.CameraState.StickToWeapon;
				}
				break;
			}
			if (this.CameraHolder != null)
			{
				MatrixFrame matrixFrame = this._cameraHolderInitialFrame;
				matrixFrame.rotation.RotateAboutForward(this.CameraDirection - this.CurrentDirection);
				matrixFrame.rotation.RotateAboutSide(this.CameraReleaseAngle - this.CurrentReleaseAngle);
				this.CameraHolder.SetFrame(ref matrixFrame, true);
				matrixFrame = this.CameraHolder.GetGlobalFrame();
				matrixFrame.rotation.s.z = 0f;
				matrixFrame.rotation.s.Normalize();
				matrixFrame.rotation.u = Vec3.CrossProduct(matrixFrame.rotation.s, matrixFrame.rotation.f);
				matrixFrame.rotation.u.Normalize();
				matrixFrame.rotation.f = Vec3.CrossProduct(matrixFrame.rotation.u, matrixFrame.rotation.s);
				matrixFrame.rotation.f.Normalize();
				if (base.PilotAgent == null)
				{
					this._cameraMoveBackFactor = ((1f - this._cameraMoveBackFactor > 1E-05f) ? MBMath.LerpFPSIndependent(this._cameraMoveBackFactor, 1f, dt * 8f) : 1f);
				}
				else
				{
					this._cameraMoveBackFactor = ((this._cameraMoveBackFactor > 1E-05f) ? MBMath.LerpFPSIndependent(this._cameraMoveBackFactor, 0f, dt * 8f) : 0f);
				}
				if (this._cameraMoveBackFactor > 0f)
				{
					matrixFrame.origin += matrixFrame.rotation.u * this._cameraMoveBackFactor * 3f + matrixFrame.rotation.f * this._cameraMoveBackFactor * 0.3f;
				}
				this.CameraHolder.SetGlobalFrame(in matrixFrame, true);
			}
			else
			{
				this._cameraMoveBackFactor = ((base.PilotAgent == null) ? 1f : 0f);
			}
			if (flag && !this._hasFrameChangedInPreviousFrame)
			{
				this.OnRotationStarted();
			}
			else if (!flag && this._hasFrameChangedInPreviousFrame)
			{
				this.OnRotationStopped();
			}
			this._hasFrameChangedInPreviousFrame = flag;
			if ((flag && GameNetwork.IsClient && base.PilotAgent == Agent.Main) || GameNetwork.IsServerOrRecorder)
			{
				float num3 = ((GameNetwork.IsClient && base.PilotAgent == Agent.Main) ? 0.0001f : 0.02f);
				if (this._syncTimer > 0.2f && (MathF.Abs(this.CurrentDirection - this._lastSyncedDirection) > num3 || MathF.Abs(this.CurrentReleaseAngle - this._lastSyncedReleaseAngle) > num3))
				{
					this._lastSyncedDirection = this.CurrentDirection;
					this._lastSyncedReleaseAngle = this.CurrentReleaseAngle;
					MissionLobbyComponent missionBehavior = Mission.Current.GetMissionBehavior<MissionLobbyComponent>();
					if ((missionBehavior == null || missionBehavior.CurrentMultiplayerState != MissionLobbyComponent.MultiplayerGameState.Ending) && GameNetwork.IsClient && base.PilotAgent == Agent.Main)
					{
						GameNetwork.BeginModuleEventAsClient();
						GameNetwork.WriteMessage(new SetMachineRotation(base.Id, this.CurrentDirection, this.CurrentReleaseAngle));
						GameNetwork.EndModuleEventAsClient();
					}
					if (GameNetwork.IsServerOrRecorder)
					{
						GameNetwork.BeginBroadcastModuleEvent();
						GameNetwork.WriteMessage(new SetMachineTargetRotation(base.Id, this.CurrentDirection, this.CurrentReleaseAngle));
						GameNetwork.EventBroadcastFlags eventBroadcastFlags = GameNetwork.EventBroadcastFlags.ExcludeTargetPlayer | GameNetwork.EventBroadcastFlags.AddToMissionRecord;
						Agent pilotAgent = base.PilotAgent;
						NetworkCommunicator networkCommunicator;
						if (pilotAgent == null)
						{
							networkCommunicator = null;
						}
						else
						{
							MissionPeer missionPeer = pilotAgent.MissionPeer;
							networkCommunicator = ((missionPeer != null) ? missionPeer.GetNetworkPeer() : null);
						}
						GameNetwork.EndBroadcastModuleEvent(eventBroadcastFlags, networkCommunicator);
					}
				}
			}
			this._syncTimer += dt;
			if (this._syncTimer >= 1f)
			{
				this._syncTimer -= 1f;
			}
			if (flag)
			{
				this.ApplyAimChange();
			}
		}

		// Token: 0x06002FEB RID: 12267 RVA: 0x000BDB10 File Offset: 0x000BBD10
		public void GiveInput(float inputX, float inputY)
		{
			this._exactInputGiven = false;
			this._inputGiven = true;
			this._inputX = inputX;
			this._inputY = inputY;
			this._inputX = MBMath.ClampFloat(this._inputX, -1f, 1f);
			this._inputY = MBMath.ClampFloat(this._inputY, -1f, 1f);
		}

		// Token: 0x06002FEC RID: 12268 RVA: 0x000BDB6F File Offset: 0x000BBD6F
		public void GiveExactInput(float targetX, float targetY)
		{
			this._exactInputGiven = true;
			this._inputGiven = false;
			this._inputTargetX = MBMath.ClampAngle(targetX, 0f, this.DirectionRestriction);
			this._inputTargetY = MBMath.ClampAngle(targetY, this.ReleaseAngleRestrictionCenter, this.ReleaseAngleRestrictionAngle);
		}

		// Token: 0x06002FED RID: 12269 RVA: 0x000BDBAE File Offset: 0x000BBDAE
		protected virtual bool CanRotate()
		{
			return this.State == RangedSiegeWeapon.WeaponState.Idle;
		}

		// Token: 0x06002FEE RID: 12270 RVA: 0x000BDBB9 File Offset: 0x000BBDB9
		protected virtual void ApplyAimChange()
		{
			if (this.CanRotate())
			{
				this.ApplyCurrentDirectionToEntity();
				return;
			}
			this.TargetDirection = this.CurrentDirection;
			this.TargetReleaseAngle = this.CurrentReleaseAngle;
		}

		// Token: 0x06002FEF RID: 12271 RVA: 0x000BDBE4 File Offset: 0x000BBDE4
		protected virtual void ApplyCurrentDirectionToEntity()
		{
			MatrixFrame rotationObjectInitialFrame = this._rotationObjectInitialFrame;
			rotationObjectInitialFrame.rotation.RotateAboutUp(this.CurrentDirection);
			this.RotationObject.GameEntity.SetFrame(ref rotationObjectInitialFrame, true);
		}

		// Token: 0x06002FF0 RID: 12272 RVA: 0x000BDC20 File Offset: 0x000BBE20
		public virtual float GetTargetReleaseAngle(Vec3 target)
		{
			return Mission.GetMissileVerticalAimCorrection(target - this.MissileStartingGlobalPositionForSimulation, this.ShootingSpeed, ref this.OriginalMissileWeaponStatsDataForTargeting, ItemObject.GetAirFrictionConstant(this.OriginalMissileItem.PrimaryWeapon.WeaponClass, this.OriginalMissileItem.PrimaryWeapon.WeaponFlags));
		}

		// Token: 0x06002FF1 RID: 12273 RVA: 0x000BDC70 File Offset: 0x000BBE70
		private void CalculateLocalAnglesFromGlobalDirection(Vec3 globalDirection, out float localTargetDirection, out float localTargetAngle)
		{
			globalDirection.Normalize();
			MatrixFrame globalFrame = base.GameEntity.GetGlobalFrame();
			if (!globalFrame.rotation.IsUnit())
			{
				globalFrame.rotation.Orthonormalize();
			}
			globalFrame.rotation.RotateAboutAnArbitraryVector(in globalFrame.rotation.u, 3.1415927f);
			Vec3 vec = globalFrame.rotation.TransformToLocal(in globalDirection);
			localTargetDirection = vec.AsVec2.RotationInRadians;
			localTargetAngle = MathF.Atan2(vec.z, MathF.Sqrt(vec.x * vec.x + vec.y * vec.y));
		}

		// Token: 0x06002FF2 RID: 12274 RVA: 0x000BDD18 File Offset: 0x000BBF18
		private void CalculateLocalDirectionAndLocalAngleToShootTarget(Vec3 target, out float localTargetDirection, out float localTargetAngle)
		{
			float targetReleaseAngle = this.GetTargetReleaseAngle(target);
			if (targetReleaseAngle > 1.5707964f)
			{
				localTargetDirection = 3.1415927f;
				localTargetAngle = 3.1415927f;
				return;
			}
			Vec3 vec = new Vec3((target - this.MissileStartingGlobalPositionForSimulation).AsVec2, 0f, -1f).NormalizedCopy();
			vec += new Vec3(0f, 0f, MathF.Sin(targetReleaseAngle), -1f);
			vec.Normalize();
			Vec3 globalVelocity = this.GetGlobalVelocity();
			vec *= this.ShootingSpeed;
			vec -= new Vec3(globalVelocity.AsVec2, 0f, -1f);
			vec.Normalize();
			this.CalculateLocalAnglesFromGlobalDirection(vec, out localTargetDirection, out localTargetAngle);
		}

		// Token: 0x06002FF3 RID: 12275 RVA: 0x000BDDDC File Offset: 0x000BBFDC
		public virtual bool AimAtThreat(Threat threat)
		{
			Vec3 estimatedTargetGlobalPoint = this.GetEstimatedTargetGlobalPoint(threat);
			return this.AimAtTarget(estimatedTargetGlobalPoint);
		}

		// Token: 0x06002FF4 RID: 12276 RVA: 0x000BDDF8 File Offset: 0x000BBFF8
		public bool AimAtTarget(Vec3 target)
		{
			float num;
			float num2;
			this.CalculateLocalDirectionAndLocalAngleToShootTarget(target, out num, out num2);
			if (num >= 3.1415927f)
			{
				return false;
			}
			if (!this._exactInputGiven || num != this._inputTargetX || num2 != this._inputTargetY)
			{
				this.GiveExactInput(num, num2);
			}
			return this.CheckIsTargetReached(target);
		}

		// Token: 0x06002FF5 RID: 12277 RVA: 0x000BDE43 File Offset: 0x000BC043
		public virtual bool CheckIsTargetReached(Vec3 target)
		{
			return MathF.Abs(this.CurrentDirection - this._inputTargetX) < 0.001f && MathF.Abs(this.CurrentReleaseAngle - this._inputTargetY) < 0.001f;
		}

		// Token: 0x06002FF6 RID: 12278 RVA: 0x000BDE7C File Offset: 0x000BC07C
		public Vec3 GetEstimatedTargetGlobalPoint(Threat threat)
		{
			Vec3 targetingPosition = threat.TargetingPosition;
			return targetingPosition + this.GetEstimatedTargetMovementVector(targetingPosition, threat.GetGlobalVelocity());
		}

		// Token: 0x06002FF7 RID: 12279 RVA: 0x000BDEA3 File Offset: 0x000BC0A3
		public Vec3 GetEstimatedTargetGlobalPointForAgent(Agent agent)
		{
			return agent.CollisionCapsuleCenter + this.GetEstimatedTargetMovementVector(agent.CollisionCapsuleCenter, agent.GetAverageRealGlobalVelocity());
		}

		// Token: 0x06002FF8 RID: 12280 RVA: 0x000BDEC4 File Offset: 0x000BC0C4
		public virtual void AimAtRotation(float horizontalRotation, float verticalRotation)
		{
			horizontalRotation = MBMath.ClampFloat(horizontalRotation, -3.1415927f, 3.1415927f);
			verticalRotation = MBMath.ClampFloat(verticalRotation, -3.1415927f, 3.1415927f);
			horizontalRotation = MBMath.ClampAngle(horizontalRotation, 0f, this.DirectionRestriction);
			verticalRotation = MBMath.ClampAngle(verticalRotation, this.ReleaseAngleRestrictionCenter, this.ReleaseAngleRestrictionAngle);
			if (!this._exactInputGiven || horizontalRotation != this._inputTargetX || verticalRotation != this._inputTargetY)
			{
				this.GiveExactInput(horizontalRotation, verticalRotation);
			}
		}

		// Token: 0x06002FF9 RID: 12281 RVA: 0x000BDF3E File Offset: 0x000BC13E
		protected void OnLoadingAmmoPointUsingCancelled(Agent agent, bool isCanceledBecauseOfAnimation)
		{
			if (agent.IsAIControlled)
			{
				if (isCanceledBecauseOfAnimation)
				{
					this.SendAgentToAmmoPickup(agent);
					return;
				}
				this.SendReloaderAgentToOriginalPoint();
			}
		}

		// Token: 0x06002FFA RID: 12282 RVA: 0x000BDF59 File Offset: 0x000BC159
		protected void OnAmmoPickupUsingCancelled(Agent agent, bool isCanceledBecauseOfAnimation)
		{
			if (agent.IsAIControlled)
			{
				this.SendAgentToAmmoPickup(agent);
			}
		}

		// Token: 0x06002FFB RID: 12283 RVA: 0x000BDF6C File Offset: 0x000BC16C
		protected void SendAgentToAmmoPickup(Agent agent)
		{
			this.ReloaderAgent = agent;
			EquipmentIndex primaryWieldedItemIndex = agent.GetPrimaryWieldedItemIndex();
			if (primaryWieldedItemIndex != EquipmentIndex.None && agent.Equipment[primaryWieldedItemIndex].CurrentUsageItem.WeaponClass == this.OriginalMissileItem.PrimaryWeapon.WeaponClass)
			{
				agent.AIMoveToGameObjectEnable(this.LoadAmmoStandingPoint, this, base.Ai.GetScriptedFrameFlags(agent));
				return;
			}
			StandingPoint standingPoint = base.AmmoPickUpPoints.FirstOrDefault<StandingPoint>((StandingPoint x) => !x.HasUser);
			if (standingPoint != null)
			{
				agent.AIMoveToGameObjectEnable(standingPoint, this, base.Ai.GetScriptedFrameFlags(agent));
				return;
			}
			this.SendReloaderAgentToOriginalPoint();
		}

		// Token: 0x06002FFC RID: 12284 RVA: 0x000BE01C File Offset: 0x000BC21C
		protected void SendReloaderAgentToOriginalPoint()
		{
			if (this.ReloaderAgent != null)
			{
				if (this.ReloaderAgentOriginalPoint != null && !this.ReloaderAgentOriginalPoint.HasAIMovingTo && !this.ReloaderAgentOriginalPoint.HasUser)
				{
					if (this.ReloaderAgent.InteractingWithAnyGameObject())
					{
						this.ReloaderAgent.StopUsingGameObject(true, Agent.StopUsingGameObjectFlags.None);
					}
					this.ReloaderAgent.AIMoveToGameObjectEnable(this.ReloaderAgentOriginalPoint, this, base.Ai.GetScriptedFrameFlags(this.ReloaderAgent));
					return;
				}
				if (this.ReloaderAgentOriginalPoint == null || (this.ReloaderAgentOriginalPoint.MovingAgent != this.ReloaderAgent && this.ReloaderAgentOriginalPoint.UserAgent != this.ReloaderAgent))
				{
					if (this.ReloaderAgent.IsUsingGameObject)
					{
						this.ReloaderAgent.StopUsingGameObject(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
					}
					this.ReloaderAgent = null;
				}
			}
		}

		// Token: 0x06002FFD RID: 12285 RVA: 0x000BE0E4 File Offset: 0x000BC2E4
		private void UpdateState(float dt)
		{
			if (this.LoadAmmoStandingPoint != null)
			{
				if (this.ReloaderAgent != null)
				{
					if (!this.ReloaderAgent.IsActive() || this.ReloaderAgent.Detachment != this)
					{
						this.ReloaderAgent = null;
					}
					else if (this.ReloaderAgentOriginalPoint.UserAgent == this.ReloaderAgent)
					{
						this.ReloaderAgent = null;
					}
				}
				if (this.State == RangedSiegeWeapon.WeaponState.LoadingAmmo && this.ReloaderAgent == null && !this.LoadAmmoStandingPoint.HasUser)
				{
					this.SortCanPickUpAmmoStandingPoints();
					StandingPoint standingPoint = null;
					StandingPoint standingPoint2 = null;
					foreach (StandingPoint standingPoint3 in this.CanPickUpAmmoStandingPoints)
					{
						if (standingPoint3.HasUser && standingPoint3.UserAgent.IsAIControlled)
						{
							if (standingPoint3 != base.PilotStandingPoint)
							{
								standingPoint = standingPoint3;
								break;
							}
							standingPoint2 = standingPoint3;
						}
					}
					if (standingPoint == null && standingPoint2 != null)
					{
						standingPoint = standingPoint2;
					}
					if (standingPoint != null)
					{
						if (this.HasAmmo)
						{
							Agent userAgent = standingPoint.UserAgent;
							userAgent.StopUsingGameObject(true, Agent.StopUsingGameObjectFlags.DoNotWieldWeaponAfterStoppingUsingGameObject);
							this.ReloaderAgentOriginalPoint = standingPoint;
							this.SendAgentToAmmoPickup(userAgent);
						}
						else
						{
							base.IsDisabledForAI = true;
						}
					}
				}
			}
			switch (this.State)
			{
			case RangedSiegeWeapon.WeaponState.Idle:
			case RangedSiegeWeapon.WeaponState.WaitingAfterShooting:
			case RangedSiegeWeapon.WeaponState.LoadingAmmo:
			case RangedSiegeWeapon.WeaponState.WaitingBeforeIdle:
				return;
			case RangedSiegeWeapon.WeaponState.WaitingBeforeProjectileLeaving:
				goto IL_0429;
			case RangedSiegeWeapon.WeaponState.Shooting:
			{
				for (int i = 0; i < this.Skeletons.Length; i++)
				{
					int animationIndexAtChannel = this.Skeletons[i].GetAnimationIndexAtChannel(0);
					float animationParameterAtChannel = this.Skeletons[i].GetAnimationParameterAtChannel(0);
					if (animationIndexAtChannel == this.FireAnimationIndices[i] && animationParameterAtChannel >= 0.9999f)
					{
						this.State = ((!this.AttackClickWillReload) ? RangedSiegeWeapon.WeaponState.WaitingBeforeReloading : RangedSiegeWeapon.WeaponState.WaitingAfterShooting);
						this._animationTimeElapsed = 0f;
					}
				}
				return;
			}
			case RangedSiegeWeapon.WeaponState.WaitingBeforeReloading:
				break;
			case RangedSiegeWeapon.WeaponState.Reloading:
			{
				int num = 0;
				if (this.ReloadStandingPoints.Count == 0)
				{
					if (base.PilotAgent != null && !base.PilotAgent.IsInBeingStruckAction)
					{
						num = 1;
					}
				}
				else
				{
					foreach (StandingPoint standingPoint4 in this.ReloadStandingPoints)
					{
						if (standingPoint4.HasUser && !standingPoint4.UserAgent.IsInBeingStruckAction)
						{
							num++;
						}
					}
				}
				if (num == 0)
				{
					this.State = RangedSiegeWeapon.WeaponState.ReloadingPaused;
					return;
				}
				if (this._currentReloaderCount != num)
				{
					this._currentReloaderCount = num;
					float num2 = MathF.Sqrt((float)this._currentReloaderCount);
					for (int j = 0; j < this.SkeletonOwnerObjects.Length; j++)
					{
						float animationParameterAtChannel2 = this.SkeletonOwnerObjects[j].GameEntity.Skeleton.GetAnimationParameterAtChannel(0);
						this.SkeletonOwnerObjects[j].SetAnimationAtChannelSynched(this.SetUpAnimations[j], 0, num2);
						if (animationParameterAtChannel2 > 0f)
						{
							this.SkeletonOwnerObjects[j].SetAnimationChannelParameterSynched(0, animationParameterAtChannel2);
						}
					}
				}
				for (int k = 0; k < this.Skeletons.Length; k++)
				{
					int animationIndexAtChannel2 = this.Skeletons[k].GetAnimationIndexAtChannel(0);
					float animationParameterAtChannel3 = this.Skeletons[k].GetAnimationParameterAtChannel(0);
					this.Skeletons[k].SetAnimationSpeedAtChannel(0, this.FinalReloadSpeed * this.ReloadSpeedMultiplier);
					if (animationIndexAtChannel2 == this.SetUpAnimationIndices[k] && animationParameterAtChannel3 >= 0.9999f)
					{
						this.State = RangedSiegeWeapon.WeaponState.LoadingAmmo;
						this._animationTimeElapsed = 0f;
					}
				}
				return;
			}
			case RangedSiegeWeapon.WeaponState.ReloadingPaused:
				if (this.ReloadStandingPoints.Count == 0)
				{
					if (base.PilotAgent != null && !base.PilotAgent.IsInBeingStruckAction)
					{
						this.State = RangedSiegeWeapon.WeaponState.Reloading;
						return;
					}
					return;
				}
				else
				{
					using (List<StandingPoint>.Enumerator enumerator = this.ReloadStandingPoints.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							StandingPoint standingPoint5 = enumerator.Current;
							if (standingPoint5.HasUser && !standingPoint5.UserAgent.IsInBeingStruckAction)
							{
								this.State = RangedSiegeWeapon.WeaponState.Reloading;
								break;
							}
						}
						return;
					}
				}
				break;
			default:
				Debug.FailedAssert("Invalid WeaponState.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Objects\\Siege\\RangedSiegeWeapon.cs", "UpdateState", 2000);
				return;
			}
			this._animationTimeElapsed += dt;
			if (this._animationTimeElapsed < this.TimeGapBetweenShootingEndAndReloadingStart || (this._cameraState != RangedSiegeWeapon.CameraState.RememberLastShotDirection && this._cameraState != RangedSiegeWeapon.CameraState.FreeMove && this._cameraState != RangedSiegeWeapon.CameraState.StickToWeapon && !(this.CameraHolder == null)))
			{
				return;
			}
			if (this.ReloadStandingPoints.Count == 0)
			{
				if (base.PilotAgent != null && !base.PilotAgent.IsInBeingStruckAction)
				{
					this.State = RangedSiegeWeapon.WeaponState.Reloading;
					return;
				}
				return;
			}
			else
			{
				using (List<StandingPoint>.Enumerator enumerator = this.ReloadStandingPoints.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						StandingPoint standingPoint6 = enumerator.Current;
						if (standingPoint6.HasUser && !standingPoint6.UserAgent.IsInBeingStruckAction)
						{
							this.State = RangedSiegeWeapon.WeaponState.Reloading;
							break;
						}
					}
					return;
				}
			}
			IL_0429:
			this._animationTimeElapsed += dt;
			if (this._animationTimeElapsed >= this.TimeGapBetweenShootActionAndProjectileLeaving)
			{
				this.State = RangedSiegeWeapon.WeaponState.Shooting;
				return;
			}
		}

		// Token: 0x06002FFE RID: 12286 RVA: 0x000BE5F8 File Offset: 0x000BC7F8
		public bool Shoot()
		{
			this.LastShooterAgent = base.PilotAgent;
			if (this.State == RangedSiegeWeapon.WeaponState.Idle)
			{
				this.State = RangedSiegeWeapon.WeaponState.WaitingBeforeProjectileLeaving;
				if (!GameNetwork.IsClientOrReplay)
				{
					this._animationTimeElapsed = 0f;
				}
				return true;
			}
			return false;
		}

		// Token: 0x06002FFF RID: 12287 RVA: 0x000BE62A File Offset: 0x000BC82A
		public void ManualReload()
		{
			if (this.AttackClickWillReload)
			{
				this.State = RangedSiegeWeapon.WeaponState.WaitingBeforeReloading;
			}
		}

		// Token: 0x06003000 RID: 12288 RVA: 0x000BE63B File Offset: 0x000BC83B
		public void AiRequestsShoot()
		{
			this._aiRequestsShoot = true;
		}

		// Token: 0x06003001 RID: 12289 RVA: 0x000BE644 File Offset: 0x000BC844
		public void AiRequestsManualReload()
		{
			this._aiRequestsManualReload = true;
		}

		// Token: 0x06003002 RID: 12290 RVA: 0x000BE650 File Offset: 0x000BC850
		private Vec3 GetBallisticErrorAppliedDirection(float BallisticErrorAmount)
		{
			Mat3 mat = new Mat3
			{
				f = this.ShootingDirection,
				u = Vec3.Up
			};
			mat.Orthonormalize();
			float num = MBRandom.RandomFloat * 6.2831855f;
			mat.RotateAboutForward(num);
			float num2 = BallisticErrorAmount * MBRandom.RandomFloat;
			mat.RotateAboutSide(num2.ToRadians());
			return mat.f;
		}

		// Token: 0x06003003 RID: 12291 RVA: 0x000BE6B8 File Offset: 0x000BC8B8
		protected void ShootProjectile()
		{
			if (this.LoadedMissileItem.StringId == this.MultipleProjectileId)
			{
				ItemObject @object = Game.Current.ObjectManager.GetObject<ItemObject>(this.MultipleProjectileFlyingId);
				for (int i = 0; i < this.MultipleProjectileCount; i++)
				{
					this.ShootProjectileAux(@object, true);
				}
			}
			else if (this.LoadedMissileItem.StringId == this.MultipleFireProjectileId)
			{
				ItemObject object2 = Game.Current.ObjectManager.GetObject<ItemObject>(this.MultipleFireProjectileFlyingId);
				for (int j = 0; j < this.MultipleProjectileCount; j++)
				{
					this.ShootProjectileAux(object2, true);
				}
			}
			else if (this.LoadedMissileItem.StringId == this.SingleProjectileId)
			{
				this.ShootProjectileAux(Game.Current.ObjectManager.GetObject<ItemObject>(this.SingleProjectileFlyingId), false);
			}
			else if (this.LoadedMissileItem.StringId == this.SingleFireProjectileId)
			{
				this.ShootProjectileAux(Game.Current.ObjectManager.GetObject<ItemObject>(this.SingleFireProjectileFlyingId), false);
			}
			else
			{
				this.ShootProjectileAux(this.LoadedMissileItem, false);
			}
			this.LastShooterAgent = null;
		}

		// Token: 0x06003004 RID: 12292 RVA: 0x000BE7E0 File Offset: 0x000BC9E0
		protected virtual Mission.Missile ShootProjectileAux(ItemObject missileItem, bool randomizeMissileSpeed)
		{
			Vec3 vec;
			Mat3 mat;
			float num;
			float num2;
			this.SetupProjectileToShoot(randomizeMissileSpeed, out vec, out mat, out num, out num2);
			MissionObject missionObject = base.GameEntity.Root.GetFirstScriptOfType<MissionObject>() ?? this;
			Mission mission = Mission.Current;
			Agent lastShooterAgent = this.LastShooterAgent;
			ItemModifier itemModifier = null;
			IAgentOriginBase origin = this.LastShooterAgent.Origin;
			return mission.AddCustomMissile(lastShooterAgent, new MissionWeapon(missileItem, itemModifier, (origin != null) ? origin.Banner : null, 1), this.ProjectileEntityCurrentGlobalPosition, vec, mat, num2, num, false, missionObject, -1);
		}

		// Token: 0x06003005 RID: 12293 RVA: 0x000BE858 File Offset: 0x000BCA58
		protected void SetupProjectileToShoot(bool randomizeMissileSpeed, out Vec3 direction, out Mat3 orientation, out float missileBaseSpeed, out float missileShootingSpeed)
		{
			orientation = Mat3.Identity;
			Vec3 globalVelocity = this.GetGlobalVelocity();
			if (randomizeMissileSpeed)
			{
				float num = this.ShootingSpeed * MBRandom.RandomFloatRanged(0.9f, 1.1f);
				orientation.f = this.GetBallisticErrorAppliedDirection(2.5f);
				orientation.Orthonormalize();
				direction = num * orientation.f + globalVelocity;
				missileShootingSpeed = direction.Normalize();
				missileBaseSpeed = num;
				return;
			}
			orientation.f = this.GetBallisticErrorAppliedDirection(this.MaximumBallisticError);
			orientation.Orthonormalize();
			direction = this.ShootingSpeed * orientation.f + globalVelocity;
			missileShootingSpeed = direction.Normalize();
			missileBaseSpeed = this.ShootingSpeed;
		}

		// Token: 0x170008FC RID: 2300
		// (get) Token: 0x06003006 RID: 12294 RVA: 0x000BE918 File Offset: 0x000BCB18
		protected virtual Vec3 ShootingDirection
		{
			get
			{
				return this.Projectile.GameEntity.GetGlobalFrame().rotation.u.NormalizedCopy();
			}
		}

		// Token: 0x170008FD RID: 2301
		// (get) Token: 0x06003007 RID: 12295 RVA: 0x000BE94C File Offset: 0x000BCB4C
		public virtual Vec3 ProjectileEntityCurrentGlobalPosition
		{
			get
			{
				return this.Projectile.GameEntity.GetGlobalFrame().origin;
			}
		}

		// Token: 0x06003008 RID: 12296 RVA: 0x000BE974 File Offset: 0x000BCB74
		protected void OnRotationStarted()
		{
			if (this.MoveSound == null || !this.MoveSound.IsValid)
			{
				this.MoveSound = SoundEvent.CreateEvent(this.MoveSoundIndex, base.Scene);
				this.MoveSound.PlayInPosition(this.RotationObject.GameEntity.GlobalPosition);
			}
		}

		// Token: 0x06003009 RID: 12297 RVA: 0x000BE9CC File Offset: 0x000BCBCC
		protected void OnRotationStopped()
		{
			this.MoveSound.Stop();
			this.MoveSound = null;
		}

		// Token: 0x0600300A RID: 12298
		public abstract override SiegeEngineType GetSiegeEngineType();

		// Token: 0x170008FE RID: 2302
		// (get) Token: 0x0600300B RID: 12299 RVA: 0x000BE9E0 File Offset: 0x000BCBE0
		public override BattleSideEnum Side
		{
			get
			{
				if (base.PilotAgent != null)
				{
					return base.PilotAgent.Team.Side;
				}
				return this.DefaultSide;
			}
		}

		// Token: 0x0600300C RID: 12300 RVA: 0x000BEA04 File Offset: 0x000BCC04
		public bool CanShootAtThreat(Threat threat, int attemptCount = 5)
		{
			WeakGameEntity weakGameEntity = WeakGameEntity.Invalid;
			if (threat.TargetableObject != null)
			{
				weakGameEntity = threat.TargetableObject.GetTargetEntity();
			}
			ValueTuple<Vec3, Vec3> valueTuple = threat.ComputeGlobalTargetingBoundingBoxMinMax();
			Vec3 item = valueTuple.Item1;
			Vec3 item2 = valueTuple.Item2;
			Vec3 vec = (item2 + item) / 2f;
			Vec3 vec2 = new Vec3(vec.AsVec2, item.z, -1f);
			Vec3 vec3 = new Vec3(vec.AsVec2, item2.z, -1f);
			for (int i = 0; i < attemptCount; i++)
			{
				Vec3 vec4 = Vec3.Lerp(vec2, vec3, (float)i / (float)(attemptCount - 1));
				Scene scene = base.Scene;
				Vec3 vec5 = this.MissileStartingGlobalPositionForSimulation;
				float num;
				GameEntity gameEntity;
				if (!scene.RayCastForClosestEntityOrTerrainIgnoreEntity(in vec5, in vec4, base.GameEntity.Root, out num, out gameEntity, this._projectileRadiusCached, BodyFlags.CommonCollisionExcludeFlagsForMissile) || (!(gameEntity == null) && !(gameEntity.Root != weakGameEntity.Root)))
				{
					Vec3 estimatedTargetMovementVector = this.GetEstimatedTargetMovementVector(vec4, threat.GetGlobalVelocity());
					vec4 += estimatedTargetMovementVector;
					Scene scene2 = base.Scene;
					vec5 = this.MissileStartingGlobalPositionForSimulation;
					if ((!scene2.RayCastForClosestEntityOrTerrainIgnoreEntity(in vec5, in vec4, base.GameEntity.Root, out num, out gameEntity, this._projectileRadiusCached, BodyFlags.CommonCollisionExcludeFlagsForMissile) || (!(gameEntity == null) && !(gameEntity.Root != weakGameEntity.Root))) && this.CanShootAtPoint(vec4))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600300D RID: 12301 RVA: 0x000BEB84 File Offset: 0x000BCD84
		public bool CanShootAtAgent(Agent agent, int attemptCount = 5)
		{
			WeakGameEntity root = base.GameEntity.Root;
			ValueTuple<Vec3, Vec3> boxMinMax = agent.CollisionCapsule.GetBoxMinMax();
			Vec3 item = boxMinMax.Item1;
			Vec3 item2 = boxMinMax.Item2;
			Vec3 vec = (item2 + item) / 2f;
			Vec3 vec2 = new Vec3(vec.AsVec2, item.z, -1f);
			Vec3 vec3 = new Vec3(vec.AsVec2, item2.z, -1f);
			for (int i = 0; i < attemptCount; i++)
			{
				Vec3 vec4 = Vec3.Lerp(vec2, vec3, (float)i / (float)(attemptCount - 1));
				Scene scene = base.Scene;
				Vec3 vec5 = this.MissileStartingGlobalPositionForSimulation;
				float num;
				GameEntity gameEntity;
				if (!scene.RayCastForClosestEntityOrTerrainIgnoreEntity(in vec5, in vec4, root, out num, out gameEntity, this._projectileRadiusCached, BodyFlags.CommonCollisionExcludeFlagsForMissile))
				{
					vec5 = agent.GetAverageRealGlobalVelocity();
					Vec3 vec6 = new Vec3(vec5.AsVec2, 0f, -1f);
					Vec3 estimatedTargetMovementVector = this.GetEstimatedTargetMovementVector(vec4, vec6);
					vec4 += estimatedTargetMovementVector;
					Scene scene2 = base.Scene;
					vec5 = this.MissileStartingGlobalPositionForSimulation;
					if (!scene2.RayCastForClosestEntityOrTerrainIgnoreEntity(in vec5, in vec4, root, out num, out gameEntity, this._projectileRadiusCached, BodyFlags.CommonCollisionExcludeFlagsForMissile) && this.CanShootAtPoint(vec4))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600300E RID: 12302 RVA: 0x000BECC8 File Offset: 0x000BCEC8
		public virtual Vec3 GetEstimatedTargetMovementVector(Vec3 targetCurrentPosition, Vec3 targetVelocity)
		{
			if (targetVelocity != Vec3.Zero)
			{
				return targetVelocity * ((base.GameEntity.GlobalPosition - targetCurrentPosition).Length / this.ShootingSpeed + this.TimeGapBetweenShootActionAndProjectileLeaving);
			}
			return Vec3.Zero;
		}

		// Token: 0x0600300F RID: 12303 RVA: 0x000BED18 File Offset: 0x000BCF18
		public bool CanShootAtPoint(Vec3 target)
		{
			float num;
			float num2;
			this.CalculateLocalDirectionAndLocalAngleToShootTarget(target, out num, out num2);
			if (num2 < this.BottomReleaseAngleRestriction || num2 > this.TopReleaseAngleRestriction)
			{
				return false;
			}
			if (this.DirectionRestriction / 2f - MathF.Abs(num) < 0f)
			{
				return false;
			}
			if (this.CheckFriendlyFireForObjects(target))
			{
				return false;
			}
			Vec3 missileStartingGlobalPositionForSimulation = this.MissileStartingGlobalPositionForSimulation;
			MatrixFrame globalFrame = base.GameEntity.GetGlobalFrame();
			Vec3 u = globalFrame.rotation.u;
			if (!u.IsUnit)
			{
				u.Normalize();
			}
			globalFrame.rotation.RotateAboutAnArbitraryVector(in u, 3.1415927f + num);
			Vec3 s = globalFrame.rotation.s;
			if (!s.IsUnit)
			{
				s.Normalize();
			}
			globalFrame.rotation.RotateAboutAnArbitraryVector(in s, num2);
			float x = globalFrame.rotation.GetEulerAngles().x;
			Vec3 vec = ((this.MissileStartingPositionEntityForSimulation == null) ? this.CanShootAtPointCheckingOffset : Vec3.Zero);
			return this.CanShootPointBallistic(missileStartingGlobalPositionForSimulation + vec, x, this.ShootingSpeed, target);
		}

		// Token: 0x06003010 RID: 12304 RVA: 0x000BEE2C File Offset: 0x000BD02C
		private bool CanShootPointBallistic(Vec3 startGlobalPos, float verticalAngle, float shootingSpeed, Vec3 targetGlobalPos)
		{
			float num = shootingSpeed * MathF.Sin(verticalAngle) / 9.806f;
			float num2 = 4.903f * num * num;
			Vec3 vec = (startGlobalPos + targetGlobalPos) / 2f + new Vec3(0f, 0f, num2, -1f);
			float projectileRadiusCached = this._projectileRadiusCached;
			if (verticalAngle <= 0f)
			{
				float num3;
				Agent agent = Mission.Current.RayCastForClosestAgent(startGlobalPos, targetGlobalPos, -1, projectileRadiusCached, out num3);
				if (agent != null && !agent.IsEnemyOf(base.PilotAgent))
				{
					return false;
				}
			}
			else
			{
				float num3;
				GameEntity gameEntity;
				if (base.Scene.RayCastForClosestEntityOrTerrainIgnoreEntity(in startGlobalPos, in vec, base.GameEntity.Root, out num3, out gameEntity, projectileRadiusCached, BodyFlags.CommonCollisionExcludeFlagsForMissile))
				{
					return false;
				}
				Agent agent2 = Mission.Current.RayCastForClosestAgent(startGlobalPos, vec, -1, projectileRadiusCached, out num3);
				if (agent2 != null && !agent2.IsEnemyOf(base.PilotAgent))
				{
					return false;
				}
				agent2 = Mission.Current.RayCastForClosestAgent(vec, targetGlobalPos, -1, projectileRadiusCached * 2f, out num3);
				if (agent2 != null && !agent2.IsEnemyOf(base.PilotAgent))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003011 RID: 12305 RVA: 0x000BEF40 File Offset: 0x000BD140
		protected unsafe virtual bool CheckFriendlyFireForObjects(Vec3 target)
		{
			if (this.Side == BattleSideEnum.Attacker)
			{
				foreach (SiegeWeapon siegeWeapon in *Mission.Current.GetAttackerWeaponsForFriendlyFirePreventing())
				{
					if (siegeWeapon.GameEntity != null && siegeWeapon.GameEntity.IsVisibleIncludeParents())
					{
						Vec3 vec = siegeWeapon.GameEntity.ComputeGlobalPhysicsBoundingBoxCenter();
						Vec3 missileStartingGlobalPositionForSimulation = this.MissileStartingGlobalPositionForSimulation;
						if ((MBMath.GetClosestPointOnLineSegmentToPoint(in missileStartingGlobalPositionForSimulation, in target, in vec) - vec).LengthSquared < 100f)
						{
							return true;
						}
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x06003012 RID: 12306 RVA: 0x000BEFFC File Offset: 0x000BD1FC
		protected internal virtual bool IsTargetValid(ITargetable target)
		{
			return true;
		}

		// Token: 0x06003013 RID: 12307 RVA: 0x000BEFFF File Offset: 0x000BD1FF
		public override OrderType GetOrder(BattleSideEnum side)
		{
			if (base.IsDestroyed)
			{
				return OrderType.None;
			}
			if (this.Side != side)
			{
				return OrderType.AttackEntity;
			}
			return OrderType.Use;
		}

		// Token: 0x06003014 RID: 12308 RVA: 0x000BF019 File Offset: 0x000BD219
		protected override WeakGameEntity GetEntityToAttachNavMeshFaces()
		{
			return this.RotationObject.GameEntity;
		}

		// Token: 0x06003015 RID: 12309
		public abstract float ProcessTargetValue(float baseValue, TargetFlags flags);

		// Token: 0x06003016 RID: 12310 RVA: 0x000BF028 File Offset: 0x000BD228
		public override void OnAfterReadFromNetwork(ValueTuple<BaseSynchedMissionObjectReadableRecord, ISynchedMissionObjectReadableRecord> synchedMissionObjectReadableRecord, bool allowVisibilityUpdate = true)
		{
			base.OnAfterReadFromNetwork(synchedMissionObjectReadableRecord, allowVisibilityUpdate);
			RangedSiegeWeapon.RangedSiegeWeaponRecord rangedSiegeWeaponRecord = (RangedSiegeWeapon.RangedSiegeWeaponRecord)synchedMissionObjectReadableRecord.Item2;
			this._state = (RangedSiegeWeapon.WeaponState)rangedSiegeWeaponRecord.State;
			this.TargetDirection = rangedSiegeWeaponRecord.TargetDirection;
			this.TargetReleaseAngle = MBMath.ClampFloat(rangedSiegeWeaponRecord.TargetReleaseAngle, this.BottomReleaseAngleRestriction, this.TopReleaseAngleRestriction);
			this.AmmoCount = rangedSiegeWeaponRecord.AmmoCount;
			this.CurrentDirection = this.TargetDirection;
			this.CurrentReleaseAngle = this.TargetReleaseAngle;
			this.CurrentDirection = this.TargetDirection;
			this.CurrentReleaseAngle = this.TargetReleaseAngle;
			this.ApplyCurrentDirectionToEntity();
			this.CheckAmmo();
			this.UpdateAmmoMesh();
			this.ChangeProjectileEntityClient(rangedSiegeWeaponRecord.ProjectileIndex);
		}

		// Token: 0x06003017 RID: 12311 RVA: 0x000BF0E0 File Offset: 0x000BD2E0
		protected virtual void UpdateAmmoMesh()
		{
			WeakGameEntity weakGameEntity = base.AmmoPickUpPoints[0].GameEntity;
			int num = this.StartingAmmoCount - this.AmmoCount;
			while (weakGameEntity.Parent.IsValid)
			{
				for (int i = 0; i < weakGameEntity.MultiMeshComponentCount; i++)
				{
					MetaMesh metaMesh = weakGameEntity.GetMetaMesh(i);
					for (int j = 0; j < metaMesh.MeshCount; j++)
					{
						metaMesh.GetMeshAtIndex(j).SetVectorArgument(0f, (float)num, 0f, 0f);
					}
				}
				weakGameEntity = weakGameEntity.Parent;
			}
		}

		// Token: 0x06003018 RID: 12312 RVA: 0x000BF177 File Offset: 0x000BD377
		protected override bool IsAnyUserBelongsToFormation(Formation formation)
		{
			bool flag = base.IsAnyUserBelongsToFormation(formation);
			Agent reloaderAgent = this.ReloaderAgent;
			return flag | (((reloaderAgent != null) ? reloaderAgent.Formation : null) == formation);
		}

		// Token: 0x06003019 RID: 12313 RVA: 0x000BF196 File Offset: 0x000BD396
		public virtual Vec3 GetGlobalVelocity()
		{
			return Vec3.Zero;
		}

		// Token: 0x0600301A RID: 12314 RVA: 0x000BF1A0 File Offset: 0x000BD3A0
		private float ComputeProjectileCapsuleRadius()
		{
			float num = 0.01f;
			if (this.LoadedMissileItem.BodyName != null)
			{
				PhysicsShape fromResource = PhysicsShape.GetFromResource(this.LoadedMissileItem.BodyName, false);
				BoundingBox boundingBox = new BoundingBox(in Vec3.Zero);
				fromResource.GetBoundingBox(out boundingBox);
				num = (boundingBox.max.AsVec2 - boundingBox.min.AsVec2).Length / 2f;
			}
			return num;
		}

		// Token: 0x0600301B RID: 12315 RVA: 0x000BF210 File Offset: 0x000BD410
		private void OnLoadedMissileItemChanged()
		{
			if (!this.LoadedMissileItem.StringId.Equals(this._lastLoadedMissileItemId))
			{
				this._projectileRadiusCached = this.ComputeProjectileCapsuleRadius();
				this._lastLoadedMissileItemId = this.LoadedMissileItem.StringId;
			}
		}

		// Token: 0x0600301C RID: 12316 RVA: 0x000BF247 File Offset: 0x000BD447
		public void SetPlayerForceUse(bool value)
		{
			this.PlayerForceUse = value;
		}

		// Token: 0x0600301D RID: 12317 RVA: 0x000BF250 File Offset: 0x000BD450
		protected override bool ShouldDisableTickIfMachineDisabled()
		{
			return base.AmmoPickUpPoints.Count == 0;
		}

		// Token: 0x0600301E RID: 12318 RVA: 0x000BF260 File Offset: 0x000BD460
		public override void OnShipCaptured(BattleSideEnum newDefaultSide)
		{
			base.OnShipCaptured(newDefaultSide);
			this.DefaultSide = newDefaultSide;
		}

		// Token: 0x0600301F RID: 12319 RVA: 0x000BF270 File Offset: 0x000BD470
		public override void OnDeploymentFinished()
		{
			base.OnDeploymentFinished();
			(base.Ai as RangedSiegeWeaponAi).InitializeThreatSeeker();
		}

		// Token: 0x04001380 RID: 4992
		private const float DefaultMissileRadius = 0.01f;

		// Token: 0x04001381 RID: 4993
		public const float DefaultDirectionRestriction = 2.0943952f;

		// Token: 0x04001382 RID: 4994
		public const string CanGoAmmoPickupTag = "can_pick_up_ammo";

		// Token: 0x04001383 RID: 4995
		public const string DontApplySidePenaltyTag = "no_ammo_pick_up_penalty";

		// Token: 0x04001384 RID: 4996
		public const string ReloadTag = "reload";

		// Token: 0x04001385 RID: 4997
		public const string AmmoLoadTag = "ammoload";

		// Token: 0x04001386 RID: 4998
		public const string CameraHolderTag = "cameraHolder";

		// Token: 0x04001387 RID: 4999
		public const string ProjectileTag = "projectile";

		// Token: 0x04001389 RID: 5001
		public string MissileItemID;

		// Token: 0x0400138A RID: 5002
		protected bool UsesMouseForAiming;

		// Token: 0x0400138B RID: 5003
		[EditableScriptComponentVariable(true, "")]
		protected int MultipleProjectileCount = 5;

		// Token: 0x0400138C RID: 5004
		private RangedSiegeWeapon.WeaponState _state;

		// Token: 0x0400138D RID: 5005
		public RangedSiegeWeapon.FiringFocus Focus;

		// Token: 0x04001390 RID: 5008
		private int _projectileIndex;

		// Token: 0x04001391 RID: 5009
		protected GameEntity MissileStartingPositionEntityForSimulation;

		// Token: 0x04001392 RID: 5010
		protected Skeleton[] Skeletons;

		// Token: 0x04001393 RID: 5011
		protected SynchedMissionObject[] SkeletonOwnerObjects;

		// Token: 0x04001394 RID: 5012
		protected string[] SkeletonNames;

		// Token: 0x04001395 RID: 5013
		protected string[] FireAnimations;

		// Token: 0x04001396 RID: 5014
		protected string[] SetUpAnimations;

		// Token: 0x04001397 RID: 5015
		protected int[] FireAnimationIndices;

		// Token: 0x04001398 RID: 5016
		protected int[] SetUpAnimationIndices;

		// Token: 0x04001399 RID: 5017
		protected SynchedMissionObject RotationObject;

		// Token: 0x0400139A RID: 5018
		private MatrixFrame _rotationObjectInitialFrame;

		// Token: 0x0400139B RID: 5019
		protected SoundEvent MoveSound;

		// Token: 0x0400139C RID: 5020
		protected SoundEvent ReloadSound;

		// Token: 0x0400139D RID: 5021
		protected int MoveSoundIndex = -1;

		// Token: 0x0400139E RID: 5022
		protected int ReloadSoundIndex = -1;

		// Token: 0x0400139F RID: 5023
		protected int FireSoundIndex = -1;

		// Token: 0x040013A0 RID: 5024
		protected ItemObject OriginalMissileItem;

		// Token: 0x040013A1 RID: 5025
		protected WeaponStatsData OriginalMissileWeaponStatsDataForTargeting;

		// Token: 0x040013A2 RID: 5026
		private ItemObject _loadedMissileItem;

		// Token: 0x040013A3 RID: 5027
		protected List<StandingPoint> CanPickUpAmmoStandingPoints;

		// Token: 0x040013A4 RID: 5028
		protected List<StandingPoint> ReloadStandingPoints;

		// Token: 0x040013A5 RID: 5029
		protected StandingPointWithWeaponRequirement LoadAmmoStandingPoint;

		// Token: 0x040013A6 RID: 5030
		protected Dictionary<StandingPoint, float> PilotReservePriorityValues = new Dictionary<StandingPoint, float>();

		// Token: 0x040013A7 RID: 5031
		protected Agent ReloaderAgent;

		// Token: 0x040013A8 RID: 5032
		protected StandingPoint ReloaderAgentOriginalPoint;

		// Token: 0x040013AA RID: 5034
		protected bool AttackClickWillReload;

		// Token: 0x040013AB RID: 5035
		protected bool WeaponNeedsClickToReload;

		// Token: 0x040013AC RID: 5036
		protected float FinalReloadSpeed = 1f;

		// Token: 0x040013AD RID: 5037
		protected float BaseReloadSpeed = 1f;

		// Token: 0x040013AE RID: 5038
		public int StartingAmmoCount;

		// Token: 0x040013AF RID: 5039
		protected int CurrentAmmo = 1;

		// Token: 0x040013B1 RID: 5041
		protected float TargetDirection;

		// Token: 0x040013B2 RID: 5042
		protected float TargetReleaseAngle;

		// Token: 0x040013B3 RID: 5043
		protected float CameraDirection;

		// Token: 0x040013B4 RID: 5044
		protected float CameraReleaseAngle;

		// Token: 0x040013B5 RID: 5045
		protected float ReloadTargetReleaseAngle;

		// Token: 0x040013B6 RID: 5046
		private MatrixFrame _cameraHolderInitialFrame;

		// Token: 0x040013B7 RID: 5047
		protected float MaxRotateSpeed;

		// Token: 0x040013B8 RID: 5048
		private RangedSiegeWeapon.CameraState _cameraState;

		// Token: 0x040013B9 RID: 5049
		private bool _inputGiven;

		// Token: 0x040013BA RID: 5050
		protected float DontMoveTimer;

		// Token: 0x040013BB RID: 5051
		private float _inputX;

		// Token: 0x040013BC RID: 5052
		private float _inputY;

		// Token: 0x040013BD RID: 5053
		private bool _exactInputGiven;

		// Token: 0x040013BE RID: 5054
		private float _inputTargetX;

		// Token: 0x040013BF RID: 5055
		private float _inputTargetY;

		// Token: 0x040013C0 RID: 5056
		private Vec3 _ammoPickupCenter;

		// Token: 0x040013C1 RID: 5057
		private float _lastSyncedDirection;

		// Token: 0x040013C2 RID: 5058
		private float _lastSyncedReleaseAngle;

		// Token: 0x040013C3 RID: 5059
		private float _syncTimer;

		// Token: 0x040013C4 RID: 5060
		private float _cameraMoveBackFactor;

		// Token: 0x040013C5 RID: 5061
		public float TopReleaseAngleRestriction = 1.5707964f;

		// Token: 0x040013C6 RID: 5062
		public float BottomReleaseAngleRestriction = -1.5707964f;

		// Token: 0x040013C7 RID: 5063
		protected float CurrentDirection;

		// Token: 0x040013C8 RID: 5064
		protected float CurrentReleaseAngle;

		// Token: 0x040013C9 RID: 5065
		protected float ReleaseAngleRestrictionCenter;

		// Token: 0x040013CA RID: 5066
		protected float ReleaseAngleRestrictionAngle;

		// Token: 0x040013CB RID: 5067
		private float _animationTimeElapsed;

		// Token: 0x040013CC RID: 5068
		protected float TimeGapBetweenShootingEndAndReloadingStart = 0.6f;

		// Token: 0x040013CD RID: 5069
		protected float TimeGapBetweenShootActionAndProjectileLeaving;

		// Token: 0x040013CE RID: 5070
		private int _currentReloaderCount;

		// Token: 0x040013CF RID: 5071
		protected Agent LastShooterAgent;

		// Token: 0x040013D0 RID: 5072
		private float _lastCanPickUpAmmoStandingPointsSortedAngle = -3.1415927f;

		// Token: 0x040013D1 RID: 5073
		protected BattleSideEnum DefaultSide;

		// Token: 0x040013D2 RID: 5074
		private bool _aiRequestsShoot;

		// Token: 0x040013D3 RID: 5075
		private bool _aiRequestsManualReload;

		// Token: 0x040013D4 RID: 5076
		private bool _hasFrameChangedInPreviousFrame;

		// Token: 0x040013D5 RID: 5077
		private string _lastLoadedMissileItemId;

		// Token: 0x040013D6 RID: 5078
		private float _projectileRadiusCached;

		// Token: 0x02000620 RID: 1568
		[DefineSynchedMissionObjectType(typeof(RangedSiegeWeapon))]
		public struct RangedSiegeWeaponRecord : ISynchedMissionObjectReadableRecord
		{
			// Token: 0x17000AAE RID: 2734
			// (get) Token: 0x06003FCC RID: 16332 RVA: 0x000F7CB8 File Offset: 0x000F5EB8
			// (set) Token: 0x06003FCD RID: 16333 RVA: 0x000F7CC0 File Offset: 0x000F5EC0
			public int State { get; private set; }

			// Token: 0x17000AAF RID: 2735
			// (get) Token: 0x06003FCE RID: 16334 RVA: 0x000F7CC9 File Offset: 0x000F5EC9
			// (set) Token: 0x06003FCF RID: 16335 RVA: 0x000F7CD1 File Offset: 0x000F5ED1
			public float TargetDirection { get; private set; }

			// Token: 0x17000AB0 RID: 2736
			// (get) Token: 0x06003FD0 RID: 16336 RVA: 0x000F7CDA File Offset: 0x000F5EDA
			// (set) Token: 0x06003FD1 RID: 16337 RVA: 0x000F7CE2 File Offset: 0x000F5EE2
			public float TargetReleaseAngle { get; private set; }

			// Token: 0x17000AB1 RID: 2737
			// (get) Token: 0x06003FD2 RID: 16338 RVA: 0x000F7CEB File Offset: 0x000F5EEB
			// (set) Token: 0x06003FD3 RID: 16339 RVA: 0x000F7CF3 File Offset: 0x000F5EF3
			public int AmmoCount { get; private set; }

			// Token: 0x17000AB2 RID: 2738
			// (get) Token: 0x06003FD4 RID: 16340 RVA: 0x000F7CFC File Offset: 0x000F5EFC
			// (set) Token: 0x06003FD5 RID: 16341 RVA: 0x000F7D04 File Offset: 0x000F5F04
			public int ProjectileIndex { get; private set; }

			// Token: 0x06003FD6 RID: 16342 RVA: 0x000F7D10 File Offset: 0x000F5F10
			public bool ReadFromNetwork(ref bool bufferReadValid)
			{
				this.State = GameNetworkMessage.ReadIntFromPacket(CompressionMission.RangedSiegeWeaponStateCompressionInfo, ref bufferReadValid);
				this.TargetDirection = GameNetworkMessage.ReadFloatFromPacket(CompressionBasic.RadianCompressionInfo, ref bufferReadValid);
				this.TargetReleaseAngle = GameNetworkMessage.ReadFloatFromPacket(CompressionBasic.RadianCompressionInfo, ref bufferReadValid);
				this.AmmoCount = GameNetworkMessage.ReadIntFromPacket(CompressionMission.RangedSiegeWeaponAmmoCompressionInfo, ref bufferReadValid);
				this.ProjectileIndex = GameNetworkMessage.ReadIntFromPacket(CompressionMission.RangedSiegeWeaponAmmoIndexCompressionInfo, ref bufferReadValid);
				return bufferReadValid;
			}
		}

		// Token: 0x02000621 RID: 1569
		public enum WeaponState
		{
			// Token: 0x0400208E RID: 8334
			Invalid = -1,
			// Token: 0x0400208F RID: 8335
			Idle,
			// Token: 0x04002090 RID: 8336
			WaitingBeforeProjectileLeaving,
			// Token: 0x04002091 RID: 8337
			Shooting,
			// Token: 0x04002092 RID: 8338
			WaitingAfterShooting,
			// Token: 0x04002093 RID: 8339
			WaitingBeforeReloading,
			// Token: 0x04002094 RID: 8340
			LoadingAmmo,
			// Token: 0x04002095 RID: 8341
			WaitingBeforeIdle,
			// Token: 0x04002096 RID: 8342
			Reloading,
			// Token: 0x04002097 RID: 8343
			ReloadingPaused,
			// Token: 0x04002098 RID: 8344
			NumberOfStates
		}

		// Token: 0x02000622 RID: 1570
		public enum FiringFocus
		{
			// Token: 0x0400209A RID: 8346
			Troops,
			// Token: 0x0400209B RID: 8347
			Walls,
			// Token: 0x0400209C RID: 8348
			RangedSiegeWeapons,
			// Token: 0x0400209D RID: 8349
			PrimarySiegeWeapons
		}

		// Token: 0x02000623 RID: 1571
		public enum CameraState
		{
			// Token: 0x0400209F RID: 8351
			StickToWeapon,
			// Token: 0x040020A0 RID: 8352
			DoNotMove,
			// Token: 0x040020A1 RID: 8353
			MoveDownToReload,
			// Token: 0x040020A2 RID: 8354
			RememberLastShotDirection,
			// Token: 0x040020A3 RID: 8355
			FreeMove,
			// Token: 0x040020A4 RID: 8356
			ApproachToCamera
		}

		// Token: 0x02000624 RID: 1572
		public enum ForceUseState
		{
			// Token: 0x040020A6 RID: 8358
			NotForced,
			// Token: 0x040020A7 RID: 8359
			ForcefullyUsed
		}

		// Token: 0x02000625 RID: 1573
		// (Invoke) Token: 0x06003FD8 RID: 16344
		public delegate void OnSiegeWeaponReloadDone();
	}
}
