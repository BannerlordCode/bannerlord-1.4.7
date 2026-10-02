using System;
using System.Numerics;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x02000038 RID: 56
	public class RainbowRichTextWidget : RichTextWidget
	{
		// Token: 0x0600034D RID: 845 RVA: 0x0000A952 File Offset: 0x00008B52
		public RainbowRichTextWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600034E RID: 846 RVA: 0x0000A968 File Offset: 0x00008B68
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			base.Brush.FontColor = Color.Lerp(base.ReadOnlyBrush.FontColor, this.targetColor, dt);
			if (base.Brush.FontColor.ToVec3().Distance(this.targetColor.ToVec3()) < 1f)
			{
				Random random = new Random();
				this.targetColor = Color.FromVector3(new Vector3((float)random.Next(255), (float)random.Next(255), (float)random.Next(255)));
			}
		}

		// Token: 0x04000155 RID: 341
		private Color targetColor = Color.White;
	}
}
