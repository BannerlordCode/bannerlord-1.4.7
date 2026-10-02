using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x02000076 RID: 118
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class AddFriendByUsernameAndIdMessage : Message
	{
		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x0600024A RID: 586 RVA: 0x000039A1 File Offset: 0x00001BA1
		// (set) Token: 0x0600024B RID: 587 RVA: 0x000039A9 File Offset: 0x00001BA9
		[JsonProperty]
		public string Username { get; private set; }

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x0600024C RID: 588 RVA: 0x000039B2 File Offset: 0x00001BB2
		// (set) Token: 0x0600024D RID: 589 RVA: 0x000039BA File Offset: 0x00001BBA
		[JsonProperty]
		public int UserId { get; private set; }

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x0600024E RID: 590 RVA: 0x000039C3 File Offset: 0x00001BC3
		// (set) Token: 0x0600024F RID: 591 RVA: 0x000039CB File Offset: 0x00001BCB
		[JsonProperty]
		public bool DontUseNameForUnknownPlayer { get; private set; }

		// Token: 0x06000250 RID: 592 RVA: 0x000039D4 File Offset: 0x00001BD4
		public AddFriendByUsernameAndIdMessage()
		{
		}

		// Token: 0x06000251 RID: 593 RVA: 0x000039DC File Offset: 0x00001BDC
		public AddFriendByUsernameAndIdMessage(string username, int userId, bool dontUseNameForUnknownPlayer)
		{
			this.Username = username;
			this.UserId = userId;
			this.DontUseNameForUnknownPlayer = dontUseNameForUnknownPlayer;
		}
	}
}
