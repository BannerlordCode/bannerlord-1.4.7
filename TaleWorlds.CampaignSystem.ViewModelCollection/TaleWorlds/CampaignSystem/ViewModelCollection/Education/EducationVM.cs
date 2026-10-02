using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.ViewModelCollection.Input;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Education
{
	// Token: 0x020000F6 RID: 246
	public class EducationVM : ViewModel
	{
		// Token: 0x06001633 RID: 5683 RVA: 0x00056EB4 File Offset: 0x000550B4
		public EducationVM(Hero child, Action<bool> onDone, Action<EducationCampaignBehavior.EducationCharacterProperties[]> onOptionSelect, Action<List<BasicCharacterObject>, List<Equipment>> sendPossibleCharactersAndEquipment)
		{
			this._onDone = onDone;
			this._onOptionSelect = onOptionSelect;
			this._sendPossibleCharactersAndEquipment = sendPossibleCharactersAndEquipment;
			this._child = child;
			this._educationBehavior = Campaign.Current.GetCampaignBehavior<IEducationLogic>();
			int num;
			this._educationBehavior.GetStageProperties(this._child, out num);
			this._pageCount = num + 1;
			this.GainedPropertiesController = new EducationGainedPropertiesVM(this._child, this._pageCount);
			this.Options = new MBBindingList<EducationOptionVM>();
			this.Review = new EducationReviewVM(this._pageCount);
			this.CanGoBack = true;
			this.InitWithStageIndex(0);
		}

		// Token: 0x06001634 RID: 5684 RVA: 0x00056F80 File Offset: 0x00055180
		public override void RefreshValues()
		{
			base.RefreshValues();
			TextObject currentPageTitleTextObj = this._currentPageTitleTextObj;
			this.StageTitleText = ((currentPageTitleTextObj != null) ? currentPageTitleTextObj.ToString() : null) ?? "";
			TextObject currentPageDescriptionTextObj = this._currentPageDescriptionTextObj;
			this.PageDescriptionText = ((currentPageDescriptionTextObj != null) ? currentPageDescriptionTextObj.ToString() : null) ?? "";
			TextObject currentPageInstructionTextObj = this._currentPageInstructionTextObj;
			this.ChooseText = ((currentPageInstructionTextObj != null) ? currentPageInstructionTextObj.ToString() : null) ?? "";
			this.Options.ApplyActionOnAllItems(delegate(EducationOptionVM o)
			{
				o.RefreshValues();
			});
			foreach (EducationOptionVM educationOptionVM in this.Options)
			{
				if (educationOptionVM.IsSelected)
				{
					this.OptionEffectText = educationOptionVM.OptionEffect;
					this.OptionDescriptionText = educationOptionVM.OptionDescription;
				}
			}
		}

		// Token: 0x06001635 RID: 5685 RVA: 0x0005707C File Offset: 0x0005527C
		private void InitWithStageIndex(int index)
		{
			this._latestOptionId = null;
			this.CanAdvance = false;
			this._currentPageIndex = index;
			this.OptionEffectText = "";
			this.OptionDescriptionText = "";
			this.Options.Clear();
			if (index < this._pageCount - 1)
			{
				List<BasicCharacterObject> list = new List<BasicCharacterObject>();
				List<Equipment> list2 = new List<Equipment>();
				TextObject textObject;
				TextObject textObject2;
				TextObject textObject3;
				EducationCampaignBehavior.EducationCharacterProperties[] array;
				string[] array2;
				this._educationBehavior.GetPageProperties(this._child, this._selectedOptions.Take<string>(index).ToList<string>(), out textObject, out textObject2, out textObject3, out array, out array2);
				this._currentPageTitleTextObj = textObject;
				this._currentPageDescriptionTextObj = textObject2;
				this._currentPageInstructionTextObj = textObject3;
				for (int i = 0; i < array2.Length; i++)
				{
					TextObject textObject4;
					TextObject textObject5;
					TextObject textObject6;
					ValueTuple<CharacterAttribute, int>[] array3;
					ValueTuple<SkillObject, int>[] array4;
					ValueTuple<SkillObject, int>[] array5;
					EducationCampaignBehavior.EducationCharacterProperties[] array6;
					this._educationBehavior.GetOptionProperties(this._child, array2[i], this._selectedOptions, out textObject4, out textObject5, out textObject6, out array3, out array4, out array5, out array6);
					this.Options.Add(new EducationOptionVM(new Action<object>(this.OnOptionSelect), array2[i], textObject4, textObject5, textObject6, false, array3, array4, array5, array6));
					foreach (EducationCampaignBehavior.EducationCharacterProperties educationCharacterProperties in array6)
					{
						if (educationCharacterProperties.Character != null && !list.Contains(educationCharacterProperties.Character))
						{
							list.Add(educationCharacterProperties.Character);
						}
						if (educationCharacterProperties.Equipment != null && !list2.Contains(educationCharacterProperties.Equipment))
						{
							list2.Add(educationCharacterProperties.Equipment);
						}
					}
				}
				this.OnlyHasOneOption = this.Options.Count == 1;
				if (this._selectedOptions.Count > index)
				{
					string text = this._selectedOptions[index];
					int num = array2.IndexOf(text);
					if (num >= 0)
					{
						Action<EducationCampaignBehavior.EducationCharacterProperties[]> onOptionSelect = this._onOptionSelect;
						if (onOptionSelect != null)
						{
							onOptionSelect(this.Options[num].CharacterProperties);
						}
						if (index == this._currentPageIndex)
						{
							this.Options[num].ExecuteAction();
							this.CanAdvance = true;
						}
					}
				}
				else
				{
					EducationCampaignBehavior.EducationCharacterProperties[] array8 = new EducationCampaignBehavior.EducationCharacterProperties[(array != null) ? array.Length : 1];
					for (int k = 0; k < ((array != null) ? array.Length : 0); k++)
					{
						array8[k] = array[k];
						if (array8[k].Character != null && !list.Contains(array8[k].Character))
						{
							list.Add(array8[k].Character);
						}
						if (array8[k].Equipment != null && !list2.Contains(array8[k].Equipment))
						{
							list2.Add(array8[k].Equipment);
						}
					}
					Action<EducationCampaignBehavior.EducationCharacterProperties[]> onOptionSelect2 = this._onOptionSelect;
					if (onOptionSelect2 != null)
					{
						onOptionSelect2(array8);
					}
				}
				if (this.OnlyHasOneOption)
				{
					this.Options[0].ExecuteAction();
				}
				this._sendPossibleCharactersAndEquipment(list, list2);
			}
			else
			{
				this._currentPageTitleTextObj = new TextObject("{=Ck9HT8fQ}Summary", null);
				this._currentPageInstructionTextObj = null;
				this._currentPageDescriptionTextObj = null;
				this.OnlyHasOneOption = false;
				this.CanAdvance = true;
			}
			TextObject currentPageTitleTextObj = this._currentPageTitleTextObj;
			this.StageTitleText = ((currentPageTitleTextObj != null) ? currentPageTitleTextObj.ToString() : null) ?? "";
			TextObject currentPageInstructionTextObj = this._currentPageInstructionTextObj;
			this.ChooseText = ((currentPageInstructionTextObj != null) ? currentPageInstructionTextObj.ToString() : null) ?? "";
			TextObject currentPageDescriptionTextObj = this._currentPageDescriptionTextObj;
			this.PageDescriptionText = ((currentPageDescriptionTextObj != null) ? currentPageDescriptionTextObj.ToString() : null) ?? "";
			if (this._currentPageIndex == 0)
			{
				this.NextText = this._nextPageTextObj.ToString();
				this.PreviousText = GameTexts.FindText("str_exit", null).ToString();
			}
			else if (this._currentPageIndex == this._pageCount - 1)
			{
				this.NextText = GameTexts.FindText("str_done", null).ToString();
				this.PreviousText = this._previousPageTextObj.ToString();
			}
			else
			{
				this.NextText = this._nextPageTextObj.ToString();
				this.PreviousText = this._previousPageTextObj.ToString();
			}
			this.UpdateGainedProperties();
			this.Review.SetCurrentPage(this._currentPageIndex);
		}

		// Token: 0x06001636 RID: 5686 RVA: 0x000574B0 File Offset: 0x000556B0
		private void OnOptionSelect(object optionIdAsObj)
		{
			if (optionIdAsObj != this._latestOptionId)
			{
				string optionId = (string)optionIdAsObj;
				EducationOptionVM educationOptionVM = this.Options.FirstOrDefault<EducationOptionVM>((EducationOptionVM o) => (string)o.Identifier == optionId);
				this.Options.ApplyActionOnAllItems(delegate(EducationOptionVM o)
				{
					o.IsSelected = false;
				});
				educationOptionVM.IsSelected = true;
				string actionText = educationOptionVM.ActionText;
				if (this._currentPageIndex == this._selectedOptions.Count)
				{
					this._selectedOptions.Add(optionId);
				}
				else if (this._currentPageIndex < this._selectedOptions.Count)
				{
					this._selectedOptions[this._currentPageIndex] = optionId;
				}
				else
				{
					Debug.FailedAssert("Skipped a stage for education!!!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem.ViewModelCollection\\Education\\EducationVM.cs", "OnOptionSelect", 210);
				}
				this.OptionEffectText = educationOptionVM.OptionEffect;
				this.OptionDescriptionText = educationOptionVM.OptionDescription;
				Action<EducationCampaignBehavior.EducationCharacterProperties[]> onOptionSelect = this._onOptionSelect;
				if (onOptionSelect != null)
				{
					onOptionSelect(educationOptionVM.CharacterProperties);
				}
				this.UpdateGainedProperties();
				this.CanAdvance = true;
				this._latestOptionId = optionIdAsObj;
				this.Review.SetGainForStage(this._currentPageIndex, this.OptionEffectText);
			}
		}

		// Token: 0x06001637 RID: 5687 RVA: 0x000575F0 File Offset: 0x000557F0
		private void UpdateGainedProperties()
		{
			this.GainedPropertiesController.UpdateWithSelections(this._selectedOptions, this._currentPageIndex);
		}

		// Token: 0x06001638 RID: 5688 RVA: 0x0005760C File Offset: 0x0005580C
		public void ExecuteNextStage()
		{
			if (this._currentPageIndex + 1 < this._pageCount)
			{
				this.InitWithStageIndex(this._currentPageIndex + 1);
				return;
			}
			this._educationBehavior.Finalize(this._child, this._selectedOptions);
			Action<bool> onDone = this._onDone;
			if (onDone == null)
			{
				return;
			}
			onDone(false);
		}

		// Token: 0x06001639 RID: 5689 RVA: 0x00057660 File Offset: 0x00055860
		public void ExecutePreviousStage()
		{
			if (this._currentPageIndex > 0)
			{
				this.InitWithStageIndex(this._currentPageIndex - 1);
				return;
			}
			if (this._currentPageIndex == 0)
			{
				Action<bool> onDone = this._onDone;
				if (onDone == null)
				{
					return;
				}
				onDone(true);
			}
		}

		// Token: 0x0600163A RID: 5690 RVA: 0x00057693 File Offset: 0x00055893
		public override void OnFinalize()
		{
			base.OnFinalize();
			InputKeyItemVM cancelInputKey = this.CancelInputKey;
			if (cancelInputKey != null)
			{
				cancelInputKey.OnFinalize();
			}
			InputKeyItemVM doneInputKey = this.DoneInputKey;
			if (doneInputKey == null)
			{
				return;
			}
			doneInputKey.OnFinalize();
		}

		// Token: 0x0600163B RID: 5691 RVA: 0x000576BC File Offset: 0x000558BC
		public void SetCancelInputKey(HotKey hotKey)
		{
			this.CancelInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x0600163C RID: 5692 RVA: 0x000576CB File Offset: 0x000558CB
		public void SetDoneInputKey(HotKey hotKey)
		{
			this.DoneInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x17000755 RID: 1877
		// (get) Token: 0x0600163D RID: 5693 RVA: 0x000576DA File Offset: 0x000558DA
		// (set) Token: 0x0600163E RID: 5694 RVA: 0x000576E2 File Offset: 0x000558E2
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

		// Token: 0x17000756 RID: 1878
		// (get) Token: 0x0600163F RID: 5695 RVA: 0x00057700 File Offset: 0x00055900
		// (set) Token: 0x06001640 RID: 5696 RVA: 0x00057708 File Offset: 0x00055908
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

		// Token: 0x17000757 RID: 1879
		// (get) Token: 0x06001641 RID: 5697 RVA: 0x00057726 File Offset: 0x00055926
		// (set) Token: 0x06001642 RID: 5698 RVA: 0x0005772E File Offset: 0x0005592E
		[DataSourceProperty]
		public string StageTitleText
		{
			get
			{
				return this._stageTitleText;
			}
			set
			{
				if (value != this._stageTitleText)
				{
					this._stageTitleText = value;
					base.OnPropertyChangedWithValue<string>(value, "StageTitleText");
				}
			}
		}

		// Token: 0x17000758 RID: 1880
		// (get) Token: 0x06001643 RID: 5699 RVA: 0x00057751 File Offset: 0x00055951
		// (set) Token: 0x06001644 RID: 5700 RVA: 0x00057759 File Offset: 0x00055959
		[DataSourceProperty]
		public string ChooseText
		{
			get
			{
				return this._chooseText;
			}
			set
			{
				if (value != this._chooseText)
				{
					this._chooseText = value;
					base.OnPropertyChangedWithValue<string>(value, "ChooseText");
				}
			}
		}

		// Token: 0x17000759 RID: 1881
		// (get) Token: 0x06001645 RID: 5701 RVA: 0x0005777C File Offset: 0x0005597C
		// (set) Token: 0x06001646 RID: 5702 RVA: 0x00057784 File Offset: 0x00055984
		[DataSourceProperty]
		public string PageDescriptionText
		{
			get
			{
				return this._pageDescriptionText;
			}
			set
			{
				if (value != this._pageDescriptionText)
				{
					this._pageDescriptionText = value;
					base.OnPropertyChangedWithValue<string>(value, "PageDescriptionText");
				}
			}
		}

		// Token: 0x1700075A RID: 1882
		// (get) Token: 0x06001647 RID: 5703 RVA: 0x000577A7 File Offset: 0x000559A7
		// (set) Token: 0x06001648 RID: 5704 RVA: 0x000577AF File Offset: 0x000559AF
		[DataSourceProperty]
		public string OptionEffectText
		{
			get
			{
				return this._optionEffectText;
			}
			set
			{
				if (value != this._optionEffectText)
				{
					this._optionEffectText = value;
					base.OnPropertyChangedWithValue<string>(value, "OptionEffectText");
				}
			}
		}

		// Token: 0x1700075B RID: 1883
		// (get) Token: 0x06001649 RID: 5705 RVA: 0x000577D2 File Offset: 0x000559D2
		// (set) Token: 0x0600164A RID: 5706 RVA: 0x000577DA File Offset: 0x000559DA
		[DataSourceProperty]
		public string OptionDescriptionText
		{
			get
			{
				return this._optionDescriptionText;
			}
			set
			{
				if (value != this._optionDescriptionText)
				{
					this._optionDescriptionText = value;
					base.OnPropertyChangedWithValue<string>(value, "OptionDescriptionText");
				}
			}
		}

		// Token: 0x1700075C RID: 1884
		// (get) Token: 0x0600164B RID: 5707 RVA: 0x000577FD File Offset: 0x000559FD
		// (set) Token: 0x0600164C RID: 5708 RVA: 0x00057805 File Offset: 0x00055A05
		[DataSourceProperty]
		public string NextText
		{
			get
			{
				return this._nextText;
			}
			set
			{
				if (value != this._nextText)
				{
					this._nextText = value;
					base.OnPropertyChangedWithValue<string>(value, "NextText");
				}
			}
		}

		// Token: 0x1700075D RID: 1885
		// (get) Token: 0x0600164D RID: 5709 RVA: 0x00057828 File Offset: 0x00055A28
		// (set) Token: 0x0600164E RID: 5710 RVA: 0x00057830 File Offset: 0x00055A30
		[DataSourceProperty]
		public string PreviousText
		{
			get
			{
				return this._previousText;
			}
			set
			{
				if (value != this._previousText)
				{
					this._previousText = value;
					base.OnPropertyChangedWithValue<string>(value, "PreviousText");
				}
			}
		}

		// Token: 0x1700075E RID: 1886
		// (get) Token: 0x0600164F RID: 5711 RVA: 0x00057853 File Offset: 0x00055A53
		// (set) Token: 0x06001650 RID: 5712 RVA: 0x0005785B File Offset: 0x00055A5B
		[DataSourceProperty]
		public bool CanAdvance
		{
			get
			{
				return this._canAdvance;
			}
			set
			{
				if (value != this._canAdvance)
				{
					this._canAdvance = value;
					base.OnPropertyChangedWithValue(value, "CanAdvance");
				}
			}
		}

		// Token: 0x1700075F RID: 1887
		// (get) Token: 0x06001651 RID: 5713 RVA: 0x00057879 File Offset: 0x00055A79
		// (set) Token: 0x06001652 RID: 5714 RVA: 0x00057881 File Offset: 0x00055A81
		[DataSourceProperty]
		public bool CanGoBack
		{
			get
			{
				return this._canGoBack;
			}
			set
			{
				if (value != this._canGoBack)
				{
					this._canGoBack = value;
					base.OnPropertyChangedWithValue(value, "CanGoBack");
				}
			}
		}

		// Token: 0x17000760 RID: 1888
		// (get) Token: 0x06001653 RID: 5715 RVA: 0x0005789F File Offset: 0x00055A9F
		// (set) Token: 0x06001654 RID: 5716 RVA: 0x000578A7 File Offset: 0x00055AA7
		[DataSourceProperty]
		public bool OnlyHasOneOption
		{
			get
			{
				return this._onlyHasOneOption;
			}
			set
			{
				if (value != this._onlyHasOneOption)
				{
					this._onlyHasOneOption = value;
					base.OnPropertyChangedWithValue(value, "OnlyHasOneOption");
				}
			}
		}

		// Token: 0x17000761 RID: 1889
		// (get) Token: 0x06001655 RID: 5717 RVA: 0x000578C5 File Offset: 0x00055AC5
		// (set) Token: 0x06001656 RID: 5718 RVA: 0x000578CD File Offset: 0x00055ACD
		[DataSourceProperty]
		public MBBindingList<EducationOptionVM> Options
		{
			get
			{
				return this._options;
			}
			set
			{
				if (value != this._options)
				{
					this._options = value;
					base.OnPropertyChangedWithValue<MBBindingList<EducationOptionVM>>(value, "Options");
				}
			}
		}

		// Token: 0x17000762 RID: 1890
		// (get) Token: 0x06001657 RID: 5719 RVA: 0x000578EB File Offset: 0x00055AEB
		// (set) Token: 0x06001658 RID: 5720 RVA: 0x000578F3 File Offset: 0x00055AF3
		[DataSourceProperty]
		public EducationGainedPropertiesVM GainedPropertiesController
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
					base.OnPropertyChangedWithValue<EducationGainedPropertiesVM>(value, "GainedPropertiesController");
				}
			}
		}

		// Token: 0x17000763 RID: 1891
		// (get) Token: 0x06001659 RID: 5721 RVA: 0x00057911 File Offset: 0x00055B11
		// (set) Token: 0x0600165A RID: 5722 RVA: 0x00057919 File Offset: 0x00055B19
		[DataSourceProperty]
		public EducationReviewVM Review
		{
			get
			{
				return this._review;
			}
			set
			{
				if (value != this._review)
				{
					this._review = value;
					base.OnPropertyChangedWithValue<EducationReviewVM>(value, "Review");
				}
			}
		}

		// Token: 0x04000A18 RID: 2584
		private readonly Action<bool> _onDone;

		// Token: 0x04000A19 RID: 2585
		private readonly Action<EducationCampaignBehavior.EducationCharacterProperties[]> _onOptionSelect;

		// Token: 0x04000A1A RID: 2586
		private readonly Action<List<BasicCharacterObject>, List<Equipment>> _sendPossibleCharactersAndEquipment;

		// Token: 0x04000A1B RID: 2587
		private readonly IEducationLogic _educationBehavior;

		// Token: 0x04000A1C RID: 2588
		private readonly Hero _child;

		// Token: 0x04000A1D RID: 2589
		private readonly TextObject _nextPageTextObj = new TextObject("{=Rvr1bcu8}Next", null);

		// Token: 0x04000A1E RID: 2590
		private readonly TextObject _previousPageTextObj = new TextObject("{=WXAaWZVf}Previous", null);

		// Token: 0x04000A1F RID: 2591
		private readonly int _pageCount;

		// Token: 0x04000A20 RID: 2592
		private readonly List<string> _selectedOptions = new List<string>();

		// Token: 0x04000A21 RID: 2593
		private TextObject _currentPageTitleTextObj;

		// Token: 0x04000A22 RID: 2594
		private TextObject _currentPageDescriptionTextObj;

		// Token: 0x04000A23 RID: 2595
		private TextObject _currentPageInstructionTextObj;

		// Token: 0x04000A24 RID: 2596
		private object _latestOptionId;

		// Token: 0x04000A25 RID: 2597
		private int _currentPageIndex;

		// Token: 0x04000A26 RID: 2598
		private InputKeyItemVM _cancelInputKey;

		// Token: 0x04000A27 RID: 2599
		private InputKeyItemVM _doneInputKey;

		// Token: 0x04000A28 RID: 2600
		private string _stageTitleText;

		// Token: 0x04000A29 RID: 2601
		private string _chooseText;

		// Token: 0x04000A2A RID: 2602
		private string _pageDescriptionText;

		// Token: 0x04000A2B RID: 2603
		private string _optionEffectText;

		// Token: 0x04000A2C RID: 2604
		private string _optionDescriptionText;

		// Token: 0x04000A2D RID: 2605
		private string _nextText;

		// Token: 0x04000A2E RID: 2606
		private string _previousText;

		// Token: 0x04000A2F RID: 2607
		private bool _canAdvance;

		// Token: 0x04000A30 RID: 2608
		private bool _canGoBack;

		// Token: 0x04000A31 RID: 2609
		private bool _onlyHasOneOption;

		// Token: 0x04000A32 RID: 2610
		private MBBindingList<EducationOptionVM> _options;

		// Token: 0x04000A33 RID: 2611
		private EducationGainedPropertiesVM _gainedPropertiesController;

		// Token: 0x04000A34 RID: 2612
		private EducationReviewVM _review;
	}
}
