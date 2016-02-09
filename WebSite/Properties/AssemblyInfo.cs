using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using angularjs.Properties;
using FoodApp.Common;
using FoodApp.Properties;
using SharpKit.JavaScript;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("FoodApp")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("FoodApp")]
[assembly: AssemblyCopyright("Copyright ©  2015")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("8bc33d82-c0e7-44bf-af2a-999547d6dd0b")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Revision and Build Numbers 
// by using the '*' as shown below:
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
[assembly: JsMergedFile(Filename = WebApiResources._assemblyOutputPath, Sources = new[] {
    angularjsResources.includeBeforeResources,
    CommonApiResources.includeClientJs,
    WebApiResources._assemblyClientPath,
    angularjsResources.includeAfterResources
})]
