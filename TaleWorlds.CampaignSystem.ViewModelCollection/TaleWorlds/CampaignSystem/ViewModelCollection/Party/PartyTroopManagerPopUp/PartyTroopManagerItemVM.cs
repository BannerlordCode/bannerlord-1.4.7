using System;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Party.PartyTroopManagerPopUp
{
	// Token: 0x02000033 RID: 51
	public class PartyTroopManagerItemVM : ViewModel
	{
		// Token: 0x17000168 RID: 360
		// (get) Token: 0x060004FF RID: 1279 RVA: 0x0001C4AA File Offset: 0x0001A6AA
		// (set) Token: 0x06000500 RID: 1280 RVA: 0x0001C4B2 File Offset: 0x0001A6B2
		public Action<PartyTroopManagerItemVM> SetFocused { get; private set; }

		// Token: 0x06000501 RID: 1281 RVA: 0x0001C4BB File Offset: 0x0001A6BB
		public PartyTroopManagerItemVM(PartyCharacterVM baseTroop, Action<PartyTroopManagerItemVM> setFocused)
		{
			this.PartyCharacter = baseTroop;
			this.SetFocused = setFocused;
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x0001C4D1 File Offset: 0x0001A6D1
		public void ExecuteSetFocused()
		{
			if (this.PartyCharacter.Character != null)
			{
				Action<PartyTroopManagerItemVM> setFocused = this.SetFocused;
				if (setFocused != null)
				{
					setFocused(this);
				}
				this.IsFocused = true;
			}
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x0001C4F9 File Offset: 0x0001A6F9
		public void ExecuteSetUnfocused()
		{
			Action<PartyTroopManagerItemVM> setFocused = this.SetFocused;
			if (setFocused != null)
			{
				setFocused(null);
			}
			this.IsFocused = false;
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x0001C514 File Offset: 0x0001A714
		public void ExecuteOpenTroopEncyclopedia()
		{
			this.PartyCharacter.ExecuteOpenTroopEncyclopedia();
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x06000505 RID: 1285 RVA: 0x0001C521 File Offset: 0x0001A721
		// (set) Token: 0x06000506 RID: 1286 RVA: 0x0001C529 File Offset: 0x0001A729
		[DataSourceProperty]
		public bool IsFocused
		{
			get
			{
				return this._isFocused;
			}
			set
			{
				if (value != this._isFocused)
				{
					this._isFocused = value;
					base.OnPropertyChangedWithValue(value, "IsFocused");
				}
			}
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06000507 RID: 1287 RVA: 0x0001C547 File Offset: 0x0001A747
		// (set) Token: 0x06000508 RID: 1288 RVA: 0x0001C54F File Offset: 0x0001A74F
		[DataSourceProperty]
		public PartyCharacterVM PartyCharacter
		{
			get
			{
				return this._partyCharacter;
			}
			set
			{
				if (value != this._partyCharacter)
				{
					this._partyCharacter = value;
					base.OnPropertyChangedWithValue<PartyCharacterVM>(value, "PartyCharacter");
				}
			}
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06000509 RID: 1289 RVA: 0x0001C56D File Offset: 0x0001A76D
		// (set) Token: 0x0600050A RID: 1290 RVA: 0x0001C57A File Offset: 0x0001A77A
		[DataSourceProperty]
		public bool IsTroopUpgradable
		{
			get
			{
				return this.PartyCharacter.IsTroopUpgradable;
			}
			set
			{
				if (value != this.PartyCharacter.IsTroopUpgradable)
				{
					this.PartyCharacter.IsTroopUpgradable = value;
					base.OnPropertyChangedWithValue(value, "IsTroopUpgradable");
				}
			}
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x0600050B RID: 1291 RVA: 0x0001C5A2 File Offset: 0x0001A7A2
		// (set) Token: 0x0600050C RID: 1292 RVA: 0x0001C5AF File Offset: 0x0001A7AF
		[DataSourceProperty]
		public bool IsTroopRecruitable
		{
			get
			{
				return this.PartyCharacter.IsTroopRecruitable;
			}
			set
			{
				if (value != this.PartyCharacter.IsTroopRecruitable)
				{
					this.PartyCharacter.IsTroopRecruitable = value;
					base.OnPropertyChangedWithValue(value, "IsTroopRecruitable");
				}
			}
		}

		// Token: 0x0400022B RID: 555
		private bool _isFocused;

		// Token: 0x0400022C RID: 556
		private PartyCharacterVM _partyCharacter;
	}
}
