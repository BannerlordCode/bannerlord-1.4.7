using System;
using TaleWorlds.GauntletUI;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Encyclopedia
{
	// Token: 0x02000156 RID: 342
	public class EncyclopediaCharacterTableauWidget : CharacterTableauWidget
	{
		// Token: 0x0600123F RID: 4671 RVA: 0x00032564 File Offset: 0x00030764
		public EncyclopediaCharacterTableauWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06001240 RID: 4672 RVA: 0x0003256D File Offset: 0x0003076D
		private void UpdateVisual(bool isDead)
		{
			base.Brush.SaturationFactor = (float)(isDead ? (-100) : 0);
		}

		// Token: 0x17000677 RID: 1655
		// (get) Token: 0x06001241 RID: 4673 RVA: 0x00032583 File Offset: 0x00030783
		// (set) Token: 0x06001242 RID: 4674 RVA: 0x0003258B File Offset: 0x0003078B
		[Editor(false)]
		public bool IsDead
		{
			get
			{
				return this._isDead;
			}
			set
			{
				if (this._isDead != value)
				{
					this._isDead = value;
					base.OnPropertyChanged(value, "IsDead");
					this.UpdateVisual(value);
				}
			}
		}

		// Token: 0x0400084F RID: 2127
		private bool _isDead;
	}
}
