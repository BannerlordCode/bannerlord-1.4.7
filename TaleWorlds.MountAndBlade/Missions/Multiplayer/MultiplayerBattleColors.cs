using System;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade.Missions.Multiplayer
{
	// Token: 0x020003E9 RID: 1001
	public readonly struct MultiplayerBattleColors
	{
		// Token: 0x06003700 RID: 14080 RVA: 0x000E370A File Offset: 0x000E190A
		public MultiplayerBattleColors(MultiplayerBattleColors.MultiplayerCultureColorInfo attackerColors, MultiplayerBattleColors.MultiplayerCultureColorInfo defenderColors)
		{
			this.AttackerColors = attackerColors;
			this.DefenderColors = defenderColors;
		}

		// Token: 0x06003701 RID: 14081 RVA: 0x000E371A File Offset: 0x000E191A
		public static MultiplayerBattleColors CreateWith(BasicCultureObject attackerCulture, BasicCultureObject defenderCulture)
		{
			return MultiplayerBattleColors.GetCultureColors(attackerCulture, defenderCulture);
		}

		// Token: 0x06003702 RID: 14082 RVA: 0x000E3724 File Offset: 0x000E1924
		public MultiplayerBattleColors.MultiplayerCultureColorInfo GetPeerColors(MissionPeer peer)
		{
			if (peer == null)
			{
				return this.AttackerColors;
			}
			if (this.AttackerColors.Culture == this.DefenderColors.Culture)
			{
				if (peer.Team == null)
				{
					return this.AttackerColors;
				}
				if (peer.Team.Side != BattleSideEnum.Attacker)
				{
					return this.DefenderColors;
				}
				return this.AttackerColors;
			}
			else
			{
				if (peer.Culture != this.AttackerColors.Culture)
				{
					return this.DefenderColors;
				}
				return this.AttackerColors;
			}
		}

		// Token: 0x06003703 RID: 14083 RVA: 0x000E37A0 File Offset: 0x000E19A0
		private static MultiplayerBattleColors GetCultureColors(BasicCultureObject attackerCulture, BasicCultureObject defenderCulture)
		{
			if (attackerCulture == null)
			{
				attackerCulture = MultiplayerBattleColors.GetFallbackCulture();
			}
			if (defenderCulture == null)
			{
				defenderCulture = MultiplayerBattleColors.GetFallbackCulture();
			}
			bool flag = !string.IsNullOrEmpty(attackerCulture.StringId) && !string.IsNullOrEmpty(defenderCulture.StringId) && attackerCulture.StringId == defenderCulture.StringId;
			MultiplayerBattleColors.MultiplayerCultureColorInfo multiplayerCultureColorInfo = new MultiplayerBattleColors.MultiplayerCultureColorInfo(attackerCulture, false);
			MultiplayerBattleColors.MultiplayerCultureColorInfo multiplayerCultureColorInfo2 = new MultiplayerBattleColors.MultiplayerCultureColorInfo(defenderCulture, flag);
			return new MultiplayerBattleColors(multiplayerCultureColorInfo, multiplayerCultureColorInfo2);
		}

		// Token: 0x06003704 RID: 14084 RVA: 0x000E3808 File Offset: 0x000E1A08
		private static BasicCultureObject GetFallbackCulture()
		{
			MBReadOnlyList<BasicCultureObject> objectTypeList = MBObjectManager.Instance.GetObjectTypeList<BasicCultureObject>();
			if (objectTypeList != null && objectTypeList.Count > 0)
			{
				return objectTypeList.FirstOrDefault<BasicCultureObject>();
			}
			Debug.FailedAssert("No culture objects in the object manager", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\Multiplayer\\MultiplayerBattleColors.cs", "GetFallbackCulture", 114);
			return null;
		}

		// Token: 0x040017AF RID: 6063
		public readonly MultiplayerBattleColors.MultiplayerCultureColorInfo AttackerColors;

		// Token: 0x040017B0 RID: 6064
		public readonly MultiplayerBattleColors.MultiplayerCultureColorInfo DefenderColors;

		// Token: 0x02000699 RID: 1689
		public readonly struct MultiplayerCultureColorInfo
		{
			// Token: 0x060041AB RID: 16811 RVA: 0x000FC214 File Offset: 0x000FA414
			public MultiplayerCultureColorInfo(BasicCultureObject culture, bool swapColors)
			{
				this.Culture = culture;
				this.Color1 = Color.FromUint(this.Color1Uint = (swapColors ? ((culture != null) ? new uint?(culture.Color2) : null) : ((culture != null) ? new uint?(culture.Color) : null)) ?? 0U);
				this.Color2 = Color.FromUint(this.Color2Uint = (swapColors ? ((culture != null) ? new uint?(culture.Color) : null) : ((culture != null) ? new uint?(culture.Color2) : null)) ?? 0U);
				this.ClothingColor1 = Color.FromUint(this.ClothingColor1Uint = (swapColors ? ((culture != null) ? new uint?(culture.Color2) : null) : ((culture != null) ? new uint?(culture.Color) : null)) ?? 0U);
				this.ClothingColor2 = Color.FromUint(this.ClothingColor2Uint = (swapColors ? ((culture != null) ? new uint?(culture.Color) : null) : ((culture != null) ? new uint?(culture.Color2) : null)) ?? 0U);
				this.BannerBackgroundColor = Color.FromUint(this.BannerBackgroundColorUint = (swapColors ? ((culture != null) ? new uint?(culture.BackgroundColor2) : null) : ((culture != null) ? new uint?(culture.BackgroundColor1) : null)) ?? 0U);
				this.BannerForegroundColor = Color.FromUint(this.BannerForegroundColorUint = (swapColors ? ((culture != null) ? new uint?(culture.ForegroundColor2) : null) : ((culture != null) ? new uint?(culture.ForegroundColor1) : null)) ?? 0U);
			}

			// Token: 0x040022C7 RID: 8903
			public readonly BasicCultureObject Culture;

			// Token: 0x040022C8 RID: 8904
			public readonly Color Color1;

			// Token: 0x040022C9 RID: 8905
			public readonly uint Color1Uint;

			// Token: 0x040022CA RID: 8906
			public readonly Color Color2;

			// Token: 0x040022CB RID: 8907
			public readonly uint Color2Uint;

			// Token: 0x040022CC RID: 8908
			public readonly Color ClothingColor1;

			// Token: 0x040022CD RID: 8909
			public readonly uint ClothingColor1Uint;

			// Token: 0x040022CE RID: 8910
			public readonly Color ClothingColor2;

			// Token: 0x040022CF RID: 8911
			public readonly uint ClothingColor2Uint;

			// Token: 0x040022D0 RID: 8912
			public readonly Color BannerBackgroundColor;

			// Token: 0x040022D1 RID: 8913
			public readonly uint BannerBackgroundColorUint;

			// Token: 0x040022D2 RID: 8914
			public readonly Color BannerForegroundColor;

			// Token: 0x040022D3 RID: 8915
			public readonly uint BannerForegroundColorUint;
		}
	}
}
