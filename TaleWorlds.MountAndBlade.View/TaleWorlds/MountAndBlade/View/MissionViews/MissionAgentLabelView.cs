using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View.Tableaus.Thumbnails;

namespace TaleWorlds.MountAndBlade.View.MissionViews
{
	// Token: 0x0200006B RID: 107
	public class MissionAgentLabelView : MissionView
	{
		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060003FF RID: 1023 RVA: 0x0001E71E File Offset: 0x0001C91E
		// (set) Token: 0x06000400 RID: 1024 RVA: 0x0001E726 File Offset: 0x0001C926
		private bool IndicatorsActive
		{
			get
			{
				return this._indicatorsActive;
			}
			set
			{
				if (this._indicatorsActive != value)
				{
					this._indicatorsActive = value;
					this.UpdateAllAgentMeshVisibilities();
				}
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000401 RID: 1025 RVA: 0x0001E73E File Offset: 0x0001C93E
		private OrderController PlayerOrderController
		{
			get
			{
				Team playerTeam = base.Mission.PlayerTeam;
				if (playerTeam == null)
				{
					return null;
				}
				return playerTeam.PlayerOrderController;
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000402 RID: 1026 RVA: 0x0001E756 File Offset: 0x0001C956
		private SiegeWeaponController PlayerSiegeWeaponController
		{
			get
			{
				Team playerTeam = base.Mission.PlayerTeam;
				if (playerTeam == null)
				{
					return null;
				}
				return playerTeam.PlayerOrderController.SiegeWeaponController;
			}
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x0001E774 File Offset: 0x0001C974
		public MissionAgentLabelView()
		{
			this._agentMeshes = new Dictionary<Agent, MetaMesh>();
			this._labelMaterials = new Dictionary<Texture, Material>();
			this._closeAgentsWithMeshes = new List<Agent>();
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x0001E7C8 File Offset: 0x0001C9C8
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			base.Mission.Teams.OnPlayerTeamChanged += this.Mission_OnPlayerTeamChanged;
			base.Mission.OnMainAgentChanged += this.OnMainAgentChanged;
			ManagedOptions.OnManagedOptionChanged = (ManagedOptions.OnManagedOptionChangedDelegate)Delegate.Combine(ManagedOptions.OnManagedOptionChanged, new ManagedOptions.OnManagedOptionChangedDelegate(this.OnManagedOptionChanged));
			base.MissionScreen.OnSpectateAgentFocusIn += this.HandleSpectateAgentFocusIn;
			base.MissionScreen.OnSpectateAgentFocusOut += this.HandleSpectateAgentFocusOut;
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x0001E85C File Offset: 0x0001CA5C
		public override void AfterStart()
		{
			if (this.PlayerOrderController != null)
			{
				this.PlayerOrderController.OnSelectedFormationsChanged += this.OrderController_OnSelectedFormationsChanged;
				base.Mission.PlayerTeam.OnFormationsChanged += this.PlayerTeam_OnFormationsChanged;
			}
			BannerBearerLogic missionBehavior = base.Mission.GetMissionBehavior<BannerBearerLogic>();
			if (missionBehavior != null)
			{
				missionBehavior.OnBannerBearerAgentUpdated += this.BannerBearerLogic_OnBannerBearerAgentUpdated;
			}
			this.UpdateAlwaysShowFriendlyTroopBanners();
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x0001E8CC File Offset: 0x0001CACC
		public override void OnMissionTick(float dt)
		{
			bool isOrderFlagVisible = this._isOrderFlagVisible;
			this.UpdateIsOrderFlagVisible();
			if (!this._isOrderFlagVisible && isOrderFlagVisible)
			{
				this.UpdateAllAgentMeshVisibilities();
				this.SetHighlightForAgents(false, false, false);
				this.SetHighlightForAgents(false, true, false);
			}
			if (this._isOrderFlagVisible && !isOrderFlagVisible)
			{
				this.UpdateAllAgentMeshVisibilities();
				this.SetHighlightForAgents(true, false, false);
				this.SetHighlightForAgents(true, true, false);
			}
			this.UpdateProximityBannerTransparencies();
			this.IndicatorsActive = this._alwaysShowFriendlyTroopBanners || base.Input.IsGameKeyDown(5);
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x0001E954 File Offset: 0x0001CB54
		private void UpdateProximityBannerTransparencies()
		{
			for (int i = 0; i < this._closeAgentsWithMeshes.Count; i++)
			{
				Agent agent = this._closeAgentsWithMeshes[i];
				this.SetBannerHighlightVisibility(agent, this.IsAgentListeningToOrders(agent));
			}
			this._closeAgentsWithMeshes.Clear();
			AgentProximityMap.ProximityMapSearchStruct proximityMapSearchStruct = AgentProximityMap.BeginSearch(base.Mission, base.MissionScreen.CombatCamera.Position.AsVec2, 8f, false);
			while (proximityMapSearchStruct.LastFoundAgent != null)
			{
				if (this._agentMeshes.ContainsKey(proximityMapSearchStruct.LastFoundAgent))
				{
					this._closeAgentsWithMeshes.Add(proximityMapSearchStruct.LastFoundAgent);
				}
				AgentProximityMap.FindNext(base.Mission, ref proximityMapSearchStruct);
			}
			for (int j = 0; j < this._closeAgentsWithMeshes.Count; j++)
			{
				Agent agent2 = this._closeAgentsWithMeshes[j];
				this.SetBannerHighlightVisibility(agent2, this.IsAgentListeningToOrders(agent2));
			}
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x0001EA3E File Offset: 0x0001CC3E
		public override void OnRemoveBehavior()
		{
			this.UnregisterEvents();
			base.OnRemoveBehavior();
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x0001EA4C File Offset: 0x0001CC4C
		public override void OnMissionScreenFinalize()
		{
			this.UnregisterEvents();
			base.OnMissionScreenFinalize();
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x0001EA5C File Offset: 0x0001CC5C
		private void UnregisterEvents()
		{
			if (base.Mission != null)
			{
				base.Mission.Teams.OnPlayerTeamChanged -= this.Mission_OnPlayerTeamChanged;
				base.Mission.OnMainAgentChanged -= this.OnMainAgentChanged;
			}
			ManagedOptions.OnManagedOptionChanged = (ManagedOptions.OnManagedOptionChangedDelegate)Delegate.Remove(ManagedOptions.OnManagedOptionChanged, new ManagedOptions.OnManagedOptionChangedDelegate(this.OnManagedOptionChanged));
			if (base.MissionScreen != null)
			{
				base.MissionScreen.OnSpectateAgentFocusIn -= this.HandleSpectateAgentFocusIn;
				base.MissionScreen.OnSpectateAgentFocusOut -= this.HandleSpectateAgentFocusOut;
			}
			if (this.PlayerOrderController != null)
			{
				this.PlayerOrderController.OnSelectedFormationsChanged -= this.OrderController_OnSelectedFormationsChanged;
				if (base.Mission != null)
				{
					base.Mission.PlayerTeam.OnFormationsChanged -= this.PlayerTeam_OnFormationsChanged;
				}
			}
			BannerBearerLogic missionBehavior = base.Mission.GetMissionBehavior<BannerBearerLogic>();
			if (missionBehavior != null)
			{
				missionBehavior.OnBannerBearerAgentUpdated -= this.BannerBearerLogic_OnBannerBearerAgentUpdated;
			}
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x0001EB5E File Offset: 0x0001CD5E
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow killingBlow)
		{
			this.RemoveAgentLabel(affectedAgent);
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x0001EB67 File Offset: 0x0001CD67
		public override void OnAgentBuild(Agent agent, Banner banner)
		{
			this.InitAgentLabel(agent, banner);
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x0001EB71 File Offset: 0x0001CD71
		public override void OnAssignPlayerAsSergeantOfFormation(Agent agent)
		{
			this.SetBannerHighlightVisibility(agent, true);
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x0001EB7B File Offset: 0x0001CD7B
		public override void OnClearScene()
		{
			this._agentMeshes.Clear();
			this._labelMaterials.Clear();
			this._closeAgentsWithMeshes.Clear();
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x0001EB9E File Offset: 0x0001CD9E
		private void PlayerTeam_OnFormationsChanged(Team team, Formation formation)
		{
			this.UpdateIsOrderFlagVisible();
			if (this._isOrderFlagVisible)
			{
				this.DehighlightAllAgents();
				this.SetHighlightForAgents(true, false, false);
			}
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x0001EBC0 File Offset: 0x0001CDC0
		private void Mission_OnPlayerTeamChanged(Team previousTeam, Team currentTeam)
		{
			this.DehighlightAllAgents();
			this._isOrderFlagVisible = false;
			if (((previousTeam != null) ? previousTeam.PlayerOrderController : null) != null)
			{
				previousTeam.PlayerOrderController.OnSelectedFormationsChanged -= this.OrderController_OnSelectedFormationsChanged;
				previousTeam.PlayerOrderController.SiegeWeaponController.OnSelectedSiegeWeaponsChanged -= this.PlayerSiegeWeaponController_OnSelectedSiegeWeaponsChanged;
			}
			if (this.PlayerOrderController != null)
			{
				this.PlayerOrderController.OnSelectedFormationsChanged += this.OrderController_OnSelectedFormationsChanged;
				this.PlayerSiegeWeaponController.OnSelectedSiegeWeaponsChanged += this.PlayerSiegeWeaponController_OnSelectedSiegeWeaponsChanged;
			}
			this.SetHighlightForAgents(true, false, true);
			foreach (Agent agent in base.Mission.Agents)
			{
				this.UpdateVisibilityOfAgentMesh(agent);
			}
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x0001ECA8 File Offset: 0x0001CEA8
		private void OrderController_OnSelectedFormationsChanged()
		{
			this.UpdateAllAgentMeshVisibilities();
			this.DehighlightAllAgents();
			this.UpdateIsOrderFlagVisible();
			if (this._isOrderFlagVisible)
			{
				this.SetHighlightForAgents(true, false, false);
			}
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x0001ECCD File Offset: 0x0001CECD
		private void PlayerSiegeWeaponController_OnSelectedSiegeWeaponsChanged()
		{
			this.DehighlightAllAgents();
			this.SetHighlightForAgents(true, true, false);
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x0001ECDE File Offset: 0x0001CEDE
		private void BannerBearerLogic_OnBannerBearerAgentUpdated(Agent agent, bool isBannerBearer)
		{
			this.RemoveAgentLabel(agent);
			this.InitAgentLabel(agent, null);
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x0001ECF0 File Offset: 0x0001CEF0
		private void RemoveAgentLabel(Agent agent)
		{
			if (agent.IsHuman && this._agentMeshes.ContainsKey(agent))
			{
				if (agent.AgentVisuals != null)
				{
					agent.AgentVisuals.ReplaceMeshWithMesh(this._agentMeshes[agent], null, BodyMeshTypes.Label);
				}
				this._agentMeshes.Remove(agent);
			}
			if (this._closeAgentsWithMeshes.Contains(agent))
			{
				this._closeAgentsWithMeshes.Remove(agent);
			}
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x0001ED64 File Offset: 0x0001CF64
		private void InitAgentLabel(Agent agent, Banner peerBanner = null)
		{
			if (agent.IsHuman)
			{
				Banner banner = peerBanner ?? agent.Origin.Banner;
				if (banner != null)
				{
					MetaMesh copy = MetaMesh.GetCopy("troop_banner_selection", false, true);
					Material tableauMaterial = Material.GetFromResource("agent_label_with_tableau");
					Banner banner2 = banner;
					BannerDebugInfo bannerDebugInfo = BannerDebugInfo.CreateManual(base.GetType().Name);
					Texture texture = banner2.GetTableauTextureSmall(in bannerDebugInfo, null);
					if (copy != null && tableauMaterial != null)
					{
						Texture fromResource = Texture.GetFromResource("banner_top_of_head");
						Material material;
						if (this._labelMaterials.TryGetValue(texture ?? fromResource, out material))
						{
							tableauMaterial = material;
						}
						else
						{
							tableauMaterial = tableauMaterial.CreateCopy();
							Action<Texture> action = delegate(Texture tex)
							{
								tableauMaterial.SetTexture(Material.MBTextureType.DiffuseMap, tex);
							};
							Banner banner3 = banner;
							bannerDebugInfo = BannerDebugInfo.CreateManual(base.GetType().Name);
							texture = banner3.GetTableauTextureSmall(in bannerDebugInfo, action);
							tableauMaterial.SetTexture(Material.MBTextureType.DiffuseMap2, fromResource);
							this._labelMaterials.Add(texture, tableauMaterial);
						}
						copy.SetMaterial(tableauMaterial);
						copy.SetVectorArgument(0.5f, 0.5f, 0.25f, 0.25f);
						agent.AgentVisuals.AddMultiMesh(copy, BodyMeshTypes.Label);
						this._agentMeshes.Add(agent, copy);
						this.UpdateVisibilityOfAgentMesh(agent);
						this.SetBannerHighlightVisibility(agent, false);
					}
				}
			}
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x0001EECC File Offset: 0x0001D0CC
		private void UpdateVisibilityOfAgentMesh(Agent agent)
		{
			if (agent.IsActive() && this._agentMeshes.ContainsKey(agent))
			{
				bool flag = this.IsMeshVisibleForAgent(agent);
				this._agentMeshes[agent].SetVisibilityMask(flag ? VisibilityMaskFlags.Final : ((VisibilityMaskFlags)0U));
			}
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x0001EF10 File Offset: 0x0001D110
		private bool IsMeshVisibleForAgent(Agent agent)
		{
			return (this._isResumingView || (!base.IsViewSuspended && !this._isSuspendingView)) && this.IsAllyInAllyTeam(agent) && base.MissionScreen.LastFollowedAgent != agent && BannerlordConfig.FriendlyTroopsBannerOpacity > 0f && !base.MissionScreen.IsPhotoModeEnabled && (this.IndicatorsActive || base.Mission.Mode == MissionMode.Deployment || this.IsAgentListeningToOrders(agent));
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x0001EF86 File Offset: 0x0001D186
		public override void OnMissionModeChange(MissionMode oldMissionMode, bool atStart)
		{
			base.OnMissionModeChange(oldMissionMode, atStart);
			this.UpdateAllAgentMeshVisibilities();
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x0001EF96 File Offset: 0x0001D196
		private void OnUpdateOpacityValueOfAgentMesh(Agent agent)
		{
			if (agent.IsActive() && this._agentMeshes.ContainsKey(agent))
			{
				this.SetBannerHighlightVisibility(agent, this.IsAgentListeningToOrders(agent));
			}
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x0001EFBC File Offset: 0x0001D1BC
		private bool IsAllyInAllyTeam(Agent agent)
		{
			if (((agent != null) ? agent.Team : null) != null && base.Mission != null && agent != base.Mission.MainAgent)
			{
				Team team = null;
				Team team3;
				if (GameNetwork.IsSessionActive)
				{
					Team team2;
					if (!GameNetwork.IsMyPeerReady)
					{
						team2 = null;
					}
					else
					{
						NetworkCommunicator myPeer = GameNetwork.MyPeer;
						if (myPeer == null)
						{
							team2 = null;
						}
						else
						{
							MissionPeer component = myPeer.GetComponent<MissionPeer>();
							team2 = ((component != null) ? component.Team : null);
						}
					}
					team3 = team2;
				}
				else
				{
					team3 = base.Mission.PlayerTeam;
					team = base.Mission.PlayerAllyTeam;
				}
				return agent.Team == team3 || agent.Team == team;
			}
			return false;
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x0001F04E File Offset: 0x0001D24E
		private void OnMainAgentChanged(Agent oldAgent)
		{
			this.UpdateAllAgentMeshVisibilities();
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x0001F056 File Offset: 0x0001D256
		private void HandleSpectateAgentFocusIn(Agent agent)
		{
			this.UpdateVisibilityOfAgentMesh(agent);
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x0001F05F File Offset: 0x0001D25F
		private void HandleSpectateAgentFocusOut(Agent agent)
		{
			this.UpdateVisibilityOfAgentMesh(agent);
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x0001F068 File Offset: 0x0001D268
		private void OnManagedOptionChanged(ManagedOptions.ManagedOptionsType optionType)
		{
			if (optionType == ManagedOptions.ManagedOptionsType.AlwaysShowFriendlyTroopBannersType)
			{
				this.UpdateAlwaysShowFriendlyTroopBanners();
				this.UpdateAllAgentMeshVisibilities();
			}
			if (optionType == ManagedOptions.ManagedOptionsType.FriendlyTroopsBannerOpacity || optionType == ManagedOptions.ManagedOptionsType.AlwaysShowFriendlyTroopBannersType)
			{
				this.UpdateAllAgentMeshVisibilities();
			}
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x0001F08C File Offset: 0x0001D28C
		private void UpdateAlwaysShowFriendlyTroopBanners()
		{
			float config = ManagedOptions.GetConfig(ManagedOptions.ManagedOptionsType.AlwaysShowFriendlyTroopBannersType);
			this._alwaysShowFriendlyTroopBanners = config == 2f || (config == 1f && GameNetwork.IsMultiplayer);
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x0001F0C4 File Offset: 0x0001D2C4
		private void UpdateAllAgentMeshVisibilities()
		{
			foreach (Agent agent in base.Mission.Agents)
			{
				if (agent.IsHuman)
				{
					this.UpdateVisibilityOfAgentMesh(agent);
					if (this.IsMeshVisibleForAgent(agent))
					{
						this.OnUpdateOpacityValueOfAgentMesh(agent);
					}
				}
			}
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x0001F134 File Offset: 0x0001D334
		private bool IsAgentListeningToOrders(Agent agent)
		{
			this.UpdateIsOrderFlagVisible();
			if (!this._isOrderFlagVisible)
			{
				return false;
			}
			if (this.PlayerOrderController != null && agent.Formation != null && this.PlayerOrderController.IsFormationListening(agent.Formation))
			{
				return true;
			}
			if (this.PlayerSiegeWeaponController != null && agent.IsUsingGameObject)
			{
				UsableMissionObject currentlyUsedGameObject = agent.CurrentlyUsedGameObject;
				for (int i = 0; i < this.PlayerSiegeWeaponController.SelectedWeapons.Count; i++)
				{
					SiegeWeapon siegeWeapon = this.PlayerSiegeWeaponController.SelectedWeapons[i];
					for (int j = 0; j < siegeWeapon.StandingPoints.Count; j++)
					{
						if (currentlyUsedGameObject == siegeWeapon.StandingPoints[j])
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x0001F1E4 File Offset: 0x0001D3E4
		private void SetBannerHighlightVisibility(Agent agent, bool highlightVisibility)
		{
			MetaMesh metaMesh;
			if (!this._agentMeshes.TryGetValue(agent, out metaMesh))
			{
				Debug.FailedAssert("Trying to update the banner of an agent that isn't present in _agentMeshes!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.View\\MissionViews\\MissionAgentLabelView.cs", "SetBannerHighlightVisibility", 499);
				return;
			}
			float num = (highlightVisibility ? 1f : (-1f));
			float num2 = (agent.Position + this._meshOffset).Distance(base.MissionScreen.CombatCamera.Position);
			if (num2 < 1.5f)
			{
				num = 0f;
			}
			else if (num2 < 8f)
			{
				num *= (num2 - 1.5f) / 6.5f;
			}
			metaMesh.SetVectorArgument2(20f, 0.4f, 0.44f, num * BannerlordConfig.FriendlyTroopsBannerOpacity);
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x0001F299 File Offset: 0x0001D499
		private void UpdateIsOrderFlagVisible()
		{
			this._isOrderFlagVisible = this.PlayerOrderController != null && base.MissionScreen.OrderFlag != null && base.MissionScreen.OrderFlag.IsVisible;
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x0001F2CC File Offset: 0x0001D4CC
		private void SetHighlightForAgents(bool highlight, bool useSiegeMachineUsers, bool useAllTeamAgents)
		{
			if (this.PlayerOrderController == null)
			{
				bool flag = base.Mission.PlayerTeam == null;
				Debug.Print(string.Format("PlayerOrderController is null and playerTeamIsNull: {0}", flag), 0, Debug.DebugColor.White, 17179869184UL);
			}
			if (useSiegeMachineUsers)
			{
				using (List<SiegeWeapon>.Enumerator enumerator = this.PlayerSiegeWeaponController.SelectedWeapons.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						SiegeWeapon siegeWeapon = enumerator.Current;
						foreach (StandingPoint standingPoint in siegeWeapon.StandingPoints)
						{
							Agent userAgent = standingPoint.UserAgent;
							if (userAgent != null)
							{
								this.SetBannerHighlightVisibility(userAgent, highlight);
							}
						}
					}
					return;
				}
			}
			if (useAllTeamAgents)
			{
				if (this.PlayerOrderController.Owner != null)
				{
					Team team = this.PlayerOrderController.Owner.Team;
					if (team == null)
					{
						Debug.Print("PlayerOrderController.Owner.Team is null, overriding with Mission.Current.PlayerTeam", 0, Debug.DebugColor.White, 17179869184UL);
						team = Mission.Current.PlayerTeam;
					}
					using (List<Agent>.Enumerator enumerator3 = team.ActiveAgents.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							Agent agent2 = enumerator3.Current;
							this.SetBannerHighlightVisibility(agent2, highlight);
						}
						return;
					}
				}
				Debug.Print("PlayerOrderController.Owner is null", 0, Debug.DebugColor.White, 17179869184UL);
				return;
			}
			Action<Agent> <>9__0;
			foreach (Formation formation in this.PlayerOrderController.SelectedFormations)
			{
				Action<Agent> action;
				if ((action = <>9__0) == null)
				{
					action = (<>9__0 = delegate(Agent agent)
					{
						this.SetBannerHighlightVisibility(agent, highlight);
					});
				}
				formation.ApplyActionOnEachUnit(action, null);
			}
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x0001F4D8 File Offset: 0x0001D6D8
		private void DehighlightAllAgents()
		{
			foreach (KeyValuePair<Agent, MetaMesh> keyValuePair in this._agentMeshes)
			{
				this.SetBannerHighlightVisibility(keyValuePair.Key, false);
			}
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x0001F534 File Offset: 0x0001D734
		public override void OnAgentTeamChanged(Team prevTeam, Team newTeam, Agent agent)
		{
			this.UpdateVisibilityOfAgentMesh(agent);
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x0001F53D File Offset: 0x0001D73D
		public override void OnPhotoModeActivated()
		{
			base.OnPhotoModeActivated();
			this.UpdateAllAgentMeshVisibilities();
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x0001F54B File Offset: 0x0001D74B
		public override void OnPhotoModeDeactivated()
		{
			base.OnPhotoModeDeactivated();
			this.UpdateAllAgentMeshVisibilities();
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x0001F559 File Offset: 0x0001D759
		protected override void OnSuspendView()
		{
			base.OnSuspendView();
			this._isSuspendingView = true;
			this.UpdateAllAgentMeshVisibilities();
			this._isSuspendingView = false;
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x0001F575 File Offset: 0x0001D775
		protected override void OnResumeView()
		{
			base.OnResumeView();
			this._isResumingView = true;
			this.UpdateAllAgentMeshVisibilities();
			this._isResumingView = false;
		}

		// Token: 0x04000265 RID: 613
		private const float _highlightedLabelScaleFactor = 20f;

		// Token: 0x04000266 RID: 614
		private const float _labelBannerWidth = 0.4f;

		// Token: 0x04000267 RID: 615
		private const float _labelBlackBorderWidth = 0.44f;

		// Token: 0x04000268 RID: 616
		private readonly Vec3 _meshOffset = new Vec3(0f, 0f, 2f, -1f);

		// Token: 0x04000269 RID: 617
		private const float _nearDistance = 1.5f;

		// Token: 0x0400026A RID: 618
		private const float _farDistance = 8f;

		// Token: 0x0400026B RID: 619
		private readonly List<Agent> _closeAgentsWithMeshes;

		// Token: 0x0400026C RID: 620
		private readonly Dictionary<Agent, MetaMesh> _agentMeshes;

		// Token: 0x0400026D RID: 621
		private readonly Dictionary<Texture, Material> _labelMaterials;

		// Token: 0x0400026E RID: 622
		private bool _isSuspendingView;

		// Token: 0x0400026F RID: 623
		private bool _isResumingView;

		// Token: 0x04000270 RID: 624
		private bool _isOrderFlagVisible;

		// Token: 0x04000271 RID: 625
		private bool _alwaysShowFriendlyTroopBanners;

		// Token: 0x04000272 RID: 626
		private bool _indicatorsActive;
	}
}
