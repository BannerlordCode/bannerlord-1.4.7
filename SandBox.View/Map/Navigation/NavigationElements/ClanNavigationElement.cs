using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Workshops;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.ScreenSystem;

namespace SandBox.View.Map.Navigation.NavigationElements
{
	// Token: 0x0200006A RID: 106
	public class ClanNavigationElement : MapNavigationElementBase
	{
		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000489 RID: 1161 RVA: 0x00024762 File Offset: 0x00022962
		public override string StringId
		{
			get
			{
				return "clan";
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x0600048A RID: 1162 RVA: 0x00024769 File Offset: 0x00022969
		public override bool IsActive
		{
			get
			{
				return base._game.GameStateManager.ActiveState is ClanState;
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x0600048B RID: 1163 RVA: 0x00024783 File Offset: 0x00022983
		public override bool IsLockingNavigation
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x0600048C RID: 1164 RVA: 0x00024786 File Offset: 0x00022986
		public override bool HasAlert
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x00024789 File Offset: 0x00022989
		public ClanNavigationElement(MapNavigationHandler handler)
			: base(handler)
		{
			this._clanScreenPermissionEvent = new ClanScreenPermissionEvent(new Action<bool, TextObject>(this.OnClanScreenPermission));
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x000247AC File Offset: 0x000229AC
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
			Mission mission = Mission.Current;
			if (mission != null && !mission.IsClanWindowAccessAllowed)
			{
				return new NavigationPermissionItem(false, null);
			}
			this._mostRecentClanScreenPermission = null;
			Game.Current.EventManager.TriggerEvent<ClanScreenPermissionEvent>(this._clanScreenPermissionEvent);
			NavigationPermissionItem? mostRecentClanScreenPermission = this._mostRecentClanScreenPermission;
			if (mostRecentClanScreenPermission == null)
			{
				return new NavigationPermissionItem(true, null);
			}
			return mostRecentClanScreenPermission.GetValueOrDefault();
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x0002483C File Offset: 0x00022A3C
		protected override TextObject GetTooltip()
		{
			if (!Input.IsGamepadActive && (base.Permission.IsAuthorized || this.IsActive))
			{
				string text = Game.Current.GameTextManager.GetHotKeyGameText("GenericCampaignPanelsGameKeyCategory", 41).ToString();
				TextObject textObject = GameTexts.FindText("str_hotkey_with_hint", null);
				textObject.SetTextVariable("TEXT", GameTexts.FindText("str_clan", null).ToString());
				textObject.SetTextVariable("HOTKEY", text);
				return textObject;
			}
			return GameTexts.FindText("str_clan", null);
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x000248C4 File Offset: 0x00022AC4
		protected override TextObject GetAlertTooltip()
		{
			return TextObject.GetEmpty();
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x000248CB File Offset: 0x00022ACB
		public override void OpenView()
		{
			this.PrepareToOpenClanScreen(delegate
			{
				this.OpenClanScreenAction();
			});
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x000248E0 File Offset: 0x00022AE0
		public override void OpenView(params object[] parameters)
		{
			if (parameters.Length != 0)
			{
				ClanNavigationElement.<>c__DisplayClass15_0 CS$<>8__locals1 = new ClanNavigationElement.<>c__DisplayClass15_0();
				CS$<>8__locals1.<>4__this = this;
				object obj = parameters[0];
				if ((CS$<>8__locals1.hero = obj as Hero) != null)
				{
					this.PrepareToOpenClanScreen(delegate
					{
						CS$<>8__locals1.<>4__this.OpenClanScreenAction(CS$<>8__locals1.hero);
					});
					return;
				}
				ClanNavigationElement.<>c__DisplayClass15_1 CS$<>8__locals2 = new ClanNavigationElement.<>c__DisplayClass15_1();
				CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
				if ((CS$<>8__locals2.party = obj as PartyBase) != null)
				{
					this.PrepareToOpenClanScreen(delegate
					{
						CS$<>8__locals2.CS$<>8__locals1.<>4__this.OpenClanScreenAction(CS$<>8__locals2.party);
					});
					return;
				}
				ClanNavigationElement.<>c__DisplayClass15_2 CS$<>8__locals3 = new ClanNavigationElement.<>c__DisplayClass15_2();
				CS$<>8__locals3.CS$<>8__locals2 = CS$<>8__locals2;
				if ((CS$<>8__locals3.settlement = obj as Settlement) != null)
				{
					this.PrepareToOpenClanScreen(delegate
					{
						CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>4__this.OpenClanScreenAction(CS$<>8__locals3.settlement);
					});
					return;
				}
				ClanNavigationElement.<>c__DisplayClass15_3 CS$<>8__locals4 = new ClanNavigationElement.<>c__DisplayClass15_3();
				CS$<>8__locals4.CS$<>8__locals3 = CS$<>8__locals3;
				if ((CS$<>8__locals4.workshop = obj as Workshop) != null)
				{
					this.PrepareToOpenClanScreen(delegate
					{
						CS$<>8__locals4.CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>4__this.OpenClanScreenAction(CS$<>8__locals4.workshop);
					});
					return;
				}
				Alley alley;
				if ((alley = obj as Alley) != null)
				{
					this.PrepareToOpenClanScreen(delegate
					{
						CS$<>8__locals4.CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>4__this.OpenClanScreenAction(alley);
					});
					return;
				}
				Debug.FailedAssert(string.Format("Invalid parameter type when opening the clan screen from navigation: {0}", obj.GetType()), "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox.View\\Map\\Navigation\\NavigationElements\\ClanNavigationElement.cs", "OpenView", 110);
			}
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x00024A24 File Offset: 0x00022C24
		public override void GoToLink()
		{
			Campaign.Current.EncyclopediaManager.GoToLink(Hero.MainHero.Clan.EncyclopediaLink);
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x00024A44 File Offset: 0x00022C44
		public void OnClanScreenPermission(bool isAvailable, TextObject reasonString)
		{
			if (!isAvailable)
			{
				this._mostRecentClanScreenPermission = new NavigationPermissionItem?(new NavigationPermissionItem(isAvailable, reasonString));
			}
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x00024A5C File Offset: 0x00022C5C
		private void PrepareToOpenClanScreen(Action openClanScreenAction)
		{
			if (base.Permission.IsAuthorized)
			{
				IChangeableScreen changeableScreen;
				if ((changeableScreen = ScreenManager.TopScreen as IChangeableScreen) != null && changeableScreen.AnyUnsavedChanges())
				{
					InformationManager.ShowInquiry(changeableScreen.CanChangesBeApplied() ? MapNavigationHelper.GetUnsavedChangedInquiry(openClanScreenAction) : MapNavigationHelper.GetUnapplicableChangedInquiry(), false, false);
					return;
				}
				MapNavigationHelper.SwitchToANewScreen(openClanScreenAction);
			}
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x00024AB4 File Offset: 0x00022CB4
		private void OpenClanScreenAction()
		{
			ClanState clanState = base._game.GameStateManager.CreateState<ClanState>();
			base._game.GameStateManager.PushState(clanState, 0);
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x00024AE4 File Offset: 0x00022CE4
		private void OpenClanScreenAction(Hero hero)
		{
			ClanState clanState = base._game.GameStateManager.CreateState<ClanState>(new object[] { hero });
			base._game.GameStateManager.PushState(clanState, 0);
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x00024B20 File Offset: 0x00022D20
		private void OpenClanScreenAction(PartyBase party)
		{
			ClanState clanState = base._game.GameStateManager.CreateState<ClanState>(new object[] { party });
			base._game.GameStateManager.PushState(clanState, 0);
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x00024B5C File Offset: 0x00022D5C
		private void OpenClanScreenAction(Settlement settlement)
		{
			ClanState clanState = base._game.GameStateManager.CreateState<ClanState>(new object[] { settlement });
			base._game.GameStateManager.PushState(clanState, 0);
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x00024B98 File Offset: 0x00022D98
		private void OpenClanScreenAction(Workshop workshop)
		{
			ClanState clanState = base._game.GameStateManager.CreateState<ClanState>(new object[] { workshop });
			base._game.GameStateManager.PushState(clanState, 0);
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x00024BD4 File Offset: 0x00022DD4
		private void OpenClanScreenAction(Alley alley)
		{
			ClanState clanState = base._game.GameStateManager.CreateState<ClanState>(new object[] { alley });
			base._game.GameStateManager.PushState(clanState, 0);
		}

		// Token: 0x04000221 RID: 545
		private readonly ClanScreenPermissionEvent _clanScreenPermissionEvent;

		// Token: 0x04000222 RID: 546
		private NavigationPermissionItem? _mostRecentClanScreenPermission;
	}
}
