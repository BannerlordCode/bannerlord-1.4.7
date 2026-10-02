using System;
using Newtonsoft.Json;
using TaleWorlds.Core;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000C5 RID: 197
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class UpdateCharacterMessage : Message
	{
		// Token: 0x17000117 RID: 279
		// (get) Token: 0x0600038D RID: 909 RVA: 0x000046DE File Offset: 0x000028DE
		// (set) Token: 0x0600038E RID: 910 RVA: 0x000046E6 File Offset: 0x000028E6
		[JsonProperty]
		public BodyProperties BodyProperties { get; private set; }

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x0600038F RID: 911 RVA: 0x000046EF File Offset: 0x000028EF
		// (set) Token: 0x06000390 RID: 912 RVA: 0x000046F7 File Offset: 0x000028F7
		[JsonProperty]
		public bool IsFemale { get; private set; }

		// Token: 0x06000391 RID: 913 RVA: 0x00004700 File Offset: 0x00002900
		public UpdateCharacterMessage()
		{
		}

		// Token: 0x06000392 RID: 914 RVA: 0x00004708 File Offset: 0x00002908
		public UpdateCharacterMessage(BodyProperties bodyProperties, bool isFemale)
		{
			this.BodyProperties = bodyProperties;
			this.IsFemale = isFemale;
		}
	}
}
