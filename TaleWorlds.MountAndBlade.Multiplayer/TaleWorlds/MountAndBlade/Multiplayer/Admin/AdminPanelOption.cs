using System;
using System.Collections.Generic;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Multiplayer.Admin
{
	// Token: 0x02000075 RID: 117
	internal class AdminPanelOption<T> : IAdminPanelOptionInternal<T>, IAdminPanelOptionInternal, IAdminPanelOption<T>, IAdminPanelOption
	{
		// Token: 0x1700004E RID: 78
		// (get) Token: 0x0600037C RID: 892 RVA: 0x0000FB3B File Offset: 0x0000DD3B
		// (set) Token: 0x0600037D RID: 893 RVA: 0x0000FB43 File Offset: 0x0000DD43
		private protected T DefaultValue { protected get; private set; }

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x0600037E RID: 894 RVA: 0x0000FB4C File Offset: 0x0000DD4C
		// (set) Token: 0x0600037F RID: 895 RVA: 0x0000FB54 File Offset: 0x0000DD54
		private protected T InitialValue { protected get; private set; }

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000380 RID: 896 RVA: 0x0000FB5D File Offset: 0x0000DD5D
		// (set) Token: 0x06000381 RID: 897 RVA: 0x0000FB65 File Offset: 0x0000DD65
		private protected T CurrentValue { protected get; private set; }

		// Token: 0x06000382 RID: 898 RVA: 0x0000FB6E File Offset: 0x0000DD6E
		public AdminPanelOption(string uniqueId)
		{
			this._uniqueId = uniqueId;
			this._onValueChangedAdditionalCallbacks = new List<Action>();
			this._optionType = MultiplayerOptions.OptionType.NumOfSlots;
			this._accessMode = MultiplayerOptions.MultiplayerOptionsAccessMode.NumAccessModes;
		}

		// Token: 0x06000383 RID: 899 RVA: 0x0000FB97 File Offset: 0x0000DD97
		protected virtual void OnValueChanged(T previousValue, T newValue)
		{
			this.OnRefresh();
		}

		// Token: 0x06000384 RID: 900 RVA: 0x0000FB9F File Offset: 0x0000DD9F
		protected virtual bool OnGetCanRevertToDefaultValue()
		{
			return true;
		}

		// Token: 0x06000385 RID: 901 RVA: 0x0000FBA4 File Offset: 0x0000DDA4
		protected virtual T GetOptionValue(MultiplayerOptions.OptionType optionType, MultiplayerOptions.MultiplayerOptionsAccessMode accessMode = MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions)
		{
			switch (optionType.GetOptionProperty().OptionValueType)
			{
			case MultiplayerOptions.OptionValueType.Bool:
				return (T)((object)optionType.GetBoolValue(accessMode));
			case MultiplayerOptions.OptionValueType.Integer:
				return (T)((object)optionType.GetIntValue(accessMode));
			case MultiplayerOptions.OptionValueType.Enum:
				Debug.FailedAssert("Unsupported option value type", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Admin\\Internal\\AdminPanelOption.cs", "GetOptionValue", 63);
				break;
			case MultiplayerOptions.OptionValueType.String:
				return (T)((object)optionType.GetStrValue(accessMode));
			}
			return default(T);
		}

		// Token: 0x06000386 RID: 902 RVA: 0x0000FC28 File Offset: 0x0000DE28
		protected virtual bool AreEqualValues(T first, T second)
		{
			return (first == null && second == null) || ((first == null || second != null) && (first != null || second == null) && first.Equals(second));
		}

		// Token: 0x06000387 RID: 903 RVA: 0x0000FC7E File Offset: 0x0000DE7E
		public void AddValueChangedCallback(Action callback)
		{
			this._onValueChangedAdditionalCallbacks.Add(callback);
		}

		// Token: 0x06000388 RID: 904 RVA: 0x0000FC8C File Offset: 0x0000DE8C
		public void RemoveValueChangedCallback(Action callback)
		{
			this._onValueChangedAdditionalCallbacks.Remove(callback);
		}

		// Token: 0x06000389 RID: 905 RVA: 0x0000FC9B File Offset: 0x0000DE9B
		public virtual void OnFinalize()
		{
			this._onValueChangedAdditionalCallbacks.Clear();
			this._onRefresh = null;
			this._onApplied = null;
		}

		// Token: 0x0600038A RID: 906 RVA: 0x0000FCB6 File Offset: 0x0000DEB6
		protected virtual void OnRefresh()
		{
			Action onRefresh = this._onRefresh;
			if (onRefresh == null)
			{
				return;
			}
			onRefresh();
		}

		// Token: 0x0600038B RID: 907 RVA: 0x0000FCC8 File Offset: 0x0000DEC8
		public AdminPanelOption<T> BuildOptionType(MultiplayerOptions.OptionType optionType, MultiplayerOptions.MultiplayerOptionsAccessMode accessMode = MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions, bool buildDefaultValue = true, bool buildInitialValue = true)
		{
			this._optionType = optionType;
			this._accessMode = accessMode;
			if (buildDefaultValue)
			{
				T optionValue = this.GetOptionValue(optionType, MultiplayerOptions.MultiplayerOptionsAccessMode.DefaultMapOptions);
				this.BuildDefaultValue(optionValue);
			}
			if (buildInitialValue)
			{
				T optionValue2 = this.GetOptionValue(optionType, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
				this.BuildInitialValue(optionValue2);
			}
			return this;
		}

		// Token: 0x0600038C RID: 908 RVA: 0x0000FD0D File Offset: 0x0000DF0D
		public AdminPanelOption<T> BuildIsRequired(bool isRequired)
		{
			this._isRequired = isRequired;
			return this;
		}

		// Token: 0x0600038D RID: 909 RVA: 0x0000FD17 File Offset: 0x0000DF17
		public AdminPanelOption<T> BuildRequiresRestart(bool requiresRestart)
		{
			this._requiresRestart = requiresRestart;
			return this;
		}

		// Token: 0x0600038E RID: 910 RVA: 0x0000FD21 File Offset: 0x0000DF21
		public AdminPanelOption<T> BuildName(TextObject name)
		{
			this._nameTextObj = name;
			return this;
		}

		// Token: 0x0600038F RID: 911 RVA: 0x0000FD2B File Offset: 0x0000DF2B
		public AdminPanelOption<T> BuildDescription(TextObject description)
		{
			this._descriptionTextObj = description;
			return this;
		}

		// Token: 0x06000390 RID: 912 RVA: 0x0000FD35 File Offset: 0x0000DF35
		public AdminPanelOption<T> BuildInitialValue(T value)
		{
			this.InitialValue = value;
			this.SetValue(this.InitialValue);
			return this;
		}

		// Token: 0x06000391 RID: 913 RVA: 0x0000FD4B File Offset: 0x0000DF4B
		public AdminPanelOption<T> BuildDefaultValue(T value)
		{
			this.DefaultValue = value;
			this.SetValue(this.DefaultValue);
			return this;
		}

		// Token: 0x06000392 RID: 914 RVA: 0x0000FD61 File Offset: 0x0000DF61
		public AdminPanelOption<T> BuildOnAppliedCallback(Action<T> onApplied)
		{
			this._onApplied = onApplied;
			return this;
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000393 RID: 915 RVA: 0x0000FD6B File Offset: 0x0000DF6B
		public string UniqueId
		{
			get
			{
				return this._uniqueId;
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000394 RID: 916 RVA: 0x0000FD73 File Offset: 0x0000DF73
		public bool IsRequired
		{
			get
			{
				return this._isRequired;
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000395 RID: 917 RVA: 0x0000FD7B File Offset: 0x0000DF7B
		public bool RequiresMissionRestart
		{
			get
			{
				return this._requiresRestart;
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000396 RID: 918 RVA: 0x0000FD83 File Offset: 0x0000DF83
		public bool IsDirty
		{
			get
			{
				return !this.AreEqualValues(this.InitialValue, this.CurrentValue);
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000397 RID: 919 RVA: 0x0000FD9A File Offset: 0x0000DF9A
		public bool CanRevertToDefaultValue
		{
			get
			{
				return !this.AreEqualValues(this.DefaultValue, this.CurrentValue) && this.OnGetCanRevertToDefaultValue();
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000398 RID: 920 RVA: 0x0000FDB8 File Offset: 0x0000DFB8
		public string Name
		{
			get
			{
				TextObject nameTextObj = this._nameTextObj;
				return ((nameTextObj != null) ? nameTextObj.ToString() : null) ?? string.Empty;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000399 RID: 921 RVA: 0x0000FDD5 File Offset: 0x0000DFD5
		public string Description
		{
			get
			{
				TextObject descriptionTextObj = this._descriptionTextObj;
				return ((descriptionTextObj != null) ? descriptionTextObj.ToString() : null) ?? string.Empty;
			}
		}

		// Token: 0x0600039A RID: 922 RVA: 0x0000FDF2 File Offset: 0x0000DFF2
		public T GetValue()
		{
			return this.CurrentValue;
		}

		// Token: 0x0600039B RID: 923 RVA: 0x0000FDFC File Offset: 0x0000DFFC
		public void SetValue(T value)
		{
			T currentValue = this.CurrentValue;
			this.CurrentValue = value;
			if (!this.AreEqualValues(currentValue, this.CurrentValue))
			{
				this.OnValueChanged(currentValue, this.CurrentValue);
			}
			for (int i = 0; i < this._onValueChangedAdditionalCallbacks.Count; i++)
			{
				Action action = this._onValueChangedAdditionalCallbacks[i];
				if (action != null)
				{
					action();
				}
			}
		}

		// Token: 0x0600039C RID: 924 RVA: 0x0000FE60 File Offset: 0x0000E060
		public virtual bool GetIsAvailable()
		{
			return true;
		}

		// Token: 0x0600039D RID: 925 RVA: 0x0000FE63 File Offset: 0x0000E063
		public void OnApplyChanges()
		{
			this.InitialValue = this.CurrentValue;
			Action<T> onApplied = this._onApplied;
			if (onApplied == null)
			{
				return;
			}
			onApplied(this.CurrentValue);
		}

		// Token: 0x0600039E RID: 926 RVA: 0x0000FE87 File Offset: 0x0000E087
		public void RevertChanges()
		{
			this.SetValue(this.InitialValue);
		}

		// Token: 0x0600039F RID: 927 RVA: 0x0000FE95 File Offset: 0x0000E095
		public void RestoreDefaults()
		{
			this.SetValue(this.DefaultValue);
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x0000FEA3 File Offset: 0x0000E0A3
		public void SetOnRefreshCallback(Action callback)
		{
			this._onRefresh = callback;
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x0000FEAC File Offset: 0x0000E0AC
		public virtual bool GetIsDisabled(out string reason)
		{
			reason = string.Empty;
			return false;
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x0000FEB6 File Offset: 0x0000E0B6
		public MultiplayerOptions.OptionType GetOptionType()
		{
			return this._optionType;
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x0000FEBE File Offset: 0x0000E0BE
		public MultiplayerOptions.MultiplayerOptionsAccessMode GetOptionAccessMode()
		{
			return this._accessMode;
		}

		// Token: 0x04000103 RID: 259
		private MultiplayerOptions.OptionType _optionType;

		// Token: 0x04000104 RID: 260
		private MultiplayerOptions.MultiplayerOptionsAccessMode _accessMode;

		// Token: 0x04000108 RID: 264
		private readonly string _uniqueId;

		// Token: 0x04000109 RID: 265
		private bool _isRequired;

		// Token: 0x0400010A RID: 266
		private bool _requiresRestart;

		// Token: 0x0400010B RID: 267
		private Action _onRefresh;

		// Token: 0x0400010C RID: 268
		private List<Action> _onValueChangedAdditionalCallbacks;

		// Token: 0x0400010D RID: 269
		private Action<T> _onApplied;

		// Token: 0x0400010E RID: 270
		private TextObject _nameTextObj;

		// Token: 0x0400010F RID: 271
		private TextObject _descriptionTextObj;
	}
}
