using System;
using System.Collections.Generic;
using NetworkMessages.FromClient;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Engine.Options;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;

namespace TaleWorlds.MountAndBlade.View.MissionViews
{
	// Token: 0x02000079 RID: 121
	[DefaultView]
	public class MissionMainAgentController : MissionView
	{
		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000482 RID: 1154 RVA: 0x00022124 File Offset: 0x00020324
		// (remove) Token: 0x06000483 RID: 1155 RVA: 0x0002215C File Offset: 0x0002035C
		public event MissionMainAgentController.OnLockedAgentChangedDelegate OnLockedAgentChanged;

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000484 RID: 1156 RVA: 0x00022194 File Offset: 0x00020394
		// (remove) Token: 0x06000485 RID: 1157 RVA: 0x000221CC File Offset: 0x000203CC
		public event MissionMainAgentController.OnPotentialLockedAgentChangedDelegate OnPotentialLockedAgentChanged;

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000486 RID: 1158 RVA: 0x00022201 File Offset: 0x00020401
		// (set) Token: 0x06000487 RID: 1159 RVA: 0x00022209 File Offset: 0x00020409
		public bool IsDisabled { get; set; }

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000488 RID: 1160 RVA: 0x00022212 File Offset: 0x00020412
		// (set) Token: 0x06000489 RID: 1161 RVA: 0x0002221A File Offset: 0x0002041A
		public Vec3 CustomLookDir { get; set; }

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x0600048A RID: 1162 RVA: 0x00022224 File Offset: 0x00020424
		public bool IsPlayerAiming
		{
			get
			{
				if (this._isPlayerAiming)
				{
					return true;
				}
				if (base.Mission.MainAgent == null)
				{
					return false;
				}
				bool flag = false;
				bool flag2 = false;
				bool flag3 = false;
				if (base.Input != null)
				{
					flag2 = base.Input.IsGameKeyDown(9);
				}
				if (base.Mission.MainAgent != null)
				{
					if (base.Mission.MainAgent.WieldedWeapon.CurrentUsageItem != null)
					{
						flag = base.Mission.MainAgent.WieldedWeapon.CurrentUsageItem.IsRangedWeapon || base.Mission.MainAgent.WieldedWeapon.CurrentUsageItem.IsAmmo;
					}
					flag3 = base.Mission.MainAgent.MovementFlags.HasAnyFlag(Agent.MovementControlFlag.AttackMask);
				}
				return flag && flag2 && flag3;
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x0600048B RID: 1163 RVA: 0x000222ED File Offset: 0x000204ED
		// (set) Token: 0x0600048C RID: 1164 RVA: 0x000222F5 File Offset: 0x000204F5
		public Agent LockedAgent
		{
			get
			{
				return this._lockedAgent;
			}
			private set
			{
				if (this._lockedAgent != value)
				{
					this._lockedAgent = value;
					MissionMainAgentController.OnLockedAgentChangedDelegate onLockedAgentChanged = this.OnLockedAgentChanged;
					if (onLockedAgentChanged == null)
					{
						return;
					}
					onLockedAgentChanged(value);
				}
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x0600048D RID: 1165 RVA: 0x00022318 File Offset: 0x00020518
		// (set) Token: 0x0600048E RID: 1166 RVA: 0x00022320 File Offset: 0x00020520
		public Agent PotentialLockTargetAgent
		{
			get
			{
				return this._potentialLockTargetAgent;
			}
			private set
			{
				if (this._potentialLockTargetAgent != value)
				{
					this._potentialLockTargetAgent = value;
					MissionMainAgentController.OnPotentialLockedAgentChangedDelegate onPotentialLockedAgentChanged = this.OnPotentialLockedAgentChanged;
					if (onPotentialLockedAgentChanged == null)
					{
						return;
					}
					onPotentialLockedAgentChanged(value);
				}
			}
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x00022343 File Offset: 0x00020543
		public MissionMainAgentController()
		{
			this.InteractionComponent = new MissionMainAgentInteractionComponent(this);
			this.CustomLookDir = Vec3.Zero;
			this.IsChatOpen = false;
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x00022378 File Offset: 0x00020578
		public override void EarlyStart()
		{
			base.EarlyStart();
			Game.Current.EventManager.RegisterEvent<MissionPlayerToggledOrderViewEvent>(new Action<MissionPlayerToggledOrderViewEvent>(this.OnPlayerToggleOrder));
			base.Mission.OnMainAgentChanged += this.Mission_OnMainAgentChanged;
			MissionMultiplayerGameModeBaseClient missionBehavior = base.Mission.GetMissionBehavior<MissionMultiplayerGameModeBaseClient>();
			if (((missionBehavior != null) ? missionBehavior.RoundComponent : null) != null)
			{
				missionBehavior.RoundComponent.OnRoundStarted += this.Disable;
				missionBehavior.RoundComponent.OnPreparationEnded += this.Enable;
			}
			ManagedOptions.OnManagedOptionChanged = (ManagedOptions.OnManagedOptionChangedDelegate)Delegate.Combine(ManagedOptions.OnManagedOptionChanged, new ManagedOptions.OnManagedOptionChangedDelegate(this.OnManagedOptionChanged));
			this.UpdateLockTargetOption();
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x0002242C File Offset: 0x0002062C
		public override void OnMissionScreenFinalize()
		{
			base.OnMissionScreenFinalize();
			base.Mission.OnMainAgentChanged -= this.Mission_OnMainAgentChanged;
			Game.Current.EventManager.UnregisterEvent<MissionPlayerToggledOrderViewEvent>(new Action<MissionPlayerToggledOrderViewEvent>(this.OnPlayerToggleOrder));
			MissionMultiplayerGameModeBaseClient missionBehavior = base.Mission.GetMissionBehavior<MissionMultiplayerGameModeBaseClient>();
			if (((missionBehavior != null) ? missionBehavior.RoundComponent : null) != null)
			{
				missionBehavior.RoundComponent.OnRoundStarted -= this.Disable;
				missionBehavior.RoundComponent.OnPreparationEnded -= this.Enable;
			}
			ManagedOptions.OnManagedOptionChanged = (ManagedOptions.OnManagedOptionChangedDelegate)Delegate.Remove(ManagedOptions.OnManagedOptionChanged, new ManagedOptions.OnManagedOptionChangedDelegate(this.OnManagedOptionChanged));
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x000224DC File Offset: 0x000206DC
		public override bool IsReady()
		{
			bool flag = true;
			if (base.Mission.MainAgent != null)
			{
				flag = base.Mission.MainAgent.AgentVisuals.CheckResources(true);
			}
			return flag;
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x00022510 File Offset: 0x00020710
		private void Mission_OnMainAgentChanged(Agent oldAgent)
		{
			if (base.Mission.MainAgent != null)
			{
				this._isPlayerAgentAdded = true;
				this._strafeModeActive = false;
				this._autoDismountModeActive = false;
			}
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x00022534 File Offset: 0x00020734
		public override void OnPreMissionTick(float dt)
		{
			base.OnPreMissionTick(dt);
			if (base.MissionScreen == null)
			{
				return;
			}
			if (base.Mission.MainAgent == null && GameNetwork.MyPeer != null)
			{
				MissionPeer component = GameNetwork.MyPeer.GetComponent<MissionPeer>();
				if (component != null)
				{
					if (component.HasSpawnedAgentVisuals)
					{
						this.AgentVisualsMovementCheck();
					}
					else if (component.FollowedAgent != null)
					{
						this.RequestToSpawnAsBotCheck();
					}
				}
			}
			Agent mainAgent = base.Mission.MainAgent;
			if (mainAgent != null && mainAgent.State == AgentState.Active && !base.MissionScreen.IsCheatGhostMode && !base.Mission.MainAgent.IsAIControlled && !base.MissionScreen.IsPhotoModeEnabled && !this.IsDisabled && this._activated)
			{
				this.InteractionComponent.FocusTick();
				this.InteractionComponent.FocusedItemHealthTick();
				this.ControlTick();
				this.InteractionComponent.FocusStateCheckTick();
				this.LookTick(dt);
				return;
			}
			this.InteractionComponent.ClearFocus();
			this.LockedAgent = null;
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x0002262A File Offset: 0x0002082A
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow blow)
		{
			if (this.InteractionComponent.CurrentFocusedObject == affectedAgent || affectedAgent == base.Mission.MainAgent)
			{
				this.InteractionComponent.ClearFocus();
			}
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x00022653 File Offset: 0x00020853
		public override void OnAgentDeleted(Agent affectedAgent)
		{
			if (this.InteractionComponent.CurrentFocusedObject == affectedAgent)
			{
				this.InteractionComponent.ClearFocus();
			}
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x0002266E File Offset: 0x0002086E
		public override void OnClearScene()
		{
			this.InteractionComponent.OnClearScene();
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x0002267C File Offset: 0x0002087C
		private void LookTick(float dt)
		{
			if (!this.IsDisabled)
			{
				Agent mainAgent = base.Mission.MainAgent;
				if (mainAgent != null)
				{
					if (this._isPlayerAgentAdded)
					{
						this._isPlayerAgentAdded = false;
						mainAgent.LookDirectionAsAngle = mainAgent.MovementDirectionAsAngle;
					}
					if (base.Mission.ClearSceneTimerElapsedTime >= 0f)
					{
						Vec3 vec;
						if (this.LockedAgent != null)
						{
							float num = 0f;
							float agentScale = this.LockedAgent.AgentScale;
							float agentScale2 = mainAgent.AgentScale;
							if (!this.LockedAgent.GetAgentFlags().HasAnyFlag(AgentFlag.IsHumanoid))
							{
								num += this.LockedAgent.Monster.BodyCapsulePoint1.z * agentScale;
							}
							else if (this.LockedAgent.HasMount)
							{
								num += (this.LockedAgent.MountAgent.Monster.RiderCameraHeightAdder + this.LockedAgent.MountAgent.Monster.BodyCapsulePoint1.z + this.LockedAgent.MountAgent.Monster.BodyCapsuleRadius) * this.LockedAgent.MountAgent.AgentScale + this.LockedAgent.Monster.CrouchEyeHeight * agentScale;
							}
							else if (this.LockedAgent.CrouchMode || this.LockedAgent.IsSitting())
							{
								num += (this.LockedAgent.Monster.CrouchEyeHeight + 0.2f) * agentScale;
							}
							else
							{
								num += (this.LockedAgent.Monster.StandingEyeHeight + 0.2f) * agentScale;
							}
							if (!mainAgent.GetAgentFlags().HasAnyFlag(AgentFlag.IsHumanoid))
							{
								num -= this.LockedAgent.Monster.BodyCapsulePoint1.z * agentScale2;
							}
							else if (mainAgent.HasMount)
							{
								num -= (mainAgent.MountAgent.Monster.RiderCameraHeightAdder + mainAgent.MountAgent.Monster.BodyCapsulePoint1.z + mainAgent.MountAgent.Monster.BodyCapsuleRadius) * mainAgent.MountAgent.AgentScale + mainAgent.Monster.CrouchEyeHeight * agentScale2;
							}
							else if (mainAgent.CrouchMode || mainAgent.IsSitting())
							{
								num -= (mainAgent.Monster.CrouchEyeHeight + 0.2f) * agentScale2;
							}
							else
							{
								num -= (mainAgent.Monster.StandingEyeHeight + 0.2f) * agentScale2;
							}
							if (this.LockedAgent.GetAgentFlags().HasAnyFlag(AgentFlag.IsHumanoid))
							{
								num -= 0.3f * agentScale;
							}
							num = MBMath.Lerp(this._lastLockedAgentHeightDifference, num, MathF.Min(8f * dt, 1f), 1E-05f);
							this._lastLockedAgentHeightDifference = num;
							vec = (this.LockedAgent.VisualPosition + ((this.LockedAgent.MountAgent != null) ? (this.LockedAgent.MountAgent.GetMovementDirection().ToVec3(0f) * this.LockedAgent.MountAgent.Monster.RiderBodyCapsuleForwardAdder) : Vec3.Zero) + new Vec3(0f, 0f, num, -1f) - (mainAgent.VisualPosition + ((mainAgent.MountAgent != null) ? (mainAgent.MountAgent.GetMovementDirection().ToVec3(0f) * mainAgent.MountAgent.Monster.RiderBodyCapsuleForwardAdder) : Vec3.Zero))).NormalizedCopy();
						}
						else if (this.CustomLookDir.IsNonZero)
						{
							vec = this.CustomLookDir;
						}
						else
						{
							Mat3 identity = Mat3.Identity;
							identity.RotateAboutUp(base.MissionScreen.CameraBearing);
							identity.RotateAboutSide(base.MissionScreen.CameraElevation);
							vec = identity.f;
						}
						if (!base.MissionScreen.IsViewingCharacter() && !mainAgent.IsLookDirectionLocked && mainAgent.MovementLockedState != AgentMovementLockedState.FrameLocked)
						{
							mainAgent.LookDirection = vec;
						}
						mainAgent.HeadCameraMode = base.Mission.CameraIsFirstPerson;
					}
				}
			}
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x00022A6D File Offset: 0x00020C6D
		private void AgentVisualsMovementCheck()
		{
			if (base.Input.IsGameKeyReleased(13))
			{
				this.BreakAgentVisualsInvulnerability();
			}
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x00022A84 File Offset: 0x00020C84
		public void BreakAgentVisualsInvulnerability()
		{
			if (GameNetwork.IsClient)
			{
				GameNetwork.BeginModuleEventAsClient();
				GameNetwork.WriteMessage(new AgentVisualsBreakInvulnerability());
				GameNetwork.EndModuleEventAsClient();
				return;
			}
			Mission.Current.GetMissionBehavior<SpawnComponent>().SetEarlyAgentVisualsDespawning(GameNetwork.MyPeer.GetComponent<MissionPeer>(), true);
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x00022ABC File Offset: 0x00020CBC
		private void RequestToSpawnAsBotCheck()
		{
			if (base.Input.IsGameKeyPressed(13))
			{
				if (GameNetwork.IsClient)
				{
					GameNetwork.BeginModuleEventAsClient();
					GameNetwork.WriteMessage(new RequestToSpawnAsBot());
					GameNetwork.EndModuleEventAsClient();
					return;
				}
				if (GameNetwork.MyPeer.GetComponent<MissionPeer>().HasSpawnTimerExpired)
				{
					GameNetwork.MyPeer.GetComponent<MissionPeer>().WantsToSpawnAsBot = true;
				}
			}
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x00022B18 File Offset: 0x00020D18
		private Agent FindTargetedLockableAgent(Agent player)
		{
			Vec3 direction = base.MissionScreen.CombatCamera.Direction;
			Vec3 vec = direction;
			Vec3 position = base.MissionScreen.CombatCamera.Position;
			Vec3 visualPosition = player.VisualPosition;
			float num = new Vec3(position.x, position.y, 0f, -1f).Distance(new Vec3(visualPosition.x, visualPosition.y, 0f, -1f));
			Vec3 vec2 = position * (1f - num) + (position + direction) * num;
			float num2 = 0f;
			Agent agent = null;
			foreach (Agent agent2 in base.Mission.Agents)
			{
				if ((agent2.IsMount && agent2.RiderAgent != null && agent2.RiderAgent.IsEnemyOf(player)) || (!agent2.IsMount && agent2.IsEnemyOf(player)))
				{
					Vec3 vec3 = agent2.GetChestGlobalPosition() - vec2;
					float num3 = vec3.Normalize();
					if (num3 < 20f)
					{
						float num4 = Vec2.DotProduct(vec.AsVec2.Normalized(), vec3.AsVec2.Normalized());
						float num5 = Vec2.DotProduct(new Vec2(vec.AsVec2.Length, vec.z), new Vec2(vec3.AsVec2.Length, vec3.z));
						if (num4 > 0.95f && num5 > 0.95f)
						{
							float num6 = num4 * num4 * num4 / MathF.Pow(num3, 0.15f);
							if (num6 > num2)
							{
								num2 = num6;
								agent = agent2;
							}
						}
					}
				}
			}
			if (agent != null && agent.IsMount && agent.RiderAgent != null)
			{
				return agent.RiderAgent;
			}
			return agent;
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x00022D34 File Offset: 0x00020F34
		private void ControlTick()
		{
			if (base.MissionScreen != null && base.MissionScreen.IsPhotoModeEnabled)
			{
				return;
			}
			if (this.IsChatOpen)
			{
				return;
			}
			Agent mainAgent = base.Mission.MainAgent;
			bool flag = false;
			if (this.LockedAgent != null && (!base.Mission.Agents.ContainsQ(this.LockedAgent) || !this.LockedAgent.IsActive() || this.LockedAgent.Position.DistanceSquared(mainAgent.Position) > 625f || base.Input.IsGameKeyReleased(26) || base.Input.IsGameKeyDown(25) || (base.Mission.Mode != MissionMode.Battle && base.Mission.Mode != MissionMode.Stealth) || (!mainAgent.WieldedWeapon.IsEmpty && mainAgent.WieldedWeapon.CurrentUsageItem.IsRangedWeapon) || base.MissionScreen == null || base.MissionScreen.GetSpectatingData(base.MissionScreen.CombatCamera.Frame.origin).CameraType != SpectatorCameraTypes.LockToMainPlayer || this.IsThereAnyCustomCameraAddition()))
			{
				this.LockedAgent = null;
				flag = true;
			}
			if (base.Mission.Mode == MissionMode.Conversation)
			{
				mainAgent.MovementFlags = Agent.MovementControlFlag.None;
				mainAgent.MovementInputVector = Vec2.Zero;
			}
			else if (base.Mission.ClearSceneTimerElapsedTime >= 0f && mainAgent.State == AgentState.Active)
			{
				bool flag2 = false;
				bool flag3 = false;
				bool flag4 = false;
				bool flag5 = false;
				Vec2 vec = new Vec2(base.Input.GetGameKeyAxis("MovementAxisX"), base.Input.GetGameKeyAxis("MovementAxisY"));
				if (this._autoDismountModeActive)
				{
					if (!base.Input.IsGameKeyDown(0) && mainAgent.MountAgent != null)
					{
						if (mainAgent.GetCurrentVelocity().y > 0f)
						{
							vec.y = -1f;
						}
					}
					else
					{
						this._autoDismountModeActive = false;
					}
				}
				if (MathF.Abs(vec.x) < 0.2f)
				{
					vec.x = 0f;
				}
				if (MathF.Abs(vec.y) < 0.2f)
				{
					vec.y = 0f;
				}
				if (vec.IsNonZero())
				{
					float rotationInRadians = vec.RotationInRadians;
					if (rotationInRadians > -0.7853982f && rotationInRadians < 0.7853982f)
					{
						flag3 = true;
					}
					else if (rotationInRadians < -2.3561945f || rotationInRadians > 2.3561945f)
					{
						flag5 = true;
					}
					else if (rotationInRadians < 0f)
					{
						flag2 = true;
					}
					else
					{
						flag4 = true;
					}
				}
				mainAgent.EventControlFlags = Agent.EventControlFlag.None;
				mainAgent.MovementFlags = Agent.MovementControlFlag.None;
				mainAgent.MovementInputVector = Vec2.Zero;
				using (List<MissionBehavior>.Enumerator enumerator = base.Mission.MissionBehaviors.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						IPlayerInputEffector playerInputEffector;
						if ((playerInputEffector = enumerator.Current as IPlayerInputEffector) != null)
						{
							mainAgent.EventControlFlags |= playerInputEffector.OnCollectPlayerEventControlFlags();
						}
					}
				}
				if (!base.MissionScreen.IsRadialMenuActive && !base.Mission.IsOrderMenuOpen)
				{
					if (base.Input.IsGameKeyPressed(14))
					{
						if (mainAgent.MountAgent == null || mainAgent.MovementVelocity.LengthSquared > 0.09f)
						{
							mainAgent.EventControlFlags |= Agent.EventControlFlag.Jump;
						}
						else
						{
							mainAgent.EventControlFlags |= Agent.EventControlFlag.Rear;
						}
					}
					if (base.Input.IsGameKeyPressed(13))
					{
						mainAgent.MovementFlags |= Agent.MovementControlFlag.Action;
					}
				}
				if (mainAgent.MountAgent != null && mainAgent.GetCurrentVelocity().y < 0.5f && (base.Input.IsGameKeyDown(3) || base.Input.IsGameKeyDown(2)))
				{
					if (base.Input.IsGameKeyPressed(16))
					{
						this._strafeModeActive = true;
					}
				}
				else
				{
					this._strafeModeActive = false;
				}
				Agent.MovementControlFlag movementControlFlag = this._lastMovementKeyPressed;
				if (base.Input.IsGameKeyPressed(0))
				{
					movementControlFlag = Agent.MovementControlFlag.Forward;
				}
				else if (base.Input.IsGameKeyPressed(1))
				{
					movementControlFlag = Agent.MovementControlFlag.Backward;
				}
				else if (base.Input.IsGameKeyPressed(2))
				{
					movementControlFlag = Agent.MovementControlFlag.StrafeLeft;
				}
				else if (base.Input.IsGameKeyPressed(3))
				{
					movementControlFlag = Agent.MovementControlFlag.StrafeRight;
				}
				if (movementControlFlag != this._lastMovementKeyPressed)
				{
					this._lastMovementKeyPressed = movementControlFlag;
					Game game = Game.Current;
					if (game != null)
					{
						game.EventManager.TriggerEvent<MissionPlayerMovementFlagsChangeEvent>(new MissionPlayerMovementFlagsChangeEvent(this._lastMovementKeyPressed));
					}
				}
				if (!base.Input.GetIsMouseActive())
				{
					bool flag6 = true;
					if (flag3)
					{
						movementControlFlag = Agent.MovementControlFlag.Forward;
					}
					else if (flag5)
					{
						movementControlFlag = Agent.MovementControlFlag.Backward;
					}
					else if (flag4)
					{
						movementControlFlag = Agent.MovementControlFlag.StrafeLeft;
					}
					else if (flag2)
					{
						movementControlFlag = Agent.MovementControlFlag.StrafeRight;
					}
					else
					{
						flag6 = false;
					}
					if (flag6)
					{
						base.Mission.SetLastMovementKeyPressed(movementControlFlag);
					}
				}
				else
				{
					base.Mission.SetLastMovementKeyPressed(this._lastMovementKeyPressed);
				}
				if (base.Input.IsGameKeyPressed(0))
				{
					if (this._lastForwardKeyPressTime + 0.3f > Time.ApplicationTime)
					{
						mainAgent.EventControlFlags &= ~(Agent.EventControlFlag.DoubleTapToDirectionUp | Agent.EventControlFlag.DoubleTapToDirectionDown | Agent.EventControlFlag.DoubleTapToDirectionRight);
						mainAgent.EventControlFlags |= Agent.EventControlFlag.DoubleTapToDirectionUp;
					}
					this._lastForwardKeyPressTime = Time.ApplicationTime;
				}
				if (base.Input.IsGameKeyPressed(1))
				{
					if (this._lastBackwardKeyPressTime + 0.3f > Time.ApplicationTime)
					{
						mainAgent.EventControlFlags &= ~(Agent.EventControlFlag.DoubleTapToDirectionUp | Agent.EventControlFlag.DoubleTapToDirectionDown | Agent.EventControlFlag.DoubleTapToDirectionRight);
						mainAgent.EventControlFlags |= Agent.EventControlFlag.DoubleTapToDirectionDown;
					}
					this._lastBackwardKeyPressTime = Time.ApplicationTime;
				}
				if (base.Input.IsGameKeyPressed(2))
				{
					if (this._lastLeftKeyPressTime + 0.3f > Time.ApplicationTime)
					{
						mainAgent.EventControlFlags &= ~(Agent.EventControlFlag.DoubleTapToDirectionUp | Agent.EventControlFlag.DoubleTapToDirectionDown | Agent.EventControlFlag.DoubleTapToDirectionRight);
						mainAgent.EventControlFlags |= Agent.EventControlFlag.DoubleTapToDirectionLeft;
					}
					this._lastLeftKeyPressTime = Time.ApplicationTime;
				}
				if (base.Input.IsGameKeyPressed(3))
				{
					if (this._lastRightKeyPressTime + 0.3f > Time.ApplicationTime)
					{
						mainAgent.EventControlFlags &= ~(Agent.EventControlFlag.DoubleTapToDirectionUp | Agent.EventControlFlag.DoubleTapToDirectionDown | Agent.EventControlFlag.DoubleTapToDirectionRight);
						mainAgent.EventControlFlags |= Agent.EventControlFlag.DoubleTapToDirectionRight;
					}
					this._lastRightKeyPressTime = Time.ApplicationTime;
				}
				if (this._isTargetLockEnabled && !this.IsThereAnyCustomCameraAddition())
				{
					if (base.Input.IsGameKeyDown(26) && this.LockedAgent == null && !base.Input.IsGameKeyDown(25) && (base.Mission.Mode == MissionMode.Battle || base.Mission.Mode == MissionMode.Stealth) && (mainAgent.WieldedWeapon.IsEmpty || !mainAgent.WieldedWeapon.CurrentUsageItem.IsRangedWeapon) && !GameNetwork.IsMultiplayer)
					{
						float applicationTime = Time.ApplicationTime;
						if (this._lastLockKeyPressTime <= 0f)
						{
							this._lastLockKeyPressTime = applicationTime;
						}
						if (applicationTime > this._lastLockKeyPressTime + 0.3f)
						{
							this.PotentialLockTargetAgent = this.FindTargetedLockableAgent(mainAgent);
						}
					}
					else
					{
						this.PotentialLockTargetAgent = null;
					}
					if (this.LockedAgent == null && !flag && base.Input.IsGameKeyReleased(26) && !GameNetwork.IsMultiplayer)
					{
						this._lastLockKeyPressTime = 0f;
						if (!base.Input.IsGameKeyDown(25) && (base.Mission.Mode == MissionMode.Battle || base.Mission.Mode == MissionMode.Stealth) && (mainAgent.WieldedWeapon.IsEmpty || !mainAgent.WieldedWeapon.CurrentUsageItem.IsRangedWeapon) && base.MissionScreen != null && base.MissionScreen.GetSpectatingData(base.MissionScreen.CombatCamera.Frame.origin).CameraType == SpectatorCameraTypes.LockToMainPlayer)
						{
							this.LockedAgent = this.FindTargetedLockableAgent(mainAgent);
						}
					}
				}
				if (mainAgent.MountAgent != null && !this._strafeModeActive)
				{
					if (flag2 || vec.x > 0f)
					{
						mainAgent.MovementFlags |= Agent.MovementControlFlag.TurnRight;
					}
					else if (flag4 || vec.x < 0f)
					{
						mainAgent.MovementFlags |= Agent.MovementControlFlag.TurnLeft;
					}
				}
				mainAgent.MovementInputVector = vec;
				if (!base.MissionScreen.MouseVisible && !base.MissionScreen.IsRadialMenuActive && !this._isPlayerOrderOpen && mainAgent.CombatActionsEnabled)
				{
					object obj;
					if (NativeOptions.GetConfig(NativeOptions.NativeOptionsType.EnableAlternateAiming) != 0f && TaleWorlds.InputSystem.Input.IsGamepadActive)
					{
						WeaponComponentData currentUsageItem = mainAgent.WieldedWeapon.CurrentUsageItem;
						if (currentUsageItem == null || !currentUsageItem.WeaponFlags.HasAllFlags(WeaponFlags.StringHeldByHand))
						{
							WeaponComponentData currentUsageItem2 = mainAgent.WieldedWeapon.CurrentUsageItem;
							obj = currentUsageItem2 != null && currentUsageItem2.IsRangedWeapon && !mainAgent.WieldedWeapon.CurrentUsageItem.IsConsumable && !mainAgent.WieldedWeapon.CurrentUsageItem.WeaponFlags.HasAllFlags(WeaponFlags.StringHeldByHand);
						}
						else
						{
							obj = 1;
						}
					}
					else
					{
						obj = 0;
					}
					object obj2 = obj;
					if (obj2 != null)
					{
						if (mainAgent.GetAgentFlags().HasAnyFlag(AgentFlag.CanAttack))
						{
							this.HandleRangedWeaponAttackAlternativeAiming(mainAgent);
						}
					}
					else if (base.Input.IsGameKeyDown(9) && mainAgent.GetAgentFlags().HasAnyFlag(AgentFlag.CanAttack))
					{
						mainAgent.MovementFlags |= mainAgent.AttackDirectionToMovementFlag(mainAgent.GetAttackDirection());
					}
					if (obj2 == null && base.Input.IsGameKeyDown(10))
					{
						if (ManagedOptions.GetConfig(ManagedOptions.ManagedOptionsType.ControlBlockDirection) == 2f && MissionGameModels.Current.AutoBlockModel != null)
						{
							Agent.UsageDirection blockDirection = MissionGameModels.Current.AutoBlockModel.GetBlockDirection(base.Mission);
							if (blockDirection == Agent.UsageDirection.AttackLeft)
							{
								mainAgent.MovementFlags |= Agent.MovementControlFlag.DefendRight;
							}
							else if (blockDirection == Agent.UsageDirection.AttackRight)
							{
								mainAgent.MovementFlags |= Agent.MovementControlFlag.DefendLeft;
							}
							else if (blockDirection == Agent.UsageDirection.AttackUp)
							{
								mainAgent.MovementFlags |= Agent.MovementControlFlag.DefendUp;
							}
							else if (blockDirection == Agent.UsageDirection.AttackDown)
							{
								mainAgent.MovementFlags |= Agent.MovementControlFlag.DefendDown;
							}
						}
						else
						{
							mainAgent.MovementFlags |= mainAgent.GetDefendMovementFlag();
						}
					}
				}
				if (!base.MissionScreen.IsRadialMenuActive && !base.Mission.IsOrderMenuOpen)
				{
					if (base.Input.IsGameKeyPressed(16) && (mainAgent.KickClear() || mainAgent.MountAgent != null))
					{
						mainAgent.EventControlFlags |= Agent.EventControlFlag.Kick;
					}
					if (base.Input.IsGameKeyPressed(18))
					{
						mainAgent.TryToWieldWeaponInSlot(EquipmentIndex.WeaponItemBeginSlot, Agent.WeaponWieldActionType.WithAnimation, false);
					}
					else if (base.Input.IsGameKeyPressed(19))
					{
						mainAgent.TryToWieldWeaponInSlot(EquipmentIndex.Weapon1, Agent.WeaponWieldActionType.WithAnimation, false);
					}
					else if (base.Input.IsGameKeyPressed(20))
					{
						mainAgent.TryToWieldWeaponInSlot(EquipmentIndex.Weapon2, Agent.WeaponWieldActionType.WithAnimation, false);
					}
					else if (base.Input.IsGameKeyPressed(21))
					{
						mainAgent.TryToWieldWeaponInSlot(EquipmentIndex.Weapon3, Agent.WeaponWieldActionType.WithAnimation, false);
					}
					else if (base.Input.IsGameKeyPressed(11) && this._lastWieldNextPrimaryWeaponTriggerTime + 0.2f < Time.ApplicationTime)
					{
						this._lastWieldNextPrimaryWeaponTriggerTime = Time.ApplicationTime;
						mainAgent.WieldNextWeapon(Agent.HandIndex.MainHand, Agent.WeaponWieldActionType.WithAnimation);
					}
					else if (base.Input.IsGameKeyPressed(12) && this._lastWieldNextOffhandWeaponTriggerTime + 0.2f < Time.ApplicationTime)
					{
						this._lastWieldNextOffhandWeaponTriggerTime = Time.ApplicationTime;
						mainAgent.WieldNextWeapon(Agent.HandIndex.OffHand, Agent.WeaponWieldActionType.WithAnimation);
					}
					else if (base.Input.IsGameKeyPressed(23))
					{
						mainAgent.TryToSheathWeaponInHand(Agent.HandIndex.MainHand, Agent.WeaponWieldActionType.WithAnimation);
					}
					if (base.Input.IsGameKeyPressed(17) || this._weaponUsageToggleRequested)
					{
						mainAgent.EventControlFlags |= Agent.EventControlFlag.ToggleAlternativeWeapon;
						this._weaponUsageToggleRequested = false;
					}
					if (this._overrideControlsThisFrame.HasAnyFlag(mainAgent.WalkMode ? MissionMainAgentController.OverrideMainAgentControlFlag.Run : MissionMainAgentController.OverrideMainAgentControlFlag.Walk) || base.Input.IsGameKeyPressed(30))
					{
						mainAgent.EventControlFlags |= (mainAgent.WalkMode ? Agent.EventControlFlag.Run : Agent.EventControlFlag.Walk);
					}
					if (mainAgent.IsInWater())
					{
						if (base.Input.IsGameKeyDown(14))
						{
							mainAgent.EventControlFlags |= Agent.EventControlFlag.Jump;
						}
						if (base.Input.IsGameKeyDown(15))
						{
							mainAgent.EventControlFlags |= Agent.EventControlFlag.Crouch;
						}
					}
					if (mainAgent.MountAgent != null)
					{
						if (base.Input.IsGameKeyPressed(15) || this._autoDismountModeActive)
						{
							if (mainAgent.GetCurrentVelocity().y < 0.5f && mainAgent.MountAgent.GetCurrentActionType(0) != Agent.ActionCodeType.Rear)
							{
								mainAgent.EventControlFlags |= Agent.EventControlFlag.Dismount;
							}
							else if (base.Input.IsGameKeyPressed(15))
							{
								this._autoDismountModeActive = true;
								mainAgent.EventControlFlags &= ~(Agent.EventControlFlag.DoubleTapToDirectionUp | Agent.EventControlFlag.DoubleTapToDirectionDown | Agent.EventControlFlag.DoubleTapToDirectionRight);
								mainAgent.EventControlFlags |= Agent.EventControlFlag.DoubleTapToDirectionDown;
							}
						}
					}
					else if (this._overrideControlsThisFrame.HasAnyFlag(mainAgent.GetScriptedFlags().HasAnyFlag(Agent.AIScriptedFrameFlags.Crouch) ? MissionMainAgentController.OverrideMainAgentControlFlag.Stand : MissionMainAgentController.OverrideMainAgentControlFlag.Crouch) || (!TaleWorlds.InputSystem.Input.IsGamepadActive && base.Input.IsGameKeyPressed(15)) || (mainAgent.EventControlFlags.HasAnyFlag(Agent.EventControlFlag.Crouch) && !mainAgent.GetScriptedFlags().HasAnyFlag(Agent.AIScriptedFrameFlags.Crouch)) || (mainAgent.EventControlFlags.HasAnyFlag(Agent.EventControlFlag.Stand) && mainAgent.GetScriptedFlags().HasAnyFlag(Agent.AIScriptedFrameFlags.Crouch)))
					{
						if (mainAgent.GetScriptedFlags().HasAnyFlag(Agent.AIScriptedFrameFlags.Crouch))
						{
							mainAgent.SetScriptedFlags(mainAgent.GetScriptedFlags() & ~Agent.AIScriptedFrameFlags.Crouch);
						}
						else if (mainAgent.IsCrouchingAllowed())
						{
							mainAgent.SetScriptedFlags(mainAgent.GetScriptedFlags() | Agent.AIScriptedFrameFlags.Crouch);
						}
					}
					if (mainAgent.GetScriptedFlags().HasAnyFlag(Agent.AIScriptedFrameFlags.Crouch) && (mainAgent.EventControlFlags.HasAnyFlag(Agent.EventControlFlag.Dismount | Agent.EventControlFlag.Mount | Agent.EventControlFlag.Jump | Agent.EventControlFlag.Stand | Agent.EventControlFlag.Kick) || mainAgent.HasMount || mainAgent.IsInWater()))
					{
						mainAgent.SetScriptedFlags(mainAgent.GetScriptedFlags() & ~Agent.AIScriptedFrameFlags.Crouch);
					}
					if (mainAgent.CrouchMode != mainAgent.GetScriptedFlags().HasAnyFlag(Agent.AIScriptedFrameFlags.Crouch))
					{
						mainAgent.EventControlFlags |= (mainAgent.GetScriptedFlags().HasAnyFlag(Agent.AIScriptedFrameFlags.Crouch) ? Agent.EventControlFlag.Crouch : Agent.EventControlFlag.Stand);
					}
				}
			}
			this._overrideControlsThisFrame = MissionMainAgentController.OverrideMainAgentControlFlag.None;
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x00023B08 File Offset: 0x00021D08
		private void HandleRangedWeaponAttackAlternativeAiming(Agent player)
		{
			if (base.Input.GetKeyState(InputKey.ControllerLTrigger).x > 0.2f)
			{
				if (base.Input.GetKeyState(InputKey.ControllerRTrigger).x < 0.6f)
				{
					player.MovementFlags |= player.AttackDirectionToMovementFlag(player.GetAttackDirection());
				}
				this._isPlayerAiming = true;
				return;
			}
			if (this._isPlayerAiming)
			{
				player.MovementFlags |= Agent.MovementControlFlag.DefendUp;
				this._isPlayerAiming = false;
			}
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x00023B8F File Offset: 0x00021D8F
		public override bool IsThereAgentAction(Agent userAgent, Agent otherAgent)
		{
			return otherAgent.IsMount && otherAgent.IsActive();
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x00023BA1 File Offset: 0x00021DA1
		public void Disable()
		{
			this._activated = false;
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x00023BAA File Offset: 0x00021DAA
		public void Enable()
		{
			this._activated = true;
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x00023BB3 File Offset: 0x00021DB3
		private void OnPlayerToggleOrder(MissionPlayerToggledOrderViewEvent obj)
		{
			this._isPlayerOrderOpen = obj.IsOrderEnabled;
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x00023BC1 File Offset: 0x00021DC1
		public void OnWeaponUsageToggleRequested()
		{
			this._weaponUsageToggleRequested = true;
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x00023BCA File Offset: 0x00021DCA
		public void AddOverrideControlsForFrame(MissionMainAgentController.OverrideMainAgentControlFlag overrideFlag)
		{
			this._overrideControlsThisFrame |= overrideFlag;
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x00023BDA File Offset: 0x00021DDA
		private void OnManagedOptionChanged(ManagedOptions.ManagedOptionsType optionType)
		{
			if (optionType == ManagedOptions.ManagedOptionsType.LockTarget)
			{
				this.UpdateLockTargetOption();
			}
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x00023BE7 File Offset: 0x00021DE7
		private void UpdateLockTargetOption()
		{
			this._isTargetLockEnabled = ManagedOptions.GetConfig(ManagedOptions.ManagedOptionsType.LockTarget) == 1f;
			this.LockedAgent = null;
			this.PotentialLockTargetAgent = null;
			this._lastLockKeyPressTime = 0f;
			this._lastLockedAgentHeightDifference = 0f;
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x00023C24 File Offset: 0x00021E24
		private bool IsThereAnyCustomCameraAddition()
		{
			return base.Mission.CustomCameraTargetLocalOffset.IsNonZero || base.Mission.CustomCameraLocalOffset.IsNonZero || base.Mission.CustomCameraLocalOffset2.IsNonZero || base.Mission.CustomCameraGlobalOffset.IsNonZero || base.Mission.CustomCameraLocalRotationalOffset.IsNonZero || base.Mission.CustomCameraFixedDistance != float.MinValue;
		}

		// Token: 0x0400029C RID: 668
		private const float MinValueForAimStart = 0.2f;

		// Token: 0x0400029D RID: 669
		private const float MaxValueForAttackEnd = 0.6f;

		// Token: 0x040002A2 RID: 674
		private float _lastForwardKeyPressTime;

		// Token: 0x040002A3 RID: 675
		private float _lastBackwardKeyPressTime;

		// Token: 0x040002A4 RID: 676
		private float _lastLeftKeyPressTime;

		// Token: 0x040002A5 RID: 677
		private float _lastRightKeyPressTime;

		// Token: 0x040002A6 RID: 678
		private float _lastWieldNextPrimaryWeaponTriggerTime;

		// Token: 0x040002A7 RID: 679
		private float _lastWieldNextOffhandWeaponTriggerTime;

		// Token: 0x040002A8 RID: 680
		private bool _activated = true;

		// Token: 0x040002A9 RID: 681
		private bool _strafeModeActive;

		// Token: 0x040002AA RID: 682
		private bool _autoDismountModeActive;

		// Token: 0x040002AB RID: 683
		private bool _isPlayerAgentAdded;

		// Token: 0x040002AC RID: 684
		private bool _isPlayerAiming;

		// Token: 0x040002AD RID: 685
		private bool _isPlayerOrderOpen;

		// Token: 0x040002AE RID: 686
		private bool _isTargetLockEnabled;

		// Token: 0x040002AF RID: 687
		private Agent.MovementControlFlag _lastMovementKeyPressed = Agent.MovementControlFlag.Forward;

		// Token: 0x040002B0 RID: 688
		private Agent _lockedAgent;

		// Token: 0x040002B1 RID: 689
		private Agent _potentialLockTargetAgent;

		// Token: 0x040002B2 RID: 690
		private MissionMainAgentController.OverrideMainAgentControlFlag _overrideControlsThisFrame;

		// Token: 0x040002B3 RID: 691
		private float _lastLockKeyPressTime;

		// Token: 0x040002B4 RID: 692
		private float _lastLockedAgentHeightDifference;

		// Token: 0x040002B5 RID: 693
		public readonly MissionMainAgentInteractionComponent InteractionComponent;

		// Token: 0x040002B6 RID: 694
		public bool IsChatOpen;

		// Token: 0x040002B7 RID: 695
		private bool _weaponUsageToggleRequested;

		// Token: 0x020000DF RID: 223
		public enum OverrideMainAgentControlFlag
		{
			// Token: 0x040003E5 RID: 997
			None,
			// Token: 0x040003E6 RID: 998
			Walk,
			// Token: 0x040003E7 RID: 999
			Run,
			// Token: 0x040003E8 RID: 1000
			Crouch = 4,
			// Token: 0x040003E9 RID: 1001
			Stand = 8
		}

		// Token: 0x020000E0 RID: 224
		// (Invoke) Token: 0x0600064A RID: 1610
		public delegate void OnLockedAgentChangedDelegate(Agent newAgent);

		// Token: 0x020000E1 RID: 225
		// (Invoke) Token: 0x0600064E RID: 1614
		public delegate void OnPotentialLockedAgentChangedDelegate(Agent newPotentialAgent);
	}
}
