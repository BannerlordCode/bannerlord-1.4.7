using System;
using Sandbox.View.GameStates;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View.Screens;
using TaleWorlds.SaveSystem;
using TaleWorlds.SaveSystem.Load;
using TaleWorlds.ScreenSystem;

namespace SandBox.View
{
	// Token: 0x02000007 RID: 7
	[GameStateScreen(typeof(PreloadState))]
	public class PreloadScreen : ScreenBase, IGameStateListener
	{
		// Token: 0x06000013 RID: 19 RVA: 0x0000296C File Offset: 0x00000B6C
		public PreloadScreen(PreloadState inventoryState)
		{
			this._state = inventoryState;
			this._delayCounter = 0;
			this._delayInFrames = Math.Max(0, this._state.LoadDelayInFrames);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002999 File Offset: 0x00000B99
		protected override void OnInitialize()
		{
			base.OnInitialize();
			LoadingWindow.EnableGlobalLoadingWindow();
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000029A8 File Offset: 0x00000BA8
		protected override void OnFrameTick(float dt)
		{
			base.OnFrameTick(dt);
			if (this._delayCounter != this._delayInFrames)
			{
				this._delayCounter++;
				return;
			}
			SaveGameFileInfo saveFileWithName = MBSaveLoad.GetSaveFileWithName(this._state.SaveToLoad);
			if (saveFileWithName == null)
			{
				throw new MBException("Preload state called without a valid save name. Game will be stuck at this point.");
			}
			SandBoxSaveHelper.TryLoadSave(saveFileWithName, new Action<LoadResult>(this.StartGame), null);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002A09 File Offset: 0x00000C09
		private void StartGame(LoadResult loadResult)
		{
			MBGameManager.StartNewGame(new SandBoxGameManager(loadResult));
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002A16 File Offset: 0x00000C16
		void IGameStateListener.OnActivate()
		{
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002A18 File Offset: 0x00000C18
		void IGameStateListener.OnDeactivate()
		{
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002A1A File Offset: 0x00000C1A
		void IGameStateListener.OnInitialize()
		{
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002A1C File Offset: 0x00000C1C
		void IGameStateListener.OnFinalize()
		{
		}

		// Token: 0x04000004 RID: 4
		private readonly PreloadState _state;

		// Token: 0x04000005 RID: 5
		private readonly int _delayInFrames;

		// Token: 0x04000006 RID: 6
		private int _delayCounter;
	}
}
