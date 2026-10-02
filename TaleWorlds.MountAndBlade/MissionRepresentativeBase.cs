using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002CC RID: 716
	public abstract class MissionRepresentativeBase : PeerComponent
	{
		// Token: 0x170007CF RID: 1999
		// (get) Token: 0x0600296A RID: 10602 RVA: 0x0009BB3D File Offset: 0x00099D3D
		protected MissionRepresentativeBase.PlayerTypes PlayerType
		{
			get
			{
				if (!base.Peer.Communicator.IsNetworkActive)
				{
					return MissionRepresentativeBase.PlayerTypes.Bot;
				}
				if (!base.Peer.Communicator.IsServerPeer)
				{
					return MissionRepresentativeBase.PlayerTypes.Client;
				}
				return MissionRepresentativeBase.PlayerTypes.Server;
			}
		}

		// Token: 0x170007D0 RID: 2000
		// (get) Token: 0x0600296B RID: 10603 RVA: 0x0009BB68 File Offset: 0x00099D68
		// (set) Token: 0x0600296C RID: 10604 RVA: 0x0009BB70 File Offset: 0x00099D70
		public Agent ControlledAgent { get; private set; }

		// Token: 0x170007D1 RID: 2001
		// (get) Token: 0x0600296D RID: 10605 RVA: 0x0009BB7C File Offset: 0x00099D7C
		// (set) Token: 0x0600296E RID: 10606 RVA: 0x0009BBBC File Offset: 0x00099DBC
		public int Gold
		{
			get
			{
				if (this._gold < 0)
				{
					return this._gold;
				}
				bool flag;
				MultiplayerOptions.Instance.GetOptionFromOptionType(MultiplayerOptions.OptionType.UnlimitedGold, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions).GetValue(out flag);
				if (!flag)
				{
					return this._gold;
				}
				return 2000;
			}
			private set
			{
				if (value < 0)
				{
					this._gold = value;
					return;
				}
				bool flag;
				MultiplayerOptions.Instance.GetOptionFromOptionType(MultiplayerOptions.OptionType.UnlimitedGold, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions).GetValue(out flag);
				this._gold = ((!flag) ? value : 2000);
			}
		}

		// Token: 0x170007D2 RID: 2002
		// (get) Token: 0x0600296F RID: 10607 RVA: 0x0009BBFA File Offset: 0x00099DFA
		public MissionPeer MissionPeer
		{
			get
			{
				if (this._missionPeer == null)
				{
					this._missionPeer = base.GetComponent<MissionPeer>();
				}
				return this._missionPeer;
			}
		}

		// Token: 0x1400007D RID: 125
		// (add) Token: 0x06002970 RID: 10608 RVA: 0x0009BC18 File Offset: 0x00099E18
		// (remove) Token: 0x06002971 RID: 10609 RVA: 0x0009BC50 File Offset: 0x00099E50
		public event Action OnGoldUpdated;

		// Token: 0x06002973 RID: 10611 RVA: 0x0009BC8D File Offset: 0x00099E8D
		public void SetAgent(Agent agent)
		{
			this.ControlledAgent = agent;
			if (this.ControlledAgent != null)
			{
				this.ControlledAgent.SetMissionRepresentative(this);
				this.OnAgentSpawned();
			}
		}

		// Token: 0x06002974 RID: 10612 RVA: 0x0009BCB0 File Offset: 0x00099EB0
		public virtual void OnAgentSpawned()
		{
		}

		// Token: 0x06002975 RID: 10613 RVA: 0x0009BCB2 File Offset: 0x00099EB2
		public virtual void Tick(float dt)
		{
		}

		// Token: 0x06002976 RID: 10614 RVA: 0x0009BCB4 File Offset: 0x00099EB4
		public void UpdateGold(int gold)
		{
			this.Gold = gold;
			Action onGoldUpdated = this.OnGoldUpdated;
			if (onGoldUpdated == null)
			{
				return;
			}
			onGoldUpdated();
		}

		// Token: 0x04000FE3 RID: 4067
		private int _gold;

		// Token: 0x04000FE4 RID: 4068
		private MissionPeer _missionPeer;

		// Token: 0x020005B0 RID: 1456
		protected enum PlayerTypes
		{
			// Token: 0x04001EEB RID: 7915
			Bot,
			// Token: 0x04001EEC RID: 7916
			Client,
			// Token: 0x04001EED RID: 7917
			Server
		}
	}
}
