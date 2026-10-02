using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.CampaignSystem.ViewModelCollection.Input;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.CharacterCreation.OptionsStage
{
	// Token: 0x02000158 RID: 344
	public class CharacterCreationOptionsStageVM : CharacterCreationStageBaseVM
	{
		// Token: 0x0600206E RID: 8302 RVA: 0x000766AC File Offset: 0x000748AC
		public CharacterCreationOptionsStageVM(CharacterCreationManager characterCreationManager, Action affirmativeAction, TextObject affirmativeActionText, Action negativeAction, TextObject negativeActionText)
			: base(characterCreationManager, affirmativeAction, affirmativeActionText, negativeAction, negativeActionText)
		{
			base.Title = GameTexts.FindText("str_difficulty", null).ToString();
			base.Description = GameTexts.FindText("str_determine_difficulty", null).ToString();
			MBBindingList<CampaignOptionItemVM> mbbindingList = new MBBindingList<CampaignOptionItemVM>();
			List<ICampaignOptionData> characterCreationCampaignOptions = CampaignOptionsManager.GetCharacterCreationCampaignOptions();
			for (int i = 0; i < characterCreationCampaignOptions.Count; i++)
			{
				mbbindingList.Add(new CampaignOptionItemVM(characterCreationCampaignOptions[i]));
			}
			this.OptionsController = new CampaignOptionsControllerVM(mbbindingList);
			base.CanAdvance = this.CanAdvanceToNextStage();
			this.CameraControlKeys = new MBBindingList<InputKeyItemVM>();
		}

		// Token: 0x0600206F RID: 8303 RVA: 0x00076744 File Offset: 0x00074944
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.OptionsController.RefreshValues();
		}

		// Token: 0x06002070 RID: 8304 RVA: 0x00076757 File Offset: 0x00074957
		private void OnOptionChange(string identifier)
		{
		}

		// Token: 0x06002071 RID: 8305 RVA: 0x00076759 File Offset: 0x00074959
		public override bool CanAdvanceToNextStage()
		{
			return true;
		}

		// Token: 0x06002072 RID: 8306 RVA: 0x0007675C File Offset: 0x0007495C
		public override void OnNextStage()
		{
			this._affirmativeAction();
		}

		// Token: 0x06002073 RID: 8307 RVA: 0x00076769 File Offset: 0x00074969
		public override void OnPreviousStage()
		{
			this._negativeAction();
		}

		// Token: 0x06002074 RID: 8308 RVA: 0x00076778 File Offset: 0x00074978
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

		// Token: 0x06002075 RID: 8309 RVA: 0x000767EC File Offset: 0x000749EC
		public void SetCancelInputKey(HotKey hotKey)
		{
			this.CancelInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x06002076 RID: 8310 RVA: 0x000767FB File Offset: 0x000749FB
		public void SetDoneInputKey(HotKey hotKey)
		{
			this.DoneInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x06002077 RID: 8311 RVA: 0x0007680C File Offset: 0x00074A0C
		public void AddCameraControlInputKey(HotKey hotKey)
		{
			InputKeyItemVM inputKeyItemVM = InputKeyItemVM.CreateFromHotKey(hotKey, true);
			this.CameraControlKeys.Add(inputKeyItemVM);
		}

		// Token: 0x06002078 RID: 8312 RVA: 0x00076830 File Offset: 0x00074A30
		public void AddCameraControlInputKey(GameKey gameKey)
		{
			InputKeyItemVM inputKeyItemVM = InputKeyItemVM.CreateFromGameKey(gameKey, true);
			this.CameraControlKeys.Add(inputKeyItemVM);
		}

		// Token: 0x06002079 RID: 8313 RVA: 0x00076854 File Offset: 0x00074A54
		public void AddCameraControlInputKey(GameAxisKey gameAxisKey, TextObject keyName)
		{
			InputKeyItemVM inputKeyItemVM = InputKeyItemVM.CreateFromForcedID(gameAxisKey.AxisKey.ToString(), keyName, true);
			this.CameraControlKeys.Add(inputKeyItemVM);
		}

		// Token: 0x17000B0A RID: 2826
		// (get) Token: 0x0600207A RID: 8314 RVA: 0x00076880 File Offset: 0x00074A80
		// (set) Token: 0x0600207B RID: 8315 RVA: 0x00076888 File Offset: 0x00074A88
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

		// Token: 0x17000B0B RID: 2827
		// (get) Token: 0x0600207C RID: 8316 RVA: 0x000768A6 File Offset: 0x00074AA6
		// (set) Token: 0x0600207D RID: 8317 RVA: 0x000768AE File Offset: 0x00074AAE
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

		// Token: 0x17000B0C RID: 2828
		// (get) Token: 0x0600207E RID: 8318 RVA: 0x000768CC File Offset: 0x00074ACC
		// (set) Token: 0x0600207F RID: 8319 RVA: 0x000768D4 File Offset: 0x00074AD4
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

		// Token: 0x17000B0D RID: 2829
		// (get) Token: 0x06002080 RID: 8320 RVA: 0x000768F2 File Offset: 0x00074AF2
		// (set) Token: 0x06002081 RID: 8321 RVA: 0x000768FA File Offset: 0x00074AFA
		[DataSourceProperty]
		public CampaignOptionsControllerVM OptionsController
		{
			get
			{
				return this._optionsController;
			}
			set
			{
				if (value != this._optionsController)
				{
					this._optionsController = value;
					base.OnPropertyChangedWithValue<CampaignOptionsControllerVM>(value, "OptionsController");
				}
			}
		}

		// Token: 0x17000B0E RID: 2830
		// (get) Token: 0x06002082 RID: 8322 RVA: 0x00076918 File Offset: 0x00074B18
		// (set) Token: 0x06002083 RID: 8323 RVA: 0x00076920 File Offset: 0x00074B20
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

		// Token: 0x04000F17 RID: 3863
		private InputKeyItemVM _cancelInputKey;

		// Token: 0x04000F18 RID: 3864
		private InputKeyItemVM _doneInputKey;

		// Token: 0x04000F19 RID: 3865
		private MBBindingList<InputKeyItemVM> _cameraControlKeys;

		// Token: 0x04000F1A RID: 3866
		private CampaignOptionsControllerVM _optionsController;

		// Token: 0x04000F1B RID: 3867
		private bool _characterGamepadControlsEnabled;
	}
}
