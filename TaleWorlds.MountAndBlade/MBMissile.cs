using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001CE RID: 462
	public abstract class MBMissile
	{
		// Token: 0x06001BB4 RID: 7092 RVA: 0x000604F2 File Offset: 0x0005E6F2
		protected MBMissile(Mission mission)
		{
			this._mission = mission;
		}

		// Token: 0x1700059B RID: 1435
		// (get) Token: 0x06001BB5 RID: 7093 RVA: 0x00060501 File Offset: 0x0005E701
		// (set) Token: 0x06001BB6 RID: 7094 RVA: 0x00060509 File Offset: 0x0005E709
		public int Index { get; set; }

		// Token: 0x06001BB7 RID: 7095 RVA: 0x00060512 File Offset: 0x0005E712
		public Vec3 GetPosition()
		{
			return MBAPI.IMBMission.GetPositionOfMissile(this._mission.Pointer, this.Index);
		}

		// Token: 0x06001BB8 RID: 7096 RVA: 0x0006052F File Offset: 0x0005E72F
		public Vec3 GetOldPosition()
		{
			return MBAPI.IMBMission.GetOldPositionOfMissile(this._mission.Pointer, this.Index);
		}

		// Token: 0x06001BB9 RID: 7097 RVA: 0x0006054C File Offset: 0x0005E74C
		public Vec3 GetVelocity()
		{
			return MBAPI.IMBMission.GetVelocityOfMissile(this._mission.Pointer, this.Index);
		}

		// Token: 0x06001BBA RID: 7098 RVA: 0x00060569 File Offset: 0x0005E769
		public void SetVelocity(in Vec3 velocity)
		{
			MBAPI.IMBMission.SetVelocityOfMissile(this._mission.Pointer, this.Index, in velocity);
		}

		// Token: 0x06001BBB RID: 7099 RVA: 0x00060587 File Offset: 0x0005E787
		public bool GetHasRigidBody()
		{
			return MBAPI.IMBMission.GetMissileHasRigidBody(this._mission.Pointer, this.Index);
		}

		// Token: 0x04000910 RID: 2320
		private readonly Mission _mission;
	}
}
