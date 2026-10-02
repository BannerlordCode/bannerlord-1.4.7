using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.Encounters
{
	// Token: 0x020002EC RID: 748
	public class LocationEncounter
	{
		// Token: 0x17000A1E RID: 2590
		// (get) Token: 0x060029DB RID: 10715 RVA: 0x000AEEA6 File Offset: 0x000AD0A6
		public Settlement Settlement { get; }

		// Token: 0x17000A1F RID: 2591
		// (get) Token: 0x060029DC RID: 10716 RVA: 0x000AEEAE File Offset: 0x000AD0AE
		// (set) Token: 0x060029DD RID: 10717 RVA: 0x000AEEB6 File Offset: 0x000AD0B6
		public List<AccompanyingCharacter> CharactersAccompanyingPlayer { get; private set; }

		// Token: 0x060029DE RID: 10718 RVA: 0x000AEEBF File Offset: 0x000AD0BF
		protected LocationEncounter(Settlement settlement)
		{
			this.Settlement = settlement;
			this.CharactersAccompanyingPlayer = new List<AccompanyingCharacter>();
		}

		// Token: 0x060029DF RID: 10719 RVA: 0x000AEEDC File Offset: 0x000AD0DC
		public void AddAccompanyingCharacter(LocationCharacter locationCharacter, bool isFollowing = false)
		{
			if (!this.CharactersAccompanyingPlayer.Any<AccompanyingCharacter>((AccompanyingCharacter x) => x.LocationCharacter.Character == locationCharacter.Character))
			{
				AccompanyingCharacter accompanyingCharacter = new AccompanyingCharacter(locationCharacter, isFollowing);
				this.CharactersAccompanyingPlayer.Add(accompanyingCharacter);
			}
		}

		// Token: 0x060029E0 RID: 10720 RVA: 0x000AEF28 File Offset: 0x000AD128
		public AccompanyingCharacter GetAccompanyingCharacter(LocationCharacter locationCharacter)
		{
			return this.CharactersAccompanyingPlayer.Find((AccompanyingCharacter x) => x.LocationCharacter == locationCharacter);
		}

		// Token: 0x060029E1 RID: 10721 RVA: 0x000AEF5C File Offset: 0x000AD15C
		public AccompanyingCharacter GetAccompanyingCharacter(CharacterObject character)
		{
			return this.CharactersAccompanyingPlayer.Find(delegate(AccompanyingCharacter x)
			{
				LocationCharacter locationCharacter = x.LocationCharacter;
				return ((locationCharacter != null) ? locationCharacter.Character : null) == character;
			});
		}

		// Token: 0x060029E2 RID: 10722 RVA: 0x000AEF90 File Offset: 0x000AD190
		public void RemoveAccompanyingCharacter(LocationCharacter locationCharacter)
		{
			if (this.CharactersAccompanyingPlayer.Any<AccompanyingCharacter>((AccompanyingCharacter x) => x.LocationCharacter == locationCharacter))
			{
				AccompanyingCharacter accompanyingCharacter = this.CharactersAccompanyingPlayer.Find((AccompanyingCharacter x) => x.LocationCharacter == locationCharacter);
				this.CharactersAccompanyingPlayer.Remove(accompanyingCharacter);
			}
		}

		// Token: 0x060029E3 RID: 10723 RVA: 0x000AEFE8 File Offset: 0x000AD1E8
		public void RemoveAccompanyingCharacter(Hero hero)
		{
			for (int i = this.CharactersAccompanyingPlayer.Count - 1; i >= 0; i--)
			{
				if (this.CharactersAccompanyingPlayer[i].LocationCharacter.Character.IsHero && this.CharactersAccompanyingPlayer[i].LocationCharacter.Character.HeroObject == hero)
				{
					this.CharactersAccompanyingPlayer.Remove(this.CharactersAccompanyingPlayer[i]);
					return;
				}
			}
		}

		// Token: 0x060029E4 RID: 10724 RVA: 0x000AF061 File Offset: 0x000AD261
		public void RemoveAllAccompanyingCharacters()
		{
			this.CharactersAccompanyingPlayer.Clear();
		}

		// Token: 0x060029E5 RID: 10725 RVA: 0x000AF06E File Offset: 0x000AD26E
		public void OnCharacterLocationChanged(LocationCharacter locationCharacter, Location fromLocation, Location toLocation)
		{
			if ((fromLocation == CampaignMission.Current.Location && toLocation == null) || (fromLocation == null && toLocation == CampaignMission.Current.Location))
			{
				CampaignMission.Current.OnCharacterLocationChanged(locationCharacter, fromLocation, toLocation);
			}
		}

		// Token: 0x060029E6 RID: 10726 RVA: 0x000AF09D File Offset: 0x000AD29D
		public virtual bool IsWorkshopLocation(Location location)
		{
			return false;
		}

		// Token: 0x060029E7 RID: 10727 RVA: 0x000AF0A0 File Offset: 0x000AD2A0
		public virtual bool IsTavern(Location location)
		{
			return false;
		}

		// Token: 0x060029E8 RID: 10728 RVA: 0x000AF0A3 File Offset: 0x000AD2A3
		public virtual IMission CreateAndOpenMissionController(Location nextLocation, Location previousLocation = null, CharacterObject talkToChar = null, string playerSpecialSpawnTag = null)
		{
			return null;
		}

		// Token: 0x04000C1E RID: 3102
		public bool IsInsideOfASettlement;
	}
}
