using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SandBox.ViewModelCollection.Input;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.SaveSystem;
using TaleWorlds.ScreenSystem;

namespace SandBox.ViewModelCollection.SaveLoad
{
	// Token: 0x02000017 RID: 23
	public class SaveLoadVM : ViewModel
	{
		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060001FA RID: 506 RVA: 0x00009131 File Offset: 0x00007331
		private IEnumerable<SaveGameFileInfo> _allSavedGames
		{
			get
			{
				return this.SaveGroups.SelectMany<SavedGameGroupVM, SaveGameFileInfo>((SavedGameGroupVM s) => s.SavedGamesList.Select<SavedGameVM, SaveGameFileInfo>((SavedGameVM v) => v.Save));
			}
		}

		// Token: 0x060001FB RID: 507 RVA: 0x00009160 File Offset: 0x00007360
		public SaveLoadVM(bool isSaving, bool isCampaignMapOnStack)
		{
			this._isSaving = isSaving;
			this.SaveGroups = new MBBindingList<SavedGameGroupVM>();
			this.IsVisualDisabled = false;
		}

		// Token: 0x060001FC RID: 508 RVA: 0x00009228 File Offset: 0x00007428
		public async Task InitializeAsync()
		{
			if (!this.IsLoadingSaves)
			{
				this.IsBusyWithAnAction = true;
				this.IsLoadingSaves = true;
				try
				{
					SaveGameFileInfo[] array = await Task.Run<SaveGameFileInfo[]>(() => MBSaveLoad.GetSaveFiles(null));
					if (!this._isFinalized)
					{
						int num = 0;
						IEnumerable<SaveGameFileInfo> enumerable = array.Where<SaveGameFileInfo>((SaveGameFileInfo s) => s.IsCorrupted);
						foreach (IGrouping<string, SaveGameFileInfo> grouping in from s in array
							where !s.IsCorrupted
							select s into m
							group m by m.MetaData.GetUniqueGameId() into s
							orderby this.GetMostRecentSaveInGroup(s) descending
							select s)
						{
							SavedGameGroupVM savedGameGroupVM = new SavedGameGroupVM();
							if (string.IsNullOrWhiteSpace(grouping.Key))
							{
								savedGameGroupVM.IdentifierID = this._uncategorizedSaveGroupName.ToString();
							}
							else
							{
								num++;
								this._categorizedSaveGroupName.SetTextVariable("ID", num);
								savedGameGroupVM.IdentifierID = this._categorizedSaveGroupName.ToString();
							}
							foreach (SaveGameFileInfo saveGameFileInfo in grouping.OrderByDescending<SaveGameFileInfo, DateTime>((SaveGameFileInfo s) => s.MetaData.GetCreationTime()))
							{
								bool ironmanMode = saveGameFileInfo.MetaData.GetIronmanMode();
								savedGameGroupVM.SavedGamesList.Add(new SavedGameVM(saveGameFileInfo, this.IsSaving, new Action<SavedGameVM>(this.OnDeleteSavedGame), new Action<SavedGameVM>(this.OnSaveSelection), new Action(this.OnCancelLoadSave), new Action(this.ExecuteDone), false, ironmanMode));
							}
							this.SaveGroups.Add(savedGameGroupVM);
						}
						if (enumerable.Any<SaveGameFileInfo>())
						{
							SavedGameGroupVM savedGameGroupVM2 = new SavedGameGroupVM();
							savedGameGroupVM2.IdentifierID = new TextObject("{=o9PIe7am}Corrupted", null).ToString();
							foreach (SaveGameFileInfo saveGameFileInfo2 in enumerable)
							{
								savedGameGroupVM2.SavedGamesList.Add(new SavedGameVM(saveGameFileInfo2, this.IsSaving, new Action<SavedGameVM>(this.OnDeleteSavedGame), new Action<SavedGameVM>(this.OnSaveSelection), new Action(this.OnCancelLoadSave), new Action(this.ExecuteDone), true, false));
							}
							this.SaveGroups.Add(savedGameGroupVM2);
						}
						this.RefreshCanCreateNewSave();
						this.RefreshCanSearch();
						this.OnSaveSelection(this.GetFirstAvailableSavedGame());
						this.RefreshValues();
					}
				}
				catch (Exception ex)
				{
					Debug.Print("SaveLoadVM.InitializeAsync failed: " + ex, 0, Debug.DebugColor.White, 17592186044416UL);
				}
				finally
				{
					this.IsBusyWithAnAction = false;
					this.IsLoadingSaves = false;
				}
			}
		}

		// Token: 0x060001FD RID: 509 RVA: 0x00009270 File Offset: 0x00007470
		private SavedGameVM GetFirstAvailableSavedGame()
		{
			for (int i = 0; i < this.SaveGroups.Count; i++)
			{
				SavedGameGroupVM savedGameGroupVM = this.SaveGroups[i];
				for (int j = 0; j < savedGameGroupVM.SavedGamesList.Count; j++)
				{
					SavedGameVM savedGameVM = savedGameGroupVM.SavedGamesList[j];
					if (!savedGameVM.IsCorrupted && !savedGameVM.IsDisabled)
					{
						return savedGameVM;
					}
				}
			}
			return null;
		}

		// Token: 0x060001FE RID: 510 RVA: 0x000092D6 File Offset: 0x000074D6
		private void RefreshCanCreateNewSave()
		{
			this.CanCreateNewSave = !MBSaveLoad.IsMaxNumberOfSavesReached();
			this.CreateNewSaveHint = new HintViewModel(this.CanCreateNewSave ? null : new TextObject("{=DeXfSjgY}Cannot create a new save. Save limit reached.", null), null);
		}

		// Token: 0x060001FF RID: 511 RVA: 0x00009308 File Offset: 0x00007508
		private void RefreshCanSearch()
		{
			this.IsSearchAvailable = true;
		}

		// Token: 0x06000200 RID: 512 RVA: 0x00009314 File Offset: 0x00007514
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.TitleText = new TextObject("{=hiCxFj4E}Saved Campaigns", null).ToString();
			this.DoneText = new TextObject("{=WiNRdfsm}Done", null).ToString();
			this.CreateNewSaveSlotText = new TextObject("{=eL8nhkhQ}Create New Save Slot", null).ToString();
			this.CancelText = new TextObject("{=3CpNUnVl}Cancel", null).ToString();
			this.SaveLoadText = (this._isSaving ? new TextObject("{=bV75iwKa}Save", null).ToString() : new TextObject("{=9NuttOBC}Load", null).ToString());
			this.SearchPlaceholderText = new TextObject("{=tQOPRBFg}Search...", null).ToString();
			if (this.IsVisualDisabled)
			{
				this.VisualDisabledText = this._visualIsDisabledText.ToString();
			}
			this.SaveGroups.ApplyActionOnAllItems(delegate(SavedGameGroupVM x)
			{
				x.RefreshValues();
			});
			SavedGameVM currentSelectedSave = this.CurrentSelectedSave;
			if (currentSelectedSave == null)
			{
				return;
			}
			currentSelectedSave.RefreshValues();
		}

		// Token: 0x06000201 RID: 513 RVA: 0x00009418 File Offset: 0x00007618
		private DateTime GetMostRecentSaveInGroup(IGrouping<string, SaveGameFileInfo> group)
		{
			SaveGameFileInfo saveGameFileInfo = group.OrderByDescending<SaveGameFileInfo, DateTime>((SaveGameFileInfo g) => g.MetaData.GetCreationTime()).FirstOrDefault<SaveGameFileInfo>();
			if (saveGameFileInfo == null)
			{
				return default(DateTime);
			}
			return saveGameFileInfo.MetaData.GetCreationTime();
		}

		// Token: 0x06000202 RID: 514 RVA: 0x00009468 File Offset: 0x00007668
		private void OnSaveSelection(SavedGameVM save)
		{
			if (save != this.CurrentSelectedSave)
			{
				if (this.CurrentSelectedSave != null)
				{
					this.CurrentSelectedSave.IsSelected = false;
				}
				this.CurrentSelectedSave = save;
				if (this.CurrentSelectedSave != null)
				{
					this.CurrentSelectedSave.IsSelected = true;
				}
				this.IsAnyItemSelected = this.CurrentSelectedSave != null;
				this.IsActionEnabled = this.IsAnyItemSelected && !this.CurrentSelectedSave.IsCorrupted && !this.CurrentSelectedSave.IsDisabled;
			}
		}

		// Token: 0x06000203 RID: 515 RVA: 0x000094E8 File Offset: 0x000076E8
		public void ExecuteCreateNewSaveGame()
		{
			InformationManager.ShowTextInquiry(new TextInquiryData(new TextObject("{=7WdWK2Dt}Save Game", null).ToString(), new TextObject("{=WDlVhNuq}Name your save file", null).ToString(), true, true, new TextObject("{=WiNRdfsm}Done", null).ToString(), new TextObject("{=3CpNUnVl}Cancel", null).ToString(), new Action<string>(this.OnSaveAsDone), null, false, new Func<string, Tuple<bool, string>>(this.IsSaveGameNameApplicable), "", ""), false, false);
		}

		// Token: 0x06000204 RID: 516 RVA: 0x00009568 File Offset: 0x00007768
		private Tuple<bool, string> IsSaveGameNameApplicable(string inputText)
		{
			string text = string.Empty;
			bool flag = true;
			if (string.IsNullOrEmpty(inputText))
			{
				text = this._textIsEmptyReasonText.ToString();
				flag = false;
			}
			else if (inputText.All<char>((char c) => char.IsWhiteSpace(c)))
			{
				text = this._allSpaceReasonText.ToString();
				flag = false;
			}
			else if (inputText.Any<char>((char c) => !char.IsLetterOrDigit(c) && !char.IsWhiteSpace(c)))
			{
				text = this._textHasSpecialCharReasonText.ToString();
				flag = false;
			}
			else if (inputText.Length >= 30)
			{
				this._textTooLongReasonText.SetTextVariable("MAX_LENGTH", 30);
				text = this._textTooLongReasonText.ToString();
				flag = false;
			}
			else if (MBSaveLoad.IsSaveFileNameReserved(inputText))
			{
				text = this._saveNameReservedReasonText.ToString();
				flag = false;
			}
			else if (this._allSavedGames.Any<SaveGameFileInfo>((SaveGameFileInfo s) => string.Equals(s.Name, inputText, StringComparison.InvariantCultureIgnoreCase)))
			{
				text = this._saveAlreadyExistsReasonText.ToString();
				flag = false;
			}
			return new Tuple<bool, string>(flag, text);
		}

		// Token: 0x06000205 RID: 517 RVA: 0x000096A2 File Offset: 0x000078A2
		private void OnSaveAsDone(string saveName)
		{
			Campaign.Current.SaveHandler.SaveAs(saveName);
			this.ExecuteDone();
		}

		// Token: 0x06000206 RID: 518 RVA: 0x000096BA File Offset: 0x000078BA
		public void ExecuteDone()
		{
			ScreenManager.PopScreen();
		}

		// Token: 0x06000207 RID: 519 RVA: 0x000096C1 File Offset: 0x000078C1
		public void ExecuteLoadSave()
		{
			this.LoadSelectedSave();
		}

		// Token: 0x06000208 RID: 520 RVA: 0x000096C9 File Offset: 0x000078C9
		private void LoadSelectedSave()
		{
			if (!this.IsBusyWithAnAction && this.CurrentSelectedSave != null && !this.CurrentSelectedSave.IsCorrupted && !this.CurrentSelectedSave.IsDisabled)
			{
				this.CurrentSelectedSave.ExecuteSaveLoad();
				this.IsBusyWithAnAction = true;
			}
		}

		// Token: 0x06000209 RID: 521 RVA: 0x00009707 File Offset: 0x00007907
		private void OnCancelLoadSave()
		{
			this.IsBusyWithAnAction = false;
		}

		// Token: 0x0600020A RID: 522 RVA: 0x00009710 File Offset: 0x00007910
		private void ExecuteResetCurrentSave()
		{
			this.CurrentSelectedSave = null;
		}

		// Token: 0x0600020B RID: 523 RVA: 0x0000971C File Offset: 0x0000791C
		private void OnDeleteSavedGame(SavedGameVM savedGame)
		{
			if (!this.IsBusyWithAnAction)
			{
				this.IsBusyWithAnAction = true;
				string text = new TextObject("{=M1AEHJ76}Please notice that this save is created for a session which has Ironman mode enabled. There is no other save file for the related session. Are you sure you want to delete this save game?", null).ToString();
				string text2 = new TextObject("{=HH2mZq8J}Are you sure you want to delete this save game?", null).ToString();
				string text3 = new TextObject("{=QHV8aeEg}Delete Save", null).ToString();
				string text4 = (savedGame.Save.MetaData.GetIronmanMode() ? text : text2);
				InformationManager.ShowInquiry(new InquiryData(text3, text4, true, true, new TextObject("{=aeouhelq}Yes", null).ToString(), new TextObject("{=8OkPHu4f}No", null).ToString(), delegate
				{
					this.IsBusyWithAnAction = true;
					bool flag = MBSaveLoad.DeleteSaveGame(savedGame.Save.Name);
					this.IsBusyWithAnAction = false;
					if (flag)
					{
						this.DeleteSave(savedGame);
						this.OnSaveSelection(this.GetFirstAvailableSavedGame());
						this.RefreshCanCreateNewSave();
						this.RefreshCanSearch();
						return;
					}
					this.OnDeleteSaveUnsuccessful();
				}, delegate
				{
					this.IsBusyWithAnAction = false;
				}, "", 0f, null, null, null), false, false);
			}
		}

		// Token: 0x0600020C RID: 524 RVA: 0x000097F4 File Offset: 0x000079F4
		private void OnDeleteSaveUnsuccessful()
		{
			string text = new TextObject("{=oZrVNUOk}Error", null).ToString();
			string text2 = new TextObject("{=PY00wRz4}Failed to delete the save file.", null).ToString();
			InformationManager.ShowInquiry(new InquiryData(text, text2, true, false, new TextObject("{=WiNRdfsm}Done", null).ToString(), string.Empty, null, null, "", 0f, null, null, null), false, false);
		}

		// Token: 0x0600020D RID: 525 RVA: 0x00009858 File Offset: 0x00007A58
		private void DeleteSave(SavedGameVM save)
		{
			foreach (SavedGameGroupVM savedGameGroupVM in this.SaveGroups)
			{
				if (savedGameGroupVM.SavedGamesList.Contains(save))
				{
					savedGameGroupVM.SavedGamesList.Remove(save);
					break;
				}
			}
			if (string.IsNullOrEmpty(BannerlordConfig.LatestSaveGameName) || save.Save.Name == BannerlordConfig.LatestSaveGameName)
			{
				SavedGameVM firstAvailableSavedGame = this.GetFirstAvailableSavedGame();
				BannerlordConfig.LatestSaveGameName = ((firstAvailableSavedGame != null) ? firstAvailableSavedGame.Save.Name : null) ?? string.Empty;
				BannerlordConfig.Save();
			}
		}

		// Token: 0x0600020E RID: 526 RVA: 0x0000990C File Offset: 0x00007B0C
		public void DeleteSelectedSave()
		{
			if (this.CurrentSelectedSave != null)
			{
				this.OnDeleteSavedGame(this.CurrentSelectedSave);
			}
		}

		// Token: 0x0600020F RID: 527 RVA: 0x00009924 File Offset: 0x00007B24
		public override void OnFinalize()
		{
			this._isFinalized = true;
			base.OnFinalize();
			InputKeyItemVM doneInputKey = this.DoneInputKey;
			if (doneInputKey != null)
			{
				doneInputKey.OnFinalize();
			}
			InputKeyItemVM cancelInputKey = this.CancelInputKey;
			if (cancelInputKey != null)
			{
				cancelInputKey.OnFinalize();
			}
			InputKeyItemVM deleteInputKey = this.DeleteInputKey;
			if (deleteInputKey == null)
			{
				return;
			}
			deleteInputKey.OnFinalize();
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000210 RID: 528 RVA: 0x00009970 File Offset: 0x00007B70
		// (set) Token: 0x06000211 RID: 529 RVA: 0x00009978 File Offset: 0x00007B78
		[DataSourceProperty]
		public bool IsLoadingSaves
		{
			get
			{
				return this._isLoadingSaves;
			}
			set
			{
				if (value != this._isLoadingSaves)
				{
					this._isLoadingSaves = value;
					base.OnPropertyChangedWithValue(value, "IsLoadingSaves");
				}
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000212 RID: 530 RVA: 0x00009996 File Offset: 0x00007B96
		// (set) Token: 0x06000213 RID: 531 RVA: 0x0000999E File Offset: 0x00007B9E
		[DataSourceProperty]
		public bool IsBusyWithAnAction
		{
			get
			{
				return this._isBusyWithAnAction;
			}
			set
			{
				if (value != this._isBusyWithAnAction)
				{
					this._isBusyWithAnAction = value;
					base.OnPropertyChangedWithValue(value, "IsBusyWithAnAction");
				}
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000214 RID: 532 RVA: 0x000099BC File Offset: 0x00007BBC
		// (set) Token: 0x06000215 RID: 533 RVA: 0x000099C4 File Offset: 0x00007BC4
		[DataSourceProperty]
		public bool IsSearchAvailable
		{
			get
			{
				return this._isSearchAvailable;
			}
			set
			{
				if (value != this._isSearchAvailable)
				{
					this._isSearchAvailable = value;
					base.OnPropertyChangedWithValue(value, "IsSearchAvailable");
				}
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000216 RID: 534 RVA: 0x000099E2 File Offset: 0x00007BE2
		// (set) Token: 0x06000217 RID: 535 RVA: 0x000099EA File Offset: 0x00007BEA
		[DataSourceProperty]
		public string SearchText
		{
			get
			{
				return this._searchText;
			}
			set
			{
				if (value != this._searchText)
				{
					value.IndexOf(this._searchText ?? "");
					this._searchText = value;
					base.OnPropertyChangedWithValue<string>(value, "SearchText");
				}
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000218 RID: 536 RVA: 0x00009A23 File Offset: 0x00007C23
		// (set) Token: 0x06000219 RID: 537 RVA: 0x00009A2B File Offset: 0x00007C2B
		[DataSourceProperty]
		public string SearchPlaceholderText
		{
			get
			{
				return this._searchPlaceholderText;
			}
			set
			{
				if (value != this._searchPlaceholderText)
				{
					this._searchPlaceholderText = value;
					base.OnPropertyChangedWithValue<string>(value, "SearchPlaceholderText");
				}
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x0600021A RID: 538 RVA: 0x00009A4E File Offset: 0x00007C4E
		// (set) Token: 0x0600021B RID: 539 RVA: 0x00009A56 File Offset: 0x00007C56
		[DataSourceProperty]
		public string VisualDisabledText
		{
			get
			{
				return this._visualDisabledText;
			}
			set
			{
				if (value != this._visualDisabledText)
				{
					this._visualDisabledText = value;
					base.OnPropertyChangedWithValue<string>(value, "VisualDisabledText");
				}
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x0600021C RID: 540 RVA: 0x00009A79 File Offset: 0x00007C79
		// (set) Token: 0x0600021D RID: 541 RVA: 0x00009A81 File Offset: 0x00007C81
		[DataSourceProperty]
		public MBBindingList<SavedGameGroupVM> SaveGroups
		{
			get
			{
				return this._saveGroups;
			}
			set
			{
				if (value != this._saveGroups)
				{
					this._saveGroups = value;
					base.OnPropertyChangedWithValue<MBBindingList<SavedGameGroupVM>>(value, "SaveGroups");
				}
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x0600021E RID: 542 RVA: 0x00009A9F File Offset: 0x00007C9F
		// (set) Token: 0x0600021F RID: 543 RVA: 0x00009AA7 File Offset: 0x00007CA7
		[DataSourceProperty]
		public SavedGameVM CurrentSelectedSave
		{
			get
			{
				return this._currentSelectedSave;
			}
			set
			{
				if (value != this._currentSelectedSave)
				{
					this._currentSelectedSave = value;
					base.OnPropertyChangedWithValue<SavedGameVM>(value, "CurrentSelectedSave");
				}
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000220 RID: 544 RVA: 0x00009AC5 File Offset: 0x00007CC5
		// (set) Token: 0x06000221 RID: 545 RVA: 0x00009ACD File Offset: 0x00007CCD
		[DataSourceProperty]
		public string CreateNewSaveSlotText
		{
			get
			{
				return this._createNewSaveSlotText;
			}
			set
			{
				if (value != this._createNewSaveSlotText)
				{
					this._createNewSaveSlotText = value;
					base.OnPropertyChangedWithValue<string>(value, "CreateNewSaveSlotText");
				}
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000222 RID: 546 RVA: 0x00009AF0 File Offset: 0x00007CF0
		// (set) Token: 0x06000223 RID: 547 RVA: 0x00009AF8 File Offset: 0x00007CF8
		[DataSourceProperty]
		public string TitleText
		{
			get
			{
				return this._titleText;
			}
			set
			{
				if (value != this._titleText)
				{
					this._titleText = value;
					base.OnPropertyChangedWithValue<string>(value, "TitleText");
				}
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000224 RID: 548 RVA: 0x00009B1B File Offset: 0x00007D1B
		// (set) Token: 0x06000225 RID: 549 RVA: 0x00009B23 File Offset: 0x00007D23
		[DataSourceProperty]
		public string CancelText
		{
			get
			{
				return this._cancelText;
			}
			set
			{
				if (value != this._cancelText)
				{
					this._cancelText = value;
					base.OnPropertyChangedWithValue<string>(value, "CancelText");
				}
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000226 RID: 550 RVA: 0x00009B46 File Offset: 0x00007D46
		// (set) Token: 0x06000227 RID: 551 RVA: 0x00009B4E File Offset: 0x00007D4E
		[DataSourceProperty]
		public bool IsSaving
		{
			get
			{
				return this._isSaving;
			}
			set
			{
				if (value != this._isSaving)
				{
					this._isSaving = value;
					base.OnPropertyChangedWithValue(value, "IsSaving");
				}
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000228 RID: 552 RVA: 0x00009B6C File Offset: 0x00007D6C
		// (set) Token: 0x06000229 RID: 553 RVA: 0x00009B74 File Offset: 0x00007D74
		[DataSourceProperty]
		public bool CanCreateNewSave
		{
			get
			{
				return this._canCreateNewSave;
			}
			set
			{
				if (value != this._canCreateNewSave)
				{
					this._canCreateNewSave = value;
					base.OnPropertyChangedWithValue(value, "CanCreateNewSave");
				}
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x0600022A RID: 554 RVA: 0x00009B92 File Offset: 0x00007D92
		// (set) Token: 0x0600022B RID: 555 RVA: 0x00009B9A File Offset: 0x00007D9A
		[DataSourceProperty]
		public bool IsVisualDisabled
		{
			get
			{
				return this._isVisualDisabled;
			}
			set
			{
				if (value != this._isVisualDisabled)
				{
					this._isVisualDisabled = value;
					base.OnPropertyChangedWithValue(value, "IsVisualDisabled");
				}
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x0600022C RID: 556 RVA: 0x00009BB8 File Offset: 0x00007DB8
		// (set) Token: 0x0600022D RID: 557 RVA: 0x00009BC0 File Offset: 0x00007DC0
		[DataSourceProperty]
		public HintViewModel CreateNewSaveHint
		{
			get
			{
				return this._createNewSaveHint;
			}
			set
			{
				if (value != this._createNewSaveHint)
				{
					this._createNewSaveHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "CreateNewSaveHint");
				}
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x0600022E RID: 558 RVA: 0x00009BDE File Offset: 0x00007DDE
		// (set) Token: 0x0600022F RID: 559 RVA: 0x00009BE6 File Offset: 0x00007DE6
		[DataSourceProperty]
		public bool IsActionEnabled
		{
			get
			{
				return this._isActionEnabled;
			}
			set
			{
				if (value != this._isActionEnabled)
				{
					this._isActionEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsActionEnabled");
				}
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000230 RID: 560 RVA: 0x00009C04 File Offset: 0x00007E04
		// (set) Token: 0x06000231 RID: 561 RVA: 0x00009C0C File Offset: 0x00007E0C
		[DataSourceProperty]
		public bool IsAnyItemSelected
		{
			get
			{
				return this._isAnyItemSelected;
			}
			set
			{
				if (value != this._isAnyItemSelected)
				{
					this._isAnyItemSelected = value;
					base.OnPropertyChangedWithValue(value, "IsAnyItemSelected");
				}
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000232 RID: 562 RVA: 0x00009C2A File Offset: 0x00007E2A
		// (set) Token: 0x06000233 RID: 563 RVA: 0x00009C32 File Offset: 0x00007E32
		[DataSourceProperty]
		public string DoneText
		{
			get
			{
				return this._doneText;
			}
			set
			{
				if (value != this._doneText)
				{
					this._doneText = value;
					base.OnPropertyChangedWithValue<string>(value, "DoneText");
				}
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000234 RID: 564 RVA: 0x00009C55 File Offset: 0x00007E55
		// (set) Token: 0x06000235 RID: 565 RVA: 0x00009C5D File Offset: 0x00007E5D
		[DataSourceProperty]
		public string SaveLoadText
		{
			get
			{
				return this._saveLoadText;
			}
			set
			{
				if (value != this._saveLoadText)
				{
					this._saveLoadText = value;
					base.OnPropertyChangedWithValue<string>(value, "SaveLoadText");
				}
			}
		}

		// Token: 0x06000236 RID: 566 RVA: 0x00009C80 File Offset: 0x00007E80
		public void SetDoneInputKey(HotKey hotkey)
		{
			this.DoneInputKey = InputKeyItemVM.CreateFromHotKey(hotkey, true);
		}

		// Token: 0x06000237 RID: 567 RVA: 0x00009C8F File Offset: 0x00007E8F
		public void SetCancelInputKey(HotKey hotkey)
		{
			this.CancelInputKey = InputKeyItemVM.CreateFromHotKey(hotkey, true);
		}

		// Token: 0x06000238 RID: 568 RVA: 0x00009C9E File Offset: 0x00007E9E
		public void SetDeleteInputKey(HotKey hotkey)
		{
			this.DeleteInputKey = InputKeyItemVM.CreateFromHotKey(hotkey, true);
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000239 RID: 569 RVA: 0x00009CAD File Offset: 0x00007EAD
		// (set) Token: 0x0600023A RID: 570 RVA: 0x00009CB5 File Offset: 0x00007EB5
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

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x0600023B RID: 571 RVA: 0x00009CD3 File Offset: 0x00007ED3
		// (set) Token: 0x0600023C RID: 572 RVA: 0x00009CDB File Offset: 0x00007EDB
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

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x0600023D RID: 573 RVA: 0x00009CF9 File Offset: 0x00007EF9
		// (set) Token: 0x0600023E RID: 574 RVA: 0x00009D01 File Offset: 0x00007F01
		public InputKeyItemVM DeleteInputKey
		{
			get
			{
				return this._deleteInputKey;
			}
			set
			{
				if (value != this._deleteInputKey)
				{
					this._deleteInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "DeleteInputKey");
				}
			}
		}

		// Token: 0x040000E3 RID: 227
		private const int _maxSaveFileNameLength = 30;

		// Token: 0x040000E4 RID: 228
		private readonly TextObject _categorizedSaveGroupName = new TextObject("{=nVGqjtaa}Campaign {ID}", null);

		// Token: 0x040000E5 RID: 229
		private readonly TextObject _uncategorizedSaveGroupName = new TextObject("{=uncategorized_save}Uncategorized", null);

		// Token: 0x040000E6 RID: 230
		private readonly TextObject _textIsEmptyReasonText = new TextObject("{=7AI8jA0b}Input text cannot be empty.", null);

		// Token: 0x040000E7 RID: 231
		private readonly TextObject _textHasSpecialCharReasonText = new TextObject("{=kXRdeawC}Input text cannot include special characters.", null);

		// Token: 0x040000E8 RID: 232
		private readonly TextObject _textTooLongReasonText = new TextObject("{=B3W6fcQX}Input text cannot be longer than {MAX_LENGTH} characters.", null);

		// Token: 0x040000E9 RID: 233
		private readonly TextObject _saveAlreadyExistsReasonText = new TextObject("{=aG6XMhA1}A saved game file already exists with this name.", null);

		// Token: 0x040000EA RID: 234
		private readonly TextObject _saveNameReservedReasonText = new TextObject("{=M4WMKyE1}Input text includes reserved text.", null);

		// Token: 0x040000EB RID: 235
		private readonly TextObject _allSpaceReasonText = new TextObject("{=Rtakaivj}Input text needs to include at least one non-space character.", null);

		// Token: 0x040000EC RID: 236
		private readonly TextObject _visualIsDisabledText = new TextObject("{=xlEZ02Qw}Character visual is disabled during 'Save As' on the campaign map.", null);

		// Token: 0x040000ED RID: 237
		private bool _isFinalized;

		// Token: 0x040000EE RID: 238
		private bool _isLoadingSaves;

		// Token: 0x040000EF RID: 239
		private bool _isBusyWithAnAction;

		// Token: 0x040000F0 RID: 240
		private bool _isSearchAvailable;

		// Token: 0x040000F1 RID: 241
		private string _searchText;

		// Token: 0x040000F2 RID: 242
		private string _searchPlaceholderText;

		// Token: 0x040000F3 RID: 243
		private string _doneText;

		// Token: 0x040000F4 RID: 244
		private string _createNewSaveSlotText;

		// Token: 0x040000F5 RID: 245
		private string _titleText;

		// Token: 0x040000F6 RID: 246
		private string _visualDisabledText;

		// Token: 0x040000F7 RID: 247
		private bool _isSaving;

		// Token: 0x040000F8 RID: 248
		private bool _isActionEnabled;

		// Token: 0x040000F9 RID: 249
		private bool _isAnyItemSelected;

		// Token: 0x040000FA RID: 250
		private bool _canCreateNewSave;

		// Token: 0x040000FB RID: 251
		private bool _isVisualDisabled;

		// Token: 0x040000FC RID: 252
		private string _saveLoadText;

		// Token: 0x040000FD RID: 253
		private string _cancelText;

		// Token: 0x040000FE RID: 254
		private HintViewModel _createNewSaveHint;

		// Token: 0x040000FF RID: 255
		private MBBindingList<SavedGameGroupVM> _saveGroups;

		// Token: 0x04000100 RID: 256
		private SavedGameVM _currentSelectedSave;

		// Token: 0x04000101 RID: 257
		private InputKeyItemVM _doneInputKey;

		// Token: 0x04000102 RID: 258
		private InputKeyItemVM _cancelInputKey;

		// Token: 0x04000103 RID: 259
		private InputKeyItemVM _deleteInputKey;
	}
}
