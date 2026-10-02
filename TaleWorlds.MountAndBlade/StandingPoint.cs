using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000357 RID: 855
	public class StandingPoint : UsableMissionObject
	{
		// Token: 0x17000929 RID: 2345
		// (get) Token: 0x0600310E RID: 12558 RVA: 0x000C76A0 File Offset: 0x000C58A0
		public virtual Agent.AIScriptedFrameFlags DisableScriptedFrameFlags
		{
			get
			{
				return Agent.AIScriptedFrameFlags.None;
			}
		}

		// Token: 0x1700092A RID: 2346
		// (get) Token: 0x0600310F RID: 12559 RVA: 0x000C76A3 File Offset: 0x000C58A3
		public override bool DisableCombatActionsOnUse
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700092B RID: 2347
		// (get) Token: 0x06003110 RID: 12560 RVA: 0x000C76A6 File Offset: 0x000C58A6
		// (set) Token: 0x06003111 RID: 12561 RVA: 0x000C76AE File Offset: 0x000C58AE
		[EditableScriptComponentVariable(false, "")]
		public Agent FavoredUser { get; set; }

		// Token: 0x1700092C RID: 2348
		// (get) Token: 0x06003112 RID: 12562 RVA: 0x000C76B7 File Offset: 0x000C58B7
		public virtual bool PlayerStopsUsingWhenInteractsWithOther
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700092D RID: 2349
		// (get) Token: 0x06003113 RID: 12563 RVA: 0x000C76BA File Offset: 0x000C58BA
		public bool UseOwnPositionInsteadOfWorldPosition
		{
			get
			{
				return this._useOwnPositionInsteadOfWorldPosition;
			}
		}

		// Token: 0x1700092E RID: 2350
		// (get) Token: 0x06003114 RID: 12564 RVA: 0x000C76C2 File Offset: 0x000C58C2
		public float CustomPlayerInteractionDistance
		{
			get
			{
				return this._customPlayerInteractionDistance;
			}
		}

		// Token: 0x06003115 RID: 12565 RVA: 0x000C76CC File Offset: 0x000C58CC
		protected internal override void OnInit()
		{
			base.OnInit();
			this._cachedAgentDistances = new Dictionary<Agent, StandingPoint.AgentDistanceCache>();
			bool flag = base.GameEntity.HasTag("attacker");
			bool flag2 = base.GameEntity.HasTag("defender");
			if (flag && !flag2)
			{
				this.StandingPointSide = BattleSideEnum.Attacker;
			}
			else if (!flag && flag2)
			{
				this.StandingPointSide = BattleSideEnum.Defender;
			}
			base.SetScriptComponentToTick(this.GetTickRequirement());
		}

		// Token: 0x06003116 RID: 12566 RVA: 0x000C773C File Offset: 0x000C593C
		public void OnParentMachinePhysicsStateChanged()
		{
			base.GameEntityWithWorldPosition.InvalidateWorldPosition();
		}

		// Token: 0x06003117 RID: 12567 RVA: 0x000C7749 File Offset: 0x000C5949
		public override bool IsDisabledForAgent(Agent agent)
		{
			return base.IsDisabledForAgent(agent) || (this.StandingPointSide != BattleSideEnum.None && agent.IsAIControlled && agent.Team != null && agent.Team.Side != this.StandingPointSide);
		}

		// Token: 0x06003118 RID: 12568 RVA: 0x000C7787 File Offset: 0x000C5987
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			if (!GameNetwork.IsClientOrReplay && base.HasUser)
			{
				return base.GetTickRequirement() | ScriptComponentBehavior.TickRequirement.Tick | ScriptComponentBehavior.TickRequirement.TickParallel3;
			}
			return base.GetTickRequirement();
		}

		// Token: 0x06003119 RID: 12569 RVA: 0x000C77AC File Offset: 0x000C59AC
		private void TickAux(bool isParallel)
		{
			if (!GameNetwork.IsClientOrReplay && base.HasUser)
			{
				if (!base.UserAgent.IsActive() || this.DoesActionTypeStopUsingGameObject(MBAnimation.GetActionType(base.UserAgent.GetCurrentAction(0))))
				{
					if (isParallel)
					{
						this._needsSingleThreadTickOnce = true;
						return;
					}
					Agent userAgent = base.UserAgent;
					Agent.StopUsingGameObjectFlags stopUsingGameObjectFlags = Agent.StopUsingGameObjectFlags.None;
					if (this._autoAttachOnUsingStopped)
					{
						stopUsingGameObjectFlags |= Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject;
					}
					userAgent.StopUsingGameObject(false, stopUsingGameObjectFlags);
					Action<Agent, bool> onUsingStoppedAction = this._onUsingStoppedAction;
					if (onUsingStoppedAction == null)
					{
						return;
					}
					onUsingStoppedAction(userAgent, true);
					return;
				}
				else if (this.AutoSheathWeapons)
				{
					if (base.UserAgent.GetPrimaryWieldedItemIndex() != EquipmentIndex.None)
					{
						if (isParallel)
						{
							this._needsSingleThreadTickOnce = true;
						}
						else
						{
							base.UserAgent.TryToSheathWeaponInHand(Agent.HandIndex.MainHand, Agent.WeaponWieldActionType.Instant);
						}
					}
					if (base.UserAgent.GetOffhandWieldedItemIndex() != EquipmentIndex.None)
					{
						if (isParallel)
						{
							this._needsSingleThreadTickOnce = true;
							return;
						}
						base.UserAgent.TryToSheathWeaponInHand(Agent.HandIndex.OffHand, Agent.WeaponWieldActionType.Instant);
						return;
					}
				}
				else if (this.AutoWieldWeapons && base.UserAgent.Equipment.HasAnyWeapon() && base.UserAgent.GetPrimaryWieldedItemIndex() == EquipmentIndex.None && base.UserAgent.GetOffhandWieldedItemIndex() == EquipmentIndex.None)
				{
					if (isParallel)
					{
						this._needsSingleThreadTickOnce = true;
						return;
					}
					base.UserAgent.WieldInitialWeapons(Agent.WeaponWieldActionType.Instant, Equipment.InitialWeaponEquipPreference.Any);
				}
			}
		}

		// Token: 0x0600311A RID: 12570 RVA: 0x000C78D3 File Offset: 0x000C5AD3
		protected internal override void OnTickParallel3(float dt)
		{
			this.TickAux(true);
		}

		// Token: 0x0600311B RID: 12571 RVA: 0x000C78DC File Offset: 0x000C5ADC
		protected internal override void OnTick(float dt)
		{
			base.OnTick(dt);
			if (this._needsSingleThreadTickOnce)
			{
				this._needsSingleThreadTickOnce = false;
				this.TickAux(false);
			}
		}

		// Token: 0x0600311C RID: 12572 RVA: 0x000C78FB File Offset: 0x000C5AFB
		protected virtual bool DoesActionTypeStopUsingGameObject(Agent.ActionCodeType actionType)
		{
			return actionType == Agent.ActionCodeType.Jump || actionType == Agent.ActionCodeType.Kick || actionType == Agent.ActionCodeType.WeaponBash;
		}

		// Token: 0x0600311D RID: 12573 RVA: 0x000C7910 File Offset: 0x000C5B10
		public override void OnUse(Agent userAgent, sbyte agentBoneIndex)
		{
			if (!this._autoAttachOnUsingStopped && this.MovingAgent != null)
			{
				Agent movingAgent = this.MovingAgent;
				movingAgent.StopUsingGameObject(true, Agent.StopUsingGameObjectFlags.None);
				Action<Agent, bool> onUsingStoppedAction = this._onUsingStoppedAction;
				if (onUsingStoppedAction != null)
				{
					onUsingStoppedAction(movingAgent, false);
				}
			}
			base.OnUse(userAgent, agentBoneIndex);
			if (this.LockUserFrames)
			{
				WorldFrame userFrameForAgent = this.GetUserFrameForAgent(userAgent);
				Vec2 asVec = userFrameForAgent.Origin.AsVec2;
				userAgent.SetTargetPositionAndDirection(in asVec, in userFrameForAgent.Rotation.f);
				return;
			}
			if (this.LockUserPositions)
			{
				userAgent.SetTargetPosition(this.GetUserFrameForAgent(userAgent).Origin.AsVec2);
			}
		}

		// Token: 0x0600311E RID: 12574 RVA: 0x000C79AB File Offset: 0x000C5BAB
		public override void OnUseStopped(Agent userAgent, bool isSuccessful, int preferenceIndex)
		{
			base.OnUseStopped(userAgent, isSuccessful, preferenceIndex);
			if (this.LockUserFrames || this.LockUserPositions)
			{
				userAgent.ClearTargetFrame();
			}
		}

		// Token: 0x0600311F RID: 12575 RVA: 0x000C79CC File Offset: 0x000C5BCC
		public override WorldFrame GetUserFrameForAgent(Agent agent)
		{
			if (!Mission.Current.IsTeleportingAgents && !this.TranslateUser)
			{
				return agent.GetWorldFrame();
			}
			if (!Mission.Current.IsTeleportingAgents && (this.LockUserFrames || this.LockUserPositions))
			{
				return base.GetUserFrameForAgent(agent);
			}
			WorldFrame userFrameForAgent = base.GetUserFrameForAgent(agent);
			MatrixFrame lookFrame = agent.LookFrame;
			Vec2 vec = (lookFrame.origin.AsVec2 - userFrameForAgent.Origin.AsVec2).Normalized();
			Vec2 vec2 = userFrameForAgent.Origin.AsVec2 + agent.GetInteractionDistanceToUsable(this) * 0.5f * vec;
			Mat3 rotation = lookFrame.rotation;
			userFrameForAgent.Origin.SetVec2(vec2);
			userFrameForAgent.Rotation = rotation;
			return userFrameForAgent;
		}

		// Token: 0x06003120 RID: 12576 RVA: 0x000C7A92 File Offset: 0x000C5C92
		public virtual bool HasAlternative()
		{
			return false;
		}

		// Token: 0x06003121 RID: 12577 RVA: 0x000C7A98 File Offset: 0x000C5C98
		public virtual float GetUsageScoreForAgent(Agent agent)
		{
			WorldPosition origin = this.GetUserFrameForAgent(agent).Origin;
			WorldPosition worldPosition = agent.GetWorldPosition();
			float pathDistance = this.GetPathDistance(agent, ref origin, ref worldPosition);
			float num = ((pathDistance < 0f) ? float.MinValue : (-pathDistance));
			if (agent == this.FavoredUser)
			{
				num *= 0.5f;
			}
			return num;
		}

		// Token: 0x06003122 RID: 12578 RVA: 0x000C7AEC File Offset: 0x000C5CEC
		public virtual float GetUsageScoreForAgent(ValueTuple<Agent, float> agentPair)
		{
			float item = agentPair.Item2;
			float num = ((item < 0f) ? float.MinValue : (-item));
			if (agentPair.Item1 == this.FavoredUser)
			{
				num *= 0.5f;
			}
			return num;
		}

		// Token: 0x06003123 RID: 12579 RVA: 0x000C7B29 File Offset: 0x000C5D29
		public void SetupOnUsingStoppedBehavior(bool autoAttach, Action<Agent, bool> action)
		{
			this._autoAttachOnUsingStopped = autoAttach;
			this._onUsingStoppedAction = action;
		}

		// Token: 0x06003124 RID: 12580 RVA: 0x000C7B3C File Offset: 0x000C5D3C
		private float GetPathDistance(Agent agent, ref WorldPosition userPosition, ref WorldPosition agentPosition)
		{
			StandingPoint.AgentDistanceCache agentDistanceCache;
			float num;
			if (this._cachedAgentDistances.TryGetValue(agent, out agentDistanceCache))
			{
				if (agentDistanceCache.AgentPosition.DistanceSquared(agentPosition.AsVec2) < 1f && agentDistanceCache.StandingPointPosition.DistanceSquared(userPosition.AsVec2) < 1f)
				{
					num = agentDistanceCache.PathDistance;
				}
				else
				{
					if (!Mission.Current.Scene.GetPathDistanceBetweenPositions(ref userPosition, ref agentPosition, agent.Monster.BodyCapsuleRadius, out num))
					{
						num = float.MaxValue;
					}
					agentDistanceCache = new StandingPoint.AgentDistanceCache
					{
						AgentPosition = agentPosition.AsVec2,
						StandingPointPosition = userPosition.AsVec2,
						PathDistance = num
					};
					this._cachedAgentDistances[agent] = agentDistanceCache;
				}
			}
			else
			{
				if (!Mission.Current.Scene.GetPathDistanceBetweenPositions(ref userPosition, ref agentPosition, agent.Monster.BodyCapsuleRadius, out num))
				{
					num = float.MaxValue;
				}
				agentDistanceCache = new StandingPoint.AgentDistanceCache
				{
					AgentPosition = agentPosition.AsVec2,
					StandingPointPosition = userPosition.AsVec2,
					PathDistance = num
				};
				this._cachedAgentDistances[agent] = agentDistanceCache;
			}
			return num;
		}

		// Token: 0x06003125 RID: 12581 RVA: 0x000C7C5B File Offset: 0x000C5E5B
		public override void OnEndMission()
		{
			base.OnEndMission();
			this.FavoredUser = null;
		}

		// Token: 0x06003126 RID: 12582 RVA: 0x000C7C6A File Offset: 0x000C5E6A
		protected internal virtual bool IsUsableBySide(BattleSideEnum side)
		{
			return !base.IsDeactivated && (base.IsInstantUse || !base.HasUser) && (this.StandingPointSide == BattleSideEnum.None || side == this.StandingPointSide);
		}

		// Token: 0x06003127 RID: 12583 RVA: 0x000C7C9A File Offset: 0x000C5E9A
		public override TextObject GetDescriptionText(WeakGameEntity gameEntity)
		{
			return null;
		}

		// Token: 0x06003128 RID: 12584 RVA: 0x000C7CA0 File Offset: 0x000C5EA0
		public override bool IsUsableByAgent(Agent userAgent)
		{
			switch (this._validControllerType)
			{
			case StandingPoint.ValidControllerType.None:
				return false;
			case StandingPoint.ValidControllerType.PlayerOnly:
				return userAgent.IsPlayerControlled;
			case StandingPoint.ValidControllerType.AIOnly:
				return userAgent.IsAIControlled;
			case StandingPoint.ValidControllerType.PlayerOrAI:
				return true;
			default:
				return true;
			}
		}

		// Token: 0x06003129 RID: 12585 RVA: 0x000C7CDF File Offset: 0x000C5EDF
		public void SetUsableByAIOnly()
		{
			this._validControllerType = StandingPoint.ValidControllerType.AIOnly;
		}

		// Token: 0x0600312A RID: 12586 RVA: 0x000C7CE8 File Offset: 0x000C5EE8
		public void SetUsableByPlayerOnly()
		{
			this._validControllerType = StandingPoint.ValidControllerType.PlayerOnly;
		}

		// Token: 0x0600312B RID: 12587 RVA: 0x000C7CF1 File Offset: 0x000C5EF1
		public void SetUsableByPlayerOrAI()
		{
			this._validControllerType = StandingPoint.ValidControllerType.PlayerOrAI;
		}

		// Token: 0x0600312C RID: 12588 RVA: 0x000C7CFA File Offset: 0x000C5EFA
		public StandingPoint()
			: base(false)
		{
		}

		// Token: 0x04001494 RID: 5268
		public bool AutoSheathWeapons = true;

		// Token: 0x04001495 RID: 5269
		public bool AutoEquipWeaponsOnUseStopped;

		// Token: 0x04001496 RID: 5270
		private bool _autoAttachOnUsingStopped = true;

		// Token: 0x04001497 RID: 5271
		private Action<Agent, bool> _onUsingStoppedAction;

		// Token: 0x04001498 RID: 5272
		public bool AutoWieldWeapons;

		// Token: 0x04001499 RID: 5273
		public readonly bool TranslateUser = true;

		// Token: 0x0400149A RID: 5274
		public bool HasRecentlyBeenRechecked;

		// Token: 0x0400149C RID: 5276
		private Dictionary<Agent, StandingPoint.AgentDistanceCache> _cachedAgentDistances;

		// Token: 0x0400149D RID: 5277
		[EditableScriptComponentVariable(true, "")]
		private bool _useOwnPositionInsteadOfWorldPosition;

		// Token: 0x0400149E RID: 5278
		[EditableScriptComponentVariable(true, "")]
		private float _customPlayerInteractionDistance;

		// Token: 0x0400149F RID: 5279
		private bool _needsSingleThreadTickOnce;

		// Token: 0x040014A0 RID: 5280
		private StandingPoint.ValidControllerType _validControllerType = StandingPoint.ValidControllerType.PlayerOrAI;

		// Token: 0x040014A1 RID: 5281
		protected BattleSideEnum StandingPointSide = BattleSideEnum.None;

		// Token: 0x0200063A RID: 1594
		public struct StackArray8StandingPoint
		{
			// Token: 0x17000ABF RID: 2751
			public StandingPoint this[int index]
			{
				get
				{
					switch (index)
					{
					case 0:
						return this._element0;
					case 1:
						return this._element1;
					case 2:
						return this._element2;
					case 3:
						return this._element3;
					case 4:
						return this._element4;
					case 5:
						return this._element5;
					case 6:
						return this._element6;
					case 7:
						return this._element7;
					default:
						return null;
					}
				}
				set
				{
					switch (index)
					{
					case 0:
						this._element0 = value;
						return;
					case 1:
						this._element1 = value;
						return;
					case 2:
						this._element2 = value;
						return;
					case 3:
						this._element3 = value;
						return;
					case 4:
						this._element4 = value;
						return;
					case 5:
						this._element5 = value;
						return;
					case 6:
						this._element6 = value;
						return;
					case 7:
						this._element7 = value;
						return;
					default:
						return;
					}
				}
			}

			// Token: 0x040020EC RID: 8428
			private StandingPoint _element0;

			// Token: 0x040020ED RID: 8429
			private StandingPoint _element1;

			// Token: 0x040020EE RID: 8430
			private StandingPoint _element2;

			// Token: 0x040020EF RID: 8431
			private StandingPoint _element3;

			// Token: 0x040020F0 RID: 8432
			private StandingPoint _element4;

			// Token: 0x040020F1 RID: 8433
			private StandingPoint _element5;

			// Token: 0x040020F2 RID: 8434
			private StandingPoint _element6;

			// Token: 0x040020F3 RID: 8435
			private StandingPoint _element7;

			// Token: 0x040020F4 RID: 8436
			public const int Length = 8;
		}

		// Token: 0x0200063B RID: 1595
		private struct AgentDistanceCache
		{
			// Token: 0x040020F5 RID: 8437
			public Vec2 AgentPosition;

			// Token: 0x040020F6 RID: 8438
			public Vec2 StandingPointPosition;

			// Token: 0x040020F7 RID: 8439
			public float PathDistance;
		}

		// Token: 0x0200063C RID: 1596
		private enum ValidControllerType
		{
			// Token: 0x040020F9 RID: 8441
			None,
			// Token: 0x040020FA RID: 8442
			PlayerOnly,
			// Token: 0x040020FB RID: 8443
			AIOnly,
			// Token: 0x040020FC RID: 8444
			PlayerOrAI
		}
	}
}
