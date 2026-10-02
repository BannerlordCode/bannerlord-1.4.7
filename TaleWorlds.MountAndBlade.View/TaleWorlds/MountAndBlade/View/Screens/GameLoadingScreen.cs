using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade.View.Screens
{
	// Token: 0x02000055 RID: 85
	[GameStateScreen(typeof(GameLoadingState))]
	public class GameLoadingScreen : ScreenBase, IGameStateListener
	{
		// Token: 0x060002B6 RID: 694 RVA: 0x00011F2E File Offset: 0x0001012E
		public GameLoadingScreen(GameLoadingState gameLoadingState)
		{
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x00011F36 File Offset: 0x00010136
		protected override void OnActivate()
		{
			base.OnActivate();
			LoadingWindow.EnableGlobalLoadingWindow();
			Utilities.SetScreenTextRenderingState(false);
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x00011F49 File Offset: 0x00010149
		protected override void OnFrameTick(float dt)
		{
			base.OnFrameTick(dt);
			Utilities.SetScreenTextRenderingState(true);
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x00011F58 File Offset: 0x00010158
		void IGameStateListener.OnActivate()
		{
		}

		// Token: 0x060002BA RID: 698 RVA: 0x00011F5A File Offset: 0x0001015A
		void IGameStateListener.OnDeactivate()
		{
		}

		// Token: 0x060002BB RID: 699 RVA: 0x00011F5C File Offset: 0x0001015C
		void IGameStateListener.OnInitialize()
		{
		}

		// Token: 0x060002BC RID: 700 RVA: 0x00011F5E File Offset: 0x0001015E
		void IGameStateListener.OnFinalize()
		{
		}
	}
}
