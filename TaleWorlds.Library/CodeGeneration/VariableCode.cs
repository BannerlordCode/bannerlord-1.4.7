using System;

namespace TaleWorlds.Library.CodeGeneration
{
	// Token: 0x020000C6 RID: 198
	public class VariableCode
	{
		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x06000747 RID: 1863 RVA: 0x000183E2 File Offset: 0x000165E2
		// (set) Token: 0x06000748 RID: 1864 RVA: 0x000183EA File Offset: 0x000165EA
		public string Name { get; set; }

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x06000749 RID: 1865 RVA: 0x000183F3 File Offset: 0x000165F3
		// (set) Token: 0x0600074A RID: 1866 RVA: 0x000183FB File Offset: 0x000165FB
		public string Type { get; set; }

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x0600074B RID: 1867 RVA: 0x00018404 File Offset: 0x00016604
		// (set) Token: 0x0600074C RID: 1868 RVA: 0x0001840C File Offset: 0x0001660C
		public bool IsStatic { get; set; }

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x0600074D RID: 1869 RVA: 0x00018415 File Offset: 0x00016615
		// (set) Token: 0x0600074E RID: 1870 RVA: 0x0001841D File Offset: 0x0001661D
		public VariableCodeAccessModifier AccessModifier { get; set; }

		// Token: 0x0600074F RID: 1871 RVA: 0x00018426 File Offset: 0x00016626
		public VariableCode()
		{
			this.Type = "System.Object";
			this.Name = "Unnamed variable";
			this.IsStatic = false;
			this.AccessModifier = VariableCodeAccessModifier.Private;
		}

		// Token: 0x06000750 RID: 1872 RVA: 0x00018454 File Offset: 0x00016654
		public string GenerateLine()
		{
			string text = "";
			if (this.AccessModifier == VariableCodeAccessModifier.Public)
			{
				text += "public ";
			}
			else if (this.AccessModifier == VariableCodeAccessModifier.Protected)
			{
				text += "protected ";
			}
			else if (this.AccessModifier == VariableCodeAccessModifier.Private)
			{
				text += "private ";
			}
			else if (this.AccessModifier == VariableCodeAccessModifier.Internal)
			{
				text += "internal ";
			}
			if (this.IsStatic)
			{
				text += "static ";
			}
			return string.Concat(new string[] { text, this.Type, " ", this.Name, ";" });
		}
	}
}
