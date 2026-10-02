using System;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.HUD
{
	// Token: 0x02000055 RID: 85
	public class ControllerEquippedItemVM : EquipmentActionItemVM
	{
		// Token: 0x06000707 RID: 1799 RVA: 0x00019A58 File Offset: 0x00017C58
		public ControllerEquippedItemVM(string item, string itemTypeAsString, object identifier, HotKey key, Action<EquipmentActionItemVM> onSelection)
			: base(item, itemTypeAsString, identifier, onSelection, false)
		{
			if (key != null)
			{
				this.ShortcutKey = InputKeyItemVM.CreateFromHotKey(key, true);
			}
		}

		// Token: 0x06000708 RID: 1800 RVA: 0x00019A78 File Offset: 0x00017C78
		public override void OnFinalize()
		{
			base.OnFinalize();
			InputKeyItemVM shortcutKey = this.ShortcutKey;
			if (shortcutKey != null)
			{
				shortcutKey.OnFinalize();
			}
			this.ShortcutKey = null;
		}

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x06000709 RID: 1801 RVA: 0x00019A98 File Offset: 0x00017C98
		// (set) Token: 0x0600070A RID: 1802 RVA: 0x00019AA0 File Offset: 0x00017CA0
		[DataSourceProperty]
		public InputKeyItemVM ShortcutKey
		{
			get
			{
				return this._shortcutKey;
			}
			set
			{
				if (value != this._shortcutKey)
				{
					this._shortcutKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "ShortcutKey");
				}
			}
		}

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x0600070B RID: 1803 RVA: 0x00019ABE File Offset: 0x00017CBE
		// (set) Token: 0x0600070C RID: 1804 RVA: 0x00019AC6 File Offset: 0x00017CC6
		[DataSourceProperty]
		public float DropProgress
		{
			get
			{
				return this._dropProgress;
			}
			set
			{
				if (value != this._dropProgress)
				{
					this._dropProgress = value;
					base.OnPropertyChangedWithValue(value, "DropProgress");
				}
			}
		}

		// Token: 0x0400031E RID: 798
		private InputKeyItemVM _shortcutKey;

		// Token: 0x0400031F RID: 799
		private float _dropProgress;
	}
}
