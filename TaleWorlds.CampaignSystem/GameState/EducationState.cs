using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameState
{
	// Token: 0x02000390 RID: 912
	public class EducationState : GameState
	{
		// Token: 0x17000C8C RID: 3212
		// (get) Token: 0x060034F7 RID: 13559 RVA: 0x000D96A1 File Offset: 0x000D78A1
		public override bool IsMenuState
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000C8D RID: 3213
		// (get) Token: 0x060034F8 RID: 13560 RVA: 0x000D96A4 File Offset: 0x000D78A4
		// (set) Token: 0x060034F9 RID: 13561 RVA: 0x000D96AC File Offset: 0x000D78AC
		public Hero Child { get; private set; }

		// Token: 0x17000C8E RID: 3214
		// (get) Token: 0x060034FA RID: 13562 RVA: 0x000D96B5 File Offset: 0x000D78B5
		// (set) Token: 0x060034FB RID: 13563 RVA: 0x000D96BD File Offset: 0x000D78BD
		public IEducationStateHandler Handler
		{
			get
			{
				return this._handler;
			}
			set
			{
				this._handler = value;
			}
		}

		// Token: 0x060034FC RID: 13564 RVA: 0x000D96C6 File Offset: 0x000D78C6
		public EducationState()
		{
			Debug.FailedAssert("Do not use EducationState with default constructor!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\GameState\\EducationState.cs", ".ctor", 22);
		}

		// Token: 0x060034FD RID: 13565 RVA: 0x000D96E4 File Offset: 0x000D78E4
		public EducationState(Hero child)
		{
			this.Child = child;
		}

		// Token: 0x04000F26 RID: 3878
		private IEducationStateHandler _handler;
	}
}
