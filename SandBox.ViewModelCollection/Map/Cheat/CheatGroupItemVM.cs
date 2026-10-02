using System;
using TaleWorlds.Localization;

namespace SandBox.ViewModelCollection.Map.Cheat
{
	// Token: 0x0200004F RID: 79
	public class CheatGroupItemVM : CheatItemBaseVM
	{
		// Token: 0x060004E7 RID: 1255 RVA: 0x00012CD3 File Offset: 0x00010ED3
		public CheatGroupItemVM(GameplayCheatGroup cheatGroup, Action<CheatGroupItemVM> onSelectCheatGroup)
		{
			this.CheatGroup = cheatGroup;
			this._onSelectCheatGroup = onSelectCheatGroup;
			this.RefreshValues();
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x00012CEF File Offset: 0x00010EEF
		public override void RefreshValues()
		{
			base.RefreshValues();
			TextObject name = this.CheatGroup.GetName();
			base.Name = ((name != null) ? name.ToString() : null);
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x00012D14 File Offset: 0x00010F14
		public override void ExecuteAction()
		{
			Action<CheatGroupItemVM> onSelectCheatGroup = this._onSelectCheatGroup;
			if (onSelectCheatGroup == null)
			{
				return;
			}
			onSelectCheatGroup(this);
		}

		// Token: 0x0400026C RID: 620
		public readonly GameplayCheatGroup CheatGroup;

		// Token: 0x0400026D RID: 621
		private readonly Action<CheatGroupItemVM> _onSelectCheatGroup;
	}
}
