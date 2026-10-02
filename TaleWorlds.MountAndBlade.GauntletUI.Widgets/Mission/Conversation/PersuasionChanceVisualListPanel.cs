using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission.Conversation
{
	// Token: 0x02000104 RID: 260
	public class PersuasionChanceVisualListPanel : ListPanel
	{
		// Token: 0x170004F6 RID: 1270
		// (get) Token: 0x06000DE7 RID: 3559 RVA: 0x00026248 File Offset: 0x00024448
		// (set) Token: 0x06000DE8 RID: 3560 RVA: 0x00026250 File Offset: 0x00024450
		public bool IsFailChance { get; set; }

		// Token: 0x06000DE9 RID: 3561 RVA: 0x00026259 File Offset: 0x00024459
		public PersuasionChanceVisualListPanel(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000DEA RID: 3562 RVA: 0x00026262 File Offset: 0x00024462
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			base.IsVisible = !this.IsFailChance && this.ChanceValue > 0;
		}

		// Token: 0x170004F7 RID: 1271
		// (get) Token: 0x06000DEB RID: 3563 RVA: 0x00026285 File Offset: 0x00024485
		// (set) Token: 0x06000DEC RID: 3564 RVA: 0x0002628D File Offset: 0x0002448D
		public int ChanceValue
		{
			get
			{
				return this._chanceValue;
			}
			set
			{
				if (this._chanceValue != value)
				{
					this._chanceValue = value;
					base.OnPropertyChanged(value, "ChanceValue");
				}
			}
		}

		// Token: 0x04000650 RID: 1616
		private int _chanceValue;
	}
}
