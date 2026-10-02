using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000043 RID: 67
	[Serializable]
	public class GetPublishedLobbyNewsMessageResult : FunctionResult
	{
		// Token: 0x1700006A RID: 106
		// (get) Token: 0x0600014D RID: 333 RVA: 0x00002F02 File Offset: 0x00001102
		// (set) Token: 0x0600014E RID: 334 RVA: 0x00002F0A File Offset: 0x0000110A
		[JsonProperty]
		public PublishedLobbyNewsArticle[] Content { get; private set; }

		// Token: 0x0600014F RID: 335 RVA: 0x00002F13 File Offset: 0x00001113
		public GetPublishedLobbyNewsMessageResult()
		{
		}

		// Token: 0x06000150 RID: 336 RVA: 0x00002F1B File Offset: 0x0000111B
		public GetPublishedLobbyNewsMessageResult(PublishedLobbyNewsArticle[] content)
		{
			this.Content = content;
		}
	}
}
