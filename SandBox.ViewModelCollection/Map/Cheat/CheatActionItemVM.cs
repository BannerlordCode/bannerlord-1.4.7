using System;

namespace SandBox.ViewModelCollection.Map.Cheat
{
	// Token: 0x0200004E RID: 78
	public class CheatActionItemVM : CheatItemBaseVM
	{
		// Token: 0x060004E4 RID: 1252 RVA: 0x00012C6E File Offset: 0x00010E6E
		public CheatActionItemVM(GameplayCheatItem cheat, Action<CheatActionItemVM> onCheatExecuted)
		{
			this._onCheatExecuted = onCheatExecuted;
			this.Cheat = cheat;
			this.RefreshValues();
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x00012C8A File Offset: 0x00010E8A
		public override void RefreshValues()
		{
			base.RefreshValues();
			GameplayCheatItem cheat = this.Cheat;
			base.Name = ((cheat != null) ? cheat.GetName().ToString() : null);
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x00012CAF File Offset: 0x00010EAF
		public override void ExecuteAction()
		{
			GameplayCheatItem cheat = this.Cheat;
			if (cheat != null)
			{
				cheat.ExecuteCheat();
			}
			Action<CheatActionItemVM> onCheatExecuted = this._onCheatExecuted;
			if (onCheatExecuted == null)
			{
				return;
			}
			onCheatExecuted(this);
		}

		// Token: 0x0400026A RID: 618
		public readonly GameplayCheatItem Cheat;

		// Token: 0x0400026B RID: 619
		private readonly Action<CheatActionItemVM> _onCheatExecuted;
	}
}
