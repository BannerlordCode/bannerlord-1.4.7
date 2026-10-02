using System;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Multiplayer.Admin.Internal
{
	// Token: 0x02000077 RID: 119
	internal class AdminPanelMultiSelectionItem : IAdminPanelMultiSelectionItem
	{
		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060003AE RID: 942 RVA: 0x00010013 File Offset: 0x0000E213
		public string Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060003AF RID: 943 RVA: 0x0001001B File Offset: 0x0000E21B
		public string DisplayName
		{
			get
			{
				string displayName = this._displayName;
				if (displayName == null)
				{
					return null;
				}
				return displayName.ToString();
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060003B0 RID: 944 RVA: 0x0001002E File Offset: 0x0000E22E
		public bool IsFallbackValue
		{
			get
			{
				return this._isFallbackValue;
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060003B1 RID: 945 RVA: 0x00010036 File Offset: 0x0000E236
		public bool IsDisabled
		{
			get
			{
				return this._isDisabled;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060003B2 RID: 946 RVA: 0x0001003E File Offset: 0x0000E23E
		public bool CanBeApplied
		{
			get
			{
				return this._canBeApplied;
			}
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x00010046 File Offset: 0x0000E246
		public AdminPanelMultiSelectionItem(string value, TextObject displayName, bool isFallbackValue = false, bool isDisabled = false, bool canBeApplied = true)
		{
			this._value = value;
			this._displayName = ((displayName != null) ? displayName.ToString() : null);
			this._isFallbackValue = isFallbackValue;
			this._isDisabled = isDisabled;
			this._canBeApplied = canBeApplied;
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x0001007E File Offset: 0x0000E27E
		public void SetIsFallbackValue(bool value)
		{
			this._isFallbackValue = value;
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x00010087 File Offset: 0x0000E287
		public void SetIsDisabled(bool value)
		{
			this._isDisabled = value;
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x00010090 File Offset: 0x0000E290
		public void SetCanBeApplied(bool value)
		{
			this._canBeApplied = value;
		}

		// Token: 0x04000116 RID: 278
		private string _value;

		// Token: 0x04000117 RID: 279
		private string _displayName;

		// Token: 0x04000118 RID: 280
		private bool _isFallbackValue;

		// Token: 0x04000119 RID: 281
		private bool _isDisabled;

		// Token: 0x0400011A RID: 282
		private bool _canBeApplied;
	}
}
