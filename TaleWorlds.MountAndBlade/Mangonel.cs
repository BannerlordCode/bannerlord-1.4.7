using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Objects.Siege;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200034A RID: 842
	public class Mangonel : RangedSiegeWeapon, ISpawnable
	{
		// Token: 0x170008D6 RID: 2262
		// (get) Token: 0x06002F7A RID: 12154 RVA: 0x000BA465 File Offset: 0x000B8665
		protected override float MaximumBallisticError
		{
			get
			{
				return 1.5f;
			}
		}

		// Token: 0x170008D7 RID: 2263
		// (get) Token: 0x06002F7B RID: 12155 RVA: 0x000BA46C File Offset: 0x000B866C
		protected override float ShootingSpeed
		{
			get
			{
				return this.ProjectileSpeed;
			}
		}

		// Token: 0x06002F7C RID: 12156 RVA: 0x000BA474 File Offset: 0x000B8674
		protected override void RegisterAnimationParameters()
		{
			this.SkeletonOwnerObjects = new SynchedMissionObject[2];
			this.Skeletons = new Skeleton[2];
			this.SkeletonNames = new string[1];
			this.FireAnimations = new string[2];
			this.FireAnimationIndices = new int[2];
			this.SetUpAnimations = new string[2];
			this.SetUpAnimationIndices = new int[2];
			this.SkeletonOwnerObjects[0] = this._body;
			this.Skeletons[0] = this._body.GameEntity.Skeleton;
			this.SkeletonNames[0] = this.MangonelBodySkeleton;
			this.FireAnimations[0] = this.MangonelBodyFire;
			this.FireAnimationIndices[0] = MBAnimation.GetAnimationIndexWithName(this.MangonelBodyFire);
			this.SetUpAnimations[0] = this.MangonelBodyReload;
			this.SetUpAnimationIndices[0] = MBAnimation.GetAnimationIndexWithName(this.MangonelBodyReload);
			this.SkeletonOwnerObjects[1] = this._rope;
			this.Skeletons[1] = this._rope.GameEntity.Skeleton;
			this.FireAnimations[1] = this.MangonelRopeFire;
			this.FireAnimationIndices[1] = MBAnimation.GetAnimationIndexWithName(this.MangonelRopeFire);
			this.SetUpAnimations[1] = this.MangonelRopeReload;
			this.SetUpAnimationIndices[1] = MBAnimation.GetAnimationIndexWithName(this.MangonelRopeReload);
			this._missileBoneName = this.ProjectileBoneName;
			this._idleAnimationActionIndex = ActionIndexCache.Create(this.IdleActionName);
			this._shootAnimationActionIndex = ActionIndexCache.Create(this.ShootActionName);
			this._reload1AnimationActionIndex = ActionIndexCache.Create(this.Reload1ActionName);
			this._reload2AnimationActionIndex = ActionIndexCache.Create(this.Reload2ActionName);
			this._rotateLeftAnimationActionIndex = ActionIndexCache.Create(this.RotateLeftActionName);
			this._rotateRightAnimationActionIndex = ActionIndexCache.Create(this.RotateRightActionName);
			this._loadAmmoBeginAnimationActionIndex = ActionIndexCache.Create(this.LoadAmmoBeginActionName);
			this._loadAmmoEndAnimationActionIndex = ActionIndexCache.Create(this.LoadAmmoEndActionName);
			this._reload2IdleActionIndex = ActionIndexCache.Create(this.Reload2IdleActionName);
		}

		// Token: 0x06002F7D RID: 12157 RVA: 0x000BA65E File Offset: 0x000B885E
		public override UsableMachineAIBase CreateAIBehaviorObject()
		{
			return new MangonelAI(this);
		}

		// Token: 0x06002F7E RID: 12158 RVA: 0x000BA666 File Offset: 0x000B8866
		public override SiegeEngineType GetSiegeEngineType()
		{
			if (this.DefaultSide != BattleSideEnum.Attacker)
			{
				return DefaultSiegeEngineTypes.Catapult;
			}
			return DefaultSiegeEngineTypes.Onager;
		}

		// Token: 0x06002F7F RID: 12159 RVA: 0x000BA67C File Offset: 0x000B887C
		protected internal override void OnInit()
		{
			List<SynchedMissionObject> list = base.GameEntity.CollectScriptComponentsWithTagIncludingChildrenRecursive<SynchedMissionObject>("rope");
			if (list.Count > 0)
			{
				this._rope = list[0];
			}
			list = base.GameEntity.CollectScriptComponentsWithTagIncludingChildrenRecursive<SynchedMissionObject>("body");
			this._body = list[0];
			this._bodySkeleton = this._body.GameEntity.Skeleton;
			this.RotationObject = this._body;
			List<WeakGameEntity> list2 = base.GameEntity.CollectChildrenEntitiesWithTag("vertical_adjuster");
			this._verticalAdjuster = TaleWorlds.Engine.GameEntity.CreateFromWeakEntity(list2[0]);
			this._verticalAdjusterSkeleton = this._verticalAdjuster.Skeleton;
			if (this._verticalAdjusterSkeleton != null)
			{
				this._verticalAdjusterSkeleton.SetAnimationAtChannel(this.MangonelAimAnimation, 0, 1f, -1f, 0f);
			}
			this._verticalAdjusterStartingLocalFrame = this._verticalAdjuster.GetFrame();
			this._verticalAdjusterStartingLocalFrame = this._body.GameEntity.GetBoneEntitialFrameWithIndex(0).TransformToLocal(in this._verticalAdjusterStartingLocalFrame);
			base.OnInit();
			this.TimeGapBetweenShootActionAndProjectileLeaving = 0.23f;
			this.TimeGapBetweenShootingEndAndReloadingStart = 0f;
			this._rotateStandingPoints = new List<StandingPoint>();
			if (base.StandingPoints != null)
			{
				foreach (StandingPoint standingPoint in base.StandingPoints)
				{
					if (standingPoint.GameEntity.HasTag("rotate"))
					{
						if (standingPoint.GameEntity.HasTag("left") && this._rotateStandingPoints.Count > 0)
						{
							this._rotateStandingPoints.Insert(0, standingPoint);
						}
						else
						{
							this._rotateStandingPoints.Add(standingPoint);
						}
					}
				}
				MatrixFrame globalFrame = this._body.GameEntity.GetGlobalFrame();
				this._standingPointLocalIKFrames = new MatrixFrame[base.StandingPoints.Count];
				for (int i = 0; i < base.StandingPoints.Count; i++)
				{
					this._standingPointLocalIKFrames[i] = base.StandingPoints[i].GameEntity.GetGlobalFrame().TransformToLocalNonOrthogonal(in globalFrame);
					base.StandingPoints[i].AddComponent(new ClearHandInverseKinematicsOnStopUsageComponent());
				}
			}
			this._missileBoneIndex = Skeleton.GetBoneIndexFromName(this.Skeletons[0].GetName(), this._missileBoneName);
			this.ApplyAimChange();
			foreach (StandingPoint standingPoint2 in this.ReloadStandingPoints)
			{
				if (standingPoint2 != base.PilotStandingPoint)
				{
					this._reloadWithoutPilot = standingPoint2;
				}
			}
			if (!GameNetwork.IsClientOrReplay)
			{
				this.SetActivationLoadAmmoPoint(false);
			}
			this.EnemyRangeToStopUsing = 9f;
			base.SetScriptComponentToTick(this.GetTickRequirement());
			if (base.AmmoPickUpPoints != null)
			{
				foreach (StandingPoint standingPoint3 in base.AmmoPickUpPoints)
				{
					standingPoint3.LockUserFrames = true;
				}
			}
			this._isInitialProjectilePositionUpdated = false;
		}

		// Token: 0x06002F80 RID: 12160 RVA: 0x000BA9D4 File Offset: 0x000B8BD4
		protected internal override void OnEditorInit()
		{
		}

		// Token: 0x06002F81 RID: 12161 RVA: 0x000BA9D8 File Offset: 0x000B8BD8
		public override void OnPilotAssignedDuringSpawn()
		{
			base.PilotAgent.SetActionChannel(1, in this._idleAnimationActionIndex, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true);
			MatrixFrame globalFrame = base.PilotStandingPoint.GameEntity.GetGlobalFrame();
			base.PilotAgent.TeleportToPosition(globalFrame.origin);
			base.PilotAgent.DisableScriptedMovement();
			Agent pilotAgent = base.PilotAgent;
			Vec2 vec = globalFrame.rotation.f.AsVec2;
			vec = vec.Normalized();
			pilotAgent.SetMovementDirection(in vec);
		}

		// Token: 0x06002F82 RID: 12162 RVA: 0x000BAA73 File Offset: 0x000B8C73
		protected override bool CanRotate()
		{
			return base.State == RangedSiegeWeapon.WeaponState.Idle || base.State == RangedSiegeWeapon.WeaponState.LoadingAmmo || base.State == RangedSiegeWeapon.WeaponState.WaitingBeforeIdle;
		}

		// Token: 0x06002F83 RID: 12163 RVA: 0x000BAA94 File Offset: 0x000B8C94
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			if (base.GameEntity.IsVisibleIncludeParents())
			{
				return base.GetTickRequirement() | ScriptComponentBehavior.TickRequirement.Tick | ScriptComponentBehavior.TickRequirement.TickParallel;
			}
			return base.GetTickRequirement();
		}

		// Token: 0x06002F84 RID: 12164 RVA: 0x000BAAC4 File Offset: 0x000B8CC4
		protected internal override void OnTick(float dt)
		{
			base.OnTick(dt);
			if (!base.GameEntity.IsVisibleIncludeParents())
			{
				return;
			}
			if (!GameNetwork.IsClientOrReplay)
			{
				foreach (StandingPoint standingPoint in base.AmmoPickUpPoints)
				{
					if (standingPoint.HasUser)
					{
						Agent userAgent = standingPoint.UserAgent;
						ActionIndexCache currentAction = userAgent.GetCurrentAction(1);
						if (!(currentAction == ActionIndexCache.act_pickup_boulder_begin))
						{
							if (currentAction == ActionIndexCache.act_pickup_boulder_end)
							{
								MissionWeapon missionWeapon = new MissionWeapon(this.OriginalMissileItem, null, null, 1);
								userAgent.EquipWeaponToExtraSlotAndWield(ref missionWeapon);
								userAgent.StopUsingGameObject(true, Agent.StopUsingGameObjectFlags.None);
								this.ConsumeAmmo();
								if (userAgent.IsAIControlled)
								{
									if (!this.LoadAmmoStandingPoint.HasUser && !this.LoadAmmoStandingPoint.IsDeactivated)
									{
										userAgent.AIMoveToGameObjectEnable(this.LoadAmmoStandingPoint, this, base.Ai.GetScriptedFrameFlags(userAgent));
									}
									else if (this.ReloaderAgentOriginalPoint != null && !this.ReloaderAgentOriginalPoint.HasUser && !this.ReloaderAgentOriginalPoint.HasAIMovingTo)
									{
										userAgent.AIMoveToGameObjectEnable(this.ReloaderAgentOriginalPoint, this, base.Ai.GetScriptedFrameFlags(userAgent));
									}
									else
									{
										Agent reloaderAgent = this.ReloaderAgent;
										if (reloaderAgent != null)
										{
											Formation formation = reloaderAgent.Formation;
											if (formation != null)
											{
												formation.AttachUnit(this.ReloaderAgent);
											}
										}
										this.ReloaderAgent = null;
									}
								}
							}
							else if (!userAgent.SetActionChannel(1, in ActionIndexCache.act_pickup_boulder_begin, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true) && userAgent.Controller != AgentControllerType.AI)
							{
								userAgent.StopUsingGameObject(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
							}
						}
					}
				}
			}
			switch (base.State)
			{
			case RangedSiegeWeapon.WeaponState.LoadingAmmo:
				if (!GameNetwork.IsClientOrReplay)
				{
					if (this.LoadAmmoStandingPoint.HasUser)
					{
						Agent userAgent2 = this.LoadAmmoStandingPoint.UserAgent;
						if (userAgent2.GetCurrentAction(1) == this._loadAmmoEndAnimationActionIndex)
						{
							EquipmentIndex primaryWieldedItemIndex = userAgent2.GetPrimaryWieldedItemIndex();
							if (primaryWieldedItemIndex != EquipmentIndex.None && userAgent2.Equipment[primaryWieldedItemIndex].CurrentUsageItem.WeaponClass == this.OriginalMissileItem.PrimaryWeapon.WeaponClass)
							{
								base.ChangeProjectileEntityServer(userAgent2, userAgent2.Equipment[primaryWieldedItemIndex].Item.StringId);
								userAgent2.RemoveEquippedWeapon(primaryWieldedItemIndex);
								this._timeElapsedAfterLoading = 0f;
								base.Projectile.SetVisibleSynched(true, false);
								base.State = RangedSiegeWeapon.WeaponState.WaitingBeforeIdle;
								return;
							}
							userAgent2.StopUsingGameObject(true, Agent.StopUsingGameObjectFlags.None);
							if (!userAgent2.IsPlayerControlled)
							{
								base.SendAgentToAmmoPickup(userAgent2);
								return;
							}
						}
						else if (userAgent2.GetCurrentAction(1) != this._loadAmmoBeginAnimationActionIndex && !userAgent2.SetActionChannel(1, in this._loadAmmoBeginAnimationActionIndex, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true))
						{
							for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumAllWeaponSlots; equipmentIndex++)
							{
								if (!userAgent2.Equipment[equipmentIndex].IsEmpty && userAgent2.Equipment[equipmentIndex].CurrentUsageItem.WeaponClass == this.OriginalMissileItem.PrimaryWeapon.WeaponClass)
								{
									userAgent2.RemoveEquippedWeapon(equipmentIndex);
								}
							}
							userAgent2.StopUsingGameObject(true, Agent.StopUsingGameObjectFlags.None);
							if (!userAgent2.IsPlayerControlled)
							{
								base.SendAgentToAmmoPickup(userAgent2);
								return;
							}
						}
					}
					else if (this.LoadAmmoStandingPoint.HasAIMovingTo)
					{
						Agent movingAgent = this.LoadAmmoStandingPoint.MovingAgent;
						EquipmentIndex primaryWieldedItemIndex2 = movingAgent.GetPrimaryWieldedItemIndex();
						if (primaryWieldedItemIndex2 == EquipmentIndex.None || movingAgent.Equipment[primaryWieldedItemIndex2].CurrentUsageItem.WeaponClass != this.OriginalMissileItem.PrimaryWeapon.WeaponClass)
						{
							movingAgent.StopUsingGameObject(true, Agent.StopUsingGameObjectFlags.None);
							base.SendAgentToAmmoPickup(movingAgent);
						}
					}
				}
				break;
			case RangedSiegeWeapon.WeaponState.WaitingBeforeIdle:
				this._timeElapsedAfterLoading += dt;
				if (this._timeElapsedAfterLoading > 1f)
				{
					base.State = RangedSiegeWeapon.WeaponState.Idle;
					return;
				}
				break;
			case RangedSiegeWeapon.WeaponState.Reloading:
			case RangedSiegeWeapon.WeaponState.ReloadingPaused:
				break;
			default:
				return;
			}
		}

		// Token: 0x06002F85 RID: 12165 RVA: 0x000BAEF4 File Offset: 0x000B90F4
		protected internal override void OnTickParallel(float dt)
		{
			base.OnTickParallel(dt);
			if (!base.GameEntity.IsVisibleIncludeParents())
			{
				return;
			}
			if (!this._isInitialProjectilePositionUpdated)
			{
				this._isInitialProjectilePositionUpdated = true;
				this.UpdateProjectilePosition();
			}
			if (base.State == RangedSiegeWeapon.WeaponState.WaitingBeforeProjectileLeaving)
			{
				this.UpdateProjectilePosition();
			}
			if (this._verticalAdjusterSkeleton != null)
			{
				float num = MBMath.ClampFloat((this.CurrentReleaseAngle - this.BottomReleaseAngleRestriction) / (this.TopReleaseAngleRestriction - this.BottomReleaseAngleRestriction), 0f, 1f);
				this._verticalAdjusterSkeleton.SetAnimationParameterAtChannel(0, num);
			}
			MatrixFrame matrixFrame = this.Skeletons[0].GetBoneEntitialFrameWithIndex(0).TransformToParent(in this._verticalAdjusterStartingLocalFrame);
			this._verticalAdjuster.SetFrame(ref matrixFrame, true);
			MatrixFrame globalFrame = this._body.GameEntity.GetGlobalFrame();
			for (int i = 0; i < base.StandingPoints.Count; i++)
			{
				if (base.StandingPoints[i].HasUser)
				{
					if (base.StandingPoints[i].UserAgent.IsInBeingStruckAction || base.AmmoPickUpPoints.IndexOf(base.StandingPoints[i]) >= 0)
					{
						base.StandingPoints[i].UserAgent.ClearHandInverseKinematics();
					}
					else
					{
						ActionIndexCache currentAction = base.StandingPoints[i].UserAgent.GetCurrentAction(1);
						float currentActionProgress = base.StandingPoints[i].UserAgent.GetCurrentActionProgress(1);
						if (currentAction != this._reload2IdleActionIndex && (currentAction != this._reload2AnimationActionIndex || currentActionProgress > 0.1f) && (currentAction != this._shootAnimationActionIndex || currentActionProgress < 0.15f))
						{
							base.StandingPoints[i].UserAgent.SetHandInverseKinematicsFrameForMissionObjectUsage(in this._standingPointLocalIKFrames[i], in globalFrame, 0f);
						}
						else
						{
							base.StandingPoints[i].UserAgent.ClearHandInverseKinematics();
						}
					}
				}
			}
			if (!GameNetwork.IsClientOrReplay)
			{
				for (int j = 0; j < this._rotateStandingPoints.Count; j++)
				{
					StandingPoint standingPoint = this._rotateStandingPoints[j];
					if (standingPoint.HasUser)
					{
						Agent userAgent = standingPoint.UserAgent;
						int num2 = 1;
						ActionIndexCache actionIndexCache = ((j == 0) ? this._rotateLeftAnimationActionIndex : this._rotateRightAnimationActionIndex);
						if (!userAgent.SetActionChannel(num2, in actionIndexCache, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true) && standingPoint.UserAgent.Controller != AgentControllerType.AI)
						{
							standingPoint.UserAgent.StopUsingGameObjectMT(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
						}
					}
				}
				if (base.PilotAgent != null)
				{
					ActionIndexCache currentAction2 = base.PilotAgent.GetCurrentAction(1);
					if (base.State == RangedSiegeWeapon.WeaponState.WaitingBeforeProjectileLeaving)
					{
						if (base.PilotAgent.IsInBeingStruckAction)
						{
							if (currentAction2 != ActionIndexCache.act_none && currentAction2 != ActionIndexCache.act_strike_bent_over)
							{
								base.PilotAgent.SetActionChannel(1, in ActionIndexCache.act_strike_bent_over, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true);
							}
						}
						else if (!base.PilotAgent.SetActionChannel(1, in this._shootAnimationActionIndex, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true) && base.PilotAgent.Controller != AgentControllerType.AI)
						{
							base.PilotAgent.StopUsingGameObjectMT(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
						}
					}
					else if (!base.PilotAgent.SetActionChannel(1, in this._idleAnimationActionIndex, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true) && currentAction2 != this._reload1AnimationActionIndex && currentAction2 != this._shootAnimationActionIndex && base.PilotAgent.Controller != AgentControllerType.AI)
					{
						base.PilotAgent.StopUsingGameObjectMT(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
					}
				}
				if (this._reloadWithoutPilot.HasUser)
				{
					Agent userAgent2 = this._reloadWithoutPilot.UserAgent;
					if (!userAgent2.SetActionChannel(1, in this._reload2IdleActionIndex, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true) && userAgent2.GetCurrentAction(1) != this._reload2AnimationActionIndex && userAgent2.Controller != AgentControllerType.AI)
					{
						userAgent2.StopUsingGameObjectMT(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
					}
				}
			}
			RangedSiegeWeapon.WeaponState state = base.State;
			if (state == RangedSiegeWeapon.WeaponState.Reloading)
			{
				foreach (StandingPoint standingPoint2 in this.ReloadStandingPoints)
				{
					if (standingPoint2.HasUser)
					{
						ActionIndexCache currentAction3 = standingPoint2.UserAgent.GetCurrentAction(1);
						if (currentAction3 == this._reload1AnimationActionIndex || currentAction3 == this._reload2AnimationActionIndex)
						{
							standingPoint2.UserAgent.SetCurrentActionProgress(1, this._bodySkeleton.GetAnimationParameterAtChannel(0));
						}
						else if (!GameNetwork.IsClientOrReplay)
						{
							ActionIndexCache actionIndexCache2 = ((standingPoint2 == base.PilotStandingPoint) ? this._reload1AnimationActionIndex : this._reload2AnimationActionIndex);
							if (!standingPoint2.UserAgent.SetActionChannel(1, in actionIndexCache2, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, this._bodySkeleton.GetAnimationParameterAtChannel(0), false, -0.2f, 0, true) && standingPoint2.UserAgent.Controller != AgentControllerType.AI)
							{
								standingPoint2.UserAgent.StopUsingGameObjectMT(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
							}
						}
					}
				}
			}
		}

		// Token: 0x06002F86 RID: 12166 RVA: 0x000BB4A0 File Offset: 0x000B96A0
		protected override void SetActivationLoadAmmoPoint(bool activate)
		{
			this.LoadAmmoStandingPoint.SetIsDeactivatedSynched(!activate);
		}

		// Token: 0x06002F87 RID: 12167 RVA: 0x000BB4B4 File Offset: 0x000B96B4
		protected override void UpdateProjectilePosition()
		{
			MatrixFrame boneEntitialFrameWithIndex = this.Skeletons[0].GetBoneEntitialFrameWithIndex(this._missileBoneIndex);
			base.Projectile.GameEntity.SetFrame(ref boneEntitialFrameWithIndex, true);
		}

		// Token: 0x06002F88 RID: 12168 RVA: 0x000BB4EC File Offset: 0x000B96EC
		protected override void OnRangedSiegeWeaponStateChange()
		{
			base.OnRangedSiegeWeaponStateChange();
			RangedSiegeWeapon.WeaponState state = base.State;
			if (state != RangedSiegeWeapon.WeaponState.Idle)
			{
				if (state != RangedSiegeWeapon.WeaponState.Shooting)
				{
					if (state == RangedSiegeWeapon.WeaponState.WaitingBeforeIdle)
					{
						this.UpdateProjectilePosition();
						return;
					}
				}
				else
				{
					if (!GameNetwork.IsClientOrReplay)
					{
						base.Projectile.SetVisibleSynched(false, false);
						return;
					}
					base.Projectile.GameEntity.SetVisibilityExcludeParents(false);
					return;
				}
			}
			else
			{
				if (!GameNetwork.IsClientOrReplay)
				{
					base.Projectile.SetVisibleSynched(true, false);
					return;
				}
				base.Projectile.GameEntity.SetVisibilityExcludeParents(true);
			}
		}

		// Token: 0x06002F89 RID: 12169 RVA: 0x000BB56B File Offset: 0x000B976B
		protected override void GetSoundEventIndices()
		{
			this.MoveSoundIndex = SoundEvent.GetEventIdFromString("event:/mission/siege/mangonel/move");
			this.ReloadSoundIndex = SoundEvent.GetEventIdFromString("event:/mission/siege/mangonel/reload");
			this.FireSoundIndex = SoundEvent.GetEventIdFromString("event:/mission/siege/mangonel/fire");
		}

		// Token: 0x170008D8 RID: 2264
		// (get) Token: 0x06002F8A RID: 12170 RVA: 0x000BB5A0 File Offset: 0x000B97A0
		protected override float HorizontalAimSensitivity
		{
			get
			{
				if (this.DefaultSide == BattleSideEnum.Defender)
				{
					return 0.25f;
				}
				float num = 0.05f;
				foreach (StandingPoint standingPoint in this._rotateStandingPoints)
				{
					if (standingPoint.HasUser && !standingPoint.UserAgent.IsInBeingStruckAction)
					{
						num += 0.1f;
					}
				}
				return num;
			}
		}

		// Token: 0x170008D9 RID: 2265
		// (get) Token: 0x06002F8B RID: 12171 RVA: 0x000BB620 File Offset: 0x000B9820
		protected override float VerticalAimSensitivity
		{
			get
			{
				return 0.1f;
			}
		}

		// Token: 0x170008DA RID: 2266
		// (get) Token: 0x06002F8C RID: 12172 RVA: 0x000BB628 File Offset: 0x000B9828
		protected override Vec3 ShootingDirection
		{
			get
			{
				Mat3 rotation = this._body.GameEntity.GetGlobalFrame().rotation;
				rotation.RotateAboutSide(-this.CurrentReleaseAngle);
				Vec3 vec = new Vec3(0f, -1f, 0f, -1f);
				return rotation.TransformToParent(in vec);
			}
		}

		// Token: 0x170008DB RID: 2267
		// (get) Token: 0x06002F8D RID: 12173 RVA: 0x000BB67F File Offset: 0x000B987F
		// (set) Token: 0x06002F8E RID: 12174 RVA: 0x000BB6AB File Offset: 0x000B98AB
		protected override bool HasAmmo
		{
			get
			{
				return base.HasAmmo || base.CurrentlyUsedAmmoPickUpPoint != null || this.LoadAmmoStandingPoint.HasUser || this.LoadAmmoStandingPoint.HasAIMovingTo;
			}
			set
			{
				base.HasAmmo = value;
			}
		}

		// Token: 0x06002F8F RID: 12175 RVA: 0x000BB6B4 File Offset: 0x000B98B4
		protected override void ApplyAimChange()
		{
			base.ApplyAimChange();
			this.ShootingDirection.Normalize();
		}

		// Token: 0x06002F90 RID: 12176 RVA: 0x000BB6D6 File Offset: 0x000B98D6
		public override TextObject GetDescriptionText(WeakGameEntity gameEntity)
		{
			if (!gameEntity.HasTag(this.AmmoPickUpTag))
			{
				return new TextObject("{=NbpcDXtJ}Mangonel", null);
			}
			return new TextObject("{=pzfbPbWW}Boulder", null);
		}

		// Token: 0x06002F91 RID: 12177 RVA: 0x000BB700 File Offset: 0x000B9900
		public override TextObject GetActionTextForStandingPoint(UsableMissionObject usableGameObject)
		{
			TextObject textObject;
			if (usableGameObject.GameEntity.HasTag("reload"))
			{
				textObject = new TextObject((base.PilotStandingPoint == usableGameObject) ? "{=fEQAPJ2e}{KEY} Use" : "{=Na81xuXn}{KEY} Rearm", null);
			}
			else if (usableGameObject.GameEntity.HasTag("rotate"))
			{
				textObject = new TextObject("{=5wx4BF5h}{KEY} Rotate", null);
			}
			else if (usableGameObject.GameEntity.HasTag(this.AmmoPickUpTag))
			{
				textObject = new TextObject("{=bNYm3K6b}{KEY} Pick Up", null);
			}
			else if (usableGameObject.GameEntity.HasTag("ammoload"))
			{
				textObject = new TextObject("{=ibC4xPoo}{KEY} Load Ammo", null);
			}
			else
			{
				textObject = new TextObject("{=fEQAPJ2e}{KEY} Use", null);
			}
			textObject.SetTextVariable("KEY", HyperlinkTexts.GetKeyHyperlinkText(HotKeyManager.GetHotKeyId("CombatHotKeyCategory", 13), 1f));
			return textObject;
		}

		// Token: 0x06002F92 RID: 12178 RVA: 0x000BB7DC File Offset: 0x000B99DC
		public override TargetFlags GetTargetFlags()
		{
			TargetFlags targetFlags = TargetFlags.None;
			targetFlags |= TargetFlags.IsFlammable;
			targetFlags |= TargetFlags.IsSiegeEngine;
			if (this.Side == BattleSideEnum.Attacker)
			{
				targetFlags |= TargetFlags.IsAttacker;
			}
			if (base.IsDestroyed || this.IsDeactivated)
			{
				targetFlags |= TargetFlags.NotAThreat;
			}
			if (this.Side == BattleSideEnum.Attacker && DebugSiegeBehavior.DebugDefendState == DebugSiegeBehavior.DebugStateDefender.DebugDefendersToMangonels)
			{
				targetFlags |= TargetFlags.DebugThreat;
			}
			if (this.Side == BattleSideEnum.Defender && DebugSiegeBehavior.DebugAttackState == DebugSiegeBehavior.DebugStateAttacker.DebugAttackersToMangonels)
			{
				targetFlags |= TargetFlags.DebugThreat;
			}
			return targetFlags;
		}

		// Token: 0x06002F93 RID: 12179 RVA: 0x000BB848 File Offset: 0x000B9A48
		public override float GetTargetValue(List<Vec3> weaponPos)
		{
			return 40f * base.GetUserMultiplierOfWeapon() * this.GetDistanceMultiplierOfWeapon(weaponPos[0]) * base.GetHitPointMultiplierOfWeapon();
		}

		// Token: 0x06002F94 RID: 12180 RVA: 0x000BB86C File Offset: 0x000B9A6C
		public override float ProcessTargetValue(float baseValue, TargetFlags flags)
		{
			if (flags.HasAnyFlag(TargetFlags.NotAThreat))
			{
				return -1000f;
			}
			if (flags.HasAnyFlag(TargetFlags.IsSiegeEngine))
			{
				baseValue *= 10000f;
			}
			if (flags.HasAnyFlag(TargetFlags.IsStructure))
			{
				baseValue *= 2.5f;
			}
			if (flags.HasAnyFlag(TargetFlags.IsSmall))
			{
				baseValue *= 8f;
			}
			if (flags.HasAnyFlag(TargetFlags.IsMoving))
			{
				baseValue *= 8f;
			}
			if (flags.HasAnyFlag(TargetFlags.DebugThreat))
			{
				baseValue *= 10000f;
			}
			if (flags.HasAnyFlag(TargetFlags.IsSiegeTower))
			{
				baseValue *= 8f;
			}
			return baseValue;
		}

		// Token: 0x06002F95 RID: 12181 RVA: 0x000BB8FF File Offset: 0x000B9AFF
		protected override float GetDetachmentWeightAux(BattleSideEnum side)
		{
			return base.GetDetachmentWeightAuxForExternalAmmoWeapons(side);
		}

		// Token: 0x06002F96 RID: 12182 RVA: 0x000BB908 File Offset: 0x000B9B08
		public void SetSpawnedFromSpawner()
		{
			this._spawnedFromSpawner = true;
		}

		// Token: 0x0400134A RID: 4938
		private const string BodyTag = "body";

		// Token: 0x0400134B RID: 4939
		private const string RopeTag = "rope";

		// Token: 0x0400134C RID: 4940
		private const string RotateTag = "rotate";

		// Token: 0x0400134D RID: 4941
		private const string LeftTag = "left";

		// Token: 0x0400134E RID: 4942
		private const string VerticalAdjusterTag = "vertical_adjuster";

		// Token: 0x0400134F RID: 4943
		private string _missileBoneName = "end_throwarm";

		// Token: 0x04001350 RID: 4944
		private List<StandingPoint> _rotateStandingPoints;

		// Token: 0x04001351 RID: 4945
		private SynchedMissionObject _body;

		// Token: 0x04001352 RID: 4946
		private SynchedMissionObject _rope;

		// Token: 0x04001353 RID: 4947
		private GameEntity _verticalAdjuster;

		// Token: 0x04001354 RID: 4948
		private MatrixFrame _verticalAdjusterStartingLocalFrame;

		// Token: 0x04001355 RID: 4949
		private Skeleton _verticalAdjusterSkeleton;

		// Token: 0x04001356 RID: 4950
		private Skeleton _bodySkeleton;

		// Token: 0x04001357 RID: 4951
		private float _timeElapsedAfterLoading;

		// Token: 0x04001358 RID: 4952
		private bool _isInitialProjectilePositionUpdated;

		// Token: 0x04001359 RID: 4953
		private MatrixFrame[] _standingPointLocalIKFrames;

		// Token: 0x0400135A RID: 4954
		private StandingPoint _reloadWithoutPilot;

		// Token: 0x0400135B RID: 4955
		public string MangonelBodySkeleton = "mangonel_skeleton";

		// Token: 0x0400135C RID: 4956
		public string MangonelBodyFire = "mangonel_fire";

		// Token: 0x0400135D RID: 4957
		public string MangonelBodyReload = "mangonel_set_up";

		// Token: 0x0400135E RID: 4958
		public string MangonelRopeFire = "mangonel_holder_fire";

		// Token: 0x0400135F RID: 4959
		public string MangonelRopeReload = "mangonel_holder_set_up";

		// Token: 0x04001360 RID: 4960
		public string MangonelAimAnimation = "mangonel_a_anglearm_state";

		// Token: 0x04001361 RID: 4961
		public string ProjectileBoneName = "end_throwarm";

		// Token: 0x04001362 RID: 4962
		public string IdleActionName;

		// Token: 0x04001363 RID: 4963
		public string ShootActionName;

		// Token: 0x04001364 RID: 4964
		public string Reload1ActionName;

		// Token: 0x04001365 RID: 4965
		public string Reload2ActionName;

		// Token: 0x04001366 RID: 4966
		public string RotateLeftActionName;

		// Token: 0x04001367 RID: 4967
		public string RotateRightActionName;

		// Token: 0x04001368 RID: 4968
		public string LoadAmmoBeginActionName;

		// Token: 0x04001369 RID: 4969
		public string LoadAmmoEndActionName;

		// Token: 0x0400136A RID: 4970
		public string Reload2IdleActionName;

		// Token: 0x0400136B RID: 4971
		public float ProjectileSpeed = 40f;

		// Token: 0x0400136C RID: 4972
		private ActionIndexCache _idleAnimationActionIndex;

		// Token: 0x0400136D RID: 4973
		private ActionIndexCache _shootAnimationActionIndex;

		// Token: 0x0400136E RID: 4974
		private ActionIndexCache _reload1AnimationActionIndex;

		// Token: 0x0400136F RID: 4975
		private ActionIndexCache _reload2AnimationActionIndex;

		// Token: 0x04001370 RID: 4976
		private ActionIndexCache _rotateLeftAnimationActionIndex;

		// Token: 0x04001371 RID: 4977
		private ActionIndexCache _rotateRightAnimationActionIndex;

		// Token: 0x04001372 RID: 4978
		private ActionIndexCache _loadAmmoBeginAnimationActionIndex;

		// Token: 0x04001373 RID: 4979
		private ActionIndexCache _loadAmmoEndAnimationActionIndex;

		// Token: 0x04001374 RID: 4980
		private ActionIndexCache _reload2IdleActionIndex;

		// Token: 0x04001375 RID: 4981
		private sbyte _missileBoneIndex;
	}
}
