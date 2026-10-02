using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer
{
	// Token: 0x02000090 RID: 144
	public class MultiplayerTroopTypeIconWidget : Widget
	{
		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x060007D1 RID: 2001 RVA: 0x0001697C File Offset: 0x00014B7C
		// (set) Token: 0x060007D2 RID: 2002 RVA: 0x00016984 File Offset: 0x00014B84
		public float ScaleFactor { get; set; } = 1f;

		// Token: 0x060007D3 RID: 2003 RVA: 0x0001698D File Offset: 0x00014B8D
		public MultiplayerTroopTypeIconWidget(UIContext context)
			: base(context)
		{
			this.BackgroundWidget = this;
		}

		// Token: 0x060007D4 RID: 2004 RVA: 0x000169A8 File Offset: 0x00014BA8
		private void UpdateIcon()
		{
			if (this.BackgroundWidget == null || this.ForegroundWidget == null || string.IsNullOrEmpty(this.IconSpriteType))
			{
				return;
			}
			string text = "MPHud\\TroopIcons\\" + this.IconSpriteType;
			string text2 = text + "_Outline";
			this.ForegroundWidget.Sprite = base.Context.SpriteData.GetSprite(text);
			this.BackgroundWidget.Sprite = base.Context.SpriteData.GetSprite(text2);
			if (this.BackgroundWidget.Sprite != null)
			{
				float num = (float)this.BackgroundWidget.Sprite.Width;
				this.BackgroundWidget.SuggestedWidth = num * this.ScaleFactor;
				this.ForegroundWidget.SuggestedWidth = num * this.ScaleFactor;
				float num2 = (float)this.BackgroundWidget.Sprite.Height;
				this.BackgroundWidget.SuggestedHeight = num2 * this.ScaleFactor;
				this.ForegroundWidget.SuggestedHeight = num2 * this.ScaleFactor;
			}
		}

		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x060007D5 RID: 2005 RVA: 0x00016AA5 File Offset: 0x00014CA5
		// (set) Token: 0x060007D6 RID: 2006 RVA: 0x00016AAD File Offset: 0x00014CAD
		[DataSourceProperty]
		public Widget BackgroundWidget
		{
			get
			{
				return this._backgroundWidget;
			}
			set
			{
				if (this._backgroundWidget != value)
				{
					this._backgroundWidget = value;
					base.OnPropertyChanged<Widget>(value, "BackgroundWidget");
					this.UpdateIcon();
				}
			}
		}

		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x060007D7 RID: 2007 RVA: 0x00016AD1 File Offset: 0x00014CD1
		// (set) Token: 0x060007D8 RID: 2008 RVA: 0x00016AD9 File Offset: 0x00014CD9
		[DataSourceProperty]
		public Widget ForegroundWidget
		{
			get
			{
				return this._foregroundWidget;
			}
			set
			{
				if (this._foregroundWidget != value)
				{
					this._foregroundWidget = value;
					base.OnPropertyChanged<Widget>(value, "ForegroundWidget");
					this.UpdateIcon();
				}
			}
		}

		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x060007D9 RID: 2009 RVA: 0x00016AFD File Offset: 0x00014CFD
		// (set) Token: 0x060007DA RID: 2010 RVA: 0x00016B05 File Offset: 0x00014D05
		[DataSourceProperty]
		public string IconSpriteType
		{
			get
			{
				return this._iconSpriteType;
			}
			set
			{
				if (this._iconSpriteType != value)
				{
					this._iconSpriteType = value;
					base.OnPropertyChanged<string>(value, "IconSpriteType");
					this.UpdateIcon();
				}
			}
		}

		// Token: 0x04000372 RID: 882
		private Widget _backgroundWidget;

		// Token: 0x04000373 RID: 883
		private Widget _foregroundWidget;

		// Token: 0x04000374 RID: 884
		private string _iconSpriteType;
	}
}
