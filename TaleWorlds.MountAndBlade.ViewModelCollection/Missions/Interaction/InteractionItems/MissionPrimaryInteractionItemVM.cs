using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Missions.Interaction.InteractionItems
{
	// Token: 0x02000044 RID: 68
	public class MissionPrimaryInteractionItemVM : MissionGenericInteractionItemVM
	{
		// Token: 0x060005DA RID: 1498 RVA: 0x00016312 File Offset: 0x00014512
		protected override void OnResetData()
		{
			base.OnResetData();
			this.FocusTypeString = string.Empty;
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x060005DB RID: 1499 RVA: 0x00016325 File Offset: 0x00014525
		// (set) Token: 0x060005DC RID: 1500 RVA: 0x0001632D File Offset: 0x0001452D
		[DataSourceProperty]
		public string FocusTypeString
		{
			get
			{
				return this._focusTypeString;
			}
			set
			{
				if (value != this._focusTypeString)
				{
					this._focusTypeString = value;
					base.OnPropertyChangedWithValue<string>(value, "FocusTypeString");
				}
			}
		}

		// Token: 0x0400029F RID: 671
		private string _focusTypeString;
	}
}
