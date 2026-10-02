using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Objects.Siege;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200035E RID: 862
	public class Trebuchet : RangedSiegeWeapon, ISpawnable
	{
		// Token: 0x17000932 RID: 2354
		// (get) Token: 0x06003150 RID: 12624 RVA: 0x000C8382 File Offset: 0x000C6582
		public override float DirectionRestriction
		{
			get
			{
				return 1.3962635f;
			}
		}

		// Token: 0x06003151 RID: 12625 RVA: 0x000C838C File Offset: 0x000C658C
		public override TextObject GetActionTextForStandingPoint(UsableMissionObject usableGameObject)
		{
			TextObject textObject;
			if (usableGameObject.GameEntity.HasTag(this.AmmoPickUpTag))
			{
				textObject = new TextObject("{=bNYm3K6b}{KEY} Pick Up", null);
			}
			else if (usableGameObject.GameEntity.HasTag("reload"))
			{
				textObject = new TextObject((base.PilotStandingPoint == usableGameObject) ? "{=fEQAPJ2e}{KEY} Use" : "{=Na81xuXn}{KEY} Rearm", null);
			}
			else if (usableGameObject.GameEntity.HasTag("rotate"))
			{
				textObject = new TextObject("{=5wx4BF5h}{KEY} Rotate", null);
			}
			else if (usableGameObject.GameEntity.HasTag("ammoload"))
			{
				textObject = new TextObject("{=ibC4xPoo}{KEY} Load Ammo", null);
			}
			else
			{
				textObject = TextObject.GetEmpty();
			}
			textObject.SetTextVariable("KEY", HyperlinkTexts.GetKeyHyperlinkText(HotKeyManager.GetHotKeyId("CombatHotKeyCategory", 13), 1f));
			return textObject;
		}

		// Token: 0x06003152 RID: 12626 RVA: 0x000C845F File Offset: 0x000C665F
		public override TextObject GetDescriptionText(WeakGameEntity gameEntity)
		{
			if (!gameEntity.HasTag(this.AmmoPickUpTag))
			{
				return new TextObject("{=4Skg9QhO}Trebuchet", null);
			}
			return new TextObject("{=pzfbPbWW}Boulder", null);
		}

		// Token: 0x06003153 RID: 12627 RVA: 0x000C8488 File Offset: 0x000C6688
		protected override void RegisterAnimationParameters()
		{
			this.SkeletonOwnerObjects = new SynchedMissionObject[3];
			this.Skeletons = new Skeleton[3];
			this.SkeletonNames = new string[3];
			this.FireAnimations = new string[3];
			this.FireAnimationIndices = new int[3];
			this.SetUpAnimations = new string[3];
			this.SetUpAnimationIndices = new int[3];
			this.SkeletonOwnerObjects[0] = this._body;
			this.Skeletons[0] = this._body.GameEntity.Skeleton;
			this.SkeletonNames[0] = "trebuchet_a_skeleton";
			this.FireAnimations[0] = this.BodyFireAnimation;
			this.FireAnimationIndices[0] = MBAnimation.GetAnimationIndexWithName(this.BodyFireAnimation);
			this.SetUpAnimations[0] = this.BodySetUpAnimation;
			this.SetUpAnimationIndices[0] = MBAnimation.GetAnimationIndexWithName(this.BodySetUpAnimation);
			this.SkeletonOwnerObjects[1] = this._sling;
			this.Skeletons[1] = this._sling.GameEntity.Skeleton;
			this.SkeletonNames[1] = "trebuchet_a_sling_skeleton";
			this.FireAnimations[1] = this.SlingFireAnimation;
			this.FireAnimationIndices[1] = MBAnimation.GetAnimationIndexWithName(this.SlingFireAnimation);
			this.SetUpAnimations[1] = this.SlingSetUpAnimation;
			this.SetUpAnimationIndices[1] = MBAnimation.GetAnimationIndexWithName(this.SlingSetUpAnimation);
			this.SkeletonOwnerObjects[2] = this._rope;
			this.Skeletons[2] = this._rope.GameEntity.Skeleton;
			this.SkeletonNames[2] = "trebuchet_a_rope_skeleton";
			this.FireAnimations[2] = this.RopeFireAnimation;
			this.FireAnimationIndices[2] = MBAnimation.GetAnimationIndexWithName(this.RopeFireAnimation);
			this.SetUpAnimations[2] = this.RopeSetUpAnimation;
			this.SetUpAnimationIndices[2] = MBAnimation.GetAnimationIndexWithName(this.RopeSetUpAnimation);
		}

		// Token: 0x06003154 RID: 12628 RVA: 0x000C8651 File Offset: 0x000C6851
		public override SiegeEngineType GetSiegeEngineType()
		{
			return DefaultSiegeEngineTypes.Trebuchet;
		}

		// Token: 0x06003155 RID: 12629 RVA: 0x000C8658 File Offset: 0x000C6858
		protected override void GetSoundEventIndices()
		{
			this.MoveSoundIndex = SoundEvent.GetEventIdFromString("event:/mission/siege/trebuchet/move");
			this.ReloadSoundIndex = SoundEvent.GetEventIdFromString("event:/mission/siege/trebuchet/reload");
			this.FireSoundIndex = SoundEvent.GetEventIdFromString("event:/mission/siege/trebuchet/fire");
		}

		// Token: 0x17000933 RID: 2355
		// (get) Token: 0x06003156 RID: 12630 RVA: 0x000C868A File Offset: 0x000C688A
		protected override float ShootingSpeed
		{
			get
			{
				return this.ProjectileSpeed;
			}
		}

		// Token: 0x06003157 RID: 12631 RVA: 0x000C8692 File Offset: 0x000C6892
		public override UsableMachineAIBase CreateAIBehaviorObject()
		{
			return new TrebuchetAI(this);
		}

		// Token: 0x06003158 RID: 12632 RVA: 0x000C869C File Offset: 0x000C689C
		protected internal override void OnInit()
		{
			List<SynchedMissionObject> list = base.GameEntity.CollectScriptComponentsWithTagIncludingChildrenRecursive<SynchedMissionObject>("body");
			this._body = list[0];
			list = base.GameEntity.CollectScriptComponentsWithTagIncludingChildrenRecursive<SynchedMissionObject>("sling");
			this._sling = list[0];
			list = base.GameEntity.CollectScriptComponentsWithTagIncludingChildrenRecursive<SynchedMissionObject>("rope");
			this._rope = list[0];
			List<WeakGameEntity> list2 = base.GameEntity.CollectChildrenEntitiesWithTag("vertical_adjuster");
			this._verticalAdjuster = TaleWorlds.Engine.GameEntity.CreateFromWeakEntity(list2[0]);
			this._verticalAdjusterSkeleton = this._verticalAdjuster.Skeleton;
			this._verticalAdjusterSkeleton.SetAnimationAtChannel(this.VerticalAdjusterAnimation, 0, 1f, -1f, 0f);
			this._verticalAdjusterStartingLocalFrame = this._verticalAdjuster.GetFrame();
			this._verticalAdjusterStartingLocalFrame = this._body.GameEntity.GetBoneEntitialFrameWithIndex(0).TransformToLocal(in this._verticalAdjusterStartingLocalFrame);
			list = base.GameEntity.CollectScriptComponentsWithTagIncludingChildrenRecursive<SynchedMissionObject>("rotate_entity");
			this.RotationObject = list[0];
			base.OnInit();
			this.TimeGapBetweenShootActionAndProjectileLeaving = 1.6f;
			this.TimeGapBetweenShootingEndAndReloadingStart = 0f;
			this._ammoLoadPoints = new List<StandingPointWithWeaponRequirement>();
			if (base.StandingPoints != null)
			{
				for (int i = 0; i < base.StandingPoints.Count; i++)
				{
					if (base.StandingPoints[i].GameEntity.HasTag("ammoload"))
					{
						this._ammoLoadPoints.Add(base.StandingPoints[i] as StandingPointWithWeaponRequirement);
					}
					else if (base.StandingPoints[i] != base.PilotStandingPoint && !base.StandingPoints[i].GameEntity.HasTag(this.AmmoPickUpTag) && !GameNetwork.IsClientOrReplay)
					{
						base.StandingPoints[i].SetIsDisabledForPlayersSynched(true);
					}
				}
				MatrixFrame globalFrame = this._body.GameEntity.GetGlobalFrame();
				this._standingPointLocalIKFrames = new MatrixFrame[base.StandingPoints.Count];
				for (int j = 0; j < base.StandingPoints.Count; j++)
				{
					this._standingPointLocalIKFrames[j] = base.StandingPoints[j].GameEntity.GetGlobalFrame().TransformToLocal(in globalFrame);
					base.StandingPoints[j].AddComponent(new ClearHandInverseKinematicsOnStopUsageComponent());
				}
			}
			this.ApplyAimChange();
			if (!GameNetwork.IsClientOrReplay)
			{
				this.SetActivationLoadAmmoPoint(false);
				this.EnemyRangeToStopUsing = 11f;
				this.MachinePositionOffsetToStopUsingLocal = new Vec2(0f, 2.8f);
				this._sling.SetAnimationAtChannelSynched((base.State == RangedSiegeWeapon.WeaponState.Idle) ? this.IdleWithAmmoAnimation : this.IdleEmptyAnimation, 0, 1f);
			}
			this._missileBoneIndex = Skeleton.GetBoneIndexFromName(this._sling.GameEntity.Skeleton.GetName(), "bn_projectile_holder");
			this._shootAnimPlayed = false;
			this.UpdateAmmoMesh();
			base.SetScriptComponentToTick(this.GetTickRequirement());
			this.UpdateProjectilePosition();
		}

		// Token: 0x06003159 RID: 12633 RVA: 0x000C89C8 File Offset: 0x000C6BC8
		public override void AfterMissionStart()
		{
			if (base.AmmoPickUpPoints != null)
			{
				foreach (StandingPoint standingPoint in base.AmmoPickUpPoints)
				{
					standingPoint.LockUserFrames = true;
				}
			}
			if (this._ammoLoadPoints != null)
			{
				foreach (StandingPointWithWeaponRequirement standingPointWithWeaponRequirement in this._ammoLoadPoints)
				{
					standingPointWithWeaponRequirement.LockUserFrames = true;
				}
			}
		}

		// Token: 0x0600315A RID: 12634 RVA: 0x000C8A6C File Offset: 0x000C6C6C
		protected override void OnRangedSiegeWeaponStateChange()
		{
			base.OnRangedSiegeWeaponStateChange();
			if (base.State == RangedSiegeWeapon.WeaponState.WaitingBeforeIdle)
			{
				this.UpdateProjectilePosition();
			}
			if (GameNetwork.IsClientOrReplay)
			{
				return;
			}
			RangedSiegeWeapon.WeaponState state = base.State;
			if (state <= RangedSiegeWeapon.WeaponState.Shooting)
			{
				if (state == RangedSiegeWeapon.WeaponState.Idle)
				{
					base.Projectile.SetVisibleSynched(true, false);
					return;
				}
				if (state != RangedSiegeWeapon.WeaponState.Shooting)
				{
					return;
				}
				base.Projectile.SetVisibleSynched(false, false);
				return;
			}
			else
			{
				if (state == RangedSiegeWeapon.WeaponState.LoadingAmmo)
				{
					this._sling.SetAnimationAtChannelSynched(this.IdleEmptyAnimation, 0, 1f);
					return;
				}
				if (state != RangedSiegeWeapon.WeaponState.Reloading)
				{
					return;
				}
				this._shootAnimPlayed = false;
				return;
			}
		}

		// Token: 0x17000934 RID: 2356
		// (get) Token: 0x0600315B RID: 12635 RVA: 0x000C8AED File Offset: 0x000C6CED
		protected override float HorizontalAimSensitivity
		{
			get
			{
				return 0.1f;
			}
		}

		// Token: 0x17000935 RID: 2357
		// (get) Token: 0x0600315C RID: 12636 RVA: 0x000C8AF4 File Offset: 0x000C6CF4
		protected override float VerticalAimSensitivity
		{
			get
			{
				return 0.075f;
			}
		}

		// Token: 0x17000936 RID: 2358
		// (get) Token: 0x0600315D RID: 12637 RVA: 0x000C8AFC File Offset: 0x000C6CFC
		protected override Vec3 ShootingDirection
		{
			get
			{
				Mat3 rotation = this.RotationObject.GameEntity.GetGlobalFrame().rotation;
				rotation.RotateAboutSide(-this.CurrentReleaseAngle);
				Vec3 vec = new Vec3(0f, -1f, 0f, -1f);
				return rotation.TransformToParent(in vec);
			}
		}

		// Token: 0x17000937 RID: 2359
		// (get) Token: 0x0600315E RID: 12638 RVA: 0x000C8B53 File Offset: 0x000C6D53
		// (set) Token: 0x0600315F RID: 12639 RVA: 0x000C8B7F File Offset: 0x000C6D7F
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

		// Token: 0x06003160 RID: 12640 RVA: 0x000C8B88 File Offset: 0x000C6D88
		public override float ProcessTargetValue(float baseValue, TargetFlags flags)
		{
			if (flags.HasAnyFlag(TargetFlags.NotAThreat))
			{
				return -1000f;
			}
			if (flags.HasAllFlags(TargetFlags.IsSiegeEngine | TargetFlags.IsAttacker))
			{
				baseValue *= 1.5f;
			}
			if (flags.HasAnyFlag(TargetFlags.IsSiegeEngine))
			{
				baseValue *= 2.5f;
			}
			if (flags.HasAnyFlag(TargetFlags.IsStructure))
			{
				baseValue *= 0.1f;
			}
			if (flags.HasAnyFlag(TargetFlags.DebugThreat))
			{
				baseValue *= 10000f;
			}
			return baseValue;
		}

		// Token: 0x06003161 RID: 12641 RVA: 0x000C8BF4 File Offset: 0x000C6DF4
		public override TargetFlags GetTargetFlags()
		{
			TargetFlags targetFlags = TargetFlags.None;
			targetFlags |= TargetFlags.IsFlammable;
			targetFlags |= TargetFlags.IsSiegeEngine;
			targetFlags |= TargetFlags.IsAttacker;
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

		// Token: 0x06003162 RID: 12642 RVA: 0x000C8C57 File Offset: 0x000C6E57
		public override float GetTargetValue(List<Vec3> weaponPos)
		{
			return 40f * base.GetUserMultiplierOfWeapon() * this.GetDistanceMultiplierOfWeapon(weaponPos[0]) * base.GetHitPointMultiplierOfWeapon();
		}

		// Token: 0x06003163 RID: 12643 RVA: 0x000C8C7A File Offset: 0x000C6E7A
		protected override bool CanRotate()
		{
			return base.State == RangedSiegeWeapon.WeaponState.Idle || base.State == RangedSiegeWeapon.WeaponState.LoadingAmmo || base.State == RangedSiegeWeapon.WeaponState.WaitingBeforeIdle;
		}

		// Token: 0x06003164 RID: 12644 RVA: 0x000C8C98 File Offset: 0x000C6E98
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			if (base.GameEntity.IsVisibleIncludeParents())
			{
				return base.GetTickRequirement() | ScriptComponentBehavior.TickRequirement.Tick | ScriptComponentBehavior.TickRequirement.TickParallel;
			}
			return base.GetTickRequirement();
		}

		// Token: 0x06003165 RID: 12645 RVA: 0x000C8CC8 File Offset: 0x000C6EC8
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
								MissionWeapon missionWeapon = new MissionWeapon(this.OriginalMissileItem, null, null);
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
					bool flag = false;
					foreach (StandingPointWithWeaponRequirement standingPointWithWeaponRequirement in this._ammoLoadPoints)
					{
						if (flag)
						{
							if (standingPointWithWeaponRequirement.IsDeactivated)
							{
								if ((standingPointWithWeaponRequirement.HasUser || standingPointWithWeaponRequirement.HasAIMovingTo) && (standingPointWithWeaponRequirement.UserAgent == this.ReloaderAgent || standingPointWithWeaponRequirement.MovingAgent == this.ReloaderAgent))
								{
									base.SendReloaderAgentToOriginalPoint();
								}
								standingPointWithWeaponRequirement.SetIsDeactivatedSynched(true);
							}
						}
						else if (standingPointWithWeaponRequirement.HasUser)
						{
							flag = true;
							Agent userAgent2 = standingPointWithWeaponRequirement.UserAgent;
							ActionIndexCache currentAction2 = userAgent2.GetCurrentAction(1);
							if (currentAction2 == ActionIndexCache.act_usage_trebuchet_load_ammo && userAgent2.GetCurrentActionProgress(1) > 0.56f)
							{
								EquipmentIndex primaryWieldedItemIndex = userAgent2.GetPrimaryWieldedItemIndex();
								if (primaryWieldedItemIndex != EquipmentIndex.None && userAgent2.Equipment[primaryWieldedItemIndex].CurrentUsageItem.WeaponClass == this.OriginalMissileItem.PrimaryWeapon.WeaponClass)
								{
									base.ChangeProjectileEntityServer(userAgent2, userAgent2.Equipment[primaryWieldedItemIndex].Item.StringId);
									userAgent2.RemoveEquippedWeapon(primaryWieldedItemIndex);
									this._timeElapsedAfterLoading = 0f;
									base.Projectile.SetVisibleSynched(true, false);
									this._sling.SetAnimationAtChannelSynched(this.IdleWithAmmoAnimation, 0, 1f);
									base.State = RangedSiegeWeapon.WeaponState.WaitingBeforeIdle;
								}
								else
								{
									userAgent2.StopUsingGameObject(true, Agent.StopUsingGameObjectFlags.None);
									if (!userAgent2.IsPlayerControlled)
									{
										base.SendAgentToAmmoPickup(userAgent2);
									}
								}
							}
							else if (currentAction2 != ActionIndexCache.act_usage_trebuchet_load_ammo && !userAgent2.SetActionChannel(1, in ActionIndexCache.act_usage_trebuchet_load_ammo, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true))
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
								}
							}
						}
						else if (standingPointWithWeaponRequirement.HasAIMovingTo)
						{
							Agent movingAgent = standingPointWithWeaponRequirement.MovingAgent;
							EquipmentIndex primaryWieldedItemIndex2 = movingAgent.GetPrimaryWieldedItemIndex();
							if (primaryWieldedItemIndex2 == EquipmentIndex.None || movingAgent.Equipment[primaryWieldedItemIndex2].CurrentUsageItem.WeaponClass != this.OriginalMissileItem.PrimaryWeapon.WeaponClass)
							{
								movingAgent.StopUsingGameObject(true, Agent.StopUsingGameObjectFlags.None);
								base.SendAgentToAmmoPickup(movingAgent);
							}
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

		// Token: 0x06003166 RID: 12646 RVA: 0x000C91C0 File Offset: 0x000C73C0
		protected internal override void OnTickParallel(float dt)
		{
			base.OnTickParallel(dt);
			if (!base.GameEntity.IsVisibleIncludeParents())
			{
				return;
			}
			if (base.State == RangedSiegeWeapon.WeaponState.WaitingBeforeProjectileLeaving)
			{
				this.UpdateProjectilePosition();
			}
			float num = MBMath.ClampFloat((this.CurrentReleaseAngle - this.BottomReleaseAngleRestriction) / (this.TopReleaseAngleRestriction - this.BottomReleaseAngleRestriction), 0f, 1f);
			this._verticalAdjusterSkeleton.SetAnimationParameterAtChannel(0, num);
			MatrixFrame matrixFrame = this._body.GameEntity.GetBoneEntitialFrameWithIndex(0).TransformToParent(in this._verticalAdjusterStartingLocalFrame);
			this._verticalAdjuster.SetFrame(ref matrixFrame, true);
			MatrixFrame globalFrame = this._body.GameEntity.GetGlobalFrame();
			for (int i = 0; i < base.StandingPoints.Count; i++)
			{
				if (base.StandingPoints[i].HasUser)
				{
					if (base.StandingPoints[i].UserAgent.IsInBeingStruckAction)
					{
						base.StandingPoints[i].UserAgent.ClearHandInverseKinematics();
					}
					else if (base.StandingPoints[i] != base.PilotStandingPoint)
					{
						if (base.StandingPoints[i].UserAgent.GetCurrentAction(1) == ActionIndexCache.act_usage_trebuchet_reload_2)
						{
							base.StandingPoints[i].UserAgent.SetHandInverseKinematicsFrameForMissionObjectUsage(in this._standingPointLocalIKFrames[i], in globalFrame, 0f);
						}
						else
						{
							base.StandingPoints[i].UserAgent.ClearHandInverseKinematics();
						}
					}
					else
					{
						base.StandingPoints[i].UserAgent.SetHandInverseKinematicsFrameForMissionObjectUsage(in this._standingPointLocalIKFrames[i], in globalFrame, 0f);
					}
				}
			}
			if (!GameNetwork.IsClientOrReplay)
			{
				if (base.PilotAgent != null)
				{
					ActionIndexCache currentAction = base.PilotAgent.GetCurrentAction(1);
					if (base.State == RangedSiegeWeapon.WeaponState.WaitingBeforeProjectileLeaving || base.State == RangedSiegeWeapon.WeaponState.Shooting || base.State == RangedSiegeWeapon.WeaponState.WaitingBeforeReloading)
					{
						if (!this._shootAnimPlayed && currentAction != ActionIndexCache.act_usage_trebuchet_shoot)
						{
							this._shootAnimPlayed = base.PilotAgent.SetActionChannel(1, in ActionIndexCache.act_usage_trebuchet_shoot, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true);
						}
						else if (currentAction != ActionIndexCache.act_usage_trebuchet_shoot && !base.PilotAgent.SetActionChannel(1, in ActionIndexCache.act_usage_trebuchet_reload_idle, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true) && base.PilotAgent.Controller != AgentControllerType.AI)
						{
							base.PilotAgent.StopUsingGameObjectMT(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
						}
					}
					else if (currentAction != ActionIndexCache.act_usage_trebuchet_reload && currentAction != ActionIndexCache.act_usage_trebuchet_shoot && !base.PilotAgent.SetActionChannel(1, in ActionIndexCache.act_usage_trebuchet_idle, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true) && base.PilotAgent.Controller != AgentControllerType.AI)
					{
						base.PilotAgent.StopUsingGameObjectMT(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
					}
				}
				if (base.State != RangedSiegeWeapon.WeaponState.Reloading)
				{
					foreach (StandingPoint standingPoint in this.ReloadStandingPoints)
					{
						if (standingPoint.HasUser && standingPoint != base.PilotStandingPoint)
						{
							Agent userAgent = standingPoint.UserAgent;
							if (!userAgent.SetActionChannel(1, in ActionIndexCache.act_usage_trebuchet_reload_2_idle, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true) && userAgent.Controller != AgentControllerType.AI)
							{
								userAgent.StopUsingGameObjectMT(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
							}
						}
					}
				}
				foreach (StandingPoint standingPoint2 in base.StandingPoints)
				{
					if (standingPoint2.HasUser && this.ReloadStandingPoints.IndexOf(standingPoint2) < 0 && (!(standingPoint2 is StandingPointWithWeaponRequirement) || (this._ammoLoadPoints.IndexOf((StandingPointWithWeaponRequirement)standingPoint2) < 0 && base.AmmoPickUpPoints.IndexOf(standingPoint2) < 0)))
					{
						Agent userAgent2 = standingPoint2.UserAgent;
						if (!userAgent2.SetActionChannel(1, in ActionIndexCache.act_usage_trebuchet_reload_2_idle, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true) && userAgent2.Controller != AgentControllerType.AI)
						{
							userAgent2.StopUsingGameObjectMT(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
						}
					}
				}
			}
			RangedSiegeWeapon.WeaponState state = base.State;
			if (state == RangedSiegeWeapon.WeaponState.Reloading)
			{
				for (int j = 0; j < this.ReloadStandingPoints.Count; j++)
				{
					if (this.ReloadStandingPoints[j].HasUser)
					{
						Agent userAgent3 = this.ReloadStandingPoints[j].UserAgent;
						ActionIndexCache currentAction2 = userAgent3.GetCurrentAction(1);
						if (currentAction2 == ActionIndexCache.act_usage_trebuchet_reload || currentAction2 == ActionIndexCache.act_usage_trebuchet_reload_2)
						{
							userAgent3.SetCurrentActionProgress(1, this.Skeletons[0].GetAnimationParameterAtChannel(0));
						}
						else if (!GameNetwork.IsClientOrReplay)
						{
							ActionIndexCache actionIndexCache = ActionIndexCache.act_usage_trebuchet_reload;
							if (this.ReloadStandingPoints[j].GameEntity.HasTag("right"))
							{
								actionIndexCache = ActionIndexCache.act_usage_trebuchet_reload_2;
							}
							if (!userAgent3.SetActionChannel(1, in actionIndexCache, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, this.Skeletons[0].GetAnimationParameterAtChannel(0), false, -0.2f, 0, true) && userAgent3.Controller != AgentControllerType.AI)
							{
								userAgent3.StopUsingGameObjectMT(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
							}
						}
					}
				}
			}
		}

		// Token: 0x06003167 RID: 12647 RVA: 0x000C97A4 File Offset: 0x000C79A4
		protected override void SetActivationLoadAmmoPoint(bool activate)
		{
			foreach (StandingPointWithWeaponRequirement standingPointWithWeaponRequirement in this._ammoLoadPoints)
			{
				standingPointWithWeaponRequirement.SetIsDeactivatedSynched(!activate);
			}
		}

		// Token: 0x06003168 RID: 12648 RVA: 0x000C97F8 File Offset: 0x000C79F8
		protected override void UpdateProjectilePosition()
		{
			MatrixFrame boneEntitialFrameWithIndex = this._sling.GameEntity.GetBoneEntitialFrameWithIndex(this._missileBoneIndex);
			base.Projectile.GameEntity.SetFrame(ref boneEntitialFrameWithIndex, true);
		}

		// Token: 0x06003169 RID: 12649 RVA: 0x000C9835 File Offset: 0x000C7A35
		protected internal override bool IsStandingPointNotUsedOnAccountOfBeingAmmoLoad(StandingPoint standingPoint)
		{
			return (this._ammoLoadPoints.Contains(standingPoint) && this.LoadAmmoStandingPoint != standingPoint) || base.IsStandingPointNotUsedOnAccountOfBeingAmmoLoad(standingPoint);
		}

		// Token: 0x0600316A RID: 12650 RVA: 0x000C9857 File Offset: 0x000C7A57
		protected override float GetDetachmentWeightAux(BattleSideEnum side)
		{
			return base.GetDetachmentWeightAuxForExternalAmmoWeapons(side);
		}

		// Token: 0x0600316B RID: 12651 RVA: 0x000C9860 File Offset: 0x000C7A60
		public void SetSpawnedFromSpawner()
		{
			this._spawnedFromSpawner = true;
		}

		// Token: 0x040014B0 RID: 5296
		public const float TrebuchetDirectionRestriction = 1.3962635f;

		// Token: 0x040014B1 RID: 5297
		private const string BodyTag = "body";

		// Token: 0x040014B2 RID: 5298
		private const string SlingTag = "sling";

		// Token: 0x040014B3 RID: 5299
		private const string RopeTag = "rope";

		// Token: 0x040014B4 RID: 5300
		private const string RotateTag = "rotate";

		// Token: 0x040014B5 RID: 5301
		private const string VerticalAdjusterTag = "vertical_adjuster";

		// Token: 0x040014B6 RID: 5302
		private const string MissileBoneName = "bn_projectile_holder";

		// Token: 0x040014B7 RID: 5303
		private const string RotateObjectTag = "rotate_entity";

		// Token: 0x040014B8 RID: 5304
		public float ProjectileSpeed = 45f;

		// Token: 0x040014B9 RID: 5305
		private SynchedMissionObject _body;

		// Token: 0x040014BA RID: 5306
		private SynchedMissionObject _sling;

		// Token: 0x040014BB RID: 5307
		private SynchedMissionObject _rope;

		// Token: 0x040014BC RID: 5308
		public string IdleWithAmmoAnimation;

		// Token: 0x040014BD RID: 5309
		public string IdleEmptyAnimation;

		// Token: 0x040014BE RID: 5310
		public string BodyFireAnimation;

		// Token: 0x040014BF RID: 5311
		public string BodySetUpAnimation;

		// Token: 0x040014C0 RID: 5312
		public string SlingFireAnimation;

		// Token: 0x040014C1 RID: 5313
		public string SlingSetUpAnimation;

		// Token: 0x040014C2 RID: 5314
		public string RopeFireAnimation;

		// Token: 0x040014C3 RID: 5315
		public string RopeSetUpAnimation;

		// Token: 0x040014C4 RID: 5316
		public string VerticalAdjusterAnimation;

		// Token: 0x040014C5 RID: 5317
		private GameEntity _verticalAdjuster;

		// Token: 0x040014C6 RID: 5318
		private Skeleton _verticalAdjusterSkeleton;

		// Token: 0x040014C7 RID: 5319
		private MatrixFrame _verticalAdjusterStartingLocalFrame;

		// Token: 0x040014C8 RID: 5320
		private float _timeElapsedAfterLoading;

		// Token: 0x040014C9 RID: 5321
		private bool _shootAnimPlayed;

		// Token: 0x040014CA RID: 5322
		private MatrixFrame[] _standingPointLocalIKFrames;

		// Token: 0x040014CB RID: 5323
		private List<StandingPointWithWeaponRequirement> _ammoLoadPoints;

		// Token: 0x040014CC RID: 5324
		private sbyte _missileBoneIndex;
	}
}
