using System;

namespace TaleWorlds.GauntletUI
{
	// Token: 0x0200002C RID: 44
	public readonly struct ImageFitResult
	{
		// Token: 0x0600034A RID: 842 RVA: 0x0000EDC7 File Offset: 0x0000CFC7
		public ImageFitResult(float offsetX, float offsetY, float width, float height)
		{
			this.OffsetX = offsetX;
			this.OffsetY = offsetY;
			this.Width = width;
			this.Height = height;
		}

		// Token: 0x0400019B RID: 411
		public readonly float OffsetX;

		// Token: 0x0400019C RID: 412
		public readonly float OffsetY;

		// Token: 0x0400019D RID: 413
		public readonly float Width;

		// Token: 0x0400019E RID: 414
		public readonly float Height;
	}
}
