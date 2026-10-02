using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002DC RID: 732
	public class MonsterMissionData : IMonsterMissionData
	{
		// Token: 0x170007F1 RID: 2033
		// (get) Token: 0x06002A93 RID: 10899 RVA: 0x000A3E5A File Offset: 0x000A205A
		// (set) Token: 0x06002A94 RID: 10900 RVA: 0x000A3E62 File Offset: 0x000A2062
		public Monster Monster { get; private set; }

		// Token: 0x170007F2 RID: 2034
		// (get) Token: 0x06002A95 RID: 10901 RVA: 0x000A3E6B File Offset: 0x000A206B
		public CapsuleData BodyCapsule
		{
			get
			{
				return new CapsuleData(this.Monster.BodyCapsuleRadius, this.Monster.BodyCapsulePoint1, this.Monster.BodyCapsulePoint2);
			}
		}

		// Token: 0x170007F3 RID: 2035
		// (get) Token: 0x06002A96 RID: 10902 RVA: 0x000A3E93 File Offset: 0x000A2093
		public CapsuleData CrouchedBodyCapsule
		{
			get
			{
				return new CapsuleData(this.Monster.CrouchedBodyCapsuleRadius, this.Monster.CrouchedBodyCapsulePoint1, this.Monster.CrouchedBodyCapsulePoint2);
			}
		}

		// Token: 0x170007F4 RID: 2036
		// (get) Token: 0x06002A97 RID: 10903 RVA: 0x000A3EBB File Offset: 0x000A20BB
		public MBActionSet ActionSet
		{
			get
			{
				if (!this._actionSet.IsValid && !string.IsNullOrEmpty(this.Monster.ActionSetCode))
				{
					this._actionSet = MBActionSet.GetActionSet(this.Monster.ActionSetCode);
				}
				return this._actionSet;
			}
		}

		// Token: 0x170007F5 RID: 2037
		// (get) Token: 0x06002A98 RID: 10904 RVA: 0x000A3EF8 File Offset: 0x000A20F8
		public MBActionSet FemaleActionSet
		{
			get
			{
				if (!this._femaleActionSet.IsValid && !string.IsNullOrEmpty(this.Monster.FemaleActionSetCode))
				{
					this._femaleActionSet = MBActionSet.GetActionSet(this.Monster.FemaleActionSetCode);
				}
				return this._femaleActionSet;
			}
		}

		// Token: 0x06002A99 RID: 10905 RVA: 0x000A3F35 File Offset: 0x000A2135
		public MonsterMissionData(Monster monster)
		{
			this._actionSet = MBActionSet.InvalidActionSet;
			this._femaleActionSet = MBActionSet.InvalidActionSet;
			this.Monster = monster;
		}

		// Token: 0x04001036 RID: 4150
		private MBActionSet _actionSet;

		// Token: 0x04001037 RID: 4151
		private MBActionSet _femaleActionSet;
	}
}
