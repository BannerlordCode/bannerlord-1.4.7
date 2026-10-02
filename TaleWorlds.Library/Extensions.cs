using System;
using System.Collections.Generic;
using System.Reflection;

namespace TaleWorlds.Library
{
	// Token: 0x0200002D RID: 45
	public static class Extensions
	{
		// Token: 0x0600016A RID: 362 RVA: 0x00005E70 File Offset: 0x00004070
		public static List<Type> GetTypesSafe(this Assembly assembly, Func<Type, bool> func = null)
		{
			List<Type> list = new List<Type>();
			Type[] array;
			try
			{
				array = assembly.GetTypes();
			}
			catch (ReflectionTypeLoadException ex)
			{
				array = ex.Types;
				foreach (Exception ex2 in ex.LoaderExceptions)
				{
				}
			}
			catch (Exception)
			{
				array = Array.Empty<Type>();
			}
			try
			{
				foreach (Type type in array)
				{
					if (type != null && (func == null || func(type)))
					{
						list.Add(type);
					}
				}
			}
			catch (Exception)
			{
			}
			return list;
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00005F1C File Offset: 0x0000411C
		public static Assembly[] GetReferencingAssembliesSafe(this Assembly baseAssembly, Func<Assembly, bool> func = null)
		{
			Assembly[] assemblies;
			try
			{
				assemblies = AppDomain.CurrentDomain.GetAssemblies();
			}
			catch (Exception ex)
			{
				Debug.FailedAssert(ex.Message, "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.Library\\Extensions.cs", "GetReferencingAssembliesSafe", 72);
				return Array.Empty<Assembly>();
			}
			List<Assembly> list = new List<Assembly>();
			foreach (Assembly assembly in assemblies)
			{
				foreach (AssemblyName assemblyName in assembly.GetReferencedAssemblies())
				{
					try
					{
						if (assemblyName.ToString() == baseAssembly.GetName().ToString() && (func == null || func(assembly)))
						{
							list.Add(assembly);
							break;
						}
					}
					catch
					{
						Debug.FailedAssert(string.Format("Error while resolving references of assembly: {0}", assembly), "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.Library\\Extensions.cs", "GetReferencingAssembliesSafe", 93);
					}
				}
			}
			return list.ToArray();
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00006018 File Offset: 0x00004218
		public static object[] GetCustomAttributesSafe(this Type type, Type attributeType, bool inherit)
		{
			try
			{
				return type.GetCustomAttributes(attributeType, inherit);
			}
			catch (Exception ex)
			{
				Debug.FailedAssert(string.Concat(new string[] { "Failed to get custom attributes (", attributeType.Name, ") for type: ", type.Name, ". Exception: ", ex.Message }), "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.Library\\Extensions.cs", "GetCustomAttributesSafe", 113);
			}
			return Array.Empty<object>();
		}

		// Token: 0x0600016D RID: 365 RVA: 0x0000609C File Offset: 0x0000429C
		public static object[] GetCustomAttributesSafe(this Type type, bool inherit)
		{
			try
			{
				return type.GetCustomAttributes(inherit);
			}
			catch (Exception ex)
			{
				Debug.FailedAssert("Failed to get custom attributes for type: " + type.Name + ". Exception: " + ex.Message, "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.Library\\Extensions.cs", "GetCustomAttributesSafe", 128);
			}
			return Array.Empty<object>();
		}

		// Token: 0x0600016E RID: 366 RVA: 0x000060FC File Offset: 0x000042FC
		public static IEnumerable<Attribute> GetCustomAttributesSafe(this Type type, Type attributeType)
		{
			try
			{
				return type.GetCustomAttributes(attributeType);
			}
			catch (Exception ex)
			{
				Debug.FailedAssert(string.Concat(new string[] { "Failed to get custom attributes (", attributeType.Name, ") for type: ", type.Name, ". Exception: ", ex.Message }), "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.Library\\Extensions.cs", "GetCustomAttributesSafe", 143);
			}
			return new List<Attribute>();
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00006180 File Offset: 0x00004380
		public static object[] GetCustomAttributesSafe(this PropertyInfo property, Type attributeType, bool inherit)
		{
			try
			{
				return property.GetCustomAttributes(attributeType, inherit);
			}
			catch (Exception ex)
			{
				Debug.FailedAssert(string.Concat(new string[] { "Failed to get custom attributes (", attributeType.Name, ") for property: ", property.Name, ". Exception: ", ex.Message }), "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.Library\\Extensions.cs", "GetCustomAttributesSafe", 158);
			}
			return Array.Empty<object>();
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00006204 File Offset: 0x00004404
		public static object[] GetCustomAttributesSafe(this PropertyInfo property, bool inherit)
		{
			try
			{
				return property.GetCustomAttributes(inherit);
			}
			catch (Exception ex)
			{
				Debug.FailedAssert("Failed to get custom attributes for property: " + property.Name + ". Exception: " + ex.Message, "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.Library\\Extensions.cs", "GetCustomAttributesSafe", 173);
			}
			return Array.Empty<object>();
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00006264 File Offset: 0x00004464
		public static IEnumerable<Attribute> GetCustomAttributesSafe(this PropertyInfo property, Type attributeType)
		{
			try
			{
				return property.GetCustomAttributes(attributeType);
			}
			catch (Exception ex)
			{
				Debug.FailedAssert("Failed to get custom attributes for property: " + property.Name + ". Exception: " + ex.Message, "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.Library\\Extensions.cs", "GetCustomAttributesSafe", 188);
			}
			return new List<Attribute>();
		}

		// Token: 0x06000172 RID: 370 RVA: 0x000062C4 File Offset: 0x000044C4
		public static object[] GetCustomAttributesSafe(this FieldInfo field, Type attributeType, bool inherit)
		{
			try
			{
				return field.GetCustomAttributes(attributeType, false);
			}
			catch (Exception ex)
			{
				Debug.FailedAssert(string.Concat(new string[] { "Failed to get custom attributes (", attributeType.Name, ") for field: ", field.Name, ". Exception: ", ex.Message }), "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.Library\\Extensions.cs", "GetCustomAttributesSafe", 203);
			}
			return Array.Empty<object>();
		}

		// Token: 0x06000173 RID: 371 RVA: 0x00006348 File Offset: 0x00004548
		public static object[] GetCustomAttributesSafe(this FieldInfo field, bool inherit)
		{
			try
			{
				return field.GetCustomAttributes(inherit);
			}
			catch (Exception ex)
			{
				Debug.FailedAssert("Failed to get custom attributes for field: " + field.Name + ". Exception: " + ex.Message, "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.Library\\Extensions.cs", "GetCustomAttributesSafe", 218);
			}
			return Array.Empty<object>();
		}

		// Token: 0x06000174 RID: 372 RVA: 0x000063A8 File Offset: 0x000045A8
		public static IEnumerable<Attribute> GetCustomAttributesSafe(this FieldInfo field, Type attributeType)
		{
			try
			{
				return field.GetCustomAttributes(attributeType);
			}
			catch (Exception ex)
			{
				Debug.FailedAssert("Failed to get custom attributes for field: " + field.Name + ". Exception: " + ex.Message, "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.Library\\Extensions.cs", "GetCustomAttributesSafe", 233);
			}
			return new List<Attribute>();
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00006408 File Offset: 0x00004608
		public static object[] GetCustomAttributesSafe(this MethodInfo method, Type attributeType, bool inherit)
		{
			try
			{
				return method.GetCustomAttributes(attributeType, false);
			}
			catch (Exception ex)
			{
				Debug.FailedAssert(string.Concat(new string[] { "Failed to get custom attributes (", attributeType.Name, ") for method: ", method.Name, ". Exception: ", ex.Message }), "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.Library\\Extensions.cs", "GetCustomAttributesSafe", 248);
			}
			return Array.Empty<object>();
		}

		// Token: 0x06000176 RID: 374 RVA: 0x0000648C File Offset: 0x0000468C
		public static object[] GetCustomAttributesSafe(this MethodInfo method, bool inherit)
		{
			try
			{
				return method.GetCustomAttributes(inherit);
			}
			catch (Exception ex)
			{
				Debug.FailedAssert("Failed to get custom attributes for method: " + method.Name + ". Exception: " + ex.Message, "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.Library\\Extensions.cs", "GetCustomAttributesSafe", 263);
			}
			return Array.Empty<object>();
		}

		// Token: 0x06000177 RID: 375 RVA: 0x000064EC File Offset: 0x000046EC
		public static IEnumerable<Attribute> GetCustomAttributesSafe(this MethodInfo method, Type attributeType)
		{
			try
			{
				return method.GetCustomAttributes(attributeType);
			}
			catch (Exception ex)
			{
				Debug.FailedAssert("Failed to get custom attributes for method: " + method.Name + ". Exception: " + ex.Message, "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.Library\\Extensions.cs", "GetCustomAttributesSafe", 278);
			}
			return new List<Attribute>();
		}

		// Token: 0x06000178 RID: 376 RVA: 0x0000654C File Offset: 0x0000474C
		public static object[] GetCustomAttributesSafe(this Assembly assembly, Type attributeType, bool inherit)
		{
			try
			{
				return assembly.GetCustomAttributes(attributeType, false);
			}
			catch (Exception ex)
			{
				Debug.FailedAssert(string.Concat(new string[] { "Failed to get custom attributes (", attributeType.Name, ") for assembly: ", assembly.FullName, ". Exception: ", ex.Message }), "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.Library\\Extensions.cs", "GetCustomAttributesSafe", 293);
			}
			return Array.Empty<object>();
		}

		// Token: 0x06000179 RID: 377 RVA: 0x000065D0 File Offset: 0x000047D0
		public static object[] GetCustomAttributesSafe(this Assembly assembly, bool inherit)
		{
			try
			{
				return assembly.GetCustomAttributes(inherit);
			}
			catch (Exception ex)
			{
				Debug.FailedAssert("Failed to get custom attributes for assembly: " + assembly.FullName + ". Exception: " + ex.Message, "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.Library\\Extensions.cs", "GetCustomAttributesSafe", 308);
			}
			return Array.Empty<object>();
		}

		// Token: 0x0600017A RID: 378 RVA: 0x00006630 File Offset: 0x00004830
		public static IEnumerable<Attribute> GetCustomAttributesSafe(this Assembly assembly, Type attributeType)
		{
			try
			{
				return assembly.GetCustomAttributes(attributeType);
			}
			catch (Exception ex)
			{
				Debug.FailedAssert("Failed to get custom attributes for assembly: " + assembly.FullName + ". Exception: " + ex.Message, "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.Library\\Extensions.cs", "GetCustomAttributesSafe", 323);
			}
			return new List<Attribute>();
		}

		// Token: 0x0600017B RID: 379 RVA: 0x00006690 File Offset: 0x00004890
		public static MBList<T> ToMBList<T>(this T[] source)
		{
			MBList<T> mblist = new MBList<T>(source.Length);
			mblist.AddRange(source);
			return mblist;
		}

		// Token: 0x0600017C RID: 380 RVA: 0x000066A1 File Offset: 0x000048A1
		public static MBList<T> ToMBList<T>(this List<T> source)
		{
			MBList<T> mblist = new MBList<T>(source.Count);
			mblist.AddRange(source);
			return mblist;
		}

		// Token: 0x0600017D RID: 381 RVA: 0x000066B8 File Offset: 0x000048B8
		public static MBList<T> ToMBList<T>(this IEnumerable<T> source)
		{
			T[] array;
			if ((array = source as T[]) != null)
			{
				return array.ToMBList<T>();
			}
			List<T> list;
			if ((list = source as List<T>) != null)
			{
				return list.ToMBList<T>();
			}
			MBList<T> mblist = new MBList<T>();
			mblist.AddRange(source);
			return mblist;
		}

		// Token: 0x0600017E RID: 382 RVA: 0x000066F4 File Offset: 0x000048F4
		public static void AppendList<T>(this List<T> list1, List<T> list2)
		{
			if (list1.Count + list2.Count > list1.Capacity)
			{
				list1.Capacity = list1.Count + list2.Count;
			}
			for (int i = 0; i < list2.Count; i++)
			{
				list1.Add(list2[i]);
			}
		}

		// Token: 0x0600017F RID: 383 RVA: 0x00006747 File Offset: 0x00004947
		public static MBReadOnlyDictionary<TKey, TValue> GetReadOnlyDictionary<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
		{
			return new MBReadOnlyDictionary<TKey, TValue>(dictionary);
		}

		// Token: 0x06000180 RID: 384 RVA: 0x0000674F File Offset: 0x0000494F
		public static bool HasAnyFlag<T>(this T p1, T p2) where T : struct
		{
			return EnumHelper<T>.HasAnyFlag(p1, p2);
		}

		// Token: 0x06000181 RID: 385 RVA: 0x0000675D File Offset: 0x0000495D
		public static bool HasAllFlags<T>(this T p1, T p2) where T : struct
		{
			return EnumHelper<T>.HasAllFlags(p1, p2);
		}

		// Token: 0x06000182 RID: 386 RVA: 0x0000676B File Offset: 0x0000496B
		public static int GetDeterministicHashCode(this string text)
		{
			return Common.GetDJB2(text);
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00006774 File Offset: 0x00004974
		public static int IndexOfMin<TSource>(this IReadOnlyList<TSource> self, Func<TSource, int> func)
		{
			int num = int.MaxValue;
			int num2 = -1;
			for (int i = 0; i < self.Count; i++)
			{
				int num3 = func(self[i]);
				if (num3 < num)
				{
					num = num3;
					num2 = i;
				}
			}
			return num2;
		}

		// Token: 0x06000184 RID: 388 RVA: 0x000067B4 File Offset: 0x000049B4
		public static int IndexOfMin<TSource>(this MBReadOnlyList<TSource> self, Func<TSource, int> func)
		{
			int num = int.MaxValue;
			int num2 = -1;
			for (int i = 0; i < self.Count; i++)
			{
				int num3 = func(self[i]);
				if (num3 < num)
				{
					num = num3;
					num2 = i;
				}
			}
			return num2;
		}

		// Token: 0x06000185 RID: 389 RVA: 0x000067F4 File Offset: 0x000049F4
		public static int IndexOfMax<TSource>(this IReadOnlyList<TSource> self, Func<TSource, int> func)
		{
			int num = int.MinValue;
			int num2 = -1;
			for (int i = 0; i < self.Count; i++)
			{
				int num3 = func(self[i]);
				if (num3 > num)
				{
					num = num3;
					num2 = i;
				}
			}
			return num2;
		}

		// Token: 0x06000186 RID: 390 RVA: 0x00006834 File Offset: 0x00004A34
		public static int IndexOfMax<TSource>(this MBReadOnlyList<TSource> self, Func<TSource, int> func)
		{
			int num = int.MinValue;
			int num2 = -1;
			for (int i = 0; i < self.Count; i++)
			{
				int num3 = func(self[i]);
				if (num3 > num)
				{
					num = num3;
					num2 = i;
				}
			}
			return num2;
		}

		// Token: 0x06000187 RID: 391 RVA: 0x00006874 File Offset: 0x00004A74
		public static int IndexOf<TValue>(this TValue[] source, TValue item)
		{
			for (int i = 0; i < source.Length; i++)
			{
				if (source[i].Equals(item))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06000188 RID: 392 RVA: 0x000068B0 File Offset: 0x00004AB0
		public static int FindIndex<TValue>(this IReadOnlyList<TValue> source, Func<TValue, bool> predicate)
		{
			for (int i = 0; i < source.Count; i++)
			{
				if (predicate(source[i]))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06000189 RID: 393 RVA: 0x000068E0 File Offset: 0x00004AE0
		public static int FindIndex<TValue>(this MBReadOnlyList<TValue> source, Func<TValue, bool> predicate)
		{
			for (int i = 0; i < source.Count; i++)
			{
				if (predicate(source[i]))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x0600018A RID: 394 RVA: 0x00006910 File Offset: 0x00004B10
		public static int FindLastIndex<TValue>(this IReadOnlyList<TValue> source, Func<TValue, bool> predicate)
		{
			for (int i = source.Count - 1; i >= 0; i--)
			{
				if (predicate(source[i]))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x0600018B RID: 395 RVA: 0x00006944 File Offset: 0x00004B44
		public static int FindLastIndex<TValue>(this MBReadOnlyList<TValue> source, Func<TValue, bool> predicate)
		{
			for (int i = source.Count - 1; i >= 0; i--)
			{
				if (predicate(source[i]))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x0600018C RID: 396 RVA: 0x00006978 File Offset: 0x00004B78
		public static void Randomize<T>(this IList<T> array)
		{
			Random random = new Random();
			int i = array.Count;
			while (i > 1)
			{
				i--;
				int num = random.Next(0, i + 1);
				T t = array[num];
				array[num] = array[i];
				array[i] = t;
			}
		}
	}
}
