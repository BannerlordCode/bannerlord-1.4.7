using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade.MissionRepresentatives;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002B0 RID: 688
	public class MissionMultiplayerGameModeDuelClient : MissionMultiplayerGameModeBaseClient
	{
		// Token: 0x1700076C RID: 1900
		// (get) Token: 0x060026AC RID: 9900 RVA: 0x0008EBCE File Offset: 0x0008CDCE
		public override bool IsGameModeUsingGold
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700076D RID: 1901
		// (get) Token: 0x060026AD RID: 9901 RVA: 0x0008EBD1 File Offset: 0x0008CDD1
		public override bool IsGameModeTactical
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700076E RID: 1902
		// (get) Token: 0x060026AE RID: 9902 RVA: 0x0008EBD4 File Offset: 0x0008CDD4
		public override bool IsGameModeUsingRoundCountdown
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700076F RID: 1903
		// (get) Token: 0x060026AF RID: 9903 RVA: 0x0008EBD7 File Offset: 0x0008CDD7
		public override bool IsGameModeUsingAllowCultureChange
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000770 RID: 1904
		// (get) Token: 0x060026B0 RID: 9904 RVA: 0x0008EBDA File Offset: 0x0008CDDA
		public override bool IsGameModeUsingAllowTroopChange
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000771 RID: 1905
		// (get) Token: 0x060026B1 RID: 9905 RVA: 0x0008EBDD File Offset: 0x0008CDDD
		public override MultiplayerGameType GameType
		{
			get
			{
				return MultiplayerGameType.Duel;
			}
		}

		// Token: 0x17000772 RID: 1906
		// (get) Token: 0x060026B2 RID: 9906 RVA: 0x0008EBE0 File Offset: 0x0008CDE0
		public bool IsInDuel
		{
			get
			{
				MissionPeer component = GameNetwork.MyPeer.GetComponent<MissionPeer>();
				bool? flag;
				if (component == null)
				{
					flag = null;
				}
				else
				{
					Team team = component.Team;
					flag = ((team != null) ? new bool?(team.IsDefender) : null);
				}
				return flag ?? false;
			}
		}

		// Token: 0x17000773 RID: 1907
		// (get) Token: 0x060026B3 RID: 9907 RVA: 0x0008EC37 File Offset: 0x0008CE37
		// (set) Token: 0x060026B4 RID: 9908 RVA: 0x0008EC3F File Offset: 0x0008CE3F
		public DuelMissionRepresentative MyRepresentative { get; private set; }

		// Token: 0x060026B5 RID: 9909 RVA: 0x0008EC48 File Offset: 0x0008CE48
		private void OnMyClientSynchronized()
		{
			this.MyRepresentative = GameNetwork.MyPeer.GetComponent<DuelMissionRepresentative>();
			Action onMyRepresentativeAssigned = this.OnMyRepresentativeAssigned;
			if (onMyRepresentativeAssigned != null)
			{
				onMyRepresentativeAssigned();
			}
			this.MyRepresentative.AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode.Add);
		}

		// Token: 0x060026B6 RID: 9910 RVA: 0x0008EC77 File Offset: 0x0008CE77
		public override int GetGoldAmount()
		{
			return 0;
		}

		// Token: 0x060026B7 RID: 9911 RVA: 0x0008EC7A File Offset: 0x0008CE7A
		public override void OnGoldAmountChangedForRepresentative(MissionRepresentativeBase representative, int goldAmount)
		{
		}

		// Token: 0x060026B8 RID: 9912 RVA: 0x0008EC7C File Offset: 0x0008CE7C
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			base.MissionNetworkComponent.OnMyClientSynchronized += this.OnMyClientSynchronized;
		}

		// Token: 0x060026B9 RID: 9913 RVA: 0x0008EC9B File Offset: 0x0008CE9B
		public override void OnRemoveBehavior()
		{
			base.OnRemoveBehavior();
			base.MissionNetworkComponent.OnMyClientSynchronized -= this.OnMyClientSynchronized;
			if (this.MyRepresentative != null)
			{
				this.MyRepresentative.AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode.Remove);
			}
		}

		// Token: 0x060026BA RID: 9914 RVA: 0x0008ECCE File Offset: 0x0008CECE
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow blow)
		{
			base.OnAgentRemoved(affectedAgent, affectorAgent, agentState, blow);
			if (this.MyRepresentative != null)
			{
				this.MyRepresentative.CheckHasRequestFromAndRemoveRequestIfNeeded(affectedAgent.MissionPeer);
			}
		}

		// Token: 0x060026BB RID: 9915 RVA: 0x0008ECF8 File Offset: 0x0008CEF8
		public override bool CanRequestCultureChange()
		{
			NetworkCommunicator myPeer = GameNetwork.MyPeer;
			MissionPeer missionPeer = ((myPeer != null) ? myPeer.GetComponent<MissionPeer>() : null);
			return ((missionPeer != null) ? missionPeer.Team : null) != null && missionPeer.Team.IsAttacker;
		}

		// Token: 0x060026BC RID: 9916 RVA: 0x0008ED34 File Offset: 0x0008CF34
		public override bool CanRequestTroopChange()
		{
			NetworkCommunicator myPeer = GameNetwork.MyPeer;
			MissionPeer missionPeer = ((myPeer != null) ? myPeer.GetComponent<MissionPeer>() : null);
			return ((missionPeer != null) ? missionPeer.Team : null) != null && missionPeer.Team.IsAttacker;
		}

		// Token: 0x04000EAA RID: 3754
		public Action OnMyRepresentativeAssigned;
	}
}
