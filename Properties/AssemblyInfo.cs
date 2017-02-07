using System;
using System.Resources;
using System.Reflection;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Functional.Maybe")]
[assembly: AssemblyDescription(@"
	Option types for C# with LINQ support and rich fluent syntax for many popular uses:

	var maybeOne = ""one"".ToMaybe();
	var maybeAnother = Maybe<string>.Nothing;

	var maybeBoth = 
		from one in maybeOne
		from another in maybeAnother
		select one + another;

	maybeBoth.Match(
		both =>Console.WriteLine(""Result is: {0}"", both), 
		@else: () => Console.WriteLine(""Result is Nothing, as one of the inputs was Nothing"")
	);")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Andrey Tsvetkov; original version by William Casarin")]
[assembly: AssemblyProduct("Functional.Maybe")]
[assembly: AssemblyCopyright("Copyright © 2017")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: NeutralResourcesLanguage("en")]
[assembly: CLSCompliant(true)]

[assembly: AssemblyVersion("1.1.1")]
