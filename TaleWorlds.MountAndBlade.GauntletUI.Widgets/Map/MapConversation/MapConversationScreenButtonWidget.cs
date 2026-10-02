using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Map.MapConversation
{
	// Token: 0x02000126 RID: 294
	public class MapConversationScreenButtonWidget : ButtonWidget
	{
		// Token: 0x17000580 RID: 1408
		// (get) Token: 0x06000F7E RID: 3966 RVA: 0x0002AD8D File Offset: 0x00028F8D
		// (set) Token: 0x06000F7F RID: 3967 RVA: 0x0002AD95 File Offset: 0x00028F95
		public Widget ConversationParent { get; set; }

		// Token: 0x06000F80 RID: 3968 RVA: 0x0002AD9E File Offset: 0x00028F9E
		public MapConversationScreenButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x17000581 RID: 1409
		// (get) Token: 0x06000F81 RID: 3969 RVA: 0x0002ADA7 File Offset: 0x00028FA7
		// (set) Token: 0x06000F82 RID: 3970 RVA: 0x0002ADAF File Offset: 0x00028FAF
		public bool IsBarterActive
		{
			get
			{
				return this._isBarterActive;
			}
			set
			{
				if (this._isBarterActive != value)
				{
					this._isBarterActive = value;
					this.ConversationParent.IsVisible = !this.IsBarterActive;
				}
			}
		}

		// Token: 0x0400070D RID: 1805
		private bool _isBarterActive;
	}
}
