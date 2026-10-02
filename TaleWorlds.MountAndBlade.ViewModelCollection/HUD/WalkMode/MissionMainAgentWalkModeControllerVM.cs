using System;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.HUD.WalkMode
{
	// Token: 0x0200005A RID: 90
	public class MissionMainAgentWalkModeControllerVM : ViewModel
	{
		// Token: 0x0600075A RID: 1882 RVA: 0x0001A95D File Offset: 0x00018B5D
		public MissionMainAgentWalkModeControllerVM()
		{
			this.ControlModes = new MBBindingList<WalkModeItemVM>();
			this.RefreshValues();
		}

		// Token: 0x0600075B RID: 1883 RVA: 0x0001A976 File Offset: 0x00018B76
		public override void OnFinalize()
		{
			base.OnFinalize();
			this.ControlModes.ApplyActionOnAllItems(delegate(WalkModeItemVM o)
			{
				o.OnFinalize();
			});
			this.ControlModes.Clear();
		}

		// Token: 0x0600075C RID: 1884 RVA: 0x0001A9B4 File Offset: 0x00018BB4
		public void AddWalkMode(string typeId, TextObject name, MissionMainAgentWalkModeControllerVM.GetIsWalkModeActivatedDelegate getIsActive, MissionMainAgentWalkModeControllerVM.SetIsWalkModeActivatedDelegate setIsActive, MissionMainAgentWalkModeControllerVM.GetCanChangeWalkModeActivatedDelegate canChangeActive, HotKey hotKey, bool isHotkeyConsoleOnly)
		{
			WalkModeItemVM walkModeItemVM = new WalkModeItemVM(typeId, name, getIsActive, setIsActive, canChangeActive, new Action<WalkModeItemVM>(this.OnItemToggled));
			walkModeItemVM.SetToggleInputKey(hotKey, isHotkeyConsoleOnly);
			this.ControlModes.Add(walkModeItemVM);
		}

		// Token: 0x0600075D RID: 1885 RVA: 0x0001A9F0 File Offset: 0x00018BF0
		public void AddWalkMode(string typeId, TextObject name, MissionMainAgentWalkModeControllerVM.GetIsWalkModeActivatedDelegate getIsActive, MissionMainAgentWalkModeControllerVM.SetIsWalkModeActivatedDelegate setIsActive, MissionMainAgentWalkModeControllerVM.GetCanChangeWalkModeActivatedDelegate canChangeActive, GameKey hotKey, bool isHotkeyConsoleOnly)
		{
			WalkModeItemVM walkModeItemVM = new WalkModeItemVM(typeId, name, getIsActive, setIsActive, canChangeActive, new Action<WalkModeItemVM>(this.OnItemToggled));
			walkModeItemVM.SetToggleInputKey(hotKey, isHotkeyConsoleOnly);
			this.ControlModes.Add(walkModeItemVM);
		}

		// Token: 0x0600075E RID: 1886 RVA: 0x0001AA2C File Offset: 0x00018C2C
		private void OnItemToggled(WalkModeItemVM item)
		{
			this.LastUsedItem = item;
		}

		// Token: 0x0600075F RID: 1887 RVA: 0x0001AA38 File Offset: 0x00018C38
		public void SetEnabled(bool isEnabled)
		{
			this.IsEnabled = isEnabled;
			if (isEnabled)
			{
				for (int i = 0; i < this.ControlModes.Count; i++)
				{
					this.ControlModes[i].OnEnabled();
				}
			}
		}

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x06000760 RID: 1888 RVA: 0x0001AA76 File Offset: 0x00018C76
		// (set) Token: 0x06000761 RID: 1889 RVA: 0x0001AA7E File Offset: 0x00018C7E
		[DataSourceProperty]
		public MBBindingList<WalkModeItemVM> ControlModes
		{
			get
			{
				return this._controlModes;
			}
			set
			{
				if (value != this._controlModes)
				{
					this._controlModes = value;
					base.OnPropertyChangedWithValue<MBBindingList<WalkModeItemVM>>(value, "ControlModes");
				}
			}
		}

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x06000762 RID: 1890 RVA: 0x0001AA9C File Offset: 0x00018C9C
		// (set) Token: 0x06000763 RID: 1891 RVA: 0x0001AAA4 File Offset: 0x00018CA4
		[DataSourceProperty]
		public WalkModeItemVM LastUsedItem
		{
			get
			{
				return this._lastUsedItem;
			}
			set
			{
				if (value != this._lastUsedItem)
				{
					this._lastUsedItem = value;
					base.OnPropertyChangedWithValue<WalkModeItemVM>(value, "LastUsedItem");
				}
			}
		}

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x06000764 RID: 1892 RVA: 0x0001AAC2 File Offset: 0x00018CC2
		// (set) Token: 0x06000765 RID: 1893 RVA: 0x0001AACA File Offset: 0x00018CCA
		[DataSourceProperty]
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
			set
			{
				if (value != this._isEnabled)
				{
					this._isEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsEnabled");
				}
			}
		}

		// Token: 0x04000344 RID: 836
		private MBBindingList<WalkModeItemVM> _controlModes;

		// Token: 0x04000345 RID: 837
		private WalkModeItemVM _lastUsedItem;

		// Token: 0x04000346 RID: 838
		private bool _isEnabled;

		// Token: 0x020000F2 RID: 242
		// (Invoke) Token: 0x06000D13 RID: 3347
		public delegate bool GetIsWalkModeActivatedDelegate();

		// Token: 0x020000F3 RID: 243
		// (Invoke) Token: 0x06000D17 RID: 3351
		public delegate void SetIsWalkModeActivatedDelegate(bool value);

		// Token: 0x020000F4 RID: 244
		// (Invoke) Token: 0x06000D1B RID: 3355
		public delegate bool GetCanChangeWalkModeActivatedDelegate();
	}
}
