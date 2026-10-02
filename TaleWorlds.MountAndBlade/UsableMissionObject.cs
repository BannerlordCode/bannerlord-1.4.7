using System;
using System.Collections.Generic;
using NetworkMessages.FromServer;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200037D RID: 893
	public abstract class UsableMissionObject : SynchedMissionObject, IFocusable, IUsable, IVisible
	{
		// Token: 0x1700098E RID: 2446
		// (get) Token: 0x06003334 RID: 13108 RVA: 0x000D2C72 File Offset: 0x000D0E72
		public virtual FocusableObjectType FocusableObjectType
		{
			get
			{
				return FocusableObjectType.Item;
			}
		}

		// Token: 0x1700098F RID: 2447
		// (get) Token: 0x06003335 RID: 13109 RVA: 0x000D2C75 File Offset: 0x000D0E75
		public virtual bool IsFocusable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000990 RID: 2448
		// (get) Token: 0x06003336 RID: 13110 RVA: 0x000D2C78 File Offset: 0x000D0E78
		// (set) Token: 0x06003337 RID: 13111 RVA: 0x000D2C80 File Offset: 0x000D0E80
		public Agent UserAgent
		{
			get
			{
				return this._userAgent;
			}
			private set
			{
				if (this._userAgent != value)
				{
					this.PreviousUserAgent = this._userAgent;
					this._userAgent = value;
					base.SetScriptComponentToTickMT(this.GetTickRequirement());
				}
			}
		}

		// Token: 0x17000991 RID: 2449
		// (get) Token: 0x06003338 RID: 13112 RVA: 0x000D2CAA File Offset: 0x000D0EAA
		// (set) Token: 0x06003339 RID: 13113 RVA: 0x000D2CB2 File Offset: 0x000D0EB2
		public Agent PreviousUserAgent { get; private set; }

		// Token: 0x17000992 RID: 2450
		// (get) Token: 0x0600333A RID: 13114 RVA: 0x000D2CBB File Offset: 0x000D0EBB
		// (set) Token: 0x0600333B RID: 13115 RVA: 0x000D2CC3 File Offset: 0x000D0EC3
		public GameEntityWithWorldPosition GameEntityWithWorldPosition { get; private set; }

		// Token: 0x17000993 RID: 2451
		// (get) Token: 0x0600333C RID: 13116 RVA: 0x000D2CCC File Offset: 0x000D0ECC
		// (set) Token: 0x0600333D RID: 13117 RVA: 0x000D2CD4 File Offset: 0x000D0ED4
		public virtual Agent MovingAgent { get; private set; }

		// Token: 0x17000994 RID: 2452
		// (get) Token: 0x0600333E RID: 13118 RVA: 0x000D2CDD File Offset: 0x000D0EDD
		// (set) Token: 0x0600333F RID: 13119 RVA: 0x000D2CE5 File Offset: 0x000D0EE5
		public List<Agent> DefendingAgents { get; private set; }

		// Token: 0x17000995 RID: 2453
		// (get) Token: 0x06003340 RID: 13120 RVA: 0x000D2CEE File Offset: 0x000D0EEE
		public bool HasDefendingAgent
		{
			get
			{
				return this.DefendingAgents != null && this.GetDefendingAgentCount() > 0;
			}
		}

		// Token: 0x17000996 RID: 2454
		// (get) Token: 0x06003341 RID: 13121 RVA: 0x000D2D03 File Offset: 0x000D0F03
		public virtual bool DisableCombatActionsOnUse
		{
			get
			{
				return !this.IsInstantUse;
			}
		}

		// Token: 0x17000997 RID: 2455
		// (get) Token: 0x06003342 RID: 13122 RVA: 0x000D2D0E File Offset: 0x000D0F0E
		// (set) Token: 0x06003343 RID: 13123 RVA: 0x000D2D16 File Offset: 0x000D0F16
		public virtual bool LockUserFrames { get; set; }

		// Token: 0x17000998 RID: 2456
		// (get) Token: 0x06003344 RID: 13124 RVA: 0x000D2D1F File Offset: 0x000D0F1F
		// (set) Token: 0x06003345 RID: 13125 RVA: 0x000D2D27 File Offset: 0x000D0F27
		public virtual bool LockUserPositions { get; set; }

		// Token: 0x17000999 RID: 2457
		// (get) Token: 0x06003346 RID: 13126 RVA: 0x000D2D30 File Offset: 0x000D0F30
		// (set) Token: 0x06003347 RID: 13127 RVA: 0x000D2D38 File Offset: 0x000D0F38
		public bool IsInstantUse { get; protected set; }

		// Token: 0x1700099A RID: 2458
		// (get) Token: 0x06003348 RID: 13128 RVA: 0x000D2D41 File Offset: 0x000D0F41
		// (set) Token: 0x06003349 RID: 13129 RVA: 0x000D2D4C File Offset: 0x000D0F4C
		public bool IsDeactivated
		{
			get
			{
				return this._isDeactivated;
			}
			set
			{
				if (value != this._isDeactivated)
				{
					this._isDeactivated = value;
					if (this._isDeactivated && !GameNetwork.IsClientOrReplay)
					{
						Agent userAgent = this.UserAgent;
						if (userAgent != null)
						{
							userAgent.StopUsingGameObject(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
						}
						bool flag = false;
						while (this.HasAIMovingTo)
						{
							this.MovingAgent.StopUsingGameObject(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
							flag = true;
						}
						while (this.HasDefendingAgent)
						{
							this.DefendingAgents[0].StopUsingGameObject(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
							flag = true;
						}
						if (flag)
						{
							base.SetScriptComponentToTick(this.GetTickRequirement());
						}
					}
				}
			}
		}

		// Token: 0x1700099B RID: 2459
		// (get) Token: 0x0600334A RID: 13130 RVA: 0x000D2DD4 File Offset: 0x000D0FD4
		// (set) Token: 0x0600334B RID: 13131 RVA: 0x000D2DDC File Offset: 0x000D0FDC
		public bool IsDisabledForPlayers
		{
			get
			{
				return this._isDisabledForPlayers;
			}
			set
			{
				if (value != this._isDisabledForPlayers)
				{
					this._isDisabledForPlayers = value;
					if (this._isDisabledForPlayers && !GameNetwork.IsClientOrReplay && this.UserAgent != null && !this.UserAgent.IsAIControlled)
					{
						this.UserAgent.StopUsingGameObject(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
					}
				}
			}
		}

		// Token: 0x1700099C RID: 2460
		// (get) Token: 0x0600334C RID: 13132 RVA: 0x000D2E2A File Offset: 0x000D102A
		public virtual WeakGameEntity InteractionEntity
		{
			get
			{
				return base.GameEntity;
			}
		}

		// Token: 0x1700099D RID: 2461
		// (get) Token: 0x0600334D RID: 13133 RVA: 0x000D2E32 File Offset: 0x000D1032
		public bool HasAIUser
		{
			get
			{
				return this.HasUser && this.UserAgent.IsAIControlled;
			}
		}

		// Token: 0x1700099E RID: 2462
		// (get) Token: 0x0600334E RID: 13134 RVA: 0x000D2E49 File Offset: 0x000D1049
		public bool HasUser
		{
			get
			{
				return this.UserAgent != null;
			}
		}

		// Token: 0x1700099F RID: 2463
		// (get) Token: 0x0600334F RID: 13135 RVA: 0x000D2E54 File Offset: 0x000D1054
		public virtual bool HasAIMovingTo
		{
			get
			{
				return this.MovingAgent != null;
			}
		}

		// Token: 0x170009A0 RID: 2464
		// (get) Token: 0x06003350 RID: 13136 RVA: 0x000D2E60 File Offset: 0x000D1060
		// (set) Token: 0x06003351 RID: 13137 RVA: 0x000D2E7C File Offset: 0x000D107C
		public bool IsVisible
		{
			get
			{
				return base.GameEntity.IsVisibleIncludeParents();
			}
			set
			{
				base.GameEntity.SetVisibilityExcludeParents(value);
			}
		}

		// Token: 0x06003352 RID: 13138 RVA: 0x000D2E98 File Offset: 0x000D1098
		protected UsableMissionObject(bool isInstantUse = false)
		{
			this._components = new List<UsableMissionObjectComponent>();
			this.IsInstantUse = isInstantUse;
			this.GameEntityWithWorldPosition = null;
			this._needsSingleThreadTickOnce = false;
		}

		// Token: 0x06003353 RID: 13139 RVA: 0x000D2ED6 File Offset: 0x000D10D6
		public virtual void OnUserConversationStart()
		{
		}

		// Token: 0x06003354 RID: 13140 RVA: 0x000D2ED8 File Offset: 0x000D10D8
		public virtual void OnUserConversationEnd()
		{
		}

		// Token: 0x06003355 RID: 13141 RVA: 0x000D2EDA File Offset: 0x000D10DA
		public void SetAreUserPositionsUpdatedInTheMachineTick(bool value)
		{
			this._areUserPositionsUpdatedInTheMachineTick = value;
		}

		// Token: 0x06003356 RID: 13142 RVA: 0x000D2EE3 File Offset: 0x000D10E3
		public bool GetIsUserPositionsUpdatedInTheMachineTick()
		{
			return this._areUserPositionsUpdatedInTheMachineTick;
		}

		// Token: 0x06003357 RID: 13143 RVA: 0x000D2EEB File Offset: 0x000D10EB
		public void SetIsDeactivatedSynched(bool value)
		{
			if (this.IsDeactivated != value)
			{
				if (GameNetwork.IsServerOrRecorder)
				{
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new SetUsableMissionObjectIsDeactivated(base.Id, value));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
				}
				this.IsDeactivated = value;
			}
		}

		// Token: 0x06003358 RID: 13144 RVA: 0x000D2F21 File Offset: 0x000D1121
		public void SetIsDisabledForPlayersSynched(bool value)
		{
			if (this.IsDisabledForPlayers != value)
			{
				if (GameNetwork.IsServerOrRecorder)
				{
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new SetUsableMissionObjectIsDisabledForPlayers(base.Id, value));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
				}
				this.IsDisabledForPlayers = value;
			}
		}

		// Token: 0x06003359 RID: 13145 RVA: 0x000D2F57 File Offset: 0x000D1157
		public virtual bool IsDisabledForAgent(Agent agent)
		{
			return this.IsDeactivated || agent.MountAgent != null || (this.IsDisabledForPlayers && !agent.IsAIControlled) || !agent.IsAbleToUseMachine();
		}

		// Token: 0x0600335A RID: 13146 RVA: 0x000D2F84 File Offset: 0x000D1184
		public void AddComponent(UsableMissionObjectComponent component)
		{
			this._components.Add(component);
			component.OnAdded(base.Scene);
			base.SetScriptComponentToTick(this.GetTickRequirement());
		}

		// Token: 0x0600335B RID: 13147 RVA: 0x000D2FAA File Offset: 0x000D11AA
		public void RemoveComponent(UsableMissionObjectComponent component)
		{
			component.OnRemoved();
			this._components.Remove(component);
			base.SetScriptComponentToTick(this.GetTickRequirement());
		}

		// Token: 0x0600335C RID: 13148 RVA: 0x000D2FCB File Offset: 0x000D11CB
		public T GetComponent<T>() where T : UsableMissionObjectComponent
		{
			return this._components.Find((UsableMissionObjectComponent c) => c is T) as T;
		}

		// Token: 0x0600335D RID: 13149 RVA: 0x000D3001 File Offset: 0x000D1201
		private void CollectChildEntities()
		{
			this.CollectChildEntitiesAux(base.GameEntity);
		}

		// Token: 0x0600335E RID: 13150 RVA: 0x000D3010 File Offset: 0x000D1210
		private void CollectChildEntitiesAux(WeakGameEntity entity)
		{
			foreach (WeakGameEntity weakGameEntity in entity.GetChildren())
			{
				this.CollectChildEntity(weakGameEntity);
				if (weakGameEntity.GetScriptCount() == 0)
				{
					this.CollectChildEntitiesAux(weakGameEntity);
				}
			}
		}

		// Token: 0x0600335F RID: 13151 RVA: 0x000D3070 File Offset: 0x000D1270
		public void RefreshGameEntityWithWorldPosition()
		{
			this.GameEntityWithWorldPosition = new GameEntityWithWorldPosition(base.GameEntity);
		}

		// Token: 0x06003360 RID: 13152 RVA: 0x000D3083 File Offset: 0x000D1283
		protected virtual void CollectChildEntity(WeakGameEntity childEntity)
		{
		}

		// Token: 0x06003361 RID: 13153 RVA: 0x000D3085 File Offset: 0x000D1285
		protected virtual bool VerifyChildEntities(ref string errorMessage)
		{
			return true;
		}

		// Token: 0x06003362 RID: 13154 RVA: 0x000D3088 File Offset: 0x000D1288
		protected internal override void OnInit()
		{
			base.OnInit();
			this.CollectChildEntities();
			this.LockUserFrames = !this.IsInstantUse;
			this.RefreshGameEntityWithWorldPosition();
		}

		// Token: 0x06003363 RID: 13155 RVA: 0x000D30AB File Offset: 0x000D12AB
		protected internal override void OnEditorInit()
		{
			base.OnEditorInit();
			this.CollectChildEntities();
		}

		// Token: 0x06003364 RID: 13156 RVA: 0x000D30BC File Offset: 0x000D12BC
		protected internal override void OnMissionReset()
		{
			base.OnMissionReset();
			foreach (UsableMissionObjectComponent usableMissionObjectComponent in this._components)
			{
				usableMissionObjectComponent.OnMissionReset();
			}
		}

		// Token: 0x06003365 RID: 13157 RVA: 0x000D3114 File Offset: 0x000D1314
		public virtual void OnFocusGain(Agent userAgent)
		{
			foreach (UsableMissionObjectComponent usableMissionObjectComponent in this._components)
			{
				usableMissionObjectComponent.OnFocusGain(userAgent);
			}
		}

		// Token: 0x06003366 RID: 13158 RVA: 0x000D3168 File Offset: 0x000D1368
		public virtual void OnFocusLose(Agent userAgent)
		{
			foreach (UsableMissionObjectComponent usableMissionObjectComponent in this._components)
			{
				usableMissionObjectComponent.OnFocusLose(userAgent);
			}
		}

		// Token: 0x06003367 RID: 13159 RVA: 0x000D31BC File Offset: 0x000D13BC
		public virtual TextObject GetInfoTextForBeingNotInteractable(Agent userAgent)
		{
			return TextObject.GetEmpty();
		}

		// Token: 0x06003368 RID: 13160 RVA: 0x000D31C3 File Offset: 0x000D13C3
		public virtual void SetUserForClient(Agent userAgent)
		{
			Agent userAgent2 = this.UserAgent;
			if (userAgent2 != null)
			{
				userAgent2.SetUsedGameObjectForClient(null);
			}
			this.UserAgent = userAgent;
			if (userAgent != null)
			{
				userAgent.SetUsedGameObjectForClient(this);
			}
		}

		// Token: 0x06003369 RID: 13161 RVA: 0x000D31E8 File Offset: 0x000D13E8
		public virtual void OnUse(Agent userAgent, sbyte agentBoneIndex)
		{
			if (!GameNetwork.IsClientOrReplay)
			{
				if (!userAgent.IsAIControlled && this.HasAIUser)
				{
					this.UserAgent.StopUsingGameObject(false, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
				}
				if (this.IsAIMovingTo(userAgent))
				{
					Formation formation = userAgent.Formation;
					if (formation != null)
					{
						formation.Team.DetachmentManager.RemoveAgentAsMovingToDetachment(userAgent);
					}
					this.RemoveMovingAgent(userAgent);
					base.SetScriptComponentToTick(this.GetTickRequirement());
				}
				while (this.HasAIMovingTo && !this.IsInstantUse)
				{
					this.MovingAgent.StopUsingGameObject(false, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
				}
				foreach (UsableMissionObjectComponent usableMissionObjectComponent in this._components)
				{
					usableMissionObjectComponent.OnUse(userAgent);
				}
				this.UserAgent = userAgent;
				if (GameNetwork.IsServerOrRecorder)
				{
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new UseObject(userAgent.Index, base.Id));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
					return;
				}
			}
			else
			{
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
		}

		// Token: 0x0600336A RID: 13162 RVA: 0x000D3340 File Offset: 0x000D1540
		public virtual void OnAIMoveToUse(Agent userAgent, IDetachment detachment)
		{
			this.AddMovingAgent(userAgent);
			Formation formation = userAgent.Formation;
			if (formation != null)
			{
				formation.Team.DetachmentManager.AddAgentAsMovingToDetachment(userAgent, detachment);
			}
			base.SetScriptComponentToTick(this.GetTickRequirement());
		}

		// Token: 0x0600336B RID: 13163 RVA: 0x000D3374 File Offset: 0x000D1574
		public virtual void OnUseStopped(Agent userAgent, bool isSuccessful, int preferenceIndex)
		{
			foreach (UsableMissionObjectComponent usableMissionObjectComponent in this._components)
			{
				usableMissionObjectComponent.OnUseStopped(userAgent, isSuccessful);
			}
			this.UserAgent = null;
		}

		// Token: 0x0600336C RID: 13164 RVA: 0x000D33D0 File Offset: 0x000D15D0
		public virtual void OnMoveToStopped(Agent movingAgent)
		{
			Formation formation = movingAgent.Formation;
			if (formation != null)
			{
				formation.Team.DetachmentManager.RemoveAgentAsMovingToDetachment(movingAgent);
			}
			this.RemoveMovingAgent(movingAgent);
			base.SetScriptComponentToTick(this.GetTickRequirement());
		}

		// Token: 0x0600336D RID: 13165 RVA: 0x000D3401 File Offset: 0x000D1601
		public virtual int GetMovingAgentCount()
		{
			if (this.MovingAgent == null)
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x0600336E RID: 13166 RVA: 0x000D340E File Offset: 0x000D160E
		public virtual Agent GetMovingAgentWithIndex(int index)
		{
			return this.MovingAgent;
		}

		// Token: 0x0600336F RID: 13167 RVA: 0x000D3416 File Offset: 0x000D1616
		public virtual void RemoveMovingAgent(Agent movingAgent)
		{
			this.MovingAgent = null;
		}

		// Token: 0x06003370 RID: 13168 RVA: 0x000D341F File Offset: 0x000D161F
		public virtual void AddMovingAgent(Agent movingAgent)
		{
			this.MovingAgent = movingAgent;
		}

		// Token: 0x06003371 RID: 13169 RVA: 0x000D3428 File Offset: 0x000D1628
		public void OnAIDefendBegin(Agent agent, IDetachment detachment)
		{
			this.AddDefendingAgent(agent);
			Formation formation = agent.Formation;
			if (formation != null)
			{
				formation.Team.DetachmentManager.AddAgentAsDefendingToDetachment(agent, detachment);
			}
			base.SetScriptComponentToTick(this.GetTickRequirement());
		}

		// Token: 0x06003372 RID: 13170 RVA: 0x000D345A File Offset: 0x000D165A
		public void OnAIDefendEnd(Agent agent)
		{
			Formation formation = agent.Formation;
			if (formation != null)
			{
				formation.Team.DetachmentManager.RemoveAgentAsDefendingToDetachment(agent);
			}
			this.RemoveDefendingAgent(agent);
			base.SetScriptComponentToTick(this.GetTickRequirement());
		}

		// Token: 0x06003373 RID: 13171 RVA: 0x000D348B File Offset: 0x000D168B
		public void InitializeDefendingAgents()
		{
			if (this.DefendingAgents == null)
			{
				this.DefendingAgents = new List<Agent>();
			}
		}

		// Token: 0x06003374 RID: 13172 RVA: 0x000D34A0 File Offset: 0x000D16A0
		public int GetDefendingAgentCount()
		{
			return this.DefendingAgents.Count;
		}

		// Token: 0x06003375 RID: 13173 RVA: 0x000D34AD File Offset: 0x000D16AD
		public void AddDefendingAgent(Agent agent)
		{
			this.DefendingAgents.Add(agent);
		}

		// Token: 0x06003376 RID: 13174 RVA: 0x000D34BB File Offset: 0x000D16BB
		public void RemoveDefendingAgent(Agent agent)
		{
			this.DefendingAgents.Remove(agent);
		}

		// Token: 0x06003377 RID: 13175 RVA: 0x000D34CA File Offset: 0x000D16CA
		public bool IsAgentDefending(Agent agent)
		{
			return this.DefendingAgents.Contains(agent);
		}

		// Token: 0x06003378 RID: 13176 RVA: 0x000D34D8 File Offset: 0x000D16D8
		public virtual void SimulateTick(float dt)
		{
		}

		// Token: 0x06003379 RID: 13177 RVA: 0x000D34DC File Offset: 0x000D16DC
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			if (this.HasUser || this.HasAIMovingTo)
			{
				return base.GetTickRequirement() | ScriptComponentBehavior.TickRequirement.Tick | ScriptComponentBehavior.TickRequirement.TickParallel2;
			}
			if (this.HasDefendingAgent)
			{
				return base.GetTickRequirement() | ScriptComponentBehavior.TickRequirement.Tick;
			}
			using (List<UsableMissionObjectComponent>.Enumerator enumerator = this._components.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.IsOnTickRequired())
					{
						return base.GetTickRequirement() | ScriptComponentBehavior.TickRequirement.Tick;
					}
				}
			}
			return base.GetTickRequirement();
		}

		// Token: 0x0600337A RID: 13178 RVA: 0x000D3570 File Offset: 0x000D1770
		protected internal override void OnTickParallel2(float dt)
		{
			for (int i = this.GetMovingAgentCount() - 1; i >= 0; i--)
			{
				if (!this.GetMovingAgentWithIndex(i).IsActive())
				{
					this._needsSingleThreadTickOnce = true;
				}
			}
		}

		// Token: 0x0600337B RID: 13179 RVA: 0x000D35A8 File Offset: 0x000D17A8
		protected internal override void OnTick(float dt)
		{
			base.OnTick(dt);
			foreach (UsableMissionObjectComponent usableMissionObjectComponent in this._components)
			{
				usableMissionObjectComponent.OnTick(dt);
			}
			if (!this._areUserPositionsUpdatedInTheMachineTick && this.HasUser && this.HasUserPositionsChanged(this.UserAgent))
			{
				if (this.LockUserFrames)
				{
					WorldFrame userFrameForAgent = this.GetUserFrameForAgent(this.UserAgent);
					Agent userAgent = this.UserAgent;
					Vec2 asVec = userFrameForAgent.Origin.AsVec2;
					userAgent.SetTargetPositionAndDirection(in asVec, in userFrameForAgent.Rotation.f);
				}
				else if (this.LockUserPositions)
				{
					this.UserAgent.SetTargetPosition(this.GetUserFrameForAgent(this.UserAgent).Origin.AsVec2);
				}
			}
			if (this._needsSingleThreadTickOnce)
			{
				this._needsSingleThreadTickOnce = false;
				for (int i = this.GetMovingAgentCount() - 1; i >= 0; i--)
				{
					Agent movingAgentWithIndex = this.GetMovingAgentWithIndex(i);
					if (!movingAgentWithIndex.IsActive())
					{
						Formation formation = movingAgentWithIndex.Formation;
						if (formation != null)
						{
							formation.Team.DetachmentManager.RemoveAgentAsMovingToDetachment(movingAgentWithIndex);
						}
						this.RemoveMovingAgent(movingAgentWithIndex);
						base.SetScriptComponentToTick(this.GetTickRequirement());
					}
				}
			}
		}

		// Token: 0x0600337C RID: 13180 RVA: 0x000D36F4 File Offset: 0x000D18F4
		protected internal override void OnEditorTick(float dt)
		{
			base.OnEditorTick(dt);
			foreach (UsableMissionObjectComponent usableMissionObjectComponent in this._components)
			{
				usableMissionObjectComponent.OnEditorTick(dt);
			}
		}

		// Token: 0x0600337D RID: 13181 RVA: 0x000D374C File Offset: 0x000D194C
		protected internal override void OnEditorValidate()
		{
			base.OnEditorValidate();
			foreach (UsableMissionObjectComponent usableMissionObjectComponent in this._components)
			{
				usableMissionObjectComponent.OnEditorValidate();
			}
			string text = null;
			if (!this.VerifyChildEntities(ref text))
			{
				MBDebug.ShowWarning(text);
			}
		}

		// Token: 0x0600337E RID: 13182 RVA: 0x000D37B4 File Offset: 0x000D19B4
		protected override void OnRemoved(int removeReason)
		{
			base.OnRemoved(removeReason);
			foreach (UsableMissionObjectComponent usableMissionObjectComponent in this._components)
			{
				usableMissionObjectComponent.OnRemoved();
			}
		}

		// Token: 0x0600337F RID: 13183 RVA: 0x000D380C File Offset: 0x000D1A0C
		public virtual WorldFrame GetUserFrameForAgent(Agent agent)
		{
			return this.GameEntityWithWorldPosition.WorldFrame;
		}

		// Token: 0x06003380 RID: 13184 RVA: 0x000D381C File Offset: 0x000D1A1C
		public override string ToString()
		{
			string text = base.GetType() + " with Components:";
			foreach (UsableMissionObjectComponent usableMissionObjectComponent in this._components)
			{
				text = string.Concat(new object[] { text, "[", usableMissionObjectComponent, "]" });
			}
			return text;
		}

		// Token: 0x06003381 RID: 13185 RVA: 0x000D38A0 File Offset: 0x000D1AA0
		public virtual bool IsAIMovingTo(Agent agent)
		{
			return this.MovingAgent == agent;
		}

		// Token: 0x06003382 RID: 13186 RVA: 0x000D38AC File Offset: 0x000D1AAC
		public virtual bool HasUserPositionsChanged(Agent agent)
		{
			return base.GameEntity.GetHasFrameChanged();
		}

		// Token: 0x06003383 RID: 13187 RVA: 0x000D38C8 File Offset: 0x000D1AC8
		public override void WriteToNetwork()
		{
			base.WriteToNetwork();
			GameNetworkMessage.WriteBoolToPacket(this.IsDeactivated);
			GameNetworkMessage.WriteBoolToPacket(this.IsDisabledForPlayers);
			GameNetworkMessage.WriteBoolToPacket(this.UserAgent != null);
			if (this.UserAgent != null)
			{
				GameNetworkMessage.WriteAgentIndexToPacket(this.UserAgent.Index);
			}
		}

		// Token: 0x06003384 RID: 13188 RVA: 0x000D3917 File Offset: 0x000D1B17
		public virtual bool IsUsableByAgent(Agent userAgent)
		{
			return true;
		}

		// Token: 0x06003385 RID: 13189 RVA: 0x000D391A File Offset: 0x000D1B1A
		public void SetCustomLocalFrame(in MatrixFrame customLocalFrame)
		{
			this.GameEntityWithWorldPosition.SetCustomLocalFrame(in customLocalFrame);
		}

		// Token: 0x06003386 RID: 13190 RVA: 0x000D3928 File Offset: 0x000D1B28
		public override void OnEndMission()
		{
			this.UserAgent = null;
			for (int i = this.GetMovingAgentCount() - 1; i >= 0; i--)
			{
				this.RemoveMovingAgent(this.GetMovingAgentWithIndex(i));
			}
			if (this.HasDefendingAgent)
			{
				for (int j = this.GetDefendingAgentCount() - 1; j >= 0; j--)
				{
					this.DefendingAgents.RemoveAt(j);
				}
			}
			base.SetScriptComponentToTick(this.GetTickRequirement());
		}

		// Token: 0x06003387 RID: 13191 RVA: 0x000D3990 File Offset: 0x000D1B90
		public override void OnAfterReadFromNetwork(ValueTuple<BaseSynchedMissionObjectReadableRecord, ISynchedMissionObjectReadableRecord> synchedMissionObjectReadableRecord, bool allowVisibilityUpdate = true)
		{
			base.OnAfterReadFromNetwork(synchedMissionObjectReadableRecord, allowVisibilityUpdate);
			UsableMissionObject.UsableMissionObjectRecord usableMissionObjectRecord = (UsableMissionObject.UsableMissionObjectRecord)synchedMissionObjectReadableRecord.Item2;
			this.IsDeactivated = usableMissionObjectRecord.IsDeactivated;
			this.IsDisabledForPlayers = usableMissionObjectRecord.IsDisabledForPlayers;
			if (usableMissionObjectRecord.IsUserAgentExists)
			{
				Agent agentFromIndex = Mission.MissionNetworkHelper.GetAgentFromIndex(usableMissionObjectRecord.AgentIndex, false);
				if (agentFromIndex != null)
				{
					this.SetUserForClient(agentFromIndex);
				}
			}
		}

		// Token: 0x06003388 RID: 13192
		public abstract TextObject GetDescriptionText(WeakGameEntity gameEntity);

		// Token: 0x040015A2 RID: 5538
		private Agent _userAgent;

		// Token: 0x040015A7 RID: 5543
		private bool _areUserPositionsUpdatedInTheMachineTick;

		// Token: 0x040015A8 RID: 5544
		private readonly List<UsableMissionObjectComponent> _components;

		// Token: 0x040015A9 RID: 5545
		[EditableScriptComponentVariable(false, "")]
		public TextObject DescriptionMessage = TextObject.GetEmpty();

		// Token: 0x040015AA RID: 5546
		[EditableScriptComponentVariable(false, "")]
		public TextObject ActionMessage = TextObject.GetEmpty();

		// Token: 0x040015AB RID: 5547
		private bool _needsSingleThreadTickOnce;

		// Token: 0x040015AF RID: 5551
		private bool _isDeactivated;

		// Token: 0x040015B0 RID: 5552
		private bool _isDisabledForPlayers;

		// Token: 0x02000655 RID: 1621
		[DefineSynchedMissionObjectType(typeof(UsableMissionObject))]
		public struct UsableMissionObjectRecord : ISynchedMissionObjectReadableRecord
		{
			// Token: 0x17000AC1 RID: 2753
			// (get) Token: 0x06004047 RID: 16455 RVA: 0x000F8757 File Offset: 0x000F6957
			// (set) Token: 0x06004048 RID: 16456 RVA: 0x000F875F File Offset: 0x000F695F
			public bool IsDeactivated { get; private set; }

			// Token: 0x17000AC2 RID: 2754
			// (get) Token: 0x06004049 RID: 16457 RVA: 0x000F8768 File Offset: 0x000F6968
			// (set) Token: 0x0600404A RID: 16458 RVA: 0x000F8770 File Offset: 0x000F6970
			public bool IsDisabledForPlayers { get; private set; }

			// Token: 0x17000AC3 RID: 2755
			// (get) Token: 0x0600404B RID: 16459 RVA: 0x000F8779 File Offset: 0x000F6979
			// (set) Token: 0x0600404C RID: 16460 RVA: 0x000F8781 File Offset: 0x000F6981
			public bool IsUserAgentExists { get; private set; }

			// Token: 0x17000AC4 RID: 2756
			// (get) Token: 0x0600404D RID: 16461 RVA: 0x000F878A File Offset: 0x000F698A
			// (set) Token: 0x0600404E RID: 16462 RVA: 0x000F8792 File Offset: 0x000F6992
			public int AgentIndex { get; private set; }

			// Token: 0x0600404F RID: 16463 RVA: 0x000F879B File Offset: 0x000F699B
			public UsableMissionObjectRecord(bool isDeactivated, bool isDisabledForPlayers, bool isUserAgentExists, int agentIndex)
			{
				this.IsDeactivated = isDeactivated;
				this.IsDisabledForPlayers = isDisabledForPlayers;
				this.IsUserAgentExists = isUserAgentExists;
				this.AgentIndex = agentIndex;
			}

			// Token: 0x06004050 RID: 16464 RVA: 0x000F87BA File Offset: 0x000F69BA
			public bool ReadFromNetwork(ref bool bufferReadValid)
			{
				this.IsDeactivated = GameNetworkMessage.ReadBoolFromPacket(ref bufferReadValid);
				this.IsDisabledForPlayers = GameNetworkMessage.ReadBoolFromPacket(ref bufferReadValid);
				this.IsUserAgentExists = GameNetworkMessage.ReadBoolFromPacket(ref bufferReadValid);
				if (this.IsUserAgentExists)
				{
					this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref bufferReadValid);
				}
				return bufferReadValid;
			}
		}
	}
}
