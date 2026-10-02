using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000130 RID: 304
	[Serializable]
	public class LobbyNotification
	{
		// Token: 0x17000289 RID: 649
		// (get) Token: 0x0600082B RID: 2091 RVA: 0x0000BCA7 File Offset: 0x00009EA7
		// (set) Token: 0x0600082C RID: 2092 RVA: 0x0000BCAF File Offset: 0x00009EAF
		public int Id { get; set; }

		// Token: 0x1700028A RID: 650
		// (get) Token: 0x0600082D RID: 2093 RVA: 0x0000BCB8 File Offset: 0x00009EB8
		// (set) Token: 0x0600082E RID: 2094 RVA: 0x0000BCC0 File Offset: 0x00009EC0
		public NotificationType Type { get; set; }

		// Token: 0x1700028B RID: 651
		// (get) Token: 0x0600082F RID: 2095 RVA: 0x0000BCC9 File Offset: 0x00009EC9
		// (set) Token: 0x06000830 RID: 2096 RVA: 0x0000BCD1 File Offset: 0x00009ED1
		public DateTime Date { get; set; }

		// Token: 0x1700028C RID: 652
		// (get) Token: 0x06000831 RID: 2097 RVA: 0x0000BCDA File Offset: 0x00009EDA
		// (set) Token: 0x06000832 RID: 2098 RVA: 0x0000BCE2 File Offset: 0x00009EE2
		public string Message { get; set; }

		// Token: 0x1700028D RID: 653
		// (get) Token: 0x06000833 RID: 2099 RVA: 0x0000BCEB File Offset: 0x00009EEB
		// (set) Token: 0x06000834 RID: 2100 RVA: 0x0000BCF3 File Offset: 0x00009EF3
		public Dictionary<string, string> Parameters { get; set; }

		// Token: 0x06000835 RID: 2101 RVA: 0x0000BCFC File Offset: 0x00009EFC
		public LobbyNotification()
		{
		}

		// Token: 0x06000836 RID: 2102 RVA: 0x0000BD04 File Offset: 0x00009F04
		public LobbyNotification(NotificationType type, DateTime date, string message)
		{
			this.Id = -1;
			this.Type = type;
			this.Date = date;
			this.Message = message;
			this.Parameters = new Dictionary<string, string>();
		}

		// Token: 0x06000837 RID: 2103 RVA: 0x0000BD34 File Offset: 0x00009F34
		public LobbyNotification(int id, NotificationType type, DateTime date, string message, string serializedParameters)
		{
			this.Id = id;
			this.Type = type;
			this.Date = date;
			this.Message = message;
			try
			{
				this.Parameters = JsonConvert.DeserializeObject<Dictionary<string, string>>(serializedParameters);
			}
			catch (Exception)
			{
				this.Parameters = new Dictionary<string, string>();
			}
		}

		// Token: 0x06000838 RID: 2104 RVA: 0x0000BD94 File Offset: 0x00009F94
		public string GetParametersAsString()
		{
			string text = "{}";
			try
			{
				text = JsonConvert.SerializeObject(this.Parameters, Formatting.None);
			}
			catch (Exception)
			{
			}
			return text;
		}

		// Token: 0x06000839 RID: 2105 RVA: 0x0000BDCC File Offset: 0x00009FCC
		public TextObject GetTextObjectOfMessage()
		{
			TextObject textObject;
			if (!GameTexts.TryGetText(this.Message, out textObject, null))
			{
				textObject = new TextObject("{=!}" + this.Message, null);
			}
			return textObject;
		}

		// Token: 0x0400034C RID: 844
		public const string BadgeIdParameterName = "badge_id";

		// Token: 0x0400034D RID: 845
		public const string FriendRequesterParameterName = "friend_requester";
	}
}
