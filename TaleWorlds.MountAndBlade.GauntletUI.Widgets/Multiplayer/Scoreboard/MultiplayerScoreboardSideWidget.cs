using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Scoreboard
{
	// Token: 0x02000094 RID: 148
	public class MultiplayerScoreboardSideWidget : Widget
	{
		// Token: 0x06000807 RID: 2055 RVA: 0x000173C8 File Offset: 0x000155C8
		public MultiplayerScoreboardSideWidget(UIContext context)
			: base(context)
		{
			this._nameColumnItemDescription = new ContainerItemDescription();
			this._nameColumnItemDescription.WidgetIndex = 3;
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x000173F3 File Offset: 0x000155F3
		private void AvatarColumnWidthRatioUpdated()
		{
			if (this.TitlesListPanel == null)
			{
				return;
			}
			this._nameColumnItemDescription.WidthStretchRatio = this.NameColumnWidthRatio;
			this.TitlesListPanel.AddItemDescription(this._nameColumnItemDescription);
		}

		// Token: 0x06000809 RID: 2057 RVA: 0x00017420 File Offset: 0x00015620
		private void UpdateBackgroundColors()
		{
			if (string.IsNullOrEmpty(this.CultureId))
			{
				return;
			}
			base.Color = this.CultureColor;
		}

		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x0600080A RID: 2058 RVA: 0x0001743C File Offset: 0x0001563C
		// (set) Token: 0x0600080B RID: 2059 RVA: 0x00017444 File Offset: 0x00015644
		public Color CultureColor
		{
			get
			{
				return this._cultureColor;
			}
			set
			{
				if (value != this._cultureColor)
				{
					this._cultureColor = value;
					base.OnPropertyChanged(value, "CultureColor");
					this.UpdateBackgroundColors();
				}
			}
		}

		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x0600080C RID: 2060 RVA: 0x0001746D File Offset: 0x0001566D
		// (set) Token: 0x0600080D RID: 2061 RVA: 0x00017475 File Offset: 0x00015675
		public string CultureId
		{
			get
			{
				return this._cultureId;
			}
			set
			{
				if (value != this._cultureId)
				{
					this._cultureId = value;
					base.OnPropertyChanged<string>(value, "CultureId");
					this.UpdateBackgroundColors();
				}
			}
		}

		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x0600080E RID: 2062 RVA: 0x0001749E File Offset: 0x0001569E
		// (set) Token: 0x0600080F RID: 2063 RVA: 0x000174A6 File Offset: 0x000156A6
		public bool UseSecondary
		{
			get
			{
				return this._useSecondary;
			}
			set
			{
				if (value != this._useSecondary)
				{
					this._useSecondary = value;
					base.OnPropertyChanged(value, "UseSecondary");
					this.UpdateBackgroundColors();
				}
			}
		}

		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x06000810 RID: 2064 RVA: 0x000174CA File Offset: 0x000156CA
		// (set) Token: 0x06000811 RID: 2065 RVA: 0x000174D2 File Offset: 0x000156D2
		public float NameColumnWidthRatio
		{
			get
			{
				return this._nameColumnWidthRatio;
			}
			set
			{
				if (value != this._nameColumnWidthRatio)
				{
					this._nameColumnWidthRatio = value;
					base.OnPropertyChanged(value, "NameColumnWidthRatio");
					this.AvatarColumnWidthRatioUpdated();
				}
			}
		}

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x06000812 RID: 2066 RVA: 0x000174F6 File Offset: 0x000156F6
		// (set) Token: 0x06000813 RID: 2067 RVA: 0x000174FE File Offset: 0x000156FE
		public ListPanel TitlesListPanel
		{
			get
			{
				return this._titlesListPanel;
			}
			set
			{
				if (value != this._titlesListPanel)
				{
					this._titlesListPanel = value;
					base.OnPropertyChanged<ListPanel>(value, "TitlesListPanel");
					this.AvatarColumnWidthRatioUpdated();
				}
			}
		}

		// Token: 0x04000394 RID: 916
		private ContainerItemDescription _nameColumnItemDescription;

		// Token: 0x04000395 RID: 917
		private float _nameColumnWidthRatio = 1f;

		// Token: 0x04000396 RID: 918
		private ListPanel _titlesListPanel;

		// Token: 0x04000397 RID: 919
		private string _cultureId;

		// Token: 0x04000398 RID: 920
		private Color _cultureColor;

		// Token: 0x04000399 RID: 921
		private bool _useSecondary;
	}
}
