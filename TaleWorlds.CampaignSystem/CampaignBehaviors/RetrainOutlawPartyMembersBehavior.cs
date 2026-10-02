using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x0200043D RID: 1085
	public class RetrainOutlawPartyMembersBehavior : CampaignBehaviorBase, IRetrainOutlawPartyMembersCampaignBehavior, ICampaignBehavior
	{
		// Token: 0x0600459A RID: 17818 RVA: 0x00157E9C File Offset: 0x0015609C
		private int GetRetrainedNumberInternal(CharacterObject character)
		{
			int num;
			if (!this._retrainTable.TryGetValue(character, out num))
			{
				return 0;
			}
			return num;
		}

		// Token: 0x0600459B RID: 17819 RVA: 0x00157EBC File Offset: 0x001560BC
		private int SetRetrainedNumberInternal(CharacterObject character, int numberRetrained)
		{
			this._retrainTable[character] = numberRetrained;
			return numberRetrained;
		}

		// Token: 0x0600459C RID: 17820 RVA: 0x00157ED9 File Offset: 0x001560D9
		public override void RegisterEvents()
		{
			CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, new Action(this.DailyTick));
		}

		// Token: 0x0600459D RID: 17821 RVA: 0x00157EF4 File Offset: 0x001560F4
		private void DailyTick()
		{
			if (MBRandom.RandomFloat > 0.5f)
			{
				int num = MBRandom.RandomInt(MobileParty.MainParty.MemberRoster.Count);
				bool flag = false;
				int num2 = 0;
				while (num2 < MobileParty.MainParty.MemberRoster.Count && !flag)
				{
					int num3 = (num2 + num) % MobileParty.MainParty.MemberRoster.Count;
					CharacterObject characterAtIndex = MobileParty.MainParty.MemberRoster.GetCharacterAtIndex(num3);
					if (characterAtIndex.Occupation == Occupation.Bandit)
					{
						int elementNumber = MobileParty.MainParty.MemberRoster.GetElementNumber(num3);
						int num4 = this.GetRetrainedNumberInternal(characterAtIndex);
						if (num4 < elementNumber && !flag)
						{
							num4++;
							this.SetRetrainedNumberInternal(characterAtIndex, num4);
						}
						else if (num4 > elementNumber)
						{
							this.SetRetrainedNumberInternal(characterAtIndex, elementNumber);
						}
					}
					num2++;
				}
			}
		}

		// Token: 0x0600459E RID: 17822 RVA: 0x00157FC1 File Offset: 0x001561C1
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<Dictionary<CharacterObject, int>>("_retrainTable", ref this._retrainTable);
		}

		// Token: 0x0600459F RID: 17823 RVA: 0x00157FD8 File Offset: 0x001561D8
		public int GetRetrainedNumber(CharacterObject character)
		{
			if (character.Occupation == Occupation.Bandit)
			{
				int retrainedNumberInternal = this.GetRetrainedNumberInternal(character);
				int troopCount = MobileParty.MainParty.MemberRoster.GetTroopCount(character);
				return MathF.Min(retrainedNumberInternal, troopCount);
			}
			return 0;
		}

		// Token: 0x060045A0 RID: 17824 RVA: 0x0015800F File Offset: 0x0015620F
		public void SetRetrainedNumber(CharacterObject character, int number)
		{
			this.SetRetrainedNumberInternal(character, number);
		}

		// Token: 0x0400139B RID: 5019
		private Dictionary<CharacterObject, int> _retrainTable = new Dictionary<CharacterObject, int>();
	}
}
