using System;
using TaleWorlds.MountAndBlade.Diamond;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002E5 RID: 741
	public interface ICommunityClientHandler
	{
		// Token: 0x06002ACF RID: 10959
		void OnJoinCustomGameResponse(string address, int port, PlayerJoinGameResponseDataFromHost response);

		// Token: 0x06002AD0 RID: 10960
		void OnQuitFromGame();
	}
}
