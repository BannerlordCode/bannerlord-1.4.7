using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer
{
	// Token: 0x02000087 RID: 135
	public class MultiplayerBattleResultColorizedWidget : Widget
	{
		// Token: 0x0600078D RID: 1933 RVA: 0x000160D3 File Offset: 0x000142D3
		public MultiplayerBattleResultColorizedWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600078E RID: 1934 RVA: 0x000160E4 File Offset: 0x000142E4
		private void BattleResultUpdated()
		{
			if (this.BattleResult == 2)
			{
				base.Color = this.DrawColor;
				return;
			}
			if (this.BattleResult == 1)
			{
				base.Color = this.VictoryColor;
				return;
			}
			if (this.BattleResult == 0)
			{
				base.Color = this.DefeatColor;
			}
		}

		// Token: 0x170002AF RID: 687
		// (get) Token: 0x0600078F RID: 1935 RVA: 0x00016131 File Offset: 0x00014331
		// (set) Token: 0x06000790 RID: 1936 RVA: 0x00016139 File Offset: 0x00014339
		[Editor(false)]
		public int BattleResult
		{
			get
			{
				return this._battleResult;
			}
			set
			{
				if (this._battleResult != value)
				{
					this._battleResult = value;
					base.OnPropertyChanged(value, "BattleResult");
					this.BattleResultUpdated();
				}
			}
		}

		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x06000791 RID: 1937 RVA: 0x0001615D File Offset: 0x0001435D
		// (set) Token: 0x06000792 RID: 1938 RVA: 0x00016165 File Offset: 0x00014365
		[Editor(false)]
		public Color DrawColor
		{
			get
			{
				return this._drawColor;
			}
			set
			{
				if (this._drawColor != value)
				{
					this._drawColor = value;
					base.OnPropertyChanged(value, "DrawColor");
				}
			}
		}

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x06000793 RID: 1939 RVA: 0x00016188 File Offset: 0x00014388
		// (set) Token: 0x06000794 RID: 1940 RVA: 0x00016190 File Offset: 0x00014390
		[Editor(false)]
		public Color VictoryColor
		{
			get
			{
				return this._victoryColor;
			}
			set
			{
				if (this._victoryColor != value)
				{
					this._victoryColor = value;
					base.OnPropertyChanged(value, "VictoryColor");
				}
			}
		}

		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x06000795 RID: 1941 RVA: 0x000161B3 File Offset: 0x000143B3
		// (set) Token: 0x06000796 RID: 1942 RVA: 0x000161BB File Offset: 0x000143BB
		[Editor(false)]
		public Color DefeatColor
		{
			get
			{
				return this._defeatColor;
			}
			set
			{
				if (this._defeatColor != value)
				{
					this._defeatColor = value;
					base.OnPropertyChanged(value, "DefeatColor");
				}
			}
		}

		// Token: 0x0400034E RID: 846
		private int _battleResult = -1;

		// Token: 0x0400034F RID: 847
		private Color _drawColor;

		// Token: 0x04000350 RID: 848
		private Color _victoryColor;

		// Token: 0x04000351 RID: 849
		private Color _defeatColor;
	}
}
