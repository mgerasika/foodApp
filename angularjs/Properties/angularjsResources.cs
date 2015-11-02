namespace angularjs.Properties
{
    public static class angularjsResources
    {
        public const string includeClientBeforeJs = "../angularjs/" + _assemblyClientBeforePath;
        public const string includeClientAfterJs = "../angularjs/" + _assemblyClientAfterPath;

        internal const string _assemblyClientBeforePath = "client/js/clientBefore.tmp.js";
        internal const string _assemblyClientAfterPath = "client/js/clientAfter.tmp.js";
        internal const string _fileClientBeforeJs = "~/" + _assemblyClientBeforePath;
        internal const string _fileClientAfterJs = "~/" + _assemblyClientAfterPath;
    }
}