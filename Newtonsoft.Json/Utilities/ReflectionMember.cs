using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000064 RID: 100
	[NullableContext(2)]
	[Nullable(0)]
	internal class ReflectionMember
	{
		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000571 RID: 1393 RVA: 0x00016F36 File Offset: 0x00015136
		// (set) Token: 0x06000572 RID: 1394 RVA: 0x00016F3E File Offset: 0x0001513E
		public Type MemberType { get; set; }

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000573 RID: 1395 RVA: 0x00016F47 File Offset: 0x00015147
		// (set) Token: 0x06000574 RID: 1396 RVA: 0x00016F4F File Offset: 0x0001514F
		[Nullable(new byte[] { 2, 1, 2 })]
		public Func<object, object> Getter
		{
			[return: Nullable(new byte[] { 2, 1, 2 })]
			get;
			[param: Nullable(new byte[] { 2, 1, 2 })]
			set;
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000575 RID: 1397 RVA: 0x00016F58 File Offset: 0x00015158
		// (set) Token: 0x06000576 RID: 1398 RVA: 0x00016F60 File Offset: 0x00015160
		[Nullable(new byte[] { 2, 1, 2 })]
		public Action<object, object> Setter
		{
			[return: Nullable(new byte[] { 2, 1, 2 })]
			get;
			[param: Nullable(new byte[] { 2, 1, 2 })]
			set;
		}
	}
}
