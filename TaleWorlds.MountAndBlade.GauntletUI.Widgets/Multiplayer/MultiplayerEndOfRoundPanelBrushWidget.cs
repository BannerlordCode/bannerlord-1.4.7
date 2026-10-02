using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer
{
	// Token: 0x02000089 RID: 137
	public class MultiplayerEndOfRoundPanelBrushWidget : BrushWidget
	{
		// Token: 0x0600079D RID: 1949 RVA: 0x000162E3 File Offset: 0x000144E3
		public MultiplayerEndOfRoundPanelBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600079E RID: 1950 RVA: 0x000162EC File Offset: 0x000144EC
		private void IsShownUpdated()
		{
			if (this.IsShown)
			{
				string text = (this.IsRoundWinner ? "Victory" : "Defeat");
				base.EventFired(text, Array.Empty<object>());
			}
		}

		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x0600079F RID: 1951 RVA: 0x00016322 File Offset: 0x00014522
		// (set) Token: 0x060007A0 RID: 1952 RVA: 0x0001632A File Offset: 0x0001452A
		[DataSourceProperty]
		public bool IsShown
		{
			get
			{
				return this._isShown;
			}
			set
			{
				if (value != this._isShown)
				{
					this._isShown = value;
					base.OnPropertyChanged(value, "IsShown");
					this.IsShownUpdated();
				}
			}
		}

		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x060007A1 RID: 1953 RVA: 0x0001634E File Offset: 0x0001454E
		// (set) Token: 0x060007A2 RID: 1954 RVA: 0x00016356 File Offset: 0x00014556
		[DataSourceProperty]
		public bool IsRoundWinner
		{
			get
			{
				return this._isRoundWinner;
			}
			set
			{
				if (value != this._isRoundWinner)
				{
					this._isRoundWinner = value;
					base.OnPropertyChanged(value, "IsRoundWinner");
				}
			}
		}

		// Token: 0x04000358 RID: 856
		private bool _isShown;

		// Token: 0x04000359 RID: 857
		private bool _isRoundWinner;
	}
}
