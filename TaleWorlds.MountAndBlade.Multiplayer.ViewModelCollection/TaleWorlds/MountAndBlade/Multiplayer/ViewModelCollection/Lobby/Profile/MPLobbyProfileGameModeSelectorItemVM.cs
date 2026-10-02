using System;
using TaleWorlds.Core.ViewModelCollection.Selector;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Profile
{
	// Token: 0x02000039 RID: 57
	public class MPLobbyProfileGameModeSelectorItemVM : SelectorItemVM
	{
		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x06000546 RID: 1350 RVA: 0x000122D2 File Offset: 0x000104D2
		// (set) Token: 0x06000547 RID: 1351 RVA: 0x000122DA File Offset: 0x000104DA
		public string GameModeCode { get; private set; }

		// Token: 0x06000548 RID: 1352 RVA: 0x000122E3 File Offset: 0x000104E3
		public MPLobbyProfileGameModeSelectorItemVM(string gameModeCode, TextObject gameModeName)
			: base(gameModeName)
		{
			this.GameModeCode = gameModeCode;
		}
	}
}
