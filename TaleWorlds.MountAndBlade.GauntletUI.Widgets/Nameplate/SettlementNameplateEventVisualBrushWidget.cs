using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Nameplate
{
	// Token: 0x02000080 RID: 128
	public class SettlementNameplateEventVisualBrushWidget : BrushWidget
	{
		// Token: 0x0600071B RID: 1819 RVA: 0x00014C19 File Offset: 0x00012E19
		public SettlementNameplateEventVisualBrushWidget(UIContext context)
			: base(context)
		{
			base.EventManager.AddLateUpdateAction(this, new Action<float>(this.LateUpdateAction), 1);
		}

		// Token: 0x0600071C RID: 1820 RVA: 0x00014C42 File Offset: 0x00012E42
		private void LateUpdateAction(float dt)
		{
			if (!this._determinedVisual)
			{
				this.RegisterBrushStatesOfWidget();
				this.UpdateVisual(this.Type);
				this._determinedVisual = true;
			}
		}

		// Token: 0x0600071D RID: 1821 RVA: 0x00014C68 File Offset: 0x00012E68
		private void UpdateVisual(int type)
		{
			switch (type)
			{
			case 0:
				this.SetState("Tournament");
				break;
			case 1:
				this.SetState("AvailableIssue");
				break;
			case 2:
				this.SetState("ActiveQuest");
				break;
			case 3:
				this.SetState("ActiveStoryQuest");
				break;
			case 4:
				this.SetState("TrackedIssue");
				break;
			case 5:
				this.SetState("TrackedStoryQuest");
				break;
			case 6:
				this.SetState(this.AdditionalParameters);
				base.MarginLeft = 2f;
				base.MarginRight = 2f;
				break;
			}
			Brush brush = base.Brush;
			Sprite sprite;
			if (brush == null)
			{
				sprite = null;
			}
			else
			{
				Style style = brush.GetStyle(base.CurrentState);
				if (style == null)
				{
					sprite = null;
				}
				else
				{
					StyleLayer layer = style.GetLayer(0);
					sprite = ((layer != null) ? layer.Sprite : null);
				}
			}
			Sprite sprite2 = sprite;
			if (sprite2 != null)
			{
				base.SuggestedWidth = base.SuggestedHeight / (float)sprite2.Height * (float)sprite2.Width;
			}
		}

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x0600071E RID: 1822 RVA: 0x00014D5A File Offset: 0x00012F5A
		// (set) Token: 0x0600071F RID: 1823 RVA: 0x00014D62 File Offset: 0x00012F62
		[Editor(false)]
		public int Type
		{
			get
			{
				return this._type;
			}
			set
			{
				if (this._type != value)
				{
					this._type = value;
					base.OnPropertyChanged(value, "Type");
				}
			}
		}

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x06000720 RID: 1824 RVA: 0x00014D80 File Offset: 0x00012F80
		// (set) Token: 0x06000721 RID: 1825 RVA: 0x00014D88 File Offset: 0x00012F88
		[Editor(false)]
		public string AdditionalParameters
		{
			get
			{
				return this._additionalParameters;
			}
			set
			{
				if (this._additionalParameters != value)
				{
					this._additionalParameters = value;
					base.OnPropertyChanged<string>(value, "AdditionalParameters");
				}
			}
		}

		// Token: 0x04000316 RID: 790
		private bool _determinedVisual;

		// Token: 0x04000317 RID: 791
		private int _type = -1;

		// Token: 0x04000318 RID: 792
		private string _additionalParameters;
	}
}
