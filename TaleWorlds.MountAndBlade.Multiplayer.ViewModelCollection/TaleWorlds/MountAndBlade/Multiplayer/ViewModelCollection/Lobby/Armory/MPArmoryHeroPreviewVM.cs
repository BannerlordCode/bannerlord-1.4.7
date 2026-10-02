using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Armory
{
	// Token: 0x0200007B RID: 123
	public class MPArmoryHeroPreviewVM : ViewModel
	{
		// Token: 0x06000C3D RID: 3133 RVA: 0x00025F30 File Offset: 0x00024130
		public MPArmoryHeroPreviewVM()
		{
			this.HeroVisual = new CharacterViewModel();
		}

		// Token: 0x06000C3E RID: 3134 RVA: 0x00025F43 File Offset: 0x00024143
		public override void RefreshValues()
		{
			base.RefreshValues();
			BasicCharacterObject character = this._character;
			this.ClassName = ((character != null) ? character.Name.ToString() : null) ?? "";
		}

		// Token: 0x06000C3F RID: 3135 RVA: 0x00025F74 File Offset: 0x00024174
		public void SetCharacter(BasicCharacterObject character, DynamicBodyProperties dynamicBodyProperties, int race, bool isFemale)
		{
			this._character = character;
			this.HeroVisual.FillFrom(character, -1, null);
			this.HeroVisual.BodyProperties = new BodyProperties(dynamicBodyProperties, character.BodyPropertyRange.BodyPropertyMin.StaticProperties).ToString();
			this.HeroVisual.IsFemale = isFemale;
			this.HeroVisual.Race = race;
			this.ClassName = character.Name.ToString();
		}

		// Token: 0x06000C40 RID: 3136 RVA: 0x00025FF4 File Offset: 0x000241F4
		public void SetCharacterClass(BasicCharacterObject classCharacter)
		{
			this._character = classCharacter;
			this._orgEquipmentWithoutPerks = classCharacter.Equipment;
			this.HeroVisual.SetEquipment(this._orgEquipmentWithoutPerks);
			this.HeroVisual.ArmorColor1 = classCharacter.Culture.Color;
			this.HeroVisual.ArmorColor2 = classCharacter.Culture.Color2;
			if (NetworkMain.GameClient.PlayerData != null)
			{
				string text = NetworkMain.GameClient.PlayerData.Sigil;
				if (NetworkMain.GameClient.PlayerData.IsUsingClanSigil && NetworkMain.GameClient.ClanInfo != null)
				{
					text = NetworkMain.GameClient.ClanInfo.Sigil;
				}
				Banner banner = new Banner(text, classCharacter.Culture.BackgroundColor1, classCharacter.Culture.ForegroundColor1);
				this.HeroVisual.BannerCodeText = banner.BannerCode;
			}
			this.ClassName = classCharacter.Name.ToString();
		}

		// Token: 0x06000C41 RID: 3137 RVA: 0x000260DC File Offset: 0x000242DC
		public void SetCharacterPerks(List<IReadOnlyPerkObject> selectedPerks)
		{
			Equipment equipment = this._orgEquipmentWithoutPerks.Clone(false);
			MPArmoryVM.ApplyPerkEffectsToEquipment(ref equipment, selectedPerks);
			this.HeroVisual.SetEquipment(equipment);
		}

		// Token: 0x17000405 RID: 1029
		// (get) Token: 0x06000C42 RID: 3138 RVA: 0x0002610A File Offset: 0x0002430A
		// (set) Token: 0x06000C43 RID: 3139 RVA: 0x00026112 File Offset: 0x00024312
		[DataSourceProperty]
		public CharacterViewModel HeroVisual
		{
			get
			{
				return this._heroVisual;
			}
			set
			{
				if (value != this._heroVisual)
				{
					this._heroVisual = value;
					base.OnPropertyChangedWithValue<CharacterViewModel>(value, "HeroVisual");
				}
			}
		}

		// Token: 0x17000406 RID: 1030
		// (get) Token: 0x06000C44 RID: 3140 RVA: 0x00026130 File Offset: 0x00024330
		// (set) Token: 0x06000C45 RID: 3141 RVA: 0x00026138 File Offset: 0x00024338
		[DataSourceProperty]
		public string ClassName
		{
			get
			{
				return this._className;
			}
			set
			{
				if (value != this._className)
				{
					this._className = value;
					base.OnPropertyChangedWithValue<string>(value, "ClassName");
				}
			}
		}

		// Token: 0x04000591 RID: 1425
		private BasicCharacterObject _character;

		// Token: 0x04000592 RID: 1426
		private Equipment _orgEquipmentWithoutPerks;

		// Token: 0x04000593 RID: 1427
		private CharacterViewModel _heroVisual;

		// Token: 0x04000594 RID: 1428
		private string _className;
	}
}
