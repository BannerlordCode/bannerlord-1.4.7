using System;
using System.Numerics;
using TaleWorlds.Library;

namespace TaleWorlds.GauntletUI
{
	// Token: 0x0200002B RID: 43
	public class ImageFit
	{
		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x0600033B RID: 827 RVA: 0x0000EB29 File Offset: 0x0000CD29
		// (set) Token: 0x0600033C RID: 828 RVA: 0x0000EB31 File Offset: 0x0000CD31
		public ImageFit.ImageFitTypes Type { get; set; }

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x0600033D RID: 829 RVA: 0x0000EB3A File Offset: 0x0000CD3A
		// (set) Token: 0x0600033E RID: 830 RVA: 0x0000EB42 File Offset: 0x0000CD42
		public ImageFit.ImageHorizontalAlignments HorizontalAlignment { get; set; }

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x0600033F RID: 831 RVA: 0x0000EB4B File Offset: 0x0000CD4B
		// (set) Token: 0x06000340 RID: 832 RVA: 0x0000EB53 File Offset: 0x0000CD53
		public ImageFit.ImageVerticalAlignments VerticalAlignment { get; set; }

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x06000341 RID: 833 RVA: 0x0000EB5C File Offset: 0x0000CD5C
		// (set) Token: 0x06000342 RID: 834 RVA: 0x0000EB64 File Offset: 0x0000CD64
		public float OffsetX { get; set; }

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x06000343 RID: 835 RVA: 0x0000EB6D File Offset: 0x0000CD6D
		// (set) Token: 0x06000344 RID: 836 RVA: 0x0000EB75 File Offset: 0x0000CD75
		public float OffsetY { get; set; }

		// Token: 0x06000345 RID: 837 RVA: 0x0000EB7E File Offset: 0x0000CD7E
		public ImageFit()
		{
			this.Type = ImageFit.ImageFitTypes.StretchToFit;
			this.HorizontalAlignment = ImageFit.ImageHorizontalAlignments.Center;
			this.VerticalAlignment = ImageFit.ImageVerticalAlignments.Center;
		}

		// Token: 0x06000346 RID: 838 RVA: 0x0000EB9C File Offset: 0x0000CD9C
		public ImageFitResult GetFittedRectangle(in Vector2 containerSize, in Vector2 imageSize)
		{
			switch (this.Type)
			{
			case ImageFit.ImageFitTypes.StretchToFit:
				return new ImageFitResult(0f, 0f, containerSize.X, containerSize.Y);
			case ImageFit.ImageFitTypes.Cover:
				return this.GetRectangleForCover(in containerSize, in imageSize);
			case ImageFit.ImageFitTypes.Contain:
				return this.GetRectangleForContain(in containerSize, in imageSize);
			default:
				Debug.FailedAssert(string.Format("Image fit type not handled: {0}", this.Type), "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\GauntletUI\\TaleWorlds.GauntletUI\\ImageFit.cs", "GetFittedRectangle", 55);
				return new ImageFitResult(0f, 0f, containerSize.X, containerSize.Y);
			}
		}

		// Token: 0x06000347 RID: 839 RVA: 0x0000EC34 File Offset: 0x0000CE34
		private ImageFitResult GetRectangleForCover(in Vector2 containerSize, in Vector2 imageSize)
		{
			float num = containerSize.X / imageSize.X;
			float num2 = containerSize.Y / imageSize.Y;
			float num3 = MathF.Max(num, num2);
			float num4 = imageSize.X * num3;
			float num5 = imageSize.Y * num3;
			Vector2 vector = new Vector2(num4, num5);
			float num6;
			float num7;
			this.GetImageAlignment(in containerSize, in vector, out num6, out num7);
			return new ImageFitResult(num6, num7, num4, num5);
		}

		// Token: 0x06000348 RID: 840 RVA: 0x0000EC98 File Offset: 0x0000CE98
		private ImageFitResult GetRectangleForContain(in Vector2 containerSize, in Vector2 imageSize)
		{
			float num = containerSize.X / imageSize.X;
			float num2 = containerSize.Y / imageSize.Y;
			float num3 = MathF.Min(num, num2);
			float num4 = imageSize.X * num3;
			float num5 = imageSize.Y * num3;
			Vector2 vector = new Vector2(num4, num5);
			float num6;
			float num7;
			this.GetImageAlignment(in containerSize, in vector, out num6, out num7);
			return new ImageFitResult(num6, num7, num4, num5);
		}

		// Token: 0x06000349 RID: 841 RVA: 0x0000ECFC File Offset: 0x0000CEFC
		private void GetImageAlignment(in Vector2 containerSize, in Vector2 imageSize, out float x, out float y)
		{
			x = 0f;
			y = 0f;
			switch (this.HorizontalAlignment)
			{
			case ImageFit.ImageHorizontalAlignments.Left:
				x = 0f;
				break;
			case ImageFit.ImageHorizontalAlignments.Center:
				x = (containerSize.X - imageSize.X) * 0.5f;
				break;
			case ImageFit.ImageHorizontalAlignments.Right:
				x = containerSize.X - imageSize.X;
				break;
			}
			switch (this.VerticalAlignment)
			{
			case ImageFit.ImageVerticalAlignments.Top:
				y = 0f;
				break;
			case ImageFit.ImageVerticalAlignments.Center:
				y = (containerSize.Y - imageSize.Y) * 0.5f;
				break;
			case ImageFit.ImageVerticalAlignments.Bottom:
				y = containerSize.Y - imageSize.Y;
				break;
			}
			x += this.OffsetX;
			y += this.OffsetY;
		}

		// Token: 0x0200007C RID: 124
		public enum ImageFitTypes : byte
		{
			// Token: 0x04000436 RID: 1078
			StretchToFit,
			// Token: 0x04000437 RID: 1079
			Cover,
			// Token: 0x04000438 RID: 1080
			Contain
		}

		// Token: 0x0200007D RID: 125
		public enum ImageHorizontalAlignments : byte
		{
			// Token: 0x0400043A RID: 1082
			Left,
			// Token: 0x0400043B RID: 1083
			Center,
			// Token: 0x0400043C RID: 1084
			Right
		}

		// Token: 0x0200007E RID: 126
		public enum ImageVerticalAlignments : byte
		{
			// Token: 0x0400043E RID: 1086
			Top,
			// Token: 0x0400043F RID: 1087
			Center,
			// Token: 0x04000440 RID: 1088
			Bottom
		}
	}
}
