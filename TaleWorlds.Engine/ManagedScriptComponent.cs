using System;
using TaleWorlds.DotNet;

namespace TaleWorlds.Engine
{
	// Token: 0x0200005B RID: 91
	[EngineClass("rglManaged_script_component")]
	public sealed class ManagedScriptComponent : ScriptComponent
	{
		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000911 RID: 2321 RVA: 0x000081B5 File Offset: 0x000063B5
		public ScriptComponentBehavior ScriptComponentBehavior
		{
			get
			{
				return EngineApplicationInterface.IScriptComponent.GetScriptComponentBehavior(base.Pointer);
			}
		}

		// Token: 0x06000912 RID: 2322 RVA: 0x000081C7 File Offset: 0x000063C7
		public void SetVariableEditorWidgetStatus(string field, bool enabled)
		{
			EngineApplicationInterface.IScriptComponent.SetVariableEditorWidgetStatus(base.Pointer, field, enabled);
		}

		// Token: 0x06000913 RID: 2323 RVA: 0x000081DB File Offset: 0x000063DB
		public void SetVariableEditorWidgetValue(string field, RglScriptFieldType fieldType, double value)
		{
			EngineApplicationInterface.IScriptComponent.SetVariableEditorWidgetValue(base.Pointer, field, fieldType, value);
		}

		// Token: 0x06000914 RID: 2324 RVA: 0x000081F0 File Offset: 0x000063F0
		private ManagedScriptComponent()
		{
		}

		// Token: 0x06000915 RID: 2325 RVA: 0x000081F8 File Offset: 0x000063F8
		internal ManagedScriptComponent(UIntPtr pointer)
			: base(pointer)
		{
		}
	}
}
