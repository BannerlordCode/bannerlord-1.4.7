using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001DF RID: 479
	public struct MBTeam
	{
		// Token: 0x06001C40 RID: 7232 RVA: 0x0006106B File Offset: 0x0005F26B
		internal MBTeam(Mission mission, int index)
		{
			this._mission = mission;
			this.Index = index;
		}

		// Token: 0x170005B3 RID: 1459
		// (get) Token: 0x06001C41 RID: 7233 RVA: 0x0006107B File Offset: 0x0005F27B
		public static MBTeam InvalidTeam
		{
			get
			{
				return new MBTeam(null, -1);
			}
		}

		// Token: 0x06001C42 RID: 7234 RVA: 0x00061084 File Offset: 0x0005F284
		public override int GetHashCode()
		{
			return this.Index;
		}

		// Token: 0x06001C43 RID: 7235 RVA: 0x0006108C File Offset: 0x0005F28C
		public override bool Equals(object obj)
		{
			return ((MBTeam)obj).Index == this.Index;
		}

		// Token: 0x06001C44 RID: 7236 RVA: 0x000610A1 File Offset: 0x0005F2A1
		public static bool operator ==(MBTeam team1, MBTeam team2)
		{
			return team1.Index == team2.Index;
		}

		// Token: 0x06001C45 RID: 7237 RVA: 0x000610B1 File Offset: 0x0005F2B1
		public static bool operator !=(MBTeam team1, MBTeam team2)
		{
			return team1.Index != team2.Index;
		}

		// Token: 0x170005B4 RID: 1460
		// (get) Token: 0x06001C46 RID: 7238 RVA: 0x000610C4 File Offset: 0x0005F2C4
		public bool IsValid
		{
			get
			{
				return this.Index >= 0;
			}
		}

		// Token: 0x06001C47 RID: 7239 RVA: 0x000610D2 File Offset: 0x0005F2D2
		public bool IsEnemyOf(MBTeam otherTeam)
		{
			return MBAPI.IMBTeam.IsEnemy(this._mission.Pointer, this.Index, otherTeam.Index);
		}

		// Token: 0x06001C48 RID: 7240 RVA: 0x000610F5 File Offset: 0x0005F2F5
		public void SetIsEnemyOf(MBTeam otherTeam, bool isEnemyOf)
		{
			MBAPI.IMBTeam.SetIsEnemy(this._mission.Pointer, this.Index, otherTeam.Index, isEnemyOf);
		}

		// Token: 0x06001C49 RID: 7241 RVA: 0x00061119 File Offset: 0x0005F319
		public override string ToString()
		{
			return "Mission Team: " + this.Index;
		}

		// Token: 0x04000987 RID: 2439
		public readonly int Index;

		// Token: 0x04000988 RID: 2440
		private readonly Mission _mission;
	}
}
