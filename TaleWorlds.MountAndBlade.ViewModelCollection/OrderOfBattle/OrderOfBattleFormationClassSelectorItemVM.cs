using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Core.ViewModelCollection.Selector;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.OrderOfBattle
{
	// Token: 0x0200002F RID: 47
	public class OrderOfBattleFormationClassSelectorItemVM : SelectorItemVM
	{
		// Token: 0x0600038B RID: 907 RVA: 0x0000D5C8 File Offset: 0x0000B7C8
		public OrderOfBattleFormationClassSelectorItemVM(DeploymentFormationClass formationClass)
			: base(formationClass.ToString())
		{
			this.FormationClass = formationClass;
			this.FormationClassInt = (int)formationClass;
			this.RefreshValues();
		}

		// Token: 0x0600038C RID: 908 RVA: 0x0000D5F1 File Offset: 0x0000B7F1
		public override void RefreshValues()
		{
			base.Hint = new HintViewModel(this.FormationClass.GetClassName(), null);
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x0600038D RID: 909 RVA: 0x0000D60A File Offset: 0x0000B80A
		// (set) Token: 0x0600038E RID: 910 RVA: 0x0000D612 File Offset: 0x0000B812
		[DataSourceProperty]
		public int FormationClassInt
		{
			get
			{
				return this._formationClassInt;
			}
			set
			{
				if (value != this._formationClassInt)
				{
					this._formationClassInt = value;
					base.OnPropertyChangedWithValue(value, "FormationClassInt");
				}
			}
		}

		// Token: 0x0400018C RID: 396
		public readonly DeploymentFormationClass FormationClass;

		// Token: 0x0400018D RID: 397
		private int _formationClassInt;
	}
}
