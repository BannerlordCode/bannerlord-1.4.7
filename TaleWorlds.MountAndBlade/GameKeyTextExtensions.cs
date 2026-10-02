using System;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200022B RID: 555
	public static class GameKeyTextExtensions
	{
		// Token: 0x060020B8 RID: 8376 RVA: 0x000733BD File Offset: 0x000715BD
		public static TextObject GetHotKeyGameText(this GameTextManager gameTextManager, string categoryName, string hotKeyId)
		{
			return gameTextManager.GetHotKeyGameTextFromKeyID(HotKeyManager.GetHotKeyId(categoryName, hotKeyId));
		}

		// Token: 0x060020B9 RID: 8377 RVA: 0x000733CC File Offset: 0x000715CC
		public static TextObject GetHotKeyGameText(this GameTextManager gameTextManager, string categoryName, int gameKeyId)
		{
			return gameTextManager.GetHotKeyGameTextFromKeyID(HotKeyManager.GetHotKeyId(categoryName, gameKeyId));
		}

		// Token: 0x060020BA RID: 8378 RVA: 0x000733DB File Offset: 0x000715DB
		public static TextObject GetHotKeyGameTextFromKeyID(this GameTextManager gameTextManager, string keyId)
		{
			return gameTextManager.FindText("str_game_key_text", keyId.ToLower());
		}
	}
}
