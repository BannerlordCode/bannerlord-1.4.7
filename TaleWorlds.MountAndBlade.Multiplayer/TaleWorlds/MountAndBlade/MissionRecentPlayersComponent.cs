using System;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000015 RID: 21
	public class MissionRecentPlayersComponent : MissionNetwork
	{
		// Token: 0x0600014F RID: 335 RVA: 0x00005800 File Offset: 0x00003A00
		public override void AfterStart()
		{
			base.AfterStart();
			MissionPeer.OnTeamChanged += this.TeamChange;
			MissionPeer.OnPlayerKilled += this.OnPlayerKilled;
			this._myId = NetworkMain.GameClient.PlayerID;
		}

		// Token: 0x06000150 RID: 336 RVA: 0x0000583A File Offset: 0x00003A3A
		private void TeamChange(NetworkCommunicator player, Team oldTeam, Team nextTeam)
		{
			if (player.VirtualPlayer.Id != this._myId)
			{
				RecentPlayersManager.AddOrUpdatePlayerEntry(player.VirtualPlayer.Id, player.UserName, InteractionType.InGameTogether, player.ForcedAvatarIndex);
			}
		}

		// Token: 0x06000151 RID: 337 RVA: 0x00005874 File Offset: 0x00003A74
		private void OnPlayerKilled(MissionPeer killerPeer, MissionPeer killedPeer)
		{
			if (killerPeer != null && killedPeer != null && killerPeer.Peer != null && killedPeer.Peer != null)
			{
				PlayerId id = killerPeer.Peer.Id;
				PlayerId id2 = killedPeer.Peer.Id;
				if (id == this._myId && id2 != this._myId)
				{
					RecentPlayersManager.AddOrUpdatePlayerEntry(id2, killedPeer.Name, InteractionType.Killed, killedPeer.GetNetworkPeer().ForcedAvatarIndex);
					return;
				}
				if (id2 == this._myId && id != this._myId)
				{
					RecentPlayersManager.AddOrUpdatePlayerEntry(id, killerPeer.Name, InteractionType.KilledBy, killerPeer.GetNetworkPeer().ForcedAvatarIndex);
				}
			}
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00005924 File Offset: 0x00003B24
		public override void OnRemoveBehavior()
		{
			MissionPeer.OnTeamChanged -= this.TeamChange;
			MissionPeer.OnPlayerKilled -= this.OnPlayerKilled;
			base.OnRemoveBehavior();
		}

		// Token: 0x0400003A RID: 58
		private PlayerId _myId;
	}
}
