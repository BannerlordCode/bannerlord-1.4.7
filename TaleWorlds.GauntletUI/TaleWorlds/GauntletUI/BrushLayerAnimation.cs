using System;
using TaleWorlds.Library;

namespace TaleWorlds.GauntletUI
{
	// Token: 0x02000013 RID: 19
	public class BrushLayerAnimation
	{
		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000146 RID: 326 RVA: 0x00007237 File Offset: 0x00005437
		// (set) Token: 0x06000147 RID: 327 RVA: 0x0000723F File Offset: 0x0000543F
		public string LayerName { get; set; }

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000148 RID: 328 RVA: 0x00007248 File Offset: 0x00005448
		public MBReadOnlyList<BrushAnimationProperty> Collections
		{
			get
			{
				return this._collections;
			}
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00007250 File Offset: 0x00005450
		public BrushLayerAnimation()
		{
			this.LayerName = null;
			this._collections = new MBList<BrushAnimationProperty>();
		}

		// Token: 0x0600014A RID: 330 RVA: 0x0000726A File Offset: 0x0000546A
		internal void RemoveAnimationProperty(BrushAnimationProperty property)
		{
			this._collections.Remove(property);
		}

		// Token: 0x0600014B RID: 331 RVA: 0x00007279 File Offset: 0x00005479
		public void AddAnimationProperty(BrushAnimationProperty property)
		{
			this._collections.Add(property);
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00007288 File Offset: 0x00005488
		private void FillFrom(BrushLayerAnimation brushLayerAnimation)
		{
			this.LayerName = brushLayerAnimation.LayerName;
			this._collections = new MBList<BrushAnimationProperty>();
			foreach (BrushAnimationProperty brushAnimationProperty in brushLayerAnimation._collections)
			{
				BrushAnimationProperty brushAnimationProperty2 = brushAnimationProperty.Clone();
				this._collections.Add(brushAnimationProperty2);
			}
		}

		// Token: 0x0600014D RID: 333 RVA: 0x000072FC File Offset: 0x000054FC
		public BrushLayerAnimation Clone()
		{
			BrushLayerAnimation brushLayerAnimation = new BrushLayerAnimation();
			brushLayerAnimation.FillFrom(this);
			return brushLayerAnimation;
		}

		// Token: 0x04000072 RID: 114
		private MBList<BrushAnimationProperty> _collections;
	}
}
