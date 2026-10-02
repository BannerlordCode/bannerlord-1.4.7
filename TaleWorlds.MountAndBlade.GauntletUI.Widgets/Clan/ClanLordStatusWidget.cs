using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Clan
{
	// Token: 0x02000175 RID: 373
	public class ClanLordStatusWidget : Widget
	{
		// Token: 0x0600137E RID: 4990 RVA: 0x00034F31 File Offset: 0x00033131
		public ClanLordStatusWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600137F RID: 4991 RVA: 0x00034F44 File Offset: 0x00033144
		private void SetVisualState(int type)
		{
			switch (type)
			{
			case 0:
				this.SetState("Dead");
				return;
			case 1:
				this.SetState("Married");
				return;
			case 2:
				this.SetState("Pregnant");
				return;
			case 3:
				this.SetState("InBattle");
				return;
			case 4:
				this.SetState("InSiege");
				return;
			case 5:
				this.SetState("Child");
				return;
			case 6:
				this.SetState("Prisoner");
				return;
			case 7:
				this.SetState("Sick");
				return;
			default:
				return;
			}
		}

		// Token: 0x170006E9 RID: 1769
		// (get) Token: 0x06001380 RID: 4992 RVA: 0x00034FD7 File Offset: 0x000331D7
		// (set) Token: 0x06001381 RID: 4993 RVA: 0x00034FDF File Offset: 0x000331DF
		[Editor(false)]
		public int StatusType
		{
			get
			{
				return this._statusType;
			}
			set
			{
				if (this._statusType != value)
				{
					this._statusType = value;
					base.OnPropertyChanged(value, "StatusType");
					this.SetVisualState(value);
				}
			}
		}

		// Token: 0x040008D5 RID: 2261
		private int _statusType = -1;
	}
}
