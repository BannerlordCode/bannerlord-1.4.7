using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission
{
	// Token: 0x020000DD RID: 221
	public class DisguiseMarkerBrushWidget : BrushWidget
	{
		// Token: 0x170003F5 RID: 1013
		// (get) Token: 0x06000B58 RID: 2904 RVA: 0x0001FA01 File Offset: 0x0001DC01
		// (set) Token: 0x06000B59 RID: 2905 RVA: 0x0001FA09 File Offset: 0x0001DC09
		public Vec2 Position { get; set; }

		// Token: 0x06000B5A RID: 2906 RVA: 0x0001FA12 File Offset: 0x0001DC12
		public DisguiseMarkerBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000B5B RID: 2907 RVA: 0x0001FA1C File Offset: 0x0001DC1C
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			base.ScaledPositionYOffset = this.Position.y - base.Size.Y / 2f;
			base.ScaledPositionXOffset = this.Position.x - base.Size.X / 2f;
		}

		// Token: 0x06000B5C RID: 2908 RVA: 0x0001FA76 File Offset: 0x0001DC76
		private void UpdateState()
		{
			this.SetState(this.OffenseTypeIdentifier);
		}

		// Token: 0x170003F6 RID: 1014
		// (get) Token: 0x06000B5D RID: 2909 RVA: 0x0001FA84 File Offset: 0x0001DC84
		// (set) Token: 0x06000B5E RID: 2910 RVA: 0x0001FA8C File Offset: 0x0001DC8C
		public string OffenseTypeIdentifier
		{
			get
			{
				return this._offenseTypeIdentifier;
			}
			set
			{
				if (value != this._offenseTypeIdentifier)
				{
					this._offenseTypeIdentifier = value;
					base.OnPropertyChanged<string>(value, "OffenseTypeIdentifier");
					this.UpdateState();
				}
			}
		}

		// Token: 0x04000521 RID: 1313
		private string _offenseTypeIdentifier;
	}
}
