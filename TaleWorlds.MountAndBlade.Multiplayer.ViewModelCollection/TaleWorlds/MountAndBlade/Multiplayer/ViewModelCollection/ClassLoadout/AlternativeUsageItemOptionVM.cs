using System;
using TaleWorlds.Core.ViewModelCollection.Selector;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.ClassLoadout
{
	// Token: 0x020000A1 RID: 161
	public class AlternativeUsageItemOptionVM : SelectorItemVM
	{
		// Token: 0x06000F6D RID: 3949 RVA: 0x0002FA12 File Offset: 0x0002DC12
		public AlternativeUsageItemOptionVM(string usageType, TextObject s, TextObject hint, SelectorVM<AlternativeUsageItemOptionVM> parentSelector, int index)
			: base(s, hint)
		{
			this.UsageType = usageType;
			this._index = index;
			this._parentSelector = parentSelector;
		}

		// Token: 0x06000F6E RID: 3950 RVA: 0x0002FA33 File Offset: 0x0002DC33
		private void ExecuteSelection()
		{
			this._parentSelector.SelectedIndex = this._index;
		}

		// Token: 0x17000526 RID: 1318
		// (get) Token: 0x06000F6F RID: 3951 RVA: 0x0002FA46 File Offset: 0x0002DC46
		// (set) Token: 0x06000F70 RID: 3952 RVA: 0x0002FA4E File Offset: 0x0002DC4E
		[DataSourceProperty]
		public string UsageType
		{
			get
			{
				return this._usageType;
			}
			set
			{
				if (value != this._usageType)
				{
					this._usageType = value;
					base.OnPropertyChangedWithValue<string>(value, "UsageType");
				}
			}
		}

		// Token: 0x04000728 RID: 1832
		private int _index;

		// Token: 0x04000729 RID: 1833
		private SelectorVM<AlternativeUsageItemOptionVM> _parentSelector;

		// Token: 0x0400072A RID: 1834
		private string _usageType;
	}
}
