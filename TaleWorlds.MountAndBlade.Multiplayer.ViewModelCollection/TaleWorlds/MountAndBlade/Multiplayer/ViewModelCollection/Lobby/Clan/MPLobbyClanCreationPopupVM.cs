using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Messages.FromLobbyServer.ToClient;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;
using TaleWorlds.PlatformService;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Clan
{
	// Token: 0x02000069 RID: 105
	public class MPLobbyClanCreationPopupVM : ViewModel
	{
		// Token: 0x06000A16 RID: 2582 RVA: 0x0001F4B9 File Offset: 0x0001D6B9
		public MPLobbyClanCreationPopupVM()
		{
			this.PartyMembersList = new MBBindingList<MPLobbyClanMemberItemVM>();
			this.PrepareFactionsList();
			this.PrepareSigilIconsList();
			this.RefreshValues();
		}

		// Token: 0x06000A17 RID: 2583 RVA: 0x0001F4E0 File Offset: 0x0001D6E0
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.CreateClanText = new TextObject("{=ECb8IPbA}Create Clan", null).ToString();
			this.NameText = new TextObject("{=PDdh1sBj}Name", null).ToString();
			this.TagText = new TextObject("{=OUvFT99g}Tag", null).ToString();
			this.FactionText = new TextObject("{=PUjDWe5j}Culture", null).ToString();
			this.SigilText = new TextObject("{=P5Z9owOy}Sigil", null).ToString();
			this.CreateText = new TextObject("{=65oGXBYQ}Create", null).ToString();
			this.CancelText = new TextObject("{=3CpNUnVl}Cancel", null).ToString();
			this.WaitingForConfirmationText = new TextObject("{=08KLQa3P}Waiting For Party Members", null).ToString();
			this.ResetAll();
		}

		// Token: 0x06000A18 RID: 2584 RVA: 0x0001F5A9 File Offset: 0x0001D7A9
		private void ResetAll()
		{
			this.ResetErrorTexts();
			this.ResetUserInputs();
		}

		// Token: 0x06000A19 RID: 2585 RVA: 0x0001F5B7 File Offset: 0x0001D7B7
		private void ResetErrorTexts()
		{
			this.NameErrorText = "";
			this.TagErrorText = "";
			this.FactionErrorText = "";
			this.SigilIconErrorText = "";
		}

		// Token: 0x06000A1A RID: 2586 RVA: 0x0001F5E5 File Offset: 0x0001D7E5
		private void ResetUserInputs()
		{
			this.NameInputText = "";
			this.TagInputText = "";
			this.OnFactionSelection(null);
			this.OnSigilIconSelection(null);
		}

		// Token: 0x06000A1B RID: 2587 RVA: 0x0001F60B File Offset: 0x0001D80B
		public void ExecuteOpenPopup()
		{
			this.RefreshValues();
			this.IsEnabled = true;
		}

		// Token: 0x06000A1C RID: 2588 RVA: 0x0001F61A File Offset: 0x0001D81A
		public void ExecuteClosePopup()
		{
			this.IsEnabled = false;
		}

		// Token: 0x06000A1D RID: 2589 RVA: 0x0001F624 File Offset: 0x0001D824
		private void PrepareFactionsList()
		{
			this._selectedFaction = null;
			this.FactionsList = new MBBindingList<MPCultureItemVM>
			{
				new MPCultureItemVM(Game.Current.ObjectManager.GetObject<BasicCultureObject>("vlandia").StringId, new Action<MPCultureItemVM>(this.OnFactionSelection)),
				new MPCultureItemVM(Game.Current.ObjectManager.GetObject<BasicCultureObject>("sturgia").StringId, new Action<MPCultureItemVM>(this.OnFactionSelection)),
				new MPCultureItemVM(Game.Current.ObjectManager.GetObject<BasicCultureObject>("empire").StringId, new Action<MPCultureItemVM>(this.OnFactionSelection)),
				new MPCultureItemVM(Game.Current.ObjectManager.GetObject<BasicCultureObject>("battania").StringId, new Action<MPCultureItemVM>(this.OnFactionSelection)),
				new MPCultureItemVM(Game.Current.ObjectManager.GetObject<BasicCultureObject>("khuzait").StringId, new Action<MPCultureItemVM>(this.OnFactionSelection)),
				new MPCultureItemVM(Game.Current.ObjectManager.GetObject<BasicCultureObject>("aserai").StringId, new Action<MPCultureItemVM>(this.OnFactionSelection))
			};
		}

		// Token: 0x06000A1E RID: 2590 RVA: 0x0001F764 File Offset: 0x0001D964
		private void PrepareSigilIconsList()
		{
			this.IconsList = new MBBindingList<MPLobbySigilItemVM>();
			this._selectedSigilIcon = null;
			foreach (BannerIconGroup bannerIconGroup in BannerManager.Instance.BannerIconGroups)
			{
				if (!bannerIconGroup.IsPattern)
				{
					foreach (KeyValuePair<int, BannerIconData> keyValuePair in bannerIconGroup.AvailableIcons)
					{
						MPLobbySigilItemVM mplobbySigilItemVM = new MPLobbySigilItemVM(keyValuePair.Key, new Action<MPLobbySigilItemVM>(this.OnSigilIconSelection));
						this.IconsList.Add(mplobbySigilItemVM);
					}
				}
			}
		}

		// Token: 0x06000A1F RID: 2591 RVA: 0x0001F830 File Offset: 0x0001DA30
		private void PreparePartyMembersList()
		{
			this.PartyMembersList.Clear();
			foreach (PartyPlayerInLobbyClient partyPlayerInLobbyClient in NetworkMain.GameClient.PlayersInParty)
			{
				if (partyPlayerInLobbyClient.PlayerId != NetworkMain.GameClient.PlayerID)
				{
					MPLobbyClanMemberItemVM mplobbyClanMemberItemVM = new MPLobbyClanMemberItemVM(partyPlayerInLobbyClient.PlayerId);
					mplobbyClanMemberItemVM.InviteAcceptInfo = new TextObject("{=c0ZdKSkn}Waiting", null).ToString();
					this.PartyMembersList.Add(mplobbyClanMemberItemVM);
				}
			}
		}

		// Token: 0x06000A20 RID: 2592 RVA: 0x0001F8D0 File Offset: 0x0001DAD0
		private void OnFactionSelection(MPCultureItemVM faction)
		{
			if (faction != this._selectedFaction)
			{
				if (this._selectedFaction != null)
				{
					this._selectedFaction.IsSelected = false;
				}
				this._selectedFaction = faction;
				if (this._selectedFaction != null)
				{
					this._selectedFaction.IsSelected = true;
					this.FactionErrorText = "";
				}
			}
		}

		// Token: 0x06000A21 RID: 2593 RVA: 0x0001F920 File Offset: 0x0001DB20
		private void OnSigilIconSelection(MPLobbySigilItemVM sigilIcon)
		{
			if (sigilIcon != this._selectedSigilIcon)
			{
				if (this._selectedSigilIcon != null)
				{
					this._selectedSigilIcon.IsSelected = false;
				}
				this._selectedSigilIcon = sigilIcon;
				if (this._selectedSigilIcon != null)
				{
					this._selectedSigilIcon.IsSelected = true;
					this.SigilIconErrorText = "";
				}
			}
		}

		// Token: 0x06000A22 RID: 2594 RVA: 0x0001F970 File Offset: 0x0001DB70
		private void UpdateNameErrorText(StringValidationError error)
		{
			this.NameErrorText = "";
			if (error == StringValidationError.InvalidLength)
			{
				this.NameErrorText = new TextObject("{=bExIl1A2}Name Length Is Invalid", null).ToString();
				return;
			}
			if (error == StringValidationError.AlreadyExists)
			{
				this.NameErrorText = new TextObject("{=Agtv9l7S}This Name Already Exists", null).ToString();
				return;
			}
			if (error == StringValidationError.HasNonLettersCharacters)
			{
				this.NameErrorText = new TextObject("{=lO1hok44}Name Has Invalid Characters In It", null).ToString();
				return;
			}
			if (error == StringValidationError.ContainsProfanity)
			{
				this.NameErrorText = new TextObject("{=cl2DnRYR}Name Should Not Contain Offensive Words", null).ToString();
				return;
			}
			if (error == StringValidationError.Unspecified)
			{
				this.NameErrorText = new TextObject("{=UEgS8RcB}Name Has Invalid Content", null).ToString();
			}
		}

		// Token: 0x06000A23 RID: 2595 RVA: 0x0001FA10 File Offset: 0x0001DC10
		private void UpdateTagErrorText(StringValidationError error)
		{
			this.TagErrorText = "";
			if (error == StringValidationError.InvalidLength)
			{
				this.TagErrorText = new TextObject("{=MjnlWhih}Tag Length Is Invalid", null).ToString();
				return;
			}
			if (error == StringValidationError.AlreadyExists)
			{
				this.TagErrorText = new TextObject("{=ulzyykHO}This Tag Already Exists", null).ToString();
				return;
			}
			if (error == StringValidationError.HasNonLettersCharacters)
			{
				this.TagErrorText = new TextObject("{=FjmxNxZJ}Tag Has Invalid Characters In It", null).ToString();
				return;
			}
			if (error == StringValidationError.ContainsProfanity)
			{
				this.TagErrorText = new TextObject("{=jyJXcOLe}Tag Should Not Contain Offensive Words", null).ToString();
				return;
			}
			if (error == StringValidationError.Unspecified)
			{
				this.TagErrorText = new TextObject("{=hCNnqVgK}Tag Has Invalid Content", null).ToString();
			}
		}

		// Token: 0x06000A24 RID: 2596 RVA: 0x0001FAAD File Offset: 0x0001DCAD
		public void UpdateFactionErrorText()
		{
			this.FactionErrorText = "";
			this.FactionErrorText = new TextObject("{=p83IO9ls}You must select a culture", null).ToString();
		}

		// Token: 0x06000A25 RID: 2597 RVA: 0x0001FAD0 File Offset: 0x0001DCD0
		public void UpdateSigilIconErrorText()
		{
			this.SigilIconErrorText = "";
			this.SigilIconErrorText = new TextObject("{=uOrwqeQl}You must select a sigil icon", null).ToString();
		}

		// Token: 0x06000A26 RID: 2598 RVA: 0x0001FAF4 File Offset: 0x0001DCF4
		public void UpdateConfirmation(PlayerId playerId, ClanCreationAnswer answer)
		{
			foreach (MPLobbyClanMemberItemVM mplobbyClanMemberItemVM in this.PartyMembersList)
			{
				if (mplobbyClanMemberItemVM.ProvidedID == playerId)
				{
					if (answer == ClanCreationAnswer.Accepted)
					{
						mplobbyClanMemberItemVM.InviteAcceptInfo = new TextObject("{=JTMegIk4}Accepted", null).ToString();
					}
					else if (answer == ClanCreationAnswer.Declined)
					{
						mplobbyClanMemberItemVM.InviteAcceptInfo = new TextObject("{=FgaORzy5}Declined", null).ToString();
					}
				}
			}
		}

		// Token: 0x06000A27 RID: 2599 RVA: 0x0001FB80 File Offset: 0x0001DD80
		private BasicCultureObject GetSelectedCulture()
		{
			return Game.Current.ObjectManager.GetObject<BasicCultureObject>(this._selectedFaction.CultureCode);
		}

		// Token: 0x06000A28 RID: 2600 RVA: 0x0001FB9C File Offset: 0x0001DD9C
		private Banner GetCreatedClanSigil()
		{
			BasicCultureObject selectedCulture = this.GetSelectedCulture();
			Banner banner = new Banner(selectedCulture.Banner, selectedCulture.BackgroundColor1, selectedCulture.ForegroundColor1);
			banner.SetIconMeshId(this._selectedSigilIcon.IconID);
			return banner;
		}

		// Token: 0x06000A29 RID: 2601 RVA: 0x0001FBD8 File Offset: 0x0001DDD8
		private async void ExecuteTryCreateClan()
		{
			bool areAllInputsValid = true;
			this.ResetErrorTexts();
			CheckClanParameterValidResult checkClanParameterValidResult = await NetworkMain.GameClient.ClanNameExists(this.NameInputText);
			if (!checkClanParameterValidResult.IsValid)
			{
				areAllInputsValid = false;
				this.UpdateNameErrorText(checkClanParameterValidResult.Error);
			}
			TaskAwaiter<bool> taskAwaiter = PlatformServices.Instance.VerifyString(this.NameInputText).GetAwaiter();
			TaskAwaiter<bool> taskAwaiter2;
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (!taskAwaiter.GetResult())
			{
				areAllInputsValid = false;
				this.UpdateNameErrorText(StringValidationError.Unspecified);
			}
			CheckClanParameterValidResult checkClanParameterValidResult2 = await NetworkMain.GameClient.ClanTagExists(this.TagInputText);
			if (!checkClanParameterValidResult2.IsValid)
			{
				areAllInputsValid = false;
				this.UpdateTagErrorText(checkClanParameterValidResult2.Error);
			}
			taskAwaiter = PlatformServices.Instance.VerifyString(this.TagInputText).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (!taskAwaiter.GetResult())
			{
				areAllInputsValid = false;
				this.UpdateTagErrorText(StringValidationError.Unspecified);
			}
			if (this._selectedFaction == null)
			{
				areAllInputsValid = false;
				this.UpdateFactionErrorText();
			}
			if (this._selectedSigilIcon == null)
			{
				areAllInputsValid = false;
				this.UpdateSigilIconErrorText();
			}
			if (areAllInputsValid)
			{
				this.HasCreationStarted = true;
				NetworkMain.GameClient.SendCreateClanMessage(this.NameInputText, this.TagInputText, this.GetSelectedCulture().StringId, this.GetCreatedClanSigil().Serialize());
			}
		}

		// Token: 0x06000A2A RID: 2602 RVA: 0x0001FC11 File Offset: 0x0001DE11
		public void ExecuteSwitchToWaiting()
		{
			this.PreparePartyMembersList();
			this.IsWaiting = true;
		}

		// Token: 0x06000A2B RID: 2603 RVA: 0x0001FC20 File Offset: 0x0001DE20
		public override void OnFinalize()
		{
			base.OnFinalize();
			InputKeyItemVM cancelInputKey = this.CancelInputKey;
			if (cancelInputKey == null)
			{
				return;
			}
			cancelInputKey.OnFinalize();
		}

		// Token: 0x06000A2C RID: 2604 RVA: 0x0001FC38 File Offset: 0x0001DE38
		public void SetCancelInputKey(HotKey hotKey)
		{
			this.CancelInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x06000A2D RID: 2605 RVA: 0x0001FC47 File Offset: 0x0001DE47
		// (set) Token: 0x06000A2E RID: 2606 RVA: 0x0001FC4F File Offset: 0x0001DE4F
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
					base.OnPropertyChanged("CancelInputKey");
				}
			}
		}

		// Token: 0x17000352 RID: 850
		// (get) Token: 0x06000A2F RID: 2607 RVA: 0x0001FC6C File Offset: 0x0001DE6C
		// (set) Token: 0x06000A30 RID: 2608 RVA: 0x0001FC74 File Offset: 0x0001DE74
		[DataSourceProperty]
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
			set
			{
				if (value != this._isEnabled)
				{
					this._isEnabled = value;
					base.OnPropertyChanged("IsEnabled");
				}
			}
		}

		// Token: 0x17000353 RID: 851
		// (get) Token: 0x06000A31 RID: 2609 RVA: 0x0001FC91 File Offset: 0x0001DE91
		// (set) Token: 0x06000A32 RID: 2610 RVA: 0x0001FC99 File Offset: 0x0001DE99
		[DataSourceProperty]
		public bool HasCreationStarted
		{
			get
			{
				return this._hasCreationStarted;
			}
			set
			{
				if (value != this._hasCreationStarted)
				{
					this._hasCreationStarted = value;
					base.OnPropertyChanged("HasCreationStarted");
				}
			}
		}

		// Token: 0x17000354 RID: 852
		// (get) Token: 0x06000A33 RID: 2611 RVA: 0x0001FCB6 File Offset: 0x0001DEB6
		// (set) Token: 0x06000A34 RID: 2612 RVA: 0x0001FCBE File Offset: 0x0001DEBE
		[DataSourceProperty]
		public bool IsWaiting
		{
			get
			{
				return this._isWaiting;
			}
			set
			{
				if (value != this._isWaiting)
				{
					this._isWaiting = value;
					base.OnPropertyChanged("IsWaiting");
				}
			}
		}

		// Token: 0x17000355 RID: 853
		// (get) Token: 0x06000A35 RID: 2613 RVA: 0x0001FCDB File Offset: 0x0001DEDB
		// (set) Token: 0x06000A36 RID: 2614 RVA: 0x0001FCE3 File Offset: 0x0001DEE3
		[DataSourceProperty]
		public string CreateClanText
		{
			get
			{
				return this._createClanText;
			}
			set
			{
				if (value != this._createClanText)
				{
					this._createClanText = value;
					base.OnPropertyChanged("CreateClanText");
				}
			}
		}

		// Token: 0x17000356 RID: 854
		// (get) Token: 0x06000A37 RID: 2615 RVA: 0x0001FD05 File Offset: 0x0001DF05
		// (set) Token: 0x06000A38 RID: 2616 RVA: 0x0001FD0D File Offset: 0x0001DF0D
		[DataSourceProperty]
		public string NameText
		{
			get
			{
				return this._nameText;
			}
			set
			{
				if (value != this._nameText)
				{
					this._nameText = value;
					base.OnPropertyChanged("NameText");
				}
			}
		}

		// Token: 0x17000357 RID: 855
		// (get) Token: 0x06000A39 RID: 2617 RVA: 0x0001FD2F File Offset: 0x0001DF2F
		// (set) Token: 0x06000A3A RID: 2618 RVA: 0x0001FD37 File Offset: 0x0001DF37
		[DataSourceProperty]
		public string NameErrorText
		{
			get
			{
				return this._nameErrorText;
			}
			set
			{
				if (value != this._nameErrorText)
				{
					this._nameErrorText = value;
					base.OnPropertyChanged("NameErrorText");
				}
			}
		}

		// Token: 0x17000358 RID: 856
		// (get) Token: 0x06000A3B RID: 2619 RVA: 0x0001FD59 File Offset: 0x0001DF59
		// (set) Token: 0x06000A3C RID: 2620 RVA: 0x0001FD61 File Offset: 0x0001DF61
		[DataSourceProperty]
		public string TagText
		{
			get
			{
				return this._tagText;
			}
			set
			{
				if (value != this._tagText)
				{
					this._tagText = value;
					base.OnPropertyChanged("TagText");
				}
			}
		}

		// Token: 0x17000359 RID: 857
		// (get) Token: 0x06000A3D RID: 2621 RVA: 0x0001FD83 File Offset: 0x0001DF83
		// (set) Token: 0x06000A3E RID: 2622 RVA: 0x0001FD8B File Offset: 0x0001DF8B
		[DataSourceProperty]
		public string TagErrorText
		{
			get
			{
				return this._tagErrorText;
			}
			set
			{
				if (value != this._tagErrorText)
				{
					this._tagErrorText = value;
					base.OnPropertyChanged("TagErrorText");
				}
			}
		}

		// Token: 0x1700035A RID: 858
		// (get) Token: 0x06000A3F RID: 2623 RVA: 0x0001FDAD File Offset: 0x0001DFAD
		// (set) Token: 0x06000A40 RID: 2624 RVA: 0x0001FDB5 File Offset: 0x0001DFB5
		[DataSourceProperty]
		public string FactionText
		{
			get
			{
				return this._factionText;
			}
			set
			{
				if (value != this._factionText)
				{
					this._factionText = value;
					base.OnPropertyChanged("FactionText");
				}
			}
		}

		// Token: 0x1700035B RID: 859
		// (get) Token: 0x06000A41 RID: 2625 RVA: 0x0001FDD7 File Offset: 0x0001DFD7
		// (set) Token: 0x06000A42 RID: 2626 RVA: 0x0001FDDF File Offset: 0x0001DFDF
		[DataSourceProperty]
		public string FactionErrorText
		{
			get
			{
				return this._factionErrorText;
			}
			set
			{
				if (value != this._factionErrorText)
				{
					this._factionErrorText = value;
					base.OnPropertyChanged("FactionErrorText");
				}
			}
		}

		// Token: 0x1700035C RID: 860
		// (get) Token: 0x06000A43 RID: 2627 RVA: 0x0001FE01 File Offset: 0x0001E001
		// (set) Token: 0x06000A44 RID: 2628 RVA: 0x0001FE09 File Offset: 0x0001E009
		[DataSourceProperty]
		public string SigilText
		{
			get
			{
				return this._sigilText;
			}
			set
			{
				if (value != this._sigilText)
				{
					this._sigilText = value;
					base.OnPropertyChanged("SigilText");
				}
			}
		}

		// Token: 0x1700035D RID: 861
		// (get) Token: 0x06000A45 RID: 2629 RVA: 0x0001FE2B File Offset: 0x0001E02B
		// (set) Token: 0x06000A46 RID: 2630 RVA: 0x0001FE33 File Offset: 0x0001E033
		[DataSourceProperty]
		public string SigilIconErrorText
		{
			get
			{
				return this._sigilIconErrorText;
			}
			set
			{
				if (value != this._sigilIconErrorText)
				{
					this._sigilIconErrorText = value;
					base.OnPropertyChanged("SigilIconErrorText");
				}
			}
		}

		// Token: 0x1700035E RID: 862
		// (get) Token: 0x06000A47 RID: 2631 RVA: 0x0001FE55 File Offset: 0x0001E055
		// (set) Token: 0x06000A48 RID: 2632 RVA: 0x0001FE5D File Offset: 0x0001E05D
		[DataSourceProperty]
		public string CreateText
		{
			get
			{
				return this._createText;
			}
			set
			{
				if (value != this._createText)
				{
					this._createText = value;
					base.OnPropertyChanged("CreateText");
				}
			}
		}

		// Token: 0x1700035F RID: 863
		// (get) Token: 0x06000A49 RID: 2633 RVA: 0x0001FE7F File Offset: 0x0001E07F
		// (set) Token: 0x06000A4A RID: 2634 RVA: 0x0001FE87 File Offset: 0x0001E087
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
					base.OnPropertyChanged("CancelText");
				}
			}
		}

		// Token: 0x17000360 RID: 864
		// (get) Token: 0x06000A4B RID: 2635 RVA: 0x0001FEA9 File Offset: 0x0001E0A9
		// (set) Token: 0x06000A4C RID: 2636 RVA: 0x0001FEB1 File Offset: 0x0001E0B1
		[DataSourceProperty]
		public string NameInputText
		{
			get
			{
				return this._nameInputText;
			}
			set
			{
				if (value != this._nameInputText)
				{
					this._nameInputText = value;
					base.OnPropertyChanged("NameInputText");
					this.NameErrorText = "";
				}
			}
		}

		// Token: 0x17000361 RID: 865
		// (get) Token: 0x06000A4D RID: 2637 RVA: 0x0001FEDE File Offset: 0x0001E0DE
		// (set) Token: 0x06000A4E RID: 2638 RVA: 0x0001FEE6 File Offset: 0x0001E0E6
		[DataSourceProperty]
		public string TagInputText
		{
			get
			{
				return this._tagInputText;
			}
			set
			{
				if (value != this._tagInputText)
				{
					this._tagInputText = value;
					base.OnPropertyChanged("TagInputText");
					this.TagErrorText = "";
				}
			}
		}

		// Token: 0x17000362 RID: 866
		// (get) Token: 0x06000A4F RID: 2639 RVA: 0x0001FF13 File Offset: 0x0001E113
		// (set) Token: 0x06000A50 RID: 2640 RVA: 0x0001FF1B File Offset: 0x0001E11B
		[DataSourceProperty]
		public string WaitingForConfirmationText
		{
			get
			{
				return this._waitingForConfirmationText;
			}
			set
			{
				if (value != this._waitingForConfirmationText)
				{
					this._waitingForConfirmationText = value;
					base.OnPropertyChanged("WaitingForConfirmationText");
				}
			}
		}

		// Token: 0x17000363 RID: 867
		// (get) Token: 0x06000A51 RID: 2641 RVA: 0x0001FF3D File Offset: 0x0001E13D
		// (set) Token: 0x06000A52 RID: 2642 RVA: 0x0001FF45 File Offset: 0x0001E145
		[DataSourceProperty]
		public MBBindingList<MPCultureItemVM> FactionsList
		{
			get
			{
				return this._factionsList;
			}
			set
			{
				if (value != this._factionsList)
				{
					this._factionsList = value;
					base.OnPropertyChanged("FactionsList");
				}
			}
		}

		// Token: 0x17000364 RID: 868
		// (get) Token: 0x06000A53 RID: 2643 RVA: 0x0001FF62 File Offset: 0x0001E162
		// (set) Token: 0x06000A54 RID: 2644 RVA: 0x0001FF6A File Offset: 0x0001E16A
		[DataSourceProperty]
		public MBBindingList<MPLobbySigilItemVM> IconsList
		{
			get
			{
				return this._iconsList;
			}
			set
			{
				if (value != this._iconsList)
				{
					this._iconsList = value;
					base.OnPropertyChanged("IconsList");
				}
			}
		}

		// Token: 0x17000365 RID: 869
		// (get) Token: 0x06000A55 RID: 2645 RVA: 0x0001FF87 File Offset: 0x0001E187
		// (set) Token: 0x06000A56 RID: 2646 RVA: 0x0001FF8F File Offset: 0x0001E18F
		[DataSourceProperty]
		public MBBindingList<MPLobbyClanMemberItemVM> PartyMembersList
		{
			get
			{
				return this._partyMembersList;
			}
			set
			{
				if (value != this._partyMembersList)
				{
					this._partyMembersList = value;
					base.OnPropertyChanged("PartyMembersList");
				}
			}
		}

		// Token: 0x040004A0 RID: 1184
		private MPCultureItemVM _selectedFaction;

		// Token: 0x040004A1 RID: 1185
		private MPLobbySigilItemVM _selectedSigilIcon;

		// Token: 0x040004A2 RID: 1186
		private InputKeyItemVM _cancelInputKey;

		// Token: 0x040004A3 RID: 1187
		private bool _isEnabled;

		// Token: 0x040004A4 RID: 1188
		private bool _hasCreationStarted;

		// Token: 0x040004A5 RID: 1189
		private bool _isWaiting;

		// Token: 0x040004A6 RID: 1190
		private string _createClanText;

		// Token: 0x040004A7 RID: 1191
		private string _nameText;

		// Token: 0x040004A8 RID: 1192
		private string _nameErrorText;

		// Token: 0x040004A9 RID: 1193
		private string _tagText;

		// Token: 0x040004AA RID: 1194
		private string _tagErrorText;

		// Token: 0x040004AB RID: 1195
		private string _factionText;

		// Token: 0x040004AC RID: 1196
		private string _factionErrorText;

		// Token: 0x040004AD RID: 1197
		private string _sigilText;

		// Token: 0x040004AE RID: 1198
		private string _sigilIconErrorText;

		// Token: 0x040004AF RID: 1199
		private string _createText;

		// Token: 0x040004B0 RID: 1200
		private string _cancelText;

		// Token: 0x040004B1 RID: 1201
		private string _nameInputText;

		// Token: 0x040004B2 RID: 1202
		private string _tagInputText;

		// Token: 0x040004B3 RID: 1203
		private string _waitingForConfirmationText;

		// Token: 0x040004B4 RID: 1204
		private MBBindingList<MPCultureItemVM> _factionsList;

		// Token: 0x040004B5 RID: 1205
		private MBBindingList<MPLobbySigilItemVM> _iconsList;

		// Token: 0x040004B6 RID: 1206
		private MBBindingList<MPLobbyClanMemberItemVM> _partyMembersList;
	}
}
