using System;
using System.Collections.Generic;
using TaleWorlds.Core.ImageIdentifiers;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement
{
	// Token: 0x0200011E RID: 286
	public readonly struct ClanCardSelectionItemInfo
	{
		// Token: 0x06001A3C RID: 6716 RVA: 0x000632F8 File Offset: 0x000614F8
		public ClanCardSelectionItemInfo(object identifier, TextObject title, ImageIdentifier image, CardSelectionItemSpriteType spriteType, string spriteName, string spriteLabel, IEnumerable<ClanCardSelectionItemPropertyInfo> properties, bool isDisabled, TextObject disabledReason, TextObject actionResult)
		{
			this.Identifier = identifier;
			this.Title = title;
			this.Image = image;
			this.SpriteType = spriteType;
			this.SpriteName = spriteName;
			this.SpriteLabel = spriteLabel;
			this.Properties = properties;
			this.IsSpecialActionItem = false;
			this.SpecialActionText = null;
			this.IsDisabled = isDisabled;
			this.DisabledReason = disabledReason;
			this.ActionResult = actionResult;
		}

		// Token: 0x06001A3D RID: 6717 RVA: 0x00063380 File Offset: 0x00061580
		public ClanCardSelectionItemInfo(TextObject specialActionText, bool isDisabled, TextObject disabledReason, TextObject actionResult)
		{
			this.Identifier = null;
			this.Title = null;
			this.Image = null;
			this.SpriteType = CardSelectionItemSpriteType.None;
			this.SpriteName = null;
			this.SpriteLabel = null;
			this.Properties = null;
			this.IsSpecialActionItem = true;
			this.SpecialActionText = specialActionText;
			this.IsDisabled = isDisabled;
			this.DisabledReason = disabledReason;
			this.ActionResult = actionResult;
		}

		// Token: 0x04000C14 RID: 3092
		public readonly object Identifier;

		// Token: 0x04000C15 RID: 3093
		public readonly TextObject Title;

		// Token: 0x04000C16 RID: 3094
		public readonly ImageIdentifier Image;

		// Token: 0x04000C17 RID: 3095
		public readonly CardSelectionItemSpriteType SpriteType;

		// Token: 0x04000C18 RID: 3096
		public readonly string SpriteName;

		// Token: 0x04000C19 RID: 3097
		public readonly string SpriteLabel;

		// Token: 0x04000C1A RID: 3098
		public readonly IEnumerable<ClanCardSelectionItemPropertyInfo> Properties;

		// Token: 0x04000C1B RID: 3099
		public readonly bool IsSpecialActionItem;

		// Token: 0x04000C1C RID: 3100
		public readonly TextObject SpecialActionText;

		// Token: 0x04000C1D RID: 3101
		public readonly bool IsDisabled;

		// Token: 0x04000C1E RID: 3102
		public readonly TextObject DisabledReason;

		// Token: 0x04000C1F RID: 3103
		public readonly TextObject ActionResult;
	}
}
