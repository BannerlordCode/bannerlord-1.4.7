using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer
{
	// Token: 0x0200008C RID: 140
	public class MultiplayerItemTabButtonWidget : ButtonWidget
	{
		// Token: 0x060007B5 RID: 1973 RVA: 0x000165F4 File Offset: 0x000147F4
		public MultiplayerItemTabButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060007B6 RID: 1974 RVA: 0x00016600 File Offset: 0x00014800
		private void UpdateIcon()
		{
			if (string.IsNullOrEmpty(this.ItemType) || this._iconWidget == null)
			{
				return;
			}
			Sprite sprite = base.Context.SpriteData.GetSprite("StdAssets\\ItemIcons\\" + this.ItemType);
			this.IconWidget.Brush.DefaultLayer.Sprite = sprite;
			Sprite sprite2 = base.Context.SpriteData.GetSprite("StdAssets\\ItemIcons\\" + this.ItemType + "_selected");
			this.IconWidget.Brush.GetLayer("Selected").Sprite = sprite2;
		}

		// Token: 0x060007B7 RID: 1975 RVA: 0x0001669B File Offset: 0x0001489B
		protected override void RefreshState()
		{
			base.RefreshState();
			if (base.IsSelected && base.ParentWidget is Container)
			{
				(base.ParentWidget as Container).OnChildSelected(this);
			}
		}

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x060007B8 RID: 1976 RVA: 0x000166C9 File Offset: 0x000148C9
		// (set) Token: 0x060007B9 RID: 1977 RVA: 0x000166D1 File Offset: 0x000148D1
		[Editor(false)]
		public string ItemType
		{
			get
			{
				return this._itemType;
			}
			set
			{
				if (value != this._itemType)
				{
					this._itemType = value;
					base.OnPropertyChanged<string>(value, "ItemType");
					this.UpdateIcon();
				}
			}
		}

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x060007BA RID: 1978 RVA: 0x000166FA File Offset: 0x000148FA
		// (set) Token: 0x060007BB RID: 1979 RVA: 0x00016702 File Offset: 0x00014902
		[Editor(false)]
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

		// Token: 0x04000361 RID: 865
		private const string BaseSpritePath = "StdAssets\\ItemIcons\\";

		// Token: 0x04000362 RID: 866
		private string _itemType;

		// Token: 0x04000363 RID: 867
		private BrushWidget _iconWidget;
	}
}
