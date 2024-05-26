using System;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
namespace WutheringWavesFrameUnlocker___WWFU
{
	internal static class Program
	{
		/// <summary>
		///  The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			// To customize application configuration such as set high DPI settings or default font,
			// see https://aka.ms/applicationconfiguration.
			//AppDomain.CurrentDomain.AssemblyResolve += ResolveAssembly;

			ApplicationConfiguration.Initialize();
			Application.Run(new MainForm());
		}

		//private static Assembly? ResolveAssembly(object? sender, ResolveEventArgs args)
		//{
		//	throw new NotImplementedException();
		//}

		//private static Assembly? ResolveAssembly(object? sender, ResolveEventArgs args)
		//{
		//	var thisAssembly = Assembly.GetExecutingAssembly();
		//	var name = args.Name.Substring(0, args.Name.IndexOf(',')) + ".dll";

		//	var resources = thisAssembly.GetManifestResourceNames().Where(s => s.EndsWith(name));
		//	var enumerable = resources.ToList();

		//	if (!enumerable.Any())
		//	{
		//		return null;
		//	}

		//	var resourceName = enumerable.First();
		//	using var stream = thisAssembly.GetManifestResourceStream(resourceName);

		//	if (stream == null)
		//	{
		//		return null;
		//	}
		//	var assembly = new byte[stream.Length];
		//	stream.Read(assembly, 0, assembly.Length);

		//	return Assembly.Load(assembly);
		//}
	}
}




//using System;
//using System.Linq;
//using System.Reflection;
//using System.Windows.Forms;
//namespace ExeWithDLL
//{
//	static class Program
//	{ 
///// <summary> 
///// 해당 애플리케이션의 주 진입점입니다.
///// </summary> 
//[STAThread] private static void Main() 
//{
//// 리소스 dll 취득
//AppDomain.CurrentDomain.AssemblyResolve += ResolveAssembly; 
//Application.EnableVisualStyles();
//Application.SetCompatibleTextRenderingDefault(false);
//Application.Run(new Form1()); 
//}
//private static Assembly ResolveAssembly(object sender, ResolveEventArgs args)
//{
//	var thisAssembly = Assembly.GetExecutingAssembly();
//	var name = args.Name.Substring(0, args.Name.IndexOf(',')) + ".dll";
//	var resources = thisAssembly.GetManifestResourceNames().Where(s => s.EndsWith(name)); var enumerable = resources.ToList();
//	if (!enumerable.Any()) return null; var resourceName = enumerable.First(); using var stream = thisAssembly.GetManifestResourceStream(resourceName);
//	if (stream == null) return null; var assembly = new byte[stream.Length]; stream.Read(assembly, 0, assembly.Length); return Assembly.Load(assembly);
//}
//}
//}
