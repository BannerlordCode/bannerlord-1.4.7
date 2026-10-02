using System;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001B0 RID: 432
	[ScriptingInterfaceBase]
	internal interface IMBGameEntityExtensions
	{
		// Token: 0x060018A8 RID: 6312
		[EngineMethod("create_from_weapon", false, null, false)]
		GameEntity CreateFromWeapon(UIntPtr scenePointer, in WeaponData weaponData, WeaponStatsData[] weaponStatsData, int weaponStatsDataLength, in WeaponData ammoWeaponData, WeaponStatsData[] ammoWeaponStatsData, int ammoWeaponStatsDataLength, bool showHolsterWithWeapon);

		// Token: 0x060018A9 RID: 6313
		[EngineMethod("fade_out", false, null, false)]
		void FadeOut(UIntPtr entityPointer, float interval, bool isRemovingFromScene);

		// Token: 0x060018AA RID: 6314
		[EngineMethod("fade_in", false, null, false)]
		void FadeIn(UIntPtr entityPointer, bool resetAlpha);

		// Token: 0x060018AB RID: 6315
		[EngineMethod("hide_if_not_fading_out", false, null, false)]
		void HideIfNotFadingOut(UIntPtr entityPointer);
	}
}
