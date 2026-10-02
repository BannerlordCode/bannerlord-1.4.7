using System;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.GameOptions
{
	// Token: 0x0200006F RID: 111
	public abstract class KeyOptionVM : ViewModel
	{
		// Token: 0x17000299 RID: 665
		// (get) Token: 0x060008B1 RID: 2225 RVA: 0x0001D897 File Offset: 0x0001BA97
		// (set) Token: 0x060008B2 RID: 2226 RVA: 0x0001D89F File Offset: 0x0001BA9F
		public Key CurrentKey
		{
			get
			{
				return this._currentKey;
			}
			protected set
			{
				this._currentKey = value;
				this.UpdateIsChanged();
			}
		}

		// Token: 0x1700029A RID: 666
		// (get) Token: 0x060008B3 RID: 2227 RVA: 0x0001D8AE File Offset: 0x0001BAAE
		// (set) Token: 0x060008B4 RID: 2228 RVA: 0x0001D8B6 File Offset: 0x0001BAB6
		public Key Key
		{
			get
			{
				return this._key;
			}
			protected set
			{
				this._key = value;
				this.UpdateIsChanged();
			}
		}

		// Token: 0x060008B5 RID: 2229 RVA: 0x0001D8C5 File Offset: 0x0001BAC5
		public KeyOptionVM(string groupId, string id, Action<KeyOptionVM> onKeybindRequest)
		{
			this._groupId = groupId;
			this._id = id;
			this._onKeybindRequest = onKeybindRequest;
			this.RevertHint = new HintViewModel(new TextObject("{=ftM2TjQ5}Revert changes", null), null);
		}

		// Token: 0x060008B6 RID: 2230
		public abstract void Set(InputKey newKey);

		// Token: 0x060008B7 RID: 2231
		public abstract void Update();

		// Token: 0x060008B8 RID: 2232
		public abstract void OnDone();

		// Token: 0x060008B9 RID: 2233
		public abstract void ExecuteRevert();

		// Token: 0x060008BA RID: 2234
		internal abstract void UpdateIsChanged();

		// Token: 0x1700029B RID: 667
		// (get) Token: 0x060008BB RID: 2235 RVA: 0x0001D8F9 File Offset: 0x0001BAF9
		// (set) Token: 0x060008BC RID: 2236 RVA: 0x0001D901 File Offset: 0x0001BB01
		[DataSourceProperty]
		public string OptionValueText
		{
			get
			{
				return this._optionValueText;
			}
			set
			{
				if (value != this._optionValueText)
				{
					this._optionValueText = value;
					base.OnPropertyChangedWithValue<string>(value, "OptionValueText");
				}
			}
		}

		// Token: 0x1700029C RID: 668
		// (get) Token: 0x060008BD RID: 2237 RVA: 0x0001D924 File Offset: 0x0001BB24
		// (set) Token: 0x060008BE RID: 2238 RVA: 0x0001D92C File Offset: 0x0001BB2C
		[DataSourceProperty]
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				if (value != this._name)
				{
					this._name = value;
					base.OnPropertyChangedWithValue<string>(value, "Name");
				}
			}
		}

		// Token: 0x1700029D RID: 669
		// (get) Token: 0x060008BF RID: 2239 RVA: 0x0001D94F File Offset: 0x0001BB4F
		// (set) Token: 0x060008C0 RID: 2240 RVA: 0x0001D957 File Offset: 0x0001BB57
		[DataSourceProperty]
		public string Description
		{
			get
			{
				return this._description;
			}
			set
			{
				if (value != this._description)
				{
					this._description = value;
					base.OnPropertyChangedWithValue<string>(value, "Description");
				}
			}
		}

		// Token: 0x1700029E RID: 670
		// (get) Token: 0x060008C1 RID: 2241 RVA: 0x0001D97A File Offset: 0x0001BB7A
		// (set) Token: 0x060008C2 RID: 2242 RVA: 0x0001D982 File Offset: 0x0001BB82
		[DataSourceProperty]
		public bool IsChanged
		{
			get
			{
				return this._isChanged;
			}
			set
			{
				if (value != this._isChanged)
				{
					this._isChanged = value;
					base.OnPropertyChangedWithValue(value, "IsChanged");
				}
			}
		}

		// Token: 0x1700029F RID: 671
		// (get) Token: 0x060008C3 RID: 2243 RVA: 0x0001D9A0 File Offset: 0x0001BBA0
		// (set) Token: 0x060008C4 RID: 2244 RVA: 0x0001D9A8 File Offset: 0x0001BBA8
		[DataSourceProperty]
		public HintViewModel RevertHint
		{
			get
			{
				return this._revertHint;
			}
			set
			{
				if (value != this._revertHint)
				{
					this._revertHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "RevertHint");
				}
			}
		}

		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x060008C5 RID: 2245 RVA: 0x0001D9C6 File Offset: 0x0001BBC6
		// (set) Token: 0x060008C6 RID: 2246 RVA: 0x0001D9CE File Offset: 0x0001BBCE
		[DataSourceProperty]
		public string ExtraInformationText
		{
			get
			{
				return this._extraInformationText;
			}
			set
			{
				if (value != this._extraInformationText)
				{
					this._extraInformationText = value;
					base.OnPropertyChangedWithValue<string>(value, "ExtraInformationText");
				}
			}
		}

		// Token: 0x040003D9 RID: 985
		private Key _currentKey;

		// Token: 0x040003DA RID: 986
		private Key _key;

		// Token: 0x040003DB RID: 987
		protected readonly string _groupId;

		// Token: 0x040003DC RID: 988
		protected readonly string _id;

		// Token: 0x040003DD RID: 989
		protected readonly Action<KeyOptionVM> _onKeybindRequest;

		// Token: 0x040003DE RID: 990
		private string _optionValueText;

		// Token: 0x040003DF RID: 991
		private string _name;

		// Token: 0x040003E0 RID: 992
		private string _description;

		// Token: 0x040003E1 RID: 993
		private string _extraInformationText;

		// Token: 0x040003E2 RID: 994
		private bool _isChanged;

		// Token: 0x040003E3 RID: 995
		private HintViewModel _revertHint;
	}
}
