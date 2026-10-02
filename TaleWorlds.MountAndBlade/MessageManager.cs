using System;
using System.Diagnostics;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001E3 RID: 483
	public static class MessageManager
	{
		// Token: 0x06001C60 RID: 7264 RVA: 0x000612A2 File Offset: 0x0005F4A2
		public static void DisplayMessage(string message)
		{
			MBAPI.IMBMessageManager.DisplayMessage(message);
		}

		// Token: 0x06001C61 RID: 7265 RVA: 0x000612AF File Offset: 0x0005F4AF
		public static void DisplayMessage(string message, uint color)
		{
			MBAPI.IMBMessageManager.DisplayMessageWithColor(message, color);
		}

		// Token: 0x06001C62 RID: 7266 RVA: 0x000612C0 File Offset: 0x0005F4C0
		[Conditional("DEBUG")]
		public static void DisplayDebugMessage(string message)
		{
			if (message.Length > 4 && message.Substring(0, 4).Equals("[DEBUG]"))
			{
				message = message.Substring(4);
			}
			MBAPI.IMBMessageManager.DisplayMessageWithColor("[DEBUG]: " + message, 4294936712U);
		}

		// Token: 0x06001C63 RID: 7267 RVA: 0x00061310 File Offset: 0x0005F510
		public static void DisplayMultilineMessage(string message, uint color)
		{
			if (message.Contains("\n"))
			{
				string[] array = message.Split(new char[] { '\n' });
				for (int i = 0; i < array.Length; i++)
				{
					MBAPI.IMBMessageManager.DisplayMessageWithColor(array[i], color);
				}
				return;
			}
			MBAPI.IMBMessageManager.DisplayMessageWithColor(message, color);
		}

		// Token: 0x06001C64 RID: 7268 RVA: 0x00061365 File Offset: 0x0005F565
		public static void EraseMessageLines()
		{
			MBAPI.IMBWindowManager.EraseMessageLines();
		}

		// Token: 0x06001C65 RID: 7269 RVA: 0x00061371 File Offset: 0x0005F571
		public static void SetMessageManager(MessageManagerBase messageManager)
		{
			MBAPI.IMBMessageManager.SetMessageManager(messageManager);
		}
	}
}
