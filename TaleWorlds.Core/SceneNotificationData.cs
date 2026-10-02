using System;
using System.Collections.Generic;
using TaleWorlds.Localization;

namespace TaleWorlds.Core
{
	// Token: 0x020000CA RID: 202
	public class SceneNotificationData
	{
		// Token: 0x170003BF RID: 959
		// (get) Token: 0x06000AE9 RID: 2793 RVA: 0x00023891 File Offset: 0x00021A91
		public virtual string SceneID { get; }

		// Token: 0x170003C0 RID: 960
		// (get) Token: 0x06000AEA RID: 2794 RVA: 0x00023899 File Offset: 0x00021A99
		public virtual string SoundEventPath { get; }

		// Token: 0x170003C1 RID: 961
		// (get) Token: 0x06000AEB RID: 2795 RVA: 0x000238A1 File Offset: 0x00021AA1
		public virtual TextObject TitleText { get; }

		// Token: 0x170003C2 RID: 962
		// (get) Token: 0x06000AEC RID: 2796 RVA: 0x000238A9 File Offset: 0x00021AA9
		public virtual TextObject AffirmativeDescriptionText { get; }

		// Token: 0x170003C3 RID: 963
		// (get) Token: 0x06000AED RID: 2797 RVA: 0x000238B1 File Offset: 0x00021AB1
		public virtual TextObject NegativeDescriptionText { get; }

		// Token: 0x170003C4 RID: 964
		// (get) Token: 0x06000AEE RID: 2798 RVA: 0x000238B9 File Offset: 0x00021AB9
		public virtual TextObject AffirmativeHintText { get; }

		// Token: 0x170003C5 RID: 965
		// (get) Token: 0x06000AEF RID: 2799 RVA: 0x000238C1 File Offset: 0x00021AC1
		public virtual TextObject AffirmativeHintTextExtended { get; }

		// Token: 0x170003C6 RID: 966
		// (get) Token: 0x06000AF0 RID: 2800 RVA: 0x000238C9 File Offset: 0x00021AC9
		public virtual TextObject AffirmativeTitleText { get; }

		// Token: 0x170003C7 RID: 967
		// (get) Token: 0x06000AF1 RID: 2801 RVA: 0x000238D1 File Offset: 0x00021AD1
		public virtual TextObject NegativeTitleText { get; }

		// Token: 0x170003C8 RID: 968
		// (get) Token: 0x06000AF2 RID: 2802 RVA: 0x000238D9 File Offset: 0x00021AD9
		public virtual TextObject AffirmativeText { get; }

		// Token: 0x170003C9 RID: 969
		// (get) Token: 0x06000AF3 RID: 2803 RVA: 0x000238E1 File Offset: 0x00021AE1
		public virtual TextObject NegativeText { get; }

		// Token: 0x170003CA RID: 970
		// (get) Token: 0x06000AF4 RID: 2804 RVA: 0x000238E9 File Offset: 0x00021AE9
		public virtual bool IsAffirmativeOptionShown { get; }

		// Token: 0x170003CB RID: 971
		// (get) Token: 0x06000AF5 RID: 2805 RVA: 0x000238F1 File Offset: 0x00021AF1
		public virtual bool IsNegativeOptionShown { get; }

		// Token: 0x170003CC RID: 972
		// (get) Token: 0x06000AF6 RID: 2806 RVA: 0x000238F9 File Offset: 0x00021AF9
		public virtual bool PauseActiveState { get; } = true;

		// Token: 0x170003CD RID: 973
		// (get) Token: 0x06000AF7 RID: 2807 RVA: 0x00023901 File Offset: 0x00021B01
		public virtual SceneNotificationData.RelevantContextType RelevantContext { get; }

		// Token: 0x170003CE RID: 974
		// (get) Token: 0x06000AF8 RID: 2808 RVA: 0x00023909 File Offset: 0x00021B09
		public virtual SceneNotificationData.NotificationSceneProperties SceneProperties { get; } = new SceneNotificationData.NotificationSceneProperties
		{
			InitializePhysics = false,
			DisableStaticShadows = false,
			OverriddenWaterStrength = null
		};

		// Token: 0x06000AF9 RID: 2809 RVA: 0x00023911 File Offset: 0x00021B11
		public virtual void OnAffirmativeAction()
		{
		}

		// Token: 0x06000AFA RID: 2810 RVA: 0x00023913 File Offset: 0x00021B13
		public virtual void OnNegativeAction()
		{
		}

		// Token: 0x06000AFB RID: 2811 RVA: 0x00023915 File Offset: 0x00021B15
		public virtual void OnCloseAction()
		{
		}

		// Token: 0x06000AFC RID: 2812 RVA: 0x00023917 File Offset: 0x00021B17
		public virtual Banner[] GetBanners()
		{
			return Array.Empty<Banner>();
		}

		// Token: 0x06000AFD RID: 2813 RVA: 0x0002391E File Offset: 0x00021B1E
		public virtual SceneNotificationData.SceneNotificationCharacter[] GetSceneNotificationCharacters()
		{
			return Array.Empty<SceneNotificationData.SceneNotificationCharacter>();
		}

		// Token: 0x06000AFE RID: 2814 RVA: 0x00023925 File Offset: 0x00021B25
		public virtual SceneNotificationData.SceneNotificationShip[] GetShips()
		{
			return Array.Empty<SceneNotificationData.SceneNotificationShip>();
		}

		// Token: 0x02000128 RID: 296
		public readonly struct SceneNotificationCharacter
		{
			// Token: 0x06000C22 RID: 3106 RVA: 0x00026A7C File Offset: 0x00024C7C
			public SceneNotificationCharacter(BasicCharacterObject character, Equipment overriddenEquipment = null, BodyProperties overriddenBodyProperties = default(BodyProperties), bool useCivilianEquipment = false, uint customColor1 = 4294967295U, uint customColor2 = 4294967295U, bool useHorse = false)
			{
				this.Character = character;
				this.OverriddenEquipment = overriddenEquipment;
				this.OverriddenBodyProperties = overriddenBodyProperties;
				this.UseCivilianEquipment = useCivilianEquipment;
				this.CustomColor1 = customColor1;
				this.CustomColor2 = customColor2;
				this.UseHorse = useHorse;
			}

			// Token: 0x040007C8 RID: 1992
			public readonly BasicCharacterObject Character;

			// Token: 0x040007C9 RID: 1993
			public readonly Equipment OverriddenEquipment;

			// Token: 0x040007CA RID: 1994
			public readonly BodyProperties OverriddenBodyProperties;

			// Token: 0x040007CB RID: 1995
			public readonly bool UseCivilianEquipment;

			// Token: 0x040007CC RID: 1996
			public readonly bool UseHorse;

			// Token: 0x040007CD RID: 1997
			public readonly uint CustomColor1;

			// Token: 0x040007CE RID: 1998
			public readonly uint CustomColor2;
		}

		// Token: 0x02000129 RID: 297
		public readonly struct SceneNotificationShip
		{
			// Token: 0x06000C23 RID: 3107 RVA: 0x00026AB3 File Offset: 0x00024CB3
			public SceneNotificationShip(string shipPrefabId, List<ShipVisualSlotInfo> shipUpgrades, float shipHitPointRatio, uint sailColor1, uint sailColor2, int shipSeed)
			{
				this.ShipPrefabId = shipPrefabId;
				this.ShipUpgrades = shipUpgrades;
				this.ShipHitPointRatio = shipHitPointRatio;
				this.SailColor1 = sailColor1;
				this.SailColor2 = sailColor2;
				this.ShipSeed = shipSeed;
			}

			// Token: 0x040007CF RID: 1999
			public readonly string ShipPrefabId;

			// Token: 0x040007D0 RID: 2000
			public readonly List<ShipVisualSlotInfo> ShipUpgrades;

			// Token: 0x040007D1 RID: 2001
			public readonly float ShipHitPointRatio;

			// Token: 0x040007D2 RID: 2002
			public readonly uint SailColor1;

			// Token: 0x040007D3 RID: 2003
			public readonly uint SailColor2;

			// Token: 0x040007D4 RID: 2004
			public readonly int ShipSeed;
		}

		// Token: 0x0200012A RID: 298
		public struct NotificationSceneProperties
		{
			// Token: 0x040007D5 RID: 2005
			public bool InitializePhysics;

			// Token: 0x040007D6 RID: 2006
			public bool DisableStaticShadows;

			// Token: 0x040007D7 RID: 2007
			public float? OverriddenWaterStrength;
		}

		// Token: 0x0200012B RID: 299
		public enum RelevantContextType
		{
			// Token: 0x040007D9 RID: 2009
			Any,
			// Token: 0x040007DA RID: 2010
			MPLobby,
			// Token: 0x040007DB RID: 2011
			CustomBattle,
			// Token: 0x040007DC RID: 2012
			Mission,
			// Token: 0x040007DD RID: 2013
			Map
		}
	}
}
