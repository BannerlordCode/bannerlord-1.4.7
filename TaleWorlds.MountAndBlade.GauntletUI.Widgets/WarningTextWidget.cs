using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x02000047 RID: 71
	public class WarningTextWidget : TextWidget
	{
		// Token: 0x060003FA RID: 1018 RVA: 0x0000C805 File Offset: 0x0000AA05
		public WarningTextWidget(UIContext context)
			: base(context)
		{
			base.UseGlobalTimeForAnimation = true;
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x060003FB RID: 1019 RVA: 0x0000C815 File Offset: 0x0000AA15
		// (set) Token: 0x060003FC RID: 1020 RVA: 0x0000C81D File Offset: 0x0000AA1D
		[Editor(false)]
		public bool IsWarned
		{
			get
			{
				return this._isWarned;
			}
			set
			{
				if (this._isWarned != value)
				{
					this._isWarned = value;
					base.OnPropertyChanged(value, "IsWarned");
					this.SetState(this._isWarned ? "Warned" : "Default");
				}
			}
		}

		// Token: 0x040001A7 RID: 423
		private bool _isWarned;
	}
}
