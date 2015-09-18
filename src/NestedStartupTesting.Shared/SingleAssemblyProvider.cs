using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Mvc;
using Microsoft.Dnx.Runtime;

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

        protected override IEnumerable<Library> GetCandidateLibraries()
        {
            var result = base.GetCandidateLibraries().Where(x => x.Name == _assemblyName).ToList();
            return result;
        }
    }
}
