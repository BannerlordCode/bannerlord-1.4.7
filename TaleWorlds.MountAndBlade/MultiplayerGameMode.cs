using System;
using TaleWorlds.MountAndBlade.Diamond;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002E0 RID: 736
	public abstract class MultiplayerGameMode
	{
		// Token: 0x170007F6 RID: 2038
		// (get) Token: 0x06002AA6 RID: 10918 RVA: 0x000A416B File Offset: 0x000A236B
		// (set) Token: 0x06002AA7 RID: 10919 RVA: 0x000A4173 File Offset: 0x000A2373
		public string Name { get; private set; }

		// Token: 0x06002AA8 RID: 10920 RVA: 0x000A417C File Offset: 0x000A237C
		protected MultiplayerGameMode(string name)
		{
			this.Name = name;
		}

		// Token: 0x06002AA9 RID: 10921
		public abstract void JoinCustomGame(JoinGameData joinGameData);

		// Token: 0x06002AAA RID: 10922
		public abstract void StartMultiplayerGame(string scene);
	}
}
