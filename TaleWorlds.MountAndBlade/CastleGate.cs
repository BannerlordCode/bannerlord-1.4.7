using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Source.Objects.Siege;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000341 RID: 833
	public class CastleGate : UsableMachine, IPointDefendable, ICastleKeyPosition, ITargetable
	{
		// Token: 0x170008B1 RID: 2225
		// (get) Token: 0x06002EB3 RID: 11955 RVA: 0x000B4E68 File Offset: 0x000B3068
		// (set) Token: 0x06002EB4 RID: 11956 RVA: 0x000B4E70 File Offset: 0x000B3070
		public TacticalPosition MiddlePosition { get; private set; }

		// Token: 0x170008B2 RID: 2226
		// (get) Token: 0x06002EB5 RID: 11957 RVA: 0x000B4E79 File Offset: 0x000B3079
		private static int BatteringRamHitSoundIdCache
		{
			get
			{
				if (CastleGate._batteringRamHitSoundId == -1)
				{
					CastleGate._batteringRamHitSoundId = SoundEvent.GetEventIdFromString("event:/mission/siege/door/hit");
				}
				return CastleGate._batteringRamHitSoundId;
			}
		}

		// Token: 0x170008B3 RID: 2227
		// (get) Token: 0x06002EB6 RID: 11958 RVA: 0x000B4E97 File Offset: 0x000B3097
		// (set) Token: 0x06002EB7 RID: 11959 RVA: 0x000B4E9F File Offset: 0x000B309F
		public TacticalPosition WaitPosition { get; private set; }

		// Token: 0x170008B4 RID: 2228
		// (get) Token: 0x06002EB8 RID: 11960 RVA: 0x000B4EA8 File Offset: 0x000B30A8
		public override FocusableObjectType FocusableObjectType
		{
			get
			{
				return FocusableObjectType.Gate;
			}
		}

		// Token: 0x170008B5 RID: 2229
		// (get) Token: 0x06002EB9 RID: 11961 RVA: 0x000B4EAB File Offset: 0x000B30AB
		// (set) Token: 0x06002EBA RID: 11962 RVA: 0x000B4EB3 File Offset: 0x000B30B3
		public CastleGate.GateState State { get; private set; }

		// Token: 0x170008B6 RID: 2230
		// (get) Token: 0x06002EBB RID: 11963 RVA: 0x000B4EBC File Offset: 0x000B30BC
		public bool IsGateOpen
		{
			get
			{
				return this.State == CastleGate.GateState.Open || base.IsDestroyed;
			}
		}

		// Token: 0x170008B7 RID: 2231
		// (get) Token: 0x06002EBC RID: 11964 RVA: 0x000B4ECE File Offset: 0x000B30CE
		// (set) Token: 0x06002EBD RID: 11965 RVA: 0x000B4ED6 File Offset: 0x000B30D6
		public IPrimarySiegeWeapon AttackerSiegeWeapon { get; set; }

		// Token: 0x170008B8 RID: 2232
		// (get) Token: 0x06002EBE RID: 11966 RVA: 0x000B4EDF File Offset: 0x000B30DF
		// (set) Token: 0x06002EBF RID: 11967 RVA: 0x000B4EE7 File Offset: 0x000B30E7
		public IEnumerable<DefencePoint> DefencePoints { get; protected set; }

		// Token: 0x06002EC0 RID: 11968 RVA: 0x000B4EF0 File Offset: 0x000B30F0
		public CastleGate()
		{
			this._attackOnlyDoorColliders = new List<GameEntity>();
		}

		// Token: 0x06002EC1 RID: 11969 RVA: 0x000B4FB0 File Offset: 0x000B31B0
		public Vec3 GetPosition()
		{
			return base.GameEntity.GlobalPosition;
		}

		// Token: 0x06002EC2 RID: 11970 RVA: 0x000B4FCB File Offset: 0x000B31CB
		public override OrderType GetOrder(BattleSideEnum side)
		{
			if (base.IsDestroyed)
			{
				return OrderType.None;
			}
			if (side != BattleSideEnum.Attacker)
			{
				return OrderType.Use;
			}
			return OrderType.AttackEntity;
		}

		// Token: 0x170008B9 RID: 2233
		// (get) Token: 0x06002EC3 RID: 11971 RVA: 0x000B4FE0 File Offset: 0x000B31E0
		// (set) Token: 0x06002EC4 RID: 11972 RVA: 0x000B4FE8 File Offset: 0x000B31E8
		public FormationAI.BehaviorSide DefenseSide { get; private set; }

		// Token: 0x170008BA RID: 2234
		// (get) Token: 0x06002EC5 RID: 11973 RVA: 0x000B4FF1 File Offset: 0x000B31F1
		public WorldFrame MiddleFrame
		{
			get
			{
				return this._middleFrame;
			}
		}

		// Token: 0x170008BB RID: 2235
		// (get) Token: 0x06002EC6 RID: 11974 RVA: 0x000B4FF9 File Offset: 0x000B31F9
		public WorldFrame DefenseWaitFrame
		{
			get
			{
				return this._defenseWaitFrame;
			}
		}

		// Token: 0x06002EC7 RID: 11975 RVA: 0x000B5004 File Offset: 0x000B3204
		protected internal override void OnInit()
		{
			base.OnInit();
			DestructableComponent firstScriptOfType = base.GameEntity.GetFirstScriptOfType<DestructableComponent>();
			if (firstScriptOfType != null)
			{
				firstScriptOfType.OnNextDestructionState += this.OnNextDestructionState;
				this.DestructibleComponentOnMissionReset = new Action(firstScriptOfType.OnMissionReset);
				if (!GameNetwork.IsClientOrReplay)
				{
					firstScriptOfType.OnDestroyed += new DestructableComponent.OnHitTakenAndDestroyedDelegate(this.OnDestroyed);
					firstScriptOfType.OnHitTaken += new DestructableComponent.OnHitTakenAndDestroyedDelegate(this.OnHitTaken);
					DestructableComponent destructableComponent = firstScriptOfType;
					destructableComponent.OnCalculateDestructionStateIndex = (Func<int, int, int, int>)Delegate.Combine(destructableComponent.OnCalculateDestructionStateIndex, new Func<int, int, int, int>(this.OnCalculateDestructionStateIndex));
				}
				firstScriptOfType.BattleSide = BattleSideEnum.Defender;
			}
			this.CollectGameEntities(true);
			base.GameEntity.SetAnimationSoundActivation(true);
			if (GameNetwork.IsClientOrReplay)
			{
				return;
			}
			this._queueManager = base.GameEntity.GetFirstScriptOfType<LadderQueueManager>();
			if (this._queueManager == null)
			{
				WeakGameEntity weakGameEntity = base.GameEntity.GetChildren().FirstOrDefault<WeakGameEntity>((WeakGameEntity ce) => ce.HasScriptOfType<LadderQueueManager>());
				if (weakGameEntity.IsValid)
				{
					this._queueManager = weakGameEntity.GetFirstScriptOfType<LadderQueueManager>();
				}
			}
			if (this._queueManager != null)
			{
				MatrixFrame identity = MatrixFrame.Identity;
				identity.origin.y = identity.origin.y - 2f;
				identity.rotation.RotateAboutSide(-1.5707964f);
				identity.rotation.RotateAboutForward(3.1415927f);
				this._queueManager.Initialize(this._queueManager.ManagedNavigationFaceId, identity, -identity.rotation.u, BattleSideEnum.Defender, 15, 0.62831855f, 3f, 2.2f, 0f, 0f, false, 1f, 2.1474836E+09f, 5f, false, -2, -2, int.MaxValue, 15);
				this._queueManager.Activate();
			}
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
			List<WeakGameEntity> list = base.GameEntity.CollectChildrenEntitiesWithTag("middle_pos");
			if (list.Count > 0)
			{
				WeakGameEntity weakGameEntity2 = list.FirstOrDefault<WeakGameEntity>();
				this.MiddlePosition = weakGameEntity2.GetFirstScriptOfType<TacticalPosition>();
				MatrixFrame globalFrame = weakGameEntity2.GetGlobalFrame();
				this._middleFrame = new WorldFrame(globalFrame.rotation, globalFrame.origin.ToWorldPosition());
				this._middleFrame.Origin.GetGroundVec3();
			}
			else
			{
				MatrixFrame globalFrame2 = base.GameEntity.GetGlobalFrame();
				this._middleFrame = new WorldFrame(globalFrame2.rotation, globalFrame2.origin.ToWorldPosition());
			}
			List<WeakGameEntity> list2 = base.GameEntity.CollectChildrenEntitiesWithTag("wait_pos");
			if (list2.Count > 0)
			{
				WeakGameEntity weakGameEntity3 = list2.FirstOrDefault<WeakGameEntity>();
				this.WaitPosition = weakGameEntity3.GetFirstScriptOfType<TacticalPosition>();
				MatrixFrame globalFrame3 = weakGameEntity3.GetGlobalFrame();
				this._defenseWaitFrame = new WorldFrame(globalFrame3.rotation, globalFrame3.origin.ToWorldPosition());
				this._defenseWaitFrame.Origin.GetGroundVec3();
			}
			else
			{
				this._defenseWaitFrame = this._middleFrame;
			}
			this._openingAnimationIndex = MBAnimation.GetAnimationIndexWithName(this.OpeningAnimationName);
			this._closingAnimationIndex = MBAnimation.GetAnimationIndexWithName(this.ClosingAnimationName);
			base.SetScriptComponentToTick(this.GetTickRequirement());
			this.OnCheckForProblems();
		}

		// Token: 0x06002EC8 RID: 11976 RVA: 0x000B5378 File Offset: 0x000B3578
		public void SetUsableTeam(Team team)
		{
			using (List<StandingPoint>.Enumerator enumerator = base.StandingPoints.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					StandingPointWithTeamLimit standingPointWithTeamLimit;
					if ((standingPointWithTeamLimit = enumerator.Current as StandingPointWithTeamLimit) != null)
					{
						standingPointWithTeamLimit.UsableTeam = team;
					}
				}
			}
		}

		// Token: 0x06002EC9 RID: 11977 RVA: 0x000B53D4 File Offset: 0x000B35D4
		public override void AfterMissionStart()
		{
			this._afterMissionStartTriggered = true;
			base.AfterMissionStart();
			this.SetInitialStateOfGate();
			this.InitializeExtraColliderPositions();
			if (!GameNetwork.IsClientOrReplay)
			{
				this.SetAutoOpenState(Mission.Current.IsSallyOutBattle);
			}
			if (this.OwningTeam == CastleGate.DoorOwnership.Attackers)
			{
				this.SetUsableTeam(Mission.Current.AttackerTeam);
			}
			else if (this.OwningTeam == CastleGate.DoorOwnership.Defenders)
			{
				this.SetUsableTeam(Mission.Current.DefenderTeam);
			}
			this._pathChecker = new AgentPathNavMeshChecker(Mission.Current, base.GameEntity.GetGlobalFrame(), 2f, this.NavigationMeshId, BattleSideEnum.Defender, AgentPathNavMeshChecker.Direction.BothDirections, 14f, 3f);
		}

		// Token: 0x06002ECA RID: 11978 RVA: 0x000B547C File Offset: 0x000B367C
		protected override void OnRemoved(int removeReason)
		{
			base.OnRemoved(removeReason);
			DestructableComponent firstScriptOfType = base.GameEntity.GetFirstScriptOfType<DestructableComponent>();
			if (firstScriptOfType != null)
			{
				firstScriptOfType.OnNextDestructionState -= this.OnNextDestructionState;
				if (!GameNetwork.IsClientOrReplay)
				{
					firstScriptOfType.OnDestroyed -= new DestructableComponent.OnHitTakenAndDestroyedDelegate(this.OnDestroyed);
					firstScriptOfType.OnHitTaken -= new DestructableComponent.OnHitTakenAndDestroyedDelegate(this.OnHitTaken);
					DestructableComponent destructableComponent = firstScriptOfType;
					destructableComponent.OnCalculateDestructionStateIndex = (Func<int, int, int, int>)Delegate.Remove(destructableComponent.OnCalculateDestructionStateIndex, new Func<int, int, int, int>(this.OnCalculateDestructionStateIndex));
				}
			}
		}

		// Token: 0x06002ECB RID: 11979 RVA: 0x000B5504 File Offset: 0x000B3704
		protected internal override void OnEditorInit()
		{
			base.OnEditorInit();
			if (base.GameEntity.HasTag("outer_gate") && base.GameEntity.HasTag("inner_gate"))
			{
				MBDebug.ShowWarning("Castle gate has both the outer gate tag and the inner gate tag.");
			}
		}

		// Token: 0x06002ECC RID: 11980 RVA: 0x000B554B File Offset: 0x000B374B
		protected internal override void OnMissionReset()
		{
			Action destructibleComponentOnMissionReset = this.DestructibleComponentOnMissionReset;
			if (destructibleComponentOnMissionReset != null)
			{
				destructibleComponentOnMissionReset();
			}
			this.CollectGameEntities(false);
			base.OnMissionReset();
			this.SetInitialStateOfGate();
			this._previousAnimationProgress = -1f;
		}

		// Token: 0x06002ECD RID: 11981 RVA: 0x000B557C File Offset: 0x000B377C
		private void SetInitialStateOfGate()
		{
			if (!GameNetwork.IsClientOrReplay && this.NavigationMeshIdToDisableOnOpen != -1)
			{
				this._openNavMeshIdDisabled = false;
				base.Scene.SetAbilityOfFacesWithId(this.NavigationMeshIdToDisableOnOpen, true);
			}
			if (!this._civilianMission)
			{
				this._doorSkeleton.SetAnimationAtChannel(this._closingAnimationIndex, 0, 1f, -1f, 0f);
				this._doorSkeleton.SetAnimationParameterAtChannel(0, 0.99f);
				this._doorSkeleton.Freeze(false);
				this.State = CastleGate.GateState.Closed;
				return;
			}
			this.OpenDoor();
			if (this._doorSkeleton != null)
			{
				this._door.SetAnimationChannelParameterSynched(0, 1f);
			}
			this.SetGateNavMeshState(true);
			base.SetDisabled(true);
			DestructableComponent firstScriptOfType = base.GameEntity.GetFirstScriptOfType<DestructableComponent>();
			if (firstScriptOfType == null)
			{
				return;
			}
			firstScriptOfType.SetDisabled(false);
		}

		// Token: 0x06002ECE RID: 11982 RVA: 0x000B564D File Offset: 0x000B384D
		public override TextObject GetDescriptionText(WeakGameEntity gameEntity)
		{
			return new TextObject("{=6wZUG0ev}Gate", null);
		}

		// Token: 0x06002ECF RID: 11983 RVA: 0x000B565C File Offset: 0x000B385C
		public override TextObject GetActionTextForStandingPoint(UsableMissionObject usableGameObject)
		{
			if (!this.IsDeactivated)
			{
				TextObject textObject = new TextObject(usableGameObject.GameEntity.HasTag("open") ? "{=5oozsaIb}{KEY} Open" : "{=TJj71hPO}{KEY} Close", null);
				textObject.SetTextVariable("KEY", HyperlinkTexts.GetKeyHyperlinkText(HotKeyManager.GetHotKeyId("CombatHotKeyCategory", 13), 1f));
				return textObject;
			}
			return TextObject.GetEmpty();
		}

		// Token: 0x06002ED0 RID: 11984 RVA: 0x000B56C0 File Offset: 0x000B38C0
		public override UsableMachineAIBase CreateAIBehaviorObject()
		{
			return new CastleGateAI(this);
		}

		// Token: 0x06002ED1 RID: 11985 RVA: 0x000B56C8 File Offset: 0x000B38C8
		public void OpenDoorAndDisableGateForCivilianMission()
		{
			this._civilianMission = true;
		}

		// Token: 0x06002ED2 RID: 11986 RVA: 0x000B56D4 File Offset: 0x000B38D4
		public void OpenDoor()
		{
			if (!base.IsDisabled)
			{
				this.State = CastleGate.GateState.Open;
				if (!this.AutoOpen)
				{
					this.SetGateNavMeshState(true);
				}
				else
				{
					this.SetGateNavMeshStateForEnemies(true);
				}
				int animationIndexAtChannel = this._doorSkeleton.GetAnimationIndexAtChannel(0);
				float animationParameterAtChannel = this._doorSkeleton.GetAnimationParameterAtChannel(0);
				this._door.SetAnimationAtChannelSynched(this._openingAnimationIndex, 0, 1f);
				if (animationIndexAtChannel == this._closingAnimationIndex)
				{
					this._door.SetAnimationChannelParameterSynched(0, 1f - animationParameterAtChannel);
				}
				SynchedMissionObject plank = this._plank;
				if (plank == null)
				{
					return;
				}
				plank.SetVisibleSynched(false, false);
			}
		}

		// Token: 0x06002ED3 RID: 11987 RVA: 0x000B5768 File Offset: 0x000B3968
		public void CloseDoor()
		{
			if (!base.IsDisabled)
			{
				this.State = CastleGate.GateState.Closed;
				if (!this.AutoOpen)
				{
					this.SetGateNavMeshState(false);
				}
				else
				{
					this.SetGateNavMeshStateForEnemies(false);
				}
				int animationIndexAtChannel = this._doorSkeleton.GetAnimationIndexAtChannel(0);
				float animationParameterAtChannel = this._doorSkeleton.GetAnimationParameterAtChannel(0);
				this._door.SetAnimationAtChannelSynched(this._closingAnimationIndex, 0, 1f);
				if (animationIndexAtChannel == this._openingAnimationIndex)
				{
					this._door.SetAnimationChannelParameterSynched(0, 1f - animationParameterAtChannel);
				}
			}
		}

		// Token: 0x06002ED4 RID: 11988 RVA: 0x000B57E8 File Offset: 0x000B39E8
		private void UpdateDoorBodies(bool updateAnyway)
		{
			if (this._attackOnlyDoorColliders.Count == 2)
			{
				float animationParameterAtChannel = this._doorSkeleton.GetAnimationParameterAtChannel(0);
				if (this._previousAnimationProgress != animationParameterAtChannel || updateAnyway)
				{
					this._previousAnimationProgress = animationParameterAtChannel;
					MatrixFrame matrixFrame = this._doorSkeleton.GetBoneEntitialFrameWithIndex(this._leftDoorBoneIndex);
					MatrixFrame matrixFrame2 = this._doorSkeleton.GetBoneEntitialFrameWithIndex(this._rightDoorBoneIndex);
					this._attackOnlyDoorColliders[0].SetFrame(ref matrixFrame2, true);
					this._attackOnlyDoorColliders[1].SetFrame(ref matrixFrame, true);
					GameEntity agentColliderLeft = this._agentColliderLeft;
					if (agentColliderLeft != null)
					{
						agentColliderLeft.SetFrame(ref matrixFrame, true);
					}
					GameEntity agentColliderRight = this._agentColliderRight;
					if (agentColliderRight != null)
					{
						agentColliderRight.SetFrame(ref matrixFrame2, true);
					}
					if (this._extraColliderLeft != null && this._extraColliderRight != null)
					{
						if (this.State == CastleGate.GateState.Closed)
						{
							if (!this._leftExtraColliderDisabled)
							{
								this._extraColliderLeft.SetBodyFlags(this._extraColliderLeft.BodyFlag | BodyFlags.Disabled);
								this._leftExtraColliderDisabled = true;
							}
							if (!this._rightExtraColliderDisabled)
							{
								this._extraColliderRight.SetBodyFlags(this._extraColliderRight.BodyFlag | BodyFlags.Disabled);
								this._rightExtraColliderDisabled = true;
								return;
							}
						}
						else
						{
							float num = (matrixFrame2.origin - matrixFrame.origin).Length * 0.5f;
							float num2 = Vec3.DotProduct(matrixFrame2.rotation.s, Vec3.Side) / (matrixFrame2.rotation.s.Length * 1f);
							float num3 = MathF.Sqrt(1f - num2 * num2);
							float num4 = num * 1.1f;
							float num5 = MBMath.Map(num2, 0.3f, 1f, 0f, 1f) * (num * 0.2f);
							this._extraColliderLeft.SetLocalPosition(matrixFrame.origin - new Vec3(num4 - num + num5, num * num3, 0f, -1f));
							this._extraColliderRight.SetLocalPosition(matrixFrame2.origin - new Vec3(-(num4 - num) - num5, num * num3, 0f, -1f));
							float num6;
							if (num2 < 0f)
							{
								num6 = num;
								num6 += num * -num2;
							}
							else
							{
								num6 = num - num * num2;
							}
							num6 = (num4 - num6) / num;
							if (num6 <= 0.0001f)
							{
								if (!this._leftExtraColliderDisabled)
								{
									this._extraColliderLeft.SetBodyFlags(this._extraColliderLeft.BodyFlag | BodyFlags.Disabled);
									this._leftExtraColliderDisabled = true;
								}
							}
							else
							{
								if (this._leftExtraColliderDisabled)
								{
									this._extraColliderLeft.SetBodyFlags(this._extraColliderLeft.BodyFlag & ~BodyFlags.Disabled);
									this._leftExtraColliderDisabled = false;
								}
								matrixFrame = this._extraColliderLeft.GetFrame();
								matrixFrame.rotation.Orthonormalize();
								matrixFrame.origin -= new Vec3(num4 - num4 * num6, 0f, 0f, -1f);
								this._extraColliderLeft.SetFrame(ref matrixFrame, true);
							}
							matrixFrame2 = this._extraColliderRight.GetFrame();
							matrixFrame2.rotation.Orthonormalize();
							float num7;
							if (num2 < 0f)
							{
								num7 = num;
								num7 += num * -num2;
							}
							else
							{
								num7 = num - num * num2;
							}
							num7 = (num4 - num7) / num;
							if (num7 > 0.0001f)
							{
								if (this._rightExtraColliderDisabled)
								{
									this._extraColliderRight.SetBodyFlags(this._extraColliderRight.BodyFlag & ~BodyFlags.Disabled);
									this._rightExtraColliderDisabled = false;
								}
								matrixFrame2.origin += new Vec3(num4 - num4 * num7, 0f, 0f, -1f);
								this._extraColliderRight.SetFrame(ref matrixFrame2, true);
								return;
							}
							if (!this._rightExtraColliderDisabled)
							{
								this._extraColliderRight.SetBodyFlags(this._extraColliderRight.BodyFlag | BodyFlags.Disabled);
								this._rightExtraColliderDisabled = true;
								return;
							}
						}
					}
				}
			}
			else if (this._attackOnlyDoorColliders.Count == 1)
			{
				MatrixFrame boneEntitialFrameWithName = this._doorSkeleton.GetBoneEntitialFrameWithName(this.RightDoorBoneName);
				this._attackOnlyDoorColliders[0].SetFrame(ref boneEntitialFrameWithName, true);
				GameEntity agentColliderRight2 = this._agentColliderRight;
				if (agentColliderRight2 == null)
				{
					return;
				}
				agentColliderRight2.SetFrame(ref boneEntitialFrameWithName, true);
			}
		}

		// Token: 0x06002ED5 RID: 11989 RVA: 0x000B5C28 File Offset: 0x000B3E28
		private void SetGateNavMeshState(bool isEnabled)
		{
			if (!GameNetwork.IsClientOrReplay)
			{
				base.Scene.SetAbilityOfFacesWithId(this.NavigationMeshId, isEnabled);
				if (this._queueManager != null)
				{
					this._queueManager.Activate();
					base.Scene.SetAbilityOfFacesWithId(this._queueManager.ManagedNavigationFaceId, isEnabled);
				}
			}
		}

		// Token: 0x06002ED6 RID: 11990 RVA: 0x000B5C7C File Offset: 0x000B3E7C
		private void SetGateNavMeshStateForEnemies(bool isEnabled)
		{
			Team attackerTeam = Mission.Current.AttackerTeam;
			if (attackerTeam != null)
			{
				foreach (Agent agent in attackerTeam.ActiveAgents)
				{
					if (agent.IsAIControlled)
					{
						agent.SetAgentExcludeStateForFaceGroupId(this.NavigationMeshId, !isEnabled);
					}
				}
			}
		}

		// Token: 0x06002ED7 RID: 11991 RVA: 0x000B5CF0 File Offset: 0x000B3EF0
		public void SetAutoOpenState(bool isEnabled)
		{
			this.AutoOpen = isEnabled;
			if (this.AutoOpen)
			{
				this.SetGateNavMeshState(true);
				this.SetGateNavMeshStateForEnemies(this.State == CastleGate.GateState.Open);
				return;
			}
			if (this.State == CastleGate.GateState.Open)
			{
				this.CloseDoor();
			}
			else
			{
				this.SetGateNavMeshState(false);
			}
			this.SetGateNavMeshStateForEnemies(true);
		}

		// Token: 0x06002ED8 RID: 11992 RVA: 0x000B5D44 File Offset: 0x000B3F44
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			if (base.GameEntity.IsVisibleIncludeParents())
			{
				return ScriptComponentBehavior.TickRequirement.Tick | base.GetTickRequirement();
			}
			return base.GetTickRequirement();
		}

		// Token: 0x06002ED9 RID: 11993 RVA: 0x000B5D70 File Offset: 0x000B3F70
		protected internal override void OnTick(float dt)
		{
			base.OnTick(dt);
			if (!base.GameEntity.IsVisibleIncludeParents())
			{
				return;
			}
			if (!GameNetwork.IsClientOrReplay && this.NavigationMeshIdToDisableOnOpen != -1)
			{
				if (this._openNavMeshIdDisabled)
				{
					if (base.IsDestroyed)
					{
						base.Scene.SetAbilityOfFacesWithId(this.NavigationMeshIdToDisableOnOpen, true);
						this._openNavMeshIdDisabled = false;
					}
					else if (this.State == CastleGate.GateState.Closed)
					{
						int animationIndexAtChannel = this._doorSkeleton.GetAnimationIndexAtChannel(0);
						float animationParameterAtChannel = this._doorSkeleton.GetAnimationParameterAtChannel(0);
						if (animationIndexAtChannel != this._closingAnimationIndex || animationParameterAtChannel > 0.4f)
						{
							base.Scene.SetAbilityOfFacesWithId(this.NavigationMeshIdToDisableOnOpen, true);
							this._openNavMeshIdDisabled = false;
						}
					}
				}
				else if (this.State == CastleGate.GateState.Open && !base.IsDestroyed)
				{
					base.Scene.SetAbilityOfFacesWithId(this.NavigationMeshIdToDisableOnOpen, false);
					this._openNavMeshIdDisabled = true;
				}
			}
			if (this._afterMissionStartTriggered)
			{
				this.UpdateDoorBodies(false);
			}
			if (!GameNetwork.IsClientOrReplay)
			{
				this.ServerTick(dt);
			}
			if (base.Ai.HasActionCompleted)
			{
				bool flag = false;
				for (int i = 0; i < base.StandingPoints.Count; i++)
				{
					if (base.StandingPoints[i].HasUser || base.StandingPoints[i].HasAIMovingTo)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					bool flag2 = false;
					for (int j = 0; j < base.UserFormations.Count; j++)
					{
						if (base.UserFormations[j].CountOfDetachableNonPlayerUnits > 0)
						{
							flag2 = true;
							break;
						}
					}
					if (!flag2)
					{
						((CastleGateAI)base.Ai).ResetInitialGateState(this.State);
					}
				}
			}
		}

		// Token: 0x06002EDA RID: 11994 RVA: 0x000B5F14 File Offset: 0x000B4114
		protected override bool IsAgentOnInconvenientNavmesh(Agent agent, StandingPoint standingPoint)
		{
			if (Mission.Current.MissionTeamAIType != Mission.MissionTeamAITypeEnum.Siege)
			{
				return false;
			}
			int currentNavigationFaceId = agent.GetCurrentNavigationFaceId();
			TeamAISiegeComponent teamAISiegeComponent;
			if ((teamAISiegeComponent = agent.Team.TeamAI as TeamAISiegeComponent) != null && currentNavigationFaceId % 10 != 1)
			{
				if (base.GameEntity.HasTag("inner_gate"))
				{
					return true;
				}
				if (base.GameEntity.HasTag("outer_gate"))
				{
					CastleGate innerGate = teamAISiegeComponent.InnerGate;
					if (innerGate != null)
					{
						Vec3 vec = base.GameEntity.GlobalPosition - agent.Position;
						Vec3 vec2 = innerGate.GameEntity.GlobalPosition - agent.Position;
						if (vec.AsVec2.DotProduct(vec2.AsVec2) > 0f)
						{
							return true;
						}
					}
				}
				foreach (int num in (Mission.Current.DefenderTeam.TeamAI as TeamAISiegeDefender).DifficultNavmeshIDs)
				{
					if (currentNavigationFaceId == num)
					{
						return true;
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x06002EDB RID: 11995 RVA: 0x000B6048 File Offset: 0x000B4248
		private void ServerTick(float dt)
		{
			if (!this.IsDeactivated)
			{
				foreach (StandingPoint standingPoint in base.StandingPoints)
				{
					if (standingPoint.HasUser)
					{
						WeakGameEntity weakGameEntity = standingPoint.GameEntity;
						if (weakGameEntity.HasTag("open"))
						{
							this.OpenDoor();
							if (this.AutoOpen)
							{
								this.SetAutoOpenState(false);
							}
						}
						else
						{
							this.CloseDoor();
							if (Mission.Current.IsSallyOutBattle)
							{
								this.SetAutoOpenState(true);
							}
						}
					}
				}
				if (this.AutoOpen && this._pathChecker != null)
				{
					this._pathChecker.Tick(dt);
					if (this._pathChecker.HasAgentsUsingPath())
					{
						if (this.State != CastleGate.GateState.Open)
						{
							this.OpenDoor();
						}
					}
					else if (this.State != CastleGate.GateState.Closed)
					{
						this.CloseDoor();
					}
				}
				if (this._doorSkeleton != null && !base.IsDestroyed)
				{
					float animationParameterAtChannel = this._doorSkeleton.GetAnimationParameterAtChannel(0);
					foreach (StandingPoint standingPoint2 in base.StandingPoints)
					{
						bool flag;
						if (animationParameterAtChannel >= 1f)
						{
							WeakGameEntity weakGameEntity = standingPoint2.GameEntity;
							flag = weakGameEntity.HasTag((this.State == CastleGate.GateState.Open) ? "open" : "close");
						}
						else
						{
							flag = true;
						}
						bool flag2 = flag;
						standingPoint2.SetIsDeactivatedSynched(flag2);
					}
					if (animationParameterAtChannel >= 1f && this.State == CastleGate.GateState.Open)
					{
						if (this._extraColliderRight != null)
						{
							this._extraColliderRight.SetBodyFlags(this._extraColliderRight.BodyFlag | BodyFlags.Disabled);
							this._rightExtraColliderDisabled = true;
						}
						if (this._extraColliderLeft != null)
						{
							this._extraColliderLeft.SetBodyFlags(this._extraColliderLeft.BodyFlag | BodyFlags.Disabled);
							this._leftExtraColliderDisabled = true;
						}
					}
					if (this._plank != null && this.State == CastleGate.GateState.Closed && animationParameterAtChannel > 0.9f)
					{
						this._plank.SetVisibleSynched(true, false);
					}
				}
			}
		}

		// Token: 0x06002EDC RID: 11996 RVA: 0x000B6264 File Offset: 0x000B4464
		public TargetFlags GetTargetFlags()
		{
			TargetFlags targetFlags = TargetFlags.None;
			targetFlags |= TargetFlags.IsStructure;
			if (base.IsDestroyed)
			{
				targetFlags |= TargetFlags.NotAThreat;
			}
			if (DebugSiegeBehavior.DebugAttackState == DebugSiegeBehavior.DebugStateAttacker.DebugAttackersToBattlements)
			{
				targetFlags |= TargetFlags.DebugThreat;
			}
			return targetFlags;
		}

		// Token: 0x06002EDD RID: 11997 RVA: 0x000B6295 File Offset: 0x000B4495
		public float GetTargetValue(List<Vec3> weaponPos)
		{
			return 10f;
		}

		// Token: 0x06002EDE RID: 11998 RVA: 0x000B629C File Offset: 0x000B449C
		public WeakGameEntity GetTargetEntity()
		{
			return base.GameEntity;
		}

		// Token: 0x06002EDF RID: 11999 RVA: 0x000B62A4 File Offset: 0x000B44A4
		public BattleSideEnum GetSide()
		{
			return BattleSideEnum.Defender;
		}

		// Token: 0x06002EE0 RID: 12000 RVA: 0x000B62A7 File Offset: 0x000B44A7
		public Vec3 GetTargetGlobalVelocity()
		{
			return Vec3.Zero;
		}

		// Token: 0x06002EE1 RID: 12001 RVA: 0x000B62B0 File Offset: 0x000B44B0
		public bool IsDestructable()
		{
			return base.GameEntity.HasScriptOfType<DestructableComponent>();
		}

		// Token: 0x06002EE2 RID: 12002 RVA: 0x000B62CB File Offset: 0x000B44CB
		public WeakGameEntity Entity()
		{
			return base.GameEntity;
		}

		// Token: 0x06002EE3 RID: 12003 RVA: 0x000B62D4 File Offset: 0x000B44D4
		public ValueTuple<Vec3, Vec3> ComputeGlobalPhysicsBoundingBoxMinMax()
		{
			return base.GameEntity.ComputeGlobalPhysicsBoundingBoxMinMax();
		}

		// Token: 0x06002EE4 RID: 12004 RVA: 0x000B62F0 File Offset: 0x000B44F0
		protected void CollectGameEntities(bool calledFromOnInit)
		{
			this.CollectDynamicGameEntities(calledFromOnInit);
			if (!GameNetwork.IsClientOrReplay)
			{
				List<WeakGameEntity> list = base.GameEntity.CollectChildrenEntitiesWithTag("plank");
				if (list.Count > 0)
				{
					this._plank = list.FirstOrDefault<WeakGameEntity>().GetFirstScriptOfType<SynchedMissionObject>();
				}
			}
		}

		// Token: 0x06002EE5 RID: 12005 RVA: 0x000B633C File Offset: 0x000B453C
		protected void OnNextDestructionState()
		{
			this.CollectDynamicGameEntities(false);
			this.UpdateDoorBodies(true);
		}

		// Token: 0x06002EE6 RID: 12006 RVA: 0x000B634C File Offset: 0x000B454C
		protected void CollectDynamicGameEntities(bool calledFromOnInit)
		{
			this._attackOnlyDoorColliders.Clear();
			List<WeakGameEntity> list;
			if (calledFromOnInit)
			{
				list = base.GameEntity.CollectChildrenEntitiesWithTag("gate").ToList<WeakGameEntity>();
				this._leftExtraColliderDisabled = false;
				this._rightExtraColliderDisabled = false;
				this._agentColliderLeft = TaleWorlds.Engine.GameEntity.CreateFromWeakEntity(base.GameEntity.GetFirstChildEntityWithTag("collider_agent_l"));
				this._agentColliderRight = TaleWorlds.Engine.GameEntity.CreateFromWeakEntity(base.GameEntity.GetFirstChildEntityWithTag("collider_agent_r"));
			}
			else
			{
				list = (from x in base.GameEntity.CollectChildrenEntitiesWithTag("gate")
					where x.IsVisibleIncludeParents()
					select x).ToList<WeakGameEntity>();
			}
			if (list.Count == 0)
			{
				return;
			}
			if (list.Count > 1)
			{
				int num = int.MinValue;
				int num2 = int.MaxValue;
				WeakGameEntity weakGameEntity = WeakGameEntity.Invalid;
				WeakGameEntity weakGameEntity2 = WeakGameEntity.Invalid;
				foreach (WeakGameEntity weakGameEntity3 in list)
				{
					int num3 = int.Parse(weakGameEntity3.Tags.FirstOrDefault<string>((string x) => x.Contains("state_")).Split(new char[] { '_' }).Last<string>());
					if (num3 > num)
					{
						num = num3;
						weakGameEntity = weakGameEntity3;
					}
					if (num3 < num2)
					{
						num2 = num3;
						weakGameEntity2 = weakGameEntity3;
					}
				}
				this._door = (calledFromOnInit ? weakGameEntity2.GetFirstScriptOfType<SynchedMissionObject>() : weakGameEntity.GetFirstScriptOfType<SynchedMissionObject>());
			}
			else
			{
				this._door = list[0].GetFirstScriptOfType<SynchedMissionObject>();
			}
			this._doorSkeleton = this._door.GameEntity.Skeleton;
			WeakGameEntity weakGameEntity4 = this._door.GameEntity.CollectChildrenEntitiesWithTag("collider_r").FirstOrDefault<WeakGameEntity>();
			if (weakGameEntity4.IsValid)
			{
				this._attackOnlyDoorColliders.Add(TaleWorlds.Engine.GameEntity.CreateFromWeakEntity(weakGameEntity4));
			}
			WeakGameEntity weakGameEntity5 = this._door.GameEntity.CollectChildrenEntitiesWithTag("collider_l").FirstOrDefault<WeakGameEntity>();
			if (weakGameEntity5.IsValid)
			{
				this._attackOnlyDoorColliders.Add(TaleWorlds.Engine.GameEntity.CreateFromWeakEntity(weakGameEntity5));
			}
			if (!weakGameEntity4.IsValid || !weakGameEntity5.IsValid)
			{
				GameEntity agentColliderLeft = this._agentColliderLeft;
				if (agentColliderLeft != null)
				{
					agentColliderLeft.SetVisibilityExcludeParents(false);
				}
				GameEntity agentColliderRight = this._agentColliderRight;
				if (agentColliderRight != null)
				{
					agentColliderRight.SetVisibilityExcludeParents(false);
				}
			}
			WeakGameEntity weakGameEntity6 = this._door.GameEntity.CollectChildrenEntitiesWithTag(this.ExtraCollisionObjectTagLeft).FirstOrDefault<WeakGameEntity>();
			if (weakGameEntity6.IsValid)
			{
				if (!this.ActivateExtraColliders)
				{
					weakGameEntity6.RemovePhysics(false);
				}
				else
				{
					if (!calledFromOnInit)
					{
						MatrixFrame matrixFrame = ((this._extraColliderLeft != null) ? this._extraColliderLeft.GetFrame() : this._doorSkeleton.GetBoneEntitialFrameWithName(this.LeftDoorBoneName));
						this._extraColliderLeft = TaleWorlds.Engine.GameEntity.CreateFromWeakEntity(weakGameEntity6);
						this._extraColliderLeft.SetFrame(ref matrixFrame, true);
					}
					else
					{
						this._extraColliderLeft = TaleWorlds.Engine.GameEntity.CreateFromWeakEntity(weakGameEntity6);
					}
					if (this._leftExtraColliderDisabled)
					{
						this._extraColliderLeft.SetBodyFlags(this._extraColliderLeft.BodyFlag | BodyFlags.Disabled);
					}
					else
					{
						this._extraColliderLeft.SetBodyFlags(this._extraColliderLeft.BodyFlag & ~BodyFlags.Disabled);
					}
				}
			}
			WeakGameEntity weakGameEntity7 = this._door.GameEntity.CollectChildrenEntitiesWithTag(this.ExtraCollisionObjectTagRight).FirstOrDefault<WeakGameEntity>();
			if (weakGameEntity7.IsValid)
			{
				if (!this.ActivateExtraColliders)
				{
					weakGameEntity7.RemovePhysics(false);
				}
				else
				{
					if (!calledFromOnInit)
					{
						MatrixFrame matrixFrame2 = ((this._extraColliderRight != null) ? this._extraColliderRight.GetFrame() : this._doorSkeleton.GetBoneEntitialFrameWithName(this.RightDoorBoneName));
						this._extraColliderRight = TaleWorlds.Engine.GameEntity.CreateFromWeakEntity(weakGameEntity7);
						this._extraColliderRight.SetFrame(ref matrixFrame2, true);
					}
					else
					{
						this._extraColliderRight = TaleWorlds.Engine.GameEntity.CreateFromWeakEntity(weakGameEntity7);
					}
					if (this._rightExtraColliderDisabled)
					{
						this._extraColliderRight.SetBodyFlags(this._extraColliderRight.BodyFlag | BodyFlags.Disabled);
					}
					else
					{
						this._extraColliderRight.SetBodyFlags(this._extraColliderRight.BodyFlag & ~BodyFlags.Disabled);
					}
				}
			}
			if (this._door != null && this._doorSkeleton != null)
			{
				this._leftDoorBoneIndex = Skeleton.GetBoneIndexFromName(this._doorSkeleton.GetName(), this.LeftDoorBoneName);
				this._rightDoorBoneIndex = Skeleton.GetBoneIndexFromName(this._doorSkeleton.GetName(), this.RightDoorBoneName);
			}
		}

		// Token: 0x06002EE7 RID: 12007 RVA: 0x000B67CC File Offset: 0x000B49CC
		private void InitializeExtraColliderPositions()
		{
			if (this._extraColliderLeft != null)
			{
				MatrixFrame boneEntitialFrameWithName = this._doorSkeleton.GetBoneEntitialFrameWithName(this.LeftDoorBoneName);
				this._extraColliderLeft.SetFrame(ref boneEntitialFrameWithName, true);
				this._extraColliderLeft.SetVisibilityExcludeParents(true);
			}
			if (this._extraColliderRight != null)
			{
				MatrixFrame boneEntitialFrameWithName2 = this._doorSkeleton.GetBoneEntitialFrameWithName(this.RightDoorBoneName);
				this._extraColliderRight.SetFrame(ref boneEntitialFrameWithName2, true);
				this._extraColliderRight.SetVisibilityExcludeParents(true);
			}
			this.UpdateDoorBodies(true);
			foreach (GameEntity gameEntity in this._attackOnlyDoorColliders)
			{
				gameEntity.SetVisibilityExcludeParents(true);
			}
			if (this._agentColliderLeft != null)
			{
				this._agentColliderLeft.SetVisibilityExcludeParents(true);
			}
			if (this._agentColliderRight != null)
			{
				this._agentColliderRight.SetVisibilityExcludeParents(true);
			}
		}

		// Token: 0x06002EE8 RID: 12008 RVA: 0x000B68CC File Offset: 0x000B4ACC
		private void OnHitTaken(DestructableComponent hitComponent, Agent hitterAgent, in MissionWeapon weapon, ScriptComponentBehavior attackerScriptComponentBehavior, int inflictedDamage)
		{
			if (!GameNetwork.IsClientOrReplay && inflictedDamage >= 200 && this.State == CastleGate.GateState.Closed && attackerScriptComponentBehavior is BatteringRam)
			{
				SynchedMissionObject plank = this._plank;
				if (plank != null)
				{
					plank.SetAnimationAtChannelSynched(this.PlankHitAnimationName, 0, 1f);
				}
				this._door.SetAnimationAtChannelSynched(this.HitAnimationName, 0, 1f);
				Mission.Current.MakeSound(CastleGate.BatteringRamHitSoundIdCache, base.GameEntity.GlobalPosition, false, true, -1, -1);
			}
		}

		// Token: 0x06002EE9 RID: 12009 RVA: 0x000B6950 File Offset: 0x000B4B50
		private void OnDestroyed(DestructableComponent destroyedComponent, Agent destroyerAgent, in MissionWeapon weapon, ScriptComponentBehavior attackerScriptComponentBehavior, int inflictedDamage)
		{
			if (!GameNetwork.IsClientOrReplay)
			{
				SynchedMissionObject plank = this._plank;
				if (plank != null)
				{
					plank.SetVisibleSynched(false, false);
				}
				foreach (StandingPoint standingPoint in base.StandingPoints)
				{
					standingPoint.SetIsDeactivatedSynched(true);
				}
				if (attackerScriptComponentBehavior is BatteringRam)
				{
					this._door.SetAnimationAtChannelSynched(this.DestroyAnimationName, 0, 1f);
				}
				this.SetGateNavMeshState(true);
			}
		}

		// Token: 0x06002EEA RID: 12010 RVA: 0x000B69E4 File Offset: 0x000B4BE4
		private int OnCalculateDestructionStateIndex(int destructionStateIndex, int inflictedDamage, int destructionStateCount)
		{
			if (inflictedDamage < 200)
			{
				return destructionStateIndex;
			}
			return MathF.Min(destructionStateIndex, destructionStateCount - 1);
		}

		// Token: 0x06002EEB RID: 12011 RVA: 0x000B69FC File Offset: 0x000B4BFC
		protected internal override bool OnCheckForProblems()
		{
			bool flag = base.OnCheckForProblems();
			if (base.GameEntity.HasTag("outer_gate") && base.GameEntity.HasTag("inner_gate"))
			{
				MBEditor.AddEntityWarning(base.GameEntity, "This castle gate has both outer and inner tag at the same time.");
				flag = true;
			}
			if (base.GameEntity.CollectChildrenEntitiesWithTag("wait_pos").Count != 1)
			{
				MBEditor.AddEntityWarning(base.GameEntity, "There must be one entity with wait position tag under castle gate.");
				flag = true;
			}
			if (base.GameEntity.HasTag("outer_gate"))
			{
				uint visibilityMask = base.GameEntity.GetVisibilityLevelMaskIncludingParents();
				WeakGameEntity weakGameEntity = base.GameEntity.GetChildren().FirstOrDefault<WeakGameEntity>((WeakGameEntity x) => x.HasTag("middle_pos") && x.GetVisibilityLevelMaskIncludingParents() == visibilityMask);
				if (weakGameEntity.IsValid)
				{
					WeakGameEntity weakGameEntity2 = base.Scene.FindWeakEntitiesWithTag("inner_gate").FirstOrDefault<WeakGameEntity>((WeakGameEntity x) => x.GetVisibilityLevelMaskIncludingParents() == visibilityMask);
					if (weakGameEntity2 != null)
					{
						if (weakGameEntity2.HasScriptOfType<CastleGate>())
						{
							Vec2 vec = weakGameEntity2.GlobalPosition.AsVec2 - weakGameEntity.GlobalPosition.AsVec2;
							Vec2 vec2 = base.GameEntity.GlobalPosition.AsVec2 - weakGameEntity.GlobalPosition.AsVec2;
							if (Vec2.DotProduct(vec, vec2) <= 0f)
							{
								MBEditor.AddEntityWarning(base.GameEntity, "Outer gate's middle position must not be between outer and inner gate.");
								flag = true;
							}
						}
						else
						{
							MBEditor.AddEntityWarning(base.GameEntity, weakGameEntity2.Name + " this entity has inner gate tag but doesn't have castle gate script.");
							flag = true;
						}
					}
					else
					{
						MBEditor.AddEntityWarning(base.GameEntity, "There is no entity with inner gate tag.");
						flag = true;
					}
				}
				else
				{
					MBEditor.AddEntityWarning(base.GameEntity, "Outer gate doesn't have any middle positions");
					flag = true;
				}
			}
			Vec3 scaleVector = base.GameEntity.GetGlobalFrame().rotation.GetScaleVector();
			if (MathF.Abs(scaleVector.x - scaleVector.y) > 1E-05f || MathF.Abs(scaleVector.x - scaleVector.z) > 1E-05f || MathF.Abs(scaleVector.y - scaleVector.z) > 1E-05f)
			{
				MBEditor.AddEntityWarning(base.GameEntity, "$$$ Non uniform scale on CastleGate at scene " + base.GameEntity.Scene.GetName());
				flag = true;
			}
			return flag;
		}

		// Token: 0x06002EEC RID: 12012 RVA: 0x000B6C65 File Offset: 0x000B4E65
		public Vec3 GetTargetingOffset()
		{
			return Vec3.Zero;
		}

		// Token: 0x040012C1 RID: 4801
		public const string OuterGateTag = "outer_gate";

		// Token: 0x040012C2 RID: 4802
		public const string InnerGateTag = "inner_gate";

		// Token: 0x040012C3 RID: 4803
		private const float ExtraColliderScaleFactor = 1.1f;

		// Token: 0x040012C4 RID: 4804
		private const string LeftDoorBodyTag = "collider_l";

		// Token: 0x040012C5 RID: 4805
		private const string RightDoorBodyTag = "collider_r";

		// Token: 0x040012C6 RID: 4806
		private const string RightDoorAgentOnlyBodyTag = "collider_agent_r";

		// Token: 0x040012C7 RID: 4807
		private const string OpenTag = "open";

		// Token: 0x040012C8 RID: 4808
		private const string CloseTag = "close";

		// Token: 0x040012C9 RID: 4809
		private const string MiddlePositionTag = "middle_pos";

		// Token: 0x040012CA RID: 4810
		private const string WaitPositionTag = "wait_pos";

		// Token: 0x040012CB RID: 4811
		private const string LeftDoorAgentOnlyBodyTag = "collider_agent_l";

		// Token: 0x040012CC RID: 4812
		private const int HeavyBlowDamageLimit = 200;

		// Token: 0x040012CE RID: 4814
		private static int _batteringRamHitSoundId = -1;

		// Token: 0x040012D0 RID: 4816
		public CastleGate.DoorOwnership OwningTeam;

		// Token: 0x040012D1 RID: 4817
		public string OpeningAnimationName = "castle_gate_a_opening";

		// Token: 0x040012D2 RID: 4818
		public string ClosingAnimationName = "castle_gate_a_closing";

		// Token: 0x040012D3 RID: 4819
		public string HitAnimationName = "castle_gate_a_hit";

		// Token: 0x040012D4 RID: 4820
		public string PlankHitAnimationName = "castle_gate_a_plank_hit";

		// Token: 0x040012D5 RID: 4821
		public string HitMeleeAnimationName = "castle_gate_a_hit_melee";

		// Token: 0x040012D6 RID: 4822
		public string DestroyAnimationName = "castle_gate_a_break";

		// Token: 0x040012D7 RID: 4823
		public int NavigationMeshId = 1000;

		// Token: 0x040012D8 RID: 4824
		public int NavigationMeshIdToDisableOnOpen = -1;

		// Token: 0x040012D9 RID: 4825
		public string LeftDoorBoneName = "bn_bottom_l";

		// Token: 0x040012DA RID: 4826
		public string RightDoorBoneName = "bn_bottom_r";

		// Token: 0x040012DB RID: 4827
		public string ExtraCollisionObjectTagRight = "extra_collider_r";

		// Token: 0x040012DC RID: 4828
		public string ExtraCollisionObjectTagLeft = "extra_collider_l";

		// Token: 0x040012DD RID: 4829
		private int _openingAnimationIndex = -1;

		// Token: 0x040012DE RID: 4830
		private int _closingAnimationIndex = -1;

		// Token: 0x040012DF RID: 4831
		private bool _leftExtraColliderDisabled;

		// Token: 0x040012E0 RID: 4832
		private bool _rightExtraColliderDisabled;

		// Token: 0x040012E1 RID: 4833
		private bool _civilianMission;

		// Token: 0x040012E2 RID: 4834
		public bool ActivateExtraColliders = true;

		// Token: 0x040012E3 RID: 4835
		public string SideTag;

		// Token: 0x040012E5 RID: 4837
		private bool _openNavMeshIdDisabled;

		// Token: 0x040012E6 RID: 4838
		private SynchedMissionObject _door;

		// Token: 0x040012E7 RID: 4839
		private Skeleton _doorSkeleton;

		// Token: 0x040012E8 RID: 4840
		private GameEntity _extraColliderRight;

		// Token: 0x040012E9 RID: 4841
		private GameEntity _extraColliderLeft;

		// Token: 0x040012EA RID: 4842
		private readonly List<GameEntity> _attackOnlyDoorColliders;

		// Token: 0x040012EB RID: 4843
		private float _previousAnimationProgress = -1f;

		// Token: 0x040012EC RID: 4844
		private GameEntity _agentColliderRight;

		// Token: 0x040012ED RID: 4845
		private GameEntity _agentColliderLeft;

		// Token: 0x040012EE RID: 4846
		private LadderQueueManager _queueManager;

		// Token: 0x040012EF RID: 4847
		private bool _afterMissionStartTriggered;

		// Token: 0x040012F0 RID: 4848
		private sbyte _rightDoorBoneIndex;

		// Token: 0x040012F1 RID: 4849
		private sbyte _leftDoorBoneIndex;

		// Token: 0x040012F4 RID: 4852
		private AgentPathNavMeshChecker _pathChecker;

		// Token: 0x040012F5 RID: 4853
		public bool AutoOpen;

		// Token: 0x040012F6 RID: 4854
		private SynchedMissionObject _plank;

		// Token: 0x040012F8 RID: 4856
		private WorldFrame _middleFrame;

		// Token: 0x040012F9 RID: 4857
		private WorldFrame _defenseWaitFrame;

		// Token: 0x040012FA RID: 4858
		private Action DestructibleComponentOnMissionReset;

		// Token: 0x0200060F RID: 1551
		public enum DoorOwnership
		{
			// Token: 0x04002054 RID: 8276
			Defenders,
			// Token: 0x04002055 RID: 8277
			Attackers
		}

		// Token: 0x02000610 RID: 1552
		public enum GateState
		{
			// Token: 0x04002057 RID: 8279
			Open,
			// Token: 0x04002058 RID: 8280
			Closed
		}
	}
}
