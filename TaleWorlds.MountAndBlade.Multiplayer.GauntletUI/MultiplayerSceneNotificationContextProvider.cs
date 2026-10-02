using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade.Multiplayer.GauntletUI
{
	// Token: 0x02000009 RID: 9
	public class MultiplayerSceneNotificationContextProvider : ISceneNotificationContextProvider
	{
		// Token: 0x06000083 RID: 131 RVA: 0x0000456A File Offset: 0x0000276A
		public bool IsContextAllowed(SceneNotificationData.RelevantContextType relevantType)
		{
			return relevantType != SceneNotificationData.RelevantContextType.MPLobby || GameStateManager.Current.ActiveState is LobbyState;
		}
	}
}
