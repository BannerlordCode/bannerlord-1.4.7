using System;

namespace TaleWorlds.CampaignSystem.Issues.IssueQuestTasks
{
	// Token: 0x02000384 RID: 900
	public class TalkToNpcQuestTask : QuestTaskBase
	{
		// Token: 0x0600349C RID: 13468 RVA: 0x000D90BF File Offset: 0x000D72BF
		public TalkToNpcQuestTask(Hero hero, Action onSucceededAction, DialogFlow dialogFlow = null)
			: base(dialogFlow, onSucceededAction, null, null)
		{
			this._character = hero.CharacterObject;
		}

		// Token: 0x0600349D RID: 13469 RVA: 0x000D90D7 File Offset: 0x000D72D7
		public TalkToNpcQuestTask(CharacterObject character, Action onSucceededAction, DialogFlow dialogFlow = null)
			: base(dialogFlow, onSucceededAction, null, null)
		{
			this._character = character;
		}

		// Token: 0x0600349E RID: 13470 RVA: 0x000D90EA File Offset: 0x000D72EA
		public bool IsTaskCharacter()
		{
			return this._character == CharacterObject.OneToOneConversationCharacter;
		}

		// Token: 0x0600349F RID: 13471 RVA: 0x000D90F9 File Offset: 0x000D72F9
		protected override void OnFinished()
		{
			this._character = null;
		}

		// Token: 0x060034A0 RID: 13472 RVA: 0x000D9102 File Offset: 0x000D7302
		public override void SetReferences()
		{
		}

		// Token: 0x04000F0F RID: 3855
		private CharacterObject _character;
	}
}
