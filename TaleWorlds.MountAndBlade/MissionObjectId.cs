using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000259 RID: 601
	public struct MissionObjectId
	{
		// Token: 0x06002239 RID: 8761 RVA: 0x0007852A File Offset: 0x0007672A
		public MissionObjectId(int id, bool createdAtRuntime = false)
		{
			this.Id = id;
			this.CreatedAtRuntime = createdAtRuntime;
		}

		// Token: 0x0600223A RID: 8762 RVA: 0x0007853A File Offset: 0x0007673A
		public static bool operator ==(MissionObjectId a, MissionObjectId b)
		{
			return a.Id == b.Id && a.CreatedAtRuntime == b.CreatedAtRuntime;
		}

		// Token: 0x0600223B RID: 8763 RVA: 0x0007855A File Offset: 0x0007675A
		public static bool operator !=(MissionObjectId a, MissionObjectId b)
		{
			return a.Id != b.Id || a.CreatedAtRuntime != b.CreatedAtRuntime;
		}

		// Token: 0x0600223C RID: 8764 RVA: 0x00078580 File Offset: 0x00076780
		public override bool Equals(object obj)
		{
			if (!(obj is MissionObjectId))
			{
				return false;
			}
			MissionObjectId missionObjectId = (MissionObjectId)obj;
			return missionObjectId.Id == this.Id && missionObjectId.CreatedAtRuntime == this.CreatedAtRuntime;
		}

		// Token: 0x0600223D RID: 8765 RVA: 0x000785BC File Offset: 0x000767BC
		public override int GetHashCode()
		{
			int num = this.Id;
			if (this.CreatedAtRuntime)
			{
				num |= 1073741824;
			}
			return num.GetHashCode();
		}

		// Token: 0x0600223E RID: 8766 RVA: 0x000785E8 File Offset: 0x000767E8
		public override string ToString()
		{
			return this.Id + " - " + this.CreatedAtRuntime.ToString();
		}

		// Token: 0x04000D4B RID: 3403
		public readonly int Id;

		// Token: 0x04000D4C RID: 3404
		public readonly bool CreatedAtRuntime;

		// Token: 0x04000D4D RID: 3405
		public static readonly MissionObjectId Invalid = new MissionObjectId(-1, false);
	}
}
