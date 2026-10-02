using System;
using System.Collections.Generic;

namespace TaleWorlds.GauntletUI
{
	// Token: 0x0200000C RID: 12
	public class BrushAnimation
	{
		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000BD RID: 189 RVA: 0x00003910 File Offset: 0x00001B10
		// (set) Token: 0x060000BE RID: 190 RVA: 0x00003918 File Offset: 0x00001B18
		public string Name { get; set; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000BF RID: 191 RVA: 0x00003921 File Offset: 0x00001B21
		// (set) Token: 0x060000C0 RID: 192 RVA: 0x00003929 File Offset: 0x00001B29
		public float Duration { get; set; }

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x00003932 File Offset: 0x00001B32
		// (set) Token: 0x060000C2 RID: 194 RVA: 0x0000393A File Offset: 0x00001B3A
		public bool Loop { get; set; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x00003943 File Offset: 0x00001B43
		// (set) Token: 0x060000C4 RID: 196 RVA: 0x0000394B File Offset: 0x00001B4B
		public AnimationInterpolation.Type InterpolationType { get; set; }

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000C5 RID: 197 RVA: 0x00003954 File Offset: 0x00001B54
		// (set) Token: 0x060000C6 RID: 198 RVA: 0x0000395C File Offset: 0x00001B5C
		public AnimationInterpolation.Function InterpolationFunction { get; set; }

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000C7 RID: 199 RVA: 0x00003965 File Offset: 0x00001B65
		// (set) Token: 0x060000C8 RID: 200 RVA: 0x0000396D File Offset: 0x00001B6D
		public BrushLayerAnimation StyleAnimation { get; set; }

		// Token: 0x060000C9 RID: 201 RVA: 0x00003976 File Offset: 0x00001B76
		public BrushAnimation()
		{
			this._data = new Dictionary<string, BrushLayerAnimation>();
		}

		// Token: 0x060000CA RID: 202 RVA: 0x0000398C File Offset: 0x00001B8C
		public void AddAnimationProperty(BrushAnimationProperty property)
		{
			BrushLayerAnimation brushLayerAnimation = null;
			if (string.IsNullOrEmpty(property.LayerName))
			{
				if (this.StyleAnimation == null)
				{
					this.StyleAnimation = new BrushLayerAnimation();
				}
				brushLayerAnimation = this.StyleAnimation;
			}
			else if (!this._data.TryGetValue(property.LayerName, out brushLayerAnimation))
			{
				brushLayerAnimation = new BrushLayerAnimation();
				brushLayerAnimation.LayerName = property.LayerName;
				this._data.Add(property.LayerName, brushLayerAnimation);
			}
			brushLayerAnimation.AddAnimationProperty(property);
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00003A04 File Offset: 0x00001C04
		public void RemoveAnimationProperty(BrushAnimationProperty property)
		{
			BrushLayerAnimation brushLayerAnimation;
			if (string.IsNullOrEmpty(property.LayerName))
			{
				if (this.StyleAnimation == null)
				{
					this.StyleAnimation = new BrushLayerAnimation();
				}
				brushLayerAnimation = this.StyleAnimation;
			}
			else
			{
				if (!this._data.ContainsKey(property.LayerName))
				{
					return;
				}
				brushLayerAnimation = this._data[property.LayerName];
			}
			brushLayerAnimation.RemoveAnimationProperty(property);
			if (brushLayerAnimation.Collections.Count == 0)
			{
				this._data.Remove(property.LayerName);
			}
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00003A88 File Offset: 0x00001C88
		public void FillFrom(BrushAnimation animation)
		{
			this.Name = animation.Name;
			this.Duration = animation.Duration;
			this.Loop = animation.Loop;
			this.InterpolationType = animation.InterpolationType;
			this.InterpolationFunction = animation.InterpolationFunction;
			if (animation.StyleAnimation != null)
			{
				this.StyleAnimation = animation.StyleAnimation.Clone();
			}
			this._data = new Dictionary<string, BrushLayerAnimation>();
			foreach (KeyValuePair<string, BrushLayerAnimation> keyValuePair in animation._data)
			{
				string key = keyValuePair.Key;
				BrushLayerAnimation brushLayerAnimation = keyValuePair.Value.Clone();
				this._data.Add(key, brushLayerAnimation);
			}
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00003B58 File Offset: 0x00001D58
		public BrushLayerAnimation GetLayerAnimation(string name)
		{
			if (this._data.ContainsKey(name))
			{
				return this._data[name];
			}
			return null;
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00003B76 File Offset: 0x00001D76
		public IEnumerable<BrushLayerAnimation> GetLayerAnimations()
		{
			return this._data.Values;
		}

		// Token: 0x04000035 RID: 53
		private Dictionary<string, BrushLayerAnimation> _data;
	}
}
