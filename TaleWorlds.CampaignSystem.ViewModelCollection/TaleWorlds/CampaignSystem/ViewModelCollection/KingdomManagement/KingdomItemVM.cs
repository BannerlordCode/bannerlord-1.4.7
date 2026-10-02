using System;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement
{
	// Token: 0x02000064 RID: 100
	public abstract class KingdomItemVM : ViewModel
	{
		// Token: 0x06000790 RID: 1936 RVA: 0x00023A16 File Offset: 0x00021C16
		protected virtual void OnSelect()
		{
			this.IsSelected = true;
		}

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x06000791 RID: 1937 RVA: 0x00023A1F File Offset: 0x00021C1F
		// (set) Token: 0x06000792 RID: 1938 RVA: 0x00023A27 File Offset: 0x00021C27
		[DataSourceProperty]
		public bool IsNew
		{
			get
			{
				return this._isNew;
			}
			set
			{
				if (value != this._isNew)
				{
					this._isNew = value;
					base.OnPropertyChangedWithValue(value, "IsNew");
				}
			}
		}

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x06000793 RID: 1939 RVA: 0x00023A45 File Offset: 0x00021C45
		// (set) Token: 0x06000794 RID: 1940 RVA: 0x00023A4D File Offset: 0x00021C4D
		[DataSourceProperty]
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				if (value != this._isSelected)
				{
					this._isSelected = value;
					base.OnPropertyChangedWithValue(value, "IsSelected");
				}
			}
		}

		// Token: 0x0400034B RID: 843
		private bool _isSelected;

		// Token: 0x0400034C RID: 844
		private bool _isNew;
	}
}
