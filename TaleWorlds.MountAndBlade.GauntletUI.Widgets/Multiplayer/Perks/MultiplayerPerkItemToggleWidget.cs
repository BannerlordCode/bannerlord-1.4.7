using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Perks
{
	// Token: 0x02000099 RID: 153
	public class MultiplayerPerkItemToggleWidget : ToggleButtonWidget
	{
		// Token: 0x06000836 RID: 2102 RVA: 0x00017A7C File Offset: 0x00015C7C
		public MultiplayerPerkItemToggleWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000837 RID: 2103 RVA: 0x00017A85 File Offset: 0x00015C85
		protected override void HandleClick()
		{
			base.HandleClick();
			MultiplayerPerkContainerPanelWidget containerPanel = this.ContainerPanel;
			if (containerPanel == null)
			{
				return;
			}
			containerPanel.PerkSelected(this._isSelectable ? this : null);
		}

		// Token: 0x06000838 RID: 2104 RVA: 0x00017AAC File Offset: 0x00015CAC
		private void UpdateIcon()
		{
			if (string.IsNullOrEmpty(this.IconType) || this._iconWidget == null)
			{
				return;
			}
			foreach (Style style in this.IconWidget.Brush.Styles)
			{
				StyleLayer[] layers = style.GetLayers();
				for (int i = 0; i < layers.Length; i++)
				{
					layers[i].Sprite = base.Context.SpriteData.GetSprite("General\\Perks\\" + this.IconType);
				}
			}
		}

		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x06000839 RID: 2105 RVA: 0x00017B54 File Offset: 0x00015D54
		// (set) Token: 0x0600083A RID: 2106 RVA: 0x00017B5C File Offset: 0x00015D5C
		[DataSourceProperty]
		public string IconType
		{
			get
			{
				return this._iconType;
			}
			set
			{
				if (value != this._iconType)
				{
					this._iconType = value;
					base.OnPropertyChanged<string>(value, "IconType");
					this.UpdateIcon();
				}
			}
		}

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x0600083B RID: 2107 RVA: 0x00017B85 File Offset: 0x00015D85
		// (set) Token: 0x0600083C RID: 2108 RVA: 0x00017B8D File Offset: 0x00015D8D
		[DataSourceProperty]
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

		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x0600083D RID: 2109 RVA: 0x00017BB1 File Offset: 0x00015DB1
		// (set) Token: 0x0600083E RID: 2110 RVA: 0x00017BB9 File Offset: 0x00015DB9
		[DataSourceProperty]
		public bool IsSelectable
		{
			get
			{
				return this._isSelectable;
			}
			set
			{
				if (value != this._isSelectable)
				{
					this._isSelectable = value;
					base.OnPropertyChanged(value, "IsSelectable");
				}
			}
		}

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x0600083F RID: 2111 RVA: 0x00017BD7 File Offset: 0x00015DD7
		// (set) Token: 0x06000840 RID: 2112 RVA: 0x00017BDF File Offset: 0x00015DDF
		[DataSourceProperty]
		public MultiplayerPerkContainerPanelWidget ContainerPanel
		{
			get
			{
				return this._containerPanel;
			}
			set
			{
				if (value != this._containerPanel)
				{
					this._containerPanel = value;
					base.OnPropertyChanged<MultiplayerPerkContainerPanelWidget>(value, "ContainerPanel");
				}
			}
		}

		// Token: 0x040003AB RID: 939
		private string _iconType;

		// Token: 0x040003AC RID: 940
		private BrushWidget _iconWidget;

		// Token: 0x040003AD RID: 941
		private bool _isSelectable;

		// Token: 0x040003AE RID: 942
		private MultiplayerPerkContainerPanelWidget _containerPanel;
	}
}
