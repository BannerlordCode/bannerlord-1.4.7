using System;
using System.Collections.Generic;
using System.Linq;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002EA RID: 746
	public class CustomGameUsableMap
	{
		// Token: 0x17000801 RID: 2049
		// (get) Token: 0x06002ADB RID: 10971 RVA: 0x000A4F3A File Offset: 0x000A313A
		// (set) Token: 0x06002ADC RID: 10972 RVA: 0x000A4F42 File Offset: 0x000A3142
		public string Map { get; private set; }

		// Token: 0x17000802 RID: 2050
		// (get) Token: 0x06002ADD RID: 10973 RVA: 0x000A4F4B File Offset: 0x000A314B
		// (set) Token: 0x06002ADE RID: 10974 RVA: 0x000A4F53 File Offset: 0x000A3153
		public bool IsCompatibleWithAllGameTypes { get; private set; }

		// Token: 0x17000803 RID: 2051
		// (get) Token: 0x06002ADF RID: 10975 RVA: 0x000A4F5C File Offset: 0x000A315C
		// (set) Token: 0x06002AE0 RID: 10976 RVA: 0x000A4F64 File Offset: 0x000A3164
		public List<string> CompatibleGameTypes { get; private set; }

		// Token: 0x06002AE1 RID: 10977 RVA: 0x000A4F6D File Offset: 0x000A316D
		public CustomGameUsableMap(string map, bool isCompatibleWithAllGameTypes, List<string> compatibleGameTypes)
		{
			this.Map = map;
			this.IsCompatibleWithAllGameTypes = isCompatibleWithAllGameTypes;
			this.CompatibleGameTypes = compatibleGameTypes;
		}

		// Token: 0x06002AE2 RID: 10978 RVA: 0x000A4F8C File Offset: 0x000A318C
		public override bool Equals(object obj)
		{
			CustomGameUsableMap customGameUsableMap;
			if ((customGameUsableMap = obj as CustomGameUsableMap) != null)
			{
				return !(customGameUsableMap.Map != this.Map) && customGameUsableMap.IsCompatibleWithAllGameTypes == this.IsCompatibleWithAllGameTypes && (((this.CompatibleGameTypes == null || this.CompatibleGameTypes.Count == 0) && (customGameUsableMap.CompatibleGameTypes == null || customGameUsableMap.CompatibleGameTypes.Count == 0)) || (this.CompatibleGameTypes != null && this.CompatibleGameTypes.Count != 0 && customGameUsableMap.CompatibleGameTypes != null && customGameUsableMap.CompatibleGameTypes.Count != 0 && this.CompatibleGameTypes.SequenceEqual<string>(customGameUsableMap.CompatibleGameTypes)));
			}
			return base.Equals(obj);
		}

		// Token: 0x06002AE3 RID: 10979 RVA: 0x000A503C File Offset: 0x000A323C
		public override int GetHashCode()
		{
			return (((((this.Map != null) ? this.Map.GetHashCode() : 0) * 397) ^ this.IsCompatibleWithAllGameTypes.GetHashCode()) * 397) ^ ((this.CompatibleGameTypes != null) ? this.CompatibleGameTypes.GetHashCode() : 0);
		}
	}
}
