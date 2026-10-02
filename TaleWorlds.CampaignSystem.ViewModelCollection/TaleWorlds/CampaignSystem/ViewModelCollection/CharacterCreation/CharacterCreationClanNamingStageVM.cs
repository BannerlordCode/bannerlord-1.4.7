using System;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.CampaignSystem.ViewModelCollection.Input;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.CharacterCreation
{
	// Token: 0x0200014B RID: 331
	public class CharacterCreationClanNamingStageVM : CharacterCreationStageBaseVM
	{
		// Token: 0x17000ABF RID: 2751
		// (get) Token: 0x06001F91 RID: 8081 RVA: 0x00073D68 File Offset: 0x00071F68
		// (set) Token: 0x06001F92 RID: 8082 RVA: 0x00073D70 File Offset: 0x00071F70
		public BasicCharacterObject Character { get; private set; }

		// Token: 0x17000AC0 RID: 2752
		// (get) Token: 0x06001F93 RID: 8083 RVA: 0x00073D79 File Offset: 0x00071F79
		// (set) Token: 0x06001F94 RID: 8084 RVA: 0x00073D81 File Offset: 0x00071F81
		public int ShieldSlotIndex { get; private set; } = 3;

		// Token: 0x17000AC1 RID: 2753
		// (get) Token: 0x06001F95 RID: 8085 RVA: 0x00073D8A File Offset: 0x00071F8A
		// (set) Token: 0x06001F96 RID: 8086 RVA: 0x00073D92 File Offset: 0x00071F92
		public ItemRosterElement ShieldRosterElement { get; private set; }

		// Token: 0x06001F97 RID: 8087 RVA: 0x00073D9C File Offset: 0x00071F9C
		public CharacterCreationClanNamingStageVM(BasicCharacterObject character, CharacterCreationManager characterCreationManager, Action affirmativeAction, TextObject affirmativeActionText, Action negativeAction, TextObject negativeActionText)
			: base(characterCreationManager, affirmativeAction, affirmativeActionText, negativeAction, negativeActionText)
		{
			this.Character = character;
			this.ClanName = Hero.MainHero.Clan.Name.ToString();
			ItemObject itemObject = this.FindShield();
			this.ShieldRosterElement = new ItemRosterElement(itemObject, 1, null);
			this.ClanBanner = new BannerImageIdentifierVM(Hero.MainHero.Clan.Banner, true);
			this.CameraControlKeys = new MBBindingList<InputKeyItemVM>();
			this.RefreshValues();
		}

		// Token: 0x06001F98 RID: 8088 RVA: 0x00073E20 File Offset: 0x00072020
		public override void RefreshValues()
		{
			base.RefreshValues();
			base.Title = new TextObject("{=wNUcqcJP}Clan Name", null).ToString();
			base.Description = new TextObject("{=JJiKk4ow}Select your family name: ", null).ToString();
			this.BottomHintText = new TextObject("{=dbBAJ8yi}You can change your banner and clan name later on clan screen", null).ToString();
		}

		// Token: 0x06001F99 RID: 8089 RVA: 0x00073E78 File Offset: 0x00072078
		public override bool CanAdvanceToNextStage()
		{
			Tuple<bool, string> tuple = FactionHelper.IsClanNameApplicable(this.ClanName);
			this.ClanNameNotApplicableReason = tuple.Item2;
			return tuple.Item1;
		}

		// Token: 0x06001F9A RID: 8090 RVA: 0x00073EA3 File Offset: 0x000720A3
		public override void OnNextStage()
		{
			this._affirmativeAction();
		}

		// Token: 0x06001F9B RID: 8091 RVA: 0x00073EB0 File Offset: 0x000720B0
		public override void OnPreviousStage()
		{
			this._negativeAction();
		}

		// Token: 0x06001F9C RID: 8092 RVA: 0x00073EC0 File Offset: 0x000720C0
		private ItemObject FindShield()
		{
			for (int i = 0; i < 4; i++)
			{
				EquipmentElement equipmentFromSlot = this.Character.Equipment.GetEquipmentFromSlot((EquipmentIndex)i);
				ItemObject item = equipmentFromSlot.Item;
				if (((item != null) ? item.PrimaryWeapon : null) != null && equipmentFromSlot.Item.PrimaryWeapon.IsShield && equipmentFromSlot.Item.IsUsingTableau)
				{
					return equipmentFromSlot.Item;
				}
			}
			foreach (ItemObject itemObject in Game.Current.ObjectManager.GetObjectTypeList<ItemObject>())
			{
				if (itemObject.PrimaryWeapon != null && itemObject.PrimaryWeapon.IsShield && itemObject.IsUsingTableau)
				{
					return itemObject;
				}
			}
			return null;
		}

		// Token: 0x06001F9D RID: 8093 RVA: 0x00073F98 File Offset: 0x00072198
		public override void OnFinalize()
		{
			base.OnFinalize();
			InputKeyItemVM cancelInputKey = this.CancelInputKey;
			if (cancelInputKey != null)
			{
				cancelInputKey.OnFinalize();
			}
			InputKeyItemVM doneInputKey = this.DoneInputKey;
			if (doneInputKey != null)
			{
				doneInputKey.OnFinalize();
			}
			foreach (InputKeyItemVM inputKeyItemVM in this.CameraControlKeys)
			{
				inputKeyItemVM.OnFinalize();
			}
		}

		// Token: 0x06001F9E RID: 8094 RVA: 0x0007400C File Offset: 0x0007220C
		public void SetCancelInputKey(HotKey hotKey)
		{
			this.CancelInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x06001F9F RID: 8095 RVA: 0x0007401B File Offset: 0x0007221B
		public void SetDoneInputKey(HotKey hotKey)
		{
			this.DoneInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x06001FA0 RID: 8096 RVA: 0x0007402C File Offset: 0x0007222C
		public void AddCameraControlInputKey(HotKey hotKey)
		{
			InputKeyItemVM inputKeyItemVM = InputKeyItemVM.CreateFromHotKey(hotKey, true);
			this.CameraControlKeys.Add(inputKeyItemVM);
		}

		// Token: 0x06001FA1 RID: 8097 RVA: 0x00074050 File Offset: 0x00072250
		public void AddCameraControlInputKey(GameKey gameKey)
		{
			InputKeyItemVM inputKeyItemVM = InputKeyItemVM.CreateFromGameKey(gameKey, true);
			this.CameraControlKeys.Add(inputKeyItemVM);
		}

		// Token: 0x06001FA2 RID: 8098 RVA: 0x00074074 File Offset: 0x00072274
		public void AddCameraControlInputKey(GameAxisKey gameAxisKey, TextObject keyName)
		{
			InputKeyItemVM inputKeyItemVM = InputKeyItemVM.CreateFromForcedID(gameAxisKey.AxisKey.ToString(), keyName, true);
			this.CameraControlKeys.Add(inputKeyItemVM);
		}

		// Token: 0x17000AC2 RID: 2754
		// (get) Token: 0x06001FA3 RID: 8099 RVA: 0x000740A0 File Offset: 0x000722A0
		// (set) Token: 0x06001FA4 RID: 8100 RVA: 0x000740A8 File Offset: 0x000722A8
		[DataSourceProperty]
		public InputKeyItemVM CancelInputKey
		{
			get
			{
				return this._cancelInputKey;
			}
			set
			{
				if (value != this._cancelInputKey)
				{
					this._cancelInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "CancelInputKey");
				}
			}
		}

		// Token: 0x17000AC3 RID: 2755
		// (get) Token: 0x06001FA5 RID: 8101 RVA: 0x000740C6 File Offset: 0x000722C6
		// (set) Token: 0x06001FA6 RID: 8102 RVA: 0x000740CE File Offset: 0x000722CE
		[DataSourceProperty]
		public InputKeyItemVM DoneInputKey
		{
			get
			{
				return this._doneInputKey;
			}
			set
			{
				if (value != this._doneInputKey)
				{
					this._doneInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "DoneInputKey");
				}
			}
		}

		// Token: 0x17000AC4 RID: 2756
		// (get) Token: 0x06001FA7 RID: 8103 RVA: 0x000740EC File Offset: 0x000722EC
		// (set) Token: 0x06001FA8 RID: 8104 RVA: 0x000740F4 File Offset: 0x000722F4
		[DataSourceProperty]
		public MBBindingList<InputKeyItemVM> CameraControlKeys
		{
			get
			{
				return this._cameraControlKeys;
			}
			set
			{
				if (value != this._cameraControlKeys)
				{
					this._cameraControlKeys = value;
					base.OnPropertyChangedWithValue<MBBindingList<InputKeyItemVM>>(value, "CameraControlKeys");
				}
			}
		}

		// Token: 0x17000AC5 RID: 2757
		// (get) Token: 0x06001FA9 RID: 8105 RVA: 0x00074112 File Offset: 0x00072312
		// (set) Token: 0x06001FAA RID: 8106 RVA: 0x0007411A File Offset: 0x0007231A
		[DataSourceProperty]
		public string ClanName
		{
			get
			{
				return this._clanName;
			}
			set
			{
				if (value != this._clanName)
				{
					this._clanName = value;
					base.OnPropertyChangedWithValue<string>(value, "ClanName");
					base.CanAdvance = this.CanAdvanceToNextStage();
				}
			}
		}

		// Token: 0x17000AC6 RID: 2758
		// (get) Token: 0x06001FAB RID: 8107 RVA: 0x00074149 File Offset: 0x00072349
		// (set) Token: 0x06001FAC RID: 8108 RVA: 0x00074151 File Offset: 0x00072351
		[DataSourceProperty]
		public string ClanNameNotApplicableReason
		{
			get
			{
				return this._clanNameNotApplicableReason;
			}
			set
			{
				if (value != this._clanNameNotApplicableReason)
				{
					this._clanNameNotApplicableReason = value;
					base.OnPropertyChangedWithValue<string>(value, "ClanNameNotApplicableReason");
				}
			}
		}

		// Token: 0x17000AC7 RID: 2759
		// (get) Token: 0x06001FAD RID: 8109 RVA: 0x00074174 File Offset: 0x00072374
		// (set) Token: 0x06001FAE RID: 8110 RVA: 0x0007417C File Offset: 0x0007237C
		[DataSourceProperty]
		public string BottomHintText
		{
			get
			{
				return this._bottomHintText;
			}
			set
			{
				if (value != this._bottomHintText)
				{
					this._bottomHintText = value;
					base.OnPropertyChangedWithValue<string>(value, "BottomHintText");
				}
			}
		}

		// Token: 0x17000AC8 RID: 2760
		// (get) Token: 0x06001FAF RID: 8111 RVA: 0x0007419F File Offset: 0x0007239F
		// (set) Token: 0x06001FB0 RID: 8112 RVA: 0x000741A7 File Offset: 0x000723A7
		[DataSourceProperty]
		public BannerImageIdentifierVM ClanBanner
		{
			get
			{
				return this._clanBanner;
			}
			set
			{
				if (value != this._clanBanner)
				{
					this._clanBanner = value;
					base.OnPropertyChangedWithValue<BannerImageIdentifierVM>(value, "ClanBanner");
				}
			}
		}

		// Token: 0x17000AC9 RID: 2761
		// (get) Token: 0x06001FB1 RID: 8113 RVA: 0x000741C5 File Offset: 0x000723C5
		// (set) Token: 0x06001FB2 RID: 8114 RVA: 0x000741CD File Offset: 0x000723CD
		[DataSourceProperty]
		public bool CharacterGamepadControlsEnabled
		{
			get
			{
				return this._characterGamepadControlsEnabled;
			}
			set
			{
				if (value != this._characterGamepadControlsEnabled)
				{
					this._characterGamepadControlsEnabled = value;
					base.OnPropertyChangedWithValue(value, "CharacterGamepadControlsEnabled");
				}
			}
		}

		// Token: 0x04000EBF RID: 3775
		private InputKeyItemVM _cancelInputKey;

		// Token: 0x04000EC0 RID: 3776
		private InputKeyItemVM _doneInputKey;

		// Token: 0x04000EC1 RID: 3777
		private MBBindingList<InputKeyItemVM> _cameraControlKeys;

		// Token: 0x04000EC2 RID: 3778
		private string _clanName;

		// Token: 0x04000EC3 RID: 3779
		private string _clanNameNotApplicableReason;

		// Token: 0x04000EC4 RID: 3780
		private string _bottomHintText;

		// Token: 0x04000EC5 RID: 3781
		private BannerImageIdentifierVM _clanBanner;

		// Token: 0x04000EC6 RID: 3782
		private bool _characterGamepadControlsEnabled;
	}
}
