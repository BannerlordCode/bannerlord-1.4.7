using System;
using TaleWorlds.DotNet;

namespace TaleWorlds.Engine
{
	// Token: 0x02000078 RID: 120
	[EngineStruct("int", false, null)]
	public readonly struct PhysicsMaterial
	{
		// Token: 0x06000A99 RID: 2713 RVA: 0x0000AE8B File Offset: 0x0000908B
		internal PhysicsMaterial(int index)
		{
			this = default(PhysicsMaterial);
			this.Index = index;
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000A9A RID: 2714 RVA: 0x0000AE9B File Offset: 0x0000909B
		public bool IsValid
		{
			get
			{
				return this.Index >= 0;
			}
		}

		// Token: 0x06000A9B RID: 2715 RVA: 0x0000AEA9 File Offset: 0x000090A9
		public PhysicsMaterialFlags GetFlags()
		{
			return PhysicsMaterial.GetFlagsAtIndex(this.Index);
		}

		// Token: 0x06000A9C RID: 2716 RVA: 0x0000AEB6 File Offset: 0x000090B6
		public float GetDynamicFriction()
		{
			return PhysicsMaterial.GetDynamicFrictionAtIndex(this.Index);
		}

		// Token: 0x06000A9D RID: 2717 RVA: 0x0000AEC3 File Offset: 0x000090C3
		public float GetStaticFriction()
		{
			return PhysicsMaterial.GetStaticFrictionAtIndex(this.Index);
		}

		// Token: 0x06000A9E RID: 2718 RVA: 0x0000AED0 File Offset: 0x000090D0
		public float GetRestitution()
		{
			return PhysicsMaterial.GetRestitutionAtIndex(this.Index);
		}

		// Token: 0x06000A9F RID: 2719 RVA: 0x0000AEDD File Offset: 0x000090DD
		public float GetLinearDamping()
		{
			return PhysicsMaterial.GetLinearDampingAtIndex(this.Index);
		}

		// Token: 0x06000AA0 RID: 2720 RVA: 0x0000AEEA File Offset: 0x000090EA
		public float GetAngularDamping()
		{
			return PhysicsMaterial.GetAngularDampingAtIndex(this.Index);
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000AA1 RID: 2721 RVA: 0x0000AEF7 File Offset: 0x000090F7
		public string Name
		{
			get
			{
				return PhysicsMaterial.GetNameAtIndex(this.Index);
			}
		}

		// Token: 0x06000AA2 RID: 2722 RVA: 0x0000AF04 File Offset: 0x00009104
		public bool Equals(PhysicsMaterial m)
		{
			return this.Index == m.Index;
		}

		// Token: 0x06000AA3 RID: 2723 RVA: 0x0000AF14 File Offset: 0x00009114
		public static int GetMaterialCount()
		{
			return EngineApplicationInterface.IPhysicsMaterial.GetMaterialCount();
		}

		// Token: 0x06000AA4 RID: 2724 RVA: 0x0000AF20 File Offset: 0x00009120
		public static PhysicsMaterial GetFromName(string id)
		{
			return EngineApplicationInterface.IPhysicsMaterial.GetIndexWithName(id);
		}

		// Token: 0x06000AA5 RID: 2725 RVA: 0x0000AF2D File Offset: 0x0000912D
		public static string GetNameAtIndex(int index)
		{
			return EngineApplicationInterface.IPhysicsMaterial.GetMaterialNameAtIndex(index);
		}

		// Token: 0x06000AA6 RID: 2726 RVA: 0x0000AF3A File Offset: 0x0000913A
		public static PhysicsMaterialFlags GetFlagsAtIndex(int index)
		{
			return EngineApplicationInterface.IPhysicsMaterial.GetFlagsAtIndex(index);
		}

		// Token: 0x06000AA7 RID: 2727 RVA: 0x0000AF47 File Offset: 0x00009147
		public static float GetRestitutionAtIndex(int index)
		{
			return EngineApplicationInterface.IPhysicsMaterial.GetRestitutionAtIndex(index);
		}

		// Token: 0x06000AA8 RID: 2728 RVA: 0x0000AF54 File Offset: 0x00009154
		public static float GetDynamicFrictionAtIndex(int index)
		{
			return EngineApplicationInterface.IPhysicsMaterial.GetDynamicFrictionAtIndex(index);
		}

		// Token: 0x06000AA9 RID: 2729 RVA: 0x0000AF61 File Offset: 0x00009161
		public static float GetStaticFrictionAtIndex(int index)
		{
			return EngineApplicationInterface.IPhysicsMaterial.GetStaticFrictionAtIndex(index);
		}

		// Token: 0x06000AAA RID: 2730 RVA: 0x0000AF6E File Offset: 0x0000916E
		public static float GetLinearDampingAtIndex(int index)
		{
			return EngineApplicationInterface.IPhysicsMaterial.GetLinearDampingAtIndex(index);
		}

		// Token: 0x06000AAB RID: 2731 RVA: 0x0000AF7B File Offset: 0x0000917B
		public static float GetAngularDampingAtIndex(int index)
		{
			return EngineApplicationInterface.IPhysicsMaterial.GetAngularDampingAtIndex(index);
		}

		// Token: 0x06000AAC RID: 2732 RVA: 0x0000AF88 File Offset: 0x00009188
		public static PhysicsMaterial GetFromIndex(int index)
		{
			return new PhysicsMaterial(index);
		}

		// Token: 0x0400016A RID: 362
		[CustomEngineStructMemberData("ignoredMember", true)]
		public readonly int Index;

		// Token: 0x0400016B RID: 363
		public static readonly PhysicsMaterial InvalidPhysicsMaterial = new PhysicsMaterial(-1);
	}
}
