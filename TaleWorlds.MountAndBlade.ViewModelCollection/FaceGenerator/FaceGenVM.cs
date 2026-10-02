using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Core.ViewModelCollection.Selector;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.FaceGenerator
{
	// Token: 0x0200007E RID: 126
	public class FaceGenVM : ViewModel
	{
		// Token: 0x170002EC RID: 748
		// (get) Token: 0x060009E2 RID: 2530 RVA: 0x000224AE File Offset: 0x000206AE
		private bool _isAgeAvailable
		{
			get
			{
				return this._openedFromMultiplayer || this._showDebugValues;
			}
		}

		// Token: 0x170002ED RID: 749
		// (get) Token: 0x060009E3 RID: 2531 RVA: 0x000224C0 File Offset: 0x000206C0
		private bool _isWeightAvailable
		{
			get
			{
				return !this._openedFromMultiplayer || this._showDebugValues;
			}
		}

		// Token: 0x170002EE RID: 750
		// (get) Token: 0x060009E4 RID: 2532 RVA: 0x000224D2 File Offset: 0x000206D2
		private bool _isBuildAvailable
		{
			get
			{
				return !this._openedFromMultiplayer || this._showDebugValues;
			}
		}

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x060009E5 RID: 2533 RVA: 0x000224E4 File Offset: 0x000206E4
		private bool _isRaceAvailable
		{
			get
			{
				return (FaceGen.GetRaceCount() > 1 && !this._openedFromMultiplayer) || this._showDebugValues;
			}
		}

		// Token: 0x060009E6 RID: 2534 RVA: 0x000224FE File Offset: 0x000206FE
		public void SetFaceGenerationParams(FaceGenerationParams faceGenerationParams)
		{
			this._faceGenerationParams = faceGenerationParams;
		}

		// Token: 0x060009E7 RID: 2535 RVA: 0x00022508 File Offset: 0x00020708
		public FaceGenVM(BodyGenerator bodyGenerator, IFaceGeneratorHandler faceGeneratorScreen, Action<float> onHeightChanged, Action onAgeChanged, TextObject affirmitiveText, TextObject negativeText, int currentStageIndex, int totalStagesCount, int furthestIndex, Action<int> goToIndex, bool canChangeGender, bool openedFromMultiplayer, IFaceGeneratorCustomFilter filter)
		{
			this._bodyGenerator = bodyGenerator;
			this._faceGeneratorScreen = faceGeneratorScreen;
			this._showDebugValues = FaceGen.ShowDebugValues;
			this._affirmitiveText = affirmitiveText;
			this._negativeText = negativeText;
			this._openedFromMultiplayer = openedFromMultiplayer;
			this._filter = filter;
			this.CanChangeGender = canChangeGender || this._showDebugValues;
			this._onHeightChanged = onHeightChanged;
			this._onAgeChanged = onAgeChanged;
			this._goToIndex = goToIndex;
			this.TotalStageCount = totalStagesCount;
			this.CurrentStageIndex = currentStageIndex;
			this.FurthestIndex = furthestIndex;
			this.CameraControlKeys = new MBBindingList<InputKeyItemVM>();
			this.BodyProperties = new MBBindingList<FaceGenPropertyVM>();
			this.FaceProperties = new MBBindingList<FaceGenPropertyVM>();
			this.EyesProperties = new MBBindingList<FaceGenPropertyVM>();
			this.NoseProperties = new MBBindingList<FaceGenPropertyVM>();
			this.MouthProperties = new MBBindingList<FaceGenPropertyVM>();
			this.HairProperties = new MBBindingList<FaceGenPropertyVM>();
			this.TaintProperties = new MBBindingList<FaceGenPropertyVM>();
			this._tabProperties = new Dictionary<FaceGenVM.FaceGenTabs, MBBindingList<FaceGenPropertyVM>>
			{
				{
					FaceGenVM.FaceGenTabs.Body,
					this.BodyProperties
				},
				{
					FaceGenVM.FaceGenTabs.Face,
					this.FaceProperties
				},
				{
					FaceGenVM.FaceGenTabs.Eyes,
					this.EyesProperties
				},
				{
					FaceGenVM.FaceGenTabs.Nose,
					this.NoseProperties
				},
				{
					FaceGenVM.FaceGenTabs.Mouth,
					this.MouthProperties
				},
				{
					FaceGenVM.FaceGenTabs.Hair,
					this.HairProperties
				},
				{
					FaceGenVM.FaceGenTabs.Taint,
					this.TaintProperties
				}
			};
			this.TaintTypes = new MBBindingList<FacegenListItemVM>();
			this.BeardTypes = new MBBindingList<FacegenListItemVM>();
			this.HairTypes = new MBBindingList<FacegenListItemVM>();
			this.IsDressed = false;
			this.genderBasedSelectedValues = new FaceGenVM.GenderBasedSelectedValue[2];
			this.genderBasedSelectedValues[0].Reset();
			this.genderBasedSelectedValues[1].Reset();
			this._undoCommands = new List<UndoRedoKey>(100);
			this._redoCommands = new List<UndoRedoKey>(100);
			this.IsUndoEnabled = false;
			this.IsRedoEnabled = false;
			this._initialValues = new Dictionary<string, float>();
			this.CanChangeRace = this._isRaceAvailable;
			this.RefreshValues();
		}

		// Token: 0x060009E8 RID: 2536 RVA: 0x00022750 File Offset: 0x00020950
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.BodyHint = new HintViewModel(GameTexts.FindText("str_body", null), null);
			this.FaceHint = new HintViewModel(GameTexts.FindText("str_face", null), null);
			this.EyesHint = new HintViewModel(GameTexts.FindText("str_eyes", null), null);
			this.NoseHint = new HintViewModel(GameTexts.FindText("str_nose", null), null);
			this.HairHint = new HintViewModel(GameTexts.FindText("str_hair", null), null);
			this.TaintHint = new HintViewModel(GameTexts.FindText("str_face_gen_markings", null), null);
			this.MouthHint = new HintViewModel(GameTexts.FindText("str_mouth", null), null);
			this.RedoHint = new HintViewModel(GameTexts.FindText("str_redo", null), null);
			this.UndoHint = new HintViewModel(GameTexts.FindText("str_undo", null), null);
			this.RandomizeHint = new HintViewModel(GameTexts.FindText("str_randomize", null), null);
			this.RandomizeAllHint = new HintViewModel(GameTexts.FindText("str_randomize_all", null), null);
			this.ResetHint = new HintViewModel(GameTexts.FindText("str_reset", null), null);
			this.ResetAllHint = new HintViewModel(GameTexts.FindText("str_reset_all", null), null);
			this.ClothHint = new HintViewModel(GameTexts.FindText("str_face_gen_change_cloth", null), null);
			this.FlipHairLbl = new TextObject("{=74PKmRWJ}Flip Hair", null).ToString();
			this.SkinColorLbl = GameTexts.FindText("sf_facegen_skin_color", null).ToString();
			this.GenderLbl = GameTexts.FindText("sf_facegen_gender", null).ToString();
			this.RaceLbl = GameTexts.FindText("sf_facegen_race", null).ToString();
			this.Title = GameTexts.FindText("sf_facegen_title", null).ToString();
			this.DoneBtnLbl = this._affirmitiveText.ToString();
			this.CancelBtnLbl = this._negativeText.ToString();
			FacegenListItemVM selectedTaintType = this._selectedTaintType;
			if (selectedTaintType != null)
			{
				selectedTaintType.RefreshValues();
			}
			FacegenListItemVM selectedBeardType = this._selectedBeardType;
			if (selectedBeardType != null)
			{
				selectedBeardType.RefreshValues();
			}
			FacegenListItemVM selectedHairType = this._selectedHairType;
			if (selectedHairType != null)
			{
				selectedHairType.RefreshValues();
			}
			this._bodyProperties.ApplyActionOnAllItems(delegate(FaceGenPropertyVM x)
			{
				x.RefreshValues();
			});
			this._faceProperties.ApplyActionOnAllItems(delegate(FaceGenPropertyVM x)
			{
				x.RefreshValues();
			});
			this._eyesProperties.ApplyActionOnAllItems(delegate(FaceGenPropertyVM x)
			{
				x.RefreshValues();
			});
			this._noseProperties.ApplyActionOnAllItems(delegate(FaceGenPropertyVM x)
			{
				x.RefreshValues();
			});
			this._mouthProperties.ApplyActionOnAllItems(delegate(FaceGenPropertyVM x)
			{
				x.RefreshValues();
			});
			this._hairProperties.ApplyActionOnAllItems(delegate(FaceGenPropertyVM x)
			{
				x.RefreshValues();
			});
			this._taintProperties.ApplyActionOnAllItems(delegate(FaceGenPropertyVM x)
			{
				x.RefreshValues();
			});
			this._taintTypes.ApplyActionOnAllItems(delegate(FacegenListItemVM x)
			{
				x.RefreshValues();
			});
			this._beardTypes.ApplyActionOnAllItems(delegate(FacegenListItemVM x)
			{
				x.RefreshValues();
			});
			this._hairTypes.ApplyActionOnAllItems(delegate(FacegenListItemVM x)
			{
				x.RefreshValues();
			});
			FaceGenPropertyVM soundPreset = this._soundPreset;
			if (soundPreset != null)
			{
				soundPreset.RefreshValues();
			}
			FaceGenPropertyVM faceTypes = this._faceTypes;
			if (faceTypes != null)
			{
				faceTypes.RefreshValues();
			}
			FaceGenPropertyVM teethTypes = this._teethTypes;
			if (teethTypes != null)
			{
				teethTypes.RefreshValues();
			}
			FaceGenPropertyVM eyebrowTypes = this._eyebrowTypes;
			if (eyebrowTypes != null)
			{
				eyebrowTypes.RefreshValues();
			}
			SelectorVM<SelectorItemVM> skinColorSelector = this._skinColorSelector;
			if (skinColorSelector != null)
			{
				skinColorSelector.RefreshValues();
			}
			SelectorVM<SelectorItemVM> hairColorSelector = this._hairColorSelector;
			if (hairColorSelector != null)
			{
				hairColorSelector.RefreshValues();
			}
			SelectorVM<SelectorItemVM> tattooColorSelector = this._tattooColorSelector;
			if (tattooColorSelector != null)
			{
				tattooColorSelector.RefreshValues();
			}
			SelectorVM<SelectorItemVM> raceSelector = this._raceSelector;
			if (raceSelector == null)
			{
				return;
			}
			raceSelector.RefreshValues();
		}

		// Token: 0x060009E9 RID: 2537 RVA: 0x00022B94 File Offset: 0x00020D94
		public void InitializeHistory(FaceGenHistory faceGenHistory)
		{
			if (faceGenHistory != null)
			{
				if (faceGenHistory.UndoCommands != null)
				{
					this._undoCommands = faceGenHistory.UndoCommands;
				}
				if (faceGenHistory.RedoCommands != null)
				{
					this._redoCommands = faceGenHistory.RedoCommands;
				}
				if (faceGenHistory.InitialValues != null)
				{
					this._initialValues = faceGenHistory.InitialValues;
				}
			}
			this.IsUndoEnabled = this._undoCommands.Count > 0;
			this.IsRedoEnabled = this._redoCommands.Count > 0;
		}

		// Token: 0x060009EA RID: 2538 RVA: 0x00022C08 File Offset: 0x00020E08
		private void FilterCategories()
		{
			FaceGeneratorStage[] availableStages = this._filter.GetAvailableStages();
			this.IsBodyEnabled = availableStages.Contains(FaceGeneratorStage.Body);
			this.IsFaceEnabled = availableStages.Contains(FaceGeneratorStage.Face);
			this.IsEyesEnabled = availableStages.Contains(FaceGeneratorStage.Eyes);
			this.IsNoseEnabled = availableStages.Contains(FaceGeneratorStage.Nose);
			this.IsMouthEnabled = availableStages.Contains(FaceGeneratorStage.Mouth);
			this.IsHairEnabled = availableStages.Contains(FaceGeneratorStage.Hair);
			this.IsTaintEnabled = availableStages.Contains(FaceGeneratorStage.Taint);
			this.Tab = (int)availableStages.FirstOrDefault<FaceGeneratorStage>();
		}

		// Token: 0x060009EB RID: 2539 RVA: 0x00022C88 File Offset: 0x00020E88
		private void SetColorCodes()
		{
			this._skinColors = MBBodyProperties.GetSkinColorGradientPoints(this._selectedRace, this.SelectedGender, (int)this._bodyGenerator.Character.Age);
			this._hairColors = MBBodyProperties.GetHairColorGradientPoints(this._selectedRace, this.SelectedGender, (int)this._bodyGenerator.Character.Age);
			this._tattooColors = MBBodyProperties.GetTatooColorGradientPoints(this._selectedRace, this.SelectedGender, (int)this._bodyGenerator.Character.Age);
			this.SkinColorSelector = new SelectorVM<SelectorItemVM>(this._skinColors.Select<uint, string>(delegate(uint t)
			{
				t %= 4278190080U;
				return "#" + Convert.ToString((long)((ulong)t), 16).PadLeft(6, '0').ToUpper() + "FF";
			}).ToList<string>(), MathF.Round(this._faceGenerationParams.CurrentSkinColorOffset * (float)(this._skinColors.Count - 1)), new Action<SelectorVM<SelectorItemVM>>(this.OnSelectSkinColor));
			this.HairColorSelector = new SelectorVM<SelectorItemVM>(this._hairColors.Select<uint, string>(delegate(uint t)
			{
				t %= 4278190080U;
				return "#" + Convert.ToString((long)((ulong)t), 16).PadLeft(6, '0').ToUpper() + "FF";
			}).ToList<string>(), MathF.Round(this._faceGenerationParams.CurrentHairColorOffset * (float)(this._hairColors.Count - 1)), new Action<SelectorVM<SelectorItemVM>>(this.OnSelectHairColor));
			this.TattooColorSelector = new SelectorVM<SelectorItemVM>(this._tattooColors.Select<uint, string>(delegate(uint t)
			{
				t %= 4278190080U;
				return "#" + Convert.ToString((long)((ulong)t), 16).PadLeft(6, '0').ToUpper() + "FF";
			}).ToList<string>(), MathF.Round(this._faceGenerationParams.CurrentFaceTattooColorOffset1 * (float)(this._tattooColors.Count - 1)), new Action<SelectorVM<SelectorItemVM>>(this.OnSelectTattooColor));
		}

		// Token: 0x060009EC RID: 2540 RVA: 0x00022E3C File Offset: 0x0002103C
		private void OnSelectSkinColor(SelectorVM<SelectorItemVM> s)
		{
			this.AddCommand();
			this._faceGenerationParams.CurrentSkinColorOffset = (float)s.SelectedIndex / (float)(this._skinColors.Count - 1);
			this.UpdateFace();
		}

		// Token: 0x060009ED RID: 2541 RVA: 0x00022E6B File Offset: 0x0002106B
		private void OnSelectTattooColor(SelectorVM<SelectorItemVM> s)
		{
			this.AddCommand();
			this._faceGenerationParams.CurrentFaceTattooColorOffset1 = (float)s.SelectedIndex / (float)(this._tattooColors.Count - 1);
			this.UpdateFace();
		}

		// Token: 0x060009EE RID: 2542 RVA: 0x00022E9A File Offset: 0x0002109A
		private void OnSelectHairColor(SelectorVM<SelectorItemVM> s)
		{
			this.AddCommand();
			this._faceGenerationParams.CurrentHairColorOffset = (float)s.SelectedIndex / (float)(this._hairColors.Count - 1);
			this.UpdateFace();
		}

		// Token: 0x060009EF RID: 2543 RVA: 0x00022ECC File Offset: 0x000210CC
		private void OnSelectRace(SelectorVM<SelectorItemVM> s)
		{
			this.AddCommand();
			this._selectedRace = s.SelectedIndex;
			if (this._initialRace == -1 && !this.TryGetInitialValue("SelectedRace", ref this._initialRace))
			{
				this.SetOrAddInitialValue("SelectedRace", (float)this._selectedRace);
			}
			this.UpdateRaceAndGenderBasedResources();
			this.Refresh(true);
		}

		// Token: 0x060009F0 RID: 2544 RVA: 0x00022F26 File Offset: 0x00021126
		private void OnHeightChanged()
		{
			Action<float> onHeightChanged = this._onHeightChanged;
			if (onHeightChanged == null)
			{
				return;
			}
			FaceGenPropertyVM heightSlider = this._heightSlider;
			onHeightChanged((heightSlider != null) ? heightSlider.Value : 0f);
		}

		// Token: 0x060009F1 RID: 2545 RVA: 0x00022F4E File Offset: 0x0002114E
		private void OnAgeChanged()
		{
			Action onAgeChanged = this._onAgeChanged;
			if (onAgeChanged == null)
			{
				return;
			}
			onAgeChanged();
		}

		// Token: 0x060009F2 RID: 2546 RVA: 0x00022F60 File Offset: 0x00021160
		private void SetTabAvailabilities()
		{
			this._tabAvailabilities = new MBList<bool> { this.IsBodyEnabled, this.IsFaceEnabled, this.IsEyesEnabled, this.IsNoseEnabled, this.IsMouthEnabled, this.IsHairEnabled, this.IsTaintEnabled };
		}

		// Token: 0x060009F3 RID: 2547 RVA: 0x00022FCC File Offset: 0x000211CC
		public void OnTabClicked(int index)
		{
			this.Tab = index;
		}

		// Token: 0x060009F4 RID: 2548 RVA: 0x00022FD8 File Offset: 0x000211D8
		public void SelectPreviousTab()
		{
			int num = ((this.Tab <= 0) ? 6 : (this.Tab - 1));
			while (!this._tabAvailabilities[num] && num != this.Tab)
			{
				num = ((num <= 0) ? 6 : (num - 1));
			}
			this.Tab = num;
		}

		// Token: 0x060009F5 RID: 2549 RVA: 0x00023028 File Offset: 0x00021228
		public void SelectNextTab()
		{
			int num = (this.Tab + 1) % 7;
			while (!this._tabAvailabilities[num] && num != this.Tab)
			{
				num = (num + 1) % 7;
			}
			this.Tab = num;
		}

		// Token: 0x060009F6 RID: 2550 RVA: 0x00023068 File Offset: 0x00021268
		public void Refresh(bool clearProperties)
		{
			if (!this._characterRefreshEnabled)
			{
				return;
			}
			this._characterRefreshEnabled = false;
			base.OnPropertyChanged("FlipHairCb");
			this._selectedRace = this._faceGenerationParams.CurrentRace;
			this.SelectedGender = this._faceGenerationParams.CurrentGender;
			this.SetColorCodes();
			int num = 0;
			MBBodyProperties.GetParamsMax(this._selectedRace, this.SelectedGender, (int)this._faceGenerationParams.CurrentAge, ref num, ref this.beardNum, ref this.faceTextureNum, ref this.mouthTextureNum, ref this.faceTattooNum, ref this._newSoundPresetSize, ref this.eyebrowTextureNum, ref this._scale);
			this.HairNum = num;
			MBBodyProperties.GetZeroProbabilities(this._selectedRace, this.SelectedGender, this._faceGenerationParams.CurrentAge, ref this._faceGenerationParams.TattooZeroProbability);
			if (clearProperties)
			{
				foreach (KeyValuePair<FaceGenVM.FaceGenTabs, MBBindingList<FaceGenPropertyVM>> keyValuePair in this._tabProperties)
				{
					keyValuePair.Value.Clear();
				}
			}
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			if (clearProperties)
			{
				int faceGenInstancesLength = MBBodyProperties.GetFaceGenInstancesLength(this._faceGenerationParams.CurrentRace, this._faceGenerationParams.CurrentGender, (int)this._faceGenerationParams.CurrentAge);
				for (int i = 0; i < faceGenInstancesLength; i++)
				{
					DeformKeyData deformKeyData = MBBodyProperties.GetDeformKeyData(i, this._faceGenerationParams.CurrentRace, this._faceGenerationParams.CurrentGender, (int)this._faceGenerationParams.CurrentAge);
					TextObject textObject = new TextObject("{=bsiRNJtk}{NAME}:", null);
					textObject.SetTextVariable("NAME", GameTexts.FindText("str_facegen_skin", deformKeyData.Id));
					if (GameTexts.FindText("str_facegen_skin", deformKeyData.Id).ToString().Contains("exist"))
					{
						Debug.FailedAssert(deformKeyData.Id + " id name doesn't exist", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.ViewModelCollection\\FaceGenerator\\FaceGenVM.cs", "Refresh", 455);
					}
					if (deformKeyData.Id == "weight")
					{
						num3 = deformKeyData.KeyTimePoint;
					}
					else if (deformKeyData.Id == "build")
					{
						num5 = deformKeyData.KeyTimePoint;
					}
					else if (deformKeyData.Id == "height")
					{
						num2 = deformKeyData.KeyTimePoint;
					}
					else if (deformKeyData.Id == "age")
					{
						num4 = deformKeyData.KeyTimePoint;
					}
					else
					{
						float num6 = this._faceGenerationParams.KeyWeights[i];
						float num7 = num6;
						if (!this.TryGetInitialValue(textObject.ToString(), ref num7))
						{
							this.SetOrAddInitialValue(textObject.ToString(), num7);
						}
						FaceGenPropertyVM faceGenPropertyVM = new FaceGenPropertyVM(i, 0.0, 1.0, textObject, deformKeyData.KeyTimePoint, deformKeyData.GroupId, (double)num6, num7, new Action<int, float, bool, bool>(this.UpdateFace), new Action(this.AddCommand), new Action(this.ResetSliderPrevValues), true, false, false);
						if (deformKeyData.GroupId > -1 && deformKeyData.GroupId < 7)
						{
							this._tabProperties[(FaceGenVM.FaceGenTabs)deformKeyData.GroupId].Add(faceGenPropertyVM);
						}
					}
				}
			}
			if (this._filter != null)
			{
				this.FilterCategories();
			}
			else if (this._tab == -1)
			{
				this.IsBodyEnabled = true;
				this.IsFaceEnabled = true;
				this.IsEyesEnabled = true;
				this.IsNoseEnabled = true;
				this.IsMouthEnabled = true;
				this.IsHairEnabled = true;
				this.IsTaintEnabled = true;
				this.Tab = 0;
			}
			this.SetTabAvailabilities();
			if (clearProperties)
			{
				TextObject textObject2 = new TextObject("{=G6hYIR5k}Voice Pitch:", null);
				float voicePitch = this._faceGenerationParams.VoicePitch;
				float num8 = voicePitch;
				if (!this.TryGetInitialValue(textObject2.ToString(), ref num8))
				{
					this.SetOrAddInitialValue(textObject2.ToString(), num8);
				}
				FaceGenPropertyVM faceGenPropertyVM = new FaceGenPropertyVM(-19, 0.0, 1.0, textObject2, -19, 0, (double)voicePitch, num8, new Action<int, float, bool, bool>(this.UpdateFace), new Action(this.AddCommand), new Action(this.ResetSliderPrevValues), true, false, false);
				this._tabProperties[FaceGenVM.FaceGenTabs.Body].Add(faceGenPropertyVM);
				TextObject textObject3 = new TextObject("{=cLJdeUWz}Height:", null);
				float num9 = ((this._heightSlider == null) ? this._faceGenerationParams.HeightMultiplier : this._heightSlider.Value);
				float num10 = num9;
				if (!this.TryGetInitialValue(textObject3.ToString(), ref num10))
				{
					this.SetOrAddInitialValue(textObject3.ToString(), num10);
				}
				this._heightSlider = new FaceGenPropertyVM(-16, (double)(this._openedFromMultiplayer ? 0.25f : 0f), (double)(this._openedFromMultiplayer ? 0.75f : 1f), textObject3, num2, 0, (double)num9, num10, new Action<int, float, bool, bool>(this.UpdateFace), new Action(this.AddCommand), new Action(this.ResetSliderPrevValues), true, false, false);
				this._tabProperties[FaceGenVM.FaceGenTabs.Body].Add(this._heightSlider);
				this.UpdateVoiceIndiciesFromCurrentParameters();
				if (this._isAgeAvailable)
				{
					double num11 = (double)(this._openedFromMultiplayer ? 25 : 3);
					TextObject textObject4 = new TextObject("{=H1emUb6k}Age:", null);
					float currentAge = this._faceGenerationParams.CurrentAge;
					float num12 = currentAge;
					if (!this.TryGetInitialValue(textObject4.ToString(), ref num12))
					{
						this.SetOrAddInitialValue(textObject4.ToString(), num12);
					}
					faceGenPropertyVM = new FaceGenPropertyVM(-11, num11, 128.0, textObject4, num4, 0, (double)currentAge, num12, new Action<int, float, bool, bool>(this.UpdateFace), new Action(this.AddCommand), new Action(this.ResetSliderPrevValues), true, false, false);
					this._tabProperties[FaceGenVM.FaceGenTabs.Body].Add(faceGenPropertyVM);
				}
				if (this._isWeightAvailable)
				{
					TextObject textObject5 = new TextObject("{=zBld61ck}Weight:", null);
					float currentWeight = this._faceGenerationParams.CurrentWeight;
					float num13 = currentWeight;
					if (!this.TryGetInitialValue(textObject5.ToString(), ref num13))
					{
						this.SetOrAddInitialValue(textObject5.ToString(), num13);
					}
					faceGenPropertyVM = new FaceGenPropertyVM(-17, 0.0, 1.0, textObject5, num3, 0, (double)currentWeight, num13, new Action<int, float, bool, bool>(this.UpdateFace), new Action(this.AddCommand), new Action(this.ResetSliderPrevValues), true, false, false);
					this._tabProperties[FaceGenVM.FaceGenTabs.Body].Add(faceGenPropertyVM);
				}
				if (this._isBuildAvailable)
				{
					TextObject textObject6 = new TextObject("{=EUAKPHek}Build:", null);
					float currentBuild = this._faceGenerationParams.CurrentBuild;
					float num14 = currentBuild;
					if (!this.TryGetInitialValue(textObject6.ToString(), ref num14))
					{
						this.SetOrAddInitialValue(textObject6.ToString(), num14);
					}
					faceGenPropertyVM = new FaceGenPropertyVM(-18, 0.0, 1.0, textObject6, num5, 0, (double)currentBuild, num14, new Action<int, float, bool, bool>(this.UpdateFace), new Action(this.AddCommand), new Action(this.ResetSliderPrevValues), true, false, false);
					this._tabProperties[FaceGenVM.FaceGenTabs.Body].Add(faceGenPropertyVM);
				}
				TextObject textObject7 = new TextObject("{=qXxpITdc}Eye Color:", null);
				float currentEyeColorOffset = this._faceGenerationParams.CurrentEyeColorOffset;
				float num15 = currentEyeColorOffset;
				if (!this.TryGetInitialValue(textObject7.ToString(), ref num15))
				{
					this.SetOrAddInitialValue(textObject7.ToString(), num15);
				}
				faceGenPropertyVM = new FaceGenPropertyVM(-12, 0.0, 1.0, textObject7, -12, 2, (double)currentEyeColorOffset, num15, new Action<int, float, bool, bool>(this.UpdateFace), new Action(this.AddCommand), new Action(this.ResetSliderPrevValues), true, false, false);
				this._tabProperties[FaceGenVM.FaceGenTabs.Eyes].Add(faceGenPropertyVM);
				this.RaceSelector = new SelectorVM<SelectorItemVM>(FaceGen.GetRaceNames(), this._selectedRace, new Action<SelectorVM<SelectorItemVM>>(this.OnSelectRace));
			}
			this.UpdateRaceAndGenderBasedResources();
			if (!this._initialValuesSet)
			{
				this._initialSelectedTaintType = this._faceGenerationParams.CurrentFaceTattoo;
				if (!this.TryGetInitialValue("SelectedTaintType", ref this._initialSelectedTaintType))
				{
					this.SetOrAddInitialValue("SelectedTaintType", (float)this._initialSelectedTaintType);
				}
				this._initialSelectedBeardType = this._faceGenerationParams.CurrentBeard;
				if (!this.TryGetInitialValue("SelectedBeardType", ref this._initialSelectedBeardType))
				{
					this.SetOrAddInitialValue("SelectedBeardType", (float)this._initialSelectedBeardType);
				}
				this._initialSelectedHairType = this._faceGenerationParams.CurrentHair;
				if (!this.TryGetInitialValue("SelectedHairType", ref this._initialSelectedHairType))
				{
					this.SetOrAddInitialValue("SelectedHairType", (float)this._initialSelectedHairType);
				}
				this._initialSelectedHairColor = this._faceGenerationParams.CurrentHairColorOffset;
				if (!this.TryGetInitialValue("SelectedHairColor", ref this._initialSelectedHairColor))
				{
					this.SetOrAddInitialValue("SelectedHairColor", this._initialSelectedHairColor);
				}
				this._initialSelectedSkinColor = this._faceGenerationParams.CurrentSkinColorOffset;
				if (!this.TryGetInitialValue("SelectedSkinColor", ref this._initialSelectedSkinColor))
				{
					this.SetOrAddInitialValue("SelectedSkinColor", this._initialSelectedSkinColor);
				}
				this._initialSelectedTaintColor = this._faceGenerationParams.CurrentFaceTattooColorOffset1;
				if (!this.TryGetInitialValue("SelectedTaintColor", ref this._initialSelectedTaintColor))
				{
					this.SetOrAddInitialValue("SelectedTaintColor", this._initialSelectedTaintColor);
				}
				this._initialRace = this._selectedRace;
				if (!this.TryGetInitialValue("SelectedRace", ref this._initialRace))
				{
					this.SetOrAddInitialValue("SelectedRace", (float)this._initialRace);
				}
				this._initialGender = this.SelectedGender;
				if (!this.TryGetInitialValue("SelectedGender", ref this._initialGender))
				{
					this.SetOrAddInitialValue("SelectedGender", (float)this._initialGender);
				}
				this._initialValuesSet = true;
			}
			this._characterRefreshEnabled = true;
			this.UpdateFace();
		}

		// Token: 0x060009F7 RID: 2551 RVA: 0x000239F4 File Offset: 0x00021BF4
		private bool TryGetInitialValue(string propertyName, ref float initialValue)
		{
			float num;
			if (this._initialValues.TryGetValue(propertyName, out num))
			{
				initialValue = num;
				return true;
			}
			return false;
		}

		// Token: 0x060009F8 RID: 2552 RVA: 0x00023A18 File Offset: 0x00021C18
		private bool TryGetInitialValue(string propertyName, ref int initialValue)
		{
			float num;
			if (this._initialValues.TryGetValue(propertyName, out num))
			{
				initialValue = (int)num;
				return true;
			}
			return false;
		}

		// Token: 0x060009F9 RID: 2553 RVA: 0x00023A3C File Offset: 0x00021C3C
		private void SetOrAddInitialValue(string propertyName, float initialValue)
		{
			if (this._initialValues.ContainsKey(propertyName))
			{
				this._initialValues[propertyName] = initialValue;
				return;
			}
			this._initialValues.Add(propertyName, initialValue);
		}

		// Token: 0x060009FA RID: 2554 RVA: 0x00023A68 File Offset: 0x00021C68
		private void UpdateRaceAndGenderBasedResources()
		{
			int num = 0;
			MBBodyProperties.GetParamsMax(this._selectedRace, this.SelectedGender, (int)this._faceGenerationParams.CurrentAge, ref num, ref this.beardNum, ref this.faceTextureNum, ref this.mouthTextureNum, ref this.faceTattooNum, ref this._newSoundPresetSize, ref this.eyebrowTextureNum, ref this._scale);
			this.HairNum = num;
			int[] array = Enumerable.Range(0, num).ToArray<int>();
			int[] array2 = Enumerable.Range(0, this.beardNum).ToArray<int>();
			if (this._filter != null)
			{
				array = this._filter.GetHaircutIndices(this._bodyGenerator.Character);
				array2 = this._filter.GetFacialHairIndices(this._bodyGenerator.Character);
			}
			this.BeardTypes.Clear();
			for (int i = 0; i < this.beardNum; i++)
			{
				if (array2.Contains(i) || i == this._faceGenerationParams.CurrentBeard)
				{
					FacegenListItemVM facegenListItemVM = new FacegenListItemVM("FaceGen\\Beard\\img" + i, i, new Action<FacegenListItemVM, bool>(this.SetSelectedBeardType));
					this.BeardTypes.Add(facegenListItemVM);
				}
			}
			string text = ((this._selectedGender == 1) ? "Female" : "Male");
			this.HairTypes.Clear();
			for (int j = 0; j < num; j++)
			{
				if (array.Contains(j) || j == this._faceGenerationParams.CurrentHair)
				{
					FacegenListItemVM facegenListItemVM2 = new FacegenListItemVM(string.Concat(new object[] { "FaceGen\\Hair\\", text, "\\img", j }), j, new Action<FacegenListItemVM, bool>(this.SetSelectedHairType));
					this.HairTypes.Add(facegenListItemVM2);
				}
			}
			this.TaintTypes.Clear();
			for (int k = 0; k < this.faceTattooNum; k++)
			{
				FacegenListItemVM facegenListItemVM3 = new FacegenListItemVM(string.Concat(new object[] { "FaceGen\\Tattoo\\", text, "\\img", k }), k, new Action<FacegenListItemVM, bool>(this.SetSelectedTattooType));
				this.TaintTypes.Add(facegenListItemVM3);
			}
			this.UpdateFace(-20, (float)this._selectedRace, true, true);
			this.UpdateFace(-1, (float)this._selectedGender, true, true);
			if (this.BeardTypes.Count > 0)
			{
				this.SetSelectedBeardType(this._faceGenerationParams.CurrentBeard, false);
			}
			this.SetSelectedHairType(this._faceGenerationParams.CurrentHair, false);
			if (this.TaintTypes.Count > 0)
			{
				this.SetSelectedTattooType(this._faceGenerationParams.CurrentFaceTattoo, false);
			}
			this.UpdateVoiceIndiciesFromCurrentParameters();
			this._faceGenerationParams.CurrentFaceTexture = MBMath.ClampInt(this._faceGenerationParams.CurrentFaceTexture, 0, this.faceTextureNum - 1);
			TextObject textObject = new TextObject("{=DmaP2qaR}Skin Type", null);
			int currentFaceTexture = this._faceGenerationParams.CurrentFaceTexture;
			int num2 = currentFaceTexture;
			if (!this.TryGetInitialValue(textObject.ToString(), ref num2))
			{
				this.SetOrAddInitialValue(textObject.ToString(), (float)num2);
			}
			this.FaceTypes = new FaceGenPropertyVM(-3, 0.0, (double)(this.faceTextureNum - 1), textObject, -3, 1, (double)currentFaceTexture, (float)num2, new Action<int, float, bool, bool>(this.UpdateFace), new Action(this.AddCommand), new Action(this.ResetSliderPrevValues), true, false, true);
			this._faceGenerationParams.CurrentMouthTexture = MBMath.ClampInt(this._faceGenerationParams.CurrentMouthTexture, 0, this.mouthTextureNum - 1);
			TextObject textObject2 = new TextObject("{=l2CNxPXG}Teeth Type", null);
			int currentMouthTexture = this._faceGenerationParams.CurrentMouthTexture;
			int num3 = currentMouthTexture;
			this.TeethTypes = new FaceGenPropertyVM(-14, 0.0, (double)(this.mouthTextureNum - 1), textObject2, -14, 4, (double)currentMouthTexture, (float)num3, new Action<int, float, bool, bool>(this.UpdateFace), new Action(this.AddCommand), new Action(this.ResetSliderPrevValues), true, false, true);
			this._faceGenerationParams.CurrentEyebrow = MBMath.ClampInt(this._faceGenerationParams.CurrentEyebrow, 0, this.eyebrowTextureNum - 1);
			TextObject textObject3 = new TextObject("{=bIcFZT6L}Eyebrow Type", null);
			int currentEyebrow = this._faceGenerationParams.CurrentEyebrow;
			int num4 = currentEyebrow;
			this.EyebrowTypes = new FaceGenPropertyVM(-15, 0.0, (double)(this.eyebrowTextureNum - 1), textObject3, -15, 4, (double)currentEyebrow, (float)num4, new Action<int, float, bool, bool>(this.UpdateFace), new Action(this.AddCommand), new Action(this.ResetSliderPrevValues), true, false, true);
		}

		// Token: 0x060009FB RID: 2555 RVA: 0x00023EDC File Offset: 0x000220DC
		private void UpdateVoiceIndiciesFromCurrentParameters()
		{
			this._isVoiceTypeUsableForOnlyNpc = MBBodyProperties.GetVoiceTypeUsableForPlayerData(this._faceGenerationParams.CurrentRace, this.SelectedGender, (float)((int)this._faceGenerationParams.CurrentAge), this._newSoundPresetSize);
			int num = 0;
			for (int i = 0; i < this._isVoiceTypeUsableForOnlyNpc.Count; i++)
			{
				if (!this._isVoiceTypeUsableForOnlyNpc[i])
				{
					num++;
				}
			}
			TextObject textObject = new TextObject("{=macpKFaG}Voice", null);
			int voiceUIIndex = this.GetVoiceUIIndex();
			int num2 = voiceUIIndex;
			this.SoundPreset = new FaceGenPropertyVM(-9, 0.0, (double)(num - 1), textObject, -9, 0, (double)voiceUIIndex, (float)num2, new Action<int, float, bool, bool>(this.UpdateFace), new Action(this.AddCommand), new Action(this.ResetSliderPrevValues), true, false, true);
			Debug.Print("Called GetVoiceTypeUsableForPlayerData", 0, Debug.DebugColor.White, 17592186044416UL);
		}

		// Token: 0x060009FC RID: 2556 RVA: 0x00023FBB File Offset: 0x000221BB
		private void UpdateFace()
		{
			if (this._characterRefreshEnabled)
			{
				this._bodyGenerator.RefreshFace(this._faceGenerationParams, this.IsDressed);
				this._faceGeneratorScreen.RefreshCharacterEntity();
			}
			this.SaveGenderBasedSelectedValues();
		}

		// Token: 0x060009FD RID: 2557 RVA: 0x00023FF0 File Offset: 0x000221F0
		private void UpdateFace(int keyNo, float value, bool calledFromInit, bool isNeedRefresh = true)
		{
			if (this._enforceConstraints)
			{
				return;
			}
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			if (keyNo > -1)
			{
				if (keyNo < this._faceGenerationParams.KeyWeights.Length)
				{
					this._faceGenerationParams.KeyWeights[keyNo] = value;
					this._enforceConstraints = MBBodyProperties.EnforceConstraints(ref this._faceGenerationParams);
					flag3 = this._enforceConstraints && !calledFromInit;
				}
			}
			else
			{
				switch (keyNo)
				{
				case -20:
					this.RestoreRaceGenderBasedSelectedValues();
					this._faceGenerationParams.SetRaceGenderAndAdjustParams((int)value, this.SelectedGender, (int)this._faceGenerationParams.CurrentAge);
					goto IL_02DE;
				case -19:
					this._faceGenerationParams.VoicePitch = value;
					goto IL_02DE;
				case -18:
					this._faceGenerationParams.CurrentBuild = value;
					this._enforceConstraints = MBBodyProperties.EnforceConstraints(ref this._faceGenerationParams);
					flag3 = this._enforceConstraints && !calledFromInit;
					goto IL_02DE;
				case -17:
					this._faceGenerationParams.CurrentWeight = value;
					this._enforceConstraints = MBBodyProperties.EnforceConstraints(ref this._faceGenerationParams);
					flag3 = this._enforceConstraints && !calledFromInit;
					goto IL_02DE;
				case -16:
					this._faceGenerationParams.HeightMultiplier = (this._openedFromMultiplayer ? MathF.Clamp(value, 0.25f, 0.75f) : MathF.Clamp(value, 0f, 1f));
					this._enforceConstraints = MBBodyProperties.EnforceConstraints(ref this._faceGenerationParams);
					flag3 = this._enforceConstraints && !calledFromInit;
					flag2 = true;
					goto IL_02DE;
				case -15:
					this._faceGenerationParams.CurrentEyebrow = (int)value;
					goto IL_02DE;
				case -14:
					this._faceGenerationParams.CurrentMouthTexture = (int)value;
					goto IL_02DE;
				case -12:
					this._faceGenerationParams.CurrentEyeColorOffset = value;
					goto IL_02DE;
				case -11:
				{
					this._faceGenerationParams.CurrentAge = value;
					this._enforceConstraints = MBBodyProperties.EnforceConstraints(ref this._faceGenerationParams);
					flag3 = this._enforceConstraints && !calledFromInit;
					flag = true;
					flag2 = true;
					BodyMeshMaturityType maturityTypeWithAge = FaceGen.GetMaturityTypeWithAge(this._faceGenerationParams.CurrentAge);
					if (this._latestMaturityType != maturityTypeWithAge)
					{
						this.UpdateVoiceIndiciesFromCurrentParameters();
						this._latestMaturityType = maturityTypeWithAge;
						goto IL_02DE;
					}
					goto IL_02DE;
				}
				case -10:
					this._faceGenerationParams.CurrentFaceTattoo = (int)value;
					goto IL_02DE;
				case -9:
					this._faceGenerationParams.CurrentVoice = this.GetVoiceRealIndex((int)value);
					goto IL_02DE;
				case -7:
					this._faceGenerationParams.CurrentBeard = (int)value;
					goto IL_02DE;
				case -6:
					this._faceGenerationParams.CurrentHair = (int)value;
					goto IL_02DE;
				case -3:
					this._faceGenerationParams.CurrentFaceTexture = (int)value;
					goto IL_02DE;
				case -1:
					this.RestoreRaceGenderBasedSelectedValues();
					this._faceGenerationParams.SetRaceGenderAndAdjustParams(this._faceGenerationParams.CurrentRace, (int)value, (int)this._faceGenerationParams.CurrentAge);
					goto IL_02DE;
				}
				MBDebug.ShowWarning("Unknown preset!");
			}
			IL_02DE:
			if (flag3)
			{
				this.UpdateFacegen();
			}
			if (isNeedRefresh)
			{
				this.UpdateFace();
			}
			else
			{
				this.SaveGenderBasedSelectedValues();
			}
			if (!calledFromInit && !this._isRandomizing && keyNo < 0)
			{
				if (keyNo != -14)
				{
					if (keyNo == -9)
					{
						this._faceGeneratorScreen.MakeVoiceDelayed();
					}
				}
				else
				{
					this._faceGeneratorScreen.SetFacialAnimation("facegen_teeth", false);
				}
			}
			this._enforceConstraints = false;
			if (flag)
			{
				this.OnAgeChanged();
			}
			if (flag2)
			{
				this.OnHeightChanged();
			}
		}

		// Token: 0x060009FE RID: 2558 RVA: 0x0002434C File Offset: 0x0002254C
		private void RestoreRaceGenderBasedSelectedValues()
		{
			if (this.genderBasedSelectedValues[this.SelectedGender].FaceTexture > -1)
			{
				this._faceGenerationParams.CurrentFaceTexture = this.genderBasedSelectedValues[this.SelectedGender].FaceTexture;
			}
			if (this.genderBasedSelectedValues[this.SelectedGender].Hair > -1)
			{
				this._faceGenerationParams.CurrentHair = this.genderBasedSelectedValues[this.SelectedGender].Hair;
			}
			if (this.genderBasedSelectedValues[this.SelectedGender].Beard > -1)
			{
				this._faceGenerationParams.CurrentBeard = this.genderBasedSelectedValues[this.SelectedGender].Beard;
			}
			if (this.genderBasedSelectedValues[this.SelectedGender].Tattoo > -1)
			{
				this._faceGenerationParams.CurrentFaceTattoo = this.genderBasedSelectedValues[this.SelectedGender].Tattoo;
			}
			if (this.genderBasedSelectedValues[this.SelectedGender].SoundPreset > -1)
			{
				this._faceGenerationParams.CurrentVoice = this.genderBasedSelectedValues[this.SelectedGender].SoundPreset;
			}
			if (this.genderBasedSelectedValues[this.SelectedGender].MouthTexture > -1)
			{
				this._faceGenerationParams.CurrentMouthTexture = this.genderBasedSelectedValues[this.SelectedGender].MouthTexture;
			}
			if (this.genderBasedSelectedValues[this.SelectedGender].EyebrowTexture > -1)
			{
				this._faceGenerationParams.CurrentEyebrow = this.genderBasedSelectedValues[this.SelectedGender].EyebrowTexture;
			}
		}

		// Token: 0x060009FF RID: 2559 RVA: 0x000244F0 File Offset: 0x000226F0
		private void SaveGenderBasedSelectedValues()
		{
			this.genderBasedSelectedValues[this.SelectedGender].FaceTexture = this._faceGenerationParams.CurrentFaceTexture;
			this.genderBasedSelectedValues[this.SelectedGender].Hair = this._faceGenerationParams.CurrentHair;
			this.genderBasedSelectedValues[this.SelectedGender].Beard = this._faceGenerationParams.CurrentBeard;
			this.genderBasedSelectedValues[this.SelectedGender].Tattoo = this._faceGenerationParams.CurrentFaceTattoo;
			this.genderBasedSelectedValues[this.SelectedGender].SoundPreset = this._faceGenerationParams.CurrentVoice;
			this.genderBasedSelectedValues[this.SelectedGender].MouthTexture = this._faceGenerationParams.CurrentMouthTexture;
			this.genderBasedSelectedValues[this.SelectedGender].EyebrowTexture = this._faceGenerationParams.CurrentEyebrow;
		}

		// Token: 0x06000A00 RID: 2560 RVA: 0x000245E4 File Offset: 0x000227E4
		private int GetVoiceUIIndex()
		{
			int num = 0;
			for (int i = 0; i < this._faceGenerationParams.CurrentVoice; i++)
			{
				if (!this._isVoiceTypeUsableForOnlyNpc[i])
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06000A01 RID: 2561 RVA: 0x0002461C File Offset: 0x0002281C
		private int GetVoiceRealIndex(int UIValue)
		{
			int num = 0;
			for (int i = 0; i < this._newSoundPresetSize; i++)
			{
				if (!this._isVoiceTypeUsableForOnlyNpc[i])
				{
					if (num == UIValue)
					{
						return i;
					}
					num++;
				}
			}
			Debug.FailedAssert("Cannot calculate voice index", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.ViewModelCollection\\FaceGenerator\\FaceGenVM.cs", "GetVoiceRealIndex", 1081);
			return -1;
		}

		// Token: 0x06000A02 RID: 2562 RVA: 0x0002466E File Offset: 0x0002286E
		public void ExecuteHearCurrentVoiceSample()
		{
			this._faceGeneratorScreen.MakeVoice();
		}

		// Token: 0x06000A03 RID: 2563 RVA: 0x0002467C File Offset: 0x0002287C
		public void ExecuteReset()
		{
			string text = GameTexts.FindText("str_reset", null).ToString();
			string text2 = new TextObject("{=hiKTvBgF}Are you sure want to reset changes done in this tab? Your changes will be lost.", null).ToString();
			InformationManager.ShowInquiry(new InquiryData(text, text2, true, true, GameTexts.FindText("str_yes", null).ToString(), GameTexts.FindText("str_no", null).ToString(), new Action(this.Reset), null, "", 0f, null, null, null), false, false);
		}

		// Token: 0x06000A04 RID: 2564 RVA: 0x000246F4 File Offset: 0x000228F4
		private void Reset()
		{
			this.TryValidateCurrentTab();
			this.AddCommand();
			this._characterRefreshEnabled = false;
			bool flag = this._initialRace != this.RaceSelector.SelectedIndex;
			switch (this.Tab)
			{
			case 0:
				this.SelectedGender = this._initialGender;
				this.RaceSelector.SelectedIndex = this._initialRace;
				this.SoundPreset.Reset();
				this.SkinColorSelector.SelectedIndex = MathF.Round(this._initialSelectedSkinColor * (float)(this._skinColors.Count - 1));
				break;
			case 1:
				this.FaceTypes.Reset();
				break;
			case 2:
				this.EyebrowTypes.Reset();
				break;
			case 4:
				this.TeethTypes.Reset();
				break;
			case 5:
				this.SetSelectedBeardType(this._initialSelectedBeardType, false);
				this.SetSelectedHairType(this._initialSelectedHairType, false);
				this.HairColorSelector.SelectedIndex = MathF.Round(this._initialSelectedHairColor * (float)(this._hairColors.Count - 1));
				break;
			case 6:
				this.SetSelectedTattooType(this._initialSelectedTaintType, false);
				this.TattooColorSelector.SelectedIndex = MathF.Round(this._initialSelectedTaintColor * (float)(this._tattooColors.Count - 1));
				break;
			}
			if (this.Tab < 0 || this.Tab >= 7)
			{
				Debug.FailedAssert("Calling Reset on invalid tab!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.ViewModelCollection\\FaceGenerator\\FaceGenVM.cs", "Reset", 1141);
			}
			else
			{
				foreach (FaceGenPropertyVM faceGenPropertyVM in this._tabProperties[(FaceGenVM.FaceGenTabs)this.Tab])
				{
					if (faceGenPropertyVM.TabID == this.Tab)
					{
						faceGenPropertyVM.Reset();
					}
				}
			}
			this._characterRefreshEnabled = true;
			if (flag)
			{
				this.Refresh(true);
			}
			this.UpdateFace();
		}

		// Token: 0x06000A05 RID: 2565 RVA: 0x000248E8 File Offset: 0x00022AE8
		private void ResetAll()
		{
			this.AddCommand();
			this._characterRefreshEnabled = false;
			bool flag = this._initialRace != this.RaceSelector.SelectedIndex;
			this.SelectedGender = this._initialGender;
			this.RaceSelector.SelectedIndex = this._initialRace;
			this.SkinColorSelector.SelectedIndex = MathF.Round(this._initialSelectedSkinColor * (float)(this._skinColors.Count - 1));
			this.HairColorSelector.SelectedIndex = MathF.Round(this._initialSelectedHairColor * (float)(this._hairColors.Count - 1));
			this.TattooColorSelector.SelectedIndex = MathF.Round(this._initialSelectedTaintColor * (float)(this._tattooColors.Count - 1));
			this.SetSelectedBeardType(this._initialSelectedBeardType, false);
			this.SetSelectedHairType(this._initialSelectedHairType, false);
			this.SetSelectedTattooType(this._initialSelectedTaintType, false);
			this.FaceTypes.Reset();
			this.SoundPreset.Reset();
			this.TeethTypes.Reset();
			this.EyebrowTypes.Reset();
			foreach (KeyValuePair<FaceGenVM.FaceGenTabs, MBBindingList<FaceGenPropertyVM>> keyValuePair in this._tabProperties)
			{
				foreach (FaceGenPropertyVM faceGenPropertyVM in keyValuePair.Value)
				{
					faceGenPropertyVM.Reset();
				}
			}
			this._characterRefreshEnabled = true;
			if (flag)
			{
				this.Refresh(FaceGen.UpdateDeformKeys);
			}
			this.UpdateFace();
		}

		// Token: 0x06000A06 RID: 2566 RVA: 0x00024A8C File Offset: 0x00022C8C
		public void ExecuteResetAll()
		{
			string text = GameTexts.FindText("str_reset_all", null).ToString();
			string text2 = new TextObject("{=1hnq3Kb1}Are you sure want to reset all properties? Your changes will be lost.", null).ToString();
			InformationManager.ShowInquiry(new InquiryData(text, text2, true, true, GameTexts.FindText("str_yes", null).ToString(), GameTexts.FindText("str_no", null).ToString(), new Action(this.ResetAll), null, "", 0f, null, null, null), false, false);
		}

		// Token: 0x06000A07 RID: 2567 RVA: 0x00024B04 File Offset: 0x00022D04
		public void ExecuteRandomize()
		{
			this.TryValidateCurrentTab();
			this.AddCommand();
			this._characterRefreshEnabled = false;
			this._isRandomizing = true;
			if (this.Tab < 0 || this.Tab >= 7)
			{
				Debug.FailedAssert("Calling ExecuteRandomize on invalid tab!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.ViewModelCollection\\FaceGenerator\\FaceGenVM.cs", "ExecuteRandomize", 1219);
			}
			else
			{
				foreach (FaceGenPropertyVM faceGenPropertyVM in this._tabProperties[(FaceGenVM.FaceGenTabs)this.Tab])
				{
					faceGenPropertyVM.Randomize();
				}
			}
			switch (this.Tab)
			{
			case 0:
				this.SkinColorSelector.SelectedIndex = MBRandom.RandomInt(this._skinColors.Count);
				break;
			case 1:
				this.FaceTypes.Value = (float)MBRandom.RandomInt((int)this.FaceTypes.Max + 1);
				break;
			case 2:
				this.EyebrowTypes.Value = (float)MBRandom.RandomInt((int)this.EyebrowTypes.Max + 1);
				break;
			case 4:
				this.TeethTypes.Value = (float)MBRandom.RandomInt((int)this.TeethTypes.Max + 1);
				break;
			case 5:
				this.SetSelectedBeardType(this.BeardTypes[MBRandom.RandomInt(this.BeardTypes.Count)], false);
				this.SetSelectedHairType(this.HairTypes[MBRandom.RandomInt(this.HairTypes.Count)], false);
				this.HairColorSelector.SelectedIndex = MBRandom.RandomInt(this._hairColors.Count);
				break;
			case 6:
				this.SetSelectedTattooType(this.TaintTypes[MBRandom.RandomInt(this.TaintTypes.Count)], false);
				this.TattooColorSelector.SelectedIndex = MBRandom.RandomInt(this._tattooColors.Count);
				break;
			}
			this._characterRefreshEnabled = true;
			this._isRandomizing = false;
			this.UpdateFace();
		}

		// Token: 0x06000A08 RID: 2568 RVA: 0x00024D0C File Offset: 0x00022F0C
		public void ExecuteRandomizeAll()
		{
			this.AddCommand();
			this._characterRefreshEnabled = false;
			this._isRandomizing = true;
			foreach (KeyValuePair<FaceGenVM.FaceGenTabs, MBBindingList<FaceGenPropertyVM>> keyValuePair in this._tabProperties)
			{
				foreach (FaceGenPropertyVM faceGenPropertyVM in keyValuePair.Value)
				{
					faceGenPropertyVM.Randomize();
				}
			}
			this.FaceTypes.Value = (float)MBRandom.RandomInt((int)this.FaceTypes.Max + 1);
			if (this.BeardTypes.Count > 0)
			{
				this.SetSelectedBeardType(this.BeardTypes[MBRandom.RandomInt(this.BeardTypes.Count)], false);
			}
			if (this.HairTypes.Count > 0)
			{
				this.SetSelectedHairType(this.HairTypes[MBRandom.RandomInt(this.HairTypes.Count)], false);
			}
			this.EyebrowTypes.Value = (float)MBRandom.RandomInt((int)this.EyebrowTypes.Max + 1);
			this.TeethTypes.Value = (float)MBRandom.RandomInt((int)this.TeethTypes.Max + 1);
			if (this.TaintTypes.Count > 0)
			{
				if (MBRandom.RandomFloat < this._faceGenerationParams.TattooZeroProbability)
				{
					this.SetSelectedTattooType(this.TaintTypes[0], false);
				}
				else
				{
					this.SetSelectedTattooType(this.TaintTypes[MBRandom.RandomInt(1, this.TaintTypes.Count)], false);
				}
			}
			this.TattooColorSelector.SelectedIndex = MBRandom.RandomInt(this._tattooColors.Count);
			this.HairColorSelector.SelectedIndex = MBRandom.RandomInt(this._hairColors.Count);
			this.SkinColorSelector.SelectedIndex = MBRandom.RandomInt(this._skinColors.Count);
			this._characterRefreshEnabled = true;
			this.UpdateFace();
			this._isRandomizing = false;
		}

		// Token: 0x06000A09 RID: 2569 RVA: 0x00024F20 File Offset: 0x00023120
		public void ExecuteCancel()
		{
			this._faceGeneratorScreen.Cancel();
		}

		// Token: 0x06000A0A RID: 2570 RVA: 0x00024F2D File Offset: 0x0002312D
		public void ExecuteDone()
		{
			this._faceGeneratorScreen.Done();
		}

		// Token: 0x06000A0B RID: 2571 RVA: 0x00024F3C File Offset: 0x0002313C
		public void ExecuteRedo()
		{
			if (this._redoCommands.Count > 0)
			{
				int num = this._redoCommands.Count - 1;
				BodyProperties bodyProperties = this._redoCommands[num].BodyProperties;
				int gender = this._redoCommands[num].Gender;
				int race = this._redoCommands[num].Race;
				this._redoCommands.RemoveAt(num);
				this._undoCommands.Add(new UndoRedoKey(this._faceGenerationParams.CurrentGender, this._faceGenerationParams.CurrentRace, this._bodyGenerator.CurrentBodyProperties));
				this.IsRedoEnabled = this._redoCommands.Count > 0;
				this.IsUndoEnabled = this._undoCommands.Count > 0;
				this._characterRefreshEnabled = false;
				this.SetBodyProperties(bodyProperties, false, race, gender, false);
				this._characterRefreshEnabled = true;
			}
		}

		// Token: 0x06000A0C RID: 2572 RVA: 0x0002501C File Offset: 0x0002321C
		public void ExecuteUndo()
		{
			if (this._undoCommands.Count > 0)
			{
				int num = this._undoCommands.Count - 1;
				BodyProperties bodyProperties = this._undoCommands[num].BodyProperties;
				int gender = this._undoCommands[num].Gender;
				int race = this._undoCommands[num].Race;
				this._undoCommands.RemoveAt(num);
				this._redoCommands.Add(new UndoRedoKey(this._faceGenerationParams.CurrentGender, this._faceGenerationParams.CurrentRace, this._bodyGenerator.CurrentBodyProperties));
				this.IsRedoEnabled = this._redoCommands.Count > 0;
				this.IsUndoEnabled = this._undoCommands.Count > 0;
				this._characterRefreshEnabled = false;
				this.SetBodyProperties(bodyProperties, false, race, gender, false);
				this._characterRefreshEnabled = true;
			}
		}

		// Token: 0x06000A0D RID: 2573 RVA: 0x000250FC File Offset: 0x000232FC
		public void ExecuteChangeClothing()
		{
			if (this.IsDressed)
			{
				this._faceGeneratorScreen.UndressCharacterEntity();
				this.IsDressed = false;
				return;
			}
			this._faceGeneratorScreen.DressCharacterEntity();
			this.IsDressed = true;
		}

		// Token: 0x06000A0E RID: 2574 RVA: 0x0002512C File Offset: 0x0002332C
		public void AddCommand()
		{
			if (this._characterRefreshEnabled)
			{
				UndoRedoKey undoRedoKey = new UndoRedoKey(this._faceGenerationParams.CurrentGender, this._faceGenerationParams.CurrentRace, this._bodyGenerator.CurrentBodyProperties);
				if (this._undoCommands.Count > 0)
				{
					UndoRedoKey undoRedoKey2 = this._undoCommands[this._undoCommands.Count - 1];
					if (undoRedoKey2.Gender == undoRedoKey.Gender && undoRedoKey2.Race == undoRedoKey.Race && undoRedoKey2.BodyProperties.Equals(undoRedoKey.BodyProperties))
					{
						return;
					}
				}
				if (this._undoCommands.Count + 1 == this._undoCommands.Capacity)
				{
					this._undoCommands.RemoveAt(0);
				}
				this._undoCommands.Add(undoRedoKey);
				this._redoCommands.Clear();
				this.IsRedoEnabled = this._redoCommands.Count > 0;
				this.IsUndoEnabled = this._undoCommands.Count > 0;
			}
		}

		// Token: 0x06000A0F RID: 2575 RVA: 0x00025236 File Offset: 0x00023436
		private void UpdateTitle()
		{
		}

		// Token: 0x06000A10 RID: 2576 RVA: 0x00025238 File Offset: 0x00023438
		private void ExecuteGoToIndex(int index)
		{
			this._goToIndex(index);
		}

		// Token: 0x06000A11 RID: 2577 RVA: 0x00025248 File Offset: 0x00023448
		public void SetBodyProperties(BodyProperties bodyProperties, bool ignoreDebugValues, int race = 0, int gender = -1, bool recordChange = false)
		{
			this._characterRefreshEnabled = false;
			bool flag = false;
			if (gender == -1)
			{
				this._faceGenerationParams.CurrentGender = this._selectedGender;
			}
			else
			{
				this._faceGenerationParams.CurrentGender = gender;
			}
			if (this._isRaceAvailable)
			{
				flag = this._faceGenerationParams.CurrentRace != race;
				this._faceGenerationParams.CurrentRace = race;
			}
			if (this._openedFromMultiplayer)
			{
				bodyProperties = bodyProperties.ClampForMultiplayer();
			}
			float num = (this._isAgeAvailable ? bodyProperties.Age : this._bodyGenerator.CurrentBodyProperties.Age);
			float num2 = (this._isWeightAvailable ? bodyProperties.Weight : this._bodyGenerator.CurrentBodyProperties.Weight);
			float num3 = (this._isWeightAvailable ? bodyProperties.Build : this._bodyGenerator.CurrentBodyProperties.Build);
			bodyProperties = new BodyProperties(new DynamicBodyProperties(num, num2, num3), bodyProperties.StaticProperties);
			this._bodyGenerator.CurrentBodyProperties = bodyProperties;
			MBBodyProperties.GetParamsFromKey(ref this._faceGenerationParams, bodyProperties, this.IsDressed && this._bodyGenerator.Character.Equipment.EarsAreHidden, this.IsDressed && this._bodyGenerator.Character.Equipment.MouthIsHidden);
			if (flag)
			{
				this._characterRefreshEnabled = true;
				this.Refresh(true);
			}
			else
			{
				this.UpdateFacegen();
				this._characterRefreshEnabled = true;
				this.UpdateFace();
			}
			if (recordChange)
			{
				this._characterRefreshEnabled = true;
				this.AddCommand();
			}
		}

		// Token: 0x06000A12 RID: 2578 RVA: 0x000253C8 File Offset: 0x000235C8
		private void ResetSliderPrevValues()
		{
			foreach (MBBindingList<FaceGenPropertyVM> mbbindingList in this._tabProperties.Values)
			{
				foreach (FaceGenPropertyVM faceGenPropertyVM in mbbindingList)
				{
					faceGenPropertyVM.PrevValue = -1.0;
				}
			}
		}

		// Token: 0x06000A13 RID: 2579 RVA: 0x00025454 File Offset: 0x00023654
		public void UpdateFacegen()
		{
			foreach (MBBindingList<FaceGenPropertyVM> mbbindingList in this._tabProperties.Values)
			{
				foreach (FaceGenPropertyVM faceGenPropertyVM in mbbindingList)
				{
					if (faceGenPropertyVM.KeyNo < 0)
					{
						switch (faceGenPropertyVM.KeyNo)
						{
						case -19:
							faceGenPropertyVM.Value = this._faceGenerationParams.VoicePitch;
							break;
						case -18:
							faceGenPropertyVM.Value = this._faceGenerationParams.CurrentBuild;
							break;
						case -17:
							faceGenPropertyVM.Value = this._faceGenerationParams.CurrentWeight;
							break;
						case -16:
							faceGenPropertyVM.Value = (this._openedFromMultiplayer ? MathF.Clamp(this._faceGenerationParams.HeightMultiplier, 0.25f, 0.75f) : MathF.Clamp(this._faceGenerationParams.HeightMultiplier, 0f, 1f));
							break;
						case -12:
							faceGenPropertyVM.Value = this._faceGenerationParams.CurrentEyeColorOffset;
							break;
						case -11:
							faceGenPropertyVM.Value = this._faceGenerationParams.CurrentAge;
							break;
						}
					}
					else
					{
						faceGenPropertyVM.Value = this._faceGenerationParams.KeyWeights[faceGenPropertyVM.KeyNo];
					}
					faceGenPropertyVM.PrevValue = -1.0;
				}
			}
			this.SelectedGender = this._faceGenerationParams.CurrentGender;
			this.SoundPreset.Value = (float)this.GetVoiceUIIndex();
			this.FaceTypes.Value = (float)this._faceGenerationParams.CurrentFaceTexture;
			this.EyebrowTypes.Value = (float)this._faceGenerationParams.CurrentEyebrow;
			this.TeethTypes.Value = (float)this._faceGenerationParams.CurrentMouthTexture;
			this.SetSelectedTattooType(this._faceGenerationParams.CurrentFaceTattoo, false);
			this.SetSelectedBeardType(this._faceGenerationParams.CurrentBeard, false);
			this.SetSelectedHairType(this._faceGenerationParams.CurrentHair, false);
			this.SkinColorSelector.SelectedIndex = MathF.Round(this._faceGenerationParams.CurrentSkinColorOffset * (float)(this._skinColors.Count - 1));
			this.HairColorSelector.SelectedIndex = MathF.Round(this._faceGenerationParams.CurrentHairColorOffset * (float)(this._hairColors.Count - 1));
			this.TattooColorSelector.SelectedIndex = MathF.Round(this._faceGenerationParams.CurrentFaceTattooColorOffset1 * (float)(this._tattooColors.Count - 1));
		}

		// Token: 0x06000A14 RID: 2580 RVA: 0x0002572C File Offset: 0x0002392C
		private void SetSelectedHairType(FacegenListItemVM item, bool addCommand)
		{
			if (this._selectedHairType != null)
			{
				this._selectedHairType.IsSelected = false;
			}
			this._selectedHairType = item;
			this._selectedHairType.IsSelected = true;
			this._faceGenerationParams.CurrentHair = item.Index;
			if (!addCommand)
			{
				return;
			}
			this.AddCommand();
			this.UpdateFace(-6, (float)item.Index, false, true);
		}

		// Token: 0x06000A15 RID: 2581 RVA: 0x0002578C File Offset: 0x0002398C
		private void SetSelectedHairType(int index, bool addCommand)
		{
			foreach (FacegenListItemVM facegenListItemVM in this.HairTypes)
			{
				if (facegenListItemVM.Index == index)
				{
					this.SetSelectedHairType(facegenListItemVM, addCommand);
					break;
				}
			}
		}

		// Token: 0x06000A16 RID: 2582 RVA: 0x000257E8 File Offset: 0x000239E8
		private void SetSelectedTattooType(FacegenListItemVM item, bool addCommand)
		{
			if (this._selectedTaintType != null)
			{
				this._selectedTaintType.IsSelected = false;
			}
			this._selectedTaintType = item;
			this._selectedTaintType.IsSelected = true;
			this._faceGenerationParams.CurrentFaceTattoo = item.Index;
			if (!addCommand)
			{
				return;
			}
			this.AddCommand();
			this.UpdateFace(-10, (float)item.Index, false, true);
		}

		// Token: 0x06000A17 RID: 2583 RVA: 0x00025848 File Offset: 0x00023A48
		private void SetSelectedTattooType(int index, bool addCommand)
		{
			foreach (FacegenListItemVM facegenListItemVM in this.TaintTypes)
			{
				if (facegenListItemVM.Index == index)
				{
					this.SetSelectedTattooType(facegenListItemVM, addCommand);
					break;
				}
			}
		}

		// Token: 0x06000A18 RID: 2584 RVA: 0x000258A4 File Offset: 0x00023AA4
		private void SetSelectedBeardType(FacegenListItemVM item, bool addCommand)
		{
			if (this._selectedBeardType != null)
			{
				this._selectedBeardType.IsSelected = false;
			}
			this._selectedBeardType = item;
			this._selectedBeardType.IsSelected = true;
			this._faceGenerationParams.CurrentBeard = item.Index;
			if (!addCommand)
			{
				return;
			}
			this.AddCommand();
			this.UpdateFace(-7, (float)item.Index, false, true);
		}

		// Token: 0x06000A19 RID: 2585 RVA: 0x00025904 File Offset: 0x00023B04
		private void SetSelectedBeardType(int index, bool addCommand)
		{
			if (this.SelectedGender == 1)
			{
				this.SetSelectedBeardType(this.BeardTypes.FirstOrDefault<FacegenListItemVM>(), addCommand);
				return;
			}
			foreach (FacegenListItemVM facegenListItemVM in this.BeardTypes)
			{
				if (facegenListItemVM.Index == index)
				{
					this.SetSelectedBeardType(facegenListItemVM, addCommand);
					break;
				}
			}
		}

		// Token: 0x06000A1A RID: 2586 RVA: 0x0002597C File Offset: 0x00023B7C
		private void TryValidateCurrentTab()
		{
			if (this.Tab <= -1 || this.Tab >= 7)
			{
				Debug.FailedAssert(string.Format("Invalid tab: {0}", this.Tab), "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.ViewModelCollection\\FaceGenerator\\FaceGenVM.cs", "TryValidateCurrentTab", 1680);
				Debug.Print(string.Format("Invalid tab: {0}", this.Tab), 0, Debug.DebugColor.White, 17592186044416UL);
				this.Tab = this._tabAvailabilities.IndexOf(true);
				if (this.Tab <= -1 || this.Tab >= 7)
				{
					Debug.FailedAssert("No valid tabs are available!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.ViewModelCollection\\FaceGenerator\\FaceGenVM.cs", "TryValidateCurrentTab", 1687);
					Debug.Print("No valid tabs are available!", 0, Debug.DebugColor.White, 17592186044416UL);
					return;
				}
			}
		}

		// Token: 0x06000A1B RID: 2587 RVA: 0x00025A44 File Offset: 0x00023C44
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
			InputKeyItemVM previousTabInputKey = this.PreviousTabInputKey;
			if (previousTabInputKey != null)
			{
				previousTabInputKey.OnFinalize();
			}
			InputKeyItemVM nextTabInputKey = this.NextTabInputKey;
			if (nextTabInputKey != null)
			{
				nextTabInputKey.OnFinalize();
			}
			for (int i = 0; i < this.CameraControlKeys.Count; i++)
			{
				this.CameraControlKeys[i].OnFinalize();
			}
		}

		// Token: 0x06000A1C RID: 2588 RVA: 0x00025AC2 File Offset: 0x00023CC2
		public void SetCancelInputKey(HotKey hotKey)
		{
			this.CancelInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x06000A1D RID: 2589 RVA: 0x00025AD1 File Offset: 0x00023CD1
		public void SetDoneInputKey(HotKey hotKey)
		{
			this.DoneInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x06000A1E RID: 2590 RVA: 0x00025AE0 File Offset: 0x00023CE0
		public void SetPreviousTabInputKey(HotKey hotKey)
		{
			this.PreviousTabInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x06000A1F RID: 2591 RVA: 0x00025AEF File Offset: 0x00023CEF
		public void SetNextTabInputKey(HotKey hotKey)
		{
			this.NextTabInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x06000A20 RID: 2592 RVA: 0x00025B00 File Offset: 0x00023D00
		public void AddCameraControlInputKey(HotKey hotKey)
		{
			InputKeyItemVM inputKeyItemVM = InputKeyItemVM.CreateFromHotKey(hotKey, true);
			this.CameraControlKeys.Add(inputKeyItemVM);
		}

		// Token: 0x06000A21 RID: 2593 RVA: 0x00025B24 File Offset: 0x00023D24
		public void AddCameraControlInputKey(GameKey gameKey)
		{
			InputKeyItemVM inputKeyItemVM = InputKeyItemVM.CreateFromGameKey(gameKey, true);
			this.CameraControlKeys.Add(inputKeyItemVM);
		}

		// Token: 0x06000A22 RID: 2594 RVA: 0x00025B48 File Offset: 0x00023D48
		public void AddCameraControlInputKey(GameAxisKey gameAxisKey)
		{
			TextObject textObject = Module.CurrentModule.GlobalTextManager.FindText("str_key_name", typeof(FaceGenHotkeyCategory).Name + "_" + gameAxisKey.Id);
			InputKeyItemVM inputKeyItemVM = InputKeyItemVM.CreateFromForcedID(gameAxisKey.AxisKey.ToString(), textObject, true);
			this.CameraControlKeys.Add(inputKeyItemVM);
		}

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x06000A23 RID: 2595 RVA: 0x00025BA8 File Offset: 0x00023DA8
		// (set) Token: 0x06000A24 RID: 2596 RVA: 0x00025BB0 File Offset: 0x00023DB0
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

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x06000A25 RID: 2597 RVA: 0x00025BCE File Offset: 0x00023DCE
		// (set) Token: 0x06000A26 RID: 2598 RVA: 0x00025BD6 File Offset: 0x00023DD6
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

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x06000A27 RID: 2599 RVA: 0x00025BF4 File Offset: 0x00023DF4
		// (set) Token: 0x06000A28 RID: 2600 RVA: 0x00025BFC File Offset: 0x00023DFC
		[DataSourceProperty]
		public InputKeyItemVM PreviousTabInputKey
		{
			get
			{
				return this._previousTabInputKey;
			}
			set
			{
				if (value != this._previousTabInputKey)
				{
					this._previousTabInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "PreviousTabInputKey");
				}
			}
		}

		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x06000A29 RID: 2601 RVA: 0x00025C1A File Offset: 0x00023E1A
		// (set) Token: 0x06000A2A RID: 2602 RVA: 0x00025C22 File Offset: 0x00023E22
		[DataSourceProperty]
		public InputKeyItemVM NextTabInputKey
		{
			get
			{
				return this._nextTabInputKey;
			}
			set
			{
				if (value != this._nextTabInputKey)
				{
					this._nextTabInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "NextTabInputKey");
				}
			}
		}

		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x06000A2B RID: 2603 RVA: 0x00025C40 File Offset: 0x00023E40
		// (set) Token: 0x06000A2C RID: 2604 RVA: 0x00025C48 File Offset: 0x00023E48
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

		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x06000A2D RID: 2605 RVA: 0x00025C66 File Offset: 0x00023E66
		[DataSourceProperty]
		public bool AreAllTabsEnabled
		{
			get
			{
				return this.IsBodyEnabled && this.IsFaceEnabled && this.IsEyesEnabled && this.IsNoseEnabled && this.IsMouthEnabled && this.IsHairEnabled && this.IsTaintEnabled;
			}
		}

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x06000A2E RID: 2606 RVA: 0x00025CA0 File Offset: 0x00023EA0
		// (set) Token: 0x06000A2F RID: 2607 RVA: 0x00025CA8 File Offset: 0x00023EA8
		[DataSourceProperty]
		public bool IsBodyEnabled
		{
			get
			{
				return this._isBodyEnabled;
			}
			set
			{
				if (value != this._isBodyEnabled)
				{
					this._isBodyEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsBodyEnabled");
				}
			}
		}

		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x06000A30 RID: 2608 RVA: 0x00025CC6 File Offset: 0x00023EC6
		// (set) Token: 0x06000A31 RID: 2609 RVA: 0x00025CCE File Offset: 0x00023ECE
		[DataSourceProperty]
		public bool IsFaceEnabled
		{
			get
			{
				return this._isFaceEnabled;
			}
			set
			{
				if (value != this._isFaceEnabled)
				{
					this._isFaceEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsFaceEnabled");
				}
			}
		}

		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x06000A32 RID: 2610 RVA: 0x00025CEC File Offset: 0x00023EEC
		// (set) Token: 0x06000A33 RID: 2611 RVA: 0x00025CF4 File Offset: 0x00023EF4
		[DataSourceProperty]
		public bool IsEyesEnabled
		{
			get
			{
				return this._isEyesEnabled;
			}
			set
			{
				if (value != this._isEyesEnabled)
				{
					this._isEyesEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsEyesEnabled");
				}
			}
		}

		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x06000A34 RID: 2612 RVA: 0x00025D12 File Offset: 0x00023F12
		// (set) Token: 0x06000A35 RID: 2613 RVA: 0x00025D1A File Offset: 0x00023F1A
		[DataSourceProperty]
		public bool IsNoseEnabled
		{
			get
			{
				return this._isNoseEnabled;
			}
			set
			{
				if (value != this._isNoseEnabled)
				{
					this._isNoseEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsNoseEnabled");
				}
			}
		}

		// Token: 0x170002FA RID: 762
		// (get) Token: 0x06000A36 RID: 2614 RVA: 0x00025D38 File Offset: 0x00023F38
		// (set) Token: 0x06000A37 RID: 2615 RVA: 0x00025D40 File Offset: 0x00023F40
		[DataSourceProperty]
		public bool IsMouthEnabled
		{
			get
			{
				return this._isMouthEnabled;
			}
			set
			{
				if (value != this._isMouthEnabled)
				{
					this._isMouthEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsMouthEnabled");
				}
			}
		}

		// Token: 0x170002FB RID: 763
		// (get) Token: 0x06000A38 RID: 2616 RVA: 0x00025D5E File Offset: 0x00023F5E
		// (set) Token: 0x06000A39 RID: 2617 RVA: 0x00025D66 File Offset: 0x00023F66
		[DataSourceProperty]
		public bool IsHairEnabled
		{
			get
			{
				return this._isHairEnabled;
			}
			set
			{
				if (value != this._isHairEnabled)
				{
					this._isHairEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsHairEnabled");
				}
			}
		}

		// Token: 0x170002FC RID: 764
		// (get) Token: 0x06000A3A RID: 2618 RVA: 0x00025D84 File Offset: 0x00023F84
		// (set) Token: 0x06000A3B RID: 2619 RVA: 0x00025D8C File Offset: 0x00023F8C
		[DataSourceProperty]
		public bool IsTaintEnabled
		{
			get
			{
				return this._isTaintEnabled;
			}
			set
			{
				if (value != this._isTaintEnabled)
				{
					this._isTaintEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsTaintEnabled");
				}
			}
		}

		// Token: 0x170002FD RID: 765
		// (get) Token: 0x06000A3C RID: 2620 RVA: 0x00025DAA File Offset: 0x00023FAA
		// (set) Token: 0x06000A3D RID: 2621 RVA: 0x00025DB2 File Offset: 0x00023FB2
		[DataSourceProperty]
		public string FlipHairLbl
		{
			get
			{
				return this._flipHairLbl;
			}
			set
			{
				if (value != this._flipHairLbl)
				{
					this._flipHairLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "FlipHairLbl");
				}
			}
		}

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x06000A3E RID: 2622 RVA: 0x00025DD5 File Offset: 0x00023FD5
		// (set) Token: 0x06000A3F RID: 2623 RVA: 0x00025DDD File Offset: 0x00023FDD
		[DataSourceProperty]
		public string SkinColorLbl
		{
			get
			{
				return this._skinColorLbl;
			}
			set
			{
				if (value != this._skinColorLbl)
				{
					this._skinColorLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "SkinColorLbl");
				}
			}
		}

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x06000A40 RID: 2624 RVA: 0x00025E00 File Offset: 0x00024000
		// (set) Token: 0x06000A41 RID: 2625 RVA: 0x00025E08 File Offset: 0x00024008
		[DataSourceProperty]
		public string RaceLbl
		{
			get
			{
				return this._raceLbl;
			}
			set
			{
				if (value != this._raceLbl)
				{
					this._raceLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "RaceLbl");
				}
			}
		}

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x06000A42 RID: 2626 RVA: 0x00025E2B File Offset: 0x0002402B
		// (set) Token: 0x06000A43 RID: 2627 RVA: 0x00025E33 File Offset: 0x00024033
		[DataSourceProperty]
		public string GenderLbl
		{
			get
			{
				return this._genderLbl;
			}
			set
			{
				if (value != this._genderLbl)
				{
					this._genderLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "GenderLbl");
				}
			}
		}

		// Token: 0x17000301 RID: 769
		// (get) Token: 0x06000A44 RID: 2628 RVA: 0x00025E56 File Offset: 0x00024056
		// (set) Token: 0x06000A45 RID: 2629 RVA: 0x00025E5E File Offset: 0x0002405E
		[DataSourceProperty]
		public string CancelBtnLbl
		{
			get
			{
				return this._cancelBtnLbl;
			}
			set
			{
				if (value != this._cancelBtnLbl)
				{
					this._cancelBtnLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "CancelBtnLbl");
				}
			}
		}

		// Token: 0x17000302 RID: 770
		// (get) Token: 0x06000A46 RID: 2630 RVA: 0x00025E81 File Offset: 0x00024081
		// (set) Token: 0x06000A47 RID: 2631 RVA: 0x00025E89 File Offset: 0x00024089
		[DataSourceProperty]
		public string DoneBtnLbl
		{
			get
			{
				return this._doneBtnLbl;
			}
			set
			{
				if (value != this._doneBtnLbl)
				{
					this._doneBtnLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "DoneBtnLbl");
				}
			}
		}

		// Token: 0x17000303 RID: 771
		// (get) Token: 0x06000A48 RID: 2632 RVA: 0x00025EAC File Offset: 0x000240AC
		// (set) Token: 0x06000A49 RID: 2633 RVA: 0x00025EB4 File Offset: 0x000240B4
		[DataSourceProperty]
		public HintViewModel BodyHint
		{
			get
			{
				return this._bodyHint;
			}
			set
			{
				if (value != this._bodyHint)
				{
					this._bodyHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "BodyHint");
				}
			}
		}

		// Token: 0x17000304 RID: 772
		// (get) Token: 0x06000A4A RID: 2634 RVA: 0x00025ED2 File Offset: 0x000240D2
		// (set) Token: 0x06000A4B RID: 2635 RVA: 0x00025EDA File Offset: 0x000240DA
		[DataSourceProperty]
		public HintViewModel FaceHint
		{
			get
			{
				return this._faceHint;
			}
			set
			{
				if (value != this._faceHint)
				{
					this._faceHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "FaceHint");
				}
			}
		}

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x06000A4C RID: 2636 RVA: 0x00025EF8 File Offset: 0x000240F8
		// (set) Token: 0x06000A4D RID: 2637 RVA: 0x00025F00 File Offset: 0x00024100
		[DataSourceProperty]
		public HintViewModel EyesHint
		{
			get
			{
				return this._eyesHint;
			}
			set
			{
				if (value != this._eyesHint)
				{
					this._eyesHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "EyesHint");
				}
			}
		}

		// Token: 0x17000306 RID: 774
		// (get) Token: 0x06000A4E RID: 2638 RVA: 0x00025F1E File Offset: 0x0002411E
		// (set) Token: 0x06000A4F RID: 2639 RVA: 0x00025F26 File Offset: 0x00024126
		[DataSourceProperty]
		public HintViewModel NoseHint
		{
			get
			{
				return this._noseHint;
			}
			set
			{
				if (value != this._noseHint)
				{
					this._noseHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "NoseHint");
				}
			}
		}

		// Token: 0x17000307 RID: 775
		// (get) Token: 0x06000A50 RID: 2640 RVA: 0x00025F44 File Offset: 0x00024144
		// (set) Token: 0x06000A51 RID: 2641 RVA: 0x00025F4C File Offset: 0x0002414C
		[DataSourceProperty]
		public HintViewModel HairHint
		{
			get
			{
				return this._hairHint;
			}
			set
			{
				if (value != this._hairHint)
				{
					this._hairHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "HairHint");
				}
			}
		}

		// Token: 0x17000308 RID: 776
		// (get) Token: 0x06000A52 RID: 2642 RVA: 0x00025F6A File Offset: 0x0002416A
		// (set) Token: 0x06000A53 RID: 2643 RVA: 0x00025F72 File Offset: 0x00024172
		[DataSourceProperty]
		public HintViewModel TaintHint
		{
			get
			{
				return this._taintHint;
			}
			set
			{
				if (value != this._taintHint)
				{
					this._taintHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "TaintHint");
				}
			}
		}

		// Token: 0x17000309 RID: 777
		// (get) Token: 0x06000A54 RID: 2644 RVA: 0x00025F90 File Offset: 0x00024190
		// (set) Token: 0x06000A55 RID: 2645 RVA: 0x00025F98 File Offset: 0x00024198
		[DataSourceProperty]
		public HintViewModel MouthHint
		{
			get
			{
				return this._mouthHint;
			}
			set
			{
				if (value != this._mouthHint)
				{
					this._mouthHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "MouthHint");
				}
			}
		}

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x06000A56 RID: 2646 RVA: 0x00025FB6 File Offset: 0x000241B6
		// (set) Token: 0x06000A57 RID: 2647 RVA: 0x00025FBE File Offset: 0x000241BE
		[DataSourceProperty]
		public HintViewModel RedoHint
		{
			get
			{
				return this._redoHint;
			}
			set
			{
				if (value != this._redoHint)
				{
					this._redoHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "RedoHint");
				}
			}
		}

		// Token: 0x1700030B RID: 779
		// (get) Token: 0x06000A58 RID: 2648 RVA: 0x00025FDC File Offset: 0x000241DC
		// (set) Token: 0x06000A59 RID: 2649 RVA: 0x00025FE4 File Offset: 0x000241E4
		[DataSourceProperty]
		public HintViewModel UndoHint
		{
			get
			{
				return this._undoHint;
			}
			set
			{
				if (value != this._undoHint)
				{
					this._undoHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "UndoHint");
				}
			}
		}

		// Token: 0x1700030C RID: 780
		// (get) Token: 0x06000A5A RID: 2650 RVA: 0x00026002 File Offset: 0x00024202
		// (set) Token: 0x06000A5B RID: 2651 RVA: 0x0002600A File Offset: 0x0002420A
		[DataSourceProperty]
		public HintViewModel RandomizeHint
		{
			get
			{
				return this._randomizeHint;
			}
			set
			{
				if (value != this._randomizeHint)
				{
					this._randomizeHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "RandomizeHint");
				}
			}
		}

		// Token: 0x1700030D RID: 781
		// (get) Token: 0x06000A5C RID: 2652 RVA: 0x00026028 File Offset: 0x00024228
		// (set) Token: 0x06000A5D RID: 2653 RVA: 0x00026030 File Offset: 0x00024230
		[DataSourceProperty]
		public HintViewModel RandomizeAllHint
		{
			get
			{
				return this._randomizeAllHint;
			}
			set
			{
				if (value != this._randomizeAllHint)
				{
					this._randomizeAllHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "RandomizeAllHint");
				}
			}
		}

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x06000A5E RID: 2654 RVA: 0x0002604E File Offset: 0x0002424E
		// (set) Token: 0x06000A5F RID: 2655 RVA: 0x00026056 File Offset: 0x00024256
		[DataSourceProperty]
		public HintViewModel ResetHint
		{
			get
			{
				return this._resetHint;
			}
			set
			{
				if (value != this._resetHint)
				{
					this._resetHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "ResetHint");
				}
			}
		}

		// Token: 0x1700030F RID: 783
		// (get) Token: 0x06000A60 RID: 2656 RVA: 0x00026074 File Offset: 0x00024274
		// (set) Token: 0x06000A61 RID: 2657 RVA: 0x0002607C File Offset: 0x0002427C
		[DataSourceProperty]
		public HintViewModel ResetAllHint
		{
			get
			{
				return this._resetAllHint;
			}
			set
			{
				if (value != this._resetAllHint)
				{
					this._resetAllHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "ResetAllHint");
				}
			}
		}

		// Token: 0x17000310 RID: 784
		// (get) Token: 0x06000A62 RID: 2658 RVA: 0x0002609A File Offset: 0x0002429A
		// (set) Token: 0x06000A63 RID: 2659 RVA: 0x000260A2 File Offset: 0x000242A2
		[DataSourceProperty]
		public HintViewModel ClothHint
		{
			get
			{
				return this._clothHint;
			}
			set
			{
				if (value != this._clothHint)
				{
					this._clothHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "ClothHint");
				}
			}
		}

		// Token: 0x17000311 RID: 785
		// (get) Token: 0x06000A64 RID: 2660 RVA: 0x000260C0 File Offset: 0x000242C0
		// (set) Token: 0x06000A65 RID: 2661 RVA: 0x000260C8 File Offset: 0x000242C8
		[DataSourceProperty]
		public int HairNum
		{
			get
			{
				return this.hairNum;
			}
			set
			{
				if (value != this.hairNum)
				{
					this.hairNum = value;
					base.OnPropertyChangedWithValue(value, "HairNum");
				}
			}
		}

		// Token: 0x17000312 RID: 786
		// (get) Token: 0x06000A66 RID: 2662 RVA: 0x000260E6 File Offset: 0x000242E6
		// (set) Token: 0x06000A67 RID: 2663 RVA: 0x000260EE File Offset: 0x000242EE
		[DataSourceProperty]
		public SelectorVM<SelectorItemVM> SkinColorSelector
		{
			get
			{
				return this._skinColorSelector;
			}
			set
			{
				if (value != this._skinColorSelector)
				{
					this._skinColorSelector = value;
					base.OnPropertyChangedWithValue<SelectorVM<SelectorItemVM>>(value, "SkinColorSelector");
				}
			}
		}

		// Token: 0x17000313 RID: 787
		// (get) Token: 0x06000A68 RID: 2664 RVA: 0x0002610C File Offset: 0x0002430C
		// (set) Token: 0x06000A69 RID: 2665 RVA: 0x00026114 File Offset: 0x00024314
		[DataSourceProperty]
		public SelectorVM<SelectorItemVM> HairColorSelector
		{
			get
			{
				return this._hairColorSelector;
			}
			set
			{
				if (value != this._hairColorSelector)
				{
					this._hairColorSelector = value;
					base.OnPropertyChangedWithValue<SelectorVM<SelectorItemVM>>(value, "HairColorSelector");
				}
			}
		}

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x06000A6A RID: 2666 RVA: 0x00026132 File Offset: 0x00024332
		// (set) Token: 0x06000A6B RID: 2667 RVA: 0x0002613A File Offset: 0x0002433A
		[DataSourceProperty]
		public SelectorVM<SelectorItemVM> TattooColorSelector
		{
			get
			{
				return this._tattooColorSelector;
			}
			set
			{
				if (value != this._tattooColorSelector)
				{
					this._tattooColorSelector = value;
					base.OnPropertyChangedWithValue<SelectorVM<SelectorItemVM>>(value, "TattooColorSelector");
				}
			}
		}

		// Token: 0x17000315 RID: 789
		// (get) Token: 0x06000A6C RID: 2668 RVA: 0x00026158 File Offset: 0x00024358
		// (set) Token: 0x06000A6D RID: 2669 RVA: 0x00026160 File Offset: 0x00024360
		[DataSourceProperty]
		public SelectorVM<SelectorItemVM> RaceSelector
		{
			get
			{
				return this._raceSelector;
			}
			set
			{
				if (value != this._raceSelector)
				{
					this._raceSelector = value;
					base.OnPropertyChangedWithValue<SelectorVM<SelectorItemVM>>(value, "RaceSelector");
				}
			}
		}

		// Token: 0x17000316 RID: 790
		// (get) Token: 0x06000A6E RID: 2670 RVA: 0x0002617E File Offset: 0x0002437E
		// (set) Token: 0x06000A6F RID: 2671 RVA: 0x00026188 File Offset: 0x00024388
		[DataSourceProperty]
		public int Tab
		{
			get
			{
				return this._tab;
			}
			set
			{
				if (this._tab != value)
				{
					this._tab = value;
					base.OnPropertyChangedWithValue(value, "Tab");
					this.TryValidateCurrentTab();
				}
				switch (value)
				{
				case 0:
					this._faceGeneratorScreen.ChangeToBodyCamera();
					break;
				case 1:
				case 6:
					this._faceGeneratorScreen.ChangeToFaceCamera();
					break;
				case 2:
					this._faceGeneratorScreen.ChangeToEyeCamera();
					break;
				case 3:
					this._faceGeneratorScreen.ChangeToNoseCamera();
					break;
				case 4:
					this._faceGeneratorScreen.ChangeToMouthCamera();
					break;
				case 5:
					this._faceGeneratorScreen.ChangeToHairCamera();
					break;
				}
				this.UpdateTitle();
			}
		}

		// Token: 0x17000317 RID: 791
		// (get) Token: 0x06000A70 RID: 2672 RVA: 0x0002622D File Offset: 0x0002442D
		// (set) Token: 0x06000A71 RID: 2673 RVA: 0x00026238 File Offset: 0x00024438
		[DataSourceProperty]
		public int SelectedGender
		{
			get
			{
				return this._selectedGender;
			}
			set
			{
				if (this._initialGender == -1 && !this.TryGetInitialValue("SelectedGender", ref this._initialGender))
				{
					this.SetOrAddInitialValue("SelectedGender", (float)value);
				}
				if (this._selectedGender != value)
				{
					this.AddCommand();
					this._selectedGender = value;
					this.UpdateRaceAndGenderBasedResources();
					this.Refresh(FaceGen.UpdateDeformKeys);
					base.OnPropertyChangedWithValue(value, "SelectedGender");
					base.OnPropertyChanged("IsFemale");
				}
			}
		}

		// Token: 0x17000318 RID: 792
		// (get) Token: 0x06000A72 RID: 2674 RVA: 0x000262AC File Offset: 0x000244AC
		[DataSourceProperty]
		public bool IsFemale
		{
			get
			{
				return this.SelectedGender != 0;
			}
		}

		// Token: 0x17000319 RID: 793
		// (get) Token: 0x06000A73 RID: 2675 RVA: 0x000262B7 File Offset: 0x000244B7
		// (set) Token: 0x06000A74 RID: 2676 RVA: 0x000262BF File Offset: 0x000244BF
		[DataSourceProperty]
		public MBBindingList<FaceGenPropertyVM> BodyProperties
		{
			get
			{
				return this._bodyProperties;
			}
			set
			{
				if (value != this._bodyProperties)
				{
					this._bodyProperties = value;
					base.OnPropertyChangedWithValue<MBBindingList<FaceGenPropertyVM>>(value, "BodyProperties");
				}
			}
		}

		// Token: 0x1700031A RID: 794
		// (get) Token: 0x06000A75 RID: 2677 RVA: 0x000262DD File Offset: 0x000244DD
		// (set) Token: 0x06000A76 RID: 2678 RVA: 0x000262E5 File Offset: 0x000244E5
		[DataSourceProperty]
		public bool CanChangeGender
		{
			get
			{
				return this._canChangeGender;
			}
			set
			{
				if (value != this._canChangeGender)
				{
					this._canChangeGender = value;
					base.OnPropertyChangedWithValue(value, "CanChangeGender");
				}
			}
		}

		// Token: 0x1700031B RID: 795
		// (get) Token: 0x06000A77 RID: 2679 RVA: 0x00026303 File Offset: 0x00024503
		// (set) Token: 0x06000A78 RID: 2680 RVA: 0x0002630B File Offset: 0x0002450B
		[DataSourceProperty]
		public bool CanChangeRace
		{
			get
			{
				return this._canChangeRace;
			}
			set
			{
				if (value != this._canChangeRace)
				{
					this._canChangeRace = value;
					base.OnPropertyChangedWithValue(value, "CanChangeRace");
				}
			}
		}

		// Token: 0x1700031C RID: 796
		// (get) Token: 0x06000A79 RID: 2681 RVA: 0x00026329 File Offset: 0x00024529
		// (set) Token: 0x06000A7A RID: 2682 RVA: 0x00026331 File Offset: 0x00024531
		[DataSourceProperty]
		public bool IsUndoEnabled
		{
			get
			{
				return this._isUndoEnabled;
			}
			set
			{
				if (value != this._isUndoEnabled)
				{
					this._isUndoEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsUndoEnabled");
				}
			}
		}

		// Token: 0x1700031D RID: 797
		// (get) Token: 0x06000A7B RID: 2683 RVA: 0x0002634F File Offset: 0x0002454F
		// (set) Token: 0x06000A7C RID: 2684 RVA: 0x00026357 File Offset: 0x00024557
		[DataSourceProperty]
		public bool IsRedoEnabled
		{
			get
			{
				return this._isRedoEnabled;
			}
			set
			{
				if (value != this._isRedoEnabled)
				{
					this._isRedoEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsRedoEnabled");
				}
			}
		}

		// Token: 0x1700031E RID: 798
		// (get) Token: 0x06000A7D RID: 2685 RVA: 0x00026375 File Offset: 0x00024575
		// (set) Token: 0x06000A7E RID: 2686 RVA: 0x0002637D File Offset: 0x0002457D
		[DataSourceProperty]
		public MBBindingList<FaceGenPropertyVM> FaceProperties
		{
			get
			{
				return this._faceProperties;
			}
			set
			{
				if (value != this._faceProperties)
				{
					this._faceProperties = value;
					base.OnPropertyChangedWithValue<MBBindingList<FaceGenPropertyVM>>(value, "FaceProperties");
				}
			}
		}

		// Token: 0x1700031F RID: 799
		// (get) Token: 0x06000A7F RID: 2687 RVA: 0x0002639B File Offset: 0x0002459B
		// (set) Token: 0x06000A80 RID: 2688 RVA: 0x000263A3 File Offset: 0x000245A3
		[DataSourceProperty]
		public MBBindingList<FaceGenPropertyVM> EyesProperties
		{
			get
			{
				return this._eyesProperties;
			}
			set
			{
				if (value != this._eyesProperties)
				{
					this._eyesProperties = value;
					base.OnPropertyChangedWithValue<MBBindingList<FaceGenPropertyVM>>(value, "EyesProperties");
				}
			}
		}

		// Token: 0x17000320 RID: 800
		// (get) Token: 0x06000A81 RID: 2689 RVA: 0x000263C1 File Offset: 0x000245C1
		// (set) Token: 0x06000A82 RID: 2690 RVA: 0x000263C9 File Offset: 0x000245C9
		[DataSourceProperty]
		public MBBindingList<FaceGenPropertyVM> NoseProperties
		{
			get
			{
				return this._noseProperties;
			}
			set
			{
				if (value != this._noseProperties)
				{
					this._noseProperties = value;
					base.OnPropertyChangedWithValue<MBBindingList<FaceGenPropertyVM>>(value, "NoseProperties");
				}
			}
		}

		// Token: 0x17000321 RID: 801
		// (get) Token: 0x06000A83 RID: 2691 RVA: 0x000263E7 File Offset: 0x000245E7
		// (set) Token: 0x06000A84 RID: 2692 RVA: 0x000263EF File Offset: 0x000245EF
		[DataSourceProperty]
		public MBBindingList<FaceGenPropertyVM> MouthProperties
		{
			get
			{
				return this._mouthProperties;
			}
			set
			{
				if (value != this._mouthProperties)
				{
					this._mouthProperties = value;
					base.OnPropertyChangedWithValue<MBBindingList<FaceGenPropertyVM>>(value, "MouthProperties");
				}
			}
		}

		// Token: 0x17000322 RID: 802
		// (get) Token: 0x06000A85 RID: 2693 RVA: 0x0002640D File Offset: 0x0002460D
		// (set) Token: 0x06000A86 RID: 2694 RVA: 0x00026415 File Offset: 0x00024615
		[DataSourceProperty]
		public MBBindingList<FaceGenPropertyVM> HairProperties
		{
			get
			{
				return this._hairProperties;
			}
			set
			{
				if (value != this._hairProperties)
				{
					this._hairProperties = value;
					base.OnPropertyChangedWithValue<MBBindingList<FaceGenPropertyVM>>(value, "HairProperties");
				}
			}
		}

		// Token: 0x17000323 RID: 803
		// (get) Token: 0x06000A87 RID: 2695 RVA: 0x00026433 File Offset: 0x00024633
		// (set) Token: 0x06000A88 RID: 2696 RVA: 0x0002643B File Offset: 0x0002463B
		[DataSourceProperty]
		public MBBindingList<FaceGenPropertyVM> TaintProperties
		{
			get
			{
				return this._taintProperties;
			}
			set
			{
				if (value != this._taintProperties)
				{
					this._taintProperties = value;
					base.OnPropertyChangedWithValue<MBBindingList<FaceGenPropertyVM>>(value, "TaintProperties");
				}
			}
		}

		// Token: 0x17000324 RID: 804
		// (get) Token: 0x06000A89 RID: 2697 RVA: 0x00026459 File Offset: 0x00024659
		// (set) Token: 0x06000A8A RID: 2698 RVA: 0x00026461 File Offset: 0x00024661
		[DataSourceProperty]
		public MBBindingList<FacegenListItemVM> TaintTypes
		{
			get
			{
				return this._taintTypes;
			}
			set
			{
				if (value != this._taintTypes)
				{
					this._taintTypes = value;
					base.OnPropertyChangedWithValue<MBBindingList<FacegenListItemVM>>(value, "TaintTypes");
				}
			}
		}

		// Token: 0x17000325 RID: 805
		// (get) Token: 0x06000A8B RID: 2699 RVA: 0x0002647F File Offset: 0x0002467F
		// (set) Token: 0x06000A8C RID: 2700 RVA: 0x00026487 File Offset: 0x00024687
		[DataSourceProperty]
		public MBBindingList<FacegenListItemVM> BeardTypes
		{
			get
			{
				return this._beardTypes;
			}
			set
			{
				if (value != this._beardTypes)
				{
					this._beardTypes = value;
					base.OnPropertyChangedWithValue<MBBindingList<FacegenListItemVM>>(value, "BeardTypes");
				}
			}
		}

		// Token: 0x17000326 RID: 806
		// (get) Token: 0x06000A8D RID: 2701 RVA: 0x000264A5 File Offset: 0x000246A5
		// (set) Token: 0x06000A8E RID: 2702 RVA: 0x000264AD File Offset: 0x000246AD
		[DataSourceProperty]
		public MBBindingList<FacegenListItemVM> HairTypes
		{
			get
			{
				return this._hairTypes;
			}
			set
			{
				if (value != this._hairTypes)
				{
					this._hairTypes = value;
					base.OnPropertyChangedWithValue<MBBindingList<FacegenListItemVM>>(value, "HairTypes");
				}
			}
		}

		// Token: 0x17000327 RID: 807
		// (get) Token: 0x06000A8F RID: 2703 RVA: 0x000264CB File Offset: 0x000246CB
		// (set) Token: 0x06000A90 RID: 2704 RVA: 0x000264D3 File Offset: 0x000246D3
		[DataSourceProperty]
		public FaceGenPropertyVM SoundPreset
		{
			get
			{
				return this._soundPreset;
			}
			set
			{
				if (value != this._soundPreset)
				{
					this._soundPreset = value;
					base.OnPropertyChangedWithValue<FaceGenPropertyVM>(value, "SoundPreset");
				}
			}
		}

		// Token: 0x17000328 RID: 808
		// (get) Token: 0x06000A91 RID: 2705 RVA: 0x000264F1 File Offset: 0x000246F1
		// (set) Token: 0x06000A92 RID: 2706 RVA: 0x000264F9 File Offset: 0x000246F9
		[DataSourceProperty]
		public FaceGenPropertyVM EyebrowTypes
		{
			get
			{
				return this._eyebrowTypes;
			}
			set
			{
				if (value != this._eyebrowTypes)
				{
					this._eyebrowTypes = value;
					base.OnPropertyChangedWithValue<FaceGenPropertyVM>(value, "EyebrowTypes");
				}
			}
		}

		// Token: 0x17000329 RID: 809
		// (get) Token: 0x06000A93 RID: 2707 RVA: 0x00026517 File Offset: 0x00024717
		// (set) Token: 0x06000A94 RID: 2708 RVA: 0x0002651F File Offset: 0x0002471F
		[DataSourceProperty]
		public FaceGenPropertyVM TeethTypes
		{
			get
			{
				return this._teethTypes;
			}
			set
			{
				if (value != this._teethTypes)
				{
					this._teethTypes = value;
					base.OnPropertyChangedWithValue<FaceGenPropertyVM>(value, "TeethTypes");
				}
			}
		}

		// Token: 0x1700032A RID: 810
		// (get) Token: 0x06000A95 RID: 2709 RVA: 0x0002653D File Offset: 0x0002473D
		// (set) Token: 0x06000A96 RID: 2710 RVA: 0x0002654A File Offset: 0x0002474A
		[DataSourceProperty]
		public bool FlipHairCb
		{
			get
			{
				return this._faceGenerationParams.IsHairFlipped;
			}
			set
			{
				if (value != this._faceGenerationParams.IsHairFlipped)
				{
					this._faceGenerationParams.IsHairFlipped = value;
					base.OnPropertyChangedWithValue(value, "FlipHairCb");
					this.UpdateFace();
				}
			}
		}

		// Token: 0x1700032B RID: 811
		// (get) Token: 0x06000A97 RID: 2711 RVA: 0x00026578 File Offset: 0x00024778
		// (set) Token: 0x06000A98 RID: 2712 RVA: 0x00026580 File Offset: 0x00024780
		[DataSourceProperty]
		public bool IsDressed
		{
			get
			{
				return this._isDressed;
			}
			set
			{
				if (value != this._isDressed)
				{
					this._isDressed = value;
					base.OnPropertyChangedWithValue(value, "IsDressed");
				}
			}
		}

		// Token: 0x1700032C RID: 812
		// (get) Token: 0x06000A99 RID: 2713 RVA: 0x0002659E File Offset: 0x0002479E
		// (set) Token: 0x06000A9A RID: 2714 RVA: 0x000265A6 File Offset: 0x000247A6
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

		// Token: 0x1700032D RID: 813
		// (get) Token: 0x06000A9B RID: 2715 RVA: 0x000265C4 File Offset: 0x000247C4
		// (set) Token: 0x06000A9C RID: 2716 RVA: 0x000265CC File Offset: 0x000247CC
		[DataSourceProperty]
		public FaceGenPropertyVM FaceTypes
		{
			get
			{
				return this._faceTypes;
			}
			set
			{
				if (value != this._faceTypes)
				{
					this._faceTypes = value;
					base.OnPropertyChangedWithValue<FaceGenPropertyVM>(value, "FaceTypes");
				}
			}
		}

		// Token: 0x1700032E RID: 814
		// (get) Token: 0x06000A9D RID: 2717 RVA: 0x000265EA File Offset: 0x000247EA
		// (set) Token: 0x06000A9E RID: 2718 RVA: 0x000265F2 File Offset: 0x000247F2
		[DataSourceProperty]
		public string Title
		{
			get
			{
				return this._title;
			}
			set
			{
				if (value != this._title)
				{
					this._title = value;
					base.OnPropertyChangedWithValue<string>(value, "Title");
				}
			}
		}

		// Token: 0x1700032F RID: 815
		// (get) Token: 0x06000A9F RID: 2719 RVA: 0x00026615 File Offset: 0x00024815
		// (set) Token: 0x06000AA0 RID: 2720 RVA: 0x0002661D File Offset: 0x0002481D
		[DataSourceProperty]
		public int TotalStageCount
		{
			get
			{
				return this._totalStageCount;
			}
			set
			{
				if (value != this._totalStageCount)
				{
					this._totalStageCount = value;
					base.OnPropertyChangedWithValue(value, "TotalStageCount");
				}
			}
		}

		// Token: 0x17000330 RID: 816
		// (get) Token: 0x06000AA1 RID: 2721 RVA: 0x0002663B File Offset: 0x0002483B
		// (set) Token: 0x06000AA2 RID: 2722 RVA: 0x00026643 File Offset: 0x00024843
		[DataSourceProperty]
		public int CurrentStageIndex
		{
			get
			{
				return this._currentStageIndex;
			}
			set
			{
				if (value != this._currentStageIndex)
				{
					this._currentStageIndex = value;
					base.OnPropertyChangedWithValue(value, "CurrentStageIndex");
				}
			}
		}

		// Token: 0x17000331 RID: 817
		// (get) Token: 0x06000AA3 RID: 2723 RVA: 0x00026661 File Offset: 0x00024861
		// (set) Token: 0x06000AA4 RID: 2724 RVA: 0x00026669 File Offset: 0x00024869
		[DataSourceProperty]
		public int FurthestIndex
		{
			get
			{
				return this._furthestIndex;
			}
			set
			{
				if (value != this._furthestIndex)
				{
					this._furthestIndex = value;
					base.OnPropertyChangedWithValue(value, "FurthestIndex");
				}
			}
		}

		// Token: 0x04000467 RID: 1127
		private const float MultiplayerHeightSliderMinValue = 0.25f;

		// Token: 0x04000468 RID: 1128
		private const float MultiplayerHeightSliderMaxValue = 0.75f;

		// Token: 0x04000469 RID: 1129
		private readonly IFaceGeneratorHandler _faceGeneratorScreen;

		// Token: 0x0400046A RID: 1130
		private bool _characterRefreshEnabled = true;

		// Token: 0x0400046B RID: 1131
		private bool _initialValuesSet;

		// Token: 0x0400046C RID: 1132
		private readonly BodyGenerator _bodyGenerator;

		// Token: 0x0400046D RID: 1133
		private readonly TextObject _affirmitiveText;

		// Token: 0x0400046E RID: 1134
		private readonly TextObject _negativeText;

		// Token: 0x0400046F RID: 1135
		private FaceGenerationParams _faceGenerationParams = FaceGenerationParams.Create();

		// Token: 0x04000470 RID: 1136
		private List<UndoRedoKey> _undoCommands;

		// Token: 0x04000471 RID: 1137
		private List<UndoRedoKey> _redoCommands;

		// Token: 0x04000472 RID: 1138
		private Dictionary<string, float> _initialValues;

		// Token: 0x04000473 RID: 1139
		private List<bool> _isVoiceTypeUsableForOnlyNpc;

		// Token: 0x04000474 RID: 1140
		private MBReadOnlyList<bool> _tabAvailabilities;

		// Token: 0x04000475 RID: 1141
		private Action<float> _onHeightChanged;

		// Token: 0x04000476 RID: 1142
		private Action _onAgeChanged;

		// Token: 0x04000477 RID: 1143
		private int _initialRace = -1;

		// Token: 0x04000478 RID: 1144
		private int _initialGender = -1;

		// Token: 0x04000479 RID: 1145
		private BodyMeshMaturityType _latestMaturityType;

		// Token: 0x0400047A RID: 1146
		private bool _isRandomizing;

		// Token: 0x0400047B RID: 1147
		private readonly Action<int> _goToIndex;

		// Token: 0x0400047C RID: 1148
		private FaceGenVM.GenderBasedSelectedValue[] genderBasedSelectedValues;

		// Token: 0x0400047D RID: 1149
		private readonly Dictionary<FaceGenVM.FaceGenTabs, MBBindingList<FaceGenPropertyVM>> _tabProperties;

		// Token: 0x0400047E RID: 1150
		private List<uint> _skinColors;

		// Token: 0x0400047F RID: 1151
		private List<uint> _hairColors;

		// Token: 0x04000480 RID: 1152
		private List<uint> _tattooColors;

		// Token: 0x04000481 RID: 1153
		private readonly bool _showDebugValues;

		// Token: 0x04000482 RID: 1154
		private readonly bool _openedFromMultiplayer;

		// Token: 0x04000483 RID: 1155
		private bool _enforceConstraints;

		// Token: 0x04000484 RID: 1156
		private IFaceGeneratorCustomFilter _filter;

		// Token: 0x04000485 RID: 1157
		private InputKeyItemVM _cancelInputKey;

		// Token: 0x04000486 RID: 1158
		private InputKeyItemVM _doneInputKey;

		// Token: 0x04000487 RID: 1159
		private InputKeyItemVM _previousTabInputKey;

		// Token: 0x04000488 RID: 1160
		private InputKeyItemVM _nextTabInputKey;

		// Token: 0x04000489 RID: 1161
		private MBBindingList<InputKeyItemVM> _cameraControlKeys;

		// Token: 0x0400048A RID: 1162
		private bool _isBodyEnabled;

		// Token: 0x0400048B RID: 1163
		private bool _isFaceEnabled;

		// Token: 0x0400048C RID: 1164
		private bool _isEyesEnabled;

		// Token: 0x0400048D RID: 1165
		private bool _isNoseEnabled;

		// Token: 0x0400048E RID: 1166
		private bool _isMouthEnabled;

		// Token: 0x0400048F RID: 1167
		private bool _isHairEnabled;

		// Token: 0x04000490 RID: 1168
		private bool _isTaintEnabled;

		// Token: 0x04000491 RID: 1169
		private string _cancelBtnLbl;

		// Token: 0x04000492 RID: 1170
		private string _doneBtnLbl;

		// Token: 0x04000493 RID: 1171
		private int _initialSelectedTaintType;

		// Token: 0x04000494 RID: 1172
		private int _initialSelectedHairType;

		// Token: 0x04000495 RID: 1173
		private int _initialSelectedBeardType;

		// Token: 0x04000496 RID: 1174
		private float _initialSelectedSkinColor;

		// Token: 0x04000497 RID: 1175
		private float _initialSelectedHairColor;

		// Token: 0x04000498 RID: 1176
		private float _initialSelectedTaintColor;

		// Token: 0x04000499 RID: 1177
		private string _flipHairLbl;

		// Token: 0x0400049A RID: 1178
		private string _skinColorLbl;

		// Token: 0x0400049B RID: 1179
		private string _raceLbl;

		// Token: 0x0400049C RID: 1180
		private string _genderLbl;

		// Token: 0x0400049D RID: 1181
		private FaceGenPropertyVM _heightSlider;

		// Token: 0x0400049E RID: 1182
		private HintViewModel _bodyHint;

		// Token: 0x0400049F RID: 1183
		private HintViewModel _faceHint;

		// Token: 0x040004A0 RID: 1184
		private HintViewModel _eyesHint;

		// Token: 0x040004A1 RID: 1185
		private HintViewModel _noseHint;

		// Token: 0x040004A2 RID: 1186
		private HintViewModel _hairHint;

		// Token: 0x040004A3 RID: 1187
		private HintViewModel _taintHint;

		// Token: 0x040004A4 RID: 1188
		private HintViewModel _mouthHint;

		// Token: 0x040004A5 RID: 1189
		private HintViewModel _redoHint;

		// Token: 0x040004A6 RID: 1190
		private HintViewModel _undoHint;

		// Token: 0x040004A7 RID: 1191
		private HintViewModel _randomizeHint;

		// Token: 0x040004A8 RID: 1192
		private HintViewModel _randomizeAllHint;

		// Token: 0x040004A9 RID: 1193
		private HintViewModel _resetHint;

		// Token: 0x040004AA RID: 1194
		private HintViewModel _resetAllHint;

		// Token: 0x040004AB RID: 1195
		private HintViewModel _clothHint;

		// Token: 0x040004AC RID: 1196
		private int hairNum;

		// Token: 0x040004AD RID: 1197
		private int beardNum;

		// Token: 0x040004AE RID: 1198
		private int faceTextureNum;

		// Token: 0x040004AF RID: 1199
		private int mouthTextureNum;

		// Token: 0x040004B0 RID: 1200
		private int eyebrowTextureNum;

		// Token: 0x040004B1 RID: 1201
		private int faceTattooNum;

		// Token: 0x040004B2 RID: 1202
		private int _newSoundPresetSize;

		// Token: 0x040004B3 RID: 1203
		private float _scale = 1f;

		// Token: 0x040004B4 RID: 1204
		private int _tab = -1;

		// Token: 0x040004B5 RID: 1205
		private int _selectedRace = -1;

		// Token: 0x040004B6 RID: 1206
		private int _selectedGender = -1;

		// Token: 0x040004B7 RID: 1207
		private bool _canChangeGender;

		// Token: 0x040004B8 RID: 1208
		private bool _canChangeRace;

		// Token: 0x040004B9 RID: 1209
		private bool _isDressed;

		// Token: 0x040004BA RID: 1210
		private bool _characterGamepadControlsEnabled;

		// Token: 0x040004BB RID: 1211
		private bool _isUndoEnabled;

		// Token: 0x040004BC RID: 1212
		private bool _isRedoEnabled;

		// Token: 0x040004BD RID: 1213
		private MBBindingList<FaceGenPropertyVM> _bodyProperties;

		// Token: 0x040004BE RID: 1214
		private MBBindingList<FaceGenPropertyVM> _faceProperties;

		// Token: 0x040004BF RID: 1215
		private MBBindingList<FaceGenPropertyVM> _eyesProperties;

		// Token: 0x040004C0 RID: 1216
		private MBBindingList<FaceGenPropertyVM> _noseProperties;

		// Token: 0x040004C1 RID: 1217
		private MBBindingList<FaceGenPropertyVM> _mouthProperties;

		// Token: 0x040004C2 RID: 1218
		private MBBindingList<FaceGenPropertyVM> _hairProperties;

		// Token: 0x040004C3 RID: 1219
		private MBBindingList<FaceGenPropertyVM> _taintProperties;

		// Token: 0x040004C4 RID: 1220
		private MBBindingList<FacegenListItemVM> _taintTypes;

		// Token: 0x040004C5 RID: 1221
		private MBBindingList<FacegenListItemVM> _beardTypes;

		// Token: 0x040004C6 RID: 1222
		private MBBindingList<FacegenListItemVM> _hairTypes;

		// Token: 0x040004C7 RID: 1223
		private FaceGenPropertyVM _soundPreset;

		// Token: 0x040004C8 RID: 1224
		private FaceGenPropertyVM _faceTypes;

		// Token: 0x040004C9 RID: 1225
		private FaceGenPropertyVM _teethTypes;

		// Token: 0x040004CA RID: 1226
		private FaceGenPropertyVM _eyebrowTypes;

		// Token: 0x040004CB RID: 1227
		private SelectorVM<SelectorItemVM> _skinColorSelector;

		// Token: 0x040004CC RID: 1228
		private SelectorVM<SelectorItemVM> _hairColorSelector;

		// Token: 0x040004CD RID: 1229
		private SelectorVM<SelectorItemVM> _tattooColorSelector;

		// Token: 0x040004CE RID: 1230
		private SelectorVM<SelectorItemVM> _raceSelector;

		// Token: 0x040004CF RID: 1231
		private FacegenListItemVM _selectedTaintType;

		// Token: 0x040004D0 RID: 1232
		private FacegenListItemVM _selectedBeardType;

		// Token: 0x040004D1 RID: 1233
		private FacegenListItemVM _selectedHairType;

		// Token: 0x040004D2 RID: 1234
		private string _title = "";

		// Token: 0x040004D3 RID: 1235
		private int _totalStageCount = -1;

		// Token: 0x040004D4 RID: 1236
		private int _currentStageIndex = -1;

		// Token: 0x040004D5 RID: 1237
		private int _furthestIndex = -1;

		// Token: 0x02000116 RID: 278
		public enum FaceGenTabs
		{
			// Token: 0x040006EB RID: 1771
			None = -1,
			// Token: 0x040006EC RID: 1772
			Body,
			// Token: 0x040006ED RID: 1773
			Face,
			// Token: 0x040006EE RID: 1774
			Eyes,
			// Token: 0x040006EF RID: 1775
			Nose,
			// Token: 0x040006F0 RID: 1776
			Mouth,
			// Token: 0x040006F1 RID: 1777
			Hair,
			// Token: 0x040006F2 RID: 1778
			Taint,
			// Token: 0x040006F3 RID: 1779
			NumOfFaceGenTabs
		}

		// Token: 0x02000117 RID: 279
		public enum Presets
		{
			// Token: 0x040006F5 RID: 1781
			Gender = -1,
			// Token: 0x040006F6 RID: 1782
			FacePresets = -2,
			// Token: 0x040006F7 RID: 1783
			FaceType = -3,
			// Token: 0x040006F8 RID: 1784
			EyePresets = -4,
			// Token: 0x040006F9 RID: 1785
			HairBeardPreset = -5,
			// Token: 0x040006FA RID: 1786
			HairType = -6,
			// Token: 0x040006FB RID: 1787
			BeardType = -7,
			// Token: 0x040006FC RID: 1788
			TaintPresets = -8,
			// Token: 0x040006FD RID: 1789
			SoundPresets = -9,
			// Token: 0x040006FE RID: 1790
			TaintType = -10,
			// Token: 0x040006FF RID: 1791
			Age = -11,
			// Token: 0x04000700 RID: 1792
			EyeColor = -12,
			// Token: 0x04000701 RID: 1793
			HairAndBeardColor = -13,
			// Token: 0x04000702 RID: 1794
			TeethType = -14,
			// Token: 0x04000703 RID: 1795
			EyebrowType = -15,
			// Token: 0x04000704 RID: 1796
			Scale = -16,
			// Token: 0x04000705 RID: 1797
			Weight = -17,
			// Token: 0x04000706 RID: 1798
			Build = -18,
			// Token: 0x04000707 RID: 1799
			Pitch = -19,
			// Token: 0x04000708 RID: 1800
			Race = -20
		}

		// Token: 0x02000118 RID: 280
		public struct GenderBasedSelectedValue
		{
			// Token: 0x06000D97 RID: 3479 RVA: 0x0002B062 File Offset: 0x00029262
			public void Reset()
			{
				this.Hair = -1;
				this.Beard = -1;
				this.FaceTexture = -1;
				this.MouthTexture = -1;
				this.Tattoo = -1;
				this.SoundPreset = -1;
				this.EyebrowTexture = -1;
			}

			// Token: 0x04000709 RID: 1801
			public int Hair;

			// Token: 0x0400070A RID: 1802
			public int Beard;

			// Token: 0x0400070B RID: 1803
			public int FaceTexture;

			// Token: 0x0400070C RID: 1804
			public int MouthTexture;

			// Token: 0x0400070D RID: 1805
			public int Tattoo;

			// Token: 0x0400070E RID: 1806
			public int SoundPreset;

			// Token: 0x0400070F RID: 1807
			public int EyebrowTexture;
		}
	}
}
