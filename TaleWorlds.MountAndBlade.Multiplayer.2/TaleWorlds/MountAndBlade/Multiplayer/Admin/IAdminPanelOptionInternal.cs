using System;

namespace TaleWorlds.MountAndBlade.Multiplayer.Admin
{
	// Token: 0x0200006F RID: 111
	internal interface IAdminPanelOptionInternal
	{
		// Token: 0x0600035E RID: 862
		MultiplayerOptions.OptionType GetOptionType();

		// Token: 0x0600035F RID: 863
		MultiplayerOptions.MultiplayerOptionsAccessMode GetOptionAccessMode();

		// Token: 0x06000360 RID: 864
		void OnApplyChanges();

		// Token: 0x06000361 RID: 865
		void AddValueChangedCallback(Action callback);

		// Token: 0x06000362 RID: 866
		void RemoveValueChangedCallback(Action callback);

		// Token: 0x06000363 RID: 867
		void OnFinalize();
	}
}
