using System;

namespace SandBox.GauntletUI.Tutorial
{
	// Token: 0x02000016 RID: 22
	public class TutorialAttribute : Attribute
	{
		// Token: 0x0600013A RID: 314 RVA: 0x0000A33D File Offset: 0x0000853D
		public TutorialAttribute(string tutorialIdentifier)
		{
			this.TutorialIdentifier = tutorialIdentifier;
		}

		// Token: 0x0400006B RID: 107
		public readonly string TutorialIdentifier;
	}
}
