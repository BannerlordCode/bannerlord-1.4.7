using System;
using Helpers;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Incidents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem.GameState
{
	// Token: 0x02000399 RID: 921
	public class MapState : GameState
	{
		// Token: 0x17000C9F RID: 3231
		// (get) Token: 0x06003536 RID: 13622 RVA: 0x000D98E5 File Offset: 0x000D7AE5
		// (set) Token: 0x06003537 RID: 13623 RVA: 0x000D98ED File Offset: 0x000D7AED
		public Incident NextIncident
		{
			get
			{
				return this._nextIncident;
			}
			set
			{
				this._nextIncident = value;
			}
		}

		// Token: 0x17000CA0 RID: 3232
		// (get) Token: 0x06003538 RID: 13624 RVA: 0x000D98F6 File Offset: 0x000D7AF6
		// (set) Token: 0x06003539 RID: 13625 RVA: 0x000D98FE File Offset: 0x000D7AFE
		public MenuContext MenuContext
		{
			get
			{
				return this._menuContext;
			}
			private set
			{
				this._menuContext = value;
			}
		}

		// Token: 0x17000CA1 RID: 3233
		// (get) Token: 0x0600353A RID: 13626 RVA: 0x000D9907 File Offset: 0x000D7B07
		// (set) Token: 0x0600353B RID: 13627 RVA: 0x000D9918 File Offset: 0x000D7B18
		public string GameMenuId
		{
			get
			{
				return Campaign.Current.MapStateData.GameMenuId;
			}
			set
			{
				Campaign.Current.MapStateData.GameMenuId = value;
			}
		}

		// Token: 0x17000CA2 RID: 3234
		// (get) Token: 0x0600353C RID: 13628 RVA: 0x000D992A File Offset: 0x000D7B2A
		public bool AtMenu
		{
			get
			{
				return this.MenuContext != null;
			}
		}

		// Token: 0x17000CA3 RID: 3235
		// (get) Token: 0x0600353D RID: 13629 RVA: 0x000D9935 File Offset: 0x000D7B35
		public bool MapConversationActive
		{
			get
			{
				return this._mapConversationActive;
			}
		}

		// Token: 0x17000CA4 RID: 3236
		// (get) Token: 0x0600353E RID: 13630 RVA: 0x000D993D File Offset: 0x000D7B3D
		// (set) Token: 0x0600353F RID: 13631 RVA: 0x000D9945 File Offset: 0x000D7B45
		public IMapStateHandler Handler
		{
			get
			{
				return this._handler;
			}
			set
			{
				this._handler = value;
			}
		}

		// Token: 0x17000CA5 RID: 3237
		// (get) Token: 0x06003540 RID: 13632 RVA: 0x000D994E File Offset: 0x000D7B4E
		public bool IsSimulationActive
		{
			get
			{
				return this._battleSimulation != null;
			}
		}

		// Token: 0x06003541 RID: 13633 RVA: 0x000D9959 File Offset: 0x000D7B59
		protected override void OnIdleTick(float dt)
		{
			base.OnIdleTick(dt);
			IMapStateHandler handler = this.Handler;
			if (handler == null)
			{
				return;
			}
			handler.OnIdleTick(dt);
		}

		// Token: 0x06003542 RID: 13634 RVA: 0x000D9973 File Offset: 0x000D7B73
		private void RefreshHandler()
		{
			IMapStateHandler handler = this.Handler;
			if (handler == null)
			{
				return;
			}
			handler.OnRefreshState();
		}

		// Token: 0x06003543 RID: 13635 RVA: 0x000D9985 File Offset: 0x000D7B85
		public void OnJoinArmy()
		{
			this.RefreshHandler();
		}

		// Token: 0x06003544 RID: 13636 RVA: 0x000D998D File Offset: 0x000D7B8D
		public void OnLeaveArmy()
		{
			this.RefreshHandler();
		}

		// Token: 0x06003545 RID: 13637 RVA: 0x000D9995 File Offset: 0x000D7B95
		public void OnDispersePlayerLeadedArmy()
		{
			this.RefreshHandler();
		}

		// Token: 0x06003546 RID: 13638 RVA: 0x000D999D File Offset: 0x000D7B9D
		public void OnArmyCreated(MobileParty mobileParty)
		{
			this.RefreshHandler();
		}

		// Token: 0x06003547 RID: 13639 RVA: 0x000D99A5 File Offset: 0x000D7BA5
		public void StartIncident(Incident incident)
		{
			IMapStateHandler handler = this.Handler;
			if (handler == null)
			{
				return;
			}
			handler.OnIncidentStarted(incident);
		}

		// Token: 0x06003548 RID: 13640 RVA: 0x000D99B8 File Offset: 0x000D7BB8
		public void OnMainPartyEncounter()
		{
			IMapStateHandler handler = this.Handler;
			if (handler == null)
			{
				return;
			}
			handler.OnMainPartyEncounter();
		}

		// Token: 0x06003549 RID: 13641 RVA: 0x000D99CC File Offset: 0x000D7BCC
		public void ProcessTravel(CampaignVec2 moveTargetPoint)
		{
			MobileParty.MainParty.ForceAiNoPathMode = false;
			NavigationHelper.EmbarkDisembarkData embarkDisembarkData = NavigationHelper.EmbarkDisembarkData.Invalid;
			if (MobileParty.MainParty.HasNavalNavigationCapability)
			{
				Vec2 vec = (moveTargetPoint.ToVec2() - MobileParty.MainParty.Position.ToVec2()).Normalized();
				embarkDisembarkData = NavigationHelper.GetEmbarkAndDisembarkDataForPlayer(MobileParty.MainParty.Position, vec, moveTargetPoint, moveTargetPoint.IsOnLand);
				if (embarkDisembarkData.IsTargetingTheDeadZone)
				{
					moveTargetPoint = (MobileParty.MainParty.IsTransitionInProgress ? embarkDisembarkData.TransitionEndPosition : embarkDisembarkData.TransitionStartPosition);
				}
			}
			MobileParty.NavigationType navigationType;
			if (NavigationHelper.CanPlayerNavigateToPosition(moveTargetPoint, out navigationType))
			{
				MobileParty.MainParty.SetMoveGoToPoint(moveTargetPoint, navigationType);
			}
			if (MobileParty.MainParty.HasNavalNavigationCapability && !embarkDisembarkData.IsTargetingTheDeadZone && navigationType == MobileParty.NavigationType.Naval && MobileParty.MainParty.IsCurrentlyAtSea && MobileParty.MainParty.IsTransitionInProgress)
			{
				MobileParty.MainParty.CancelNavigationTransition();
			}
		}

		// Token: 0x0600354A RID: 13642 RVA: 0x000D9AAC File Offset: 0x000D7CAC
		protected override void OnTick(float dt)
		{
			base.OnTick(dt);
			if (Campaign.Current.SaveHandler.IsSaving)
			{
				Campaign.Current.SaveHandler.SaveTick();
				return;
			}
			if (this._battleSimulation != null)
			{
				this._battleSimulation.Tick(dt);
			}
			else if (this.AtMenu)
			{
				this.OnMenuModeTick(dt);
			}
			this.OnMapModeTick(dt);
			if (!Campaign.Current.SaveHandler.IsSaving)
			{
				Campaign.Current.SaveHandler.CampaignTick();
			}
		}

		// Token: 0x0600354B RID: 13643 RVA: 0x000D9B2D File Offset: 0x000D7D2D
		private void OnMenuModeTick(float dt)
		{
			this.MenuContext.OnTick(dt);
			IMapStateHandler handler = this.Handler;
			if (handler == null)
			{
				return;
			}
			handler.OnMenuModeTick(dt);
		}

		// Token: 0x0600354C RID: 13644 RVA: 0x000D9B4C File Offset: 0x000D7D4C
		private void OnMapModeTick(float dt)
		{
			if (this._closeScreenNextFrame)
			{
				Game.Current.GameStateManager.CleanStates(0);
				return;
			}
			if (this.Handler != null)
			{
				this.Handler.BeforeTick(dt);
			}
			if (Campaign.Current != null && base.GameStateManager.ActiveState == this)
			{
				Campaign.Current.RealTick(dt);
				IMapStateHandler handler = this.Handler;
				if (handler != null)
				{
					handler.Tick(dt);
				}
				IMapStateHandler handler2 = this.Handler;
				if (handler2 != null)
				{
					handler2.AfterTick(dt);
				}
				Campaign.Current.Tick();
				IMapStateHandler handler3 = this.Handler;
				if (handler3 == null)
				{
					return;
				}
				handler3.AfterWaitTick(dt);
			}
		}

		// Token: 0x0600354D RID: 13645 RVA: 0x000D9BE8 File Offset: 0x000D7DE8
		public void OnLoadingFinished()
		{
			if (!string.IsNullOrEmpty(this.GameMenuId))
			{
				this.EnterMenuMode();
			}
			this.RefreshHandler();
			if (Campaign.Current.CurrentMenuContext != null && Campaign.Current.CurrentMenuContext.GameMenu != null && Campaign.Current.CurrentMenuContext.GameMenu.IsWaitMenu)
			{
				Campaign.Current.CurrentMenuContext.GameMenu.StartWait();
			}
			Campaign.Current.TimeControlMode = CampaignTimeControlMode.Stop;
			IMapStateHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnGameLoadFinished();
		}

		// Token: 0x0600354E RID: 13646 RVA: 0x000D9C70 File Offset: 0x000D7E70
		public void OnMapConversationStarts(ConversationCharacterData playerCharacterData, ConversationCharacterData conversationPartnerData)
		{
			this._mapConversationActive = true;
			IMapStateHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnMapConversationStarts(playerCharacterData, conversationPartnerData);
		}

		// Token: 0x0600354F RID: 13647 RVA: 0x000D9C8C File Offset: 0x000D7E8C
		public void OnMapConversationOver()
		{
			IMapStateHandler handler = this._handler;
			if (handler != null)
			{
				handler.OnMapConversationOver();
			}
			this._mapConversationActive = false;
			if (Game.Current.GameStateManager.ActiveState is MapState)
			{
				MenuContext menuContext = this.MenuContext;
				if (menuContext != null)
				{
					menuContext.Refresh();
				}
			}
			this.RefreshHandler();
		}

		// Token: 0x06003550 RID: 13648 RVA: 0x000D9CDE File Offset: 0x000D7EDE
		internal void OnSignalPeriodicEvents()
		{
			IMapStateHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnSignalPeriodicEvents();
		}

		// Token: 0x06003551 RID: 13649 RVA: 0x000D9CF0 File Offset: 0x000D7EF0
		internal void OnHourlyTick()
		{
			IMapStateHandler handler = this._handler;
			if (handler != null)
			{
				handler.OnHourlyTick();
			}
			MenuContext menuContext = this.MenuContext;
			if (menuContext == null)
			{
				return;
			}
			menuContext.OnHourlyTick();
		}

		// Token: 0x06003552 RID: 13650 RVA: 0x000D9D13 File Offset: 0x000D7F13
		protected override void OnActivate()
		{
			base.OnActivate();
			if (!Campaign.Current.ConversationManager.IsConversationFlowActive)
			{
				MenuContext menuContext = this.MenuContext;
				if (menuContext != null)
				{
					menuContext.Refresh();
				}
			}
			this.RefreshHandler();
		}

		// Token: 0x06003553 RID: 13651 RVA: 0x000D9D43 File Offset: 0x000D7F43
		public void EnterMenuMode()
		{
			this.MenuContext = MBObjectManager.Instance.CreateObject<MenuContext>();
			IMapStateHandler handler = this._handler;
			if (handler != null)
			{
				handler.OnEnteringMenuMode(this.MenuContext);
			}
			this.MenuContext.Refresh();
		}

		// Token: 0x06003554 RID: 13652 RVA: 0x000D9D77 File Offset: 0x000D7F77
		public void ExitMenuMode()
		{
			IMapStateHandler handler = this._handler;
			if (handler != null)
			{
				handler.OnExitingMenuMode();
			}
			this.MenuContext.Destroy();
			MBObjectManager.Instance.UnregisterObject(this.MenuContext);
			this.MenuContext = null;
			this.GameMenuId = null;
		}

		// Token: 0x06003555 RID: 13653 RVA: 0x000D9DB3 File Offset: 0x000D7FB3
		public void StartBattleSimulation()
		{
			this._battleSimulation = PlayerEncounter.Current.BattleSimulation;
			IMapStateHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnBattleSimulationStarted(this._battleSimulation);
		}

		// Token: 0x06003556 RID: 13654 RVA: 0x000D9DDB File Offset: 0x000D7FDB
		public void EndBattleSimulation()
		{
			this._battleSimulation = null;
			IMapStateHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnBattleSimulationEnded();
		}

		// Token: 0x06003557 RID: 13655 RVA: 0x000D9DF4 File Offset: 0x000D7FF4
		public void OnPlayerSiegeActivated()
		{
			IMapStateHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnPlayerSiegeActivated();
		}

		// Token: 0x06003558 RID: 13656 RVA: 0x000D9E06 File Offset: 0x000D8006
		public void OnPlayerSiegeDeactivated()
		{
			IMapStateHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnPlayerSiegeDeactivated();
		}

		// Token: 0x06003559 RID: 13657 RVA: 0x000D9E18 File Offset: 0x000D8018
		public void OnSiegeEngineClick(MatrixFrame siegeEngineFrame)
		{
			IMapStateHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnSiegeEngineClick(siegeEngineFrame);
		}

		// Token: 0x04000F34 RID: 3892
		private Incident _nextIncident;

		// Token: 0x04000F35 RID: 3893
		private MenuContext _menuContext;

		// Token: 0x04000F36 RID: 3894
		private bool _mapConversationActive;

		// Token: 0x04000F37 RID: 3895
		private bool _closeScreenNextFrame;

		// Token: 0x04000F38 RID: 3896
		private IMapStateHandler _handler;

		// Token: 0x04000F39 RID: 3897
		private BattleSimulation _battleSimulation;
	}
}
