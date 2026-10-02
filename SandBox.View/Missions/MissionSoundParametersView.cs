using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;

namespace SandBox.View.Missions
{
	// Token: 0x02000023 RID: 35
	[DefaultView]
	public class MissionSoundParametersView : MissionView
	{
		// Token: 0x060000DF RID: 223 RVA: 0x0000A737 File Offset: 0x00008937
		public override void EarlyStart()
		{
			base.EarlyStart();
			this.InitializeGlobalParameters();
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x0000A745 File Offset: 0x00008945
		public override void OnMissionScreenFinalize()
		{
			base.OnMissionScreenFinalize();
			SoundManager.SetGlobalParameter("MissionCulture", 0f);
			SoundManager.SetGlobalParameter("MissionProsperity", 0f);
			SoundManager.SetGlobalParameter("MissionCombatMode", 0f);
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x0000A77A File Offset: 0x0000897A
		public override void OnMissionModeChange(MissionMode oldMissionMode, bool atStart)
		{
			this.InitializeCombatModeParameter();
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x0000A782 File Offset: 0x00008982
		private void InitializeGlobalParameters()
		{
			this.InitializeCultureParameter();
			this.InitializeProsperityParameter();
			this.InitializeCombatModeParameter();
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x0000A798 File Offset: 0x00008998
		private void InitializeCultureParameter()
		{
			MissionSoundParametersView.SoundParameterMissionCulture soundParameterMissionCulture = MissionSoundParametersView.SoundParameterMissionCulture.None;
			if (Campaign.Current != null)
			{
				Settlement currentSettlement = Settlement.CurrentSettlement;
				if (currentSettlement != null)
				{
					if (currentSettlement.IsHideout)
					{
						soundParameterMissionCulture = MissionSoundParametersView.SoundParameterMissionCulture.Bandit;
					}
					else
					{
						string stringId = currentSettlement.Culture.StringId;
						uint num = <PrivateImplementationDetails>.ComputeStringHash(stringId);
						if (num <= 2848701557U)
						{
							if (num != 744444005U)
							{
								if (num != 1759932477U)
								{
									if (num == 2848701557U)
									{
										if (stringId == "khuzait")
										{
											soundParameterMissionCulture = MissionSoundParametersView.SoundParameterMissionCulture.Khuzait;
										}
									}
								}
								else if (stringId == "battania")
								{
									soundParameterMissionCulture = MissionSoundParametersView.SoundParameterMissionCulture.Battania;
								}
							}
							else if (stringId == "empire")
							{
								soundParameterMissionCulture = MissionSoundParametersView.SoundParameterMissionCulture.Empire;
							}
						}
						else if (num <= 3015521580U)
						{
							if (num != 2894801972U)
							{
								if (num == 3015521580U)
								{
									if (stringId == "aserai")
									{
										soundParameterMissionCulture = MissionSoundParametersView.SoundParameterMissionCulture.Aserai;
									}
								}
							}
							else if (stringId == "nord")
							{
								soundParameterMissionCulture = MissionSoundParametersView.SoundParameterMissionCulture.Nord;
							}
						}
						else if (num != 3311783860U)
						{
							if (num == 4214512470U)
							{
								if (stringId == "vlandia")
								{
									soundParameterMissionCulture = MissionSoundParametersView.SoundParameterMissionCulture.Vlandia;
								}
							}
						}
						else if (stringId == "sturgia")
						{
							soundParameterMissionCulture = MissionSoundParametersView.SoundParameterMissionCulture.Sturgia;
						}
					}
				}
			}
			SoundManager.SetGlobalParameter("MissionCulture", (float)soundParameterMissionCulture);
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x0000A8CC File Offset: 0x00008ACC
		private void InitializeProsperityParameter()
		{
			MissionSoundParametersView.SoundParameterMissionProsperityLevel soundParameterMissionProsperityLevel = MissionSoundParametersView.SoundParameterMissionProsperityLevel.None;
			if (Campaign.Current != null && Settlement.CurrentSettlement != null)
			{
				switch (Settlement.CurrentSettlement.SettlementComponent.GetProsperityLevel())
				{
				case SettlementComponent.ProsperityLevel.Low:
					soundParameterMissionProsperityLevel = MissionSoundParametersView.SoundParameterMissionProsperityLevel.None;
					break;
				case SettlementComponent.ProsperityLevel.Mid:
					soundParameterMissionProsperityLevel = MissionSoundParametersView.SoundParameterMissionProsperityLevel.Mid;
					break;
				case SettlementComponent.ProsperityLevel.High:
					soundParameterMissionProsperityLevel = MissionSoundParametersView.SoundParameterMissionProsperityLevel.High;
					break;
				}
			}
			SoundManager.SetGlobalParameter("MissionProsperity", (float)soundParameterMissionProsperityLevel);
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x0000A924 File Offset: 0x00008B24
		private void InitializeCombatModeParameter()
		{
			bool flag = base.Mission.Mode == MissionMode.Battle || base.Mission.Mode == MissionMode.Duel || base.Mission.Mode == MissionMode.Tournament;
			SoundManager.SetGlobalParameter("MissionCombatMode", (float)(flag ? 1 : 0));
		}

		// Token: 0x0400007D RID: 125
		private const string CultureParameterId = "MissionCulture";

		// Token: 0x0400007E RID: 126
		private const string ProsperityParameterId = "MissionProsperity";

		// Token: 0x0400007F RID: 127
		private const string CombatParameterId = "MissionCombatMode";

		// Token: 0x0200008D RID: 141
		public enum SoundParameterMissionCulture : short
		{
			// Token: 0x040002C2 RID: 706
			None,
			// Token: 0x040002C3 RID: 707
			Aserai,
			// Token: 0x040002C4 RID: 708
			Battania,
			// Token: 0x040002C5 RID: 709
			Empire,
			// Token: 0x040002C6 RID: 710
			Khuzait,
			// Token: 0x040002C7 RID: 711
			Sturgia,
			// Token: 0x040002C8 RID: 712
			Vlandia,
			// Token: 0x040002C9 RID: 713
			Nord,
			// Token: 0x040002CA RID: 714
			ReservedA,
			// Token: 0x040002CB RID: 715
			ReservedB,
			// Token: 0x040002CC RID: 716
			Bandit
		}

		// Token: 0x0200008E RID: 142
		private enum SoundParameterMissionProsperityLevel : short
		{
			// Token: 0x040002CE RID: 718
			None,
			// Token: 0x040002CF RID: 719
			Low = 0,
			// Token: 0x040002D0 RID: 720
			Mid,
			// Token: 0x040002D1 RID: 721
			High
		}
	}
}
