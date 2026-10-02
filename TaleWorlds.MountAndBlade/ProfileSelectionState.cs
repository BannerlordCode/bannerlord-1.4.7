using System;
using TaleWorlds.Core;
using TaleWorlds.Engine.Options;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000244 RID: 580
	public class ProfileSelectionState : GameState
	{
		// Token: 0x170006BE RID: 1726
		// (get) Token: 0x0600216F RID: 8559 RVA: 0x00075590 File Offset: 0x00073790
		// (set) Token: 0x06002170 RID: 8560 RVA: 0x00075598 File Offset: 0x00073798
		public bool IsDirectPlayPossible { get; private set; } = true;

		// Token: 0x14000030 RID: 48
		// (add) Token: 0x06002171 RID: 8561 RVA: 0x000755A4 File Offset: 0x000737A4
		// (remove) Token: 0x06002172 RID: 8562 RVA: 0x000755DC File Offset: 0x000737DC
		public event ProfileSelectionState.OnProfileSelectionEvent OnProfileSelection;

		// Token: 0x06002173 RID: 8563 RVA: 0x00075611 File Offset: 0x00073811
		public void OnProfileSelected()
		{
			NativeOptions.ReadRGLConfigFiles();
			BannerlordConfig.Initialize();
			ProfileSelectionState.OnProfileSelectionEvent onProfileSelection = this.OnProfileSelection;
			if (onProfileSelection != null)
			{
				onProfileSelection();
			}
			this.StartGame();
		}

		// Token: 0x06002174 RID: 8564 RVA: 0x00075634 File Offset: 0x00073834
		public void StartGame()
		{
			Module.CurrentModule.SetInitialModuleScreenAsRootScreen();
		}

		// Token: 0x02000538 RID: 1336
		// (Invoke) Token: 0x06003C54 RID: 15444
		public delegate void OnProfileSelectionEvent();
	}
}
