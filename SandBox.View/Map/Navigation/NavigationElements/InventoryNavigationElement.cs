using System;
using Helpers;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.ScreenSystem;

namespace SandBox.View.Map.Navigation.NavigationElements
{
	// Token: 0x0200006D RID: 109
	public class InventoryNavigationElement : MapNavigationElementBase
	{
		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060004AB RID: 1195 RVA: 0x00024DA8 File Offset: 0x00022FA8
		public override string StringId
		{
			get
			{
				return "inventory";
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060004AC RID: 1196 RVA: 0x00024DAF File Offset: 0x00022FAF
		public override bool IsActive
		{
			get
			{
				return base._game.GameStateManager.ActiveState is InventoryState;
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060004AD RID: 1197 RVA: 0x00024DCC File Offset: 0x00022FCC
		public override bool IsLockingNavigation
		{
			get
			{
				GameStateManager gameStateManager = GameStateManager.Current;
				InventoryState inventoryState;
				return (inventoryState = ((gameStateManager != null) ? gameStateManager.ActiveState : null) as InventoryState) != null && inventoryState.InventoryLogic != null && inventoryState.InventoryMode != InventoryScreenHelper.InventoryMode.Default;
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060004AE RID: 1198 RVA: 0x00024E06 File Offset: 0x00023006
		public override bool HasAlert
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x00024E09 File Offset: 0x00023009
		public InventoryNavigationElement(MapNavigationHandler handler)
			: base(handler)
		{
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x00024E14 File Offset: 0x00023014
		protected override NavigationPermissionItem GetPermission()
		{
			if (!MapNavigationHelper.IsNavigationBarEnabled(this._handler))
			{
				return new NavigationPermissionItem(false, null);
			}
			if (this.IsActive)
			{
				return new NavigationPermissionItem(false, null);
			}
			if (MobileParty.MainParty.IsInRaftState || Hero.MainHero.HeroState == Hero.CharacterStates.Prisoner)
			{
				return new NavigationPermissionItem(false, null);
			}
			Mission mission = Mission.Current;
			if (mission != null && !mission.IsInventoryAccessAllowed)
			{
				return new NavigationPermissionItem(false, null);
			}
			return new NavigationPermissionItem(true, null);
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x00024E8C File Offset: 0x0002308C
		protected override TextObject GetTooltip()
		{
			if (!Input.IsGamepadActive && (base.Permission.IsAuthorized || this.IsActive))
			{
				string text = Game.Current.GameTextManager.GetHotKeyGameText("GenericCampaignPanelsGameKeyCategory", 38).ToString();
				TextObject textObject = GameTexts.FindText("str_hotkey_with_hint", null);
				textObject.SetTextVariable("TEXT", GameTexts.FindText("str_inventory", null).ToString());
				textObject.SetTextVariable("HOTKEY", text);
				return textObject;
			}
			return GameTexts.FindText("str_inventory", null);
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x00024F14 File Offset: 0x00023114
		protected override TextObject GetAlertTooltip()
		{
			return TextObject.GetEmpty();
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x00024F1C File Offset: 0x0002311C
		public override void OpenView()
		{
			if (base.Permission.IsAuthorized)
			{
				IChangeableScreen changeableScreen;
				if ((changeableScreen = ScreenManager.TopScreen as IChangeableScreen) != null && changeableScreen.AnyUnsavedChanges())
				{
					InquiryData inquiryData;
					if (!changeableScreen.CanChangesBeApplied())
					{
						inquiryData = MapNavigationHelper.GetUnapplicableChangedInquiry();
					}
					else
					{
						inquiryData = MapNavigationHelper.GetUnsavedChangedInquiry(delegate
						{
							InventoryScreenHelper.OpenScreenAsInventory(null);
						});
					}
					InformationManager.ShowInquiry(inquiryData, false, false);
					return;
				}
				MapNavigationHelper.SwitchToANewScreen(delegate
				{
					InventoryScreenHelper.OpenScreenAsInventory(null);
				});
			}
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x00024FAE File Offset: 0x000231AE
		public override void OpenView(params object[] parameters)
		{
			Debug.FailedAssert("Inventory screen shouldn't be opened with parameters from navigation", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox.View\\Map\\Navigation\\NavigationElements\\InventoryNavigationElement.cs", "OpenView", 106);
			this.OpenView();
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x00024FCC File Offset: 0x000231CC
		public override void GoToLink()
		{
		}
	}
}
