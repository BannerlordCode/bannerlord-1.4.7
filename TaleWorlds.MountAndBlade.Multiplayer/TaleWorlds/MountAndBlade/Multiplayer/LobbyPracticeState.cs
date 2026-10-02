using System;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Multiplayer
{
	// Token: 0x02000053 RID: 83
	public class LobbyPracticeState : GameState
	{
		// Token: 0x060002AB RID: 683 RVA: 0x0000BAA6 File Offset: 0x00009CA6
		protected override void OnActivate()
		{
			base.OnActivate();
			if (this._practiceOpened)
			{
				base.GameStateManager.PopState(0);
			}
		}

		// Token: 0x060002AC RID: 684 RVA: 0x0000BAC2 File Offset: 0x00009CC2
		protected override void OnTick(float dt)
		{
			base.OnTick(dt);
			if (!this._practiceOpened)
			{
				this.OpenPracticeMission();
				this._practiceOpened = true;
			}
		}

		// Token: 0x060002AD RID: 685 RVA: 0x0000BAE0 File Offset: 0x00009CE0
		private void OpenPracticeMission()
		{
			BasicCharacterObject @object = Game.Current.ObjectManager.GetObject<BasicCharacterObject>("mp_heavy_cavalry_empire_hero");
			BasicCharacterObject object2 = Game.Current.ObjectManager.GetObject<BasicCharacterObject>("mp_skirmisher_battania_troop");
			BasicCharacterObject object3 = Game.Current.ObjectManager.GetObject<BasicCharacterObject>("mp_light_ranged_khuzait_troop");
			Game.Current.PlayerTroop = @object;
			BasicCultureObject object4;
			BasicCultureObject basicCultureObject = (object4 = Game.Current.ObjectManager.GetObject<BasicCultureObject>("empire"));
			Banner banner = basicCultureObject.Banner;
			Banner banner2 = object4.Banner;
			CustomBattleCombatant customBattleCombatant = new CustomBattleCombatant(new TextObject("{=sSJSTe5p}Player Party", null), basicCultureObject, banner);
			CustomBattleCombatant customBattleCombatant2 = new CustomBattleCombatant(new TextObject("{=0xC75dN6}Enemy Party", null), object4, banner2);
			customBattleCombatant.AddCharacter(@object, 1);
			customBattleCombatant2.AddCharacter(@object, 1);
			customBattleCombatant.AddCharacter(object2, 3);
			customBattleCombatant2.AddCharacter(object2, 3);
			customBattleCombatant.AddCharacter(object3, 8);
			customBattleCombatant2.AddCharacter(object3, 8);
			customBattleCombatant.SetGeneral(@object);
			customBattleCombatant2.SetGeneral(@object);
			customBattleCombatant.Side = BattleSideEnum.Attacker;
			customBattleCombatant2.Side = BattleSideEnum.Defender;
			MultiplayerPracticeMissions.OpenMultiplayerPracticeMission("mp_practice_battle", @object, customBattleCombatant, customBattleCombatant2, true, null, "", "summer", 6f);
		}

		// Token: 0x040000E1 RID: 225
		private bool _practiceOpened;
	}
}
