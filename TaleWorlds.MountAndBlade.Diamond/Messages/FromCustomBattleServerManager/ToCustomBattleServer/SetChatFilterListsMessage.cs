using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromCustomBattleServerManager.ToCustomBattleServer
{
	// Token: 0x02000014 RID: 20
	[MessageDescription("CustomBattleServerManager", "CustomBattleServer", true)]
	[DataContract]
	[Serializable]
	public class SetChatFilterListsMessage : Message
	{
		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000084 RID: 132 RVA: 0x000026F0 File Offset: 0x000008F0
		// (set) Token: 0x06000085 RID: 133 RVA: 0x000026F8 File Offset: 0x000008F8
		[JsonProperty]
		public string[] ProfanityList { get; private set; }

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000086 RID: 134 RVA: 0x00002701 File Offset: 0x00000901
		// (set) Token: 0x06000087 RID: 135 RVA: 0x00002709 File Offset: 0x00000909
		[JsonProperty]
		public string[] AllowList { get; private set; }

		// Token: 0x06000088 RID: 136 RVA: 0x00002712 File Offset: 0x00000912
		public SetChatFilterListsMessage()
		{
		}

		// Token: 0x06000089 RID: 137 RVA: 0x0000271A File Offset: 0x0000091A
		public SetChatFilterListsMessage(string[] profanityList, string[] allowList)
		{
			this.ProfanityList = profanityList;
			this.AllowList = allowList;
		}
	}
}
