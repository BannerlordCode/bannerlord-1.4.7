using System;
using System.Collections.Generic;
using TaleWorlds.Library;

namespace TaleWorlds.Core
{
	// Token: 0x02000070 RID: 112
	public abstract class GameModelsManager
	{
		// Token: 0x060007E7 RID: 2023 RVA: 0x0001A371 File Offset: 0x00018571
		protected GameModelsManager(IEnumerable<GameModel> inputComponents)
		{
			this._gameModels = inputComponents.ToMBList<GameModel>();
		}

		// Token: 0x060007E8 RID: 2024 RVA: 0x0001A388 File Offset: 0x00018588
		protected T GetGameModel<T>() where T : GameModel
		{
			for (int i = this._gameModels.Count - 1; i >= 0; i--)
			{
				T t;
				if ((t = this._gameModels[i] as T) != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x060007E9 RID: 2025 RVA: 0x0001A3D7 File Offset: 0x000185D7
		public MBReadOnlyList<GameModel> GetGameModels()
		{
			return this._gameModels;
		}

		// Token: 0x0400040F RID: 1039
		private readonly MBList<GameModel> _gameModels;
	}
}
