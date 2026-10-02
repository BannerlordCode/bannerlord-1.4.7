using System;
using System.Collections.Generic;

namespace TaleWorlds.Library.CodeGeneration
{
	// Token: 0x020000BB RID: 187
	public class ClassCode
	{
		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x060006F2 RID: 1778 RVA: 0x00017735 File Offset: 0x00015935
		// (set) Token: 0x060006F3 RID: 1779 RVA: 0x0001773D File Offset: 0x0001593D
		public string Name { get; set; }

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x060006F4 RID: 1780 RVA: 0x00017746 File Offset: 0x00015946
		// (set) Token: 0x060006F5 RID: 1781 RVA: 0x0001774E File Offset: 0x0001594E
		public bool IsGeneric { get; set; }

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x060006F6 RID: 1782 RVA: 0x00017757 File Offset: 0x00015957
		// (set) Token: 0x060006F7 RID: 1783 RVA: 0x0001775F File Offset: 0x0001595F
		public int GenericTypeCount { get; set; }

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x060006F8 RID: 1784 RVA: 0x00017768 File Offset: 0x00015968
		// (set) Token: 0x060006F9 RID: 1785 RVA: 0x00017770 File Offset: 0x00015970
		public bool IsPartial { get; set; }

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x060006FA RID: 1786 RVA: 0x00017779 File Offset: 0x00015979
		// (set) Token: 0x060006FB RID: 1787 RVA: 0x00017781 File Offset: 0x00015981
		public ClassCodeAccessModifier AccessModifier { get; set; }

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x060006FC RID: 1788 RVA: 0x0001778A File Offset: 0x0001598A
		// (set) Token: 0x060006FD RID: 1789 RVA: 0x00017792 File Offset: 0x00015992
		public bool IsClass { get; set; }

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x060006FE RID: 1790 RVA: 0x0001779B File Offset: 0x0001599B
		// (set) Token: 0x060006FF RID: 1791 RVA: 0x000177A3 File Offset: 0x000159A3
		public List<string> InheritedInterfaces { get; private set; }

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06000700 RID: 1792 RVA: 0x000177AC File Offset: 0x000159AC
		// (set) Token: 0x06000701 RID: 1793 RVA: 0x000177B4 File Offset: 0x000159B4
		public List<ClassCode> NestedClasses { get; private set; }

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000702 RID: 1794 RVA: 0x000177BD File Offset: 0x000159BD
		// (set) Token: 0x06000703 RID: 1795 RVA: 0x000177C5 File Offset: 0x000159C5
		public List<MethodCode> Methods { get; private set; }

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x06000704 RID: 1796 RVA: 0x000177CE File Offset: 0x000159CE
		// (set) Token: 0x06000705 RID: 1797 RVA: 0x000177D6 File Offset: 0x000159D6
		public List<ConstructorCode> Constructors { get; private set; }

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x06000706 RID: 1798 RVA: 0x000177DF File Offset: 0x000159DF
		// (set) Token: 0x06000707 RID: 1799 RVA: 0x000177E7 File Offset: 0x000159E7
		public List<VariableCode> Variables { get; private set; }

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x06000708 RID: 1800 RVA: 0x000177F0 File Offset: 0x000159F0
		// (set) Token: 0x06000709 RID: 1801 RVA: 0x000177F8 File Offset: 0x000159F8
		public CommentSection CommentSection { get; set; }

		// Token: 0x0600070A RID: 1802 RVA: 0x00017804 File Offset: 0x00015A04
		public ClassCode()
		{
			this.IsClass = true;
			this.IsGeneric = false;
			this.GenericTypeCount = 0;
			this.InheritedInterfaces = new List<string>();
			this.NestedClasses = new List<ClassCode>();
			this.Methods = new List<MethodCode>();
			this.Constructors = new List<ConstructorCode>();
			this.Variables = new List<VariableCode>();
			this.AccessModifier = ClassCodeAccessModifier.DoNotMention;
			this.Name = "UnnamedClass";
			this.CommentSection = null;
		}

		// Token: 0x0600070B RID: 1803 RVA: 0x0001787C File Offset: 0x00015A7C
		public void GenerateInto(CodeGenerationFile codeGenerationFile)
		{
			if (this.CommentSection != null)
			{
				this.CommentSection.GenerateInto(codeGenerationFile);
			}
			string text = "";
			if (this.AccessModifier == ClassCodeAccessModifier.Public)
			{
				text += "public ";
			}
			else if (this.AccessModifier == ClassCodeAccessModifier.Internal)
			{
				text += "internal ";
			}
			if (this.IsPartial)
			{
				text += "partial ";
			}
			string text2 = "class";
			if (!this.IsClass)
			{
				text2 = "struct";
			}
			text = text + text2 + " " + this.Name;
			if (this.InheritedInterfaces.Count > 0)
			{
				text += " : ";
				for (int i = 0; i < this.InheritedInterfaces.Count; i++)
				{
					string text3 = this.InheritedInterfaces[i];
					text = text + " " + text3;
					if (i + 1 != this.InheritedInterfaces.Count)
					{
						text += ", ";
					}
				}
			}
			if (this.IsGeneric)
			{
				text += "<";
				for (int j = 0; j < this.GenericTypeCount; j++)
				{
					if (this.GenericTypeCount == 1)
					{
						text += "T";
					}
					else
					{
						text = text + "T" + j;
					}
					if (j + 1 != this.GenericTypeCount)
					{
						text += ", ";
					}
				}
				text += ">";
			}
			codeGenerationFile.AddLine(text);
			codeGenerationFile.AddLine("{");
			foreach (ClassCode classCode in this.NestedClasses)
			{
				classCode.GenerateInto(codeGenerationFile);
			}
			foreach (VariableCode variableCode in this.Variables)
			{
				string text4 = variableCode.GenerateLine();
				codeGenerationFile.AddLine(text4);
			}
			if (this.Variables.Count > 0)
			{
				codeGenerationFile.AddLine("");
			}
			foreach (ConstructorCode constructorCode in this.Constructors)
			{
				constructorCode.GenerateInto(codeGenerationFile);
				codeGenerationFile.AddLine("");
			}
			foreach (MethodCode methodCode in this.Methods)
			{
				methodCode.GenerateInto(codeGenerationFile);
				codeGenerationFile.AddLine("");
			}
			codeGenerationFile.AddLine("}");
		}

		// Token: 0x0600070C RID: 1804 RVA: 0x00017B44 File Offset: 0x00015D44
		public void AddVariable(VariableCode variableCode)
		{
			this.Variables.Add(variableCode);
		}

		// Token: 0x0600070D RID: 1805 RVA: 0x00017B52 File Offset: 0x00015D52
		public void AddNestedClass(ClassCode clasCode)
		{
			this.NestedClasses.Add(clasCode);
		}

		// Token: 0x0600070E RID: 1806 RVA: 0x00017B60 File Offset: 0x00015D60
		public void AddMethod(MethodCode methodCode)
		{
			this.Methods.Add(methodCode);
		}

		// Token: 0x0600070F RID: 1807 RVA: 0x00017B6E File Offset: 0x00015D6E
		public void AddConsturctor(ConstructorCode constructorCode)
		{
			constructorCode.Name = this.Name;
			this.Constructors.Add(constructorCode);
		}

		// Token: 0x06000710 RID: 1808 RVA: 0x00017B88 File Offset: 0x00015D88
		public void AddInterface(string interfaceName)
		{
			this.InheritedInterfaces.Add(interfaceName);
		}
	}
}
