using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x0200003A RID: 58
	public class SceneNotificationDescriptionTextWidget : TextWidget
	{
		// Token: 0x06000359 RID: 857 RVA: 0x0000AB6C File Offset: 0x00008D6C
		public SceneNotificationDescriptionTextWidget(UIContext context)
			: base(context)
		{
			this._defaultAlignment = base.ReadOnlyBrush.TextHorizontalAlignment;
		}

		// Token: 0x0600035A RID: 858 RVA: 0x0000AB88 File Offset: 0x00008D88
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._text.LineCount != this._cachedLineCount)
			{
				this._cachedLineCount = this._text.LineCount;
				if (this._cachedLineCount == 1)
				{
					base.ReadOnlyBrush.TextHorizontalAlignment = this._defaultAlignment;
					return;
				}
				base.ReadOnlyBrush.TextHorizontalAlignment = this.MultiLineAlignment;
			}
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x0600035B RID: 859 RVA: 0x0000ABEC File Offset: 0x00008DEC
		// (set) Token: 0x0600035C RID: 860 RVA: 0x0000ABF4 File Offset: 0x00008DF4
		[Editor(false)]
		public TextHorizontalAlignment MultiLineAlignment
		{
			get
			{
				return this._multiLineAlignment;
			}
			set
			{
				this._multiLineAlignment = value;
			}
		}

		// Token: 0x0400015B RID: 347
		private int _cachedLineCount;

		// Token: 0x0400015C RID: 348
		private TextHorizontalAlignment _defaultAlignment;

		// Token: 0x0400015D RID: 349
		private TextHorizontalAlignment _multiLineAlignment;
	}
}
