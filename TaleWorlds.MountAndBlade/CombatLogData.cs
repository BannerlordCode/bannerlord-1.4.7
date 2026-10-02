using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001ED RID: 493
	public struct CombatLogData
	{
		// Token: 0x170005C1 RID: 1473
		// (get) Token: 0x06001C9E RID: 7326 RVA: 0x00062040 File Offset: 0x00060240
		private bool IsValidForPlayer
		{
			get
			{
				return this.IsImportant && (this.IsAttackerPlayer || this.IsVictimPlayer);
			}
		}

		// Token: 0x170005C2 RID: 1474
		// (get) Token: 0x06001C9F RID: 7327 RVA: 0x0006205C File Offset: 0x0006025C
		private bool IsImportant
		{
			get
			{
				return this.TotalDamage > 0 || this.TotalFireDamage > 0 || this.CrushedThrough || this.Chamber;
			}
		}

		// Token: 0x170005C3 RID: 1475
		// (get) Token: 0x06001CA0 RID: 7328 RVA: 0x00062080 File Offset: 0x00060280
		private bool IsSpecialSelfDamage
		{
			get
			{
				return this.IsSpecialDamage && this.IsVictimAgentSameAsAttackerAgent;
			}
		}

		// Token: 0x170005C4 RID: 1476
		// (get) Token: 0x06001CA1 RID: 7329 RVA: 0x00062092 File Offset: 0x00060292
		private bool IsAttackerPlayer
		{
			get
			{
				if (!this.IsAttackerAgentHuman)
				{
					return this.DoesAttackerAgentHaveRiderAgent && this.IsAttackerAgentRiderAgentMine;
				}
				return this.IsAttackerAgentMine;
			}
		}

		// Token: 0x170005C5 RID: 1477
		// (get) Token: 0x06001CA2 RID: 7330 RVA: 0x000620B3 File Offset: 0x000602B3
		private bool IsVictimPlayer
		{
			get
			{
				if (!this.IsVictimAgentHuman)
				{
					return this.DoesVictimAgentHaveRiderAgent && this.IsVictimAgentRiderAgentMine;
				}
				return this.IsVictimAgentMine;
			}
		}

		// Token: 0x170005C6 RID: 1478
		// (get) Token: 0x06001CA3 RID: 7331 RVA: 0x000620D4 File Offset: 0x000602D4
		private bool IsAttackerMount
		{
			get
			{
				return this.IsAttackerAgentMount;
			}
		}

		// Token: 0x170005C7 RID: 1479
		// (get) Token: 0x06001CA4 RID: 7332 RVA: 0x000620DC File Offset: 0x000602DC
		private bool IsVictimMount
		{
			get
			{
				return this.IsVictimAgentMount;
			}
		}

		// Token: 0x170005C8 RID: 1480
		// (get) Token: 0x06001CA5 RID: 7333 RVA: 0x000620E4 File Offset: 0x000602E4
		public int TotalDamage
		{
			get
			{
				return this.InflictedDamage + this.ModifiedDamage;
			}
		}

		// Token: 0x170005C9 RID: 1481
		// (get) Token: 0x06001CA6 RID: 7334 RVA: 0x000620F3 File Offset: 0x000602F3
		public int TotalFireDamage
		{
			get
			{
				return this.InflictedFireDamage + this.ModifiedFireDamage;
			}
		}

		// Token: 0x170005CA RID: 1482
		// (get) Token: 0x06001CA7 RID: 7335 RVA: 0x00062102 File Offset: 0x00060302
		// (set) Token: 0x06001CA8 RID: 7336 RVA: 0x0006210A File Offset: 0x0006030A
		public float AttackProgress { get; internal set; }

		// Token: 0x06001CA9 RID: 7337 RVA: 0x00062114 File Offset: 0x00060314
		public List<ValueTuple<string, uint>> GetLogString()
		{
			CombatLogData._logStringCache.Clear();
			if (this.IsValidForPlayer && !this.IsSpecialSelfDamage && ManagedOptions.GetConfig(ManagedOptions.ManagedOptionsType.ReportDamage) > 0f)
			{
				if (this.IsSneakAttack && this.IsAttackerPlayer)
				{
					CombatLogData._logStringCache.Add(ValueTuple.Create<string, uint>(GameTexts.FindText("combat_log_sneak_attack", null).ToString(), 4289612505U));
				}
				if (this.IsRangedAttack && this.IsAttackerPlayer && this.BodyPartHit == BoneBodyPartType.Head)
				{
					CombatLogData._logStringCache.Add(ValueTuple.Create<string, uint>(GameTexts.FindText("ui_head_shot", null).ToString(), 4289612505U));
				}
				if (this.IsFriendlyFire)
				{
					CombatLogData._logStringCache.Add(ValueTuple.Create<string, uint>(GameTexts.FindText("combat_log_friendly_fire", null).ToString(), 4289612505U));
				}
				if (this.CrushedThrough && !this.IsFriendlyFire)
				{
					if (this.IsAttackerPlayer)
					{
						CombatLogData._logStringCache.Add(ValueTuple.Create<string, uint>(GameTexts.FindText("combat_log_crushed_through_attacker", null).ToString(), 4289612505U));
					}
					else
					{
						CombatLogData._logStringCache.Add(ValueTuple.Create<string, uint>(GameTexts.FindText("combat_log_crushed_through_victim", null).ToString(), 4289612505U));
					}
				}
				if (this.Chamber)
				{
					CombatLogData._logStringCache.Add(ValueTuple.Create<string, uint>(GameTexts.FindText("combat_log_chamber_blocked", null).ToString(), 4289612505U));
				}
				uint num = 4290563554U;
				GameTexts.SetVariable("DAMAGE", this.TotalDamage);
				string text = "DAMAGE_TYPE";
				string text2 = "combat_log_damage_type";
				int num2 = (int)this.DamageType;
				GameTexts.SetVariable(text, GameTexts.FindText(text2, num2.ToString()));
				MBStringBuilder mbstringBuilder = default(MBStringBuilder);
				mbstringBuilder.Initialize(16, "GetLogString");
				TextObject textObject = null;
				if (this.IsEntityToEntityCollisionDamage)
				{
					if (this.IsAttackerPlayer)
					{
						if (this.IsSpecialDamage)
						{
							mbstringBuilder.Append<TextObject>(GameTexts.FindText("combat_log_ram_damage_delivered", null));
						}
						else
						{
							mbstringBuilder.Append<TextObject>(GameTexts.FindText("combat_log_collision_damage_delivered", null));
						}
					}
					else if (this.IsSpecialDamage)
					{
						mbstringBuilder.Append<TextObject>(GameTexts.FindText("combat_log_ram_damage_received", null));
					}
					else
					{
						mbstringBuilder.Append<TextObject>(GameTexts.FindText("combat_log_collision_damage_received", null));
					}
				}
				else if (this.IsVictimAgentSameAsAttackerAgent)
				{
					mbstringBuilder.Append<TextObject>(GameTexts.FindText("ui_received_number_damage_fall", null));
					num = 4292917946U;
				}
				else if (this.IsVictimMount)
				{
					if (this.IsVictimRiderAgentSameAsAttackerAgent)
					{
						mbstringBuilder.Append<TextObject>(GameTexts.FindText("ui_received_number_damage_fall_to_horse", null));
						num = 4292917946U;
					}
					else
					{
						mbstringBuilder.Append<TextObject>(GameTexts.FindText(this.IsAttackerPlayer ? "ui_delivered_number_damage_to_horse" : "ui_horse_received_number_damage", null));
						num = (this.IsAttackerPlayer ? 4210351871U : 4292917946U);
					}
				}
				else if (this.MissionObjectHit != null)
				{
					WeakGameEntity weakGameEntity = this.MissionObjectHit.GameEntity;
					textObject = this.MissionObjectHit.HitObjectName;
					while (weakGameEntity != null)
					{
						if (!TextObject.IsNullOrEmpty(textObject))
						{
							break;
						}
						int scriptCount = weakGameEntity.GetScriptCount();
						for (int i = 0; i < scriptCount; i++)
						{
							MissionObject missionObject;
							if ((missionObject = weakGameEntity.GetScriptAtIndex(i) as MissionObject) != null && TextObject.IsNullOrEmpty(textObject) && !TextObject.IsNullOrEmpty(missionObject.HitObjectName))
							{
								textObject = missionObject.HitObjectName;
								break;
							}
						}
						weakGameEntity = weakGameEntity.Parent;
					}
				}
				else if (this.IsAttackerMount)
				{
					mbstringBuilder.Append<TextObject>(GameTexts.FindText(this.IsAttackerPlayer ? "ui_horse_charged_for_number_damage" : "ui_received_number_damage", null));
					num = (this.IsAttackerPlayer ? 4210351871U : 4292917946U);
				}
				else if (this.TotalDamage > 0)
				{
					mbstringBuilder.Append<TextObject>(GameTexts.FindText(this.IsAttackerPlayer ? "ui_delivered_number_damage" : "ui_received_number_damage", null));
					num = (this.IsAttackerPlayer ? 4210351871U : 4292917946U);
				}
				if (this.MissionObjectHit != null && this.TotalDamage > 0)
				{
					mbstringBuilder.Append<TextObject>(GameTexts.FindText("ui_delivered_number_damage_to_entity", null));
				}
				if (this.BodyPartHit != BoneBodyPartType.None)
				{
					string text3 = "BODY_PART";
					string text4 = "body_part_type";
					num2 = (int)this.BodyPartHit;
					GameTexts.SetVariable(text3, GameTexts.FindText(text4, num2.ToString()));
					mbstringBuilder.Append<string>("<Detail>");
					mbstringBuilder.Append<TextObject>(GameTexts.FindText("combat_log_detail_body_part", null));
					mbstringBuilder.Append<string>("</Detail>");
				}
				if (this.HitSpeed > 1E-05f)
				{
					GameTexts.SetVariable("SPEED", MathF.Round(this.HitSpeed, 2));
					mbstringBuilder.Append<string>("<Detail>");
					mbstringBuilder.Append<TextObject>(this.IsRangedAttack ? GameTexts.FindText("combat_log_detail_missile_speed", null) : GameTexts.FindText("combat_log_detail_move_speed", null));
					mbstringBuilder.Append<string>("</Detail>");
				}
				if (this.IsRangedAttack)
				{
					GameTexts.SetVariable("DISTANCE", MathF.Round(this.Distance, 1));
					mbstringBuilder.Append<string>("<Detail>");
					mbstringBuilder.Append<TextObject>(GameTexts.FindText("combat_log_detail_distance", null));
					mbstringBuilder.Append<string>("</Detail>");
				}
				if (this.TotalDamage > 0)
				{
					if (this.AbsorbedDamage > 0)
					{
						GameTexts.SetVariable("ABSORBED_DAMAGE", this.AbsorbedDamage);
						mbstringBuilder.Append<string>("<Detail>");
						mbstringBuilder.Append<TextObject>(GameTexts.FindText("combat_log_detail_absorbed_damage", null));
						mbstringBuilder.Append<string>("</Detail>");
					}
					if (this.ModifiedDamage != 0)
					{
						GameTexts.SetVariable("MODIFIED_DAMAGE", MathF.Abs(this.ModifiedDamage));
						mbstringBuilder.Append<string>("<Detail>");
						if (this.ModifiedDamage > 0)
						{
							mbstringBuilder.Append<TextObject>(GameTexts.FindText("combat_log_detail_extra_damage", null));
						}
						else if (this.ModifiedDamage < 0)
						{
							mbstringBuilder.Append<TextObject>(GameTexts.FindText("combat_log_detail_reduced_damage", null));
						}
						mbstringBuilder.Append<string>("</Detail>");
					}
					if (this.ReflectedDamage > 0)
					{
						GameTexts.SetVariable("REFLECTED_DAMAGE", this.ReflectedDamage);
						mbstringBuilder.Append<string>("<Detail>");
						mbstringBuilder.Append<TextObject>(GameTexts.FindText("combat_log_detail_reflected_damage", null));
						mbstringBuilder.Append<string>("</Detail>");
					}
				}
				if (this.TotalFireDamage > 0)
				{
					if (this.TotalDamage > 0 && this.TotalFireDamage > 0)
					{
						mbstringBuilder.AppendLine();
					}
					GameTexts.SetVariable("FIRE_DAMAGE", this.TotalFireDamage);
					mbstringBuilder.Append<TextObject>(GameTexts.FindText("ui_delivered_number_fire_damage_to_entity", null));
					if (this.ModifiedFireDamage != 0)
					{
						GameTexts.SetVariable("MODIFIED_DAMAGE", MathF.Abs(this.ModifiedFireDamage));
						mbstringBuilder.Append<string>("<Detail>");
						if (this.ModifiedFireDamage > 0)
						{
							mbstringBuilder.Append<TextObject>(GameTexts.FindText("combat_log_detail_extra_damage", null));
						}
						else if (this.ModifiedFireDamage < 0)
						{
							mbstringBuilder.Append<TextObject>(GameTexts.FindText("combat_log_detail_reduced_damage", null));
						}
						mbstringBuilder.Append<string>("</Detail>");
					}
				}
				if (!TextObject.IsNullOrEmpty(textObject))
				{
					GameTexts.SetVariable("OBJECT_NAME", textObject.ToString());
					mbstringBuilder.Append<string>("<Detail>");
					mbstringBuilder.Append<TextObject>(GameTexts.FindText("combat_log_detail_entity_name", null));
					mbstringBuilder.Append<string>("</Detail>");
				}
				CombatLogData._logStringCache.Add(ValueTuple.Create<string, uint>(mbstringBuilder.ToStringAndRelease(), num));
			}
			return CombatLogData._logStringCache;
		}

		// Token: 0x06001CAA RID: 7338 RVA: 0x00062848 File Offset: 0x00060A48
		public CombatLogData(bool isVictimAgentSameAsAttackerAgent, bool isAttackerAgentHuman, bool isAttackerAgentMine, bool doesAttackerAgentHaveRiderAgent, bool isAttackerAgentRiderAgentMine, bool isAttackerAgentMount, bool isVictimAgentHuman, bool isVictimAgentMine, bool isVictimAgentDead, bool doesVictimAgentHaveRiderAgent, bool isVictimAgentRiderAgentIsMine, bool isVictimAgentMount, MissionObject missionObjectHit, bool isVictimRiderAgentSameAsAttackerAgent, bool crushedThrough, bool chamber, float distance)
		{
			this.IsVictimAgentSameAsAttackerAgent = isVictimAgentSameAsAttackerAgent;
			this.IsAttackerAgentHuman = isAttackerAgentHuman;
			this.IsAttackerAgentMine = isAttackerAgentMine;
			this.DoesAttackerAgentHaveRiderAgent = doesAttackerAgentHaveRiderAgent;
			this.IsAttackerAgentRiderAgentMine = isAttackerAgentRiderAgentMine;
			this.IsAttackerAgentMount = isAttackerAgentMount;
			this.IsVictimAgentHuman = isVictimAgentHuman;
			this.IsVictimAgentMine = isVictimAgentMine;
			this.DoesVictimAgentHaveRiderAgent = doesVictimAgentHaveRiderAgent;
			this.IsVictimAgentRiderAgentMine = isVictimAgentRiderAgentIsMine;
			this.IsVictimAgentMount = isVictimAgentMount;
			this.MissionObjectHit = missionObjectHit;
			this.IsVictimRiderAgentSameAsAttackerAgent = isVictimRiderAgentSameAsAttackerAgent;
			this.IsFatalDamage = isVictimAgentDead;
			this.IsEntityToEntityCollisionDamage = false;
			this.IsSpecialDamage = false;
			this.DamageType = DamageTypes.Blunt;
			this.CrushedThrough = crushedThrough;
			this.Chamber = chamber;
			this.IsRangedAttack = false;
			this.IsFriendlyFire = false;
			this.IsSneakAttack = false;
			this.VictimAgentName = null;
			this.HitSpeed = 0f;
			this.InflictedDamage = 0;
			this.AbsorbedDamage = 0;
			this.ModifiedDamage = 0;
			this.InflictedFireDamage = 0;
			this.ModifiedFireDamage = 0;
			this.ReflectedDamage = 0;
			this.AttackProgress = 0f;
			this.BodyPartHit = BoneBodyPartType.None;
			this.Distance = distance;
		}

		// Token: 0x06001CAB RID: 7339 RVA: 0x00062952 File Offset: 0x00060B52
		public void SetVictimAgent(Agent victimAgent)
		{
			if (((victimAgent != null) ? victimAgent.MissionPeer : null) != null)
			{
				this.VictimAgentName = victimAgent.MissionPeer.DisplayedName;
				return;
			}
			this.VictimAgentName = ((victimAgent != null) ? victimAgent.Name : null);
		}

		// Token: 0x040009D2 RID: 2514
		private const string DetailTagStart = "<Detail>";

		// Token: 0x040009D3 RID: 2515
		private const string DetailTagEnd = "</Detail>";

		// Token: 0x040009D4 RID: 2516
		private const uint DamageReceivedColor = 4292917946U;

		// Token: 0x040009D5 RID: 2517
		private const uint DamageDealedColor = 4210351871U;

		// Token: 0x040009D6 RID: 2518
		private static List<ValueTuple<string, uint>> _logStringCache = new List<ValueTuple<string, uint>>();

		// Token: 0x040009D7 RID: 2519
		public readonly bool IsVictimAgentSameAsAttackerAgent;

		// Token: 0x040009D8 RID: 2520
		public readonly bool IsVictimRiderAgentSameAsAttackerAgent;

		// Token: 0x040009D9 RID: 2521
		public readonly bool IsAttackerAgentHuman;

		// Token: 0x040009DA RID: 2522
		public readonly bool IsAttackerAgentMine;

		// Token: 0x040009DB RID: 2523
		public readonly bool DoesAttackerAgentHaveRiderAgent;

		// Token: 0x040009DC RID: 2524
		public readonly bool IsAttackerAgentRiderAgentMine;

		// Token: 0x040009DD RID: 2525
		public readonly bool IsAttackerAgentMount;

		// Token: 0x040009DE RID: 2526
		public readonly bool IsVictimAgentHuman;

		// Token: 0x040009DF RID: 2527
		public readonly bool IsVictimAgentMine;

		// Token: 0x040009E0 RID: 2528
		public readonly bool DoesVictimAgentHaveRiderAgent;

		// Token: 0x040009E1 RID: 2529
		public readonly bool IsVictimAgentRiderAgentMine;

		// Token: 0x040009E2 RID: 2530
		public readonly bool IsVictimAgentMount;

		// Token: 0x040009E3 RID: 2531
		public MissionObject MissionObjectHit;

		// Token: 0x040009E4 RID: 2532
		public DamageTypes DamageType;

		// Token: 0x040009E5 RID: 2533
		public bool CrushedThrough;

		// Token: 0x040009E6 RID: 2534
		public bool Chamber;

		// Token: 0x040009E7 RID: 2535
		public bool IsRangedAttack;

		// Token: 0x040009E8 RID: 2536
		public bool IsFriendlyFire;

		// Token: 0x040009E9 RID: 2537
		public bool IsFatalDamage;

		// Token: 0x040009EA RID: 2538
		public bool IsSpecialDamage;

		// Token: 0x040009EB RID: 2539
		public bool IsEntityToEntityCollisionDamage;

		// Token: 0x040009EC RID: 2540
		public bool IsSneakAttack;

		// Token: 0x040009ED RID: 2541
		public BoneBodyPartType BodyPartHit;

		// Token: 0x040009EE RID: 2542
		public string VictimAgentName;

		// Token: 0x040009EF RID: 2543
		public float HitSpeed;

		// Token: 0x040009F0 RID: 2544
		public int InflictedDamage;

		// Token: 0x040009F1 RID: 2545
		public int AbsorbedDamage;

		// Token: 0x040009F2 RID: 2546
		public int ModifiedDamage;

		// Token: 0x040009F3 RID: 2547
		public int InflictedFireDamage;

		// Token: 0x040009F4 RID: 2548
		public int ModifiedFireDamage;

		// Token: 0x040009F5 RID: 2549
		public int ReflectedDamage;

		// Token: 0x040009F7 RID: 2551
		public float Distance;
	}
}
