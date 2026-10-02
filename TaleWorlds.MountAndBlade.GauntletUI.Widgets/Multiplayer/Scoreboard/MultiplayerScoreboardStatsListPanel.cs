using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Scoreboard
{
	// Token: 0x02000095 RID: 149
	public class MultiplayerScoreboardStatsListPanel : ListPanel
	{
		// Token: 0x06000814 RID: 2068 RVA: 0x00017524 File Offset: 0x00015724
		public MultiplayerScoreboardStatsListPanel(UIContext context)
			: base(context)
		{
			this._nameColumnItemDescription = new ContainerItemDescription
			{
				WidgetId = "name"
			};
			this._scoreColumnItemDescription = new ContainerItemDescription
			{
				WidgetId = "score"
			};
			this._soldiersColumnItemDescription = new ContainerItemDescription
			{
				WidgetId = "soldiers"
			};
		}

		// Token: 0x06000815 RID: 2069 RVA: 0x0001759B File Offset: 0x0001579B
		private void NameColumnWidthRatioUpdated()
		{
			this._nameColumnItemDescription.WidthStretchRatio = this.NameColumnWidthRatio;
			base.AddItemDescription(this._nameColumnItemDescription);
			base.SetMeasureAndLayoutDirty();
		}

		// Token: 0x06000816 RID: 2070 RVA: 0x000175C0 File Offset: 0x000157C0
		private void ScoreColumnWidthRatioUpdated()
		{
			this._scoreColumnItemDescription.WidthStretchRatio = this.ScoreColumnWidthRatio;
			base.AddItemDescription(this._scoreColumnItemDescription);
			base.SetMeasureAndLayoutDirty();
		}

		// Token: 0x06000817 RID: 2071 RVA: 0x000175E5 File Offset: 0x000157E5
		private void SoldiersColumnWidthRatioUpdated()
		{
			this._soldiersColumnItemDescription.WidthStretchRatio = this.SoldiersColumnWidthRatio;
			base.AddItemDescription(this._soldiersColumnItemDescription);
			base.SetMeasureAndLayoutDirty();
		}

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x06000818 RID: 2072 RVA: 0x0001760A File Offset: 0x0001580A
		// (set) Token: 0x06000819 RID: 2073 RVA: 0x00017612 File Offset: 0x00015812
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
					this.NameColumnWidthRatioUpdated();
				}
			}
		}

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x0600081A RID: 2074 RVA: 0x00017636 File Offset: 0x00015836
		// (set) Token: 0x0600081B RID: 2075 RVA: 0x0001763E File Offset: 0x0001583E
		public float ScoreColumnWidthRatio
		{
			get
			{
				return this._scoreColumnWidthRatio;
			}
			set
			{
				if (value != this._scoreColumnWidthRatio)
				{
					this._scoreColumnWidthRatio = value;
					base.OnPropertyChanged(value, "ScoreColumnWidthRatio");
					this.ScoreColumnWidthRatioUpdated();
				}
			}
		}

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x0600081C RID: 2076 RVA: 0x00017662 File Offset: 0x00015862
		// (set) Token: 0x0600081D RID: 2077 RVA: 0x0001766A File Offset: 0x0001586A
		public float SoldiersColumnWidthRatio
		{
			get
			{
				return this._soldiersColumnWidthRatio;
			}
			set
			{
				if (value != this._soldiersColumnWidthRatio)
				{
					this._soldiersColumnWidthRatio = value;
					base.OnPropertyChanged(value, "SoldiersColumnWidthRatio");
					this.SoldiersColumnWidthRatioUpdated();
				}
			}
		}

		// Token: 0x0400039A RID: 922
		private ContainerItemDescription _nameColumnItemDescription;

		// Token: 0x0400039B RID: 923
		private ContainerItemDescription _scoreColumnItemDescription;

		// Token: 0x0400039C RID: 924
		private ContainerItemDescription _soldiersColumnItemDescription;

		// Token: 0x0400039D RID: 925
		private const string _nameColumnWidgetID = "name";

		// Token: 0x0400039E RID: 926
		private const string _scoreColumnWidgetID = "score";

		// Token: 0x0400039F RID: 927
		private const string _soldiersColumnWidgetID = "soldiers";

		// Token: 0x040003A0 RID: 928
		private float _nameColumnWidthRatio = 1f;

		// Token: 0x040003A1 RID: 929
		private float _scoreColumnWidthRatio = 1f;

		// Token: 0x040003A2 RID: 930
		private float _soldiersColumnWidthRatio = 1f;
	}
}
