using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Objects
{
	// Token: 0x020003A3 RID: 931
	public class ShipVisual : ScriptComponentBehavior
	{
		// Token: 0x170009D4 RID: 2516
		// (get) Token: 0x060034F0 RID: 13552 RVA: 0x000D9CDE File Offset: 0x000D7EDE
		// (set) Token: 0x060034F1 RID: 13553 RVA: 0x000D9CE6 File Offset: 0x000D7EE6
		public int Seed { get; private set; }

		// Token: 0x170009D5 RID: 2517
		// (get) Token: 0x060034F2 RID: 13554 RVA: 0x000D9CEF File Offset: 0x000D7EEF
		// (set) Token: 0x060034F3 RID: 13555 RVA: 0x000D9CF7 File Offset: 0x000D7EF7
		public string CustomSailPatternId { get; private set; }

		// Token: 0x170009D6 RID: 2518
		// (get) Token: 0x060034F4 RID: 13556 RVA: 0x000D9D00 File Offset: 0x000D7F00
		// (set) Token: 0x060034F5 RID: 13557 RVA: 0x000D9D08 File Offset: 0x000D7F08
		public List<ScriptComponentBehavior> SailVisuals { get; private set; } = new List<ScriptComponentBehavior>();

		// Token: 0x170009D7 RID: 2519
		// (get) Token: 0x060034F6 RID: 13558 RVA: 0x000D9D11 File Offset: 0x000D7F11
		// (set) Token: 0x060034F7 RID: 13559 RVA: 0x000D9D19 File Offset: 0x000D7F19
		public float Health
		{
			get
			{
				return this._health;
			}
			set
			{
				this._health = MathF.Clamp(value, 0f, 1f);
			}
		}

		// Token: 0x060034F8 RID: 13560 RVA: 0x000D9D31 File Offset: 0x000D7F31
		public void Initialize(int seed, string customSailPatternId = "")
		{
			this.Seed = seed;
			this.CustomSailPatternId = customSailPatternId;
		}

		// Token: 0x0400167E RID: 5758
		private float _health = 1f;

		// Token: 0x0400167F RID: 5759
		[TupleElementNames(new string[] { "sailColor1", "sailColor2" })]
		public ValueTuple<uint, uint> SailColors = new ValueTuple<uint, uint>(Colors.White.ToUnsignedInteger(), Colors.White.ToUnsignedInteger());
	}
}
