using System;

namespace TaleWorlds.Engine
{
	// Token: 0x02000089 RID: 137
	public sealed class Shader : Resource
	{
		// Token: 0x06000C5B RID: 3163 RVA: 0x0000DB18 File Offset: 0x0000BD18
		internal Shader(UIntPtr ptr)
			: base(ptr)
		{
		}

		// Token: 0x06000C5C RID: 3164 RVA: 0x0000DB21 File Offset: 0x0000BD21
		public static Shader GetFromResource(string shaderName)
		{
			return EngineApplicationInterface.IShader.GetFromResource(shaderName);
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000C5D RID: 3165 RVA: 0x0000DB2E File Offset: 0x0000BD2E
		public string Name
		{
			get
			{
				return EngineApplicationInterface.IShader.GetName(base.Pointer);
			}
		}

		// Token: 0x06000C5E RID: 3166 RVA: 0x0000DB40 File Offset: 0x0000BD40
		public ulong GetMaterialShaderFlagMask(string flagName, bool showErrors = true)
		{
			return EngineApplicationInterface.IShader.GetMaterialShaderFlagMask(base.Pointer, flagName, showErrors);
		}
	}
}
