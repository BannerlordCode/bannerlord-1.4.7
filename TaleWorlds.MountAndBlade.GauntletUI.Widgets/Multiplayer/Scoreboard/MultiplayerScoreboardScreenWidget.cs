using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Scoreboard
{
	// Token: 0x02000093 RID: 147
	public class MultiplayerScoreboardScreenWidget : Widget
	{
		// Token: 0x060007FD RID: 2045 RVA: 0x000172F3 File Offset: 0x000154F3
		public MultiplayerScoreboardScreenWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060007FE RID: 2046 RVA: 0x000172FC File Offset: 0x000154FC
		private void UpdateSidesList()
		{
			if (this.SidesList == null)
			{
				return;
			}
			base.SuggestedWidth = (float)(this.IsSingleSide ? this.SingleColumnedWidth : this.DoubleColumnedWidth);
		}

		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x060007FF RID: 2047 RVA: 0x00017324 File Offset: 0x00015524
		// (set) Token: 0x06000800 RID: 2048 RVA: 0x0001732C File Offset: 0x0001552C
		[DataSourceProperty]
		public bool IsSingleSide
		{
			get
			{
				return this._isSingleSide;
			}
			set
			{
				if (value != this._isSingleSide)
				{
					this._isSingleSide = value;
					base.OnPropertyChanged(value, "IsSingleSide");
					this.UpdateSidesList();
				}
			}
		}

		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x06000801 RID: 2049 RVA: 0x00017350 File Offset: 0x00015550
		// (set) Token: 0x06000802 RID: 2050 RVA: 0x00017358 File Offset: 0x00015558
		[DataSourceProperty]
		public int SingleColumnedWidth
		{
			get
			{
				return this._singleColumnedWidth;
			}
			set
			{
				if (value != this._singleColumnedWidth)
				{
					this._singleColumnedWidth = value;
					base.OnPropertyChanged(value, "SingleColumnedWidth");
				}
			}
		}

		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x06000803 RID: 2051 RVA: 0x00017376 File Offset: 0x00015576
		// (set) Token: 0x06000804 RID: 2052 RVA: 0x0001737E File Offset: 0x0001557E
		[DataSourceProperty]
		public int DoubleColumnedWidth
		{
			get
			{
				return this._doubleColumnedWidth;
			}
			set
			{
				if (value != this._doubleColumnedWidth)
				{
					this._doubleColumnedWidth = value;
					base.OnPropertyChanged(value, "DoubleColumnedWidth");
				}
			}
		}

		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x06000805 RID: 2053 RVA: 0x0001739C File Offset: 0x0001559C
		// (set) Token: 0x06000806 RID: 2054 RVA: 0x000173A4 File Offset: 0x000155A4
		[DataSourceProperty]
		public ListPanel SidesList
		{
			get
			{
				return this._sidesList;
			}
			set
			{
				if (value != this._sidesList)
				{
					this._sidesList = value;
					base.OnPropertyChanged<ListPanel>(value, "SidesList");
					this.UpdateSidesList();
				}
			}
		}

		// Token: 0x04000390 RID: 912
		private bool _isSingleSide;

		// Token: 0x04000391 RID: 913
		private int _singleColumnedWidth;

		// Token: 0x04000392 RID: 914
		private int _doubleColumnedWidth;

		// Token: 0x04000393 RID: 915
		private ListPanel _sidesList;
	}
}
