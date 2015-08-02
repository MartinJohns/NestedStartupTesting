using System.Collections.Generic;
using Microsoft.AspNet.Mvc;
using Microsoft.Framework.Runtime;
using System.Linq;

namespace NestedStartupTesting.Shared
{
    public class SingleAssemblyProvider : DefaultAssemblyProvider
    {
        private readonly string _assemblyName;

        public SingleAssemblyProvider(ILibraryManager libraryManager, string assemblyName)
            : base(libraryManager)
        {
            _assemblyName = assemblyName;
        }

        protected override IEnumerable<ILibraryInformation> GetCandidateLibraries()
        {
            var result = base.GetCandidateLibraries().Where(x => x.Name == _assemblyName).ToList();
            return result;
        }
    }
}
