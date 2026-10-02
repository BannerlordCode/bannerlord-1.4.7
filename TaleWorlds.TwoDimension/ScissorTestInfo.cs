using System;

namespace TaleWorlds.TwoDimension
{
	// Token: 0x0200002C RID: 44
	public struct ScissorTestInfo
	{
		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x060001F2 RID: 498 RVA: 0x00008184 File Offset: 0x00006384
		public float X
		{
			get
			{
				return this._x;
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x060001F3 RID: 499 RVA: 0x0000818C File Offset: 0x0000638C
		public float X2
		{
			get
			{
				return this._x2;
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060001F4 RID: 500 RVA: 0x00008194 File Offset: 0x00006394
		public float Y
		{
			get
			{
				return this._y;
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x060001F5 RID: 501 RVA: 0x0000819C File Offset: 0x0000639C
		public float Y2
		{
			get
			{
				return this._y2;
			}
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x000081A4 File Offset: 0x000063A4
		public ScissorTestInfo(float x, float y, float x2, float y2)
		{
			this._x = x;
			this._y = y;
			this._x2 = x2;
			this._y2 = y2;
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x000081C4 File Offset: 0x000063C4
		public void ReduceToIntersection(ScissorTestInfo other)
		{
			this._x = Mathf.Max(this._x, other._x);
			this._y = Mathf.Max(this._y, other._y);
			this._x2 = Mathf.Min(this._x2, other._x2);
			this._y2 = Mathf.Min(this._y2, other._y2);
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x0000822D File Offset: 0x0000642D
		public SimpleRectangle GetSimpleRectangle()
		{
			return new SimpleRectangle(this._x, this._y, this._x2 - this._x, this._y2 - this._y);
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x0000825C File Offset: 0x0000645C
		public bool IsCollide(in Rectangle2D other)
		{
			Rectangle2D rectangle2D = other;
			SimpleRectangle boundingBox = rectangle2D.GetBoundingBox();
			return this.GetSimpleRectangle().IsCollide(boundingBox);
		}

		// Token: 0x040000FD RID: 253
		private float _x;

		// Token: 0x040000FE RID: 254
		private float _x2;

		// Token: 0x040000FF RID: 255
		private float _y;

		// Token: 0x04000100 RID: 256
		private float _y2;
	}
}
