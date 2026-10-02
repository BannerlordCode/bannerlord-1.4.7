using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Quest
{
	// Token: 0x0200005E RID: 94
	public class QuestMarkerBrushWidget : BrushWidget
	{
		// Token: 0x06000522 RID: 1314 RVA: 0x0000FB0B File Offset: 0x0000DD0B
		public QuestMarkerBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x0000FB14 File Offset: 0x0000DD14
		private void UpdateMarkerState(int type)
		{
			string text;
			switch (type)
			{
			case 0:
				text = "None";
				goto IL_006D;
			case 1:
				text = "AvailableIssue";
				goto IL_006D;
			case 2:
				text = "ActiveIssue";
				goto IL_006D;
			case 3:
			case 5:
			case 6:
			case 7:
				break;
			case 4:
				text = "ActiveStoryQuest";
				goto IL_006D;
			case 8:
				text = "TrackedIssue";
				goto IL_006D;
			default:
				if (type == 16)
				{
					text = "TrackedStoryQuest";
					goto IL_006D;
				}
				break;
			}
			text = "None";
			IL_006D:
			if (text != null)
			{
				this.SetState(text);
				Sprite sprite = base.Brush.GetLayer(text).Sprite;
				if (sprite != null)
				{
					float num = base.SuggestedHeight / (float)sprite.Height;
					base.SuggestedWidth = (float)sprite.Width * num;
				}
			}
		}

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x06000524 RID: 1316 RVA: 0x0000FBCB File Offset: 0x0000DDCB
		// (set) Token: 0x06000525 RID: 1317 RVA: 0x0000FBD3 File Offset: 0x0000DDD3
		public int QuestMarkerType
		{
			get
			{
				return this._questMarkerType;
			}
			set
			{
				if (value != this._questMarkerType)
				{
					this._questMarkerType = value;
					this.UpdateMarkerState(this._questMarkerType);
				}
			}
		}

		// Token: 0x04000235 RID: 565
		private int _questMarkerType;
	}
}
