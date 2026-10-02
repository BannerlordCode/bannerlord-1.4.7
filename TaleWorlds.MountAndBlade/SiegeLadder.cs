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
	// Token: 0x0200034F RID: 847
	public class SiegeLadder : SiegeWeapon, IPrimarySiegeWeapon, IOrderableWithInteractionArea, IOrderable, ISpawnable
	{
		// Token: 0x170008FF RID: 2303
		// (get) Token: 0x06003023 RID: 12323 RVA: 0x000BF3BE File Offset: 0x000BD5BE
		// (set) Token: 0x06003024 RID: 12324 RVA: 0x000BF3C6 File Offset: 0x000BD5C6
		public GameEntity InitialWaitPosition { get; private set; }

		// Token: 0x17000900 RID: 2304
		// (get) Token: 0x06003025 RID: 12325 RVA: 0x000BF3CF File Offset: 0x000BD5CF
		// (set) Token: 0x06003026 RID: 12326 RVA: 0x000BF3D7 File Offset: 0x000BD5D7
		public int OnWallNavMeshId { get; private set; }

		// Token: 0x17000901 RID: 2305
		// (get) Token: 0x06003027 RID: 12327 RVA: 0x000BF3E0 File Offset: 0x000BD5E0
		public MissionObject TargetCastlePosition
		{
			get
			{
				return this._targetWallSegment;
			}
		}

		// Token: 0x17000902 RID: 2306
		// (get) Token: 0x06003028 RID: 12328 RVA: 0x000BF3E8 File Offset: 0x000BD5E8
		// (set) Token: 0x06003029 RID: 12329 RVA: 0x000BF3F0 File Offset: 0x000BD5F0
		public FormationAI.BehaviorSide WeaponSide { get; private set; }

		// Token: 0x17000903 RID: 2307
		// (get) Token: 0x0600302A RID: 12330 RVA: 0x000BF3F9 File Offset: 0x000BD5F9
		public float SiegeWeaponPriority
		{
			get
			{
				return 8f;
			}
		}

		// Token: 0x0600302B RID: 12331 RVA: 0x000BF400 File Offset: 0x000BD600
		public override SiegeEngineType GetSiegeEngineType()
		{
			return DefaultSiegeEngineTypes.Ladder;
		}

		// Token: 0x0600302C RID: 12332 RVA: 0x000BF408 File Offset: 0x000BD608
		protected internal override void OnInit()
		{
			base.OnInit();
			this._tickOccasionallyTimer = new Timer(Mission.Current.CurrentTime, 0.2f + MBRandom.RandomFloat * 0.05f, true);
			this._aiBarriers = base.Scene.FindEntitiesWithTag(this.BarrierTagToRemove).ToList<GameEntity>();
			if (this.IndestructibleMerlonsTag != string.Empty)
			{
				foreach (WeakGameEntity weakGameEntity in base.Scene.FindWeakEntitiesWithTag(this.IndestructibleMerlonsTag))
				{
					DestructableComponent firstScriptOfType = weakGameEntity.GetFirstScriptOfType<DestructableComponent>();
					firstScriptOfType.SetDisabled(false);
					firstScriptOfType.CanBeDestroyedInitially = false;
				}
			}
			this._attackerStandingPoints = base.GameEntity.CollectScriptComponentsWithTagIncludingChildrenRecursive<StandingPoint>(this.AttackerTag);
			this._pushingWithForkStandingPoint = base.GameEntity.CollectScriptComponentsWithTagIncludingChildrenRecursive<StandingPointWithWeaponRequirement>(this.DefenderTag).FirstOrDefault<StandingPointWithWeaponRequirement>();
			this._pushingWithForkStandingPoint.AddComponent(new DropExtraWeaponOnStopUsageComponent());
			this._forkPickUpStandingPoint = base.GameEntity.CollectScriptComponentsWithTagIncludingChildrenRecursive<StandingPointWithWeaponRequirement>(this.AmmoPickUpTag).FirstOrDefault<StandingPointWithWeaponRequirement>();
			StandingPointWithWeaponRequirement forkPickUpStandingPoint = this._forkPickUpStandingPoint;
			if (forkPickUpStandingPoint != null)
			{
				forkPickUpStandingPoint.SetUsingBattleSide(BattleSideEnum.Defender);
			}
			this._ladderParticleObject = base.GameEntity.CollectScriptComponentsWithTagIncludingChildrenRecursive<SynchedMissionObject>("particles").FirstOrDefault<SynchedMissionObject>();
			this._forkEntity = base.GameEntity.CollectScriptComponentsWithTagIncludingChildrenRecursive<SynchedMissionObject>("push_fork").FirstOrDefault<SynchedMissionObject>();
			if (base.StandingPoints != null)
			{
				foreach (StandingPoint standingPoint in base.StandingPoints)
				{
					if (!standingPoint.GameEntity.HasTag(this.AmmoPickUpTag))
					{
						standingPoint.AddComponent(new ResetAnimationOnStopUsageComponent(ActionIndexCache.act_none, false));
						standingPoint.IsDeactivated = true;
					}
				}
			}
			this._forkItem = Game.Current.ObjectManager.GetObject<ItemObject>(this.PushForkItemID);
			this._pushingWithForkStandingPoint.InitRequiredWeapon(this._forkItem);
			this._forkPickUpStandingPoint.InitGivenWeapon(this._forkItem);
			WeakGameEntity weakGameEntity2 = base.GameEntity.CollectChildrenEntitiesWithTag(this.upStateEntityTag)[0];
			List<SynchedMissionObject> list = base.GameEntity.CollectScriptComponentsWithTagIncludingChildrenRecursive<SynchedMissionObject>(this.downStateEntityTag);
			this._ladderObject = list[0];
			this._ladderSkeleton = this._ladderObject.GameEntity.Skeleton;
			list = base.GameEntity.CollectScriptComponentsWithTagIncludingChildrenRecursive<SynchedMissionObject>(this.BodyTag);
			this._ladderBodyObject = list[0];
			list = base.GameEntity.CollectScriptComponentsWithTagIncludingChildrenRecursive<SynchedMissionObject>(this.CollisionBodyTag);
			this._ladderCollisionBodyObject = list[0];
			this._ladderDownFrame = this._ladderObject.GameEntity.GetFrame();
			this._turningAngle = this._downStateRotationRadian - this._ladderDownFrame.rotation.GetEulerAngles().x;
			this._ladderDownFrame.rotation.RotateAboutSide(this._turningAngle);
			this._ladderObject.GameEntity.SetFrame(ref this._ladderDownFrame, true);
			MatrixFrame frame = weakGameEntity2.GetFrame();
			frame.rotation = Mat3.Identity;
			frame.rotation.RotateAboutSide(this._upStateRotationRadian);
			this._ladderUpFrame = frame;
			this._ladderUpFrame = this._ladderObject.GameEntity.Parent.GetFrame().TransformToLocal(in this._ladderUpFrame);
			this._ladderInitialGlobalFrame = this._ladderObject.GameEntity.GetGlobalFrame();
			this._attackerStandingPointLocalIKFrames = new MatrixFrame[this._attackerStandingPoints.Count];
			MatrixFrame frame2 = this._ladderObject.GameEntity.Parent.GetFrame();
			MatrixFrame matrixFrame = frame2;
			matrixFrame.rotation.RotateAboutForward(this._turningAngle);
			this.State = this.initialState;
			for (int i = 0; i < this._attackerStandingPoints.Count; i++)
			{
				MatrixFrame matrixFrame2 = this._attackerStandingPoints[i].GameEntity.GetFrame();
				matrixFrame2 = matrixFrame.TransformToParent(in matrixFrame2);
				matrixFrame2 = frame2.TransformToLocal(in matrixFrame2);
				this._attackerStandingPoints[i].GameEntity.SetFrame(ref matrixFrame2, true);
				this._attackerStandingPointLocalIKFrames[i] = this._attackerStandingPoints[i].GameEntity.GetGlobalFrame().TransformToLocal(in this._ladderInitialGlobalFrame);
				this._attackerStandingPoints[i].AddComponent(new ClearHandInverseKinematicsOnStopUsageComponent());
			}
			this.CalculateNavigationAndPhysics();
			this.InitialWaitPosition = TaleWorlds.Engine.GameEntity.CreateFromWeakEntity(base.GameEntity.GetFirstChildEntityWithTag(this.InitialWaitPositionTag));
			foreach (WeakGameEntity weakGameEntity3 in base.Scene.FindWeakEntitiesWithTag(this._targetWallSegmentTag))
			{
				WallSegment firstScriptOfType2 = weakGameEntity3.GetFirstScriptOfType<WallSegment>();
				if (firstScriptOfType2 != null)
				{
					this._targetWallSegment = firstScriptOfType2;
					this._targetWallSegment.AttackerSiegeWeapon = this;
					break;
				}
			}
			string sideTag = this._sideTag;
			if (!(sideTag == "left"))
			{
				if (!(sideTag == "middle"))
				{
					if (!(sideTag == "right"))
					{
						this.WeaponSide = FormationAI.BehaviorSide.Middle;
					}
					else
					{
						this.WeaponSide = FormationAI.BehaviorSide.Right;
					}
				}
				else
				{
					this.WeaponSide = FormationAI.BehaviorSide.Middle;
				}
			}
			else
			{
				this.WeaponSide = FormationAI.BehaviorSide.Left;
			}
			base.SetForcedUse(false);
			LadderQueueManager[] array = base.GameEntity.GetScriptComponents<LadderQueueManager>().ToArray<LadderQueueManager>();
			MatrixFrame globalFrame = base.GameEntity.GetGlobalFrame();
			MatrixFrame matrixFrame3 = this._ladderObject.GameEntity.GetGlobalFrame();
			MatrixFrame matrixFrame4 = globalFrame.TransformToLocal(in matrixFrame3);
			int num = 0;
			int num2 = 1;
			for (int j = base.GameEntity.Name.Length - 1; j >= 0; j--)
			{
				if (char.IsDigit(base.GameEntity.Name[j]))
				{
					num += (int)(base.GameEntity.Name[j] - '0') * num2;
					num2 *= 10;
				}
				else if (num > 0)
				{
					break;
				}
			}
			if (array.Length != 0)
			{
				this._queueManagerForAttackers = array[0];
				this._queueManagerForAttackers.Initialize(this.OnWallNavMeshId, matrixFrame4, -matrixFrame4.rotation.f, BattleSideEnum.Attacker, 3, 2.3561945f, 2f, 0.8f, 6f, 5f, false, 0.8f, (float)num, 5f, false, -2, -2, num, 2);
			}
			if (array.Length > 1 && this._pushingWithForkStandingPoint != null)
			{
				this._queueManagerForDefenders = array[1];
				MatrixFrame matrixFrame5 = this._pushingWithForkStandingPoint.GameEntity.GetGlobalFrame();
				matrixFrame5.rotation.RotateAboutSide(1.5707964f);
				matrixFrame5.origin -= matrixFrame5.rotation.u;
				matrixFrame3 = base.GameEntity.GetGlobalFrame();
				matrixFrame5 = matrixFrame3.TransformToLocal(in matrixFrame5);
				this._queueManagerForDefenders.Initialize(this.OnWallNavMeshId, matrixFrame5, matrixFrame4.rotation.f, BattleSideEnum.Defender, 1, 2.8274333f, 0.5f, 0.8f, 6f, 5f, true, 0.8f, float.MaxValue, 5f, false, -2, -2, 0, 0);
			}
			base.GameEntity.Scene.MarkFacesWithIdAsLadder(this.OnWallNavMeshId, true);
			this.EnemyRangeToStopUsing = 0f;
			this._idleAnimationIndex = MBAnimation.GetAnimationIndexWithName(this.IdleAnimation);
			this._raiseAnimationIndex = MBAnimation.GetAnimationIndexWithName(this.RaiseAnimation);
			this._raiseAnimationWithoutRootBoneIndex = MBAnimation.GetAnimationIndexWithName(this.RaiseAnimationWithoutRootBone);
			this._pushBackAnimationIndex = MBAnimation.GetAnimationIndexWithName(this.PushBackAnimation);
			this._pushBackAnimationWithoutRootBoneIndex = MBAnimation.GetAnimationIndexWithName(this.PushBackAnimationWithoutRootBone);
			this._trembleWallHeavyAnimationIndex = MBAnimation.GetAnimationIndexWithName(this.TrembleWallHeavyAnimation);
			this._trembleWallLightAnimationIndex = MBAnimation.GetAnimationIndexWithName(this.TrembleWallLightAnimation);
			this._trembleGroundAnimationIndex = MBAnimation.GetAnimationIndexWithName(this.TrembleGroundAnimation);
			this.SetUpStateVisibility(false);
			base.SetScriptComponentToTick(this.GetTickRequirement());
			bool flag = false;
			foreach (WeakGameEntity weakGameEntity4 in this._ladderObject.GameEntity.GetEntityAndChildren())
			{
				PhysicsShape bodyShape = weakGameEntity4.GetBodyShape();
				if (bodyShape != null)
				{
					PhysicsShape.AddPreloadQueueWithName(bodyShape.GetName(), weakGameEntity4.GetGlobalScale());
					flag = true;
				}
			}
			if (flag)
			{
				PhysicsShape.ProcessPreloadQueue();
			}
		}

		// Token: 0x0600302D RID: 12333 RVA: 0x000BFCDC File Offset: 0x000BDEDC
		private float GetCurrentLadderAngularSpeed(int animationIndex)
		{
			float animationParameterAtChannel = this._ladderSkeleton.GetAnimationParameterAtChannel(0);
			MatrixFrame boneEntitialFrameWithIndex = this._ladderSkeleton.GetBoneEntitialFrameWithIndex(0);
			if (animationParameterAtChannel <= 0.01f)
			{
				return 0f;
			}
			this._ladderSkeleton.SetAnimationParameterAtChannel(0, animationParameterAtChannel - 0.01f);
			this._ladderSkeleton.TickAnimationsAndForceUpdate(0.0001f, this._ladderObject.GameEntity.GetGlobalFrame(), false);
			MatrixFrame boneEntitialFrameWithIndex2 = this._ladderSkeleton.GetBoneEntitialFrameWithIndex(0);
			Vec2 vec = new Vec2(boneEntitialFrameWithIndex.rotation.f.y, boneEntitialFrameWithIndex.rotation.f.z);
			Vec2 vec2 = new Vec2(boneEntitialFrameWithIndex2.rotation.f.y, boneEntitialFrameWithIndex2.rotation.f.z);
			return (vec.RotationInRadians - vec2.RotationInRadians) / (MBAnimation.GetAnimationDuration(animationIndex) * 0.01f);
		}

		// Token: 0x0600302E RID: 12334 RVA: 0x000BFDC0 File Offset: 0x000BDFC0
		private void OnLadderStateChange()
		{
			WeakGameEntity gameEntity = this._ladderObject.GameEntity;
			if (this.State != SiegeLadder.LadderState.OnWall)
			{
				this.SetVisibilityOfAIBarriers(true);
			}
			switch (this.State)
			{
			case SiegeLadder.LadderState.OnLand:
				this._animationState = SiegeLadder.LadderAnimationState.Static;
				return;
			case SiegeLadder.LadderState.FallToLand:
				if (this._ladderSkeleton.GetAnimationIndexAtChannel(0) != this._trembleGroundAnimationIndex)
				{
					gameEntity.SetFrame(ref this._ladderDownFrame, true);
					this._ladderSkeleton.SetAnimationAtChannel(this._trembleGroundAnimationIndex, 0, 1f, -1f, 0f);
					this._animationState = SiegeLadder.LadderAnimationState.Static;
				}
				if (!GameNetwork.IsClientOrReplay)
				{
					this.State = SiegeLadder.LadderState.OnLand;
					return;
				}
				break;
			case SiegeLadder.LadderState.BeingRaised:
			case SiegeLadder.LadderState.BeingPushedBack:
				break;
			case SiegeLadder.LadderState.BeingRaisedStartFromGround:
			{
				this._animationState = SiegeLadder.LadderAnimationState.Animated;
				MatrixFrame frame = gameEntity.GetFrame();
				frame.rotation.RotateAboutSide(-1.5707964f);
				gameEntity.SetFrame(ref frame, true);
				this._ladderSkeleton.SetAnimationAtChannel(this._raiseAnimationIndex, 0, 1f, -1f, 0f);
				this._ladderSkeleton.ForceUpdateBoneFrames();
				this._lastDotProductOfAnimationAndTargetRotation = -1000f;
				if (!GameNetwork.IsClientOrReplay)
				{
					this._currentActionAgentCount = 1;
					this.State = SiegeLadder.LadderState.BeingRaised;
					return;
				}
				break;
			}
			case SiegeLadder.LadderState.BeingRaisedStopped:
			{
				this._animationState = SiegeLadder.LadderAnimationState.PhysicallyDynamic;
				MatrixFrame matrixFrame = gameEntity.GetGlobalFrame();
				MatrixFrame matrixFrame2 = this._ladderSkeleton.GetBoneEntitialFrameWithIndex(0);
				MatrixFrame matrixFrame3 = matrixFrame.TransformToParent(in matrixFrame2);
				matrixFrame3.rotation.RotateAboutForward(1.5707964f);
				this._fallAngularSpeed = this.GetCurrentLadderAngularSpeed(this._raiseAnimationIndex);
				float animationParameterAtChannel = this._ladderSkeleton.GetAnimationParameterAtChannel(0);
				gameEntity.SetGlobalFrame(in matrixFrame3, true);
				this._ladderSkeleton.SetAnimationAtChannel(this._raiseAnimationWithoutRootBoneIndex, 0, 1f, -1f, 0f);
				this._ladderSkeleton.SetAnimationParameterAtChannel(0, animationParameterAtChannel);
				this._ladderSkeleton.TickAnimationsAndForceUpdate(0.0001f, gameEntity.GetGlobalFrame(), false);
				this._ladderSkeleton.SetAnimationAtChannel(this._idleAnimationIndex, 0, 1f, -1f, 0f);
				this._ladderObject.SetLocalPositionSmoothStep(ref this._ladderDownFrame.origin);
				if (!GameNetwork.IsClientOrReplay)
				{
					this.State = SiegeLadder.LadderState.BeingPushedBack;
					return;
				}
				break;
			}
			case SiegeLadder.LadderState.OnWall:
				this._animationState = SiegeLadder.LadderAnimationState.Static;
				this.SetVisibilityOfAIBarriers(false);
				return;
			case SiegeLadder.LadderState.FallToWall:
				if (GameNetwork.IsClientOrReplay)
				{
					int animationIndexAtChannel = this._ladderSkeleton.GetAnimationIndexAtChannel(0);
					if (animationIndexAtChannel != this._trembleWallHeavyAnimationIndex && animationIndexAtChannel != this._trembleWallLightAnimationIndex)
					{
						gameEntity.SetFrame(ref this._ladderUpFrame, true);
						this._ladderSkeleton.SetAnimationAtChannel((this._fallAngularSpeed < -0.5f) ? this._trembleWallHeavyAnimationIndex : this._trembleWallLightAnimationIndex, 0, 1f, -1f, 0f);
						this._animationState = SiegeLadder.LadderAnimationState.Static;
						return;
					}
				}
				else
				{
					this.State = SiegeLadder.LadderState.OnWall;
					SynchedMissionObject ladderParticleObject = this._ladderParticleObject;
					if (ladderParticleObject == null)
					{
						return;
					}
					ladderParticleObject.BurstParticlesSynched(false);
					return;
				}
				break;
			case SiegeLadder.LadderState.BeingPushedBackStartFromWall:
				this._animationState = SiegeLadder.LadderAnimationState.Animated;
				this._ladderSkeleton.SetAnimationAtChannel(this._pushBackAnimationIndex, 0, 1f, -1f, 0f);
				this._ladderSkeleton.TickAnimationsAndForceUpdate(0.0001f, gameEntity.GetGlobalFrame(), false);
				this._lastDotProductOfAnimationAndTargetRotation = -1000f;
				if (!GameNetwork.IsClientOrReplay)
				{
					this._currentActionAgentCount = 1;
					this.State = SiegeLadder.LadderState.BeingPushedBack;
					return;
				}
				break;
			case SiegeLadder.LadderState.BeingPushedBackStopped:
			{
				this._animationState = SiegeLadder.LadderAnimationState.PhysicallyDynamic;
				MatrixFrame matrixFrame2 = gameEntity.GetGlobalFrame();
				MatrixFrame matrixFrame = this._ladderSkeleton.GetBoneEntitialFrameWithIndex(0);
				MatrixFrame matrixFrame4 = matrixFrame2.TransformToParent(in matrixFrame);
				matrixFrame4.rotation.RotateAboutForward(1.5707964f);
				this._fallAngularSpeed = this.GetCurrentLadderAngularSpeed(this._pushBackAnimationIndex);
				float animationParameterAtChannel2 = this._ladderSkeleton.GetAnimationParameterAtChannel(0);
				gameEntity.SetGlobalFrame(in matrixFrame4, true);
				this._ladderSkeleton.SetAnimationAtChannel(this._pushBackAnimationWithoutRootBoneIndex, 0, 1f, -1f, 0f);
				this._ladderSkeleton.SetAnimationParameterAtChannel(0, animationParameterAtChannel2);
				this._ladderSkeleton.TickAnimationsAndForceUpdate(0.0001f, gameEntity.GetGlobalFrame(), false);
				this._ladderSkeleton.SetAnimationAtChannel(this._idleAnimationIndex, 0, 1f, -1f, 0f);
				this._ladderObject.SetLocalPositionSmoothStep(ref this._ladderUpFrame.origin);
				if (!GameNetwork.IsClientOrReplay)
				{
					this.State = SiegeLadder.LadderState.BeingRaised;
				}
				this._ladderSkeleton.ForceUpdateBoneFrames();
				break;
			}
			default:
				return;
			}
		}

		// Token: 0x0600302F RID: 12335 RVA: 0x000C01F0 File Offset: 0x000BE3F0
		private void SetVisibilityOfAIBarriers(bool visibility)
		{
			foreach (GameEntity gameEntity in this._aiBarriers)
			{
				gameEntity.SetVisibilityExcludeParents(visibility);
			}
		}

		// Token: 0x17000904 RID: 2308
		// (get) Token: 0x06003030 RID: 12336 RVA: 0x000C0244 File Offset: 0x000BE444
		public int OverTheWallNavMeshID
		{
			get
			{
				return 13;
			}
		}

		// Token: 0x06003031 RID: 12337 RVA: 0x000C0248 File Offset: 0x000BE448
		public override OrderType GetOrder(BattleSideEnum side)
		{
			if (side != BattleSideEnum.Attacker)
			{
				return OrderType.Move;
			}
			return base.GetOrder(side);
		}

		// Token: 0x17000905 RID: 2309
		// (get) Token: 0x06003032 RID: 12338 RVA: 0x000C0257 File Offset: 0x000BE457
		// (set) Token: 0x06003033 RID: 12339 RVA: 0x000C0260 File Offset: 0x000BE460
		public SiegeLadder.LadderState State
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
						GameNetwork.WriteMessage(new SetSiegeLadderState(base.Id, value));
						GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
					}
					this._state = value;
					this.OnLadderStateChange();
					this.CalculateNavigationAndPhysics();
				}
			}
		}

		// Token: 0x06003034 RID: 12340 RVA: 0x000C02B0 File Offset: 0x000BE4B0
		private void CalculateNavigationAndPhysics()
		{
			if (!GameNetwork.IsClientOrReplay)
			{
				bool flag = (this._isNavigationMeshDisabled ? (this.State != SiegeLadder.LadderState.FallToWall && this.State != SiegeLadder.LadderState.OnWall) : (this.State == SiegeLadder.LadderState.OnLand || this.State == SiegeLadder.LadderState.FallToLand));
				if (this._isNavigationMeshDisabled != flag)
				{
					this._isNavigationMeshDisabled = flag;
					this.SetAbilityOfFaces(!this._isNavigationMeshDisabled);
				}
			}
			bool flag2 = (this.State == SiegeLadder.LadderState.BeingRaisedStartFromGround || this.State == SiegeLadder.LadderState.BeingRaised) && this._animationState != SiegeLadder.LadderAnimationState.PhysicallyDynamic;
			bool flag3 = true;
			if (this._isLadderPhysicsDisabled != flag2)
			{
				this._isLadderPhysicsDisabled = flag2;
				this._ladderBodyObject.GameEntity.SetVisibilityExcludeParents(!this._isLadderPhysicsDisabled);
			}
			if (!flag2)
			{
				MatrixFrame globalFrame = this._ladderObject.GameEntity.GetGlobalFrame();
				MatrixFrame boneEntitialFrameWithIndex = this._ladderSkeleton.GetBoneEntitialFrameWithIndex(0);
				MatrixFrame matrixFrame = globalFrame.TransformToParent(in boneEntitialFrameWithIndex);
				matrixFrame.rotation.RotateAboutForward(1.5707964f);
				this._ladderBodyObject.GameEntity.SetGlobalFrame(in matrixFrame, true);
				flag3 = this.State != SiegeLadder.LadderState.BeingPushedBack || matrixFrame.rotation.f.z < 0f;
				if (!flag3)
				{
					float num = MathF.Min(2.01f - matrixFrame.rotation.u.z * 2f, 1f);
					Vec3 vec = new Vec3(1f, num, 1f, -1f);
					matrixFrame.rotation.ApplyScaleLocal(in vec);
					this._ladderCollisionBodyObject.GameEntity.SetGlobalFrame(in matrixFrame, true);
				}
			}
			if (this._isLadderCollisionPhysicsDisabled != flag3)
			{
				this._isLadderCollisionPhysicsDisabled = flag3;
				this._ladderCollisionBodyObject.GameEntity.SetVisibilityExcludeParents(!this._isLadderCollisionPhysicsDisabled);
			}
		}

		// Token: 0x06003035 RID: 12341 RVA: 0x000C047E File Offset: 0x000BE67E
		public bool HasCompletedAction()
		{
			return this.State == SiegeLadder.LadderState.OnWall;
		}

		// Token: 0x06003036 RID: 12342 RVA: 0x000C048C File Offset: 0x000BE68C
		private ActionIndexCache GetActionCodeToUseForStandingPoint(StandingPoint standingPoint)
		{
			WeakGameEntity gameEntity = standingPoint.GameEntity;
			if (!gameEntity.HasTag(this.RightStandingPointTag))
			{
				if (!gameEntity.HasTag(this.FrontStandingPointTag))
				{
					return ActionIndexCache.act_usage_ladder_lift_from_left_2_start;
				}
				return ActionIndexCache.act_usage_ladder_lift_from_left_1_start;
			}
			else
			{
				if (!gameEntity.HasTag(this.FrontStandingPointTag))
				{
					return ActionIndexCache.act_usage_ladder_lift_from_right_2_start;
				}
				return ActionIndexCache.act_usage_ladder_lift_from_right_1_start;
			}
		}

		// Token: 0x06003037 RID: 12343 RVA: 0x000C04E4 File Offset: 0x000BE6E4
		public override bool IsDisabledForBattleSide(BattleSideEnum sideEnum)
		{
			if (sideEnum == BattleSideEnum.Attacker)
			{
				return this.State == SiegeLadder.LadderState.FallToLand || this.State == SiegeLadder.LadderState.FallToWall || this.State == SiegeLadder.LadderState.OnWall || (this.State == SiegeLadder.LadderState.BeingPushedBack && this._animationState != SiegeLadder.LadderAnimationState.PhysicallyDynamic) || this.State == SiegeLadder.LadderState.BeingPushedBackStartFromWall || this.State == SiegeLadder.LadderState.BeingPushedBackStopped;
			}
			return this.State == SiegeLadder.LadderState.OnLand || this.State == SiegeLadder.LadderState.FallToLand || this.State == SiegeLadder.LadderState.BeingRaised || this.State == SiegeLadder.LadderState.BeingRaisedStartFromGround || this.State == SiegeLadder.LadderState.FallToWall;
		}

		// Token: 0x06003038 RID: 12344 RVA: 0x000C0568 File Offset: 0x000BE768
		protected override float GetDetachmentWeightAux(BattleSideEnum side)
		{
			if (side == BattleSideEnum.Attacker)
			{
				return base.GetDetachmentWeightAux(side);
			}
			if (this.IsDisabledForBattleSideAI(side))
			{
				return float.MinValue;
			}
			this.UsableStandingPoints.Clear();
			bool flag = false;
			bool flag2 = false;
			for (int i = 0; i < base.StandingPoints.Count; i++)
			{
				StandingPoint standingPoint = base.StandingPoints[i];
				if (standingPoint.IsUsableBySide(side) && (standingPoint != this._forkPickUpStandingPoint || this._pushingWithForkStandingPoint.IsUsableBySide(side)))
				{
					if (!standingPoint.HasAIMovingTo)
					{
						if (!flag2)
						{
							this.UsableStandingPoints.Clear();
						}
						flag2 = true;
					}
					else if (flag2 || standingPoint.MovingAgent.Formation.Team.Side != side)
					{
						goto IL_00A4;
					}
					flag = true;
					this.UsableStandingPoints.Add(new ValueTuple<int, StandingPoint>(i, standingPoint));
				}
				IL_00A4:;
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
			if (!base.IsDetachmentRecentlyEvaluated)
			{
				return 0.1f;
			}
			return 0.01f;
		}

		// Token: 0x17000906 RID: 2310
		// (get) Token: 0x06003039 RID: 12345 RVA: 0x000C065A File Offset: 0x000BE85A
		public bool HoldLadders
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600303A RID: 12346 RVA: 0x000C065D File Offset: 0x000BE85D
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			return base.GetTickRequirement() | ScriptComponentBehavior.TickRequirement.Tick | ScriptComponentBehavior.TickRequirement.TickParallel;
		}

		// Token: 0x17000907 RID: 2311
		// (get) Token: 0x0600303B RID: 12347 RVA: 0x000C0669 File Offset: 0x000BE869
		public bool SendLadders
		{
			get
			{
				return this.State > SiegeLadder.LadderState.OnLand;
			}
		}

		// Token: 0x0600303C RID: 12348 RVA: 0x000C0674 File Offset: 0x000BE874
		protected internal override void OnTick(float dt)
		{
			base.OnTick(dt);
			if (this._tickOccasionallyTimer.Check(Mission.Current.CurrentTime))
			{
				this.TickRare();
			}
			if (!GameNetwork.IsClientOrReplay && this._forkReappearingTimer != null && this._forkReappearingTimer.Check(Mission.Current.CurrentTime))
			{
				this._forkPickUpStandingPoint.SetIsDeactivatedSynched(false);
				this._forkEntity.SetVisibleSynched(true, false);
			}
			int num = 0;
			int num2 = 0;
			WeakGameEntity gameEntity = this._ladderObject.GameEntity;
			if (!GameNetwork.IsClientOrReplay)
			{
				if (this._queueManagerForAttackers != null)
				{
					if (this._queueManagerForAttackers.IsDeactivated)
					{
						if (this.State == SiegeLadder.LadderState.OnWall)
						{
							this._queueManagerForAttackers.Activate();
						}
					}
					else if (this.State == SiegeLadder.LadderState.OnLand)
					{
						this._queueManagerForAttackers.Deactivate();
					}
				}
				if (this._queueManagerForDefenders != null && this._queueManagerForDefenders.IsDeactivated != (this.State != SiegeLadder.LadderState.OnWall))
				{
					if (this.State != SiegeLadder.LadderState.OnWall)
					{
						this._queueManagerForDefenders.DeactivateImmediate();
					}
					else
					{
						this._queueManagerForDefenders.Activate();
					}
				}
				int animationIndexAtChannel = this._ladderSkeleton.GetAnimationIndexAtChannel(0);
				bool flag = false;
				if (animationIndexAtChannel >= 0)
				{
					flag = animationIndexAtChannel == this._trembleGroundAnimationIndex || animationIndexAtChannel == this._trembleWallHeavyAnimationIndex || animationIndexAtChannel == this._trembleWallLightAnimationIndex;
					if (flag)
					{
						flag = this._ladderSkeleton.GetAnimationParameterAtChannel(0) < 1f;
					}
				}
				num += ((this._pushingWithForkStandingPoint.HasUser && !this._pushingWithForkStandingPoint.UserAgent.IsInBeingStruckAction) ? 1 : 0);
				foreach (StandingPoint standingPoint in this._attackerStandingPoints)
				{
					if (standingPoint.HasUser && !standingPoint.UserAgent.IsInBeingStruckAction)
					{
						num2++;
					}
				}
				foreach (StandingPoint standingPoint2 in base.StandingPoints)
				{
					WeakGameEntity gameEntity2 = standingPoint2.GameEntity;
					if (!gameEntity2.HasTag(this.AmmoPickUpTag))
					{
						bool flag2 = false;
						if ((!standingPoint2.HasUser || standingPoint2.UserAgent.IsInBeingStruckAction) && this.State == SiegeLadder.LadderState.BeingRaised && gameEntity2.HasTag(this.AttackerTag))
						{
							float animationParameterAtChannel = this._ladderSkeleton.GetAnimationParameterAtChannel(0);
							float animationDuration = MBAnimation.GetAnimationDuration(this._ladderSkeleton.GetAnimationIndexAtChannel(0));
							ActionIndexCache actionCodeToUseForStandingPoint = this.GetActionCodeToUseForStandingPoint(standingPoint2);
							int animationIndexOfAction = MBActionSet.GetAnimationIndexOfAction(MBGlobals.GetActionSetWithSuffix(Game.Current.DefaultMonster, false, "_warrior"), in actionCodeToUseForStandingPoint);
							flag2 = animationParameterAtChannel * animationDuration / MathF.Max(MBAnimation.GetAnimationDuration(animationIndexOfAction), 0.01f) > 0.98f;
						}
						if (gameEntity2.HasTag(this.DefenderTag) && !this.CanLadderBePushed())
						{
							Agent userAgent = standingPoint2.UserAgent;
							if (userAgent != null)
							{
								userAgent.SetActionChannel(0, in ActionIndexCache.act_usage_ladder_push_back_stopped, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true);
							}
						}
						standingPoint2.SetIsDeactivatedSynched(flag2 || this.State == SiegeLadder.LadderState.BeingPushedBackStopped || (gameEntity2.HasTag(this.AttackerTag) && (this.State == SiegeLadder.LadderState.OnWall || this.State == SiegeLadder.LadderState.FallToWall || (this.State == SiegeLadder.LadderState.BeingPushedBack && this._animationState != SiegeLadder.LadderAnimationState.PhysicallyDynamic) || this.State == SiegeLadder.LadderState.BeingPushedBackStartFromWall)) || (gameEntity2.HasTag(this.DefenderTag) && (this.State == SiegeLadder.LadderState.OnLand || this._animationState == SiegeLadder.LadderAnimationState.PhysicallyDynamic || this.State == SiegeLadder.LadderState.BeingRaisedStopped || flag || this.State == SiegeLadder.LadderState.FallToLand || this.State == SiegeLadder.LadderState.BeingRaised || this.State == SiegeLadder.LadderState.BeingRaisedStartFromGround || !this.CanLadderBePushed())));
						num = ((this._pushingWithForkStandingPoint.HasUser && !this._pushingWithForkStandingPoint.UserAgent.IsInBeingStruckAction) ? 1 : 0);
					}
				}
				if (this._forkPickUpStandingPoint.HasUser)
				{
					Agent userAgent2 = this._forkPickUpStandingPoint.UserAgent;
					ActionIndexCache currentAction = userAgent2.GetCurrentAction(1);
					if (!(currentAction == ActionIndexCache.act_usage_ladder_pick_up_fork_begin))
					{
						if (currentAction == ActionIndexCache.act_usage_ladder_pick_up_fork_end)
						{
							MissionWeapon missionWeapon = new MissionWeapon(this._forkItem, null, null);
							userAgent2.EquipWeaponToExtraSlotAndWield(ref missionWeapon);
							this._forkPickUpStandingPoint.UserAgent.StopUsingGameObject(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
							this._forkPickUpStandingPoint.SetIsDeactivatedSynched(true);
							this._forkEntity.SetVisibleSynched(false, false);
							this._forkReappearingTimer = new Timer(Mission.Current.CurrentTime, this._forkReappearingDelay, true);
							if (userAgent2.IsAIControlled)
							{
								StandingPoint suitableStandingPointFor = this.GetSuitableStandingPointFor(userAgent2.Team.Side, userAgent2, null, null);
								if (suitableStandingPointFor != null)
								{
									((IDetachment)this).AddAgent(userAgent2, -1, Agent.AIScriptedFrameFlags.None);
									if (userAgent2.Formation != null)
									{
										userAgent2.Formation.DetachUnit(userAgent2, ((IDetachment)this).IsLoose);
										userAgent2.Detachment = this;
										userAgent2.SetDetachmentWeight(this.GetWeightOfStandingPoint(suitableStandingPointFor));
									}
								}
							}
						}
						else if (!this._forkPickUpStandingPoint.UserAgent.SetActionChannel(1, in ActionIndexCache.act_usage_ladder_pick_up_fork_begin, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true))
						{
							this._forkPickUpStandingPoint.UserAgent.StopUsingGameObject(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
						}
					}
				}
				else if (this._forkPickUpStandingPoint.HasAIMovingTo)
				{
					Agent movingAgent = this._forkPickUpStandingPoint.MovingAgent;
					if (movingAgent.Team != null && !this._pushingWithForkStandingPoint.IsUsableBySide(movingAgent.Team.Side))
					{
						movingAgent.StopUsingGameObject(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
					}
				}
			}
			switch (this.State)
			{
			case SiegeLadder.LadderState.OnLand:
			case SiegeLadder.LadderState.FallToLand:
				if (!GameNetwork.IsClientOrReplay && num2 > 0)
				{
					this.State = SiegeLadder.LadderState.BeingRaisedStartFromGround;
				}
				break;
			case SiegeLadder.LadderState.BeingRaised:
			case SiegeLadder.LadderState.BeingRaisedStartFromGround:
			case SiegeLadder.LadderState.BeingPushedBackStopped:
				if (this._animationState == SiegeLadder.LadderAnimationState.Animated)
				{
					float animationParameterAtChannel2 = this._ladderSkeleton.GetAnimationParameterAtChannel(0);
					float animationDuration2 = MBAnimation.GetAnimationDuration(this._ladderSkeleton.GetAnimationIndexAtChannel(0));
					foreach (StandingPoint standingPoint3 in this._attackerStandingPoints)
					{
						if (standingPoint3.HasUser)
						{
							MBActionSet actionSet = standingPoint3.UserAgent.ActionSet;
							ActionIndexCache actionCodeToUseForStandingPoint2 = this.GetActionCodeToUseForStandingPoint(standingPoint3);
							ActionIndexCache currentAction2 = standingPoint3.UserAgent.GetCurrentAction(1);
							if (currentAction2 == actionCodeToUseForStandingPoint2)
							{
								int animationIndexOfAction2 = MBActionSet.GetAnimationIndexOfAction(actionSet, in actionCodeToUseForStandingPoint2);
								float num3 = MBMath.ClampFloat(animationParameterAtChannel2 * animationDuration2 / MathF.Max(MBAnimation.GetAnimationDuration(animationIndexOfAction2), 0.01f), 0f, 1f);
								standingPoint3.UserAgent.SetCurrentActionProgress(1, num3);
							}
							else if (MBAnimation.GetActionType(currentAction2) == Agent.ActionCodeType.LadderRaiseEnd)
							{
								float animationDuration3 = MBAnimation.GetAnimationDuration(MBActionSet.GetAnimationIndexOfAction(actionSet, in currentAction2));
								float num4 = animationDuration2 - animationDuration3;
								float num5 = MBMath.ClampFloat((animationParameterAtChannel2 * animationDuration2 - num4) / MathF.Max(animationDuration3, 0.01f), 0f, 1f);
								standingPoint3.UserAgent.SetCurrentActionProgress(1, num5);
							}
						}
					}
					bool flag3 = false;
					if (!GameNetwork.IsClientOrReplay)
					{
						if (num2 > 0)
						{
							if (num2 != this._currentActionAgentCount)
							{
								this._currentActionAgentCount = num2;
								float num6 = MathF.Sqrt((float)this._currentActionAgentCount);
								float animationParameterAtChannel3 = this._ladderSkeleton.GetAnimationParameterAtChannel(0);
								this._ladderObject.SetAnimationAtChannelSynched(this._raiseAnimationIndex, 0, num6);
								if (animationParameterAtChannel3 > 0f)
								{
									this._ladderObject.SetAnimationChannelParameterSynched(0, animationParameterAtChannel3);
								}
							}
							using (List<StandingPoint>.Enumerator enumerator = this._attackerStandingPoints.GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									StandingPoint standingPoint4 = enumerator.Current;
									if (standingPoint4.HasUser)
									{
										ActionIndexCache actionCodeToUseForStandingPoint3 = this.GetActionCodeToUseForStandingPoint(standingPoint4);
										Agent userAgent3 = standingPoint4.UserAgent;
										ActionIndexCache currentAction3 = userAgent3.GetCurrentAction(1);
										if (currentAction3 != actionCodeToUseForStandingPoint3 && MBAnimation.GetActionType(currentAction3) != Agent.ActionCodeType.LadderRaiseEnd)
										{
											if (!userAgent3.SetActionChannel(1, in actionCodeToUseForStandingPoint3, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true) && !userAgent3.IsAIControlled)
											{
												userAgent3.StopUsingGameObject(false, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
											}
										}
										else if (MBAnimation.GetActionType(currentAction3) == Agent.ActionCodeType.LadderRaiseEnd)
										{
											standingPoint4.UserAgent.ClearTargetFrame();
										}
									}
								}
								goto IL_087C;
							}
						}
						this.State = SiegeLadder.LadderState.BeingRaisedStopped;
						flag3 = true;
					}
					IL_087C:
					if (!flag3)
					{
						MatrixFrame matrixFrame = gameEntity.GetGlobalFrame();
						MatrixFrame matrixFrame2 = this._ladderSkeleton.GetBoneEntitialFrameWithIndex(0);
						MatrixFrame matrixFrame3 = matrixFrame.TransformToParent(in matrixFrame2);
						matrixFrame3.rotation.RotateAboutForward(1.5707964f);
						if ((animationParameterAtChannel2 > 0.9f && animationParameterAtChannel2 != 1f) || matrixFrame3.rotation.f.z <= 0.2f)
						{
							this._animationState = SiegeLadder.LadderAnimationState.PhysicallyDynamic;
							this._fallAngularSpeed = this.GetCurrentLadderAngularSpeed(this._raiseAnimationIndex);
							gameEntity.SetGlobalFrame(in matrixFrame3, true);
							this._ladderSkeleton.SetAnimationAtChannel(this._raiseAnimationWithoutRootBoneIndex, 0, 1f, -1f, 0f);
							this._ladderSkeleton.SetAnimationParameterAtChannel(0, animationParameterAtChannel2);
							this._ladderSkeleton.TickAnimationsAndForceUpdate(0.0001f, gameEntity.GetGlobalFrame(), false);
							this._ladderSkeleton.SetAnimationAtChannel(this._idleAnimationIndex, 0, 1f, -1f, 0f);
							this._ladderObject.SetLocalPositionSmoothStep(ref this._ladderUpFrame.origin);
						}
					}
				}
				else if (this._animationState == SiegeLadder.LadderAnimationState.PhysicallyDynamic)
				{
					MatrixFrame frame = gameEntity.GetFrame();
					frame.rotation.RotateAboutSide(this._fallAngularSpeed * dt);
					gameEntity.SetFrame(ref frame, true);
					MatrixFrame matrixFrame2 = gameEntity.GetFrame();
					MatrixFrame matrixFrame = this._ladderSkeleton.GetBoneEntitialFrameWithIndex(0);
					MatrixFrame matrixFrame4 = matrixFrame2.TransformToParent(in matrixFrame);
					float num7 = Vec3.DotProduct(matrixFrame4.rotation.f, this._ladderUpFrame.rotation.f);
					if (this._fallAngularSpeed < 0f && num7 > 0.95f && num7 < this._lastDotProductOfAnimationAndTargetRotation)
					{
						gameEntity.SetFrame(ref this._ladderUpFrame, true);
						this._ladderSkeleton.SetAnimationParameterAtChannel(0, 0f);
						this._ladderSkeleton.TickAnimationsAndForceUpdate(0.0001f, gameEntity.GetGlobalFrame(), false);
						this._animationState = SiegeLadder.LadderAnimationState.Static;
						this._ladderSkeleton.SetAnimationAtChannel((this._fallAngularSpeed < -0.5f) ? this._trembleWallHeavyAnimationIndex : this._trembleWallLightAnimationIndex, 0, 1f, -1f, 0f);
						if (!GameNetwork.IsClientOrReplay)
						{
							this.State = SiegeLadder.LadderState.FallToWall;
						}
					}
					this._fallAngularSpeed -= dt * 2f * MathF.Max(0.3f, 1f - matrixFrame4.rotation.u.z);
					this._lastDotProductOfAnimationAndTargetRotation = num7;
				}
				break;
			case SiegeLadder.LadderState.BeingRaisedStopped:
			case SiegeLadder.LadderState.BeingPushedBack:
			case SiegeLadder.LadderState.BeingPushedBackStartFromWall:
				if (this._animationState == SiegeLadder.LadderAnimationState.Animated)
				{
					float animationParameterAtChannel4 = this._ladderSkeleton.GetAnimationParameterAtChannel(0);
					if (this._pushingWithForkStandingPoint.HasUser && this._pushingWithForkStandingPoint.UserAgent.GetCurrentAction(1) == ActionIndexCache.act_usage_ladder_push_back)
					{
						this._pushingWithForkStandingPoint.UserAgent.SetCurrentActionProgress(1, animationParameterAtChannel4);
					}
					bool flag4 = false;
					if (!GameNetwork.IsClientOrReplay)
					{
						if (num > 0)
						{
							if (num != this._currentActionAgentCount)
							{
								this._currentActionAgentCount = num;
								float num8 = MathF.Sqrt((float)this._currentActionAgentCount);
								float animationParameterAtChannel5 = this._ladderSkeleton.GetAnimationParameterAtChannel(0);
								this._ladderObject.SetAnimationAtChannelSynched(this.PushBackAnimation, 0, num8);
								if (animationParameterAtChannel5 > 0f)
								{
									this._ladderObject.SetAnimationChannelParameterSynched(0, animationParameterAtChannel5);
								}
							}
							if (this._pushingWithForkStandingPoint.HasUser)
							{
								Agent userAgent4 = this._pushingWithForkStandingPoint.UserAgent;
								if (userAgent4.GetCurrentAction(1) != ActionIndexCache.act_usage_ladder_push_back && animationParameterAtChannel4 < 1f && !userAgent4.SetActionChannel(1, in ActionIndexCache.act_usage_ladder_push_back, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true) && !userAgent4.IsAIControlled)
								{
									userAgent4.StopUsingGameObject(false, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
								}
							}
						}
						else
						{
							this.State = SiegeLadder.LadderState.BeingPushedBackStopped;
							flag4 = true;
						}
					}
					if (!flag4)
					{
						MatrixFrame matrixFrame = gameEntity.GetGlobalFrame();
						MatrixFrame matrixFrame2 = this._ladderSkeleton.GetBoneEntitialFrameWithIndex(0);
						MatrixFrame matrixFrame5 = matrixFrame.TransformToParent(in matrixFrame2);
						matrixFrame5.rotation.RotateAboutForward(1.5707964f);
						if (animationParameterAtChannel4 > 0.9999f || matrixFrame5.rotation.f.z >= 0f)
						{
							this._animationState = SiegeLadder.LadderAnimationState.PhysicallyDynamic;
							this._fallAngularSpeed = this.GetCurrentLadderAngularSpeed(this._pushBackAnimationIndex);
							gameEntity.SetGlobalFrame(in matrixFrame5, true);
							this._ladderSkeleton.SetAnimationAtChannel(this._pushBackAnimationWithoutRootBoneIndex, 0, 1f, -1f, 0f);
							this._ladderSkeleton.SetAnimationParameterAtChannel(0, animationParameterAtChannel4);
							this._ladderSkeleton.TickAnimationsAndForceUpdate(0.0001f, gameEntity.GetGlobalFrame(), false);
							this._ladderSkeleton.SetAnimationAtChannel(this._idleAnimationIndex, 0, 1f, -1f, 0f);
							this._ladderObject.SetLocalPositionSmoothStep(ref this._ladderDownFrame.origin);
						}
					}
				}
				else if (this._animationState == SiegeLadder.LadderAnimationState.PhysicallyDynamic)
				{
					MatrixFrame frame2 = gameEntity.GetFrame();
					frame2.rotation.RotateAboutSide(this._fallAngularSpeed * dt);
					gameEntity.SetFrame(ref frame2, true);
					MatrixFrame matrixFrame2 = gameEntity.GetFrame();
					MatrixFrame matrixFrame = this._ladderSkeleton.GetBoneEntitialFrameWithIndex(0);
					MatrixFrame matrixFrame6 = matrixFrame2.TransformToParent(in matrixFrame);
					matrixFrame6.rotation.RotateAboutForward(1.5707964f);
					float num9 = Vec3.DotProduct(matrixFrame6.rotation.f, this._ladderDownFrame.rotation.f);
					if (this._fallAngularSpeed > 0f && num9 > 0.95f && num9 < this._lastDotProductOfAnimationAndTargetRotation)
					{
						this._animationState = SiegeLadder.LadderAnimationState.Static;
						gameEntity.SetFrame(ref this._ladderDownFrame, true);
						this._ladderSkeleton.SetAnimationParameterAtChannel(0, 0f);
						this._ladderSkeleton.TickAnimationsAndForceUpdate(0.0001f, gameEntity.GetGlobalFrame(), false);
						this._ladderSkeleton.SetAnimationAtChannel(this._trembleGroundAnimationIndex, 0, 1f, -1f, 0f);
						this._animationState = SiegeLadder.LadderAnimationState.Static;
						if (!GameNetwork.IsClientOrReplay)
						{
							this.State = SiegeLadder.LadderState.FallToLand;
						}
					}
					this._fallAngularSpeed += dt * 2f * MathF.Max(0.3f, 1f - matrixFrame6.rotation.u.z);
					this._lastDotProductOfAnimationAndTargetRotation = num9;
				}
				break;
			case SiegeLadder.LadderState.OnWall:
			case SiegeLadder.LadderState.FallToWall:
				if (num > 0 && !GameNetwork.IsClientOrReplay)
				{
					this.State = SiegeLadder.LadderState.BeingPushedBackStartFromWall;
				}
				break;
			default:
				Debug.FailedAssert("Invalid ladder action state.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Objects\\Siege\\SiegeLadder.cs", "OnTick", 1258);
				break;
			}
			this.CalculateNavigationAndPhysics();
		}

		// Token: 0x0600303D RID: 12349 RVA: 0x000C15A4 File Offset: 0x000BF7A4
		protected internal override void OnTickParallel(float dt)
		{
			base.OnTickParallel(dt);
			for (int i = 0; i < this._attackerStandingPoints.Count; i++)
			{
				if (this._attackerStandingPoints[i].HasUser)
				{
					if (!this._attackerStandingPoints[i].UserAgent.IsInBeingStruckAction)
					{
						if (this._attackerStandingPoints[i].UserAgent.GetCurrentAction(1) != this.GetActionCodeToUseForStandingPoint(this._attackerStandingPoints[i]))
						{
							MatrixFrame matrixFrame = this._attackerStandingPointLocalIKFrames[i];
							MatrixFrame frame = this._attackerStandingPoints[i].UserAgent.Frame;
							matrixFrame.rotation = Mat3.Lerp(in matrixFrame.rotation, in this._ladderInitialGlobalFrame.TransformToLocal(in frame).rotation, MathF.Clamp(MathF.Lerp(0f, 1f - this._turningAngle * 1.2f, MathF.Pow(this._attackerStandingPoints[i].UserAgent.GetCurrentActionProgress(1), 6f), 1E-05f), 0f, 1f));
							this._attackerStandingPoints[i].UserAgent.SetHandInverseKinematicsFrameForMissionObjectUsage(in matrixFrame, in this._ladderInitialGlobalFrame, 0f);
						}
						else
						{
							this._attackerStandingPoints[i].UserAgent.SetHandInverseKinematicsFrameForMissionObjectUsage(in this._attackerStandingPointLocalIKFrames[i], in this._ladderInitialGlobalFrame, 0f);
						}
					}
					else
					{
						this._attackerStandingPoints[i].UserAgent.ClearHandInverseKinematics();
					}
				}
			}
		}

		// Token: 0x0600303E RID: 12350 RVA: 0x000C1740 File Offset: 0x000BF940
		private void TickRare()
		{
			if (!GameNetwork.IsReplay)
			{
				float num = 20f + (base.ForcedUse ? 3f : 0f);
				num *= num;
				Mission.TeamCollection teams = Mission.Current.Teams;
				int count = teams.Count;
				Vec3 globalPosition = base.GameEntity.GlobalPosition;
				for (int i = 0; i < count; i++)
				{
					Team team = teams[i];
					if (team.Side == BattleSideEnum.Attacker)
					{
						base.SetForcedUse(false);
						foreach (Formation formation in team.FormationsIncludingSpecialAndEmpty)
						{
							if (formation.CountOfUnits > 0 && formation.CachedMedianPosition.AsVec2.DistanceSquared(globalPosition.AsVec2) < num && formation.CachedMedianPosition.GetNavMeshZ() - globalPosition.z < 4f)
							{
								base.SetForcedUse(true);
								break;
							}
						}
					}
				}
			}
		}

		// Token: 0x0600303F RID: 12351 RVA: 0x000C1864 File Offset: 0x000BFA64
		public override UsableMachineAIBase CreateAIBehaviorObject()
		{
			return new SiegeLadderAI(this);
		}

		// Token: 0x06003040 RID: 12352 RVA: 0x000C186C File Offset: 0x000BFA6C
		public void SetUpStateVisibility(bool isVisible)
		{
			base.GameEntity.CollectChildrenEntitiesWithTag(this.upStateEntityTag)[0].SetVisibilityExcludeParents(isVisible);
		}

		// Token: 0x06003041 RID: 12353 RVA: 0x000C189C File Offset: 0x000BFA9C
		private void FlushQueueManager()
		{
			LadderQueueManager queueManagerForAttackers = this._queueManagerForAttackers;
			if (queueManagerForAttackers == null)
			{
				return;
			}
			queueManagerForAttackers.FlushQueueManager();
		}

		// Token: 0x06003042 RID: 12354 RVA: 0x000C18B0 File Offset: 0x000BFAB0
		private void FlushNeighborQueueManagers()
		{
			foreach (SiegeLadder siegeLadder in (from sl in Mission.Current.ActiveMissionObjects.FindAllWithType<SiegeLadder>()
				where sl.WeaponSide == this.WeaponSide
				select sl).ToList<SiegeLadder>())
			{
				if (siegeLadder != this)
				{
					siegeLadder.FlushQueueManager();
				}
			}
		}

		// Token: 0x06003043 RID: 12355 RVA: 0x000C1928 File Offset: 0x000BFB28
		private bool CanLadderBePushed()
		{
			float num = 0f;
			WeakGameEntity gameEntity = this._ladderObject.GameEntity;
			Vec3 vec;
			Vec3 vec2;
			gameEntity.GetPhysicsMinMax(true, out vec, out vec2, false);
			float num2 = (vec2 - vec).AsVec2.Length * 0.5f;
			AgentProximityMap.ProximityMapSearchStruct proximityMapSearchStruct = AgentProximityMap.BeginSearch(Mission.Current, gameEntity.GlobalPosition.AsVec2, num2, false);
			while (proximityMapSearchStruct.LastFoundAgent != null)
			{
				Agent lastFoundAgent = proximityMapSearchStruct.LastFoundAgent;
				if (lastFoundAgent.GetSteppedMachine() == this)
				{
					float num3 = (lastFoundAgent.Position.z - vec.z) / (vec2.z - vec.z) * 100f;
					if (num3 > this.LadderPushTresholdForOneAgent)
					{
						return false;
					}
					num += num3;
				}
				AgentProximityMap.FindNext(Mission.Current, ref proximityMapSearchStruct);
			}
			return num <= this.LadderPushTreshold;
		}

		// Token: 0x06003044 RID: 12356 RVA: 0x000C1A04 File Offset: 0x000BFC04
		private void InformNeighborQueueManagers(LadderQueueManager ladderQueueManager)
		{
			foreach (SiegeLadder siegeLadder in (from sl in Mission.Current.ActiveMissionObjects.FindAllWithType<SiegeLadder>()
				where sl.WeaponSide == this.WeaponSide && sl._queueManagerForAttackers != null
				select sl).ToList<SiegeLadder>())
			{
				if (siegeLadder != this && siegeLadder._queueManagerForAttackers != null)
				{
					siegeLadder._queueManagerForAttackers.AssignNeighborQueueManager(ladderQueueManager);
					LadderQueueManager queueManagerForAttackers = this._queueManagerForAttackers;
					if (queueManagerForAttackers != null)
					{
						queueManagerForAttackers.AssignNeighborQueueManager(siegeLadder._queueManagerForAttackers);
					}
				}
			}
		}

		// Token: 0x06003045 RID: 12357 RVA: 0x000C1AA0 File Offset: 0x000BFCA0
		public override void SetAbilityOfFaces(bool enabled)
		{
			base.SetAbilityOfFaces(enabled);
			base.GameEntity.Scene.SetAbilityOfFacesWithId(this.OnWallNavMeshId, enabled);
			if (Mission.Current != null)
			{
				if (enabled)
				{
					this.FlushNeighborQueueManagers();
					this.InformNeighborQueueManagers(this._queueManagerForAttackers);
					return;
				}
				this.InformNeighborQueueManagers(null);
				LadderQueueManager queueManagerForAttackers = this._queueManagerForAttackers;
				if (queueManagerForAttackers == null)
				{
					return;
				}
				queueManagerForAttackers.AssignNeighborQueueManager(null);
			}
		}

		// Token: 0x06003046 RID: 12358 RVA: 0x000C1B04 File Offset: 0x000BFD04
		protected internal override void OnMissionReset()
		{
			this._ladderSkeleton.SetAnimationAtChannel(-1, 0, 1f, -1f, 0f);
			if (this.initialState == SiegeLadder.LadderState.OnLand)
			{
				if (!GameNetwork.IsClientOrReplay)
				{
					this.State = SiegeLadder.LadderState.OnLand;
				}
				this._ladderObject.GameEntity.SetFrame(ref this._ladderDownFrame, true);
				return;
			}
			if (!GameNetwork.IsClientOrReplay)
			{
				this.State = SiegeLadder.LadderState.OnWall;
			}
			this._ladderObject.GameEntity.SetFrame(ref this._ladderUpFrame, true);
		}

		// Token: 0x06003047 RID: 12359 RVA: 0x000C1B86 File Offset: 0x000BFD86
		public override TextObject GetDescriptionText(WeakGameEntity gameEntity)
		{
			if (!gameEntity.HasTag(this.AmmoPickUpTag))
			{
				return new TextObject("{=G0AWk1rX}Ladder", null);
			}
			return new TextObject("{=F9AQxCax}Fork", null);
		}

		// Token: 0x06003048 RID: 12360 RVA: 0x000C1BB0 File Offset: 0x000BFDB0
		public override TextObject GetActionTextForStandingPoint(UsableMissionObject usableGameObject)
		{
			TextObject textObject;
			if (usableGameObject.GameEntity.HasTag(this.AmmoPickUpTag))
			{
				textObject = new TextObject("{=bNYm3K6b}{KEY} Pick Up", null);
			}
			else
			{
				textObject = (usableGameObject.GameEntity.HasTag(this.AttackerTag) ? new TextObject("{=kbNcm68J}{KEY} Lift", null) : new TextObject("{=MdQJxiGz}{KEY} Push", null));
			}
			textObject.SetTextVariable("KEY", HyperlinkTexts.GetKeyHyperlinkText(HotKeyManager.GetHotKeyId("CombatHotKeyCategory", 13), 1f));
			return textObject;
		}

		// Token: 0x06003049 RID: 12361 RVA: 0x000C1C34 File Offset: 0x000BFE34
		public override void WriteToNetwork()
		{
			base.WriteToNetwork();
			GameNetworkMessage.WriteBoolToPacket(this.initialState == SiegeLadder.LadderState.OnLand);
			GameNetworkMessage.WriteIntToPacket((int)this.State, CompressionMission.SiegeLadderStateCompressionInfo);
			GameNetworkMessage.WriteIntToPacket((int)this._animationState, CompressionMission.SiegeLadderAnimationStateCompressionInfo);
			GameNetworkMessage.WriteFloatToPacket(this._fallAngularSpeed, CompressionMission.SiegeMachineComponentAngularSpeedCompressionInfo);
			GameNetworkMessage.WriteMatrixFrameToPacket(this._ladderObject.GameEntity.GetGlobalFrame());
			int animationIndexAtChannel = this._ladderSkeleton.GetAnimationIndexAtChannel(0);
			GameNetworkMessage.WriteBoolToPacket(animationIndexAtChannel >= 0);
			if (animationIndexAtChannel >= 0)
			{
				GameNetworkMessage.WriteIntToPacket(animationIndexAtChannel, CompressionBasic.AnimationIndexCompressionInfo);
				GameNetworkMessage.WriteFloatToPacket(this._ladderSkeleton.GetAnimationParameterAtChannel(0), CompressionBasic.AnimationProgressCompressionInfo);
			}
		}

		// Token: 0x0600304A RID: 12362 RVA: 0x000C1CDC File Offset: 0x000BFEDC
		bool IOrderableWithInteractionArea.IsPointInsideInteractionArea(Vec3 point)
		{
			WeakGameEntity weakGameEntity = base.GameEntity.CollectChildrenEntitiesWithTag("ui_interaction").FirstOrDefault<WeakGameEntity>();
			return weakGameEntity.IsValid && weakGameEntity.GlobalPosition.AsVec2.DistanceSquared(point.AsVec2) < 25f;
		}

		// Token: 0x0600304B RID: 12363 RVA: 0x000C1D34 File Offset: 0x000BFF34
		public override TargetFlags GetTargetFlags()
		{
			TargetFlags targetFlags = TargetFlags.None;
			targetFlags |= TargetFlags.IsFlammable;
			targetFlags |= TargetFlags.IsSiegeEngine;
			targetFlags |= TargetFlags.IsAttacker;
			if (this.HasCompletedAction() || this.IsDeactivated)
			{
				targetFlags |= TargetFlags.NotAThreat;
			}
			return targetFlags;
		}

		// Token: 0x0600304C RID: 12364 RVA: 0x000C1D66 File Offset: 0x000BFF66
		public override float GetTargetValue(List<Vec3> weaponPos)
		{
			return 10f * base.GetUserMultiplierOfWeapon() * this.GetDistanceMultiplierOfWeapon(weaponPos[0]);
		}

		// Token: 0x0600304D RID: 12365 RVA: 0x000C1D82 File Offset: 0x000BFF82
		protected override float GetDistanceMultiplierOfWeapon(Vec3 weaponPos)
		{
			if (this.GetMinimumDistanceBetweenPositions(weaponPos) >= 10f)
			{
				return 0.9f;
			}
			return 1f;
		}

		// Token: 0x0600304E RID: 12366 RVA: 0x000C1DA0 File Offset: 0x000BFFA0
		protected override StandingPoint GetSuitableStandingPointFor(BattleSideEnum side, Agent agent = null, List<Agent> agents = null, List<ValueTuple<Agent, float>> agentValuePairs = null)
		{
			if (side == BattleSideEnum.Attacker)
			{
				return this._attackerStandingPoints.FirstOrDefault<StandingPoint>((StandingPoint sp) => !sp.IsDeactivated && (sp.IsInstantUse || (!sp.HasUser && !sp.HasAIMovingTo)));
			}
			if (this._pushingWithForkStandingPoint.IsDeactivated || (!this._pushingWithForkStandingPoint.IsInstantUse && (this._pushingWithForkStandingPoint.HasUser || this._pushingWithForkStandingPoint.HasAIMovingTo)))
			{
				return null;
			}
			return this._pushingWithForkStandingPoint;
		}

		// Token: 0x0600304F RID: 12367 RVA: 0x000C1E18 File Offset: 0x000C0018
		public void SetSpawnedFromSpawner()
		{
			this._spawnedFromSpawner = true;
		}

		// Token: 0x06003050 RID: 12368 RVA: 0x000C1E24 File Offset: 0x000C0024
		public override void OnAfterReadFromNetwork(ValueTuple<BaseSynchedMissionObjectReadableRecord, ISynchedMissionObjectReadableRecord> synchedMissionObjectReadableRecord, bool allowVisibilityUpdate = true)
		{
			base.OnAfterReadFromNetwork(synchedMissionObjectReadableRecord, allowVisibilityUpdate);
			SiegeLadder.SiegeLadderRecord siegeLadderRecord = (SiegeLadder.SiegeLadderRecord)synchedMissionObjectReadableRecord.Item2;
			this.initialState = (siegeLadderRecord.IsStateLand ? SiegeLadder.LadderState.OnLand : SiegeLadder.LadderState.OnWall);
			this._state = (SiegeLadder.LadderState)siegeLadderRecord.State;
			this._animationState = (SiegeLadder.LadderAnimationState)siegeLadderRecord.AnimationState;
			this._fallAngularSpeed = siegeLadderRecord.FallAngularSpeed;
			this._lastDotProductOfAnimationAndTargetRotation = -1000f;
			MatrixFrame matrixFrame = siegeLadderRecord.LadderFrame;
			matrixFrame.rotation.Orthonormalize();
			WeakGameEntity gameEntity = this._ladderObject.GameEntity;
			matrixFrame = siegeLadderRecord.LadderFrame;
			gameEntity.SetGlobalFrame(in matrixFrame, true);
			if (siegeLadderRecord.LadderAnimationIndex >= 0)
			{
				this._ladderSkeleton.SetAnimationAtChannel(siegeLadderRecord.LadderAnimationIndex, 0, 1f, -1f, 0f);
				this._ladderSkeleton.SetAnimationParameterAtChannel(0, MBMath.ClampFloat(siegeLadderRecord.LadderAnimationProgress, 0f, 1f));
				this._ladderSkeleton.ForceUpdateBoneFrames();
			}
		}

		// Token: 0x06003051 RID: 12369 RVA: 0x000C1F15 File Offset: 0x000C0115
		public void AssignParametersFromSpawner(string sideTag, string targetWallSegment, int onWallNavMeshId, float downStateRotationRadian, float upperStateRotationRadian, string barrierTagToRemove, string indestructibleMerlonsTag)
		{
			this._sideTag = sideTag;
			this._targetWallSegmentTag = targetWallSegment;
			this.OnWallNavMeshId = onWallNavMeshId;
			this._downStateRotationRadian = downStateRotationRadian;
			this._upStateRotationRadian = upperStateRotationRadian;
			this.BarrierTagToRemove = barrierTagToRemove;
			this.IndestructibleMerlonsTag = indestructibleMerlonsTag;
		}

		// Token: 0x06003052 RID: 12370 RVA: 0x000C1F4C File Offset: 0x000C014C
		public bool GetNavmeshFaceIds(out List<int> navmeshFaceIds)
		{
			navmeshFaceIds = new List<int> { this.OnWallNavMeshId };
			return true;
		}

		// Token: 0x06003053 RID: 12371 RVA: 0x000C1F62 File Offset: 0x000C0162
		public void OnFormationFrameChanged(Agent agent, bool hasFrame, WorldPosition position)
		{
			this._queueManagerForAttackers.OnFormationFrameChanged(agent, hasFrame, position);
		}

		// Token: 0x040013D8 RID: 5080
		public const float ClimbingLimitRadian = -0.20135832f;

		// Token: 0x040013D9 RID: 5081
		public const float ClimbingLimitDegree = -11.536982f;

		// Token: 0x040013DA RID: 5082
		public const float AutomaticUseActivationRange = 20f;

		// Token: 0x040013DB RID: 5083
		public string AttackerTag = "attacker";

		// Token: 0x040013DC RID: 5084
		public string DefenderTag = "defender";

		// Token: 0x040013DD RID: 5085
		public string downStateEntityTag = "ladderDown";

		// Token: 0x040013DE RID: 5086
		public string IdleAnimation = "siege_ladder_idle";

		// Token: 0x040013DF RID: 5087
		public int _idleAnimationIndex = -1;

		// Token: 0x040013E0 RID: 5088
		public string RaiseAnimation = "siege_ladder_rise";

		// Token: 0x040013E1 RID: 5089
		public string RaiseAnimationWithoutRootBone = "siege_ladder_rise_wo_rootbone";

		// Token: 0x040013E2 RID: 5090
		public int _raiseAnimationWithoutRootBoneIndex = -1;

		// Token: 0x040013E3 RID: 5091
		public string PushBackAnimation = "siege_ladder_push_back";

		// Token: 0x040013E4 RID: 5092
		public int _pushBackAnimationIndex = -1;

		// Token: 0x040013E5 RID: 5093
		public string PushBackAnimationWithoutRootBone = "siege_ladder_push_back_wo_rootbone";

		// Token: 0x040013E6 RID: 5094
		public int _pushBackAnimationWithoutRootBoneIndex = -1;

		// Token: 0x040013E7 RID: 5095
		public string TrembleWallHeavyAnimation = "siege_ladder_stop_wall_heavy";

		// Token: 0x040013E8 RID: 5096
		public string TrembleWallLightAnimation = "siege_ladder_stop_wall_light";

		// Token: 0x040013E9 RID: 5097
		public string TrembleGroundAnimation = "siege_ladder_stop_ground_heavy";

		// Token: 0x040013EA RID: 5098
		public string RightStandingPointTag = "right";

		// Token: 0x040013EB RID: 5099
		public string LeftStandingPointTag = "left";

		// Token: 0x040013EC RID: 5100
		public string FrontStandingPointTag = "front";

		// Token: 0x040013ED RID: 5101
		public string PushForkItemID = "push_fork";

		// Token: 0x040013EE RID: 5102
		public string upStateEntityTag = "ladderUp";

		// Token: 0x040013EF RID: 5103
		public string BodyTag = "ladder_body";

		// Token: 0x040013F0 RID: 5104
		public string CollisionBodyTag = "ladder_collision_body";

		// Token: 0x040013F1 RID: 5105
		public string InitialWaitPositionTag = "initialwaitposition";

		// Token: 0x040013F2 RID: 5106
		private string _targetWallSegmentTag = "";

		// Token: 0x040013F3 RID: 5107
		public float LadderPushTreshold = 170f;

		// Token: 0x040013F4 RID: 5108
		public float LadderPushTresholdForOneAgent = 55f;

		// Token: 0x040013F5 RID: 5109
		private WallSegment _targetWallSegment;

		// Token: 0x040013F6 RID: 5110
		private string _sideTag;

		// Token: 0x040013F7 RID: 5111
		private int _trembleWallLightAnimationIndex = -1;

		// Token: 0x040013F8 RID: 5112
		public string BarrierTagToRemove = "barrier";

		// Token: 0x040013F9 RID: 5113
		private int _trembleGroundAnimationIndex = -1;

		// Token: 0x040013FA RID: 5114
		public SiegeLadder.LadderState initialState;

		// Token: 0x040013FB RID: 5115
		private int _trembleWallHeavyAnimationIndex = -1;

		// Token: 0x040013FC RID: 5116
		public string IndestructibleMerlonsTag = string.Empty;

		// Token: 0x040013FD RID: 5117
		private int _raiseAnimationIndex = -1;

		// Token: 0x040013FE RID: 5118
		private bool _isNavigationMeshDisabled;

		// Token: 0x040013FF RID: 5119
		private bool _isLadderPhysicsDisabled;

		// Token: 0x04001400 RID: 5120
		private bool _isLadderCollisionPhysicsDisabled;

		// Token: 0x04001401 RID: 5121
		private Timer _tickOccasionallyTimer;

		// Token: 0x04001402 RID: 5122
		private float _upStateRotationRadian;

		// Token: 0x04001403 RID: 5123
		private float _downStateRotationRadian;

		// Token: 0x04001404 RID: 5124
		private float _fallAngularSpeed;

		// Token: 0x04001405 RID: 5125
		private MatrixFrame _ladderDownFrame;

		// Token: 0x04001406 RID: 5126
		private MatrixFrame _ladderUpFrame;

		// Token: 0x04001407 RID: 5127
		private SiegeLadder.LadderAnimationState _animationState;

		// Token: 0x04001408 RID: 5128
		private int _currentActionAgentCount;

		// Token: 0x04001409 RID: 5129
		private SiegeLadder.LadderState _state;

		// Token: 0x0400140A RID: 5130
		private List<GameEntity> _aiBarriers;

		// Token: 0x0400140B RID: 5131
		private List<StandingPoint> _attackerStandingPoints;

		// Token: 0x0400140C RID: 5132
		private StandingPointWithWeaponRequirement _pushingWithForkStandingPoint;

		// Token: 0x0400140D RID: 5133
		private StandingPointWithWeaponRequirement _forkPickUpStandingPoint;

		// Token: 0x0400140E RID: 5134
		private ItemObject _forkItem;

		// Token: 0x0400140F RID: 5135
		private MatrixFrame[] _attackerStandingPointLocalIKFrames;

		// Token: 0x04001410 RID: 5136
		private MatrixFrame _ladderInitialGlobalFrame;

		// Token: 0x04001411 RID: 5137
		private SynchedMissionObject _ladderParticleObject;

		// Token: 0x04001412 RID: 5138
		private SynchedMissionObject _ladderBodyObject;

		// Token: 0x04001413 RID: 5139
		private SynchedMissionObject _ladderCollisionBodyObject;

		// Token: 0x04001414 RID: 5140
		private SynchedMissionObject _ladderObject;

		// Token: 0x04001415 RID: 5141
		private Skeleton _ladderSkeleton;

		// Token: 0x04001416 RID: 5142
		private float _lastDotProductOfAnimationAndTargetRotation;

		// Token: 0x04001417 RID: 5143
		private float _turningAngle;

		// Token: 0x04001418 RID: 5144
		private LadderQueueManager _queueManagerForAttackers;

		// Token: 0x04001419 RID: 5145
		private LadderQueueManager _queueManagerForDefenders;

		// Token: 0x0400141B RID: 5147
		private Timer _forkReappearingTimer;

		// Token: 0x0400141C RID: 5148
		private float _forkReappearingDelay = 10f;

		// Token: 0x0400141E RID: 5150
		private SynchedMissionObject _forkEntity;

		// Token: 0x02000629 RID: 1577
		[DefineSynchedMissionObjectType(typeof(SiegeLadder))]
		public struct SiegeLadderRecord : ISynchedMissionObjectReadableRecord
		{
			// Token: 0x17000AB3 RID: 2739
			// (get) Token: 0x06003FE4 RID: 16356 RVA: 0x000F7FB3 File Offset: 0x000F61B3
			// (set) Token: 0x06003FE5 RID: 16357 RVA: 0x000F7FBB File Offset: 0x000F61BB
			public bool IsStateLand { get; private set; }

			// Token: 0x17000AB4 RID: 2740
			// (get) Token: 0x06003FE6 RID: 16358 RVA: 0x000F7FC4 File Offset: 0x000F61C4
			// (set) Token: 0x06003FE7 RID: 16359 RVA: 0x000F7FCC File Offset: 0x000F61CC
			public int State { get; private set; }

			// Token: 0x17000AB5 RID: 2741
			// (get) Token: 0x06003FE8 RID: 16360 RVA: 0x000F7FD5 File Offset: 0x000F61D5
			// (set) Token: 0x06003FE9 RID: 16361 RVA: 0x000F7FDD File Offset: 0x000F61DD
			public int AnimationState { get; private set; }

			// Token: 0x17000AB6 RID: 2742
			// (get) Token: 0x06003FEA RID: 16362 RVA: 0x000F7FE6 File Offset: 0x000F61E6
			// (set) Token: 0x06003FEB RID: 16363 RVA: 0x000F7FEE File Offset: 0x000F61EE
			public float FallAngularSpeed { get; private set; }

			// Token: 0x17000AB7 RID: 2743
			// (get) Token: 0x06003FEC RID: 16364 RVA: 0x000F7FF7 File Offset: 0x000F61F7
			// (set) Token: 0x06003FED RID: 16365 RVA: 0x000F7FFF File Offset: 0x000F61FF
			public MatrixFrame LadderFrame { get; private set; }

			// Token: 0x17000AB8 RID: 2744
			// (get) Token: 0x06003FEE RID: 16366 RVA: 0x000F8008 File Offset: 0x000F6208
			// (set) Token: 0x06003FEF RID: 16367 RVA: 0x000F8010 File Offset: 0x000F6210
			public bool HasAnimation { get; private set; }

			// Token: 0x17000AB9 RID: 2745
			// (get) Token: 0x06003FF0 RID: 16368 RVA: 0x000F8019 File Offset: 0x000F6219
			// (set) Token: 0x06003FF1 RID: 16369 RVA: 0x000F8021 File Offset: 0x000F6221
			public int LadderAnimationIndex { get; private set; }

			// Token: 0x17000ABA RID: 2746
			// (get) Token: 0x06003FF2 RID: 16370 RVA: 0x000F802A File Offset: 0x000F622A
			// (set) Token: 0x06003FF3 RID: 16371 RVA: 0x000F8032 File Offset: 0x000F6232
			public float LadderAnimationProgress { get; private set; }

			// Token: 0x06003FF4 RID: 16372 RVA: 0x000F803C File Offset: 0x000F623C
			public bool ReadFromNetwork(ref bool bufferReadValid)
			{
				this.IsStateLand = GameNetworkMessage.ReadBoolFromPacket(ref bufferReadValid);
				this.State = GameNetworkMessage.ReadIntFromPacket(CompressionMission.SiegeLadderStateCompressionInfo, ref bufferReadValid);
				this.AnimationState = GameNetworkMessage.ReadIntFromPacket(CompressionMission.SiegeLadderAnimationStateCompressionInfo, ref bufferReadValid);
				this.FallAngularSpeed = GameNetworkMessage.ReadFloatFromPacket(CompressionMission.SiegeMachineComponentAngularSpeedCompressionInfo, ref bufferReadValid);
				this.LadderFrame = GameNetworkMessage.ReadMatrixFrameFromPacket(ref bufferReadValid);
				this.HasAnimation = GameNetworkMessage.ReadBoolFromPacket(ref bufferReadValid);
				this.LadderAnimationIndex = -1;
				this.LadderAnimationProgress = 0f;
				if (this.HasAnimation)
				{
					this.LadderAnimationIndex = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.AnimationIndexCompressionInfo, ref bufferReadValid);
					this.LadderAnimationProgress = GameNetworkMessage.ReadFloatFromPacket(CompressionBasic.AnimationProgressCompressionInfo, ref bufferReadValid);
				}
				return bufferReadValid;
			}
		}

		// Token: 0x0200062A RID: 1578
		public enum LadderState
		{
			// Token: 0x040020B8 RID: 8376
			OnLand,
			// Token: 0x040020B9 RID: 8377
			FallToLand,
			// Token: 0x040020BA RID: 8378
			BeingRaised,
			// Token: 0x040020BB RID: 8379
			BeingRaisedStartFromGround,
			// Token: 0x040020BC RID: 8380
			BeingRaisedStopped,
			// Token: 0x040020BD RID: 8381
			OnWall,
			// Token: 0x040020BE RID: 8382
			FallToWall,
			// Token: 0x040020BF RID: 8383
			BeingPushedBack,
			// Token: 0x040020C0 RID: 8384
			BeingPushedBackStartFromWall,
			// Token: 0x040020C1 RID: 8385
			BeingPushedBackStopped,
			// Token: 0x040020C2 RID: 8386
			NumberOfStates
		}

		// Token: 0x0200062B RID: 1579
		public enum LadderAnimationState
		{
			// Token: 0x040020C4 RID: 8388
			Static,
			// Token: 0x040020C5 RID: 8389
			Animated,
			// Token: 0x040020C6 RID: 8390
			PhysicallyDynamic,
			// Token: 0x040020C7 RID: 8391
			NumberOfStates
		}
	}
}
