using System;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.GauntletUI
{
	// Token: 0x0200001C RID: 28
	public class KeybindingPopupVM : ViewModel
	{
		// Token: 0x0600010B RID: 267 RVA: 0x0000854D File Offset: 0x0000674D
		public KeybindingPopupVM(Action onCancel)
		{
			this._onCancel = onCancel;
			this.RefreshValues();
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00008564 File Offset: 0x00006764
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.PressKeyText = new TextObject("{=hvaDkG4w}Press any key.", null).ToString();
			TextObject textObject = new TextObject("{=5U8vXv4E}Press {KEY} to cancel", null);
			textObject.SetTextVariable("KEY", HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Exit").ToString());
			this.CancelText = textObject.ToString();
		}

		// Token: 0x0600010D RID: 269 RVA: 0x000085CA File Offset: 0x000067CA
		public void ExecuteCancel()
		{
			Action onCancel = this._onCancel;
			if (onCancel == null)
			{
				return;
			}
			onCancel();
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600010E RID: 270 RVA: 0x000085DC File Offset: 0x000067DC
		// (set) Token: 0x0600010F RID: 271 RVA: 0x000085E4 File Offset: 0x000067E4
		[DataSourceProperty]
		public string PressKeyText
		{
			get
			{
				return this._pressKeyText;
			}
			set
			{
				if (this._pressKeyText != value)
				{
					this._pressKeyText = value;
					base.OnPropertyChangedWithValue<string>(value, "PressKeyText");
				}
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000110 RID: 272 RVA: 0x00008607 File Offset: 0x00006807
		// (set) Token: 0x06000111 RID: 273 RVA: 0x0000860F File Offset: 0x0000680F
		[DataSourceProperty]
		public string CancelText
		{
			get
			{
				return this._cancelText;
			}
			set
			{
				if (this._cancelText != value)
				{
					this._cancelText = value;
					base.OnPropertyChangedWithValue<string>(value, "CancelText");
				}
			}
		}

		// Token: 0x040000AB RID: 171
		private readonly Action _onCancel;

		// Token: 0x040000AC RID: 172
		private string _pressKeyText;

		// Token: 0x040000AD RID: 173
		private string _cancelText;
	}
}
