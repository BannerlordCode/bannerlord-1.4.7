using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002A6 RID: 678
	public class MissionCommunityClientComponent : MissionLobbyComponent
	{
		// Token: 0x06002564 RID: 9572 RVA: 0x000873C0 File Offset: 0x000855C0
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			this._communityClient = NetworkMain.CommunityClient;
		}

		// Token: 0x06002565 RID: 9573 RVA: 0x000873D3 File Offset: 0x000855D3
		public void SetServerEndingBeforeClientLoaded(bool isServerEndingBeforeClientLoaded)
		{
			this._isServerEndedBeforeClientLoaded = isServerEndingBeforeClientLoaded;
		}

		// Token: 0x06002566 RID: 9574 RVA: 0x000873DC File Offset: 0x000855DC
		public override void QuitMission()
		{
			base.QuitMission();
			if (!this._isServerEndedBeforeClientLoaded && base.CurrentMultiplayerState != MissionLobbyComponent.MultiplayerGameState.Ending && this._communityClient.IsInGame)
			{
				this._communityClient.QuitFromGame();
			}
		}

		// Token: 0x04000E64 RID: 3684
		private CommunityClient _communityClient;

		// Token: 0x04000E65 RID: 3685
		private bool _isServerEndedBeforeClientLoaded;
	}
}
