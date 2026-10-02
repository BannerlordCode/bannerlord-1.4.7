using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Objects.Siege;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200033F RID: 831
	public class Ballista : RangedSiegeWeapon, ISpawnable
	{
		// Token: 0x1700089A RID: 2202
		// (get) Token: 0x06002E61 RID: 11873 RVA: 0x000B30AD File Offset: 0x000B12AD
		// (set) Token: 0x06002E62 RID: 11874 RVA: 0x000B30B5 File Offset: 0x000B12B5
		private protected SynchedMissionObject ballistaBody { protected get; private set; }

		// Token: 0x1700089B RID: 2203
		// (get) Token: 0x06002E63 RID: 11875 RVA: 0x000B30BE File Offset: 0x000B12BE
		// (set) Token: 0x06002E64 RID: 11876 RVA: 0x000B30C6 File Offset: 0x000B12C6
		private protected SynchedMissionObject ballistaNavel { protected get; private set; }

		// Token: 0x1700089C RID: 2204
		// (get) Token: 0x06002E65 RID: 11877 RVA: 0x000B30CF File Offset: 0x000B12CF
		public override float DirectionRestriction
		{
			get
			{
				return this.HorizontalDirectionRestriction;
			}
		}

		// Token: 0x1700089D RID: 2205
		// (get) Token: 0x06002E66 RID: 11878 RVA: 0x000B30D7 File Offset: 0x000B12D7
		protected override float ShootingSpeed
		{
			get
			{
				return this.BallistaShootingSpeed;
			}
		}

		// Token: 0x1700089E RID: 2206
		// (get) Token: 0x06002E67 RID: 11879 RVA: 0x000B30DF File Offset: 0x000B12DF
		public override Vec3 CanShootAtPointCheckingOffset
		{
			get
			{
				return new Vec3(0f, 0f, 0.5f, -1f);
			}
		}

		// Token: 0x1700089F RID: 2207
		// (get) Token: 0x06002E68 RID: 11880 RVA: 0x000B30FA File Offset: 0x000B12FA
		protected override bool WeaponMovesDownToReload
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170008A0 RID: 2208
		// (get) Token: 0x06002E69 RID: 11881 RVA: 0x000B30FD File Offset: 0x000B12FD
		public override string MultipleProjectileId
		{
			get
			{
				return "ballista_c_projectile_grape";
			}
		}

		// Token: 0x170008A1 RID: 2209
		// (get) Token: 0x06002E6A RID: 11882 RVA: 0x000B3104 File Offset: 0x000B1304
		public override string MultipleProjectileFlyingId
		{
			get
			{
				return "ballista_c_projectile_grape_projectile";
			}
		}

		// Token: 0x06002E6B RID: 11883 RVA: 0x000B310C File Offset: 0x000B130C
		protected override void RegisterAnimationParameters()
		{
			this.SkeletonOwnerObjects = new SynchedMissionObject[1];
			this.Skeletons = new Skeleton[1];
			List<SynchedMissionObject> list = this.ballistaBody.GameEntity.CollectScriptComponentsWithTagIncludingChildrenRecursive<SynchedMissionObject>(this.SkeletonTag);
			if (list.Count == 0)
			{
				this.SkeletonOwnerObjects[0] = this.ballistaBody;
			}
			else
			{
				this.SkeletonOwnerObjects[0] = list[0];
			}
			this.Skeletons[0] = this.SkeletonOwnerObjects[0].GameEntity.Skeleton;
			base.SkeletonName = "ballista_skeleton";
			base.FireAnimation = "ballista_fire";
			base.FireAnimationIndex = MBAnimation.GetAnimationIndexWithName("ballista_fire");
			base.SetUpAnimation = "ballista_set_up";
			base.SetUpAnimationIndex = MBAnimation.GetAnimationIndexWithName("ballista_set_up");
			this._idleAnimationActionIndex = ActionIndexCache.Create(this.IdleActionName);
			this._reloadAnimationActionIndex = ActionIndexCache.Create(this.ReloadActionName);
			this._placeAmmoStartAnimationActionIndex = ActionIndexCache.Create(this.PlaceAmmoStartActionName);
			this._placeAmmoEndAnimationActionIndex = ActionIndexCache.Create(this.PlaceAmmoEndActionName);
			this._pickUpAmmoStartAnimationActionIndex = ActionIndexCache.Create(this.PickUpAmmoStartActionName);
			this._pickUpAmmoEndAnimationActionIndex = ActionIndexCache.Create(this.PickUpAmmoEndActionName);
		}

		// Token: 0x06002E6C RID: 11884 RVA: 0x000B3233 File Offset: 0x000B1433
		public override SiegeEngineType GetSiegeEngineType()
		{
			return DefaultSiegeEngineTypes.Ballista;
		}

		// Token: 0x06002E6D RID: 11885 RVA: 0x000B323C File Offset: 0x000B143C
		protected internal override void OnInit()
		{
			this.ballistaBody = base.GameEntity.CollectScriptComponentsWithTagIncludingChildrenRecursive<SynchedMissionObject>(this.BodyTag)[0];
			this.ballistaNavel = base.GameEntity.CollectScriptComponentsWithTagIncludingChildrenRecursive<SynchedMissionObject>(this.NavelTag)[0];
			this.RotationObject = this;
			base.OnInit();
			this.UsesMouseForAiming = true;
			this.GetSoundEventIndices();
			this._ballistaNavelInitialFrame = this.ballistaNavel.GameEntity.GetFrame();
			MatrixFrame globalFrame = this.ballistaBody.GameEntity.GetGlobalFrame();
			this._ballistaBodyInitialLocalFrame = this.ballistaBody.GameEntity.GetFrame();
			MatrixFrame globalFrame2 = base.PilotStandingPoint.GameEntity.GetGlobalFrame();
			this._pilotInitialLocalFrame = base.PilotStandingPoint.GameEntity.GetFrame();
			this._pilotInitialLocalIKFrame = globalFrame2.TransformToLocal(in globalFrame);
			this._missileInitialLocalFrame = base.Projectile.GameEntity.GetFrame();
			base.PilotStandingPoint.AddComponent(new ClearHandInverseKinematicsOnStopUsageComponent());
			this.MissileStartingPositionEntityForSimulation = TaleWorlds.Engine.GameEntity.CreateFromWeakEntity(base.Projectile.GameEntity.Parent.GetChildren().FirstOrDefault<WeakGameEntity>((WeakGameEntity x) => x.Name == "projectile_leaving_position"));
			this.EnemyRangeToStopUsing = 7f;
			this.AttackClickWillReload = true;
			this.WeaponNeedsClickToReload = true;
			base.SetScriptComponentToTick(this.GetTickRequirement());
			this.ApplyAimChange();
		}

		// Token: 0x06002E6E RID: 11886 RVA: 0x000B33C0 File Offset: 0x000B15C0
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

		// Token: 0x06002E6F RID: 11887 RVA: 0x000B345B File Offset: 0x000B165B
		protected override bool CanRotate()
		{
			return base.State != RangedSiegeWeapon.WeaponState.Shooting;
		}

		// Token: 0x06002E70 RID: 11888 RVA: 0x000B3469 File Offset: 0x000B1669
		public override UsableMachineAIBase CreateAIBehaviorObject()
		{
			return new BallistaAI(this);
		}

		// Token: 0x06002E71 RID: 11889 RVA: 0x000B3474 File Offset: 0x000B1674
		protected override void OnRangedSiegeWeaponStateChange()
		{
			base.OnRangedSiegeWeaponStateChange();
			RangedSiegeWeapon.WeaponState state = base.State;
			if (state != RangedSiegeWeapon.WeaponState.Idle)
			{
				if (state == RangedSiegeWeapon.WeaponState.WaitingBeforeProjectileLeaving)
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
			else if (base.AmmoCount > 0)
			{
				if (!GameNetwork.IsClientOrReplay)
				{
					this.ConsumeAmmo();
					return;
				}
				this.SetAmmo(base.AmmoCount - 1);
			}
		}

		// Token: 0x170008A2 RID: 2210
		// (get) Token: 0x06002E72 RID: 11890 RVA: 0x000B34E4 File Offset: 0x000B16E4
		protected override float MaximumBallisticError
		{
			get
			{
				return 0.5f;
			}
		}

		// Token: 0x170008A3 RID: 2211
		// (get) Token: 0x06002E73 RID: 11891 RVA: 0x000B34EB File Offset: 0x000B16EB
		protected override float HorizontalAimSensitivity
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x170008A4 RID: 2212
		// (get) Token: 0x06002E74 RID: 11892 RVA: 0x000B34F2 File Offset: 0x000B16F2
		protected override float VerticalAimSensitivity
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x06002E75 RID: 11893 RVA: 0x000B34F9 File Offset: 0x000B16F9
		protected override void HandleUserAiming(float dt)
		{
			if (base.PilotAgent == null)
			{
				this.TargetReleaseAngle = 0f;
			}
			base.HandleUserAiming(dt);
		}

		// Token: 0x06002E76 RID: 11894 RVA: 0x000B3518 File Offset: 0x000B1718
		protected override void ApplyAimChange()
		{
			MatrixFrame ballistaNavelInitialFrame = this._ballistaNavelInitialFrame;
			ballistaNavelInitialFrame.rotation.RotateAboutAnArbitraryVector(in this._ballistaNavelInitialFrame.rotation.u, this.CurrentDirection);
			this.ballistaNavel.GameEntity.SetLocalFrame(ref ballistaNavelInitialFrame, false);
			MatrixFrame matrixFrame = this._ballistaNavelInitialFrame.TransformToLocal(in this._pilotInitialLocalFrame);
			MatrixFrame matrixFrame2 = ballistaNavelInitialFrame.TransformToParent(in matrixFrame);
			base.PilotStandingPoint.GameEntity.SetLocalFrame(ref matrixFrame2, false);
			MatrixFrame ballistaBodyInitialLocalFrame = this._ballistaBodyInitialLocalFrame;
			ballistaBodyInitialLocalFrame.rotation.RotateAboutAnArbitraryVector(in ballistaBodyInitialLocalFrame.rotation.s, -this.CurrentReleaseAngle);
			this.ballistaBody.GameEntity.SetLocalFrame(ref ballistaBodyInitialLocalFrame, false);
		}

		// Token: 0x06002E77 RID: 11895 RVA: 0x000B35D5 File Offset: 0x000B17D5
		protected override void ApplyCurrentDirectionToEntity()
		{
			this.ApplyAimChange();
		}

		// Token: 0x06002E78 RID: 11896 RVA: 0x000B35DD File Offset: 0x000B17DD
		protected override void GetSoundEventIndices()
		{
			this.MoveSoundIndex = SoundEvent.GetEventIdFromString("event:/mission/siege/ballista/move");
			this.ReloadSoundIndex = SoundEvent.GetEventIdFromString("event:/mission/siege/ballista/reload");
			this.FireSoundIndex = SoundEvent.GetEventIdFromString("event:/mission/siege/ballista/fire");
		}

		// Token: 0x06002E79 RID: 11897 RVA: 0x000B360F File Offset: 0x000B180F
		protected internal override bool IsTargetValid(ITargetable target)
		{
			return !(target is ICastleKeyPosition);
		}

		// Token: 0x06002E7A RID: 11898 RVA: 0x000B3620 File Offset: 0x000B1820
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			if (base.GameEntity.IsVisibleIncludeParents())
			{
				return base.GetTickRequirement() | ScriptComponentBehavior.TickRequirement.Tick | ScriptComponentBehavior.TickRequirement.TickParallel;
			}
			return base.GetTickRequirement();
		}

		// Token: 0x06002E7B RID: 11899 RVA: 0x000B364E File Offset: 0x000B184E
		protected internal override void OnTick(float dt)
		{
			base.OnTick(dt);
			if (this._changeToState != RangedSiegeWeapon.WeaponState.Invalid)
			{
				base.State = this._changeToState;
				this._changeToState = RangedSiegeWeapon.WeaponState.Invalid;
			}
		}

		// Token: 0x06002E7C RID: 11900 RVA: 0x000B3674 File Offset: 0x000B1874
		protected internal override void OnTickParallel(float dt)
		{
			base.OnTickParallel(dt);
			if (!base.GameEntity.IsVisibleIncludeParents())
			{
				return;
			}
			if (base.PilotAgent != null)
			{
				Agent pilotAgent = base.PilotAgent;
				MatrixFrame globalFrame = this.ballistaBody.GameEntity.GetGlobalFrame();
				pilotAgent.SetHandInverseKinematicsFrameForMissionObjectUsage(in this._pilotInitialLocalIKFrame, in globalFrame, this.AnimationHeightDifference);
				ActionIndexCache currentAction = base.PilotAgent.GetCurrentAction(1);
				if (currentAction == this._pickUpAmmoEndAnimationActionIndex || currentAction == this._placeAmmoStartAnimationActionIndex)
				{
					MatrixFrame frame = base.PilotAgent.Frame;
					MatrixFrame matrixFrame = base.PilotAgent.GetBoneEntitialFrame(base.PilotAgent.Monster.MainHandItemBoneIndex, false);
					matrixFrame = frame.TransformToParent(in matrixFrame);
					base.Projectile.GameEntity.SetGlobalFrame(in matrixFrame, true);
				}
				else
				{
					base.Projectile.GameEntity.SetFrame(ref this._missileInitialLocalFrame, true);
				}
			}
			if (GameNetwork.IsClientOrReplay)
			{
				return;
			}
			switch (base.State)
			{
			case RangedSiegeWeapon.WeaponState.LoadingAmmo:
			{
				bool flag = false;
				if (base.PilotAgent != null)
				{
					if (!this.HasAmmo)
					{
						if (base.PilotAgent.Controller == AgentControllerType.AI)
						{
							base.PilotAgent.StopUsingGameObjectMT(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
							return;
						}
						break;
					}
					else
					{
						ActionIndexCache currentAction2 = base.PilotAgent.GetCurrentAction(1);
						this.FinalReloadSpeed = MissionGameModels.Current.MissionSiegeEngineCalculationModel.CalculateReloadSpeed(base.PilotAgent, this.BaseReloadSpeed);
						base.PilotAgent.SetCurrentActionSpeed(1, this.FinalReloadSpeed);
						if (currentAction2 != this._pickUpAmmoStartAnimationActionIndex && currentAction2 != this._pickUpAmmoEndAnimationActionIndex && currentAction2 != this._placeAmmoStartAnimationActionIndex && currentAction2 != this._placeAmmoEndAnimationActionIndex && !base.PilotAgent.SetActionChannel(1, in this._pickUpAmmoStartAnimationActionIndex, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true) && base.PilotAgent.Controller != AgentControllerType.AI)
						{
							base.PilotAgent.StopUsingGameObjectMT(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
						}
						else if (currentAction2 == this._pickUpAmmoEndAnimationActionIndex || currentAction2 == this._placeAmmoStartAnimationActionIndex)
						{
							flag = true;
						}
						else if (currentAction2 == this._placeAmmoEndAnimationActionIndex)
						{
							flag = true;
							this._changeToState = RangedSiegeWeapon.WeaponState.WaitingBeforeIdle;
						}
					}
				}
				base.Projectile.SetVisibleSynched(flag, false);
				return;
			}
			case RangedSiegeWeapon.WeaponState.WaitingBeforeIdle:
				if (base.PilotAgent == null)
				{
					this._changeToState = RangedSiegeWeapon.WeaponState.Idle;
					return;
				}
				if (base.PilotAgent.GetCurrentAction(1) != this._placeAmmoEndAnimationActionIndex)
				{
					if (base.PilotAgent.Controller != AgentControllerType.AI)
					{
						base.PilotAgent.StopUsingGameObjectMT(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
					}
					this._changeToState = RangedSiegeWeapon.WeaponState.Idle;
					return;
				}
				if (base.PilotAgent.GetCurrentActionProgress(1) > 0.9999f)
				{
					this._changeToState = RangedSiegeWeapon.WeaponState.Idle;
					if (base.PilotAgent != null && !base.PilotAgent.SetActionChannel(1, in this._idleAnimationActionIndex, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true) && base.PilotAgent.Controller != AgentControllerType.AI)
					{
						base.PilotAgent.StopUsingGameObjectMT(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
						return;
					}
				}
				break;
			case RangedSiegeWeapon.WeaponState.Reloading:
				this.FinalReloadSpeed = MissionGameModels.Current.MissionSiegeEngineCalculationModel.CalculateReloadSpeed(base.PilotAgent, this.BaseReloadSpeed);
				if (base.PilotAgent != null && !base.PilotAgent.SetActionChannel(1, in this._reloadAnimationActionIndex, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true) && base.PilotAgent.Controller != AgentControllerType.AI)
				{
					base.PilotAgent.StopUsingGameObjectMT(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
					return;
				}
				break;
			default:
				if (base.PilotAgent != null)
				{
					if (base.PilotAgent.IsInBeingStruckAction)
					{
						if (base.PilotAgent.GetCurrentAction(1) != ActionIndexCache.act_strike_bent_over)
						{
							base.PilotAgent.SetActionChannel(1, in ActionIndexCache.act_strike_bent_over, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true);
							return;
						}
					}
					else if (!base.PilotAgent.SetActionChannel(1, in this._idleAnimationActionIndex, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true) && base.PilotAgent.Controller != AgentControllerType.AI)
					{
						base.PilotAgent.StopUsingGameObjectMT(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
					}
				}
				break;
			}
		}

		// Token: 0x06002E7D RID: 11901 RVA: 0x000B3AF1 File Offset: 0x000B1CF1
		public override TextObject GetActionTextForStandingPoint(UsableMissionObject usableGameObject)
		{
			TextObject textObject = new TextObject("{=fEQAPJ2e}{KEY} Use", null);
			textObject.SetTextVariable("KEY", HyperlinkTexts.GetKeyHyperlinkText(HotKeyManager.GetHotKeyId("CombatHotKeyCategory", 13), 1f));
			return textObject;
		}

		// Token: 0x06002E7E RID: 11902 RVA: 0x000B3B20 File Offset: 0x000B1D20
		public override TextObject GetDescriptionText(WeakGameEntity gameEntity)
		{
			return new TextObject("{=abbALYlp}Ballista", null);
		}

		// Token: 0x06002E7F RID: 11903 RVA: 0x000B3B30 File Offset: 0x000B1D30
		protected override void UpdateAmmoMesh()
		{
			int num = 8 - base.AmmoCount;
			base.GameEntity.SetVectorArgument(0f, (float)num, 0f, 0f);
		}

		// Token: 0x06002E80 RID: 11904 RVA: 0x000B3B68 File Offset: 0x000B1D68
		public override float ProcessTargetValue(float baseValue, TargetFlags flags)
		{
			if (flags.HasAnyFlag(TargetFlags.NotAThreat))
			{
				return -1000f;
			}
			if (flags.HasAnyFlag(TargetFlags.IsSiegeEngine))
			{
				baseValue *= 0.2f;
			}
			if (flags.HasAnyFlag(TargetFlags.IsStructure))
			{
				baseValue *= 0.05f;
			}
			if (flags.HasAnyFlag(TargetFlags.DebugThreat))
			{
				baseValue *= 10000f;
			}
			return baseValue;
		}

		// Token: 0x06002E81 RID: 11905 RVA: 0x000B3BC0 File Offset: 0x000B1DC0
		public override TargetFlags GetTargetFlags()
		{
			TargetFlags targetFlags = TargetFlags.None;
			targetFlags |= TargetFlags.IsFlammable;
			targetFlags |= TargetFlags.IsSiegeEngine;
			if (this.Side == BattleSideEnum.Attacker)
			{
				targetFlags |= TargetFlags.IsAttacker;
			}
			targetFlags |= TargetFlags.IsSmall;
			if (base.IsDestroyed || this.IsDeactivated)
			{
				targetFlags |= TargetFlags.NotAThreat;
			}
			if (this.Side == BattleSideEnum.Attacker && DebugSiegeBehavior.DebugDefendState == DebugSiegeBehavior.DebugStateDefender.DebugDefendersToBallistae)
			{
				targetFlags |= TargetFlags.DebugThreat;
			}
			if (this.Side == BattleSideEnum.Defender && DebugSiegeBehavior.DebugAttackState == DebugSiegeBehavior.DebugStateAttacker.DebugAttackersToBallistae)
			{
				targetFlags |= TargetFlags.DebugThreat;
			}
			return targetFlags;
		}

		// Token: 0x06002E82 RID: 11906 RVA: 0x000B3C31 File Offset: 0x000B1E31
		public override float GetTargetValue(List<Vec3> weaponPos)
		{
			return 30f * base.GetUserMultiplierOfWeapon() * this.GetDistanceMultiplierOfWeapon(weaponPos[0]) * base.GetHitPointMultiplierOfWeapon();
		}

		// Token: 0x06002E83 RID: 11907 RVA: 0x000B3C54 File Offset: 0x000B1E54
		public void SetSpawnedFromSpawner()
		{
			this._spawnedFromSpawner = true;
		}

		// Token: 0x0400127E RID: 4734
		public string NavelTag = "BallistaNavel";

		// Token: 0x0400127F RID: 4735
		public string BodyTag = "BallistaBody";

		// Token: 0x04001280 RID: 4736
		public string SkeletonTag = "SkeletonEntity";

		// Token: 0x04001281 RID: 4737
		public float AnimationHeightDifference;

		// Token: 0x04001284 RID: 4740
		private MatrixFrame _ballistaBodyInitialLocalFrame;

		// Token: 0x04001285 RID: 4741
		private MatrixFrame _ballistaNavelInitialFrame;

		// Token: 0x04001286 RID: 4742
		private MatrixFrame _pilotInitialLocalFrame;

		// Token: 0x04001287 RID: 4743
		private MatrixFrame _pilotInitialLocalIKFrame;

		// Token: 0x04001288 RID: 4744
		private MatrixFrame _missileInitialLocalFrame;

		// Token: 0x04001289 RID: 4745
		[EditableScriptComponentVariable(true, "")]
		protected string IdleActionName = "act_usage_ballista_idle_attacker";

		// Token: 0x0400128A RID: 4746
		[EditableScriptComponentVariable(true, "")]
		protected string ReloadActionName = "act_usage_ballista_reload_attacker";

		// Token: 0x0400128B RID: 4747
		[EditableScriptComponentVariable(true, "")]
		protected string PlaceAmmoStartActionName = "act_usage_ballista_ammo_place_start_attacker";

		// Token: 0x0400128C RID: 4748
		[EditableScriptComponentVariable(true, "")]
		protected string PlaceAmmoEndActionName = "act_usage_ballista_ammo_place_end_attacker";

		// Token: 0x0400128D RID: 4749
		[EditableScriptComponentVariable(true, "")]
		protected string PickUpAmmoStartActionName = "act_usage_ballista_ammo_pick_up_start_attacker";

		// Token: 0x0400128E RID: 4750
		[EditableScriptComponentVariable(true, "")]
		protected string PickUpAmmoEndActionName = "act_usage_ballista_ammo_pick_up_end_attacker";

		// Token: 0x0400128F RID: 4751
		private ActionIndexCache _idleAnimationActionIndex;

		// Token: 0x04001290 RID: 4752
		private ActionIndexCache _reloadAnimationActionIndex;

		// Token: 0x04001291 RID: 4753
		private ActionIndexCache _placeAmmoStartAnimationActionIndex;

		// Token: 0x04001292 RID: 4754
		private ActionIndexCache _placeAmmoEndAnimationActionIndex;

		// Token: 0x04001293 RID: 4755
		private ActionIndexCache _pickUpAmmoStartAnimationActionIndex;

		// Token: 0x04001294 RID: 4756
		private ActionIndexCache _pickUpAmmoEndAnimationActionIndex;

		// Token: 0x04001295 RID: 4757
		[EditableScriptComponentVariable(true, "")]
		public float HorizontalDirectionRestriction = 1.5707964f;

		// Token: 0x04001296 RID: 4758
		public float BallistaShootingSpeed = 120f;

		// Token: 0x04001297 RID: 4759
		private RangedSiegeWeapon.WeaponState _changeToState = RangedSiegeWeapon.WeaponState.Invalid;
	}
}
