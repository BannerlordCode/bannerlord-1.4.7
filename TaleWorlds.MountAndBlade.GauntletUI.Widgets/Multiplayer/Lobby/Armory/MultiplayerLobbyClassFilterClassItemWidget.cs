using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby.Armory
{
	// Token: 0x020000BB RID: 187
	public class MultiplayerLobbyClassFilterClassItemWidget : ToggleStateButtonWidget
	{
		// Token: 0x060009C9 RID: 2505 RVA: 0x0001B666 File Offset: 0x00019866
		public MultiplayerLobbyClassFilterClassItemWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060009CA RID: 2506 RVA: 0x0001B66F File Offset: 0x0001986F
		private void SetFactionColor()
		{
			if (this.FactionColorWidget == null)
			{
				return;
			}
			this.FactionColorWidget.Color = this.CultureColor;
		}

		// Token: 0x060009CB RID: 2507 RVA: 0x0001B68C File Offset: 0x0001988C
		private void UpdateIcon()
		{
			if (string.IsNullOrEmpty(this.TroopType) || this._iconWidget == null)
			{
				return;
			}
			Widget iconWidget = this.IconWidget;
			Brush iconBrush = this.IconBrush;
			Sprite sprite;
			if (iconBrush == null)
			{
				sprite = null;
			}
			else
			{
				BrushLayer layer = iconBrush.GetLayer(this.TroopType);
				sprite = ((layer != null) ? layer.Sprite : null);
			}
			iconWidget.Sprite = sprite;
		}

		// Token: 0x1700036E RID: 878
		// (get) Token: 0x060009CC RID: 2508 RVA: 0x0001B6DE File Offset: 0x000198DE
		// (set) Token: 0x060009CD RID: 2509 RVA: 0x0001B6E6 File Offset: 0x000198E6
		[Editor(false)]
		public string TroopType
		{
			get
			{
				return this._troopType;
			}
			set
			{
				if (value != this._troopType)
				{
					this._troopType = value;
					base.OnPropertyChanged<string>(value, "TroopType");
					this.UpdateIcon();
				}
			}
		}

		// Token: 0x1700036F RID: 879
		// (get) Token: 0x060009CE RID: 2510 RVA: 0x0001B70F File Offset: 0x0001990F
		// (set) Token: 0x060009CF RID: 2511 RVA: 0x0001B717 File Offset: 0x00019917
		[Editor(false)]
		public Color CultureColor
		{
			get
			{
				return this._cultureColor;
			}
			set
			{
				if (this._cultureColor != value)
				{
					this._cultureColor = value;
					base.OnPropertyChanged(value, "CultureColor");
					this.SetFactionColor();
				}
			}
		}

		// Token: 0x17000370 RID: 880
		// (get) Token: 0x060009D0 RID: 2512 RVA: 0x0001B740 File Offset: 0x00019940
		// (set) Token: 0x060009D1 RID: 2513 RVA: 0x0001B748 File Offset: 0x00019948
		[Editor(false)]
		public Brush IconBrush
		{
			get
			{
				return this._iconBrush;
			}
			set
			{
				if (this._iconBrush != value)
				{
					this._iconBrush = value;
					base.OnPropertyChanged<Brush>(value, "IconBrush");
				}
			}
		}

		// Token: 0x17000371 RID: 881
		// (get) Token: 0x060009D2 RID: 2514 RVA: 0x0001B766 File Offset: 0x00019966
		// (set) Token: 0x060009D3 RID: 2515 RVA: 0x0001B76E File Offset: 0x0001996E
		[DataSourceProperty]
		public Widget IconWidget
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
					base.OnPropertyChanged<Widget>(value, "IconWidget");
					this.UpdateIcon();
				}
			}
		}

		// Token: 0x17000372 RID: 882
		// (get) Token: 0x060009D4 RID: 2516 RVA: 0x0001B792 File Offset: 0x00019992
		// (set) Token: 0x060009D5 RID: 2517 RVA: 0x0001B79A File Offset: 0x0001999A
		[Editor(false)]
		public Widget FactionColorWidget
		{
			get
			{
				return this._factionColorWidget;
			}
			set
			{
				if (this._factionColorWidget != value)
				{
					this._factionColorWidget = value;
					base.OnPropertyChanged<Widget>(value, "FactionColorWidget");
					this.SetFactionColor();
				}
			}
		}

		// Token: 0x0400046C RID: 1132
		private string _troopType;

		// Token: 0x0400046D RID: 1133
		private Color _cultureColor;

		// Token: 0x0400046E RID: 1134
		private Brush _iconBrush;

		// Token: 0x0400046F RID: 1135
		private Widget _iconWidget;

		// Token: 0x04000470 RID: 1136
		private Widget _factionColorWidget;
	}
}
