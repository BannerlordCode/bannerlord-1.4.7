using System;
using TaleWorlds.Network;

namespace TaleWorlds.Core
{
	// Token: 0x020000DE RID: 222
	public class WaitForGameState : CoroutineState
	{
		// Token: 0x06000B73 RID: 2931 RVA: 0x00025251 File Offset: 0x00023451
		public WaitForGameState(Type stateType)
		{
			this._stateType = stateType;
		}

		// Token: 0x170003E7 RID: 999
		// (get) Token: 0x06000B74 RID: 2932 RVA: 0x00025260 File Offset: 0x00023460
		protected override bool IsFinished
		{
			get
			{
				GameState gameState = ((GameStateManager.Current != null) ? GameStateManager.Current.ActiveState : null);
				return gameState != null && this._stateType.IsInstanceOfType(gameState);
			}
		}

		// Token: 0x04000689 RID: 1673
		private Type _stateType;
	}
}
