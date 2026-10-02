using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001EC RID: 492
	public static class CombatLogManager
	{
		// Token: 0x14000024 RID: 36
		// (add) Token: 0x06001C99 RID: 7321 RVA: 0x00061DD8 File Offset: 0x0005FFD8
		// (remove) Token: 0x06001C9A RID: 7322 RVA: 0x00061E0C File Offset: 0x0006000C
		public static event Action<CombatLogData> OnGenerateCombatLog;

		// Token: 0x06001C9B RID: 7323 RVA: 0x00061E40 File Offset: 0x00060040
		public static void PrintDebugLogForInfo(Agent attackerAgent, Agent victimAgent, DamageTypes damageType, int speedBonus, int armorAmount, int inflictedDamage, int absorbedByArmor, sbyte collisionBone, float lostHpPercentage)
		{
			TextObject textObject = TextObject.GetEmpty();
			CombatLogColor combatLogColor = CombatLogColor.White;
			bool isMine = attackerAgent.IsMine;
			bool isMine2 = victimAgent.IsMine;
			GameTexts.SetVariable("AMOUNT", inflictedDamage);
			GameTexts.SetVariable("DAMAGE_TYPE", damageType.ToString().ToLower());
			GameTexts.SetVariable("LOST_HP_PERCENTAGE", lostHpPercentage);
			if (isMine2)
			{
				GameTexts.SetVariable("ATTACKER_NAME", attackerAgent.NameTextObject);
				textObject = GameTexts.FindText("combat_log_player_attacked", null);
				combatLogColor = CombatLogColor.Red;
			}
			else if (isMine)
			{
				GameTexts.SetVariable("VICTIM_NAME", victimAgent.NameTextObject);
				textObject = GameTexts.FindText("combat_log_player_attacker", null);
				combatLogColor = CombatLogColor.Green;
			}
			CombatLogManager.Print(textObject, combatLogColor);
			MBStringBuilder mbstringBuilder = default(MBStringBuilder);
			mbstringBuilder.Initialize(16, "PrintDebugLogForInfo");
			if (armorAmount > 0)
			{
				GameTexts.SetVariable("ABSORBED_AMOUNT", absorbedByArmor);
				GameTexts.SetVariable("ARMOR_AMOUNT", armorAmount);
				mbstringBuilder.AppendLine<string>(GameTexts.FindText("combat_log_damage_absorbed", null).ToString());
			}
			if (victimAgent.IsHuman)
			{
				GameTexts.SetVariable("BONE", collisionBone.ToString());
				mbstringBuilder.AppendLine<string>(GameTexts.FindText("combat_log_hit_bone", null).ToString());
			}
			if (speedBonus != 0)
			{
				GameTexts.SetVariable("SPEED_BONUS", speedBonus);
				mbstringBuilder.AppendLine<string>(GameTexts.FindText("combat_log_speed_bonus", null).ToString());
			}
			CombatLogManager.Print(new TextObject(mbstringBuilder.ToStringAndRelease(), null), CombatLogColor.White);
		}

		// Token: 0x06001C9C RID: 7324 RVA: 0x00061F98 File Offset: 0x00060198
		private static void Print(TextObject message, CombatLogColor logColor = CombatLogColor.White)
		{
			Debug.Print(message.ToString(), 0, (Debug.DebugColor)logColor, 562949953421312UL);
		}

		// Token: 0x06001C9D RID: 7325 RVA: 0x00061FC0 File Offset: 0x000601C0
		public static void GenerateCombatLog(CombatLogData logData)
		{
			Action<CombatLogData> onGenerateCombatLog = CombatLogManager.OnGenerateCombatLog;
			if (onGenerateCombatLog != null)
			{
				onGenerateCombatLog(logData);
			}
			foreach (ValueTuple<string, uint> valueTuple in logData.GetLogString())
			{
				InformationManager.DisplayMessage(new InformationMessage(valueTuple.Item1, Color.FromUint(valueTuple.Item2), "Combat"));
			}
		}
	}
}
