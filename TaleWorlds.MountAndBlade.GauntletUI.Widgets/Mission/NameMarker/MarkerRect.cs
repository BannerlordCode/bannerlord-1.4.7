using System;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission.NameMarker
{
	// Token: 0x020000F6 RID: 246
	public class MarkerRect
	{
		// Token: 0x1700048E RID: 1166
		// (get) Token: 0x06000CD3 RID: 3283 RVA: 0x0002305F File Offset: 0x0002125F
		// (set) Token: 0x06000CD4 RID: 3284 RVA: 0x00023067 File Offset: 0x00021267
		public float Left { get; private set; }

		// Token: 0x1700048F RID: 1167
		// (get) Token: 0x06000CD5 RID: 3285 RVA: 0x00023070 File Offset: 0x00021270
		// (set) Token: 0x06000CD6 RID: 3286 RVA: 0x00023078 File Offset: 0x00021278
		public float Right { get; private set; }

		// Token: 0x17000490 RID: 1168
		// (get) Token: 0x06000CD7 RID: 3287 RVA: 0x00023081 File Offset: 0x00021281
		// (set) Token: 0x06000CD8 RID: 3288 RVA: 0x00023089 File Offset: 0x00021289
		public float Top { get; private set; }

		// Token: 0x17000491 RID: 1169
		// (get) Token: 0x06000CD9 RID: 3289 RVA: 0x00023092 File Offset: 0x00021292
		// (set) Token: 0x06000CDA RID: 3290 RVA: 0x0002309A File Offset: 0x0002129A
		public float Bottom { get; private set; }

		// Token: 0x17000492 RID: 1170
		// (get) Token: 0x06000CDB RID: 3291 RVA: 0x000230A3 File Offset: 0x000212A3
		public float CenterX
		{
			get
			{
				return this.Left + (this.Right - this.Left) / 2f;
			}
		}

		// Token: 0x17000493 RID: 1171
		// (get) Token: 0x06000CDC RID: 3292 RVA: 0x000230BF File Offset: 0x000212BF
		public float CenterY
		{
			get
			{
				return this.Top + (this.Bottom - this.Top) / 2f;
			}
		}

		// Token: 0x17000494 RID: 1172
		// (get) Token: 0x06000CDD RID: 3293 RVA: 0x000230DB File Offset: 0x000212DB
		public float Width
		{
			get
			{
				return this.Right - this.Left;
			}
		}

		// Token: 0x17000495 RID: 1173
		// (get) Token: 0x06000CDE RID: 3294 RVA: 0x000230EA File Offset: 0x000212EA
		public float Height
		{
			get
			{
				return this.Bottom - this.Top;
			}
		}

		// Token: 0x06000CDF RID: 3295 RVA: 0x000230F9 File Offset: 0x000212F9
		public MarkerRect()
		{
			this.Reset();
		}

		// Token: 0x06000CE0 RID: 3296 RVA: 0x00023107 File Offset: 0x00021307
		public void Reset()
		{
			this.Left = 0f;
			this.Right = 0f;
			this.Top = 0f;
			this.Bottom = 0f;
		}

		// Token: 0x06000CE1 RID: 3297 RVA: 0x00023135 File Offset: 0x00021335
		public void UpdatePoints(float left, float right, float top, float bottom)
		{
			this.Left = left;
			this.Right = right;
			this.Top = top;
			this.Bottom = bottom;
		}

		// Token: 0x06000CE2 RID: 3298 RVA: 0x00023154 File Offset: 0x00021354
		public bool IsOverlapping(MarkerRect other)
		{
			return other.Left <= this.Right && other.Right >= this.Left && other.Top <= this.Bottom && other.Bottom >= this.Top;
		}
	}
}
