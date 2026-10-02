using System;
using System.Collections.Generic;
using System.Reflection;

namespace TaleWorlds.Library
{
	// Token: 0x02000023 RID: 35
	public static class CommandLineFunctionality
	{
		// Token: 0x060000C6 RID: 198 RVA: 0x0000471C File Offset: 0x0000291C
		private static bool CheckAssemblyReferencesThis(Assembly assembly)
		{
			Assembly assembly2 = typeof(CommandLineFunctionality).Assembly;
			if (assembly2.GetName().Name == assembly.GetName().Name)
			{
				return true;
			}
			AssemblyName[] referencedAssemblies = assembly.GetReferencedAssemblies();
			for (int i = 0; i < referencedAssemblies.Length; i++)
			{
				if (referencedAssemblies[i].Name == assembly2.GetName().Name)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x0000478C File Offset: 0x0000298C
		public static List<string> CollectCommandLineFunctions()
		{
			List<string> list = new List<string>();
			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				if (CommandLineFunctionality.CheckAssemblyReferencesThis(assembly))
				{
					foreach (Type type in assembly.GetTypesSafe(null))
					{
						foreach (MethodInfo methodInfo in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
						{
							object[] customAttributesSafe = methodInfo.GetCustomAttributesSafe(typeof(CommandLineFunctionality.CommandLineArgumentFunction), false);
							if (customAttributesSafe != null && customAttributesSafe.Length != 0)
							{
								CommandLineFunctionality.CommandLineArgumentFunction commandLineArgumentFunction = customAttributesSafe[0] as CommandLineFunctionality.CommandLineArgumentFunction;
								if (commandLineArgumentFunction != null && !(methodInfo.ReturnType != typeof(string)))
								{
									string name = commandLineArgumentFunction.Name;
									string text = commandLineArgumentFunction.GroupName + "." + name;
									if (!CommandLineFunctionality.AllFunctions.ContainsKey(text))
									{
										list.Add(text);
										CommandLineFunctionality.CommandLineFunction commandLineFunction = new CommandLineFunctionality.CommandLineFunction((Func<List<string>, string>)Delegate.CreateDelegate(typeof(Func<List<string>, string>), methodInfo));
										CommandLineFunctionality.AllFunctions.Add(text, commandLineFunction);
									}
								}
							}
						}
					}
				}
			}
			return list;
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x000048E8 File Offset: 0x00002AE8
		public static bool HasFunctionForCommand(string command)
		{
			CommandLineFunctionality.CommandLineFunction commandLineFunction;
			return CommandLineFunctionality.AllFunctions.TryGetValue(command, out commandLineFunction);
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00004904 File Offset: 0x00002B04
		public static string CallFunction(string concatName, string concatArguments, out bool found)
		{
			CommandLineFunctionality.CommandLineFunction commandLineFunction;
			if (CommandLineFunctionality.AllFunctions.TryGetValue(concatName, out commandLineFunction))
			{
				List<string> list;
				if (concatArguments != string.Empty)
				{
					list = new List<string>(concatArguments.Split(new char[] { ' ' }));
				}
				else
				{
					list = new List<string>();
				}
				found = true;
				return commandLineFunction.Call(list);
			}
			found = false;
			return "Could not find the command " + concatName;
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00004968 File Offset: 0x00002B68
		public static string CallFunction(string concatName, List<string> argList, out bool found)
		{
			CommandLineFunctionality.CommandLineFunction commandLineFunction;
			if (CommandLineFunctionality.AllFunctions.TryGetValue(concatName, out commandLineFunction))
			{
				found = true;
				return commandLineFunction.Call(argList);
			}
			found = false;
			return "Could not find the command " + concatName;
		}

		// Token: 0x04000073 RID: 115
		private static Dictionary<string, CommandLineFunctionality.CommandLineFunction> AllFunctions = new Dictionary<string, CommandLineFunctionality.CommandLineFunction>();

		// Token: 0x020000CA RID: 202
		private class CommandLineFunction
		{
			// Token: 0x06000754 RID: 1876 RVA: 0x00018560 File Offset: 0x00016760
			public CommandLineFunction(Func<List<string>, string> commandlinefunc)
			{
				this.CommandLineFunc = commandlinefunc;
				this.Children = new List<CommandLineFunctionality.CommandLineFunction>();
			}

			// Token: 0x06000755 RID: 1877 RVA: 0x0001857A File Offset: 0x0001677A
			public string Call(List<string> objects)
			{
				return this.CommandLineFunc(objects);
			}

			// Token: 0x04000259 RID: 601
			public Func<List<string>, string> CommandLineFunc;

			// Token: 0x0400025A RID: 602
			public List<CommandLineFunctionality.CommandLineFunction> Children;
		}

		// Token: 0x020000CB RID: 203
		public class CommandLineArgumentFunction : Attribute
		{
			// Token: 0x06000756 RID: 1878 RVA: 0x00018588 File Offset: 0x00016788
			public CommandLineArgumentFunction(string name, string groupname)
			{
				this.Name = name;
				this.GroupName = groupname;
			}

			// Token: 0x0400025B RID: 603
			public string Name;

			// Token: 0x0400025C RID: 604
			public string GroupName;
		}
	}
}
