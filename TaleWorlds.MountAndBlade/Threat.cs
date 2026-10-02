using System;
using System.Diagnostics;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200018C RID: 396
	public class Threat
	{
		// Token: 0x06001509 RID: 5385 RVA: 0x0004F0BC File Offset: 0x0004D2BC
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x170004B7 RID: 1207
		// (get) Token: 0x0600150A RID: 5386 RVA: 0x0004F0C4 File Offset: 0x0004D2C4
		public string Name
		{
			get
			{
				if (this.TargetableObject != null)
				{
					return this.TargetableObject.Entity().Name;
				}
				if (this.Agent != null)
				{
					return this.Agent.Name.ToString();
				}
				if (this.Formation != null)
				{
					return this.Formation.ToString();
				}
				Debug.FailedAssert("Invalid threat", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\AI\\Threat.cs", "Name", 39);
				return "Invalid";
			}
		}

		// Token: 0x170004B8 RID: 1208
		// (get) Token: 0x0600150B RID: 5387 RVA: 0x0004F138 File Offset: 0x0004D338
		public Vec3 TargetingPosition
		{
			get
			{
				if (this.TargetableObject != null)
				{
					ValueTuple<Vec3, Vec3> valueTuple = this.TargetableObject.ComputeGlobalPhysicsBoundingBoxMinMax();
					Vec3 item = valueTuple.Item1;
					return (valueTuple.Item2 + item) * 0.5f + this.TargetableObject.GetTargetingOffset();
				}
				if (this.Agent != null)
				{
					return this.Agent.CollisionCapsuleCenter;
				}
				if (this.Formation != null)
				{
					return this.Formation.GetMedianAgent(false, false, this.Formation.GetAveragePositionOfUnits(false, false)).Position;
				}
				Debug.FailedAssert("Invalid threat", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\AI\\Threat.cs", "TargetingPosition", 64);
				return Vec3.Invalid;
			}
		}

		// Token: 0x0600150C RID: 5388 RVA: 0x0004F1DC File Offset: 0x0004D3DC
		public ValueTuple<Vec3, Vec3> ComputeGlobalTargetingBoundingBoxMinMax()
		{
			if (this.TargetableObject != null)
			{
				ValueTuple<Vec3, Vec3> valueTuple = this.TargetableObject.ComputeGlobalPhysicsBoundingBoxMinMax();
				Vec3 item = valueTuple.Item1;
				Vec3 item2 = valueTuple.Item2;
				return new ValueTuple<Vec3, Vec3>(item + this.TargetableObject.GetTargetingOffset(), item2 + this.TargetableObject.GetTargetingOffset());
			}
			if (this.Agent != null)
			{
				return this.Agent.CollisionCapsule.GetBoxMinMax();
			}
			if (this.Formation != null)
			{
				Debug.FailedAssert("Nobody should be requesting a bounding box for a formation", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\AI\\Threat.cs", "ComputeGlobalTargetingBoundingBoxMinMax", 83);
				return new ValueTuple<Vec3, Vec3>(Vec3.Invalid, Vec3.Invalid);
			}
			return new ValueTuple<Vec3, Vec3>(Vec3.Invalid, Vec3.Invalid);
		}

		// Token: 0x0600150D RID: 5389 RVA: 0x0004F28C File Offset: 0x0004D48C
		public Vec3 GetGlobalVelocity()
		{
			if (this.TargetableObject != null)
			{
				return this.TargetableObject.GetTargetGlobalVelocity();
			}
			if (this.Agent != null)
			{
				return new Vec3(this.Agent.GetAverageRealGlobalVelocity().AsVec2, 0f, -1f);
			}
			return Vec3.Zero;
		}

		// Token: 0x0600150E RID: 5390 RVA: 0x0004F2E0 File Offset: 0x0004D4E0
		public override bool Equals(object obj)
		{
			Threat threat;
			return (threat = obj as Threat) != null && this.TargetableObject == threat.TargetableObject && this.Formation == threat.Formation;
		}

		// Token: 0x0600150F RID: 5391 RVA: 0x0004F317 File Offset: 0x0004D517
		[Conditional("DEBUG")]
		public void DisplayDebugInfo()
		{
		}

		// Token: 0x040005C3 RID: 1475
		public ITargetable TargetableObject;

		// Token: 0x040005C4 RID: 1476
		public Formation Formation;

		// Token: 0x040005C5 RID: 1477
		public Agent Agent;

		// Token: 0x040005C6 RID: 1478
		public float ThreatValue;

		// Token: 0x040005C7 RID: 1479
		public bool ForceTarget;
	}
}
