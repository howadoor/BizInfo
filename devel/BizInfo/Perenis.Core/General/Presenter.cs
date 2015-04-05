using System;
using System.Collections.Generic;
using System.Linq;
using Perenis.Core.Decoupling.Multimethods;

namespace Perenis.Core.General
{
    public class Presenter : IPresenter, IContextualPresenter
    {
        private MultiMethod.Action<object, object, object> _contextualPresent;
        private MultiMethod.Action<object, object> _present;

        public Presenter()
        {
        }

        public Presenter(IEnumerable<object> services)
        {
            Services = services;
        }

        public IEnumerable<object> Services
        {
            set
            {
                _present = ServiceDispatcher.Action<object, object>(Present, value.ToArray());
                _contextualPresent = ServiceDispatcher.Action<object, object, object>(Present, value.ToArray());
            }
        }

        #region IContextualPresenter Members

        public void Present(object source, object context, object target)
        {
            _contextualPresent(source, context, target);
        }

        #endregion

        #region IPresenter Members

        public void Present(object source, object target)
        {
            _present(source, target);
        }

        public bool CanPresent(object source, object target)
        {
            throw new NotImplementedException();
        }

        #endregion

        public bool CanPresent(object source)
        {
            throw new NotImplementedException();
        }
    }
}