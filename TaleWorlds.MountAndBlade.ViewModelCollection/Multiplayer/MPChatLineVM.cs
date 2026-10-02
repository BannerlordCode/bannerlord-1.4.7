using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Multiplayer
{
	// Token: 0x0200003A RID: 58
	public class MPChatLineVM : ViewModel
	{
		// Token: 0x060004E4 RID: 1252 RVA: 0x00013940 File Offset: 0x00011B40
		public MPChatLineVM(string chatLine, Color color, string category)
		{
			this.ChatLine = chatLine;
			this.Color = color;
			this.Alpha = 1f;
			this.Category = category;
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x00013968 File Offset: 0x00011B68
		public void HandleFading(float dt)
		{
			this._timeSinceCreation += dt;
			this.RefreshAlpha();
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x0001397E File Offset: 0x00011B7E
		private void RefreshAlpha()
		{
			if (this._forcedVisible)
			{
				this.Alpha = 1f;
				return;
			}
			this.Alpha = this.GetActualAlpha();
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x000139A0 File Offset: 0x00011BA0
		public void ForceInvisible()
		{
			this._timeSinceCreation = 10.5f;
			this.Alpha = 0f;
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x000139B8 File Offset: 0x00011BB8
		private float GetActualAlpha()
		{
			if (this._timeSinceCreation >= 10f)
			{
				return MBMath.ClampFloat(1f - (this._timeSinceCreation - 10f) / 0.5f, 0f, 1f);
			}
			return 1f;
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x000139F4 File Offset: 0x00011BF4
		public void ToggleForceVisible(bool visible)
		{
			this._forcedVisible = visible;
			this.RefreshAlpha();
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x060004EA RID: 1258 RVA: 0x00013A03 File Offset: 0x00011C03
		// (set) Token: 0x060004EB RID: 1259 RVA: 0x00013A0B File Offset: 0x00011C0B
		[DataSourceProperty]
		public string ChatLine
		{
			get
			{
				return this._chatLine;
			}
			set
			{
				if (this._chatLine != value)
				{
					this._chatLine = value;
					base.OnPropertyChangedWithValue<string>(value, "ChatLine");
				}
			}
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x060004EC RID: 1260 RVA: 0x00013A2E File Offset: 0x00011C2E
		// (set) Token: 0x060004ED RID: 1261 RVA: 0x00013A36 File Offset: 0x00011C36
		[DataSourceProperty]
		public Color Color
		{
			get
			{
				return this._color;
			}
			set
			{
				if (this._color != value)
				{
					this._color = value;
					base.OnPropertyChangedWithValue(value, "Color");
				}
			}
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x060004EE RID: 1262 RVA: 0x00013A59 File Offset: 0x00011C59
		// (set) Token: 0x060004EF RID: 1263 RVA: 0x00013A61 File Offset: 0x00011C61
		[DataSourceProperty]
		public float Alpha
		{
			get
			{
				return this._alpha;
			}
			set
			{
				if (this._alpha != value)
				{
					this._alpha = value;
					base.OnPropertyChangedWithValue(value, "Alpha");
				}
			}
		}

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x060004F0 RID: 1264 RVA: 0x00013A7F File Offset: 0x00011C7F
		// (set) Token: 0x060004F1 RID: 1265 RVA: 0x00013A87 File Offset: 0x00011C87
		[DataSourceProperty]
		public string Category
		{
			get
			{
				return this._category;
			}
			set
			{
				if (this._category != value)
				{
					this._category = value;
					base.OnPropertyChangedWithValue<string>(value, "Category");
				}
			}
		}

		// Token: 0x0400023A RID: 570
		private bool _forcedVisible;

		// Token: 0x0400023B RID: 571
		private string _category;

		// Token: 0x0400023C RID: 572
		private const float ChatVisibilityDuration = 10f;

		// Token: 0x0400023D RID: 573
		private const float ChatFadeOutDuration = 0.5f;

		// Token: 0x0400023E RID: 574
		private float _timeSinceCreation;

		// Token: 0x0400023F RID: 575
		private string _chatLine;

		// Token: 0x04000240 RID: 576
		private Color _color;

		// Token: 0x04000241 RID: 577
		private float _alpha;
	}
}
