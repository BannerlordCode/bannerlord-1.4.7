using System;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Friends
{
	// Token: 0x0200005B RID: 91
	public class MPLobbyPlayerTroopClassVM : ViewModel
	{
		// Token: 0x0600088D RID: 2189 RVA: 0x0001B840 File Offset: 0x00019A40
		public MPLobbyPlayerTroopClassVM()
		{
			this.Name = "Varangian Guard";
			this.Preview = new CharacterImageIdentifierVM(null);
		}

		// Token: 0x170002CA RID: 714
		// (get) Token: 0x0600088E RID: 2190 RVA: 0x0001B85F File Offset: 0x00019A5F
		// (set) Token: 0x0600088F RID: 2191 RVA: 0x0001B867 File Offset: 0x00019A67
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

		// Token: 0x170002CB RID: 715
		// (get) Token: 0x06000890 RID: 2192 RVA: 0x0001B88A File Offset: 0x00019A8A
		// (set) Token: 0x06000891 RID: 2193 RVA: 0x0001B892 File Offset: 0x00019A92
		[DataSourceProperty]
		public CharacterImageIdentifierVM Preview
		{
			get
			{
				return this._preview;
			}
			set
			{
				if (value != this._preview)
				{
					this._preview = value;
					base.OnPropertyChangedWithValue<CharacterImageIdentifierVM>(value, "Preview");
				}
			}
		}

		// Token: 0x040003FA RID: 1018
		private string _name;

		// Token: 0x040003FB RID: 1019
		private CharacterImageIdentifierVM _preview;
	}
}
