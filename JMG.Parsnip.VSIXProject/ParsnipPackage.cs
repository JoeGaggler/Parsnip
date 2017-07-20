using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Win32;

namespace JMG.Parsnip.VSIXProject
{
	/// <summary>
	/// This is the class that implements the package exposed by this assembly.
	/// </summary>
	/// <remarks>
	/// <para>
	/// The minimum requirement for a class to be considered a valid package for Visual Studio
	/// is to implement the IVsPackage interface and register itself with the shell.
	/// This package uses the helper classes defined inside the Managed Package Framework (MPF)
	/// to do it: it derives from the Package class that provides the implementation of the
	/// IVsPackage interface and uses the registration attributes defined in the framework to
	/// register itself and its components with the shell. These attributes tell the pkgdef creation
	/// utility what data to put into .pkgdef file.
	/// </para>
	/// <para>
	/// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
	/// </para>
	/// </remarks>
	[PackageRegistration(UseManagedResourcesOnly = true)]
	[InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
	[Guid(ParsnipPackage.PackageGuidString)]
	[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    [ProvideAutoLoad(UIContextGuids.SolutionExists)]
	public sealed class ParsnipPackage : Package
	{
		/// <summary>
		/// VSPackage1 GUID string.
		/// </summary>
		public const string PackageGuidString = "dedc1c53-0202-4d15-af9b-92b3678f8a2a";

		/// <summary>
		/// Initializes a new instance of the <see cref="ParsnipPackage"/> class.
		/// </summary>
		public ParsnipPackage()
		{
			// Inside this method you can place any initialization code that does not require
			// any Visual Studio service because at this point the package object is created but
			// not sited yet inside Visual Studio environment. The place to do all the other
			// initialization is the Initialize method.
		}

		#region Package Members

		/// <summary>
		/// Initialization of the package; this method is called right after the package is sited, so this is the place
		/// where you can put all the initialization code that rely on services provided by VisualStudio.
		/// </summary>
		protected override void Initialize()
		{
			base.Initialize();

			RegisterGenerator("Parsnip", "Parsnip Packrat Parser Producer", ParsnipSingleFileGenerator.GUID_STRING_BRACES, typeof(ParsnipSingleFileGenerator));
		}

		private void RegisterGenerator(String toolId, String toolName, String generatorGUID, Type generatorType)
		{
			var classId = this.ApplicationRegistryRoot.OpenSubKey("CLSID", RegistryKeyPermissionCheck.ReadWriteSubTree);
			var myClassIdKey = classId.OpenSubKey(generatorGUID, RegistryKeyPermissionCheck.ReadWriteSubTree);
			if (null != myClassIdKey)
			{
				// Already registered
				return;
			}

			myClassIdKey = classId.CreateSubKey(generatorGUID, RegistryKeyPermissionCheck.ReadWriteSubTree);

			myClassIdKey.SetValue("InprocServer32", @"C:\Windows\SysWOW64\mscoree.dll", RegistryValueKind.String);
			myClassIdKey.SetValue("ThreadingModel", @"Both", RegistryValueKind.String);
			myClassIdKey.SetValue(null, generatorType.FullName, RegistryValueKind.String);
			myClassIdKey.SetValue("Class", generatorType.FullName, RegistryValueKind.String);
			myClassIdKey.SetValue("Assembly", generatorType.Assembly.FullName, RegistryValueKind.String);
			myClassIdKey.SetValue("CodeBase", generatorType.Assembly.Location, RegistryValueKind.String);

			var generators = this.ApplicationRegistryRoot.OpenSubKey("Generators", RegistryKeyPermissionCheck.ReadWriteSubTree);
			var csharp = generators.OpenSubKey("{FAE04EC1-301F-11D3-BF4B-00C04F79EFBC}", RegistryKeyPermissionCheck.ReadWriteSubTree);
			var mygenerator = csharp.CreateSubKey(toolId, RegistryKeyPermissionCheck.ReadWriteSubTree);
			mygenerator.SetValue(null, toolName, RegistryValueKind.String);
			mygenerator.SetValue("GeneratesDesignTimeSource", 1, RegistryValueKind.DWord);
			mygenerator.SetValue("CLSID", generatorGUID, RegistryValueKind.String);
		}

		#endregion
	}
}
