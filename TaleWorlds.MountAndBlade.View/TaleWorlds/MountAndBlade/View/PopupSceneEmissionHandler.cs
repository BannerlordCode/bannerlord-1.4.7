using System;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.View
{
	// Token: 0x0200001F RID: 31
	public class PopupSceneEmissionHandler : ScriptComponentBehavior
	{
		// Token: 0x060000D3 RID: 211 RVA: 0x000069F3 File Offset: 0x00004BF3
		protected override void OnInit()
		{
			base.OnInit();
			base.SetScriptComponentToTick(this.GetTickRequirement());
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00006A07 File Offset: 0x00004C07
		protected override void OnEditorInit()
		{
			base.OnEditorInit();
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00006A0F File Offset: 0x00004C0F
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			return ScriptComponentBehavior.TickRequirement.Tick;
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00006A14 File Offset: 0x00004C14
		protected override void OnTick(float dt)
		{
			this.timeElapsed += dt;
			foreach (WeakGameEntity weakGameEntity in base.GameEntity.GetChildren())
			{
				Mesh firstMesh = weakGameEntity.GetFirstMesh();
				if (firstMesh != null)
				{
					firstMesh.SetVectorArgument(1f, 0.5f, 1f, MBMath.SmoothStep(this.startTime, this.startTime + this.transitionTime, this.timeElapsed) * 10f);
				}
			}
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00006ABC File Offset: 0x00004CBC
		protected override void OnEditorTick(float dt)
		{
			base.OnEditorTick(dt);
			this.OnTick(dt);
		}

		// Token: 0x04000024 RID: 36
		public float startTime;

		// Token: 0x04000025 RID: 37
		public float transitionTime;

		// Token: 0x04000026 RID: 38
		private float timeElapsed;
	}
}
