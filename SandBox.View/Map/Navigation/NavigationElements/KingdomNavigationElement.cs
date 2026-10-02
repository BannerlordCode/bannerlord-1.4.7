using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.ScreenSystem;

namespace SandBox.View.Map.Navigation.NavigationElements
{
	// Token: 0x0200006E RID: 110
	public class KingdomNavigationElement : MapNavigationElementBase
	{
		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060004B6 RID: 1206 RVA: 0x00024FCE File Offset: 0x000231CE
		public override string StringId
		{
			get
			{
				return "kingdom";
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060004B7 RID: 1207 RVA: 0x00024FD5 File Offset: 0x000231D5
		public override bool IsActive
		{
			get
			{
				return base._game.GameStateManager.ActiveState is KingdomState;
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060004B8 RID: 1208 RVA: 0x00024FEF File Offset: 0x000231EF
		public override bool IsLockingNavigation
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060004B9 RID: 1209 RVA: 0x00024FF2 File Offset: 0x000231F2
		public override bool HasAlert
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x00024FF5 File Offset: 0x000231F5
		public KingdomNavigationElement(MapNavigationHandler handler)
			: base(handler)
		{
			this._needToBeInKingdomText = GameTexts.FindText("str_need_to_be_a_part_of_kingdom", null);
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x00025010 File Offset: 0x00023210
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
			if (!Hero.MainHero.MapFaction.IsKingdomFaction)
			{
				return new NavigationPermissionItem(false, this._needToBeInKingdomText);
			}
			Mission mission = Mission.Current;
			if (mission != null && !mission.IsKingdomWindowAccessAllowed)
			{
				return new NavigationPermissionItem(false, null);
			}
			return new NavigationPermissionItem(true, null);
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x00025088 File Offset: 0x00023288
		protected override TextObject GetTooltip()
		{
			if (!Input.IsGamepadActive && (base.Permission.IsAuthorized || this.IsActive))
			{
				string text = Game.Current.GameTextManager.GetHotKeyGameText("GenericCampaignPanelsGameKeyCategory", 40).ToString();
				TextObject textObject = GameTexts.FindText("str_hotkey_with_hint", null);
				textObject.SetTextVariable("TEXT", GameTexts.FindText("str_kingdom", null).ToString());
				textObject.SetTextVariable("HOTKEY", text);
				return textObject;
			}
			return GameTexts.FindText("str_kingdom", null);
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x00025110 File Offset: 0x00023310
		protected override TextObject GetAlertTooltip()
		{
			return TextObject.GetEmpty();
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x00025117 File Offset: 0x00023317
		public override void OpenView()
		{
			this.PrepareToOpenKingdomScreen(delegate
			{
				this.OpenKingdomAction();
			});
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x0002512C File Offset: 0x0002332C
		public override void OpenView(params object[] parameters)
		{
			if (parameters.Length != 0)
			{
				KingdomNavigationElement.<>c__DisplayClass14_0 CS$<>8__locals1 = new KingdomNavigationElement.<>c__DisplayClass14_0();
				CS$<>8__locals1.<>4__this = this;
				object obj = parameters[0];
				if ((CS$<>8__locals1.army = obj as Army) != null)
				{
					this.PrepareToOpenKingdomScreen(delegate
					{
						CS$<>8__locals1.<>4__this.OpenKingdomAction(CS$<>8__locals1.army);
					});
					return;
				}
				KingdomNavigationElement.<>c__DisplayClass14_1 CS$<>8__locals2 = new KingdomNavigationElement.<>c__DisplayClass14_1();
				CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
				if ((CS$<>8__locals2.settlement = obj as Settlement) != null)
				{
					this.PrepareToOpenKingdomScreen(delegate
					{
						CS$<>8__locals2.CS$<>8__locals1.<>4__this.OpenKingdomAction(CS$<>8__locals2.settlement);
					});
					return;
				}
				KingdomNavigationElement.<>c__DisplayClass14_2 CS$<>8__locals3 = new KingdomNavigationElement.<>c__DisplayClass14_2();
				CS$<>8__locals3.CS$<>8__locals2 = CS$<>8__locals2;
				if ((CS$<>8__locals3.clan = obj as Clan) != null)
				{
					this.PrepareToOpenKingdomScreen(delegate
					{
						CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>4__this.OpenKingdomAction(CS$<>8__locals3.clan);
					});
					return;
				}
				KingdomNavigationElement.<>c__DisplayClass14_3 CS$<>8__locals4 = new KingdomNavigationElement.<>c__DisplayClass14_3();
				CS$<>8__locals4.CS$<>8__locals3 = CS$<>8__locals3;
				if ((CS$<>8__locals4.policy = obj as PolicyObject) != null)
				{
					this.PrepareToOpenKingdomScreen(delegate
					{
						CS$<>8__locals4.CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>4__this.OpenKingdomAction(CS$<>8__locals4.policy);
					});
					return;
				}
				KingdomNavigationElement.<>c__DisplayClass14_4 CS$<>8__locals5 = new KingdomNavigationElement.<>c__DisplayClass14_4();
				CS$<>8__locals5.CS$<>8__locals4 = CS$<>8__locals4;
				if ((CS$<>8__locals5.faction = obj as IFaction) != null)
				{
					this.PrepareToOpenKingdomScreen(delegate
					{
						CS$<>8__locals5.CS$<>8__locals4.CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>4__this.OpenKingdomAction(CS$<>8__locals5.faction);
					});
					return;
				}
				KingdomDecision decision;
				if ((decision = obj as KingdomDecision) != null)
				{
					this.PrepareToOpenKingdomScreen(delegate
					{
						CS$<>8__locals5.CS$<>8__locals4.CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>4__this.OpenKingdomAction(decision);
					});
					return;
				}
				Debug.FailedAssert(string.Format("Invalid parameter type when opening the kingdom screen from navigation: {0}", obj.GetType()), "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox.View\\Map\\Navigation\\NavigationElements\\KindomNavigationElement.cs", "OpenView", 113);
			}
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x000252A8 File Offset: 0x000234A8
		public override void GoToLink()
		{
			Campaign.Current.EncyclopediaManager.GoToLink(Hero.MainHero.MapFaction.EncyclopediaLink);
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x000252C8 File Offset: 0x000234C8
		private void PrepareToOpenKingdomScreen(Action openKingdomAction)
		{
			if (base.Permission.IsAuthorized)
			{
				IChangeableScreen changeableScreen;
				if ((changeableScreen = ScreenManager.TopScreen as IChangeableScreen) != null && changeableScreen.AnyUnsavedChanges())
				{
					InformationManager.ShowInquiry(changeableScreen.CanChangesBeApplied() ? MapNavigationHelper.GetUnsavedChangedInquiry(openKingdomAction) : MapNavigationHelper.GetUnapplicableChangedInquiry(), false, false);
					return;
				}
				MapNavigationHelper.SwitchToANewScreen(openKingdomAction);
			}
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x00025320 File Offset: 0x00023520
		private void OpenKingdomAction()
		{
			KingdomState kingdomState = base._game.GameStateManager.CreateState<KingdomState>();
			base._game.GameStateManager.PushState(kingdomState, 0);
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x00025350 File Offset: 0x00023550
		private void OpenKingdomAction(Army army)
		{
			KingdomState kingdomState = base._game.GameStateManager.CreateState<KingdomState>(new object[] { army });
			base._game.GameStateManager.PushState(kingdomState, 0);
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x0002538C File Offset: 0x0002358C
		private void OpenKingdomAction(Settlement settlement)
		{
			KingdomState kingdomState = base._game.GameStateManager.CreateState<KingdomState>(new object[] { settlement });
			base._game.GameStateManager.PushState(kingdomState, 0);
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x000253C8 File Offset: 0x000235C8
		private void OpenKingdomAction(Clan clan)
		{
			KingdomState kingdomState = base._game.GameStateManager.CreateState<KingdomState>(new object[] { clan });
			base._game.GameStateManager.PushState(kingdomState, 0);
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x00025404 File Offset: 0x00023604
		private void OpenKingdomAction(PolicyObject policy)
		{
			KingdomState kingdomState = base._game.GameStateManager.CreateState<KingdomState>(new object[] { policy });
			base._game.GameStateManager.PushState(kingdomState, 0);
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x00025440 File Offset: 0x00023640
		private void OpenKingdomAction(IFaction faction)
		{
			KingdomState kingdomState = base._game.GameStateManager.CreateState<KingdomState>(new object[] { faction });
			base._game.GameStateManager.PushState(kingdomState, 0);
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x0002547C File Offset: 0x0002367C
		private void OpenKingdomAction(KingdomDecision decision)
		{
			KingdomState kingdomState = base._game.GameStateManager.CreateState<KingdomState>(new object[] { decision });
			base._game.GameStateManager.PushState(kingdomState, 0);
		}

		// Token: 0x04000224 RID: 548
		private readonly TextObject _needToBeInKingdomText;
	}
}
