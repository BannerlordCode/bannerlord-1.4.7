using System;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x0200012D RID: 301
	public enum CustomGameJoinResponse
	{
		// Token: 0x04000337 RID: 823
		Success,
		// Token: 0x04000338 RID: 824
		IncorrectPlayerState,
		// Token: 0x04000339 RID: 825
		ServerCapacityIsFull,
		// Token: 0x0400033A RID: 826
		ErrorOnGameServer,
		// Token: 0x0400033B RID: 827
		GameServerAccessError,
		// Token: 0x0400033C RID: 828
		CustomGameServerNotAvailable,
		// Token: 0x0400033D RID: 829
		CustomGameServerFinishing,
		// Token: 0x0400033E RID: 830
		IncorrectPassword,
		// Token: 0x0400033F RID: 831
		PlayerBanned,
		// Token: 0x04000340 RID: 832
		HostReplyTimedOut,
		// Token: 0x04000341 RID: 833
		NoPlayerDataFound,
		// Token: 0x04000342 RID: 834
		UnspecifiedError,
		// Token: 0x04000343 RID: 835
		NoPlayersCanJoin,
		// Token: 0x04000344 RID: 836
		AlreadyRequestedWaitingForServerResponse,
		// Token: 0x04000345 RID: 837
		RequesterIsNotPartyLeader,
		// Token: 0x04000346 RID: 838
		NotAllPlayersReady,
		// Token: 0x04000347 RID: 839
		NotAllPlayersModulesMatchWithServer
	}
}
