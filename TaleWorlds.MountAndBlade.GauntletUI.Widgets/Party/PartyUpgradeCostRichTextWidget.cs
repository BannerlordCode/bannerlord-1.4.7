using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Party
{
	// Token: 0x0200006D RID: 109
	public class PartyUpgradeCostRichTextWidget : RichTextWidget
	{
		// Token: 0x060005F1 RID: 1521 RVA: 0x00011AC0 File Offset: 0x0000FCC0
		public PartyUpgradeCostRichTextWidget(UIContext context)
			: base(context)
		{
			this.NormalColor = new Color(1f, 1f, 1f, 1f);
			this.InsufficientColor = new Color(0.753f, 0.071f, 0.098f, 1f);
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x00011B19 File Offset: 0x0000FD19
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._requiresRefresh)
			{
				base.Brush.FontColor = (this.IsSufficient ? this.NormalColor : this.InsufficientColor);
				this._requiresRefresh = false;
			}
		}

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x060005F3 RID: 1523 RVA: 0x00011B52 File Offset: 0x0000FD52
		// (set) Token: 0x060005F4 RID: 1524 RVA: 0x00011B5A File Offset: 0x0000FD5A
		[Editor(false)]
		public bool IsSufficient
		{
			get
			{
				return this._isSufficient;
			}
			set
			{
				if (value != this._isSufficient)
				{
					this._isSufficient = value;
					base.OnPropertyChanged(value, "IsSufficient");
					this._requiresRefresh = true;
				}
			}
		}

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x060005F5 RID: 1525 RVA: 0x00011B7F File Offset: 0x0000FD7F
		// (set) Token: 0x060005F6 RID: 1526 RVA: 0x00011B87 File Offset: 0x0000FD87
		public Color NormalColor
		{
			get
			{
				return this._normalColor;
			}
			set
			{
				if (value != this._normalColor)
				{
					this._normalColor = value;
					this._requiresRefresh = true;
				}
			}
		}

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x060005F7 RID: 1527 RVA: 0x00011BA5 File Offset: 0x0000FDA5
		// (set) Token: 0x060005F8 RID: 1528 RVA: 0x00011BAD File Offset: 0x0000FDAD
		public Color InsufficientColor
		{
			get
			{
				return this._insufficientColor;
			}
			set
			{
				if (value != this._insufficientColor)
				{
					this._insufficientColor = value;
					this._requiresRefresh = true;
				}
			}
		}

		// Token: 0x0400028D RID: 653
		private bool _requiresRefresh = true;

		// Token: 0x0400028E RID: 654
		private bool _isSufficient;

		// Token: 0x0400028F RID: 655
		private Color _normalColor;

		// Token: 0x04000290 RID: 656
		private Color _insufficientColor;
	}
}
