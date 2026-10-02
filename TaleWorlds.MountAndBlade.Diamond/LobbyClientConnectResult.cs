using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Diamond;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000125 RID: 293
	public class LobbyClientConnectResult
	{
		// Token: 0x1700025E RID: 606
		// (get) Token: 0x0600078A RID: 1930 RVA: 0x0000B644 File Offset: 0x00009844
		// (set) Token: 0x0600078B RID: 1931 RVA: 0x0000B64C File Offset: 0x0000984C
		public bool Connected { get; private set; }

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x0600078C RID: 1932 RVA: 0x0000B655 File Offset: 0x00009855
		// (set) Token: 0x0600078D RID: 1933 RVA: 0x0000B65D File Offset: 0x0000985D
		public TextObject Error { get; private set; }

		// Token: 0x0600078E RID: 1934 RVA: 0x0000B666 File Offset: 0x00009866
		public LobbyClientConnectResult(bool connected, TextObject error)
		{
			this.Connected = connected;
			this.Error = error;
		}

		// Token: 0x0600078F RID: 1935 RVA: 0x0000B67C File Offset: 0x0000987C
		public static LobbyClientConnectResult FromServerConnectResult(string errorCode, Dictionary<string, string> parameters)
		{
			TextObject textObject = GameTexts.FindText("str_login_error", errorCode);
			if (textObject == null)
			{
				Debug.FailedAssert("Error text is not handled: " + errorCode, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Diamond\\LobbyClient.cs", "FromServerConnectResult", 2216);
				textObject = new TextObject("{=tzQxtv27}Unknown error.", null);
			}
			else if (parameters != null)
			{
				foreach (string text in parameters.Keys)
				{
					if (text == "BANREASON")
					{
						if (parameters[text].StartsWith("Custom:"))
						{
							textObject.SetTextVariable(text, parameters[text].Substring("Custom:".Length));
						}
						else
						{
							TextObject textObject2 = GameTexts.FindText("str_ban_reason", parameters[text]);
							textObject.SetTextVariable(text, textObject2.ToString());
						}
					}
					else if (text == "ACCESSERROR")
					{
						TextObject textObject3 = GameTexts.FindText("str_access_error", parameters[text]);
						textObject.SetTextVariable(text, textObject3.ToString());
					}
					else
					{
						textObject.SetTextVariable(text, parameters[text]);
					}
				}
			}
			return new LobbyClientConnectResult(errorCode == LoginErrorCode.None.ToString(), textObject);
		}
	}
}
