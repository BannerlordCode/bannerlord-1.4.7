using System;
using System.Collections.Generic;
using System.Linq;

namespace TaleWorlds.GauntletUI
{
	// Token: 0x0200000E RID: 14
	public class BrushAnimationProperty
	{
		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000DD RID: 221 RVA: 0x00003CB6 File Offset: 0x00001EB6
		// (set) Token: 0x060000DE RID: 222 RVA: 0x00003CBE File Offset: 0x00001EBE
		public string LayerName { get; set; }

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000DF RID: 223 RVA: 0x00003CC7 File Offset: 0x00001EC7
		public IEnumerable<BrushAnimationKeyFrame> KeyFrames
		{
			get
			{
				return this._keyFrames.AsReadOnly();
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000E0 RID: 224 RVA: 0x00003CD4 File Offset: 0x00001ED4
		public int Count
		{
			get
			{
				return this._keyFrames.Count;
			}
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00003CE1 File Offset: 0x00001EE1
		public BrushAnimationProperty()
		{
			this._keyFrames = new List<BrushAnimationKeyFrame>();
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00003CF4 File Offset: 0x00001EF4
		public BrushAnimationKeyFrame GetFrameAfter(float time)
		{
			for (int i = 0; i < this._keyFrames.Count; i++)
			{
				BrushAnimationKeyFrame brushAnimationKeyFrame = this._keyFrames[i];
				if (time < brushAnimationKeyFrame.Time)
				{
					return brushAnimationKeyFrame;
				}
			}
			return null;
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00003D30 File Offset: 0x00001F30
		public BrushAnimationKeyFrame GetFrameAt(int i)
		{
			if (i >= 0 && i < this._keyFrames.Count)
			{
				return this._keyFrames[i];
			}
			return null;
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00003D52 File Offset: 0x00001F52
		public BrushAnimationProperty Clone()
		{
			BrushAnimationProperty brushAnimationProperty = new BrushAnimationProperty();
			brushAnimationProperty.FillFrom(this);
			return brushAnimationProperty;
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00003D60 File Offset: 0x00001F60
		private void FillFrom(BrushAnimationProperty collection)
		{
			this.PropertyType = collection.PropertyType;
			this.LayerName = collection.LayerName;
			this._keyFrames = new List<BrushAnimationKeyFrame>(collection._keyFrames.Count);
			for (int i = 0; i < collection._keyFrames.Count; i++)
			{
				BrushAnimationKeyFrame brushAnimationKeyFrame = collection._keyFrames[i].Clone();
				this._keyFrames.Add(brushAnimationKeyFrame);
			}
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00003DD0 File Offset: 0x00001FD0
		public void AddKeyFrame(BrushAnimationKeyFrame keyFrame)
		{
			this._keyFrames.Add(keyFrame);
			this._keyFrames = this._keyFrames.OrderBy<BrushAnimationKeyFrame, float>((BrushAnimationKeyFrame k) => k.Time).ToList<BrushAnimationKeyFrame>();
			for (int i = 0; i < this._keyFrames.Count; i++)
			{
				this._keyFrames[i].InitializeIndex(i);
			}
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00003E46 File Offset: 0x00002046
		public void RemoveKeyFrame(BrushAnimationKeyFrame keyFrame)
		{
			this._keyFrames.Remove(keyFrame);
		}

		// Token: 0x0400003E RID: 62
		public BrushAnimationProperty.BrushAnimationPropertyType PropertyType;

		// Token: 0x0400003F RID: 63
		private List<BrushAnimationKeyFrame> _keyFrames;

		// Token: 0x02000077 RID: 119
		public enum BrushAnimationPropertyType
		{
			// Token: 0x040003FD RID: 1021
			Name,
			// Token: 0x040003FE RID: 1022
			ColorFactor,
			// Token: 0x040003FF RID: 1023
			Color,
			// Token: 0x04000400 RID: 1024
			AlphaFactor,
			// Token: 0x04000401 RID: 1025
			HueFactor,
			// Token: 0x04000402 RID: 1026
			SaturationFactor,
			// Token: 0x04000403 RID: 1027
			ValueFactor,
			// Token: 0x04000404 RID: 1028
			FontColor,
			// Token: 0x04000405 RID: 1029
			OverlayXOffset,
			// Token: 0x04000406 RID: 1030
			OverlayYOffset,
			// Token: 0x04000407 RID: 1031
			TextGlowColor,
			// Token: 0x04000408 RID: 1032
			TextOutlineColor,
			// Token: 0x04000409 RID: 1033
			TextOutlineAmount,
			// Token: 0x0400040A RID: 1034
			TextGlowRadius,
			// Token: 0x0400040B RID: 1035
			TextBlur,
			// Token: 0x0400040C RID: 1036
			TextShadowOffset,
			// Token: 0x0400040D RID: 1037
			TextShadowAngle,
			// Token: 0x0400040E RID: 1038
			TextColorFactor,
			// Token: 0x0400040F RID: 1039
			TextAlphaFactor,
			// Token: 0x04000410 RID: 1040
			TextHueFactor,
			// Token: 0x04000411 RID: 1041
			TextSaturationFactor,
			// Token: 0x04000412 RID: 1042
			TextValueFactor,
			// Token: 0x04000413 RID: 1043
			Sprite,
			// Token: 0x04000414 RID: 1044
			IsHidden,
			// Token: 0x04000415 RID: 1045
			XOffset,
			// Token: 0x04000416 RID: 1046
			YOffset,
			// Token: 0x04000417 RID: 1047
			Rotation,
			// Token: 0x04000418 RID: 1048
			OverridenWidth,
			// Token: 0x04000419 RID: 1049
			OverridenHeight,
			// Token: 0x0400041A RID: 1050
			WidthPolicy,
			// Token: 0x0400041B RID: 1051
			HeightPolicy,
			// Token: 0x0400041C RID: 1052
			HorizontalFlip,
			// Token: 0x0400041D RID: 1053
			VerticalFlip,
			// Token: 0x0400041E RID: 1054
			OverlayMethod,
			// Token: 0x0400041F RID: 1055
			OverlaySprite,
			// Token: 0x04000420 RID: 1056
			ExtendLeft,
			// Token: 0x04000421 RID: 1057
			ExtendRight,
			// Token: 0x04000422 RID: 1058
			ExtendTop,
			// Token: 0x04000423 RID: 1059
			ExtendBottom,
			// Token: 0x04000424 RID: 1060
			UseRandomBaseOverlayXOffset,
			// Token: 0x04000425 RID: 1061
			UseRandomBaseOverlayYOffset,
			// Token: 0x04000426 RID: 1062
			Font,
			// Token: 0x04000427 RID: 1063
			FontStyle,
			// Token: 0x04000428 RID: 1064
			FontSize
		}
	}
}
