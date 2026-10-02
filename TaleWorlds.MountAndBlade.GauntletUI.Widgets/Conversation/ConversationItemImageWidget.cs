using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Conversation
{
	// Token: 0x0200016D RID: 365
	public class ConversationItemImageWidget : ImageWidget
	{
		// Token: 0x170006CE RID: 1742
		// (get) Token: 0x06001332 RID: 4914 RVA: 0x0003443D File Offset: 0x0003263D
		// (set) Token: 0x06001333 RID: 4915 RVA: 0x00034445 File Offset: 0x00032645
		public Brush NormalBrush { get; set; }

		// Token: 0x170006CF RID: 1743
		// (get) Token: 0x06001334 RID: 4916 RVA: 0x0003444E File Offset: 0x0003264E
		// (set) Token: 0x06001335 RID: 4917 RVA: 0x00034456 File Offset: 0x00032656
		public Brush SpecialBrush { get; set; }

		// Token: 0x170006D0 RID: 1744
		// (get) Token: 0x06001336 RID: 4918 RVA: 0x0003445F File Offset: 0x0003265F
		// (set) Token: 0x06001337 RID: 4919 RVA: 0x00034467 File Offset: 0x00032667
		public bool IsSpecial { get; set; }

		// Token: 0x06001338 RID: 4920 RVA: 0x00034470 File Offset: 0x00032670
		public ConversationItemImageWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06001339 RID: 4921 RVA: 0x00034479 File Offset: 0x00032679
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this._isInitialized)
			{
				base.Brush = (this.IsSpecial ? this.SpecialBrush : this.NormalBrush);
				this._isInitialized = true;
			}
		}

		// Token: 0x040008B8 RID: 2232
		private bool _isInitialized;
	}
}
