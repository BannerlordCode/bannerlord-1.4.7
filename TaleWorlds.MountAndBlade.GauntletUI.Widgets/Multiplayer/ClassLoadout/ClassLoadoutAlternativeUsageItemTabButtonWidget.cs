using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.ClassLoadout
{
	// Token: 0x020000C9 RID: 201
	public class ClassLoadoutAlternativeUsageItemTabButtonWidget : ButtonWidget
	{
		// Token: 0x06000A8B RID: 2699 RVA: 0x0001D884 File Offset: 0x0001BA84
		public ClassLoadoutAlternativeUsageItemTabButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000A8C RID: 2700 RVA: 0x0001D890 File Offset: 0x0001BA90
		private void UpdateIcon()
		{
			if (string.IsNullOrEmpty(this.UsageType) || this._iconWidget == null)
			{
				return;
			}
			Sprite sprite = base.Context.SpriteData.GetSprite("MPClassLoadout\\UsageIcons\\" + this.UsageType);
			foreach (Style style in this.IconWidget.Brush.Styles)
			{
				StyleLayer[] layers = style.GetLayers();
				for (int i = 0; i < layers.Length; i++)
				{
					layers[i].Sprite = sprite;
				}
			}
		}

		// Token: 0x06000A8D RID: 2701 RVA: 0x0001D93C File Offset: 0x0001BB3C
		protected override void RefreshState()
		{
			base.RefreshState();
			if (base.IsSelected && base.ParentWidget is Container)
			{
				(base.ParentWidget as Container).OnChildSelected(this);
			}
		}

		// Token: 0x170003B0 RID: 944
		// (get) Token: 0x06000A8E RID: 2702 RVA: 0x0001D96A File Offset: 0x0001BB6A
		// (set) Token: 0x06000A8F RID: 2703 RVA: 0x0001D972 File Offset: 0x0001BB72
		public string UsageType
		{
			get
			{
				return this._usageType;
			}
			set
			{
				if (value != this._usageType)
				{
					this._usageType = value;
					base.OnPropertyChanged<string>(value, "UsageType");
					this.UpdateIcon();
				}
			}
		}

		// Token: 0x170003B1 RID: 945
		// (get) Token: 0x06000A90 RID: 2704 RVA: 0x0001D99B File Offset: 0x0001BB9B
		// (set) Token: 0x06000A91 RID: 2705 RVA: 0x0001D9A3 File Offset: 0x0001BBA3
		public BrushWidget IconWidget
		{
			get
			{
				return this._iconWidget;
			}
			set
			{
				if (value != this._iconWidget)
				{
					this._iconWidget = value;
					base.OnPropertyChanged<BrushWidget>(value, "IconWidget");
					this.UpdateIcon();
				}
			}
		}

		// Token: 0x040004D2 RID: 1234
		private string _usageType;

		// Token: 0x040004D3 RID: 1235
		private BrushWidget _iconWidget;
	}
}
