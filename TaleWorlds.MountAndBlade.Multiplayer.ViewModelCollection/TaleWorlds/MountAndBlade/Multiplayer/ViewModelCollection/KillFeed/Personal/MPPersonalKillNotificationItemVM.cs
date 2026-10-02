using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.KillFeed.Personal
{
	// Token: 0x0200008A RID: 138
	public class MPPersonalKillNotificationItemVM : ViewModel
	{
		// Token: 0x17000466 RID: 1126
		// (get) Token: 0x06000D6B RID: 3435 RVA: 0x00029451 File Offset: 0x00027651
		// (set) Token: 0x06000D6C RID: 3436 RVA: 0x00029459 File Offset: 0x00027659
		private MPPersonalKillNotificationItemVM.ItemTypes ItemTypeAsEnum
		{
			get
			{
				return this._itemTypeAsEnum;
			}
			set
			{
				this._itemType = (int)value;
				this._itemTypeAsEnum = value;
			}
		}

		// Token: 0x06000D6D RID: 3437 RVA: 0x0002946C File Offset: 0x0002766C
		public MPPersonalKillNotificationItemVM(int amount, bool isFatal, bool isMountDamage, bool isFriendlyFire, bool isHeadshot, string killedAgentName, Action<MPPersonalKillNotificationItemVM> onRemoveItem)
		{
			this._onRemoveItem = onRemoveItem;
			this.Amount = amount;
			if (isFriendlyFire)
			{
				this.ItemTypeAsEnum = (isFatal ? MPPersonalKillNotificationItemVM.ItemTypes.FriendlyFireKill : MPPersonalKillNotificationItemVM.ItemTypes.FriendlyFireDamage);
				this.Message = killedAgentName;
				return;
			}
			if (isMountDamage)
			{
				this.ItemTypeAsEnum = MPPersonalKillNotificationItemVM.ItemTypes.MountDamage;
				this.Message = GameTexts.FindText("str_damage_delivered_message", null).ToString();
				return;
			}
			if (isFatal)
			{
				this.ItemTypeAsEnum = (isHeadshot ? MPPersonalKillNotificationItemVM.ItemTypes.HeadshotKill : MPPersonalKillNotificationItemVM.ItemTypes.NormalKill);
				this.Message = killedAgentName;
				return;
			}
			this.ItemTypeAsEnum = MPPersonalKillNotificationItemVM.ItemTypes.NormalDamage;
			this.Message = GameTexts.FindText("str_damage_delivered_message", null).ToString();
		}

		// Token: 0x06000D6E RID: 3438 RVA: 0x00029500 File Offset: 0x00027700
		public MPPersonalKillNotificationItemVM(int amount, GoldGainFlags reasonType, Action<MPPersonalKillNotificationItemVM> onRemoveItem)
		{
			this._onRemoveItem = onRemoveItem;
			this.ItemTypeAsEnum = MPPersonalKillNotificationItemVM.ItemTypes.GoldChange;
			if (reasonType <= GoldGainFlags.TenthKill)
			{
				if (reasonType <= GoldGainFlags.SecondAssist)
				{
					switch (reasonType)
					{
					case GoldGainFlags.FirstRangedKill:
						this.Message = GameTexts.FindText("str_gold_gain_first_ranged_kill", null).ToString();
						goto IL_0200;
					case GoldGainFlags.FirstMeleeKill:
						this.Message = GameTexts.FindText("str_gold_gain_first_melee_kill", null).ToString();
						goto IL_0200;
					case GoldGainFlags.FirstRangedKill | GoldGainFlags.FirstMeleeKill:
						break;
					case GoldGainFlags.FirstAssist:
						this.Message = GameTexts.FindText("str_gold_gain_first_assist", null).ToString();
						goto IL_0200;
					default:
						if (reasonType == GoldGainFlags.SecondAssist)
						{
							this.Message = GameTexts.FindText("str_gold_gain_second_assist", null).ToString();
							goto IL_0200;
						}
						break;
					}
				}
				else
				{
					if (reasonType == GoldGainFlags.ThirdAssist)
					{
						this.Message = GameTexts.FindText("str_gold_gain_third_assist", null).ToString();
						goto IL_0200;
					}
					if (reasonType == GoldGainFlags.FifthKill)
					{
						this.Message = GameTexts.FindText("str_gold_gain_fifth_kill", null).ToString();
						goto IL_0200;
					}
					if (reasonType == GoldGainFlags.TenthKill)
					{
						this.Message = GameTexts.FindText("str_gold_gain_tenth_kill", null).ToString();
						goto IL_0200;
					}
				}
			}
			else if (reasonType <= GoldGainFlags.DefaultAssist)
			{
				if (reasonType == GoldGainFlags.DefaultKill)
				{
					this.Message = GameTexts.FindText("str_gold_gain_default_kill", null).ToString();
					goto IL_0200;
				}
				if (reasonType == GoldGainFlags.DefaultAssist)
				{
					this.Message = GameTexts.FindText("str_gold_gain_default_assist", null).ToString();
					goto IL_0200;
				}
			}
			else
			{
				if (reasonType == GoldGainFlags.ObjectiveCompleted)
				{
					this.Message = GameTexts.FindText("str_gold_gain_objective_completed", null).ToString();
					goto IL_0200;
				}
				if (reasonType == GoldGainFlags.ObjectiveDestroyed)
				{
					this.Message = GameTexts.FindText("str_gold_gain_objective_destroyed", null).ToString();
					goto IL_0200;
				}
				if (reasonType == GoldGainFlags.PerkBonus)
				{
					this.Message = GameTexts.FindText("str_gold_gain_perk_bonus", null).ToString();
					goto IL_0200;
				}
			}
			Debug.FailedAssert("Undefined gold change type", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection\\KillFeed\\Personal\\MPPersonalKillNotificationItemVM.cs", ".ctor", 117);
			this.Message = "";
			IL_0200:
			this.Amount = amount;
		}

		// Token: 0x06000D6F RID: 3439 RVA: 0x00029714 File Offset: 0x00027914
		public MPPersonalKillNotificationItemVM(string victimAgentName, Action<MPPersonalKillNotificationItemVM> onRemoveItem)
		{
			this._onRemoveItem = onRemoveItem;
			this.Amount = -1;
			this.Message = victimAgentName;
			this.ItemTypeAsEnum = MPPersonalKillNotificationItemVM.ItemTypes.Assist;
		}

		// Token: 0x06000D70 RID: 3440 RVA: 0x00029738 File Offset: 0x00027938
		public void ExecuteRemove()
		{
			this._onRemoveItem(this);
		}

		// Token: 0x17000467 RID: 1127
		// (get) Token: 0x06000D71 RID: 3441 RVA: 0x00029746 File Offset: 0x00027946
		// (set) Token: 0x06000D72 RID: 3442 RVA: 0x0002974E File Offset: 0x0002794E
		[DataSourceProperty]
		public string Message
		{
			get
			{
				return this._message;
			}
			set
			{
				if (value != this._message)
				{
					this._message = value;
					base.OnPropertyChangedWithValue<string>(value, "Message");
				}
			}
		}

		// Token: 0x17000468 RID: 1128
		// (get) Token: 0x06000D73 RID: 3443 RVA: 0x00029771 File Offset: 0x00027971
		// (set) Token: 0x06000D74 RID: 3444 RVA: 0x00029779 File Offset: 0x00027979
		[DataSourceProperty]
		public int ItemType
		{
			get
			{
				return this._itemType;
			}
			set
			{
				if (value != this._itemType)
				{
					this._itemType = value;
					base.OnPropertyChangedWithValue(value, "ItemType");
				}
			}
		}

		// Token: 0x17000469 RID: 1129
		// (get) Token: 0x06000D75 RID: 3445 RVA: 0x00029797 File Offset: 0x00027997
		// (set) Token: 0x06000D76 RID: 3446 RVA: 0x0002979F File Offset: 0x0002799F
		[DataSourceProperty]
		public int Amount
		{
			get
			{
				return this._amount;
			}
			set
			{
				if (value != this._amount)
				{
					this._amount = value;
					base.OnPropertyChangedWithValue(value, "Amount");
				}
			}
		}

		// Token: 0x04000621 RID: 1569
		private Action<MPPersonalKillNotificationItemVM> _onRemoveItem;

		// Token: 0x04000622 RID: 1570
		private MPPersonalKillNotificationItemVM.ItemTypes _itemTypeAsEnum;

		// Token: 0x04000623 RID: 1571
		private string _message;

		// Token: 0x04000624 RID: 1572
		private int _amount;

		// Token: 0x04000625 RID: 1573
		private int _itemType;

		// Token: 0x02000176 RID: 374
		private enum ItemTypes
		{
			// Token: 0x04000A12 RID: 2578
			NormalDamage,
			// Token: 0x04000A13 RID: 2579
			FriendlyFireDamage,
			// Token: 0x04000A14 RID: 2580
			FriendlyFireKill,
			// Token: 0x04000A15 RID: 2581
			MountDamage,
			// Token: 0x04000A16 RID: 2582
			NormalKill,
			// Token: 0x04000A17 RID: 2583
			Assist,
			// Token: 0x04000A18 RID: 2584
			GoldChange,
			// Token: 0x04000A19 RID: 2585
			HeadshotKill
		}
	}
}
