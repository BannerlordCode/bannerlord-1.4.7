using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;

namespace SandBox.ViewModelCollection.Nameplate.NameplateNotifications.SettlementNotificationTypes
{
	// Token: 0x0200002A RID: 42
	public class TroopRecruitmentNotificationItemVM : SettlementNotificationItemBaseVM
	{
		// Token: 0x17000116 RID: 278
		// (get) Token: 0x0600037B RID: 891 RVA: 0x0000F1D4 File Offset: 0x0000D3D4
		// (set) Token: 0x0600037C RID: 892 RVA: 0x0000F1DC File Offset: 0x0000D3DC
		public Hero RecruiterHero { get; private set; }

		// Token: 0x0600037D RID: 893 RVA: 0x0000F1E8 File Offset: 0x0000D3E8
		public TroopRecruitmentNotificationItemVM(Action<SettlementNotificationItemBaseVM> onRemove, Hero recruiterHero, int amount, int createdTick)
			: base(onRemove, createdTick)
		{
			base.Text = SandBoxUIHelper.GetRecruitNotificationText(amount);
			this._recruitAmount = amount;
			this.RecruiterHero = recruiterHero;
			base.CharacterName = ((recruiterHero != null) ? recruiterHero.Name.ToString() : "null hero");
			base.CharacterVisual = ((recruiterHero != null) ? new CharacterImageIdentifierVM(SandBoxUIHelper.GetCharacterCode(recruiterHero.CharacterObject, false)) : new CharacterImageIdentifierVM(null));
			base.RelationType = 0;
			base.CreatedTick = createdTick;
			if (recruiterHero != null)
			{
				base.RelationType = (recruiterHero.Clan.IsAtWarWith(Hero.MainHero.Clan) ? (-1) : 1);
			}
		}

		// Token: 0x0600037E RID: 894 RVA: 0x0000F288 File Offset: 0x0000D488
		public void AddNewAction(int addedAmount)
		{
			this._recruitAmount += addedAmount;
			base.Text = SandBoxUIHelper.GetRecruitNotificationText(this._recruitAmount);
		}

		// Token: 0x040001C4 RID: 452
		private int _recruitAmount;
	}
}
