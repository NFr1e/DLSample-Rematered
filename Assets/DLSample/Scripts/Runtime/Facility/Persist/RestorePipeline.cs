using System.Collections.Generic;
using System.Linq;

namespace DLSample.Facility.Persist
{
    public abstract class RestorePipeline
    {
        private readonly List<IRestorer> _restorers;

        public RestorePipeline(IEnumerable<IRestorer> restorers)
        {
            _restorers = restorers.OrderBy(r => r.Order).ToList();
        }

        public void Restore()
        {
            foreach (var restorer in _restorers)
            {
                restorer.Restore();
            }
        }
    }
}
