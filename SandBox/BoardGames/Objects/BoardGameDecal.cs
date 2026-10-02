using System;
using TaleWorlds.Engine;

namespace SandBox.BoardGames.Objects
{
	// Token: 0x020000FF RID: 255
	public class BoardGameDecal : ScriptComponentBehavior
	{
		// Token: 0x06000CC8 RID: 3272 RVA: 0x0005E276 File Offset: 0x0005C476
		protected override void OnInit()
		{
			base.OnInit();
			this.SetAlpha(0f);
		}

		// Token: 0x06000CC9 RID: 3273 RVA: 0x0005E28C File Offset: 0x0005C48C
		public void SetAlpha(float alpha)
		{
			base.GameEntity.SetAlpha(alpha);
		}

		// Token: 0x06000CCA RID: 3274 RVA: 0x0005E2A8 File Offset: 0x0005C4A8
		protected override bool MovesEntity()
		{
			return false;
		}
	}
}
