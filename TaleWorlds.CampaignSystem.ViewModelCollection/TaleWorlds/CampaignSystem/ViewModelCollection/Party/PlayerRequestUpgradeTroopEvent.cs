using System;
using TaleWorlds.Library.EventSystem;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Party
{
	// Token: 0x0200002D RID: 45
	public class PlayerRequestUpgradeTroopEvent : EventBase
	{
		// Token: 0x17000149 RID: 329
		// (get) Token: 0x060004A4 RID: 1188 RVA: 0x0001BA8A File Offset: 0x00019C8A
		// (set) Token: 0x060004A5 RID: 1189 RVA: 0x0001BA92 File Offset: 0x00019C92
		public CharacterObject SourceTroop { get; private set; }

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x060004A6 RID: 1190 RVA: 0x0001BA9B File Offset: 0x00019C9B
		// (set) Token: 0x060004A7 RID: 1191 RVA: 0x0001BAA3 File Offset: 0x00019CA3
		public CharacterObject TargetTroop { get; private set; }

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x060004A8 RID: 1192 RVA: 0x0001BAAC File Offset: 0x00019CAC
		// (set) Token: 0x060004A9 RID: 1193 RVA: 0x0001BAB4 File Offset: 0x00019CB4
		public int Number { get; private set; }

		// Token: 0x060004AA RID: 1194 RVA: 0x0001BABD File Offset: 0x00019CBD
		public PlayerRequestUpgradeTroopEvent(CharacterObject sourceTroop, CharacterObject targetTroop, int num)
		{
			this.SourceTroop = sourceTroop;
			this.TargetTroop = targetTroop;
			this.Number = num;
		}
	}
}
