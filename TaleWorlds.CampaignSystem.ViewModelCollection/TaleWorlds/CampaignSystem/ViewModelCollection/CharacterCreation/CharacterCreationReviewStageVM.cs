using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ViewModelCollection.Input;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.CharacterCreation
{
	// Token: 0x02000156 RID: 342
	public class CharacterCreationReviewStageVM : CharacterCreationStageBaseVM
	{
		// Token: 0x06002035 RID: 8245 RVA: 0x00075D34 File Offset: 0x00073F34
		public CharacterCreationReviewStageVM(CharacterCreationManager characterCreationManager, Action affirmativeAction, TextObject affirmativeActionText, Action negativeAction, TextObject negativeActionText, bool isBannerAndClanNameSet)
			: base(characterCreationManager, affirmativeAction, affirmativeActionText, negativeAction, negativeActionText)
		{
			this.ReviewList = new MBBindingList<CharacterCreationReviewStageItemVM>();
			base.Title = new TextObject("{=txjiykNa}Review", null).ToString();
			base.Description = characterCreationManager.CharacterCreationContent.ReviewPageDescription.ToString();
			this._isBannerAndClanNameSet = isBannerAndClanNameSet;
			this.CannotAdvanceReasonHint = new HintViewModel();
			this.ClanBanner = new BannerImageIdentifierVM(Clan.PlayerClan.Banner, false);
			this.GainedPropertiesController = new CharacterCreationGainedPropertiesVM(this.CharacterCreationManager);
			this.Name = characterCreationManager.CharacterCreationContent.MainCharacterName;
			this.NameTextQuestion = new TextObject("{=mHVmrwRQ}Enter your name", null).ToString();
			this.AddReviewedItems();
			this.CameraControlKeys = new MBBindingList<InputKeyItemVM>();
		}

		// Token: 0x06002036 RID: 8246 RVA: 0x00075E10 File Offset: 0x00074010
		private void AddReviewedItems()
		{
			string text = string.Empty;
			CultureObject selectedCulture = this.CharacterCreationManager.CharacterCreationContent.SelectedCulture;
			IEnumerable<FeatObject> culturalFeats = selectedCulture.GetCulturalFeats((FeatObject x) => x.IsPositive);
			IEnumerable<FeatObject> culturalFeats2 = selectedCulture.GetCulturalFeats((FeatObject x) => !x.IsPositive);
			foreach (FeatObject featObject in culturalFeats)
			{
				GameTexts.SetVariable("STR1", text);
				GameTexts.SetVariable("STR2", featObject.Description);
				text = GameTexts.FindText("str_string_newline_string", null).ToString();
			}
			foreach (FeatObject featObject2 in culturalFeats2)
			{
				GameTexts.SetVariable("STR1", text);
				GameTexts.SetVariable("STR2", featObject2.Description);
				text = GameTexts.FindText("str_string_newline_string", null).ToString();
			}
			CharacterCreationReviewStageItemVM characterCreationReviewStageItemVM = new CharacterCreationReviewStageItemVM(new TextObject("{=K6GYskvJ}Culture:", null).ToString(), this.CharacterCreationManager.CharacterCreationContent.SelectedCulture.Name.ToString(), text);
			this.ReviewList.Add(characterCreationReviewStageItemVM);
			foreach (KeyValuePair<NarrativeMenu, NarrativeMenuOption> keyValuePair in this.CharacterCreationManager.SelectedOptions)
			{
				NarrativeMenu key = keyValuePair.Key;
				NarrativeMenuOption value = keyValuePair.Value;
				characterCreationReviewStageItemVM = new CharacterCreationReviewStageItemVM(key.Title.ToString(), value.Text.ToString(), value.PositiveEffectText.ToString());
				this.ReviewList.Add(characterCreationReviewStageItemVM);
			}
			if (this._isBannerAndClanNameSet)
			{
				CharacterCreationReviewStageItemVM characterCreationReviewStageItemVM2 = new CharacterCreationReviewStageItemVM(new BannerImageIdentifierVM(Clan.PlayerClan.Banner, true), GameTexts.FindText("str_clan", null).ToString(), Clan.PlayerClan.Name.ToString(), null);
				this.ReviewList.Add(characterCreationReviewStageItemVM2);
			}
		}

		// Token: 0x06002037 RID: 8247 RVA: 0x00076058 File Offset: 0x00074258
		public void ExecuteRandomizeName()
		{
			this.Name = NameGenerator.Current.GenerateFirstNameForPlayer(this.CharacterCreationManager.CharacterCreationContent.SelectedCulture, Hero.MainHero.IsFemale).ToString();
		}

		// Token: 0x06002038 RID: 8248 RVA: 0x0007608C File Offset: 0x0007428C
		private void OnRefresh()
		{
			TextObject textObject = GameTexts.FindText("str_generic_character_firstname", null);
			textObject.SetTextVariable("CHARACTER_FIRSTNAME", new TextObject(this.Name, null));
			TextObject textObject2 = GameTexts.FindText("str_generic_character_name", null);
			textObject2.SetTextVariable("CHARACTER_NAME", new TextObject(this.Name, null));
			textObject2.SetTextVariable("CHARACTER_GENDER", Hero.MainHero.IsFemale ? 1 : 0);
			textObject.SetTextVariable("CHARACTER_GENDER", Hero.MainHero.IsFemale ? 1 : 0);
			Hero.MainHero.SetName(textObject2, textObject);
			base.CanAdvance = this.CanAdvanceToNextStage();
		}

		// Token: 0x06002039 RID: 8249 RVA: 0x00076131 File Offset: 0x00074331
		public override void OnNextStage()
		{
			this._affirmativeAction();
		}

		// Token: 0x0600203A RID: 8250 RVA: 0x0007613E File Offset: 0x0007433E
		public override void OnPreviousStage()
		{
			this._negativeAction();
		}

		// Token: 0x0600203B RID: 8251 RVA: 0x0007614C File Offset: 0x0007434C
		public override bool CanAdvanceToNextStage()
		{
			TextObject textObject = TextObject.GetEmpty();
			bool flag = true;
			if (string.IsNullOrEmpty(this.Name) || string.IsNullOrWhiteSpace(this.Name))
			{
				textObject = new TextObject("{=IRcy3pWJ}Name cannot be empty", null);
				flag = false;
			}
			Tuple<bool, string> tuple = CampaignUIHelper.IsStringApplicableForHeroName(this.Name);
			if (!tuple.Item1)
			{
				if (!string.IsNullOrEmpty(tuple.Item2))
				{
					textObject = new TextObject("{=!}" + tuple.Item2, null);
				}
				flag = false;
			}
			this.CannotAdvanceReasonHint.HintText = textObject;
			return flag;
		}

		// Token: 0x0600203C RID: 8252 RVA: 0x000761D0 File Offset: 0x000743D0
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

		// Token: 0x0600203D RID: 8253 RVA: 0x00076244 File Offset: 0x00074444
		public void SetCancelInputKey(HotKey hotKey)
		{
			this.CancelInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x0600203E RID: 8254 RVA: 0x00076253 File Offset: 0x00074453
		public void SetDoneInputKey(HotKey hotKey)
		{
			this.DoneInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x0600203F RID: 8255 RVA: 0x00076264 File Offset: 0x00074464
		public void AddCameraControlInputKey(HotKey hotKey)
		{
			InputKeyItemVM inputKeyItemVM = InputKeyItemVM.CreateFromHotKey(hotKey, true);
			this.CameraControlKeys.Add(inputKeyItemVM);
		}

		// Token: 0x06002040 RID: 8256 RVA: 0x00076288 File Offset: 0x00074488
		public void AddCameraControlInputKey(GameKey gameKey)
		{
			InputKeyItemVM inputKeyItemVM = InputKeyItemVM.CreateFromGameKey(gameKey, true);
			this.CameraControlKeys.Add(inputKeyItemVM);
		}

		// Token: 0x06002041 RID: 8257 RVA: 0x000762AC File Offset: 0x000744AC
		public void AddCameraControlInputKey(GameAxisKey gameAxisKey, TextObject keyName)
		{
			InputKeyItemVM inputKeyItemVM = InputKeyItemVM.CreateFromForcedID(gameAxisKey.AxisKey.ToString(), keyName, true);
			this.CameraControlKeys.Add(inputKeyItemVM);
		}

		// Token: 0x17000AF6 RID: 2806
		// (get) Token: 0x06002042 RID: 8258 RVA: 0x000762D8 File Offset: 0x000744D8
		// (set) Token: 0x06002043 RID: 8259 RVA: 0x000762E0 File Offset: 0x000744E0
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

		// Token: 0x17000AF7 RID: 2807
		// (get) Token: 0x06002044 RID: 8260 RVA: 0x000762FE File Offset: 0x000744FE
		// (set) Token: 0x06002045 RID: 8261 RVA: 0x00076306 File Offset: 0x00074506
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

		// Token: 0x17000AF8 RID: 2808
		// (get) Token: 0x06002046 RID: 8262 RVA: 0x00076324 File Offset: 0x00074524
		// (set) Token: 0x06002047 RID: 8263 RVA: 0x0007632C File Offset: 0x0007452C
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

		// Token: 0x17000AF9 RID: 2809
		// (get) Token: 0x06002048 RID: 8264 RVA: 0x0007634A File Offset: 0x0007454A
		// (set) Token: 0x06002049 RID: 8265 RVA: 0x00076352 File Offset: 0x00074552
		[DataSourceProperty]
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				if (value != this._name)
				{
					this._name = value;
					this.CharacterCreationManager.CharacterCreationContent.SetMainCharacterName(value);
					base.OnPropertyChangedWithValue<string>(value, "Name");
					this.OnRefresh();
				}
			}
		}

		// Token: 0x17000AFA RID: 2810
		// (get) Token: 0x0600204A RID: 8266 RVA: 0x0007638C File Offset: 0x0007458C
		// (set) Token: 0x0600204B RID: 8267 RVA: 0x00076394 File Offset: 0x00074594
		[DataSourceProperty]
		public string NameTextQuestion
		{
			get
			{
				return this._nameTextQuestion;
			}
			set
			{
				if (value != this._nameTextQuestion)
				{
					this._nameTextQuestion = value;
					base.OnPropertyChangedWithValue<string>(value, "NameTextQuestion");
				}
			}
		}

		// Token: 0x17000AFB RID: 2811
		// (get) Token: 0x0600204C RID: 8268 RVA: 0x000763B7 File Offset: 0x000745B7
		// (set) Token: 0x0600204D RID: 8269 RVA: 0x000763BF File Offset: 0x000745BF
		[DataSourceProperty]
		public MBBindingList<CharacterCreationReviewStageItemVM> ReviewList
		{
			get
			{
				return this._reviewList;
			}
			set
			{
				if (value != this._reviewList)
				{
					this._reviewList = value;
					base.OnPropertyChangedWithValue<MBBindingList<CharacterCreationReviewStageItemVM>>(value, "ReviewList");
				}
			}
		}

		// Token: 0x17000AFC RID: 2812
		// (get) Token: 0x0600204E RID: 8270 RVA: 0x000763DD File Offset: 0x000745DD
		// (set) Token: 0x0600204F RID: 8271 RVA: 0x000763E5 File Offset: 0x000745E5
		[DataSourceProperty]
		public CharacterCreationGainedPropertiesVM GainedPropertiesController
		{
			get
			{
				return this._gainedPropertiesController;
			}
			set
			{
				if (value != this._gainedPropertiesController)
				{
					this._gainedPropertiesController = value;
					base.OnPropertyChangedWithValue<CharacterCreationGainedPropertiesVM>(value, "GainedPropertiesController");
				}
			}
		}

		// Token: 0x17000AFD RID: 2813
		// (get) Token: 0x06002050 RID: 8272 RVA: 0x00076403 File Offset: 0x00074603
		// (set) Token: 0x06002051 RID: 8273 RVA: 0x0007640B File Offset: 0x0007460B
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

		// Token: 0x17000AFE RID: 2814
		// (get) Token: 0x06002052 RID: 8274 RVA: 0x00076429 File Offset: 0x00074629
		// (set) Token: 0x06002053 RID: 8275 RVA: 0x00076431 File Offset: 0x00074631
		[DataSourceProperty]
		public HintViewModel CannotAdvanceReasonHint
		{
			get
			{
				return this._cannotAdvanceReasonHint;
			}
			set
			{
				if (value != this._cannotAdvanceReasonHint)
				{
					this._cannotAdvanceReasonHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "CannotAdvanceReasonHint");
				}
			}
		}

		// Token: 0x17000AFF RID: 2815
		// (get) Token: 0x06002054 RID: 8276 RVA: 0x0007644F File Offset: 0x0007464F
		// (set) Token: 0x06002055 RID: 8277 RVA: 0x00076457 File Offset: 0x00074657
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

		// Token: 0x04000EFD RID: 3837
		private bool _isBannerAndClanNameSet;

		// Token: 0x04000EFE RID: 3838
		private InputKeyItemVM _cancelInputKey;

		// Token: 0x04000EFF RID: 3839
		private InputKeyItemVM _doneInputKey;

		// Token: 0x04000F00 RID: 3840
		private MBBindingList<InputKeyItemVM> _cameraControlKeys;

		// Token: 0x04000F01 RID: 3841
		private string _name = "";

		// Token: 0x04000F02 RID: 3842
		private string _nameTextQuestion = "";

		// Token: 0x04000F03 RID: 3843
		private MBBindingList<CharacterCreationReviewStageItemVM> _reviewList;

		// Token: 0x04000F04 RID: 3844
		private CharacterCreationGainedPropertiesVM _gainedPropertiesController;

		// Token: 0x04000F05 RID: 3845
		private BannerImageIdentifierVM _clanBanner;

		// Token: 0x04000F06 RID: 3846
		private HintViewModel _cannotAdvanceReasonHint;

		// Token: 0x04000F07 RID: 3847
		private bool _characterGamepadControlsEnabled;
	}
}
