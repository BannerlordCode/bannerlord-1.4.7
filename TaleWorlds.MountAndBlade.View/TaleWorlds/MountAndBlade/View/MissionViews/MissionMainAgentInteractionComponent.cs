using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View.Screens;

namespace TaleWorlds.MountAndBlade.View.MissionViews
{
	// Token: 0x0200007E RID: 126
	public class MissionMainAgentInteractionComponent
	{
		// Token: 0x14000006 RID: 6
		// (add) Token: 0x060004AE RID: 1198 RVA: 0x00023CEC File Offset: 0x00021EEC
		// (remove) Token: 0x060004AF RID: 1199 RVA: 0x00023D24 File Offset: 0x00021F24
		public event MissionMainAgentInteractionComponent.MissionFocusGainedEventDelegate OnFocusGained;

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x060004B0 RID: 1200 RVA: 0x00023D5C File Offset: 0x00021F5C
		// (remove) Token: 0x060004B1 RID: 1201 RVA: 0x00023D94 File Offset: 0x00021F94
		public event MissionMainAgentInteractionComponent.MissionFocusLostEventDelegate OnFocusLost;

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x060004B2 RID: 1202 RVA: 0x00023DCC File Offset: 0x00021FCC
		// (remove) Token: 0x060004B3 RID: 1203 RVA: 0x00023E04 File Offset: 0x00022004
		public event MissionMainAgentInteractionComponent.MissionFocusHealthChangeDelegate OnFocusHealthChanged;

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060004B4 RID: 1204 RVA: 0x00023E39 File Offset: 0x00022039
		// (set) Token: 0x060004B5 RID: 1205 RVA: 0x00023E41 File Offset: 0x00022041
		public IFocusable CurrentFocusedObject { get; private set; }

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060004B6 RID: 1206 RVA: 0x00023E4A File Offset: 0x0002204A
		// (set) Token: 0x060004B7 RID: 1207 RVA: 0x00023E52 File Offset: 0x00022052
		public IFocusable CurrentFocusedMachine { get; private set; }

		// Token: 0x060004B8 RID: 1208 RVA: 0x00023E5C File Offset: 0x0002205C
		public void SetCurrentFocusedObject(IFocusable focusedObject, IFocusable focusedMachine, sbyte focusedObjectBoneIndex, bool isInteractable)
		{
			if (this.CurrentFocusedObject != null && (this.CurrentFocusedObject != focusedObject || (this._currentInteractableObject != null && !isInteractable) || (this._currentInteractableObject == null && isInteractable)))
			{
				this.FocusLost(this.CurrentFocusedObject, this.CurrentFocusedMachine);
				this._currentInteractableObject = null;
				this._currentInteractableObjectBoneIndex = -1;
				this.CurrentFocusedObject = null;
				this.CurrentFocusedMachine = null;
			}
			if (this.CurrentFocusedObject == null && focusedObject != null)
			{
				if (focusedObject != this.CurrentFocusedObject)
				{
					this.FocusGained(focusedObject, focusedMachine, isInteractable);
				}
				if (isInteractable)
				{
					this._currentInteractableObject = focusedObject;
				}
				this.CurrentFocusedObject = focusedObject;
				this.CurrentFocusedMachine = focusedMachine;
			}
			if (this._currentInteractableObject != null && this._currentInteractableObject == focusedObject)
			{
				this._currentInteractableObjectBoneIndex = focusedObjectBoneIndex;
			}
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x00023F11 File Offset: 0x00022111
		public void ClearFocus()
		{
			if (this.CurrentFocusedObject != null)
			{
				this.FocusLost(this.CurrentFocusedObject, this.CurrentFocusedMachine);
			}
			this._currentInteractableObject = null;
			this._currentInteractableObjectBoneIndex = -1;
			this.CurrentFocusedObject = null;
			this.CurrentFocusedMachine = null;
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x00023F49 File Offset: 0x00022149
		public void OnClearScene()
		{
			this.ClearFocus();
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060004BB RID: 1211 RVA: 0x00023F51 File Offset: 0x00022151
		private Mission CurrentMission
		{
			get
			{
				return this._mainAgentController.Mission;
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060004BC RID: 1212 RVA: 0x00023F5E File Offset: 0x0002215E
		private MissionScreen CurrentMissionScreen
		{
			get
			{
				return this._mainAgentController.MissionScreen;
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060004BD RID: 1213 RVA: 0x00023F6B File Offset: 0x0002216B
		private Scene CurrentMissionScene
		{
			get
			{
				return this._mainAgentController.Mission.Scene;
			}
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x00023F7D File Offset: 0x0002217D
		public MissionMainAgentInteractionComponent(MissionMainAgentController mainAgentController)
		{
			this._mainAgentController = mainAgentController;
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x00023F8C File Offset: 0x0002218C
		private static float GetCollisionDistanceSquaredOfIntersectionFromMainAgentEye(Vec3 rayStartPoint, Vec3 rayDirection, float rayLength)
		{
			float num = rayLength * rayLength;
			Vec3 vec = rayStartPoint + rayDirection * rayLength;
			Vec3 position = Agent.Main.Position;
			float eyeGlobalHeight = Agent.Main.GetEyeGlobalHeight();
			Vec3 vec2 = new Vec3(position.x, position.y, position.z + eyeGlobalHeight, -1f);
			float num2 = vec.z - vec2.z;
			if (num2 < 0f)
			{
				num2 = MBMath.ClampFloat(-num2, 0f, (Agent.Main.HasMount ? (eyeGlobalHeight - Agent.Main.MountAgent.GetEyeGlobalHeight()) : eyeGlobalHeight) * 0.75f);
				vec2.z -= num2;
				num = vec2.DistanceSquared(vec);
			}
			return num;
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x00024048 File Offset: 0x00022248
		private void FocusGained(IFocusable focusedObject, IFocusable focusedMachine, bool isInteractable)
		{
			focusedObject.OnFocusGain(Agent.Main);
			if (focusedMachine != null)
			{
				focusedMachine.OnFocusGain(Agent.Main);
			}
			foreach (MissionBehavior missionBehavior in this.CurrentMission.MissionBehaviors)
			{
				missionBehavior.OnFocusGained(Agent.Main, focusedObject, isInteractable);
			}
			MissionMainAgentInteractionComponent.MissionFocusGainedEventDelegate onFocusGained = this.OnFocusGained;
			if (onFocusGained == null)
			{
				return;
			}
			onFocusGained(Agent.Main, focusedObject, isInteractable);
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x000240D4 File Offset: 0x000222D4
		private void FocusLost(IFocusable focusedObject, IFocusable focusedMachine)
		{
			focusedObject.OnFocusLose(Agent.Main);
			if (focusedMachine != null)
			{
				focusedMachine.OnFocusLose(Agent.Main);
			}
			foreach (MissionBehavior missionBehavior in this.CurrentMission.MissionBehaviors)
			{
				missionBehavior.OnFocusLost(Agent.Main, focusedObject);
			}
			MissionMainAgentInteractionComponent.MissionFocusLostEventDelegate onFocusLost = this.OnFocusLost;
			if (onFocusLost == null)
			{
				return;
			}
			onFocusLost(Agent.Main, focusedObject);
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x00024160 File Offset: 0x00022360
		public void FocusTick()
		{
			IFocusable focusable = null;
			sbyte b = -1;
			UsableMachine usableMachine = null;
			bool flag = false;
			bool flag2 = false;
			if (Mission.Current.Mode == MissionMode.Conversation || Mission.Current.Mode == MissionMode.CutScene)
			{
				if (this.CurrentFocusedObject != null && Mission.Current.Mode != MissionMode.Conversation)
				{
					this.ClearFocus();
				}
				return;
			}
			Agent main = Agent.Main;
			if (main != null && (this.CurrentMission.IsMainAgentItemInteractionEnabled || this.IsFocusMountable()) && !this.CurrentMission.IsOrderMenuOpen && !this.CurrentMissionScreen.SceneLayer.Input.IsGameKeyDown(25) && main.IsAbleToUseMachine())
			{
				float num = 10f;
				Vec3 direction = this.CurrentMissionScreen.CombatCamera.Direction;
				Vec3 vec = direction;
				Vec3 position = this.CurrentMissionScreen.CombatCamera.Position;
				Vec3 position2 = main.Position;
				Vec3 vec2 = new Vec3(position.x, position.y, 0f, -1f);
				float num2 = vec2.Distance(new Vec3(position2.x, position2.y, 0f, -1f));
				Vec3 vec3 = position * (1f - num2) + (position + direction) * num2;
				float num3;
				WeakGameEntity parent;
				if (this.CurrentMissionScene.FocusRayCastForFixedPhysics(vec3, vec3 + vec * num, out num3, out vec2, out parent, 0.01f, (BodyFlags)4043259711U))
				{
					num = num3;
				}
				if (this.CurrentMissionScene.RayCastForClosestEntityOrTerrain(vec3, vec3 + vec * num, out num3, 0.01f, (BodyFlags)4043259711U) && num3 < num)
				{
					num = num3;
				}
				float num4 = float.MaxValue;
				Agent agent = null;
				float num5;
				Agent agent2 = this.CurrentMission.RayCastForClosestAgent(vec3, vec3 + vec * (num + 0.01f), main.Index, 0.3f, out num5);
				if (agent2 != null && agent2.State != AgentState.Killed && agent2.State != AgentState.Unconscious && (!agent2.IsMount || (agent2.RiderAgent == null && main.MountAgent == null && main.CanReachAgent(agent2))))
				{
					flag2 = main.CanInteractWithAgent(agent2, this.CurrentMissionScreen.CameraElevation);
					if (flag2 || agent2.IsEnemyOf(main))
					{
						num4 = num5;
						focusable = agent2;
						b = -1;
					}
					else
					{
						agent = agent2;
					}
				}
				float num6;
				sbyte b2;
				Agent agent3 = this.CurrentMission.RayCastForClosestAgentsLimbs(vec3, vec3 + vec * (num + 0.01f), main.Index, 0.3f, out num6, out b2);
				if (agent3 != null && (agent3.State == AgentState.Killed || agent3.State == AgentState.Unconscious) && (!agent3.IsMount || (agent3.RiderAgent == null && main.MountAgent == null && main.CanReachAgent(agent3))) && num4 > num6)
				{
					flag2 = main.CanInteractWithAgent(agent3, this.CurrentMissionScreen.CameraElevation);
					if (flag2 || agent3.IsEnemyOf(main))
					{
						num4 = num6;
						focusable = agent3;
						b = b2;
					}
				}
				float num7 = 3f;
				num += 0.1f;
				WeakGameEntity weakGameEntity = WeakGameEntity.Invalid;
				float num8 = 0f;
				bool flag3 = false;
				float num9;
				WeakGameEntity weakGameEntity2;
				if (this.CurrentMissionScene.FocusRayCastForFixedPhysics(vec3, vec3 + vec * num, out num9, out vec2, out weakGameEntity2, 0.2f, BodyFlags.CommonFocusRayCastExcludeFlags) && num9 < num && num9 < num4)
				{
					num = num9;
					num8 = num9;
					weakGameEntity = weakGameEntity2;
					flag3 = weakGameEntity.IsValid;
				}
				bool flag4 = false;
				for (int i = 0; i < 2; i++)
				{
					float num10 = MathF.Lerp(1f, num7, (float)i / 1f, 1E-05f);
					float num11 = 0.2f * (num10 - 1f);
					if (this.CurrentMissionScene.RayCastForClosestEntityOrTerrain(vec3 + vec * num11, vec3 + vec * num, out num9, out weakGameEntity2, 0.2f * num10, BodyFlags.CommonFocusRayCastExcludeFlags) && num9 + num11 < num && num9 + num11 < num4)
					{
						bool flag5 = false;
						WeakGameEntity weakGameEntity3 = weakGameEntity2;
						while (weakGameEntity3.IsValid)
						{
							if (weakGameEntity3.HasScriptWithInterfaceOfType<IFocusable>() && weakGameEntity3.GetFirstScriptWithInterfaceOfType<IFocusable>().IsFocusable)
							{
								flag5 = true;
								break;
							}
							weakGameEntity3 = weakGameEntity3.Parent;
						}
						if (!flag4 || flag5)
						{
							num = num9 + num11;
							num8 = num9 + num11;
							weakGameEntity = weakGameEntity2;
							flag3 = weakGameEntity.IsValid;
							flag4 = true;
							if (flag5)
							{
								break;
							}
						}
					}
				}
				if (flag3)
				{
					while (!weakGameEntity.HasScriptWithInterfaceOfType<IFocusable>() || !weakGameEntity.GetFirstScriptWithInterfaceOfType<IFocusable>().IsFocusable)
					{
						parent = weakGameEntity.Parent;
						if (!parent.IsValid)
						{
							break;
						}
						weakGameEntity = weakGameEntity.Parent;
					}
					usableMachine = weakGameEntity.GetFirstScriptOfType<UsableMachine>();
					if (usableMachine != null && !usableMachine.IsDisabled)
					{
						WeakGameEntity validVacantReachableStandingPointForAgent = usableMachine.GetValidVacantReachableStandingPointForAgent(main);
						if (validVacantReachableStandingPointForAgent.IsValid)
						{
							weakGameEntity = validVacantReachableStandingPointForAgent;
						}
					}
					UsableMissionObject firstScriptOfType = weakGameEntity.GetFirstScriptOfType<UsableMissionObject>();
					if (firstScriptOfType is SpawnedItemEntity)
					{
						if (this.CurrentMission.IsMainAgentItemInteractionEnabled && firstScriptOfType.IsFocusable && !main.IsInWater() && main.CanReachObject(firstScriptOfType, MissionMainAgentInteractionComponent.GetCollisionDistanceSquaredOfIntersectionFromMainAgentEye(vec3, vec, num8)))
						{
							focusable = firstScriptOfType;
							b = -1;
							if (main.CanUseObject(firstScriptOfType))
							{
								flag = true;
							}
						}
					}
					else if (firstScriptOfType != null)
					{
						if (firstScriptOfType.IsFocusable)
						{
							focusable = firstScriptOfType;
							b = -1;
							if (this.CurrentMission.IsMainAgentObjectInteractionEnabled && !main.IsUsingGameObject && main.IsAbleToUseMachine() && main.ObjectHasVacantPosition(firstScriptOfType) && main.CanUseObject(firstScriptOfType))
							{
								flag = true;
							}
						}
					}
					else if (usableMachine != null)
					{
						if (usableMachine.IsFocusable)
						{
							focusable = usableMachine;
							b = -1;
							flag = !usableMachine.IsDeactivated;
						}
					}
					else
					{
						IFocusable firstScriptWithInterfaceOfType = weakGameEntity.GetFirstScriptWithInterfaceOfType<IFocusable>();
						if (firstScriptWithInterfaceOfType != null && firstScriptWithInterfaceOfType.IsFocusable)
						{
							focusable = firstScriptWithInterfaceOfType;
							b = -1;
						}
					}
				}
				if ((focusable == null || !flag) && main.MountAgent != null && main.CanInteractWithAgent(main.MountAgent, this.CurrentMissionScreen.CameraElevation))
				{
					focusable = main.MountAgent;
					b = -1;
					flag2 = true;
				}
				if (focusable == null && agent != null)
				{
					focusable = agent;
					flag2 = true;
				}
			}
			if (focusable == null)
			{
				this.ClearFocus();
				return;
			}
			bool flag6 = ((focusable is Agent) ? flag2 : flag);
			this.SetCurrentFocusedObject(focusable, usableMachine, b, flag6);
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x0002479C File Offset: 0x0002299C
		public void FocusStateCheckTick()
		{
			if (this.CurrentMissionScreen.SceneLayer.Input.IsGameKeyPressed(13) && (this.CurrentMission.IsMainAgentItemInteractionEnabled || this.IsFocusMountable()) && !this.CurrentMissionScreen.IsRadialMenuActive && !this.CurrentMission.IsOrderMenuOpen)
			{
				Agent main = Agent.Main;
				StandingPoint standingPoint;
				if (main.IsUsingGameObject && !(main.CurrentlyUsedGameObject is SpawnedItemEntity) && (!(this._currentInteractableObject is Agent) || (standingPoint = main.CurrentlyUsedGameObject as StandingPoint) == null || standingPoint.PlayerStopsUsingWhenInteractsWithOther))
				{
					main.HandleStopUsingAction();
					this.ClearFocus();
					return;
				}
				UsableMissionObject usableMissionObject;
				if ((usableMissionObject = this._currentInteractableObject as UsableMissionObject) != null)
				{
					if (!main.IsUsingGameObject && main.IsAbleToUseMachine() && !(usableMissionObject is SpawnedItemEntity) && main.ObjectHasVacantPosition(usableMissionObject))
					{
						main.HandleStartUsingAction(usableMissionObject, -1);
						return;
					}
				}
				else
				{
					Agent agent = this._currentInteractableObject as Agent;
					if (main.IsAbleToUseMachine() && agent != null)
					{
						agent.OnUse(main, this._currentInteractableObjectBoneIndex);
					}
				}
			}
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x000248A8 File Offset: 0x00022AA8
		private bool IsFocusMountable()
		{
			Agent agent = this._currentInteractableObject as Agent;
			return agent != null && agent.IsMount;
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x000248CC File Offset: 0x00022ACC
		public void FocusedItemHealthTick()
		{
			UsableMissionObject usableMissionObject;
			UsableMachine usableMachine;
			DestructableComponent destructableComponent;
			if ((usableMissionObject = this.CurrentFocusedObject as UsableMissionObject) != null)
			{
				WeakGameEntity weakGameEntity = usableMissionObject.GameEntity;
				while (weakGameEntity.IsValid && !weakGameEntity.HasScriptOfType<UsableMachine>())
				{
					weakGameEntity = weakGameEntity.Parent;
				}
				if (weakGameEntity.IsValid)
				{
					UsableMachine firstScriptOfType = weakGameEntity.GetFirstScriptOfType<UsableMachine>();
					if (((firstScriptOfType != null) ? firstScriptOfType.DestructionComponent : null) != null)
					{
						MissionMainAgentInteractionComponent.MissionFocusHealthChangeDelegate onFocusHealthChanged = this.OnFocusHealthChanged;
						if (onFocusHealthChanged == null)
						{
							return;
						}
						onFocusHealthChanged(this.CurrentFocusedObject, firstScriptOfType.DestructionComponent.HitPoint / firstScriptOfType.DestructionComponent.MaxHitPoint, true);
						return;
					}
				}
			}
			else if ((usableMachine = this.CurrentFocusedObject as UsableMachine) != null)
			{
				if (usableMachine.DestructionComponent != null)
				{
					MissionMainAgentInteractionComponent.MissionFocusHealthChangeDelegate onFocusHealthChanged2 = this.OnFocusHealthChanged;
					if (onFocusHealthChanged2 == null)
					{
						return;
					}
					onFocusHealthChanged2(this.CurrentFocusedObject, usableMachine.DestructionComponent.HitPoint / usableMachine.DestructionComponent.MaxHitPoint, true);
					return;
				}
			}
			else if ((destructableComponent = this.CurrentFocusedObject as DestructableComponent) != null)
			{
				MissionMainAgentInteractionComponent.MissionFocusHealthChangeDelegate onFocusHealthChanged3 = this.OnFocusHealthChanged;
				if (onFocusHealthChanged3 == null)
				{
					return;
				}
				onFocusHealthChanged3(this.CurrentFocusedObject, destructableComponent.HitPoint / destructableComponent.MaxHitPoint, true);
			}
		}

		// Token: 0x040002BC RID: 700
		private IFocusable _currentInteractableObject;

		// Token: 0x040002BD RID: 701
		private sbyte _currentInteractableObjectBoneIndex;

		// Token: 0x040002C0 RID: 704
		private readonly MissionMainAgentController _mainAgentController;

		// Token: 0x020000E2 RID: 226
		// (Invoke) Token: 0x06000652 RID: 1618
		public delegate void MissionFocusGainedEventDelegate(Agent agent, IFocusable focusableObject, bool isInteractable);

		// Token: 0x020000E3 RID: 227
		// (Invoke) Token: 0x06000656 RID: 1622
		public delegate void MissionFocusLostEventDelegate(Agent agent, IFocusable focusableObject);

		// Token: 0x020000E4 RID: 228
		// (Invoke) Token: 0x0600065A RID: 1626
		public delegate void MissionFocusHealthChangeDelegate(IFocusable focusable, float healthPercentage, bool hideHealthbarWhenFull);
	}
}
