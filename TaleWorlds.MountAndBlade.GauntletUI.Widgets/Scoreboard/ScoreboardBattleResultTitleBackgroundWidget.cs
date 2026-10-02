using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Scoreboard
{
	// Token: 0x02000054 RID: 84
	public class ScoreboardBattleResultTitleBackgroundWidget : Widget
	{
		// Token: 0x06000497 RID: 1175 RVA: 0x0000E7DF File Offset: 0x0000C9DF
		public ScoreboardBattleResultTitleBackgroundWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x0000E7E8 File Offset: 0x0000C9E8
		private void BattleResultUpdated()
		{
			if (this.DefeatWidget != null)
			{
				this.DefeatWidget.IsVisible = this.BattleResult == 0;
			}
			if (this.VictoryWidget != null)
			{
				this.VictoryWidget.IsVisible = this.BattleResult == 1;
			}
			if (this.RetreatWidget != null)
			{
				this.RetreatWidget.IsVisible = this.BattleResult == 2;
			}
		}

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x06000499 RID: 1177 RVA: 0x0000E849 File Offset: 0x0000CA49
		// (set) Token: 0x0600049A RID: 1178 RVA: 0x0000E851 File Offset: 0x0000CA51
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

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x0600049B RID: 1179 RVA: 0x0000E875 File Offset: 0x0000CA75
		// (set) Token: 0x0600049C RID: 1180 RVA: 0x0000E87D File Offset: 0x0000CA7D
		[Editor(false)]
		public Widget VictoryWidget
		{
			get
			{
				return this._victoryWidget;
			}
			set
			{
				if (this._victoryWidget != value)
				{
					this._victoryWidget = value;
					base.OnPropertyChanged<Widget>(value, "VictoryWidget");
					this.BattleResultUpdated();
				}
			}
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x0600049D RID: 1181 RVA: 0x0000E8A1 File Offset: 0x0000CAA1
		// (set) Token: 0x0600049E RID: 1182 RVA: 0x0000E8A9 File Offset: 0x0000CAA9
		[Editor(false)]
		public Widget DefeatWidget
		{
			get
			{
				return this._defeatWidget;
			}
			set
			{
				if (this._defeatWidget != value)
				{
					this._defeatWidget = value;
					base.OnPropertyChanged<Widget>(value, "DefeatWidget");
					this.BattleResultUpdated();
				}
			}
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x0600049F RID: 1183 RVA: 0x0000E8CD File Offset: 0x0000CACD
		// (set) Token: 0x060004A0 RID: 1184 RVA: 0x0000E8D5 File Offset: 0x0000CAD5
		[Editor(false)]
		public Widget RetreatWidget
		{
			get
			{
				return this._retreatWidget;
			}
			set
			{
				if (this._retreatWidget != value)
				{
					this._retreatWidget = value;
					base.OnPropertyChanged<Widget>(value, "RetreatWidget");
					this.BattleResultUpdated();
				}
			}
		}

		// Token: 0x040001F7 RID: 503
		private int _battleResult;

		// Token: 0x040001F8 RID: 504
		private Widget _victoryWidget;

		// Token: 0x040001F9 RID: 505
		private Widget _defeatWidget;

		// Token: 0x040001FA RID: 506
		private Widget _retreatWidget;
	}
}
