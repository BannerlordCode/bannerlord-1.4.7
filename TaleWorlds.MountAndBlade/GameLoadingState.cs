using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200023C RID: 572
	public class GameLoadingState : GameState
	{
		// Token: 0x170006B0 RID: 1712
		// (get) Token: 0x06002120 RID: 8480 RVA: 0x000749D7 File Offset: 0x00072BD7
		public override bool IsMusicMenuState
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002122 RID: 8482 RVA: 0x000749E2 File Offset: 0x00072BE2
		public void SetLoadingParameters(MBGameManager gameLoader)
		{
			Game.OnGameCreated += this.OnGameCreated;
			this._gameLoader = gameLoader;
		}

		// Token: 0x06002123 RID: 8483 RVA: 0x000749FC File Offset: 0x00072BFC
		protected override void OnTick(float dt)
		{
			base.OnTick(dt);
			if (!this._loadingFinished)
			{
				this._loadingFinished = this._gameLoader.DoLoadingForGameManager();
				return;
			}
			GameStateManager.Current = Game.Current.GameStateManager;
			this._gameLoader.OnLoadFinished();
		}

		// Token: 0x06002124 RID: 8484 RVA: 0x00074A39 File Offset: 0x00072C39
		private void OnGameCreated()
		{
			Game.OnGameCreated -= this.OnGameCreated;
			Game.Current.OnItemDeserializedEvent += delegate(ItemObject itemObject)
			{
				if (itemObject.Type == ItemObject.ItemTypeEnum.HandArmor)
				{
					Utilities.RegisterMeshForGPUMorph(itemObject.MultiMeshName);
				}
			};
		}

		// Token: 0x04000CB7 RID: 3255
		private bool _loadingFinished;

		// Token: 0x04000CB8 RID: 3256
		private MBGameManager _gameLoader;
	}
}
