using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.GameState
{
	// Token: 0x02000387 RID: 903
	public class BannerEditorState : GameState
	{
		// Token: 0x17000C7B RID: 3195
		// (get) Token: 0x060034C7 RID: 13511 RVA: 0x000D94CE File Offset: 0x000D76CE
		public override bool IsMenuState
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000C7C RID: 3196
		// (get) Token: 0x060034C8 RID: 13512 RVA: 0x000D94D1 File Offset: 0x000D76D1
		// (set) Token: 0x060034C9 RID: 13513 RVA: 0x000D94D9 File Offset: 0x000D76D9
		public IBannerEditorStateHandler Handler
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

		// Token: 0x060034CA RID: 13514 RVA: 0x000D94E2 File Offset: 0x000D76E2
		public BannerEditorState()
		{
		}

		// Token: 0x060034CB RID: 13515 RVA: 0x000D94EA File Offset: 0x000D76EA
		public BannerEditorState(Action endAction)
		{
			this._onEndAction = endAction;
		}

		// Token: 0x060034CC RID: 13516 RVA: 0x000D94F9 File Offset: 0x000D76F9
		public Clan GetClan()
		{
			return Clan.PlayerClan;
		}

		// Token: 0x060034CD RID: 13517 RVA: 0x000D9500 File Offset: 0x000D7700
		public CharacterObject GetCharacter()
		{
			return CharacterObject.PlayerCharacter;
		}

		// Token: 0x060034CE RID: 13518 RVA: 0x000D9507 File Offset: 0x000D7707
		protected override void OnFinalize()
		{
			base.OnFinalize();
			Action onEndAction = this._onEndAction;
			if (onEndAction == null)
			{
				return;
			}
			onEndAction();
		}

		// Token: 0x04000F17 RID: 3863
		private IBannerEditorStateHandler _handler;

		// Token: 0x04000F18 RID: 3864
		private Action _onEndAction;
	}
}
