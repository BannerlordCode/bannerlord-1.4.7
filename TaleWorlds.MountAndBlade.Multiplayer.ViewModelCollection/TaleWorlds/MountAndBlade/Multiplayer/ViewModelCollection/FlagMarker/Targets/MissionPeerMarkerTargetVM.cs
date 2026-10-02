using System;
using System.Linq;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Diamond;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.FlagMarker.Targets
{
	// Token: 0x0200009B RID: 155
	public class MissionPeerMarkerTargetVM : MissionMarkerTargetVM
	{
		// Token: 0x17000506 RID: 1286
		// (get) Token: 0x06000F1A RID: 3866 RVA: 0x0002E987 File Offset: 0x0002CB87
		// (set) Token: 0x06000F1B RID: 3867 RVA: 0x0002E98F File Offset: 0x0002CB8F
		public MissionPeer TargetPeer { get; private set; }

		// Token: 0x17000507 RID: 1287
		// (get) Token: 0x06000F1C RID: 3868 RVA: 0x0002E998 File Offset: 0x0002CB98
		public override Vec3 WorldPosition
		{
			get
			{
				MissionPeer targetPeer = this.TargetPeer;
				if (((targetPeer != null) ? targetPeer.ControlledAgent : null) != null)
				{
					return this.TargetPeer.ControlledAgent.Position + new Vec3(0f, 0f, this.TargetPeer.ControlledAgent.GetEyeGlobalHeight(), -1f);
				}
				Debug.FailedAssert("No target found!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection\\FlagMarker\\Targets\\MissionPeerMarkerTargetVM.cs", "WorldPosition", 27);
				return Vec3.One;
			}
		}

		// Token: 0x17000508 RID: 1288
		// (get) Token: 0x06000F1D RID: 3869 RVA: 0x0002EA0E File Offset: 0x0002CC0E
		protected override float HeightOffset
		{
			get
			{
				return 0.75f;
			}
		}

		// Token: 0x06000F1E RID: 3870 RVA: 0x0002EA15 File Offset: 0x0002CC15
		public MissionPeerMarkerTargetVM(MissionPeer peer, bool isFriend)
			: base(MissionMarkerType.Peer)
		{
			this.TargetPeer = peer;
			this._isFriend = isFriend;
			base.Name = peer.DisplayedName;
			this.SetVisual();
		}

		// Token: 0x06000F1F RID: 3871 RVA: 0x0002EA40 File Offset: 0x0002CC40
		private void SetVisual()
		{
			string text = "#FFFFFFFF";
			if (NetworkMain.GameClient.IsInParty && NetworkMain.GameClient.PlayersInParty.Any<PartyPlayerInLobbyClient>((PartyPlayerInLobbyClient p) => p.PlayerId.Equals(this.TargetPeer.Peer.Id)))
			{
				text = "#00FF00FF";
			}
			else if (this._isFriend)
			{
				text = "#FFFF00FF";
			}
			else if (NetworkMain.GameClient.IsInClan && NetworkMain.GameClient.PlayersInClan.Any<ClanPlayer>((ClanPlayer p) => p.PlayerId.Equals(this.TargetPeer.Peer.Id)))
			{
				text = "#00FFFFFF";
			}
			uint num = TaleWorlds.Library.Color.ConvertStringToColor("#FFFFFFFF").ToUnsignedInteger();
			uint num2 = TaleWorlds.Library.Color.ConvertStringToColor(text).ToUnsignedInteger();
			base.RefreshColor(num, num2);
		}

		// Token: 0x06000F20 RID: 3872 RVA: 0x0002EAED File Offset: 0x0002CCED
		public override void UpdateScreenPosition(Camera missionCamera)
		{
			MissionPeer targetPeer = this.TargetPeer;
			if (((targetPeer != null) ? targetPeer.ControlledAgent : null) == null)
			{
				return;
			}
			base.UpdateScreenPosition(missionCamera);
		}

		// Token: 0x040006F8 RID: 1784
		private const string _partyMemberColor = "#00FF00FF";

		// Token: 0x040006F9 RID: 1785
		private const string _friendColor = "#FFFF00FF";

		// Token: 0x040006FA RID: 1786
		private const string _clanMemberColor = "#00FFFFFF";

		// Token: 0x040006FB RID: 1787
		private bool _isFriend;
	}
}
