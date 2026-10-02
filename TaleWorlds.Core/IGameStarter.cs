using System;
using System.Collections.Generic;

namespace TaleWorlds.Core
{
	// Token: 0x0200006D RID: 109
	public interface IGameStarter
	{
		// Token: 0x060007D6 RID: 2006
		void AddModel(GameModel gameModel);

		// Token: 0x060007D7 RID: 2007
		void AddModel<T>(MBGameModel<T> gameModel) where T : GameModel;

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x060007D8 RID: 2008
		IEnumerable<GameModel> Models { get; }
	}
}
