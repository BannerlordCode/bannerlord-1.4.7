using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby
{
	// Token: 0x02000028 RID: 40
	public class MPLobbyBlockerStateVM : ViewModel
	{
		// Token: 0x060002F9 RID: 761 RVA: 0x0000BC70 File Offset: 0x00009E70
		public MPLobbyBlockerStateVM(Action<bool> setNavigationRestriction)
		{
			this._setNavigationRestriction = setNavigationRestriction;
		}

		// Token: 0x060002FA RID: 762 RVA: 0x0000BC7F File Offset: 0x00009E7F
		public void OnLobbyStateIsBlocker(TextObject description)
		{
			this._descriptionObj = description;
			this.IsEnabled = true;
			this.Description = this._descriptionObj.ToString();
		}

		// Token: 0x060002FB RID: 763 RVA: 0x0000BCA0 File Offset: 0x00009EA0
		public override void RefreshValues()
		{
			base.RefreshValues();
			TextObject descriptionObj = this._descriptionObj;
			this.Description = ((descriptionObj != null) ? descriptionObj.ToString() : null);
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0000BCC0 File Offset: 0x00009EC0
		public void OnLobbyStateNotBlocker()
		{
			this.IsEnabled = false;
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x060002FD RID: 765 RVA: 0x0000BCC9 File Offset: 0x00009EC9
		// (set) Token: 0x060002FE RID: 766 RVA: 0x0000BCD1 File Offset: 0x00009ED1
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
					this._setNavigationRestriction(value);
				}
			}
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x060002FF RID: 767 RVA: 0x0000BCFB File Offset: 0x00009EFB
		// (set) Token: 0x06000300 RID: 768 RVA: 0x0000BD03 File Offset: 0x00009F03
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

		// Token: 0x0400018C RID: 396
		private Action<bool> _setNavigationRestriction;

		// Token: 0x0400018D RID: 397
		private TextObject _descriptionObj;

		// Token: 0x0400018E RID: 398
		private bool _isEnabled;

		// Token: 0x0400018F RID: 399
		private string _description;
	}
}
