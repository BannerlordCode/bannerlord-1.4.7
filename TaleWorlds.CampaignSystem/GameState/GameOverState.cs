using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.GameState
{
	// Token: 0x02000392 RID: 914
	public class GameOverState : GameState
	{
		// Token: 0x17000C8F RID: 3215
		// (get) Token: 0x060034FE RID: 13566 RVA: 0x000D96F3 File Offset: 0x000D78F3
		public override bool IsMenuState
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000C90 RID: 3216
		// (get) Token: 0x060034FF RID: 13567 RVA: 0x000D96F6 File Offset: 0x000D78F6
		// (set) Token: 0x06003500 RID: 13568 RVA: 0x000D96FE File Offset: 0x000D78FE
		public IGameOverStateHandler Handler
		{
			get
			{
				return this._handler;
			}
			set
			{
				this._handler = value;
			}
		}

		// Token: 0x17000C91 RID: 3217
		// (get) Token: 0x06003501 RID: 13569 RVA: 0x000D9707 File Offset: 0x000D7907
		// (set) Token: 0x06003502 RID: 13570 RVA: 0x000D970F File Offset: 0x000D790F
		public GameOverState.GameOverReason Reason { get; private set; }

		// Token: 0x06003503 RID: 13571 RVA: 0x000D9718 File Offset: 0x000D7918
		public GameOverState()
		{
		}

		// Token: 0x06003504 RID: 13572 RVA: 0x000D9720 File Offset: 0x000D7920
		public GameOverState(GameOverState.GameOverReason reason)
		{
			this.Reason = reason;
		}

		// Token: 0x06003505 RID: 13573 RVA: 0x000D972F File Offset: 0x000D792F
		public static GameOverState CreateForVictory()
		{
			Game game = Game.Current;
			if (game == null)
			{
				return null;
			}
			return game.GameStateManager.CreateState<GameOverState>(new object[] { GameOverState.GameOverReason.Victory });
		}

		// Token: 0x06003506 RID: 13574 RVA: 0x000D9755 File Offset: 0x000D7955
		public static GameOverState CreateForRetirement()
		{
			Game game = Game.Current;
			if (game == null)
			{
				return null;
			}
			return game.GameStateManager.CreateState<GameOverState>(new object[] { GameOverState.GameOverReason.Retirement });
		}

		// Token: 0x06003507 RID: 13575 RVA: 0x000D977B File Offset: 0x000D797B
		public static GameOverState CreateForClanDestroyed()
		{
			Game game = Game.Current;
			if (game == null)
			{
				return null;
			}
			return game.GameStateManager.CreateState<GameOverState>(new object[] { GameOverState.GameOverReason.ClanDestroyed });
		}

		// Token: 0x04000F27 RID: 3879
		private IGameOverStateHandler _handler;

		// Token: 0x0200076F RID: 1903
		public enum GameOverReason
		{
			// Token: 0x04001EA0 RID: 7840
			Retirement,
			// Token: 0x04001EA1 RID: 7841
			ClanDestroyed,
			// Token: 0x04001EA2 RID: 7842
			Victory
		}
	}
}
