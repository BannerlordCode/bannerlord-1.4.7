using System;
using TaleWorlds.Library;

namespace TaleWorlds.TwoDimension.Standalone
{
	// Token: 0x0200000D RID: 13
	internal static class StandaloneApplicationUtility
	{
		// Token: 0x0600008F RID: 143 RVA: 0x00004ECD File Offset: 0x000030CD
		public static void TerminateWithMessageBox(string title, string message)
		{
			Debug.ShowMessageBox(message, title, 1U);
			Environment.Exit(0);
		}
	}
}
