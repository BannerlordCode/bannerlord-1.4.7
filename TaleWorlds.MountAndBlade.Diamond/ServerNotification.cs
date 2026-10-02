using System;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x0200015A RID: 346
	[Serializable]
	public class ServerNotification
	{
		// Token: 0x1700030F RID: 783
		// (get) Token: 0x0600099D RID: 2461 RVA: 0x0000EE67 File Offset: 0x0000D067
		public ServerNotificationType Type { get; }

		// Token: 0x17000310 RID: 784
		// (get) Token: 0x0600099E RID: 2462 RVA: 0x0000EE6F File Offset: 0x0000D06F
		public string Message { get; }

		// Token: 0x0600099F RID: 2463 RVA: 0x0000EE77 File Offset: 0x0000D077
		public ServerNotification(ServerNotificationType type, string message)
		{
			this.Type = type;
			this.Message = message;
		}

		// Token: 0x060009A0 RID: 2464 RVA: 0x0000EE90 File Offset: 0x0000D090
		public TextObject GetTextObjectOfMessage()
		{
			TextObject textObject;
			if (!GameTexts.TryGetText(this.Message, out textObject, null))
			{
				textObject = new TextObject("{=!}" + this.Message, null);
			}
			return textObject;
		}
	}
}
