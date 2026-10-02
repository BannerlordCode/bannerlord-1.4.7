using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameMenus
{
	// Token: 0x020000E2 RID: 226
	public class GameMenu
	{
		// Token: 0x170005D4 RID: 1492
		// (get) Token: 0x06001520 RID: 5408 RVA: 0x00060F91 File Offset: 0x0005F191
		// (set) Token: 0x06001521 RID: 5409 RVA: 0x00060F99 File Offset: 0x0005F199
		public GameMenu.MenuAndOptionType Type { get; private set; }

		// Token: 0x170005D5 RID: 1493
		// (get) Token: 0x06001522 RID: 5410 RVA: 0x00060FA2 File Offset: 0x0005F1A2
		// (set) Token: 0x06001523 RID: 5411 RVA: 0x00060FAA File Offset: 0x0005F1AA
		public string StringId { get; private set; }

		// Token: 0x170005D6 RID: 1494
		// (get) Token: 0x06001524 RID: 5412 RVA: 0x00060FB3 File Offset: 0x0005F1B3
		// (set) Token: 0x06001525 RID: 5413 RVA: 0x00060FBB File Offset: 0x0005F1BB
		public object RelatedObject { get; private set; }

		// Token: 0x170005D7 RID: 1495
		// (get) Token: 0x06001526 RID: 5414 RVA: 0x00060FC4 File Offset: 0x0005F1C4
		// (set) Token: 0x06001527 RID: 5415 RVA: 0x00060FCC File Offset: 0x0005F1CC
		public TextObject MenuTitle { get; private set; }

		// Token: 0x170005D8 RID: 1496
		// (get) Token: 0x06001528 RID: 5416 RVA: 0x00060FD5 File Offset: 0x0005F1D5
		// (set) Token: 0x06001529 RID: 5417 RVA: 0x00060FDD File Offset: 0x0005F1DD
		public GameMenu.MenuOverlayType OverlayType { get; private set; }

		// Token: 0x170005D9 RID: 1497
		// (get) Token: 0x0600152A RID: 5418 RVA: 0x00060FE6 File Offset: 0x0005F1E6
		// (set) Token: 0x0600152B RID: 5419 RVA: 0x00060FEE File Offset: 0x0005F1EE
		public bool IsReady { get; private set; }

		// Token: 0x170005DA RID: 1498
		// (get) Token: 0x0600152C RID: 5420 RVA: 0x00060FF7 File Offset: 0x0005F1F7
		public int MenuItemAmount
		{
			get
			{
				return this._menuItems.Count;
			}
		}

		// Token: 0x170005DB RID: 1499
		// (get) Token: 0x0600152D RID: 5421 RVA: 0x00061004 File Offset: 0x0005F204
		// (set) Token: 0x0600152E RID: 5422 RVA: 0x0006100C File Offset: 0x0005F20C
		public List<object> MenuRepeatObjects { get; private set; } = new List<object>();

		// Token: 0x170005DC RID: 1500
		// (get) Token: 0x0600152F RID: 5423 RVA: 0x00061015 File Offset: 0x0005F215
		public object CurrentRepeatableObject
		{
			get
			{
				if (this.MenuRepeatObjects.Count <= this.CurrentRepeatableIndex)
				{
					return null;
				}
				return this.MenuRepeatObjects[this.CurrentRepeatableIndex];
			}
		}

		// Token: 0x170005DD RID: 1501
		// (get) Token: 0x06001530 RID: 5424 RVA: 0x0006103D File Offset: 0x0005F23D
		// (set) Token: 0x06001531 RID: 5425 RVA: 0x00061045 File Offset: 0x0005F245
		public bool IsWaitMenu { get; private set; }

		// Token: 0x170005DE RID: 1502
		// (get) Token: 0x06001532 RID: 5426 RVA: 0x0006104E File Offset: 0x0005F24E
		// (set) Token: 0x06001533 RID: 5427 RVA: 0x00061056 File Offset: 0x0005F256
		public bool IsWaitActive { get; private set; }

		// Token: 0x170005DF RID: 1503
		// (get) Token: 0x06001534 RID: 5428 RVA: 0x0006105F File Offset: 0x0005F25F
		public bool IsEmpty
		{
			get
			{
				return this.MenuRepeatObjects.Count == 0 && this.MenuItemAmount == 0;
			}
		}

		// Token: 0x170005E0 RID: 1504
		// (get) Token: 0x06001535 RID: 5429 RVA: 0x00061079 File Offset: 0x0005F279
		// (set) Token: 0x06001536 RID: 5430 RVA: 0x00061081 File Offset: 0x0005F281
		public float Progress { get; private set; }

		// Token: 0x170005E1 RID: 1505
		// (get) Token: 0x06001537 RID: 5431 RVA: 0x0006108A File Offset: 0x0005F28A
		// (set) Token: 0x06001538 RID: 5432 RVA: 0x00061092 File Offset: 0x0005F292
		public float TargetWaitHours { get; private set; }

		// Token: 0x170005E2 RID: 1506
		// (get) Token: 0x06001539 RID: 5433 RVA: 0x0006109B File Offset: 0x0005F29B
		// (set) Token: 0x0600153A RID: 5434 RVA: 0x000610A3 File Offset: 0x0005F2A3
		public OnTickDelegate OnTick { get; private set; }

		// Token: 0x170005E3 RID: 1507
		// (get) Token: 0x0600153B RID: 5435 RVA: 0x000610AC File Offset: 0x0005F2AC
		// (set) Token: 0x0600153C RID: 5436 RVA: 0x000610B4 File Offset: 0x0005F2B4
		public OnConditionDelegate OnCondition { get; private set; }

		// Token: 0x170005E4 RID: 1508
		// (get) Token: 0x0600153D RID: 5437 RVA: 0x000610BD File Offset: 0x0005F2BD
		// (set) Token: 0x0600153E RID: 5438 RVA: 0x000610C5 File Offset: 0x0005F2C5
		public OnConsequenceDelegate OnConsequence { get; private set; }

		// Token: 0x170005E5 RID: 1509
		// (get) Token: 0x0600153F RID: 5439 RVA: 0x000610CE File Offset: 0x0005F2CE
		// (set) Token: 0x06001540 RID: 5440 RVA: 0x000610D6 File Offset: 0x0005F2D6
		public int CurrentRepeatableIndex { get; set; }

		// Token: 0x170005E6 RID: 1510
		// (get) Token: 0x06001541 RID: 5441 RVA: 0x000610DF File Offset: 0x0005F2DF
		public IEnumerable<GameMenuOption> MenuOptions
		{
			get
			{
				return this._menuItems;
			}
		}

		// Token: 0x06001542 RID: 5442 RVA: 0x000610E7 File Offset: 0x0005F2E7
		internal GameMenu(string idString)
		{
			this.StringId = idString;
			this._menuItems = new List<GameMenuOption>();
		}

		// Token: 0x06001543 RID: 5443 RVA: 0x0006110C File Offset: 0x0005F30C
		internal void Initialize(TextObject text, OnInitDelegate initDelegate, GameMenu.MenuOverlayType overlay, GameMenu.MenuFlags flags = GameMenu.MenuFlags.None, object relatedObject = null)
		{
			this.CurrentRepeatableIndex = 0;
			this.LastSelectedMenuObject = null;
			this._defaultText = text;
			this.OnInit = initDelegate;
			this.OverlayType = overlay;
			this.AutoSelectFirst = (flags & GameMenu.MenuFlags.AutoSelectFirst) > GameMenu.MenuFlags.None;
			this.RelatedObject = relatedObject;
			this.IsReady = true;
		}

		// Token: 0x06001544 RID: 5444 RVA: 0x00061158 File Offset: 0x0005F358
		internal void Initialize(TextObject text, OnInitDelegate initDelegate, OnConditionDelegate condition, OnConsequenceDelegate consequence, OnTickDelegate tick, GameMenu.MenuAndOptionType type, GameMenu.MenuOverlayType overlay, float targetWaitHours = 0f, GameMenu.MenuFlags flags = GameMenu.MenuFlags.None, object relatedObject = null)
		{
			this.CurrentRepeatableIndex = 0;
			this.LastSelectedMenuObject = null;
			this._defaultText = text;
			this.OnInit = initDelegate;
			this.OverlayType = overlay;
			this.AutoSelectFirst = (flags & GameMenu.MenuFlags.AutoSelectFirst) > GameMenu.MenuFlags.None;
			this.RelatedObject = relatedObject;
			this.OnConsequence = consequence;
			this.OnCondition = condition;
			this.Type = type;
			this.OnTick = tick;
			this.TargetWaitHours = targetWaitHours;
			this.IsWaitMenu = type > GameMenu.MenuAndOptionType.RegularMenuOption;
			this.IsReady = true;
		}

		// Token: 0x06001545 RID: 5445 RVA: 0x000611D7 File Offset: 0x0005F3D7
		public void SetMenuRepeatObjects(IEnumerable<object> list)
		{
			this.MenuRepeatObjects = list.ToList<object>();
		}

		// Token: 0x06001546 RID: 5446 RVA: 0x000611E5 File Offset: 0x0005F3E5
		private void AddOption(GameMenuOption newOption, int index = -1)
		{
			if (index >= 0 && this._menuItems.Count >= index)
			{
				this._menuItems.Insert(index, newOption);
				return;
			}
			this._menuItems.Add(newOption);
		}

		// Token: 0x06001547 RID: 5447 RVA: 0x00061213 File Offset: 0x0005F413
		public bool GetMenuOptionConditionsHold(Game game, MenuContext menuContext, int menuItemNumber)
		{
			if (this.IsWaitMenu)
			{
				return this._menuItems[menuItemNumber].GetConditionsHold(game, menuContext) && this.RunWaitMenuCondition(menuContext);
			}
			return this._menuItems[menuItemNumber].GetConditionsHold(game, menuContext);
		}

		// Token: 0x06001548 RID: 5448 RVA: 0x0006124F File Offset: 0x0005F44F
		public TextObject GetMenuOptionText(int menuItemNumber)
		{
			return this._menuItems[menuItemNumber].Text;
		}

		// Token: 0x06001549 RID: 5449 RVA: 0x00061262 File Offset: 0x0005F462
		public GameMenuOption GetGameMenuOption(int menuItemNumber)
		{
			return this._menuItems[menuItemNumber];
		}

		// Token: 0x0600154A RID: 5450 RVA: 0x00061270 File Offset: 0x0005F470
		public TextObject GetMenuOptionText2(int menuItemNumber)
		{
			return this._menuItems[menuItemNumber].Text2;
		}

		// Token: 0x0600154B RID: 5451 RVA: 0x00061283 File Offset: 0x0005F483
		public string GetMenuOptionIdString(int menuItemNumber)
		{
			return this._menuItems[menuItemNumber].IdString;
		}

		// Token: 0x0600154C RID: 5452 RVA: 0x00061296 File Offset: 0x0005F496
		public TextObject GetMenuOptionTooltip(int menuItemNumber)
		{
			return this._menuItems[menuItemNumber].Tooltip;
		}

		// Token: 0x0600154D RID: 5453 RVA: 0x000612A9 File Offset: 0x0005F4A9
		public bool GetMenuOptionIsLeave(int menuItemNumber)
		{
			return this._menuItems[menuItemNumber].IsLeave;
		}

		// Token: 0x0600154E RID: 5454 RVA: 0x000612BC File Offset: 0x0005F4BC
		public void SetProgressOfWaitingInMenu(float progress)
		{
			this.Progress = progress;
		}

		// Token: 0x0600154F RID: 5455 RVA: 0x000612C5 File Offset: 0x0005F4C5
		public void SetTargetedWaitingTimeAndInitialProgress(float targetedWaitingTime, float initialProgress)
		{
			this.TargetWaitHours = targetedWaitingTime;
			this.SetProgressOfWaitingInMenu(initialProgress);
		}

		// Token: 0x06001550 RID: 5456 RVA: 0x000612D8 File Offset: 0x0005F4D8
		public GameMenuOption GetLeaveMenuOption(Game game, MenuContext menuContext)
		{
			for (int i = 0; i < this._menuItems.Count; i++)
			{
				if (this._menuItems[i].IsLeave && this._menuItems[i].IsEnabled && this._menuItems[i].GetConditionsHold(game, menuContext))
				{
					return this._menuItems[i];
				}
			}
			return null;
		}

		// Token: 0x06001551 RID: 5457 RVA: 0x00061344 File Offset: 0x0005F544
		public void RunOnTick(MenuContext menuContext, float dt)
		{
			if (this.IsWaitMenu && this.IsWaitActive)
			{
				if (this.OnTick != null)
				{
					MenuCallbackArgs menuCallbackArgs = new MenuCallbackArgs(menuContext, this.MenuTitle);
					this.OnTick(menuCallbackArgs, CampaignTime.Now - this._previousTickTime);
					this._previousTickTime = CampaignTime.Now;
				}
				if (this.Progress >= 1f)
				{
					this.EndWait();
					this.RunWaitMenuConsequence(menuContext);
				}
			}
		}

		// Token: 0x06001552 RID: 5458 RVA: 0x000613B8 File Offset: 0x0005F5B8
		public bool RunWaitMenuCondition(MenuContext menuContext)
		{
			if (this.OnCondition != null)
			{
				MenuCallbackArgs menuCallbackArgs = new MenuCallbackArgs(menuContext, this.MenuTitle);
				bool flag = this.OnCondition(menuCallbackArgs);
				if (flag && !this.IsWaitActive)
				{
					menuContext.GameMenu.StartWait();
				}
				return flag;
			}
			return true;
		}

		// Token: 0x06001553 RID: 5459 RVA: 0x00061400 File Offset: 0x0005F600
		public void RunWaitMenuConsequence(MenuContext menuContext)
		{
			if (this.OnConsequence != null)
			{
				MenuCallbackArgs menuCallbackArgs = new MenuCallbackArgs(menuContext, this.MenuTitle);
				this.OnConsequence(menuCallbackArgs);
			}
		}

		// Token: 0x06001554 RID: 5460 RVA: 0x00061430 File Offset: 0x0005F630
		public void RunMenuOptionConsequence(MenuContext menuContext, int menuItemNumber)
		{
			if (menuItemNumber >= this._menuItems.Count || menuItemNumber < 0)
			{
				Debug.FailedAssert("menuItemNumber out of bounds", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\GameMenus\\GameMenu.cs", "RunMenuOptionConsequence", 263);
				menuItemNumber = this._menuItems.Count - 1;
			}
			GameMenuOption gameMenuOption = this._menuItems[menuItemNumber];
			if (gameMenuOption.IsLeave && this.IsWaitMenu)
			{
				this.EndWait();
			}
			gameMenuOption.RunConsequence(menuContext);
			if (Campaign.Current != null)
			{
				CampaignEventDispatcher.Instance.OnGameMenuOptionSelected(this, gameMenuOption);
			}
		}

		// Token: 0x06001555 RID: 5461 RVA: 0x000614B4 File Offset: 0x0005F6B4
		public void StartWait()
		{
			this._previousTickTime = CampaignTime.Now;
			this.IsWaitActive = true;
			Campaign.Current.TimeControlMode = CampaignTimeControlMode.UnstoppableFastForward;
		}

		// Token: 0x06001556 RID: 5462 RVA: 0x000614D3 File Offset: 0x0005F6D3
		public void EndWait()
		{
			this.IsWaitActive = false;
			Campaign.Current.TimeControlMode = CampaignTimeControlMode.Stop;
		}

		// Token: 0x06001557 RID: 5463 RVA: 0x000614E7 File Offset: 0x0005F6E7
		private void ResetVariablesOnInit()
		{
			this.Progress = 0f;
			this.CurrentRepeatableIndex = 0;
			this.MenuRepeatObjects.Clear();
		}

		// Token: 0x06001558 RID: 5464 RVA: 0x00061508 File Offset: 0x0005F708
		public void RunOnInit(Game game, MenuContext menuContext)
		{
			this.ResetVariablesOnInit();
			MenuCallbackArgs menuCallbackArgs = new MenuCallbackArgs(menuContext, this.MenuTitle);
			if (this.OnInit != null)
			{
				Debug.Print("[GAME MENU] " + menuContext.GameMenu.StringId, 0, Debug.DebugColor.White, 17592186044416UL);
				this.OnInit(menuCallbackArgs);
				this.MenuTitle = menuCallbackArgs.MenuTitle;
			}
			CampaignEventDispatcher.Instance.OnGameMenuOpened(menuCallbackArgs);
		}

		// Token: 0x06001559 RID: 5465 RVA: 0x0006157C File Offset: 0x0005F77C
		public void PreInit(MenuContext menuContext)
		{
			MenuCallbackArgs menuCallbackArgs = new MenuCallbackArgs(menuContext, this.MenuTitle);
			CampaignEventDispatcher.Instance.BeforeGameMenuOpened(menuCallbackArgs);
		}

		// Token: 0x0600155A RID: 5466 RVA: 0x000615A4 File Offset: 0x0005F7A4
		public void AfterInit(MenuContext menuContext)
		{
			MenuCallbackArgs menuCallbackArgs = new MenuCallbackArgs(menuContext, this.MenuTitle);
			CampaignEventDispatcher.Instance.AfterGameMenuInitialized(menuCallbackArgs);
		}

		// Token: 0x0600155B RID: 5467 RVA: 0x000615C9 File Offset: 0x0005F7C9
		public TextObject GetText()
		{
			return this._defaultText;
		}

		// Token: 0x170005E7 RID: 1511
		// (get) Token: 0x0600155C RID: 5468 RVA: 0x000615D1 File Offset: 0x0005F7D1
		// (set) Token: 0x0600155D RID: 5469 RVA: 0x000615D9 File Offset: 0x0005F7D9
		public bool AutoSelectFirst { get; private set; }

		// Token: 0x0600155E RID: 5470 RVA: 0x000615E4 File Offset: 0x0005F7E4
		public static void ActivateGameMenu(string menuId)
		{
			Campaign.Current.TimeControlMode = CampaignTimeControlMode.Stop;
			if (Campaign.Current.CurrentMenuContext == null)
			{
				Campaign.Current.GameMenuManager.SetNextMenu(menuId);
				MapState mapState = Game.Current.GameStateManager.LastOrDefault<MapState>();
				if (mapState != null)
				{
					mapState.EnterMenuMode();
				}
				bool flag;
				if (mapState == null)
				{
					flag = null != null;
				}
				else
				{
					MenuContext menuContext = mapState.MenuContext;
					flag = ((menuContext != null) ? menuContext.GameMenu : null) != null;
				}
				if (flag)
				{
					GameMenu gameMenu = mapState.MenuContext.GameMenu;
					if (gameMenu != null && gameMenu.IsWaitMenu)
					{
						mapState.MenuContext.GameMenu.StartWait();
						return;
					}
				}
			}
			else
			{
				GameMenu.SwitchToMenu(menuId);
			}
		}

		// Token: 0x0600155F RID: 5471 RVA: 0x0006167C File Offset: 0x0005F87C
		public static void SwitchToMenu(string menuId)
		{
			Campaign.Current.TimeControlMode = CampaignTimeControlMode.Stop;
			MenuContext currentMenuContext = Campaign.Current.CurrentMenuContext;
			if (currentMenuContext != null)
			{
				currentMenuContext.SwitchToMenu(menuId);
				if (currentMenuContext.GameMenu.IsWaitMenu && Campaign.Current.TimeControlMode == CampaignTimeControlMode.Stop)
				{
					currentMenuContext.GameMenu.StartWait();
					return;
				}
			}
			else
			{
				Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\GameMenus\\GameMenu.cs", "SwitchToMenu", 384);
			}
		}

		// Token: 0x06001560 RID: 5472 RVA: 0x000616E7 File Offset: 0x0005F8E7
		public static void ExitToLast()
		{
			Campaign.Current.TimeControlMode = CampaignTimeControlMode.Stop;
			Campaign.Current.GameMenuManager.ExitToLast();
		}

		// Token: 0x06001561 RID: 5473 RVA: 0x00061704 File Offset: 0x0005F904
		internal void AddOption(string optionId, TextObject optionText, GameMenuOption.OnConditionDelegate condition, GameMenuOption.OnConsequenceDelegate consequence, int index = -1, bool isLeave = false, bool isRepeatable = false, object relatedObject = null)
		{
			this.AddOption(new GameMenuOption(GameMenu.MenuAndOptionType.RegularMenuOption, optionId, optionText, optionText, condition, consequence, isLeave, isRepeatable, relatedObject), index);
		}

		// Token: 0x06001562 RID: 5474 RVA: 0x0006172B File Offset: 0x0005F92B
		internal void RemoveMenuOption(GameMenuOption option)
		{
			this._menuItems.Remove(option);
		}

		// Token: 0x040006FE RID: 1790
		private TextObject _defaultText;

		// Token: 0x04000704 RID: 1796
		public OnInitDelegate OnInit;

		// Token: 0x04000707 RID: 1799
		public object LastSelectedMenuObject;

		// Token: 0x0400070F RID: 1807
		private CampaignTime _previousTickTime;

		// Token: 0x04000710 RID: 1808
		private readonly List<GameMenuOption> _menuItems;

		// Token: 0x02000561 RID: 1377
		public enum MenuOverlayType
		{
			// Token: 0x040016F8 RID: 5880
			None,
			// Token: 0x040016F9 RID: 5881
			SettlementWithParties,
			// Token: 0x040016FA RID: 5882
			SettlementWithCharacters,
			// Token: 0x040016FB RID: 5883
			SettlementWithBoth,
			// Token: 0x040016FC RID: 5884
			Encounter
		}

		// Token: 0x02000562 RID: 1378
		public enum MenuFlags
		{
			// Token: 0x040016FE RID: 5886
			None,
			// Token: 0x040016FF RID: 5887
			AutoSelectFirst
		}

		// Token: 0x02000563 RID: 1379
		public enum MenuAndOptionType
		{
			// Token: 0x04001701 RID: 5889
			RegularMenuOption,
			// Token: 0x04001702 RID: 5890
			WaitMenuShowProgressAndHoursOption,
			// Token: 0x04001703 RID: 5891
			WaitMenuShowOnlyProgressOption,
			// Token: 0x04001704 RID: 5892
			WaitMenuHideProgressAndHoursOption
		}
	}
}
