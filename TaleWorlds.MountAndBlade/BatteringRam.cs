using System;
using System.Collections.Generic;
using System.Linq;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Network.Messages;
using TaleWorlds.MountAndBlade.Objects.Siege;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000340 RID: 832
	public class BatteringRam : SiegeWeapon, IPathHolder, IPrimarySiegeWeapon, IMoveableSiegeWeapon, ISpawnable
	{
		// Token: 0x170008A5 RID: 2213
		// (get) Token: 0x06002E85 RID: 11909 RVA: 0x000B3CF3 File Offset: 0x000B1EF3
		// (set) Token: 0x06002E86 RID: 11910 RVA: 0x000B3CFB File Offset: 0x000B1EFB
		public SiegeWeaponMovementComponent MovementComponent { get; private set; }

		// Token: 0x170008A6 RID: 2214
		// (get) Token: 0x06002E87 RID: 11911 RVA: 0x000B3D04 File Offset: 0x000B1F04
		public FormationAI.BehaviorSide WeaponSide
		{
			get
			{
				return this._weaponSide;
			}
		}

		// Token: 0x170008A7 RID: 2215
		// (get) Token: 0x06002E88 RID: 11912 RVA: 0x000B3D0C File Offset: 0x000B1F0C
		public string PathEntity
		{
			get
			{
				return this._pathEntityName;
			}
		}

		// Token: 0x170008A8 RID: 2216
		// (get) Token: 0x06002E89 RID: 11913 RVA: 0x000B3D14 File Offset: 0x000B1F14
		public bool EditorGhostEntityMove
		{
			get
			{
				return this.GhostEntityMove;
			}
		}

		// Token: 0x170008A9 RID: 2217
		// (get) Token: 0x06002E8A RID: 11914 RVA: 0x000B3D1C File Offset: 0x000B1F1C
		// (set) Token: 0x06002E8B RID: 11915 RVA: 0x000B3D24 File Offset: 0x000B1F24
		public BatteringRam.RamState State
		{
			get
			{
				return this._state;
			}
			set
			{
				if (this._state != value)
				{
					this._state = value;
				}
			}
		}

		// Token: 0x170008AA RID: 2218
		// (get) Token: 0x06002E8C RID: 11916 RVA: 0x000B3D36 File Offset: 0x000B1F36
		public MissionObject TargetCastlePosition
		{
			get
			{
				return this._gate;
			}
		}

		// Token: 0x06002E8D RID: 11917 RVA: 0x000B3D3E File Offset: 0x000B1F3E
		public bool HasCompletedAction()
		{
			return this._gate == null || this._gate.IsDestroyed || (this._gate.State == CastleGate.GateState.Open && this.HasArrivedAtTarget);
		}

		// Token: 0x170008AB RID: 2219
		// (get) Token: 0x06002E8E RID: 11918 RVA: 0x000B3D6C File Offset: 0x000B1F6C
		public float SiegeWeaponPriority
		{
			get
			{
				return 25f;
			}
		}

		// Token: 0x170008AC RID: 2220
		// (get) Token: 0x06002E8F RID: 11919 RVA: 0x000B3D73 File Offset: 0x000B1F73
		public int OverTheWallNavMeshID
		{
			get
			{
				return this.GateNavMeshId;
			}
		}

		// Token: 0x170008AD RID: 2221
		// (get) Token: 0x06002E90 RID: 11920 RVA: 0x000B3D7B File Offset: 0x000B1F7B
		public bool HoldLadders
		{
			get
			{
				return !this.MovementComponent.HasArrivedAtTarget;
			}
		}

		// Token: 0x170008AE RID: 2222
		// (get) Token: 0x06002E91 RID: 11921 RVA: 0x000B3D8B File Offset: 0x000B1F8B
		public bool SendLadders
		{
			get
			{
				return this.MovementComponent.HasArrivedAtTarget;
			}
		}

		// Token: 0x170008AF RID: 2223
		// (get) Token: 0x06002E92 RID: 11922 RVA: 0x000B3D98 File Offset: 0x000B1F98
		// (set) Token: 0x06002E93 RID: 11923 RVA: 0x000B3DA0 File Offset: 0x000B1FA0
		public bool HasArrivedAtTarget
		{
			get
			{
				return this._hasArrivedAtTarget;
			}
			set
			{
				if (!GameNetwork.IsClientOrReplay)
				{
					this.MovementComponent.SetDestinationNavMeshIdState(!value);
				}
				if (this._hasArrivedAtTarget != value)
				{
					this._hasArrivedAtTarget = value;
					if (GameNetwork.IsServerOrRecorder)
					{
						GameNetwork.BeginBroadcastModuleEvent();
						GameNetwork.WriteMessage(new SetBatteringRamHasArrivedAtTarget(base.Id));
						GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
						return;
					}
					if (GameNetwork.IsClientOrReplay)
					{
						this.MovementComponent.MoveToTargetAsClient();
					}
				}
			}
		}

		// Token: 0x06002E94 RID: 11924 RVA: 0x000B3E0A File Offset: 0x000B200A
		public override void Disable()
		{
			base.Disable();
			if (!GameNetwork.IsClientOrReplay)
			{
				if (this.DisabledNavMeshID != 0)
				{
					base.Scene.SetAbilityOfFacesWithId(this.DisabledNavMeshID, true);
				}
				base.Scene.SetAbilityOfFacesWithId(this.DynamicNavmeshIdStart + 4, false);
			}
		}

		// Token: 0x06002E95 RID: 11925 RVA: 0x000B3E49 File Offset: 0x000B2049
		public override SiegeEngineType GetSiegeEngineType()
		{
			return DefaultSiegeEngineTypes.Ram;
		}

		// Token: 0x06002E96 RID: 11926 RVA: 0x000B3E50 File Offset: 0x000B2050
		protected internal override void OnInit()
		{
			base.OnInit();
			DestructableComponent firstScriptOfType = base.GameEntity.GetFirstScriptOfType<DestructableComponent>();
			if (firstScriptOfType != null)
			{
				firstScriptOfType.BattleSide = BattleSideEnum.Attacker;
			}
			this._state = BatteringRam.RamState.Stable;
			IEnumerable<WeakGameEntity> enumerable = from ewgt in base.Scene.FindWeakEntitiesWithTag(this._gateTag).ToList<WeakGameEntity>()
				where ewgt.HasScriptOfType<CastleGate>()
				select ewgt;
			if (!enumerable.IsEmpty<WeakGameEntity>())
			{
				this._gate = enumerable.First<WeakGameEntity>().GetFirstScriptOfType<CastleGate>();
				this._gate.AttackerSiegeWeapon = this;
			}
			this.AddRegularMovementComponent();
			this._batteringRamBody = TaleWorlds.Engine.GameEntity.CreateFromWeakEntity(base.GameEntity.GetFirstChildEntityWithTag("body"));
			this._batteringRamBodySkeleton = this._batteringRamBody.Skeleton;
			this._batteringRamBodySkeleton.SetAnimationAtChannel("batteringram_idle", 0, 1f, 0f, 0f);
			this._pullStandingPoints = new List<StandingPoint>();
			this._pullStandingPointLocalIKFrames = new List<MatrixFrame>();
			MatrixFrame globalFrame = base.GameEntity.GetGlobalFrame();
			if (base.StandingPoints != null)
			{
				foreach (StandingPoint standingPoint in base.StandingPoints)
				{
					standingPoint.AddComponent(new ResetAnimationOnStopUsageComponent(ActionIndexCache.act_none, false));
					if (standingPoint.GameEntity.HasTag("pull"))
					{
						standingPoint.IsDeactivated = true;
						this._pullStandingPoints.Add(standingPoint);
						this._pullStandingPointLocalIKFrames.Add(standingPoint.GameEntity.GetGlobalFrame().TransformToLocal(in globalFrame));
						standingPoint.AddComponent(new ClearHandInverseKinematicsOnStopUsageComponent());
					}
				}
			}
			string sideTag = this._sideTag;
			if (!(sideTag == "left"))
			{
				if (!(sideTag == "middle"))
				{
					if (!(sideTag == "right"))
					{
						this._weaponSide = FormationAI.BehaviorSide.Middle;
					}
					else
					{
						this._weaponSide = FormationAI.BehaviorSide.Right;
					}
				}
				else
				{
					this._weaponSide = FormationAI.BehaviorSide.Middle;
				}
			}
			else
			{
				this._weaponSide = FormationAI.BehaviorSide.Left;
			}
			this._ditchFillDebris = base.Scene.FindEntitiesWithTag("ditch_filler").FirstOrDefault<GameEntity>((GameEntity df) => df.HasTag(this._sideTag));
			base.SetScriptComponentToTick(this.GetTickRequirement());
			Mission.Current.AddToWeaponListForFriendlyFirePreventing(this);
		}

		// Token: 0x06002E97 RID: 11927 RVA: 0x000B40B0 File Offset: 0x000B22B0
		private void AddRegularMovementComponent()
		{
			this.MovementComponent = new SiegeWeaponMovementComponent
			{
				PathEntityName = this.PathEntity,
				MinSpeed = this.MinSpeed,
				MaxSpeed = this.MaxSpeed,
				MainObject = this,
				WheelDiameter = this.WheelDiameter,
				NavMeshIdToDisableOnDestination = this.NavMeshIdToDisableOnDestination,
				MovementSoundCodeID = SoundEvent.GetEventIdFromString("event:/mission/siege/batteringram/move"),
				GhostEntitySpeedMultiplier = this.GhostEntitySpeedMultiplier
			};
			base.AddComponent(this.MovementComponent);
		}

		// Token: 0x06002E98 RID: 11928 RVA: 0x000B4134 File Offset: 0x000B2334
		protected internal override void OnDeploymentStateChanged(bool isDeployed)
		{
			base.OnDeploymentStateChanged(isDeployed);
			if (this._ditchFillDebris != null)
			{
				this._ditchFillDebris.SetVisibilityExcludeParents(isDeployed);
				if (!GameNetwork.IsClientOrReplay)
				{
					if (isDeployed)
					{
						Mission.Current.Scene.SetAbilityOfFacesWithId(this._bridgeNavMeshID1, true);
						Mission.Current.Scene.SetAbilityOfFacesWithId(this._bridgeNavMeshID2, true);
						Mission.Current.Scene.SeparateFacesWithId(this._ditchNavMeshID1, this._groundToBridgeNavMeshID1);
						Mission.Current.Scene.SeparateFacesWithId(this._ditchNavMeshID2, this._groundToBridgeNavMeshID2);
						Mission.Current.Scene.MergeFacesWithId(this._bridgeNavMeshID1, this._groundToBridgeNavMeshID1, 0);
						Mission.Current.Scene.MergeFacesWithId(this._bridgeNavMeshID2, this._groundToBridgeNavMeshID2, 0);
						return;
					}
					Mission.Current.Scene.SeparateFacesWithId(this._bridgeNavMeshID1, this._groundToBridgeNavMeshID1);
					Mission.Current.Scene.SeparateFacesWithId(this._bridgeNavMeshID2, this._groundToBridgeNavMeshID2);
					Mission.Current.Scene.SetAbilityOfFacesWithId(this._bridgeNavMeshID1, false);
					Mission.Current.Scene.SetAbilityOfFacesWithId(this._bridgeNavMeshID2, false);
					Mission.Current.Scene.MergeFacesWithId(this._ditchNavMeshID1, this._groundToBridgeNavMeshID1, 0);
					Mission.Current.Scene.MergeFacesWithId(this._ditchNavMeshID2, this._groundToBridgeNavMeshID2, 0);
				}
			}
		}

		// Token: 0x06002E99 RID: 11929 RVA: 0x000B42B0 File Offset: 0x000B24B0
		public MatrixFrame GetInitialFrame()
		{
			if (this.MovementComponent != null)
			{
				return this.MovementComponent.GetInitialFrame();
			}
			return base.GameEntity.GetGlobalFrame();
		}

		// Token: 0x06002E9A RID: 11930 RVA: 0x000B42E0 File Offset: 0x000B24E0
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			if (base.GameEntity.IsVisibleIncludeParents())
			{
				return base.GetTickRequirement() | ScriptComponentBehavior.TickRequirement.Tick | ScriptComponentBehavior.TickRequirement.TickParallel;
			}
			return base.GetTickRequirement();
		}

		// Token: 0x06002E9B RID: 11931 RVA: 0x000B4310 File Offset: 0x000B2510
		protected internal override void OnTickParallel(float dt)
		{
			base.OnTickParallel(dt);
			if (!base.GameEntity.IsVisibleIncludeParents())
			{
				return;
			}
			this.MovementComponent.TickParallelManually(dt);
			MatrixFrame globalFrame = base.GameEntity.GetGlobalFrame();
			for (int i = 0; i < this._pullStandingPoints.Count; i++)
			{
				StandingPoint standingPoint = this._pullStandingPoints[i];
				if (standingPoint.HasUser)
				{
					if (standingPoint.UserAgent.IsInBeingStruckAction)
					{
						standingPoint.UserAgent.ClearHandInverseKinematics();
					}
					else
					{
						Agent userAgent = standingPoint.UserAgent;
						MatrixFrame matrixFrame = this._pullStandingPointLocalIKFrames[i];
						userAgent.SetHandInverseKinematicsFrameForMissionObjectUsage(in matrixFrame, in globalFrame, 0f);
					}
				}
			}
			if (this.MovementComponent.HasArrivedAtTarget && !this.IsDeactivated)
			{
				int userCountNotInStruckAction = base.UserCountNotInStruckAction;
				if (userCountNotInStruckAction > 0)
				{
					float animationParameterAtChannel = this._batteringRamBodySkeleton.GetAnimationParameterAtChannel(0);
					this.UpdateHitAnimationWithProgress((userCountNotInStruckAction - 1) / 2, animationParameterAtChannel);
				}
			}
		}

		// Token: 0x06002E9C RID: 11932 RVA: 0x000B43F8 File Offset: 0x000B25F8
		protected internal override void OnTick(float dt)
		{
			base.OnTick(dt);
			if (!base.GameEntity.IsVisibleIncludeParents())
			{
				return;
			}
			if (!GameNetwork.IsClientOrReplay)
			{
				if (this.MovementComponent.HasArrivedAtTarget && !this.HasArrivedAtTarget)
				{
					this.HasArrivedAtTarget = true;
					foreach (StandingPoint standingPoint in base.StandingPoints)
					{
						standingPoint.SetIsDeactivatedSynched(standingPoint.GameEntity.HasTag("move"));
					}
					if (this.DisabledNavMeshID != 0)
					{
						base.GameEntity.Scene.SetAbilityOfFacesWithId(this.DisabledNavMeshID, false);
					}
				}
				if (this.MovementComponent.HasArrivedAtTarget)
				{
					if (this._gate == null || this._gate.IsDestroyed || this._gate.IsGateOpen)
					{
						if (!this._isAllStandingPointsDisabled)
						{
							foreach (StandingPoint standingPoint2 in base.StandingPoints)
							{
								standingPoint2.SetIsDeactivatedSynched(true);
							}
							this._isAllStandingPointsDisabled = true;
							return;
						}
					}
					else
					{
						if (this._isAllStandingPointsDisabled && !this.IsDeactivated)
						{
							foreach (StandingPoint standingPoint3 in base.StandingPoints)
							{
								standingPoint3.SetIsDeactivatedSynched(false);
							}
							this._isAllStandingPointsDisabled = false;
						}
						int userCountNotInStruckAction = base.UserCountNotInStruckAction;
						switch (this.State)
						{
						case BatteringRam.RamState.Stable:
							if (userCountNotInStruckAction > 0)
							{
								this.State = BatteringRam.RamState.Hitting;
								base.SetAbilityOfConditionalFaces(false);
								this._usedPower = userCountNotInStruckAction;
								this._storedPower = 0f;
								this.StartHitAnimationWithProgress((userCountNotInStruckAction - 1) / 2, 0f);
								return;
							}
							break;
						case BatteringRam.RamState.Hitting:
						{
							if (userCountNotInStruckAction <= 0 || this._gate == null || this._gate.IsGateOpen)
							{
								this._batteringRamBody.GetFirstScriptOfType<SynchedMissionObject>().SetAnimationAtChannelSynched("batteringram_idle", 0, 1f);
								this.State = BatteringRam.RamState.Stable;
								base.SetAbilityOfConditionalFaces(true);
								return;
							}
							int num = (userCountNotInStruckAction - 1) / 2;
							float animationParameterAtChannel = this._batteringRamBodySkeleton.GetAnimationParameterAtChannel(0);
							if ((this._usedPower - 1) / 2 != num)
							{
								this.StartHitAnimationWithProgress(num, animationParameterAtChannel);
							}
							this._usedPower = userCountNotInStruckAction;
							this._storedPower += (float)this._usedPower * dt;
							float num2 = ((num == 3) ? 0.5f : ((num == 2) ? 0.56f : 0.58f));
							string text = ((num == 3) ? "batteringram_fire" : ((num == 2) ? "batteringram_fire_weak" : "batteringram_fire_weakest"));
							if (animationParameterAtChannel >= num2)
							{
								MatrixFrame globalFrame = base.GameEntity.GetGlobalFrame();
								float num3 = this._storedPower * this.DamageMultiplier;
								num3 /= animationParameterAtChannel * MBAnimation.GetAnimationDuration(text);
								this._gate.DestructionComponent.TriggerOnHit(base.PilotAgent, (int)num3, globalFrame.origin, globalFrame.rotation.f, in MissionWeapon.Invalid, -1, this);
								this.State = BatteringRam.RamState.AfterHit;
								return;
							}
							break;
						}
						case BatteringRam.RamState.AfterHit:
							if (this._batteringRamBodySkeleton.GetAnimationParameterAtChannel(0) > 0.999f)
							{
								this.State = BatteringRam.RamState.Stable;
								base.SetAbilityOfConditionalFaces(true);
							}
							break;
						default:
							return;
						}
					}
				}
			}
		}

		// Token: 0x06002E9D RID: 11933 RVA: 0x000B4758 File Offset: 0x000B2958
		private void StartHitAnimationWithProgress(int powerStage, float progress)
		{
			string text = ((powerStage == 2) ? "batteringram_fire" : ((powerStage == 1) ? "batteringram_fire_weak" : "batteringram_fire_weakest"));
			this._batteringRamBody.GetFirstScriptOfType<SynchedMissionObject>().SetAnimationAtChannelSynched(text, 0, 1f);
			if (progress > 0f)
			{
				this._batteringRamBody.GetFirstScriptOfType<SynchedMissionObject>().SetAnimationChannelParameterSynched(0, progress);
			}
			foreach (StandingPoint standingPoint in base.StandingPoints)
			{
				if (standingPoint.HasUser && standingPoint.GameEntity.HasTag("pull"))
				{
					ActionIndexCache actionCodeForStandingPoint = this.GetActionCodeForStandingPoint(standingPoint, powerStage);
					if (!standingPoint.UserAgent.SetActionChannel(1, in actionCodeForStandingPoint, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, progress, false, -0.2f, 0, true) && standingPoint.UserAgent.Controller == AgentControllerType.AI)
					{
						standingPoint.UserAgent.StopUsingGameObject(false, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
					}
				}
			}
		}

		// Token: 0x06002E9E RID: 11934 RVA: 0x000B4868 File Offset: 0x000B2A68
		private void UpdateHitAnimationWithProgress(int powerStage, float progress)
		{
			foreach (StandingPoint standingPoint in base.StandingPoints)
			{
				if (standingPoint.HasUser && standingPoint.GameEntity.HasTag("pull"))
				{
					ActionIndexCache actionCodeForStandingPoint = this.GetActionCodeForStandingPoint(standingPoint, powerStage);
					if (standingPoint.UserAgent.GetCurrentAction(1) == actionCodeForStandingPoint)
					{
						standingPoint.UserAgent.SetCurrentActionProgress(1, progress);
					}
				}
			}
		}

		// Token: 0x06002E9F RID: 11935 RVA: 0x000B48FC File Offset: 0x000B2AFC
		private ActionIndexCache GetActionCodeForStandingPoint(StandingPoint standingPoint, int powerStage)
		{
			bool flag = standingPoint.GameEntity.HasTag("right");
			ActionIndexCache actionIndexCache = ActionIndexCache.act_none;
			switch (powerStage)
			{
			case 0:
				actionIndexCache = (flag ? ActionIndexCache.act_usage_batteringram_left_slowest : ActionIndexCache.act_usage_batteringram_right_slowest);
				break;
			case 1:
				actionIndexCache = (flag ? ActionIndexCache.act_usage_batteringram_left_slower : ActionIndexCache.act_usage_batteringram_right_slower);
				break;
			case 2:
				actionIndexCache = (flag ? ActionIndexCache.act_usage_batteringram_left : ActionIndexCache.act_usage_batteringram_right);
				break;
			default:
				Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Objects\\Siege\\BatteringRam.cs", "GetActionCodeForStandingPoint", 583);
				break;
			}
			return actionIndexCache;
		}

		// Token: 0x06002EA0 RID: 11936 RVA: 0x000B4987 File Offset: 0x000B2B87
		public override UsableMachineAIBase CreateAIBehaviorObject()
		{
			return new BatteringRamAI(this);
		}

		// Token: 0x06002EA1 RID: 11937 RVA: 0x000B4990 File Offset: 0x000B2B90
		protected internal override void OnMissionReset()
		{
			base.OnMissionReset();
			this._state = BatteringRam.RamState.Stable;
			if (!GameNetwork.IsClientOrReplay)
			{
				base.SetAbilityOfConditionalFaces(true);
			}
			this._hasArrivedAtTarget = false;
			this._batteringRamBodySkeleton.SetAnimationAtChannel("batteringram_idle", 0, 1f, 0f, 0f);
			foreach (StandingPoint standingPoint in base.StandingPoints)
			{
				standingPoint.IsDeactivated = !standingPoint.GameEntity.HasTag("move");
			}
		}

		// Token: 0x06002EA2 RID: 11938 RVA: 0x000B4A38 File Offset: 0x000B2C38
		public override void WriteToNetwork()
		{
			base.WriteToNetwork();
			GameNetworkMessage.WriteBoolToPacket(this.HasArrivedAtTarget);
			GameNetworkMessage.WriteIntToPacket((int)this.State, CompressionMission.BatteringRamStateCompressionInfo);
			GameNetworkMessage.WriteFloatToPacket(this.MovementComponent.GetTotalDistanceTraveledForPathTracker(), CompressionBasic.PositionCompressionInfo);
		}

		// Token: 0x170008B0 RID: 2224
		// (get) Token: 0x06002EA3 RID: 11939 RVA: 0x000B4A70 File Offset: 0x000B2C70
		public override bool IsDeactivated
		{
			get
			{
				return this._gate == null || this._gate.IsDestroyed || (this._gate.State == CastleGate.GateState.Open && this.HasArrivedAtTarget) || base.IsDeactivated;
			}
		}

		// Token: 0x06002EA4 RID: 11940 RVA: 0x000B4AA4 File Offset: 0x000B2CA4
		public void HighlightPath()
		{
			this.MovementComponent.HighlightPath();
		}

		// Token: 0x06002EA5 RID: 11941 RVA: 0x000B4AB4 File Offset: 0x000B2CB4
		public void SwitchGhostEntityMovementMode(bool isGhostEnabled)
		{
			if (isGhostEnabled)
			{
				if (!this._isGhostMovementOn)
				{
					base.RemoveComponent(this.MovementComponent);
					this.SetUpGhostEntity();
					this.GhostEntityMove = true;
					SiegeWeaponMovementComponent component = base.GetComponent<SiegeWeaponMovementComponent>();
					component.GhostEntitySpeedMultiplier *= 3f;
					component.SetGhostVisibility(true);
				}
				this._isGhostMovementOn = true;
				return;
			}
			if (this._isGhostMovementOn)
			{
				base.RemoveComponent(this.MovementComponent);
				PathLastNodeFixer component2 = base.GetComponent<PathLastNodeFixer>();
				base.RemoveComponent(component2);
				this.AddRegularMovementComponent();
				this.MovementComponent.SetGhostVisibility(false);
			}
			this._isGhostMovementOn = false;
		}

		// Token: 0x06002EA6 RID: 11942 RVA: 0x000B4B48 File Offset: 0x000B2D48
		private void SetUpGhostEntity()
		{
			PathLastNodeFixer pathLastNodeFixer = new PathLastNodeFixer
			{
				PathHolder = this
			};
			base.AddComponent(pathLastNodeFixer);
			this.MovementComponent = new SiegeWeaponMovementComponent
			{
				PathEntityName = this.PathEntity,
				MainObject = this,
				GhostEntitySpeedMultiplier = this.GhostEntitySpeedMultiplier
			};
			base.AddComponent(this.MovementComponent);
			this.MovementComponent.SetupGhostEntity();
		}

		// Token: 0x06002EA7 RID: 11943 RVA: 0x000B4BAA File Offset: 0x000B2DAA
		public override TextObject GetDescriptionText(WeakGameEntity gameEntity)
		{
			return new TextObject("{=MaBSSg7I}Battering Ram", null);
		}

		// Token: 0x06002EA8 RID: 11944 RVA: 0x000B4BB8 File Offset: 0x000B2DB8
		public override TextObject GetActionTextForStandingPoint(UsableMissionObject usableGameObject)
		{
			TextObject textObject = (usableGameObject.GameEntity.HasTag("pull") ? new TextObject("{=1cnJtNTt}{KEY} Pull", null) : new TextObject("{=rwZAZSvX}{KEY} Move", null));
			textObject.SetTextVariable("KEY", HyperlinkTexts.GetKeyHyperlinkText(HotKeyManager.GetHotKeyId("CombatHotKeyCategory", 13), 1f));
			return textObject;
		}

		// Token: 0x06002EA9 RID: 11945 RVA: 0x000B4C14 File Offset: 0x000B2E14
		public override OrderType GetOrder(BattleSideEnum side)
		{
			if (base.IsDestroyed)
			{
				return OrderType.None;
			}
			if (side != BattleSideEnum.Attacker)
			{
				return OrderType.AttackEntity;
			}
			if (!this.HasCompletedAction())
			{
				return OrderType.FollowEntity;
			}
			return OrderType.Use;
		}

		// Token: 0x06002EAA RID: 11946 RVA: 0x000B4C34 File Offset: 0x000B2E34
		public override TargetFlags GetTargetFlags()
		{
			TargetFlags targetFlags = TargetFlags.None;
			if (base.UserCountNotInStruckAction > 0)
			{
				targetFlags |= TargetFlags.IsMoving;
			}
			targetFlags |= TargetFlags.IsSiegeEngine;
			targetFlags |= TargetFlags.IsAttacker;
			if (this.Side == BattleSideEnum.Attacker && DebugSiegeBehavior.DebugDefendState == DebugSiegeBehavior.DebugStateDefender.DebugDefendersToRam)
			{
				targetFlags |= TargetFlags.DebugThreat;
			}
			if (this.HasCompletedAction() || base.IsDestroyed || this.IsDeactivated)
			{
				targetFlags |= TargetFlags.NotAThreat;
			}
			return targetFlags;
		}

		// Token: 0x06002EAB RID: 11947 RVA: 0x000B4C90 File Offset: 0x000B2E90
		public override float GetTargetValue(List<Vec3> weaponPos)
		{
			return 300f * base.GetUserMultiplierOfWeapon() * this.GetDistanceMultiplierOfWeapon(weaponPos[0]) * base.GetHitPointMultiplierOfWeapon();
		}

		// Token: 0x06002EAC RID: 11948 RVA: 0x000B4CB4 File Offset: 0x000B2EB4
		protected override float GetDistanceMultiplierOfWeapon(Vec3 weaponPos)
		{
			float minimumDistanceBetweenPositions = this.GetMinimumDistanceBetweenPositions(weaponPos);
			if (minimumDistanceBetweenPositions < 100f)
			{
				return 1f;
			}
			if (minimumDistanceBetweenPositions < 625f)
			{
				return 0.8f;
			}
			return 0.6f;
		}

		// Token: 0x06002EAD RID: 11949 RVA: 0x000B4CEA File Offset: 0x000B2EEA
		public void SetSpawnedFromSpawner()
		{
			this._spawnedFromSpawner = true;
		}

		// Token: 0x06002EAE RID: 11950 RVA: 0x000B4CF4 File Offset: 0x000B2EF4
		public void AssignParametersFromSpawner(string gateTag, string sideTag, int bridgeNavMeshID1, int bridgeNavMeshID2, int ditchNavMeshID1, int ditchNavMeshID2, int groundToBridgeNavMeshID1, int groundToBridgeNavMeshID2, string pathEntityName)
		{
			this._gateTag = gateTag;
			this._sideTag = sideTag;
			this._bridgeNavMeshID1 = bridgeNavMeshID1;
			this._bridgeNavMeshID2 = bridgeNavMeshID2;
			this._ditchNavMeshID1 = ditchNavMeshID1;
			this._ditchNavMeshID2 = ditchNavMeshID2;
			this._groundToBridgeNavMeshID1 = groundToBridgeNavMeshID1;
			this._groundToBridgeNavMeshID2 = groundToBridgeNavMeshID2;
			this._pathEntityName = pathEntityName;
		}

		// Token: 0x06002EAF RID: 11951 RVA: 0x000B4D48 File Offset: 0x000B2F48
		public override void OnAfterReadFromNetwork(ValueTuple<BaseSynchedMissionObjectReadableRecord, ISynchedMissionObjectReadableRecord> synchedMissionObjectReadableRecord, bool allowVisibilityUpdate = true)
		{
			base.OnAfterReadFromNetwork(synchedMissionObjectReadableRecord, allowVisibilityUpdate);
			BatteringRam.BatteringRamRecord batteringRamRecord = (BatteringRam.BatteringRamRecord)synchedMissionObjectReadableRecord.Item2;
			this.HasArrivedAtTarget = batteringRamRecord.HasArrivedAtTarget;
			this._state = (BatteringRam.RamState)batteringRamRecord.State;
			float num = batteringRamRecord.TotalDistanceTraveled;
			num += 0.05f;
			this.MovementComponent.SetTotalDistanceTraveledForPathTracker(num);
			this.MovementComponent.SetTargetFrameForPathTracker();
		}

		// Token: 0x06002EB0 RID: 11952 RVA: 0x000B4DAA File Offset: 0x000B2FAA
		public bool GetNavmeshFaceIds(out List<int> navmeshFaceIds)
		{
			navmeshFaceIds = null;
			return false;
		}

		// Token: 0x04001299 RID: 4761
		private string _pathEntityName = "Path";

		// Token: 0x0400129A RID: 4762
		private const string PullStandingPointTag = "pull";

		// Token: 0x0400129B RID: 4763
		private const string RightStandingPointTag = "right";

		// Token: 0x0400129C RID: 4764
		private const string IdleAnimation = "batteringram_idle";

		// Token: 0x0400129D RID: 4765
		private const string KnockAnimation = "batteringram_fire";

		// Token: 0x0400129E RID: 4766
		private const string KnockSlowerAnimation = "batteringram_fire_weak";

		// Token: 0x0400129F RID: 4767
		private const string KnockSlowestAnimation = "batteringram_fire_weakest";

		// Token: 0x040012A0 RID: 4768
		private const float KnockAnimationHitProgress = 0.5f;

		// Token: 0x040012A1 RID: 4769
		private const float KnockSlowerAnimationHitProgress = 0.56f;

		// Token: 0x040012A2 RID: 4770
		private const float KnockSlowestAnimationHitProgress = 0.58f;

		// Token: 0x040012A3 RID: 4771
		private string _gateTag = "gate";

		// Token: 0x040012A4 RID: 4772
		public bool GhostEntityMove = true;

		// Token: 0x040012A5 RID: 4773
		public float GhostEntitySpeedMultiplier = 1f;

		// Token: 0x040012A6 RID: 4774
		private string _sideTag;

		// Token: 0x040012A7 RID: 4775
		private FormationAI.BehaviorSide _weaponSide;

		// Token: 0x040012A8 RID: 4776
		public float WheelDiameter = 1.3f;

		// Token: 0x040012A9 RID: 4777
		public int GateNavMeshId = 7;

		// Token: 0x040012AA RID: 4778
		public int DisabledNavMeshID = 8;

		// Token: 0x040012AB RID: 4779
		private int _bridgeNavMeshID1 = 8;

		// Token: 0x040012AC RID: 4780
		private int _bridgeNavMeshID2 = 8;

		// Token: 0x040012AD RID: 4781
		private int _ditchNavMeshID1 = 9;

		// Token: 0x040012AE RID: 4782
		private int _ditchNavMeshID2 = 10;

		// Token: 0x040012AF RID: 4783
		private int _groundToBridgeNavMeshID1 = 12;

		// Token: 0x040012B0 RID: 4784
		private int _groundToBridgeNavMeshID2 = 13;

		// Token: 0x040012B1 RID: 4785
		public int NavMeshIdToDisableOnDestination = -1;

		// Token: 0x040012B2 RID: 4786
		public float MinSpeed = 0.5f;

		// Token: 0x040012B3 RID: 4787
		public float MaxSpeed = 1f;

		// Token: 0x040012B4 RID: 4788
		public float DamageMultiplier = 10f;

		// Token: 0x040012B5 RID: 4789
		private int _usedPower;

		// Token: 0x040012B6 RID: 4790
		private float _storedPower;

		// Token: 0x040012B7 RID: 4791
		private List<StandingPoint> _pullStandingPoints;

		// Token: 0x040012B8 RID: 4792
		private List<MatrixFrame> _pullStandingPointLocalIKFrames;

		// Token: 0x040012B9 RID: 4793
		private GameEntity _ditchFillDebris;

		// Token: 0x040012BA RID: 4794
		private GameEntity _batteringRamBody;

		// Token: 0x040012BB RID: 4795
		private Skeleton _batteringRamBodySkeleton;

		// Token: 0x040012BC RID: 4796
		private bool _isGhostMovementOn;

		// Token: 0x040012BD RID: 4797
		private bool _isAllStandingPointsDisabled;

		// Token: 0x040012BE RID: 4798
		private BatteringRam.RamState _state;

		// Token: 0x040012BF RID: 4799
		private CastleGate _gate;

		// Token: 0x040012C0 RID: 4800
		private bool _hasArrivedAtTarget;

		// Token: 0x0200060C RID: 1548
		[DefineSynchedMissionObjectType(typeof(BatteringRam))]
		public struct BatteringRamRecord : ISynchedMissionObjectReadableRecord
		{
			// Token: 0x17000AA7 RID: 2727
			// (get) Token: 0x06003F8B RID: 16267 RVA: 0x000F7901 File Offset: 0x000F5B01
			// (set) Token: 0x06003F8C RID: 16268 RVA: 0x000F7909 File Offset: 0x000F5B09
			public bool HasArrivedAtTarget { get; private set; }

			// Token: 0x17000AA8 RID: 2728
			// (get) Token: 0x06003F8D RID: 16269 RVA: 0x000F7912 File Offset: 0x000F5B12
			// (set) Token: 0x06003F8E RID: 16270 RVA: 0x000F791A File Offset: 0x000F5B1A
			public int State { get; private set; }

			// Token: 0x17000AA9 RID: 2729
			// (get) Token: 0x06003F8F RID: 16271 RVA: 0x000F7923 File Offset: 0x000F5B23
			// (set) Token: 0x06003F90 RID: 16272 RVA: 0x000F792B File Offset: 0x000F5B2B
			public float TotalDistanceTraveled { get; private set; }

			// Token: 0x06003F91 RID: 16273 RVA: 0x000F7934 File Offset: 0x000F5B34
			public bool ReadFromNetwork(ref bool bufferReadValid)
			{
				this.HasArrivedAtTarget = GameNetworkMessage.ReadBoolFromPacket(ref bufferReadValid);
				this.State = GameNetworkMessage.ReadIntFromPacket(CompressionMission.BatteringRamStateCompressionInfo, ref bufferReadValid);
				this.TotalDistanceTraveled = GameNetworkMessage.ReadFloatFromPacket(CompressionBasic.PositionCompressionInfo, ref bufferReadValid);
				return bufferReadValid;
			}
		}

		// Token: 0x0200060D RID: 1549
		public enum RamState
		{
			// Token: 0x0400204D RID: 8269
			Stable,
			// Token: 0x0400204E RID: 8270
			Hitting,
			// Token: 0x0400204F RID: 8271
			AfterHit,
			// Token: 0x04002050 RID: 8272
			NumberOfStates
		}
	}
}
