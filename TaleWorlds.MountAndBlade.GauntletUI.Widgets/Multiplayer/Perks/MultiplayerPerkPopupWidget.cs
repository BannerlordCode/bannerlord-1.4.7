using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Perks
{
	// Token: 0x0200009A RID: 154
	public class MultiplayerPerkPopupWidget : Widget
	{
		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x06000841 RID: 2113 RVA: 0x00017BFD File Offset: 0x00015DFD
		// (set) Token: 0x06000842 RID: 2114 RVA: 0x00017C05 File Offset: 0x00015E05
		public bool ShowAboveContainer { get; set; }

		// Token: 0x06000843 RID: 2115 RVA: 0x00017C0E File Offset: 0x00015E0E
		public MultiplayerPerkPopupWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000844 RID: 2116 RVA: 0x00017C17 File Offset: 0x00015E17
		public void SetPopupPerksContainer(MultiplayerPerkContainerPanelWidget container)
		{
			this._latestContainer = container;
			base.ApplyActionToAllChildrenRecursive(new Action<Widget>(this.SetContainersOfChildren));
		}

		// Token: 0x06000845 RID: 2117 RVA: 0x00017C34 File Offset: 0x00015E34
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (base.IsVisible && this._latestContainer != null)
			{
				float num = this._latestContainer.GlobalPosition.X - (base.Size.X / 2f - this._latestContainer.Size.X / 2f);
				base.ScaledPositionXOffset = Mathf.Clamp(num, 0f, base.Context.EventManager.PageSize.X - base.Size.X);
				if (!this.ShowAboveContainer)
				{
					base.ScaledPositionYOffset = this._latestContainer.GlobalPosition.Y + this._latestContainer.Size.Y - base.EventManager.TopUsableAreaStart;
					return;
				}
				base.ScaledPositionYOffset = this._latestContainer.GlobalPosition.Y - base.Size.Y - base.EventManager.TopUsableAreaStart;
			}
		}

		// Token: 0x06000846 RID: 2118 RVA: 0x00017D34 File Offset: 0x00015F34
		private void SetContainersOfChildren(Widget obj)
		{
			MultiplayerPerkItemToggleWidget multiplayerPerkItemToggleWidget;
			if ((multiplayerPerkItemToggleWidget = obj as MultiplayerPerkItemToggleWidget) != null)
			{
				multiplayerPerkItemToggleWidget.ContainerPanel = this._latestContainer;
			}
		}

		// Token: 0x040003AF RID: 943
		private MultiplayerPerkContainerPanelWidget _latestContainer;
	}
}
