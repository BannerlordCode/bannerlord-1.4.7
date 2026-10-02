using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Friends
{
	// Token: 0x0200005A RID: 90
	public class MPLobbyPlayerStatItemVM : ViewModel
	{
		// Token: 0x06000885 RID: 2181 RVA: 0x0001B777 File Offset: 0x00019977
		public MPLobbyPlayerStatItemVM(string gameMode, TextObject description, string value)
		{
			this.GameMode = gameMode;
			this._descriptionText = description;
			this.Value = value;
			this.RefreshValues();
		}

		// Token: 0x06000886 RID: 2182 RVA: 0x0001B79A File Offset: 0x0001999A
		public MPLobbyPlayerStatItemVM(string gameMode, TextObject description, float value)
			: this(gameMode, description, value.ToString("0.00"))
		{
		}

		// Token: 0x06000887 RID: 2183 RVA: 0x0001B7B0 File Offset: 0x000199B0
		public MPLobbyPlayerStatItemVM(string gameMode, TextObject description, int value)
			: this(gameMode, description, value.ToString())
		{
		}

		// Token: 0x06000888 RID: 2184 RVA: 0x0001B7C1 File Offset: 0x000199C1
		public override void RefreshValues()
		{
			base.RefreshValues();
			TextObject descriptionText = this._descriptionText;
			this.Description = ((descriptionText != null) ? descriptionText.ToString() : null) ?? "";
		}

		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x06000889 RID: 2185 RVA: 0x0001B7EA File Offset: 0x000199EA
		// (set) Token: 0x0600088A RID: 2186 RVA: 0x0001B7F2 File Offset: 0x000199F2
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

		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x0600088B RID: 2187 RVA: 0x0001B815 File Offset: 0x00019A15
		// (set) Token: 0x0600088C RID: 2188 RVA: 0x0001B81D File Offset: 0x00019A1D
		[DataSourceProperty]
		public string Value
		{
			get
			{
				return this._value;
			}
			set
			{
				if (value != this._value)
				{
					this._value = value;
					base.OnPropertyChangedWithValue<string>(value, "Value");
				}
			}
		}

		// Token: 0x040003F6 RID: 1014
		public readonly string GameMode;

		// Token: 0x040003F7 RID: 1015
		private readonly TextObject _descriptionText;

		// Token: 0x040003F8 RID: 1016
		private string _description;

		// Token: 0x040003F9 RID: 1017
		private string _value;
	}
}
