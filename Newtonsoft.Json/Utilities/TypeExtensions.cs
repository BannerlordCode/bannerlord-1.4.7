using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200006D RID: 109
	[NullableContext(1)]
	[Nullable(0)]
	internal static class TypeExtensions
	{
		// Token: 0x060005DD RID: 1501 RVA: 0x00018A50 File Offset: 0x00016C50
		public static MethodInfo Method(this Delegate d)
		{
			return d.Method;
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x00018A58 File Offset: 0x00016C58
		public static MemberTypes MemberType(this MemberInfo memberInfo)
		{
			return memberInfo.MemberType;
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x00018A60 File Offset: 0x00016C60
		public static bool ContainsGenericParameters(this Type type)
		{
			return type.ContainsGenericParameters;
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x00018A68 File Offset: 0x00016C68
		public static bool IsInterface(this Type type)
		{
			return type.IsInterface;
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x00018A70 File Offset: 0x00016C70
		public static bool IsGenericType(this Type type)
		{
			return type.IsGenericType;
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x00018A78 File Offset: 0x00016C78
		public static bool IsGenericTypeDefinition(this Type type)
		{
			return type.IsGenericTypeDefinition;
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x00018A80 File Offset: 0x00016C80
		public static Type BaseType(this Type type)
		{
			return type.BaseType;
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x00018A88 File Offset: 0x00016C88
		public static Assembly Assembly(this Type type)
		{
			return type.Assembly;
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x00018A90 File Offset: 0x00016C90
		public static bool IsEnum(this Type type)
		{
			return type.IsEnum;
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x00018A98 File Offset: 0x00016C98
		public static bool IsClass(this Type type)
		{
			return type.IsClass;
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x00018AA0 File Offset: 0x00016CA0
		public static bool IsSealed(this Type type)
		{
			return type.IsSealed;
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x00018AA8 File Offset: 0x00016CA8
		public static bool IsAbstract(this Type type)
		{
			return type.IsAbstract;
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x00018AB0 File Offset: 0x00016CB0
		public static bool IsVisible(this Type type)
		{
			return type.IsVisible;
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x00018AB8 File Offset: 0x00016CB8
		public static bool IsValueType(this Type type)
		{
			return type.IsValueType;
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x00018AC0 File Offset: 0x00016CC0
		public static bool IsPrimitive(this Type type)
		{
			return type.IsPrimitive;
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x00018AC8 File Offset: 0x00016CC8
		public static bool AssignableToTypeName(this Type type, string fullTypeName, bool searchInterfaces, [Nullable(2)] [NotNullWhen(true)] out Type match)
		{
			Type type2 = type;
			while (type2 != null)
			{
				if (string.Equals(type2.FullName, fullTypeName, StringComparison.Ordinal))
				{
					match = type2;
					return true;
				}
				type2 = type2.BaseType();
			}
			if (searchInterfaces)
			{
				Type[] interfaces = type.GetInterfaces();
				for (int i = 0; i < interfaces.Length; i++)
				{
					if (string.Equals(interfaces[i].Name, fullTypeName, StringComparison.Ordinal))
					{
						match = type;
						return true;
					}
				}
			}
			match = null;
			return false;
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x00018B30 File Offset: 0x00016D30
		public static bool AssignableToTypeName(this Type type, string fullTypeName, bool searchInterfaces)
		{
			Type type2;
			return type.AssignableToTypeName(fullTypeName, searchInterfaces, out type2);
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x00018B48 File Offset: 0x00016D48
		public static bool ImplementInterface(this Type type, Type interfaceType)
		{
			Type type2 = type;
			while (type2 != null)
			{
				foreach (Type type3 in ((IEnumerable<Type>)type2.GetInterfaces()))
				{
					if (type3 == interfaceType || (type3 != null && type3.ImplementInterface(interfaceType)))
					{
						return true;
					}
				}
				type2 = type2.BaseType();
			}
			return false;
		}
	}
}
