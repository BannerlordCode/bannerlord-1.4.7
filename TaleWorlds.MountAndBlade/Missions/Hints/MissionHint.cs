using System;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Missions.Hints
{
	// Token: 0x020003EE RID: 1006
	public class MissionHint
	{
		// Token: 0x06003733 RID: 14131 RVA: 0x000E490D File Offset: 0x000E2B0D
		public MissionHint(TextObject description)
		{
			this.Description = description;
		}

		// Token: 0x06003734 RID: 14132 RVA: 0x000E491C File Offset: 0x000E2B1C
		public static MissionHint CreateWithKeyAndAction(TextObject actionText, string hotKeyId)
		{
			TextObject textObject = GameTexts.FindText("str_key_action", null).CopyTextObject();
			textObject.SetTextVariable("KEY", HyperlinkTexts.GetKeyHyperlinkText(hotKeyId, 1f));
			textObject.SetTextVariable("ACTION", actionText);
			return new MissionHint(textObject);
		}

		// Token: 0x040017B9 RID: 6073
		public readonly TextObject Description;
	}
}
