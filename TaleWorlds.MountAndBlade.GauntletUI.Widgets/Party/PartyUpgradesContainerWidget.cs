using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Party
{
	// Token: 0x0200006F RID: 111
	public class PartyUpgradesContainerWidget : Widget
	{
		// Token: 0x06000605 RID: 1541 RVA: 0x00011D94 File Offset: 0x0000FF94
		public PartyUpgradesContainerWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000606 RID: 1542 RVA: 0x00011DA4 File Offset: 0x0000FFA4
		private void OnAnyUpgradeHasRequirementChanged(bool value)
		{
			base.ScaledPositionYOffset = (value ? 0f : 8f);
		}

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x06000607 RID: 1543 RVA: 0x00011DBB File Offset: 0x0000FFBB
		// (set) Token: 0x06000608 RID: 1544 RVA: 0x00011DC3 File Offset: 0x0000FFC3
		[Editor(false)]
		public bool AnyUpgradeHasRequirement
		{
			get
			{
				return this._anyUpgradeHasRequirement;
			}
			set
			{
				if (this._anyUpgradeHasRequirement != value)
				{
					this._anyUpgradeHasRequirement = value;
					this.OnAnyUpgradeHasRequirementChanged(value);
					base.OnPropertyChanged(value, "AnyUpgradeHasRequirement");
				}
			}
		}

		// Token: 0x04000297 RID: 663
		private const float _noRequirementOffset = 8f;

		// Token: 0x04000298 RID: 664
		private bool _anyUpgradeHasRequirement = true;
	}
}
