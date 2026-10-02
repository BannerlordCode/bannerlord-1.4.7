using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Order
{
	// Token: 0x02000023 RID: 35
	public class OrderTroopItemFormationClassVM : ViewModel
	{
		// Token: 0x06000322 RID: 802 RVA: 0x0000C555 File Offset: 0x0000A755
		public OrderTroopItemFormationClassVM(Formation formation, FormationClass formationClass)
		{
			this._formation = formation;
			this.FormationClass = formationClass;
			this.FormationClassValue = (int)formationClass;
		}

		// Token: 0x06000323 RID: 803 RVA: 0x0000C574 File Offset: 0x0000A774
		public void UpdateTroopCount()
		{
			switch (this.FormationClass)
			{
			case FormationClass.Infantry:
				this.TroopCount = this._formation.GetCountOfUnitsBelongingToLogicalClass(FormationClass.Infantry);
				return;
			case FormationClass.Ranged:
				this.TroopCount = this._formation.GetCountOfUnitsBelongingToLogicalClass(FormationClass.Ranged);
				return;
			case FormationClass.Cavalry:
				this.TroopCount = this._formation.GetCountOfUnitsBelongingToLogicalClass(FormationClass.Cavalry);
				return;
			case FormationClass.HorseArcher:
				this.TroopCount = this._formation.GetCountOfUnitsBelongingToLogicalClass(FormationClass.HorseArcher);
				return;
			default:
				return;
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x06000324 RID: 804 RVA: 0x0000C5EA File Offset: 0x0000A7EA
		// (set) Token: 0x06000325 RID: 805 RVA: 0x0000C5F2 File Offset: 0x0000A7F2
		[DataSourceProperty]
		public int FormationClassValue
		{
			get
			{
				return this._formationClassValue;
			}
			set
			{
				if (value != this._formationClassValue)
				{
					this._formationClassValue = value;
					base.OnPropertyChangedWithValue(value, "FormationClassValue");
				}
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x06000326 RID: 806 RVA: 0x0000C610 File Offset: 0x0000A810
		// (set) Token: 0x06000327 RID: 807 RVA: 0x0000C618 File Offset: 0x0000A818
		[DataSourceProperty]
		public int TroopCount
		{
			get
			{
				return this._troopCount;
			}
			set
			{
				if (value != this._troopCount)
				{
					this._troopCount = value;
					base.OnPropertyChangedWithValue(value, "TroopCount");
				}
			}
		}

		// Token: 0x0400015B RID: 347
		public readonly FormationClass FormationClass;

		// Token: 0x0400015C RID: 348
		private readonly Formation _formation;

		// Token: 0x0400015D RID: 349
		private int _formationClassValue;

		// Token: 0x0400015E RID: 350
		private int _troopCount;
	}
}
