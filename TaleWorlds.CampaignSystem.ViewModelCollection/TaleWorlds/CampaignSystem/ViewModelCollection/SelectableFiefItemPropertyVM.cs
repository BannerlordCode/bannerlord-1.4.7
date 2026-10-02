using System;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection
{
	// Token: 0x0200001D RID: 29
	public class SelectableFiefItemPropertyVM : SelectableItemPropertyVM
	{
		// Token: 0x060001C2 RID: 450 RVA: 0x0000C763 File Offset: 0x0000A963
		public SelectableFiefItemPropertyVM(string name, string value, int changeAmount, SelectableItemPropertyVM.PropertyType type, BasicTooltipViewModel hint = null, bool isWarning = false)
			: base(name, value, isWarning, hint)
		{
			this.ChangeAmount = changeAmount;
			base.Type = (int)type;
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060001C3 RID: 451 RVA: 0x0000C780 File Offset: 0x0000A980
		// (set) Token: 0x060001C4 RID: 452 RVA: 0x0000C788 File Offset: 0x0000A988
		[DataSourceProperty]
		public int ChangeAmount
		{
			get
			{
				return this._changeAmount;
			}
			set
			{
				if (value != this._changeAmount)
				{
					this._changeAmount = value;
					base.OnPropertyChangedWithValue(value, "ChangeAmount");
				}
			}
		}

		// Token: 0x040000D4 RID: 212
		private int _changeAmount;
	}
}
