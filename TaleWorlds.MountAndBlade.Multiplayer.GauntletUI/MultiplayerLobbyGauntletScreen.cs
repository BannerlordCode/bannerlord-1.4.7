using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Engine.Options;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.Diamond.Ranked;
using TaleWorlds.MountAndBlade.GauntletUI;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Armory.CosmeticItem;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.CustomGame;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.Screens;
using TaleWorlds.MountAndBlade.View.Tableaus.Thumbnails;
using TaleWorlds.MountAndBlade.ViewModelCollection.GameOptions;
using TaleWorlds.MountAndBlade.ViewModelCollection.GameOptions.AuxiliaryKeys;
using TaleWorlds.MountAndBlade.ViewModelCollection.GameOptions.GameKeys;
using TaleWorlds.PlayerServices;
using TaleWorlds.ScreenSystem;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.Multiplayer.GauntletUI
{
	// Token: 0x02000007 RID: 7
	[GameStateScreen(typeof(LobbyState))]
	public class MultiplayerLobbyGauntletScreen : ScreenBase, IGameStateListener, ILobbyStateHandler, IChatLogHandlerScreen
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000021 RID: 33 RVA: 0x000023F0 File Offset: 0x000005F0
		public MPLobbyVM.LobbyPage CurrentPage
		{
			get
			{
				if (this._lobbyDataSource != null)
				{
					return this._lobbyDataSource.CurrentPage;
				}
				return MPLobbyVM.LobbyPage.NotAssigned;
			}
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002408 File Offset: 0x00000608
		public MultiplayerLobbyGauntletScreen(LobbyState lobbyState)
		{
			AvatarThumbnailCache avatarThumbnailCache = AvatarThumbnailCache.Current;
			if (avatarThumbnailCache != null)
			{
				avatarThumbnailCache.FlushCache();
			}
			this._feedbackInquiries = new List<KeyValuePair<string, InquiryData>>();
			this._lobbyState = lobbyState;
			this._lobbyState.Handler = this;
			GauntletFullScreenNoticeView.Initialize();
			MultiplayerGauntletGameNotification.Initialize();
			GauntletChatLogView gauntletChatLogView = GauntletChatLogView.Current;
			if (gauntletChatLogView != null)
			{
				gauntletChatLogView.LoadMovie(true);
			}
			GauntletChatLogView gauntletChatLogView2 = GauntletChatLogView.Current;
			if (gauntletChatLogView2 != null)
			{
				gauntletChatLogView2.SetEnabled(false);
			}
			MultiplayerAdminInformationScreen.OnInitialize();
			MultiplayerReportPlayerScreen.OnInitialize();
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002480 File Offset: 0x00000680
		protected override void OnInitialize()
		{
			base.OnInitialize();
			LoadingWindow.DisableGlobalLoadingWindow();
			this._keybindingPopup = new KeybindingPopup(new Action<Key>(this.SetHotKey), this);
			MPLobbyVM lobbyDataSource = this._lobbyDataSource;
			if (lobbyDataSource != null)
			{
				lobbyDataSource.RefreshPlayerData(this._lobbyState.LobbyClient.PlayerData);
			}
			ManagedOptions.OnManagedOptionChanged = (ManagedOptions.OnManagedOptionChangedDelegate)Delegate.Combine(ManagedOptions.OnManagedOptionChanged, new ManagedOptions.OnManagedOptionChangedDelegate(this.OnManagedOptionChanged));
			InformationManager.HideAllMessages();
		}

		// Token: 0x06000024 RID: 36 RVA: 0x000024F6 File Offset: 0x000006F6
		private void OnManagedOptionChanged(ManagedOptions.ManagedOptionsType changedManagedOptionsType)
		{
			if (changedManagedOptionsType == ManagedOptions.ManagedOptionsType.EnableGenericAvatars)
			{
				MPLobbyVM lobbyDataSource = this._lobbyDataSource;
				if (lobbyDataSource != null)
				{
					lobbyDataSource.OnEnableGenericAvatarsChanged();
				}
			}
			if (changedManagedOptionsType == ManagedOptions.ManagedOptionsType.EnableGenericNames)
			{
				MPLobbyVM lobbyDataSource2 = this._lobbyDataSource;
				if (lobbyDataSource2 == null)
				{
					return;
				}
				lobbyDataSource2.OnEnableGenericNamesChanged();
			}
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002524 File Offset: 0x00000724
		void IChatLogHandlerScreen.TryUpdateChatLogLayerParameters(ref bool isTeamChatAvailable, ref bool inputEnabled, ref bool isToggleChatHintAvailable, ref bool isMouseVisible, ref InputContext inputContext)
		{
			if (this.LobbyLayer != null)
			{
				MPLobbyVM lobbyDataSource = this._lobbyDataSource;
				bool flag;
				if (lobbyDataSource == null)
				{
					flag = false;
				}
				else
				{
					MPOptionsVM options = lobbyDataSource.Options;
					bool? flag2 = ((options != null) ? new bool?(options.IsEnabled) : null);
					bool flag3 = true;
					flag = (flag2.GetValueOrDefault() == flag3) & (flag2 != null);
				}
				bool flag4;
				if (!flag)
				{
					MPLobbyVM lobbyDataSource2 = this._lobbyDataSource;
					flag4 = lobbyDataSource2 != null && !lobbyDataSource2.HasNoPopupOpen();
				}
				else
				{
					flag4 = true;
				}
				bool flag5 = flag4;
				inputEnabled = !flag5 && !this._wasChatRestricted;
				inputContext = this.LobbyLayer.Input;
				this._wasChatRestricted = flag5;
			}
		}

		// Token: 0x06000026 RID: 38 RVA: 0x000025C0 File Offset: 0x000007C0
		protected override void OnFinalize()
		{
			if (!TWParallel.IsMainThread())
			{
				Debug.FailedAssert("Lobby screen finalized from non-main thread", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.GauntletUI\\MultiplayerLobbyGauntletScreen.cs", "OnFinalize", 133);
			}
			AvatarThumbnailCache avatarThumbnailCache = AvatarThumbnailCache.Current;
			if (avatarThumbnailCache != null)
			{
				avatarThumbnailCache.FlushCache();
			}
			if (this._lobbyDataSource != null)
			{
				this._lobbyDataSource.OnFinalize();
				this._lobbyDataSource = null;
			}
			SpriteCategory mplobbyCategory = this._mplobbyCategory;
			if (mplobbyCategory != null)
			{
				mplobbyCategory.Unload();
			}
			this._optionsSpriteCategory.Unload();
			this._multiplayerSpriteCategory.Unload();
			SpriteCategory badgesCategory = this._badgesCategory;
			if (badgesCategory != null)
			{
				badgesCategory.Unload();
			}
			GauntletGameNotification.Initialize();
			MultiplayerReportPlayerScreen.OnFinalize();
			MultiplayerAdminInformationScreen.OnRemove();
			ManagedOptions.OnManagedOptionChanged = (ManagedOptions.OnManagedOptionChangedDelegate)Delegate.Remove(ManagedOptions.OnManagedOptionChanged, new ManagedOptions.OnManagedOptionChangedDelegate(this.OnManagedOptionChanged));
			this._lobbyState.Handler = null;
			this._lobbyState = null;
			base.OnFinalize();
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002698 File Offset: 0x00000898
		protected override void OnActivate()
		{
			if (this._lobbyDataSource != null && this._isFacegenOpen)
			{
				this.OnFacegenClosed(true);
			}
			MPLobbyVM lobbyDataSource = this._lobbyDataSource;
			if (lobbyDataSource != null)
			{
				lobbyDataSource.OnActivate();
			}
			MPLobbyVM lobbyDataSource2 = this._lobbyDataSource;
			if (lobbyDataSource2 != null)
			{
				lobbyDataSource2.RefreshPlayerData(this._lobbyState.LobbyClient.PlayerData);
			}
			MPLobbyVM lobbyDataSource3 = this._lobbyDataSource;
			if (lobbyDataSource3 != null)
			{
				lobbyDataSource3.RefreshRecentGames();
			}
			LoadingWindow.DisableGlobalLoadingWindow();
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002704 File Offset: 0x00000904
		private void CreateView()
		{
			this._musicSoundEvent = SoundEvent.CreateEventFromString("event:/multiplayer/lobby_music", null);
			this._musicSoundEvent.Play();
			if (!(GameStateManager.Current.ActiveState is MissionState))
			{
				LoadingWindow.DisableGlobalLoadingWindow();
			}
			this._mplobbyCategory = UIResourceManager.LoadSpriteCategory("ui_mplobby");
			this._bannerIconsCategory = UIResourceManager.LoadSpriteCategory("ui_bannericons");
			this._badgesCategory = UIResourceManager.LoadSpriteCategory("ui_mpbadges");
			this._lobbyDataSource = new MPLobbyVM(this._lobbyState, new Action<BasicCharacterObject>(this.OnOpenFacegen), new Action(this.OnForceCloseFacegen), new Action(this.OnLogout), new Action<KeyOptionVM>(this.OnKeybindRequest), new Func<string>(this.GetContinueKeyText), new Action<bool>(this.SetNavigationRestriction));
			GameKeyContext category = HotKeyManager.GetCategory("GenericPanelGameKeyCategory");
			this._lobbyDataSource.CreateInputKeyVisuals(category.GetHotKey("Exit"), category.GetHotKey("Confirm"), category.GetHotKey("SwitchToPreviousTab"), category.GetHotKey("SwitchToNextTab"), category.GetHotKey("TakeAll"), category.GetHotKey("GiveAll"));
			this._lobbyLayer = new GauntletLayer("LobbyScreen", 10, true);
			this._lobbyLayer.LoadMovie("Lobby", this._lobbyDataSource);
			this._lobbyLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
			this._lobbyLayer.IsFocusLayer = true;
			base.AddLayer(this._lobbyLayer);
			ScreenManager.TrySetFocus(this._lobbyLayer);
			this._lobbyLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericPanelGameKeyCategory"));
			this._lobbyLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("MultiplayerHotkeyCategory"));
			GameKeyContext category2 = HotKeyManager.GetCategory("MultiplayerHotkeyCategory");
			GameKeyContext genericPanelCategory = HotKeyManager.GetCategory("GenericPanelGameKeyCategory");
			this._lobbyDataSource.BadgeSelectionPopup.RefreshKeyBindings(category2.GetHotKey("InspectBadgeProgression"));
			this._lobbyDataSource.BadgeProgressionInformation.SetPreviousTabInputKey(genericPanelCategory.GetHotKey("SwitchToPreviousTab"));
			this._lobbyDataSource.BadgeProgressionInformation.SetNextTabInputKey(genericPanelCategory.GetHotKey("SwitchToNextTab"));
			this._lobbyDataSource.Armory.Cosmetics.RefreshKeyBindings(category2.GetHotKey("PerformActionOnCosmeticItem"), category2.GetHotKey("PreviewCosmeticItem"));
			this._lobbyDataSource.Armory.Cosmetics.TauntSlots.ApplyActionOnAllItems(delegate(MPArmoryCosmeticTauntSlotVM t)
			{
				t.SetSelectKeyVisual(genericPanelCategory.GetHotKey("GiveAll"));
			});
			this._lobbyDataSource.Armory.Cosmetics.TauntSlots.ApplyActionOnAllItems(delegate(MPArmoryCosmeticTauntSlotVM t)
			{
				t.SetEmptySlotKeyVisual(genericPanelCategory.GetHotKey("TakeAll"));
			});
			this._lobbyDataSource.Friends.SetToggleFriendListKey(category2.RegisteredHotKeys.FirstOrDefault<HotKey>((HotKey g) => ((g != null) ? g.Id : null) == "ToggleFriendsList"));
			this._lobbyDataSource.Matchmaking.CustomServer.SortController.InitializeWithSortState(this._cachedCustomServerSortOption, this._cachedCustomServerSortState);
			this._lobbyDataSource.Matchmaking.PremadeMatches.SortController.InitializeWithSortState(this._cachedPremadeGameSortOption, this._cachedPremadeGameSortState);
			this._lobbyDataSource.Options.SetDoneInputKey(genericPanelCategory.GetHotKey("Confirm"));
			this._lobbyDataSource.Options.SetCancelInputKey(genericPanelCategory.GetHotKey("Exit"));
			this._lobbyDataSource.Options.SetResetInputKey(genericPanelCategory.GetHotKey("Reset"));
			this._lobbyDataSource.Options.SetPreviousTabInputKey(genericPanelCategory.GetHotKey("TakeAll"));
			this._lobbyDataSource.Options.SetNextTabInputKey(genericPanelCategory.GetHotKey("GiveAll"));
			this._lobbyDataSource.Matchmaking.CustomServer.SetRefreshInputKey(genericPanelCategory.GetHotKey("Reset"));
			if (NativeOptions.GetConfig(NativeOptions.NativeOptionsType.BrightnessCalibrated) < 2f)
			{
				this._brightnessOptionDataSource = new BrightnessOptionVM(new Action<bool>(this.OnCloseBrightness))
				{
					Visible = true
				};
				this._gauntletBrightnessLayer = new GauntletLayer("MultiplayerBrightness", 11, false);
				this._gauntletBrightnessLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.Mouse);
				this._brightnessOptionMovie = this._gauntletBrightnessLayer.LoadMovie("BrightnessOption", this._brightnessOptionDataSource);
				base.AddLayer(this._gauntletBrightnessLayer);
			}
			this._optionsSpriteCategory = UIResourceManager.LoadSpriteCategory("ui_options");
			this._multiplayerSpriteCategory = UIResourceManager.LoadSpriteCategory("ui_mpmission");
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002B8F File Offset: 0x00000D8F
		protected override void OnDeactivate()
		{
			base.OnDeactivate();
			MPLobbyVM lobbyDataSource = this._lobbyDataSource;
			if (lobbyDataSource == null)
			{
				return;
			}
			lobbyDataSource.OnDeactivate();
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002BA7 File Offset: 0x00000DA7
		private void OnCloseBrightness(bool isConfirm)
		{
			this._gauntletBrightnessLayer.ReleaseMovie(this._brightnessOptionMovie);
			base.RemoveLayer(this._gauntletBrightnessLayer);
			this._brightnessOptionDataSource = null;
			this._gauntletBrightnessLayer = null;
			NativeOptions.SaveConfig();
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002BDA File Offset: 0x00000DDA
		private void OnOpenFacegen(BasicCharacterObject character)
		{
			this._isFacegenOpen = true;
			this._playerCharacter = character;
			LoadingWindow.EnableGlobalLoadingWindow();
			ScreenManager.PushScreen(ViewCreator.CreateMBFaceGeneratorScreen(character, true, null));
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002BFC File Offset: 0x00000DFC
		private void OnForceCloseFacegen()
		{
			if (this._isFacegenOpen)
			{
				this.OnFacegenClosed(false);
				ScreenManager.PopScreen();
			}
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002C14 File Offset: 0x00000E14
		private void OnFacegenClosed(bool updateCharacter)
		{
			if (updateCharacter)
			{
				NetworkMain.GameClient.UpdateCharacter(this._playerCharacter.GetBodyPropertiesMin(false), this._playerCharacter.IsFemale);
			}
			ScreenManager.TrySetFocus(this._lobbyLayer);
			this._lobbyDataSource.RefreshPlayerData(this._lobbyState.LobbyClient.PlayerData);
			this._isFacegenOpen = false;
			this._playerCharacter = null;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002C7C File Offset: 0x00000E7C
		private string GetContinueKeyText()
		{
			if (Input.IsGamepadActive)
			{
				GameTexts.SetVariable("CONSOLE_KEY_NAME", Game.Current.GameTextManager.GetHotKeyGameText("GenericPanelGameKeyCategory", "Exit"));
				return GameTexts.FindText("str_click_to_exit_console", null).ToString();
			}
			return GameTexts.FindText("str_click_to_exit", null).ToString();
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002CD4 File Offset: 0x00000ED4
		private void OnLogout()
		{
			GauntletChatLogView gauntletChatLogView = GauntletChatLogView.Current;
			if (gauntletChatLogView == null)
			{
				return;
			}
			gauntletChatLogView.SetEnabled(false);
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002CE6 File Offset: 0x00000EE6
		private void SetNavigationRestriction(bool isRestricted)
		{
			if (this._isNavigationRestricted != isRestricted)
			{
				this._isNavigationRestricted = isRestricted;
			}
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002CF8 File Offset: 0x00000EF8
		protected override void OnFrameTick(float dt)
		{
			base.OnFrameTick(dt);
			this.TickInternal(dt);
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002D08 File Offset: 0x00000F08
		private void TickInternal(float dt)
		{
			MPLobbyVM lobbyDataSource = this._lobbyDataSource;
			if (lobbyDataSource != null)
			{
				lobbyDataSource.OnTick(dt);
			}
			if (this._activeFeedbackId == null && this._feedbackInquiries.Count > 0)
			{
				this.ShowNextFeedback();
			}
			if (this._lobbyLayer == null)
			{
				return;
			}
			MPLobbyVM lobbyDataSource2 = this._lobbyDataSource;
			bool flag;
			if (lobbyDataSource2 == null)
			{
				flag = false;
			}
			else
			{
				MPOptionsVM options = lobbyDataSource2.Options;
				bool? flag2 = ((options != null) ? new bool?(options.IsEnabled) : null);
				bool flag3 = true;
				flag = (flag2.GetValueOrDefault() == flag3) & (flag2 != null);
			}
			if (flag)
			{
				MPOptionsVM options2 = this._lobbyDataSource.Options;
				KeybindingPopup keybindingPopup = this._keybindingPopup;
				options2.AreHotkeysEnabled = (keybindingPopup == null || !keybindingPopup.IsActive) && !this._lobbyLayer.IsFocusedOnInput() && !InformationManager.IsAnyInquiryActive() && this._lobbyDataSource.HasNoPopupOpen();
			}
			KeybindingPopup keybindingPopup2 = this._keybindingPopup;
			if (keybindingPopup2 != null && keybindingPopup2.IsActive)
			{
				this._keybindingPopup.Tick();
				return;
			}
			if (this._lobbyDataSource != null && !this._lobbyState.IsLoggingIn && !this._lobbyDataSource.BlockerState.IsEnabled && !this._lobbyLayer.IsFocusedOnInput())
			{
				this.HandleInput(dt);
			}
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002E34 File Offset: 0x00001034
		private void HandleInput(float dt)
		{
			bool flag = this._lobbyLayer.Input.IsHotKeyReleased("Confirm");
			bool flag2 = this._lobbyLayer.Input.IsHotKeyReleased("ToggleFriendsList");
			if (flag || flag2)
			{
				if (this._lobbyDataSource.Login.IsEnabled && flag)
				{
					this._lobbyDataSource.Login.ExecuteLogin();
					UISoundsHelper.PlayUISound("event:/ui/default");
					return;
				}
				if (this._lobbyDataSource.Options.IsEnabled && this._lobbyDataSource.Options.AreHotkeysEnabled && flag)
				{
					this._lobbyDataSource.Options.ExecuteApply();
					UISoundsHelper.PlayUISound("event:/ui/default");
					return;
				}
				if (!this._lobbyDataSource.HasNoPopupOpen() && flag)
				{
					this._lobbyDataSource.OnConfirm();
					return;
				}
				if (flag2)
				{
					UISoundsHelper.PlayUISound("event:/ui/default");
					this._lobbyDataSource.Friends.IsListEnabled = !this._lobbyDataSource.Friends.IsListEnabled;
					this._lobbyDataSource.ForceCloseContextMenus();
					return;
				}
			}
			else
			{
				if (this._lobbyLayer.Input.IsHotKeyReleased("Exit"))
				{
					this._lobbyDataSource.OnEscape();
					return;
				}
				if (this._lobbyLayer.Input.IsHotKeyPressed("TakeAll"))
				{
					if (this._lobbyDataSource.RankLeaderboard.IsEnabled)
					{
						if (this._lobbyDataSource.RankLeaderboard.IsPreviousPageAvailable)
						{
							UISoundsHelper.PlayUISound("event:/ui/checkbox");
						}
						this._lobbyDataSource.RankLeaderboard.ExecuteLoadFirstPage();
						return;
					}
					if (this._lobbyDataSource.Armory.IsEnabled)
					{
						if (Input.IsGamepadActive && this._lobbyDataSource.Armory.IsManagingTaunts)
						{
							this._lobbyDataSource.Armory.ExecuteEmptyFocusedSlot();
							return;
						}
					}
					else if (this._lobbyDataSource.Options.IsEnabled && this._lobbyDataSource.Options.AreHotkeysEnabled)
					{
						UISoundsHelper.PlayUISound("event:/ui/default");
						this._lobbyDataSource.Options.SelectPreviousCategory();
						return;
					}
				}
				else if (this._lobbyLayer.Input.IsHotKeyPressed("GiveAll"))
				{
					if (this._lobbyDataSource.RankLeaderboard.IsEnabled)
					{
						if (this._lobbyDataSource.RankLeaderboard.IsNextPageAvailable)
						{
							UISoundsHelper.PlayUISound("event:/ui/checkbox");
						}
						this._lobbyDataSource.RankLeaderboard.ExecuteLoadLastPage();
						return;
					}
					if (this._lobbyDataSource.Armory.IsEnabled)
					{
						if (Input.IsGamepadActive && this._lobbyDataSource.Armory.IsManagingTaunts)
						{
							this._lobbyDataSource.Armory.ExecuteSelectFocusedSlot();
							return;
						}
					}
					else if (this._lobbyDataSource.Options.IsEnabled && this._lobbyDataSource.Options.AreHotkeysEnabled)
					{
						UISoundsHelper.PlayUISound("event:/ui/default");
						this._lobbyDataSource.Options.SelectNextCategory();
						return;
					}
				}
				else if (this._lobbyLayer.Input.IsHotKeyPressed("SwitchToPreviousTab"))
				{
					if (this._lobbyDataSource.RankLeaderboard.IsEnabled)
					{
						if (this._lobbyDataSource.RankLeaderboard.IsPreviousPageAvailable)
						{
							UISoundsHelper.PlayUISound("event:/ui/checkbox");
						}
						this._lobbyDataSource.RankLeaderboard.ExecuteLoadPreviousPage();
						return;
					}
					if (this._lobbyDataSource.BadgeProgressionInformation.IsEnabled)
					{
						if (this._lobbyDataSource.BadgeProgressionInformation.CanDecreaseBadgeIndices)
						{
							UISoundsHelper.PlayUISound("event:/ui/checkbox");
							this._lobbyDataSource.BadgeProgressionInformation.ExecuteDecreaseActiveBadgeIndices();
							return;
						}
					}
					else if (!this._isNavigationRestricted)
					{
						this.SelectPreviousPage(MPLobbyVM.LobbyPage.NotAssigned);
						return;
					}
				}
				else if (this._lobbyLayer.Input.IsHotKeyPressed("SwitchToNextTab"))
				{
					if (this._lobbyDataSource.RankLeaderboard.IsEnabled)
					{
						if (this._lobbyDataSource.RankLeaderboard.IsNextPageAvailable)
						{
							UISoundsHelper.PlayUISound("event:/ui/checkbox");
						}
						this._lobbyDataSource.RankLeaderboard.ExecuteLoadNextPage();
						return;
					}
					if (this._lobbyDataSource.BadgeProgressionInformation.IsEnabled)
					{
						if (this._lobbyDataSource.BadgeProgressionInformation.CanIncreaseBadgeIndices)
						{
							UISoundsHelper.PlayUISound("event:/ui/checkbox");
							this._lobbyDataSource.BadgeProgressionInformation.ExecuteIncreaseActiveBadgeIndices();
							return;
						}
					}
					else if (!this._isNavigationRestricted)
					{
						this.SelectNextPage(MPLobbyVM.LobbyPage.NotAssigned);
						return;
					}
				}
				else if (this._lobbyLayer.Input.IsHotKeyReleased("Reset"))
				{
					if (this._lobbyDataSource.HasNoPopupOpen() && this._lobbyDataSource.Options.IsEnabled && this._lobbyDataSource.Options.AreHotkeysEnabled)
					{
						UISoundsHelper.PlayUISound("event:/ui/default");
						this._lobbyDataSource.Options.ExecuteCancel();
						return;
					}
					if (this._lobbyDataSource.Matchmaking.CustomServer.IsEnabled && !this._lobbyDataSource.Matchmaking.CustomServer.IsCreateGamePanelActive && !this._lobbyDataSource.Matchmaking.CustomServer.IsRefreshing)
					{
						this._lobbyDataSource.Matchmaking.CustomServer.ExecuteRefresh();
					}
				}
			}
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00003344 File Offset: 0x00001544
		private void ShowNextFeedback()
		{
			KeyValuePair<string, InquiryData> keyValuePair = this._feedbackInquiries[0];
			this._feedbackInquiries.Remove(keyValuePair);
			this._activeFeedbackId = keyValuePair.Key;
			InformationManager.ShowInquiry(keyValuePair.Value, false, false);
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00003386 File Offset: 0x00001586
		[Conditional("DEBUG")]
		private void TickDebug(float dt)
		{
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00003388 File Offset: 0x00001588
		void ILobbyStateHandler.SetConnectionState(bool isAuthenticated)
		{
			if (this._lobbyDataSource == null)
			{
				this.CreateView();
			}
			if (isAuthenticated && this._lobbyState.LobbyClient.PlayerData != null)
			{
				this._lobbyDataSource.RefreshPlayerData(this._lobbyState.LobbyClient.PlayerData);
				if (this._lobbyDataSource.CurrentPage == MPLobbyVM.LobbyPage.NotAssigned || this._lobbyDataSource.CurrentPage == MPLobbyVM.LobbyPage.Authentication)
				{
					this._lobbyDataSource.SetPage(MPLobbyVM.LobbyPage.Home, MPMatchmakingVM.MatchmakingSubPages.Default);
				}
			}
			else
			{
				if (this._isFacegenOpen)
				{
					this.OnForceCloseFacegen();
				}
				this._lobbyDataSource.SetPage(MPLobbyVM.LobbyPage.Authentication, MPMatchmakingVM.MatchmakingSubPages.Default);
			}
			this._lobbyDataSource.ConnectionStateUpdated(isAuthenticated);
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00003424 File Offset: 0x00001624
		void ILobbyStateHandler.OnRequestedToSearchBattle()
		{
			this._musicSoundEvent.SetParameter("mpMusicSwitcher", 1f);
			MPLobbyVM lobbyDataSource = this._lobbyDataSource;
			if (lobbyDataSource == null)
			{
				return;
			}
			lobbyDataSource.OnRequestedToSearchBattle();
		}

		// Token: 0x06000038 RID: 56 RVA: 0x0000344B File Offset: 0x0000164B
		void ILobbyStateHandler.OnUpdateFindingGame(MatchmakingWaitTimeStats matchmakingWaitTimeStats, string[] gameTypeInfo)
		{
			MPLobbyVM lobbyDataSource = this._lobbyDataSource;
			if (lobbyDataSource == null)
			{
				return;
			}
			lobbyDataSource.OnUpdateFindingGame(matchmakingWaitTimeStats, gameTypeInfo);
		}

		// Token: 0x06000039 RID: 57 RVA: 0x0000345F File Offset: 0x0000165F
		void ILobbyStateHandler.OnRequestedToCancelSearchBattle()
		{
			MPLobbyVM lobbyDataSource = this._lobbyDataSource;
			if (lobbyDataSource == null)
			{
				return;
			}
			lobbyDataSource.OnRequestedToCancelSearchBattle();
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00003471 File Offset: 0x00001671
		void ILobbyStateHandler.OnSearchBattleCanceled()
		{
			this._musicSoundEvent.SetParameter("mpMusicSwitcher", 0f);
			MPLobbyVM lobbyDataSource = this._lobbyDataSource;
			if (lobbyDataSource == null)
			{
				return;
			}
			lobbyDataSource.OnSearchBattleCanceled();
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00003498 File Offset: 0x00001698
		void ILobbyStateHandler.OnPause()
		{
		}

		// Token: 0x0600003C RID: 60 RVA: 0x0000349A File Offset: 0x0000169A
		void ILobbyStateHandler.OnResume()
		{
			MPLobbyVM lobbyDataSource = this._lobbyDataSource;
			if (lobbyDataSource == null)
			{
				return;
			}
			lobbyDataSource.RefreshPlayerData(this._lobbyState.LobbyClient.PlayerData);
		}

		// Token: 0x0600003D RID: 61 RVA: 0x000034BC File Offset: 0x000016BC
		void ILobbyStateHandler.OnDisconnected()
		{
			MPLobbyVM lobbyDataSource = this._lobbyDataSource;
			if (lobbyDataSource == null)
			{
				return;
			}
			lobbyDataSource.OnDisconnected();
		}

		// Token: 0x0600003E RID: 62 RVA: 0x000034CE File Offset: 0x000016CE
		void ILobbyStateHandler.OnPlayerDataReceived(PlayerData playerData)
		{
			MPLobbyVM lobbyDataSource = this._lobbyDataSource;
			if (lobbyDataSource != null)
			{
				lobbyDataSource.RefreshPlayerData(playerData);
			}
			GauntletChatLogView gauntletChatLogView = GauntletChatLogView.Current;
			if (gauntletChatLogView == null)
			{
				return;
			}
			gauntletChatLogView.OnSupportedFeaturesReceived(this._lobbyState.LobbyClient.SupportedFeatures);
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00003501 File Offset: 0x00001701
		void ILobbyStateHandler.OnPendingRejoin()
		{
			this._lobbyDataSource.SetPage(MPLobbyVM.LobbyPage.Rejoin, MPMatchmakingVM.MatchmakingSubPages.Default);
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00003510 File Offset: 0x00001710
		void ILobbyStateHandler.OnEnterBattleWithParty(string[] selectedGameTypes)
		{
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00003514 File Offset: 0x00001714
		void ILobbyStateHandler.OnPartyInvitationReceived(PlayerId playerID)
		{
			if (!this._lobbyState.LobbyClient.SupportedFeatures.SupportsFeatures(Features.Party))
			{
				this._lobbyState.LobbyClient.DeclinePartyInvitation();
				return;
			}
			MPLobbyVM lobbyDataSource = this._lobbyDataSource;
			if (lobbyDataSource == null)
			{
				return;
			}
			lobbyDataSource.PartyInvitationPopup.OpenWith(playerID);
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00003560 File Offset: 0x00001760
		void ILobbyStateHandler.OnPartyJoinRequestReceived(PlayerId joingPlayerId, PlayerId viaPlayerId, string viaPlayerName, bool newParty)
		{
			if (this._lobbyState.LobbyClient.SupportedFeatures.SupportsFeatures(Features.Party))
			{
				if (this._lobbyDataSource != null)
				{
					if (newParty)
					{
						this._lobbyDataSource.PartyJoinRequestPopup.OpenWithNewParty(joingPlayerId);
						return;
					}
					this._lobbyDataSource.PartyJoinRequestPopup.OpenWith(joingPlayerId, viaPlayerId, viaPlayerName);
					return;
				}
			}
			else
			{
				this._lobbyState.LobbyClient.DeclinePartyJoinRequest(joingPlayerId, PartyJoinDeclineReason.NoFeature);
			}
		}

		// Token: 0x06000043 RID: 67 RVA: 0x000035C9 File Offset: 0x000017C9
		void ILobbyStateHandler.OnPartyInvitationInvalidated()
		{
			MPLobbyVM lobbyDataSource = this._lobbyDataSource;
			if (lobbyDataSource == null)
			{
				return;
			}
			lobbyDataSource.PartyInvitationPopup.Close();
		}

		// Token: 0x06000044 RID: 68 RVA: 0x000035E0 File Offset: 0x000017E0
		void ILobbyStateHandler.OnPlayerInvitedToParty(PlayerId playerId)
		{
			MPLobbyVM lobbyDataSource = this._lobbyDataSource;
			if (lobbyDataSource == null)
			{
				return;
			}
			lobbyDataSource.Friends.OnPlayerInvitedToParty(playerId);
		}

		// Token: 0x06000045 RID: 69 RVA: 0x000035F8 File Offset: 0x000017F8
		void ILobbyStateHandler.OnPlayerAddedToParty(PlayerId playerId, string playerName, bool isPartyLeader)
		{
			MPLobbyVM lobbyDataSource = this._lobbyDataSource;
			if (lobbyDataSource == null)
			{
				return;
			}
			lobbyDataSource.OnPlayerAddedToParty(playerId);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x0000360B File Offset: 0x0000180B
		void ILobbyStateHandler.OnPlayerRemovedFromParty(PlayerId playerId, PartyRemoveReason reason)
		{
			MPLobbyVM lobbyDataSource = this._lobbyDataSource;
			if (lobbyDataSource == null)
			{
				return;
			}
			lobbyDataSource.OnPlayerRemovedFromParty(playerId, reason);
		}

		// Token: 0x06000047 RID: 71 RVA: 0x0000361F File Offset: 0x0000181F
		void ILobbyStateHandler.OnGameClientStateChange(LobbyClient.State state)
		{
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00003621 File Offset: 0x00001821
		void ILobbyStateHandler.OnAdminMessageReceived(string message)
		{
			InformationManager.AddSystemNotification(message);
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00003629 File Offset: 0x00001829
		public void OnBattleServerInformationReceived(BattleServerInformationForClient battleServerInformation)
		{
			UISoundsHelper.PlayUISound("event:/ui/multiplayer/match_ready");
			this._lobbyDataSource.Matchmaking.IsFindingMatch = false;
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00003648 File Offset: 0x00001848
		string ILobbyStateHandler.ShowFeedback(string title, string feedbackText)
		{
			string id = Guid.NewGuid().ToString();
			InquiryData inquiryData = new InquiryData(title, feedbackText, false, true, "", new TextObject("{=dismissnotification}Dismiss", null).ToString(), null, delegate
			{
				((ILobbyStateHandler)this).DismissFeedback(id);
			}, "", 0f, null, null, null);
			this._feedbackInquiries.Add(new KeyValuePair<string, InquiryData>(id, inquiryData));
			return id;
		}

		// Token: 0x0600004B RID: 75 RVA: 0x000036D4 File Offset: 0x000018D4
		string ILobbyStateHandler.ShowFeedback(InquiryData inquiryData)
		{
			string text = Guid.NewGuid().ToString();
			this._feedbackInquiries.Add(new KeyValuePair<string, InquiryData>(text, inquiryData));
			return text;
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00003708 File Offset: 0x00001908
		void ILobbyStateHandler.DismissFeedback(string feedbackId)
		{
			if (this._activeFeedbackId != null && this._activeFeedbackId.Equals(feedbackId))
			{
				InformationManager.HideInquiry();
				this._activeFeedbackId = null;
				return;
			}
			KeyValuePair<string, InquiryData> keyValuePair = this._feedbackInquiries.FirstOrDefault<KeyValuePair<string, InquiryData>>((KeyValuePair<string, InquiryData> q) => q.Key.Equals(feedbackId));
			if (keyValuePair.Key != null)
			{
				this._feedbackInquiries.Remove(keyValuePair);
			}
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00003778 File Offset: 0x00001978
		private void SelectPreviousPage(MPLobbyVM.LobbyPage currentPage = MPLobbyVM.LobbyPage.NotAssigned)
		{
			MPLobbyVM lobbyDataSource = this._lobbyDataSource;
			if (lobbyDataSource != null && lobbyDataSource.HasNoPopupOpen())
			{
				if (currentPage == MPLobbyVM.LobbyPage.NotAssigned)
				{
					currentPage = this._lobbyDataSource.CurrentPage;
				}
				if (currentPage < MPLobbyVM.LobbyPage.Options || currentPage > MPLobbyVM.LobbyPage.Profile)
				{
					return;
				}
				MPLobbyVM.LobbyPage lobbyPage = ((currentPage == MPLobbyVM.LobbyPage.Options) ? MPLobbyVM.LobbyPage.Profile : (currentPage - 1));
				if (this._lobbyDataSource.DisallowedPages.Contains(lobbyPage))
				{
					this.SelectPreviousPage(lobbyPage);
					return;
				}
				if (lobbyPage == MPLobbyVM.LobbyPage.Options)
				{
					UISoundsHelper.PlayUISound("event:/ui/checkbox");
				}
				else
				{
					UISoundsHelper.PlayUISound("event:/ui/tab");
				}
				this._lobbyDataSource.SetPage(lobbyPage, MPMatchmakingVM.MatchmakingSubPages.Default);
			}
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00003800 File Offset: 0x00001A00
		private void SelectNextPage(MPLobbyVM.LobbyPage currentPage = MPLobbyVM.LobbyPage.NotAssigned)
		{
			MPLobbyVM lobbyDataSource = this._lobbyDataSource;
			if (lobbyDataSource != null && lobbyDataSource.HasNoPopupOpen())
			{
				if (currentPage == MPLobbyVM.LobbyPage.NotAssigned)
				{
					currentPage = this._lobbyDataSource.CurrentPage;
				}
				if (currentPage < MPLobbyVM.LobbyPage.Options || currentPage > MPLobbyVM.LobbyPage.Profile)
				{
					return;
				}
				MPLobbyVM.LobbyPage lobbyPage = ((currentPage == MPLobbyVM.LobbyPage.Profile) ? MPLobbyVM.LobbyPage.Options : (currentPage + 1));
				if (this._lobbyDataSource.DisallowedPages.Contains(lobbyPage))
				{
					this.SelectNextPage(lobbyPage);
					return;
				}
				if (lobbyPage == MPLobbyVM.LobbyPage.Options)
				{
					UISoundsHelper.PlayUISound("event:/ui/checkbox");
				}
				else
				{
					UISoundsHelper.PlayUISound("event:/ui/tab");
				}
				this._lobbyDataSource.SetPage(lobbyPage, MPMatchmakingVM.MatchmakingSubPages.Default);
			}
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00003887 File Offset: 0x00001A87
		void ILobbyStateHandler.OnActivateCustomServer()
		{
			this._lobbyDataSource.SetPage(MPLobbyVM.LobbyPage.Matchmaking, MPMatchmakingVM.MatchmakingSubPages.CustomGameList);
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00003896 File Offset: 0x00001A96
		void ILobbyStateHandler.OnActivateHome()
		{
			this._lobbyDataSource.SetPage(MPLobbyVM.LobbyPage.Home, MPMatchmakingVM.MatchmakingSubPages.Default);
		}

		// Token: 0x06000051 RID: 81 RVA: 0x000038A5 File Offset: 0x00001AA5
		void ILobbyStateHandler.OnActivateMatchmaking()
		{
			this._lobbyDataSource.SetPage(MPLobbyVM.LobbyPage.Matchmaking, MPMatchmakingVM.MatchmakingSubPages.Default);
		}

		// Token: 0x06000052 RID: 82 RVA: 0x000038B4 File Offset: 0x00001AB4
		void ILobbyStateHandler.OnActivateArmory()
		{
			this._lobbyDataSource.SetPage(MPLobbyVM.LobbyPage.Armory, MPMatchmakingVM.MatchmakingSubPages.Default);
		}

		// Token: 0x06000053 RID: 83 RVA: 0x000038C3 File Offset: 0x00001AC3
		void ILobbyStateHandler.OnActivateProfile()
		{
			this._lobbyDataSource.SetPage(MPLobbyVM.LobbyPage.Profile, MPMatchmakingVM.MatchmakingSubPages.Default);
		}

		// Token: 0x06000054 RID: 84 RVA: 0x000038D2 File Offset: 0x00001AD2
		void ILobbyStateHandler.OnClanInvitationReceived(string clanName, string clanTag, bool isCreation)
		{
			this._lobbyDataSource.ClanInvitationPopup.Open(clanName, clanTag, isCreation);
		}

		// Token: 0x06000055 RID: 85 RVA: 0x000038E7 File Offset: 0x00001AE7
		void ILobbyStateHandler.OnClanInvitationAnswered(PlayerId playerId, ClanCreationAnswer answer)
		{
			MPLobbyVM lobbyDataSource = this._lobbyDataSource;
			if (lobbyDataSource != null)
			{
				lobbyDataSource.ClanCreationPopup.UpdateConfirmation(playerId, answer);
			}
			MPLobbyVM lobbyDataSource2 = this._lobbyDataSource;
			if (lobbyDataSource2 == null)
			{
				return;
			}
			lobbyDataSource2.ClanInvitationPopup.UpdateConfirmation(playerId, answer);
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00003918 File Offset: 0x00001B18
		void ILobbyStateHandler.OnClanCreationSuccessful()
		{
			MPLobbyVM lobbyDataSource = this._lobbyDataSource;
			if (lobbyDataSource == null)
			{
				return;
			}
			lobbyDataSource.OnClanCreationFinished();
		}

		// Token: 0x06000057 RID: 87 RVA: 0x0000392A File Offset: 0x00001B2A
		void ILobbyStateHandler.OnClanCreationFailed()
		{
			MPLobbyVM lobbyDataSource = this._lobbyDataSource;
			if (lobbyDataSource == null)
			{
				return;
			}
			lobbyDataSource.OnClanCreationFinished();
		}

		// Token: 0x06000058 RID: 88 RVA: 0x0000393C File Offset: 0x00001B3C
		void ILobbyStateHandler.OnClanCreationStarted()
		{
			this._lobbyDataSource.ClanCreationPopup.ExecuteSwitchToWaiting();
		}

		// Token: 0x06000059 RID: 89 RVA: 0x0000394E File Offset: 0x00001B4E
		void ILobbyStateHandler.OnClanInfoChanged()
		{
			MPLobbyVM lobbyDataSource = this._lobbyDataSource;
			if (lobbyDataSource == null)
			{
				return;
			}
			lobbyDataSource.OnClanInfoChanged();
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00003960 File Offset: 0x00001B60
		void ILobbyStateHandler.OnPremadeGameEligibilityStatusReceived(bool isEligible)
		{
			MPLobbyVM lobbyDataSource = this._lobbyDataSource;
			if (lobbyDataSource == null)
			{
				return;
			}
			lobbyDataSource.Matchmaking.OnPremadeGameEligibilityStatusReceived(isEligible);
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00003978 File Offset: 0x00001B78
		void ILobbyStateHandler.OnPremadeGameCreated()
		{
			MPLobbyVM lobbyDataSource = this._lobbyDataSource;
			if (lobbyDataSource == null)
			{
				return;
			}
			lobbyDataSource.OnPremadeGameCreated();
		}

		// Token: 0x0600005C RID: 92 RVA: 0x0000398C File Offset: 0x00001B8C
		void ILobbyStateHandler.OnPremadeGameListReceived()
		{
			LobbyClient gameClient = NetworkMain.GameClient;
			PremadeGameEntry[] array;
			if (gameClient == null)
			{
				array = null;
			}
			else
			{
				PremadeGameList availablePremadeGames = gameClient.AvailablePremadeGames;
				array = ((availablePremadeGames != null) ? availablePremadeGames.PremadeGameEntries : null);
			}
			PremadeGameEntry[] array2 = array;
			if (array2 != null)
			{
				MPLobbyVM lobbyDataSource = this._lobbyDataSource;
				if (lobbyDataSource == null)
				{
					return;
				}
				lobbyDataSource.Matchmaking.PremadeMatches.SetPremadeGameList(array2);
			}
		}

		// Token: 0x0600005D RID: 93 RVA: 0x000039D5 File Offset: 0x00001BD5
		void ILobbyStateHandler.OnPremadeGameCreationCancelled()
		{
			this._musicSoundEvent.SetParameter("mpMusicSwitcher", 0f);
			MPLobbyVM lobbyDataSource = this._lobbyDataSource;
			if (lobbyDataSource == null)
			{
				return;
			}
			lobbyDataSource.OnSearchBattleCanceled();
		}

		// Token: 0x0600005E RID: 94 RVA: 0x000039FC File Offset: 0x00001BFC
		void ILobbyStateHandler.OnJoinPremadeGameRequested(string clanName, string clanSigilCode, Guid partyId, PlayerId[] challengerPlayerIDs, PlayerId challengerPartyLeaderID, PremadeGameType premadeGameType)
		{
			this._lobbyDataSource.ClanMatchmakingRequestPopup.OpenWith(clanName, clanSigilCode, partyId, challengerPlayerIDs, challengerPartyLeaderID, premadeGameType);
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00003A17 File Offset: 0x00001C17
		void ILobbyStateHandler.OnJoinPremadeGameRequestSuccessful()
		{
			if (!this._lobbyDataSource.GameSearch.IsEnabled)
			{
				this._lobbyDataSource.OnPremadeGameCreated();
			}
			this._lobbyDataSource.GameSearch.OnJoinPremadeGameRequestSuccessful();
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00003A46 File Offset: 0x00001C46
		void ILobbyStateHandler.OnSigilChanged()
		{
			if (this._lobbyDataSource != null)
			{
				this._lobbyDataSource.OnSigilChanged(this._lobbyDataSource.ChangeSigilPopup.SelectedSigil.IconID);
			}
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00003A70 File Offset: 0x00001C70
		void ILobbyStateHandler.OnActivateOptions()
		{
			this._lobbyDataSource.SetPage(MPLobbyVM.LobbyPage.Options, MPMatchmakingVM.MatchmakingSubPages.Default);
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00003A7F File Offset: 0x00001C7F
		void ILobbyStateHandler.OnDeactivateOptions()
		{
			this._lobbyDataSource.SetPage(MPLobbyVM.LobbyPage.Home, MPMatchmakingVM.MatchmakingSubPages.Default);
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00003A8E File Offset: 0x00001C8E
		void ILobbyStateHandler.OnCustomGameServerListReceived(AvailableCustomGames customGameServerList)
		{
			this._lobbyDataSource.Matchmaking.CustomServer.SetCustomGameServerList(customGameServerList);
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00003AA8 File Offset: 0x00001CA8
		void ILobbyStateHandler.OnMatchmakerGameOver(int oldExperience, int newExperience, List<string> badgesEarned, int lootGained, RankBarInfo oldRankBarInfo, RankBarInfo newRankBarInfo, BattleCancelReason battleCancelReason)
		{
			if (battleCancelReason != BattleCancelReason.None)
			{
				if (battleCancelReason == BattleCancelReason.PlayerLeaveDuringWarmup)
				{
					object obj = new TextObject("{=CtMEl2NP}Game is cancelled", null);
					TextObject textObject = new TextObject("{=A6OFgTIU}Game is cancelled due to a player leaving during warmup.", null);
					InformationManager.ShowInquiry(new InquiryData(obj.ToString(), textObject.ToString(), false, true, "", GameTexts.FindText("str_dismiss", null).ToString(), null, null, "", 0f, null, null, null), false, false);
				}
				return;
			}
			MPLobbyVM lobbyDataSource = this._lobbyDataSource;
			if (lobbyDataSource == null)
			{
				return;
			}
			lobbyDataSource.AfterBattlePopup.OpenWith(oldExperience, newExperience, badgesEarned, lootGained, oldRankBarInfo, newRankBarInfo);
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00003B34 File Offset: 0x00001D34
		void ILobbyStateHandler.OnBattleServerLost()
		{
			TextObject textObject = new TextObject("{=wLpJEkKY}Battle Server Crashed", null);
			TextObject textObject2 = new TextObject("{=EzeFJo65}You have been disconnected from server!", null);
			this._lobbyDataSource.QueryPopup.ShowMessage(textObject, textObject2);
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00003B6B File Offset: 0x00001D6B
		void ILobbyStateHandler.OnRemovedFromMatchmakerGame(DisconnectType disconnectType)
		{
			this.ShowDisconnectMessage(disconnectType);
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00003B74 File Offset: 0x00001D74
		void ILobbyStateHandler.OnRemovedFromCustomGame(DisconnectType disconnectType)
		{
			this.ShowDisconnectMessage(disconnectType);
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00003B7D File Offset: 0x00001D7D
		void ILobbyStateHandler.OnPlayerAssignedPartyLeader(PlayerId partyLeaderId)
		{
			MPLobbyVM lobbyDataSource = this._lobbyDataSource;
			if (lobbyDataSource == null)
			{
				return;
			}
			lobbyDataSource.OnPlayerAssignedPartyLeader(partyLeaderId);
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00003B90 File Offset: 0x00001D90
		void ILobbyStateHandler.OnPlayerSuggestedToParty(PlayerId playerId, string playerName, PlayerId suggestingPlayerId, string suggestingPlayerName)
		{
			MPLobbyVM lobbyDataSource = this._lobbyDataSource;
			if (lobbyDataSource == null)
			{
				return;
			}
			lobbyDataSource.OnPlayerSuggestedToParty(playerId, playerName, suggestingPlayerId, suggestingPlayerName);
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00003BA7 File Offset: 0x00001DA7
		void ILobbyStateHandler.OnNotificationsReceived(LobbyNotification[] notifications)
		{
			MPLobbyVM lobbyDataSource = this._lobbyDataSource;
			if (lobbyDataSource == null)
			{
				return;
			}
			lobbyDataSource.OnNotificationsReceived(notifications);
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00003BBC File Offset: 0x00001DBC
		void ILobbyStateHandler.OnJoinCustomGameFailureResponse(CustomGameJoinResponse response)
		{
			TextObject textObject = new TextObject("{=4mMySbxI}Unspecified error", null);
			switch (response)
			{
			case CustomGameJoinResponse.IncorrectPlayerState:
				textObject = new TextObject("{=KO2adj2I}You need to be in Lobby to join a custom game", null);
				break;
			case CustomGameJoinResponse.ServerCapacityIsFull:
				textObject = new TextObject("{=IzJ7f5SQ}Server capacity is full", null);
				break;
			case CustomGameJoinResponse.ErrorOnGameServer:
				textObject = new TextObject("{=vkpMgobZ}Game server error", null);
				break;
			case CustomGameJoinResponse.GameServerAccessError:
				textObject = new TextObject("{=JQVixeQs}Couldn't access game server", null);
				break;
			case CustomGameJoinResponse.CustomGameServerNotAvailable:
				textObject = new TextObject("{=T8IniCKU}Game server is not available", null);
				break;
			case CustomGameJoinResponse.CustomGameServerFinishing:
				textObject = new TextObject("{=KRNdlbkq}Custom game is ending", null);
				break;
			case CustomGameJoinResponse.IncorrectPassword:
				textObject = new TextObject("{=Mm1Kb1bS}Incorrect password", null);
				break;
			case CustomGameJoinResponse.PlayerBanned:
				textObject = new TextObject("{=srAJw3Tg}Player is banned from server", null);
				break;
			case CustomGameJoinResponse.AlreadyRequestedWaitingForServerResponse:
				textObject = new TextObject("{=ivKntfNA}Already requested to join, waiting for server response", null);
				break;
			case CustomGameJoinResponse.RequesterIsNotPartyLeader:
				textObject = new TextObject("{=KQrpWV1n}You need be the party leader to join a custom game", null);
				break;
			case CustomGameJoinResponse.NotAllPlayersReady:
				textObject = new TextObject("{=tlsmbvQX}Not all players are ready to join", null);
				break;
			case CustomGameJoinResponse.NotAllPlayersModulesMatchWithServer:
				textObject = new TextObject("{=LCzAvLUB}Not all players' modules match with the server", null);
				break;
			}
			TextObject textObject2 = new TextObject("{=mO9bh5sy}Couldn't join custom game", null);
			this._lobbyDataSource.QueryPopup.ShowMessage(textObject2, textObject);
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00003CEC File Offset: 0x00001EEC
		void ILobbyStateHandler.OnServerStatusReceived(ServerStatus serverStatus)
		{
			MPLobbyVM lobbyDataSource = this._lobbyDataSource;
			if (lobbyDataSource != null)
			{
				lobbyDataSource.OnServerStatusReceived(serverStatus);
			}
			if (serverStatus.Announcement != null)
			{
				if (serverStatus.Announcement.Type == AnnouncementType.Alert)
				{
					InformationManager.AddSystemNotification(new TextObject(serverStatus.Announcement.Text, null).ToString());
					return;
				}
				if (serverStatus.Announcement.Type == AnnouncementType.Chat)
				{
					InformationManager.DisplayMessage(new InformationMessage(new TextObject(serverStatus.Announcement.Text, null).ToString()));
				}
			}
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00003D6A File Offset: 0x00001F6A
		void ILobbyStateHandler.OnRejoinBattleRequestAnswered(bool isSuccessful)
		{
			MPLobbyVM lobbyDataSource = this._lobbyDataSource;
			if (lobbyDataSource == null)
			{
				return;
			}
			lobbyDataSource.OnRejoinBattleRequestAnswered(isSuccessful);
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00003D7D File Offset: 0x00001F7D
		void ILobbyStateHandler.OnFriendListUpdated()
		{
			MPLobbyVM lobbyDataSource = this._lobbyDataSource;
			if (lobbyDataSource == null)
			{
				return;
			}
			lobbyDataSource.OnFriendListUpdated(false);
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00003D90 File Offset: 0x00001F90
		void ILobbyStateHandler.OnPlayerNameUpdated(string playerName)
		{
			MPLobbyVM lobbyDataSource = this._lobbyDataSource;
			if (lobbyDataSource == null)
			{
				return;
			}
			lobbyDataSource.OnPlayerNameUpdated(playerName);
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00003DA4 File Offset: 0x00001FA4
		private void ShowDisconnectMessage(DisconnectType disconnectType)
		{
			if (disconnectType != DisconnectType.QuitFromGame && disconnectType != DisconnectType.GameEnded)
			{
				TextObject textObject = new TextObject("{=JluTW3Qw}Game Ended", null);
				TextObject textObject2 = new TextObject("{=aKjpbRP5}Unknown reason", null);
				switch (disconnectType)
				{
				case DisconnectType.TimedOut:
					textObject2 = new TextObject("{=WvGviFgt}Your connection with the server timed out", null);
					break;
				case DisconnectType.KickedByHost:
					textObject2 = new TextObject("{=a0IHtkoa}You are kicked by game host", null);
					break;
				case DisconnectType.KickedByPoll:
					textObject2 = new TextObject("{=wbFB3N72}You are kicked from game by poll", null);
					break;
				case DisconnectType.BannedByPoll:
					textObject2 = new TextObject("{=OhF7NqSb}You are banned from game by poll", null);
					break;
				case DisconnectType.Inactivity:
					textObject2 = new TextObject("{=074YAjOk}You are kicked due to inactivity", null);
					break;
				case DisconnectType.ServerNotResponding:
					textObject2 = new TextObject("{=tKSxGy5p}Server not responding", null);
					break;
				case DisconnectType.KickedDueToFriendlyDamage:
					textObject2 = new TextObject("{=InUAmnX4}You are kicked due to friendly damage", null);
					break;
				case DisconnectType.PlayStateMismatch:
					textObject2 = new TextObject("{=O1bGoaE8}Server state could not be retrieved. Please try again.", null);
					break;
				}
				this._lobbyDataSource.QueryPopup.ShowMessage(textObject, textObject2);
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000071 RID: 113 RVA: 0x00003E88 File Offset: 0x00002088
		public MPLobbyVM DataSource
		{
			get
			{
				return this._lobbyDataSource;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000072 RID: 114 RVA: 0x00003E90 File Offset: 0x00002090
		public GauntletLayer LobbyLayer
		{
			get
			{
				return this._lobbyLayer;
			}
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00003E98 File Offset: 0x00002098
		private void DisableLobby()
		{
			if (!this._isLobbyActive)
			{
				return;
			}
			this._isLobbyActive = false;
			SpriteCategory mplobbyCategory = this._mplobbyCategory;
			if (mplobbyCategory != null)
			{
				mplobbyCategory.Unload();
			}
			SpriteCategory bannerIconsCategory = this._bannerIconsCategory;
			if (bannerIconsCategory != null)
			{
				bannerIconsCategory.Unload();
			}
			base.RemoveLayer(this._lobbyLayer);
			this._lobbyDataSource = null;
			this._lobbyLayer = null;
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00003EF1 File Offset: 0x000020F1
		private void OnKeybindRequest(KeyOptionVM requestedHotKeyToChange)
		{
			this._currentKey = requestedHotKeyToChange;
			this._keybindingPopup.OnToggle(true);
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00003F08 File Offset: 0x00002108
		private void SetHotKey(Key key)
		{
			if (key.IsControllerInput)
			{
				MBInformationManager.AddQuickInformation(new TextObject("{=B41vvGuo}Invalid key", null), 0, null, null, "");
				this._keybindingPopup.OnToggle(false);
				return;
			}
			GameKeyOptionVM gameKey;
			if ((gameKey = this._currentKey as GameKeyOptionVM) != null)
			{
				MPLobbyVM lobbyDataSource = this._lobbyDataSource;
				if (((lobbyDataSource != null) ? lobbyDataSource.Options.GameKeyOptionGroups.GameKeyGroups.FirstOrDefault<GameKeyGroupVM>((GameKeyGroupVM g) => g.GameKeys.Contains(gameKey)) : null) == null)
				{
					Debug.FailedAssert("Could not find GameKeyGroup during SetHotKey", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.GauntletUI\\MultiplayerLobbyGauntletScreen.cs", "SetHotKey", 1301);
					MBInformationManager.AddQuickInformation(new TextObject("{=oZrVNUOk}Error", null), 0, null, null, "");
					this._keybindingPopup.OnToggle(false);
					return;
				}
				if (key.InputKey != gameKey.CurrentKey.InputKey)
				{
					GauntletLayer lobbyLayer = this._lobbyLayer;
					if (lobbyLayer == null || !lobbyLayer.Input.IsHotKeyReleased("Exit"))
					{
						GameKeyOptionVM gameKey2 = gameKey;
						if (gameKey2 != null)
						{
							gameKey2.Set(key.InputKey);
						}
						gameKey = null;
						this._keybindingPopup.OnToggle(false);
						goto IL_0222;
					}
				}
				this._keybindingPopup.OnToggle(false);
			}
			else
			{
				AuxiliaryKeyOptionVM auxiliaryKey;
				if ((auxiliaryKey = this._currentKey as AuxiliaryKeyOptionVM) != null)
				{
					if (this._lobbyDataSource.Options.GameKeyOptionGroups.AuxiliaryKeyGroups.FirstOrDefault<AuxiliaryKeyGroupVM>((AuxiliaryKeyGroupVM g) => g.HotKeys.Contains(auxiliaryKey)) == null)
					{
						Debug.FailedAssert("Could not find AuxiliaryKeyGroup during SetHotKey", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.GauntletUI\\MultiplayerLobbyGauntletScreen.cs", "SetHotKey", 1324);
						MBInformationManager.AddQuickInformation(new TextObject("{=oZrVNUOk}Error", null), 0, null, null, "");
						this._keybindingPopup.OnToggle(false);
						return;
					}
					if (key.InputKey != auxiliaryKey.CurrentKey.InputKey)
					{
						GauntletLayer lobbyLayer2 = this._lobbyLayer;
						if (lobbyLayer2 == null || !lobbyLayer2.Input.IsHotKeyReleased("Exit"))
						{
							AuxiliaryKeyOptionVM auxiliaryKey2 = auxiliaryKey;
							if (auxiliaryKey2 != null)
							{
								auxiliaryKey2.Set(key.InputKey);
							}
							auxiliaryKey = null;
							this._keybindingPopup.OnToggle(false);
							goto IL_0222;
						}
					}
					this._keybindingPopup.OnToggle(false);
				}
			}
			IL_0222:
			MPLobbyVM lobbyDataSource2 = this._lobbyDataSource;
			if (lobbyDataSource2 == null)
			{
				return;
			}
			MPOptionsVM options = lobbyDataSource2.Options;
			if (options == null)
			{
				return;
			}
			GameKeyOptionCategoryVM gameKeyOptionGroups = options.GameKeyOptionGroups;
			if (gameKeyOptionGroups == null)
			{
				return;
			}
			gameKeyOptionGroups.RefreshValues();
		}

		// Token: 0x06000076 RID: 118 RVA: 0x0000415B File Offset: 0x0000235B
		void IGameStateListener.OnActivate()
		{
			if (this._lobbyDataSource == null)
			{
				this.CreateView();
				this._lobbyDataSource.SetPage(MPLobbyVM.LobbyPage.Authentication, MPMatchmakingVM.MatchmakingSubPages.Default);
				return;
			}
			this._optionsSpriteCategory = UIResourceManager.LoadSpriteCategory("ui_options");
			this._multiplayerSpriteCategory = UIResourceManager.LoadSpriteCategory("ui_mpmission");
		}

		// Token: 0x06000077 RID: 119 RVA: 0x0000419C File Offset: 0x0000239C
		void IGameStateListener.OnDeactivate()
		{
			if (this._lobbyDataSource != null)
			{
				MPCustomGameSortControllerVM sortController = this._lobbyDataSource.Matchmaking.CustomServer.SortController;
				this._cachedCustomServerSortOption = sortController.CurrentSortOption;
				this._cachedCustomServerSortState = (MPCustomGameSortControllerVM.SortState)sortController.CurrentSortState;
				MPCustomGameSortControllerVM sortController2 = this._lobbyDataSource.Matchmaking.PremadeMatches.SortController;
				this._cachedPremadeGameSortOption = sortController2.CurrentSortOption;
				this._cachedPremadeGameSortState = (MPCustomGameSortControllerVM.SortState)sortController2.CurrentSortState;
			}
			if (this._lobbyState.GameStateManager.LastOrDefault<LobbyPracticeState>() == null)
			{
				base.RemoveLayer(this._lobbyLayer);
				if (this._lobbyDataSource != null)
				{
					this._lobbyDataSource.OnFinalize();
					this._lobbyDataSource = null;
				}
				this._lobbyLayer = null;
				SpriteCategory mplobbyCategory = this._mplobbyCategory;
				if (mplobbyCategory != null)
				{
					mplobbyCategory.Unload();
				}
				SpriteCategory bannerIconsCategory = this._bannerIconsCategory;
				if (bannerIconsCategory != null)
				{
					bannerIconsCategory.Unload();
				}
				this._musicSoundEvent.Stop();
				this._musicSoundEvent = null;
			}
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00004283 File Offset: 0x00002483
		void IGameStateListener.OnInitialize()
		{
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00004285 File Offset: 0x00002485
		void IGameStateListener.OnFinalize()
		{
		}

		// Token: 0x0400000A RID: 10
		private List<KeyValuePair<string, InquiryData>> _feedbackInquiries;

		// Token: 0x0400000B RID: 11
		private string _activeFeedbackId;

		// Token: 0x0400000C RID: 12
		private KeybindingPopup _keybindingPopup;

		// Token: 0x0400000D RID: 13
		private KeyOptionVM _currentKey;

		// Token: 0x0400000E RID: 14
		private SpriteCategory _optionsSpriteCategory;

		// Token: 0x0400000F RID: 15
		private SpriteCategory _multiplayerSpriteCategory;

		// Token: 0x04000010 RID: 16
		private GauntletLayer _gauntletBrightnessLayer;

		// Token: 0x04000011 RID: 17
		private BrightnessOptionVM _brightnessOptionDataSource;

		// Token: 0x04000012 RID: 18
		private GauntletMovieIdentifier _brightnessOptionMovie;

		// Token: 0x04000013 RID: 19
		private LobbyState _lobbyState;

		// Token: 0x04000014 RID: 20
		private BasicCharacterObject _playerCharacter;

		// Token: 0x04000015 RID: 21
		private bool _isFacegenOpen;

		// Token: 0x04000016 RID: 22
		private SoundEvent _musicSoundEvent;

		// Token: 0x04000017 RID: 23
		private bool _isNavigationRestricted;

		// Token: 0x04000018 RID: 24
		private bool _wasChatRestricted;

		// Token: 0x04000019 RID: 25
		private MPCustomGameSortControllerVM.CustomServerSortOption? _cachedCustomServerSortOption;

		// Token: 0x0400001A RID: 26
		private MPCustomGameSortControllerVM.SortState _cachedCustomServerSortState;

		// Token: 0x0400001B RID: 27
		private MPCustomGameSortControllerVM.CustomServerSortOption? _cachedPremadeGameSortOption;

		// Token: 0x0400001C RID: 28
		private MPCustomGameSortControllerVM.SortState _cachedPremadeGameSortState;

		// Token: 0x0400001D RID: 29
		private bool _isLobbyActive;

		// Token: 0x0400001E RID: 30
		private GauntletLayer _lobbyLayer;

		// Token: 0x0400001F RID: 31
		private MPLobbyVM _lobbyDataSource;

		// Token: 0x04000020 RID: 32
		private SpriteCategory _mplobbyCategory;

		// Token: 0x04000021 RID: 33
		private SpriteCategory _bannerIconsCategory;

		// Token: 0x04000022 RID: 34
		private SpriteCategory _badgesCategory;
	}
}
