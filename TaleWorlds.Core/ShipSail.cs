using System;
using TaleWorlds.Library;

namespace TaleWorlds.Core
{
	// Token: 0x020000CD RID: 205
	public class ShipSail
	{
		// Token: 0x06000B11 RID: 2833 RVA: 0x00023E8E File Offset: 0x0002208E
		public ShipSail(SailType type, float forceMultiplier, float leftRotationLimit, float rightRotationLimit, float rotationRate)
		{
			this.Type = type;
			this.ForceMultiplier = forceMultiplier;
			this.LeftRotationLimit = leftRotationLimit;
			this.RightRotationLimit = rightRotationLimit;
			this.RotationRate = rotationRate;
		}

		// Token: 0x06000B12 RID: 2834 RVA: 0x00023EBC File Offset: 0x000220BC
		public bool NearlyEquals(ShipSail otherShipSail)
		{
			return this.Type == otherShipSail.Type && this.ForceMultiplier.ApproximatelyEqualsTo(otherShipSail.ForceMultiplier, 1E-05f) && this.LeftRotationLimit.ApproximatelyEqualsTo(otherShipSail.LeftRotationLimit, 1E-05f) && this.RightRotationLimit.ApproximatelyEqualsTo(otherShipSail.RightRotationLimit, 1E-05f) && this.RotationRate.ApproximatelyEqualsTo(otherShipSail.RotationRate, 1E-05f);
		}

		// Token: 0x04000619 RID: 1561
		public readonly SailType Type;

		// Token: 0x0400061A RID: 1562
		public readonly float ForceMultiplier;

		// Token: 0x0400061B RID: 1563
		public readonly float LeftRotationLimit;

		// Token: 0x0400061C RID: 1564
		public readonly float RightRotationLimit;

		// Token: 0x0400061D RID: 1565
		public readonly float RotationRate;
	}
}
