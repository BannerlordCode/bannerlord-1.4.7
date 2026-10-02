using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x02000035 RID: 53
	public class ParallaxContainerWidget : Widget
	{
		// Token: 0x06000331 RID: 817 RVA: 0x0000A3B0 File Offset: 0x000085B0
		public ParallaxContainerWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000332 RID: 818 RVA: 0x0000A3C4 File Offset: 0x000085C4
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			using (List<ParallaxItemBrushWidget>.Enumerator enumerator = this._parallaxItems.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					switch (enumerator.Current.InitialDirection)
					{
					}
				}
			}
		}

		// Token: 0x06000333 RID: 819 RVA: 0x0000A438 File Offset: 0x00008638
		protected override void OnChildAdded(Widget child)
		{
			base.OnChildAdded(child);
			ParallaxItemBrushWidget parallaxItemBrushWidget;
			if ((parallaxItemBrushWidget = child as ParallaxItemBrushWidget) != null)
			{
				this._parallaxItems.Add(parallaxItemBrushWidget);
			}
		}

		// Token: 0x06000334 RID: 820 RVA: 0x0000A464 File Offset: 0x00008664
		protected override void OnBeforeChildRemoved(Widget child)
		{
			base.OnBeforeChildRemoved(child);
			ParallaxItemBrushWidget parallaxItemBrushWidget;
			if ((parallaxItemBrushWidget = child as ParallaxItemBrushWidget) != null)
			{
				this._parallaxItems.Remove(parallaxItemBrushWidget);
			}
		}

		// Token: 0x0400014D RID: 333
		private List<ParallaxItemBrushWidget> _parallaxItems = new List<ParallaxItemBrushWidget>();
	}
}
