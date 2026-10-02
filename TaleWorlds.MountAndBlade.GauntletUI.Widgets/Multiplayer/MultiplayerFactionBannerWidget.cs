using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer
{
	// Token: 0x0200008A RID: 138
	public class MultiplayerFactionBannerWidget : Widget
	{
		// Token: 0x060007A3 RID: 1955 RVA: 0x00016374 File Offset: 0x00014574
		public MultiplayerFactionBannerWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060007A4 RID: 1956 RVA: 0x00016384 File Offset: 0x00014584
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (this._firstFrame)
			{
				this.UpdateBanner();
				this.UpdateIcon();
				this._firstFrame = false;
			}
		}

		// Token: 0x060007A5 RID: 1957 RVA: 0x000163A8 File Offset: 0x000145A8
		private void UpdateBanner()
		{
			if (this._bannerWidget == null)
			{
				return;
			}
			BrushWidget brushWidget;
			if ((brushWidget = this.BannerWidget as BrushWidget) != null)
			{
				using (Dictionary<string, Style>.ValueCollection.Enumerator enumerator = brushWidget.Brush.Styles.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Style style = enumerator.Current;
						StyleLayer[] layers = style.GetLayers();
						for (int i = 0; i < layers.Length; i++)
						{
							layers[i].Color = this.CultureColor1;
						}
					}
					return;
				}
			}
			this.BannerWidget.Color = this.CultureColor1;
		}

		// Token: 0x060007A6 RID: 1958 RVA: 0x00016444 File Offset: 0x00014644
		private void UpdateIcon()
		{
			if (string.IsNullOrEmpty(this.FactionCode) || this._iconWidget == null)
			{
				return;
			}
			this.IconWidget.Sprite = base.Context.SpriteData.GetSprite("StdAssets\\FactionIcons\\LargeIcons\\" + this.FactionCode);
			this.IconWidget.Color = this.CultureColor2;
		}

		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x060007A7 RID: 1959 RVA: 0x000164A3 File Offset: 0x000146A3
		// (set) Token: 0x060007A8 RID: 1960 RVA: 0x000164AB File Offset: 0x000146AB
		[DataSourceProperty]
		public Color CultureColor1
		{
			get
			{
				return this._cultureColor1;
			}
			set
			{
				if (value != this._cultureColor1)
				{
					this._cultureColor1 = value;
					base.OnPropertyChanged(value, "CultureColor1");
					this.UpdateBanner();
				}
			}
		}

		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x060007A9 RID: 1961 RVA: 0x000164D4 File Offset: 0x000146D4
		// (set) Token: 0x060007AA RID: 1962 RVA: 0x000164DC File Offset: 0x000146DC
		[DataSourceProperty]
		public Color CultureColor2
		{
			get
			{
				return this._cultureColor2;
			}
			set
			{
				if (value != this._cultureColor2)
				{
					this._cultureColor2 = value;
					base.OnPropertyChanged(value, "CultureColor2");
					this.UpdateIcon();
				}
			}
		}

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x060007AB RID: 1963 RVA: 0x00016505 File Offset: 0x00014705
		// (set) Token: 0x060007AC RID: 1964 RVA: 0x0001650D File Offset: 0x0001470D
		[DataSourceProperty]
		public string FactionCode
		{
			get
			{
				return this._factionCode;
			}
			set
			{
				if (value != this._factionCode)
				{
					this._factionCode = value;
					base.OnPropertyChanged<string>(value, "FactionCode");
					this.UpdateIcon();
				}
			}
		}

		// Token: 0x170002BA RID: 698
		// (get) Token: 0x060007AD RID: 1965 RVA: 0x00016536 File Offset: 0x00014736
		// (set) Token: 0x060007AE RID: 1966 RVA: 0x0001653E File Offset: 0x0001473E
		[DataSourceProperty]
		public Widget BannerWidget
		{
			get
			{
				return this._bannerWidget;
			}
			set
			{
				if (value != this._bannerWidget)
				{
					this._bannerWidget = value;
					base.OnPropertyChanged<Widget>(value, "BannerWidget");
					this.UpdateBanner();
				}
			}
		}

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x060007AF RID: 1967 RVA: 0x00016562 File Offset: 0x00014762
		// (set) Token: 0x060007B0 RID: 1968 RVA: 0x0001656A File Offset: 0x0001476A
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

		// Token: 0x0400035A RID: 858
		private bool _firstFrame = true;

		// Token: 0x0400035B RID: 859
		private Color _cultureColor1;

		// Token: 0x0400035C RID: 860
		private Color _cultureColor2;

		// Token: 0x0400035D RID: 861
		private string _factionCode;

		// Token: 0x0400035E RID: 862
		private Widget _bannerWidget;

		// Token: 0x0400035F RID: 863
		private Widget _iconWidget;
	}
}
