using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Mvc;
using Microsoft.Framework.Runtime;

namespace NestedStartupTesting.Web
{
    internal class MainAssemblyProvider : DefaultAssemblyProvider
    {
        public MainAssemblyProvider(ILibraryManager libraryManager)
            : base(libraryManager)
        {
        }

        protected override IEnumerable<ILibraryInformation> GetCandidateLibraries()
        {
            return base.GetCandidateLibraries().Where(x => x.Name == "NestedStartupTesting.Web");
        }
    }
}