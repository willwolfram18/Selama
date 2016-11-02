using System.Collections.Generic;
using System.Web;

namespace Selama.Tests.Common
{
    public class InMemorySession : HttpSessionStateBase
    {
        Dictionary<string, object> _session = new Dictionary<string, object>();

        public override object this[string name]
        {
            get
            {
                if (!_session.ContainsKey(name))
                {
                    _session[name] = null;
                }
                return _session[name];
            }
            set
            {
                _session[name] = value;
            }
        }

        public override void Add(string name, object value)
        {
            _session.Add(name, value);
        }
        public override void Remove(string name)
        {
            _session.Remove(name);
        }
        public override System.Collections.IEnumerator GetEnumerator()
        {
            return _session.GetEnumerator();
        }
        public override void Clear()
        {
            _session.Clear();
        }
        public override void Abandon()
        {
            _session = new Dictionary<string, object>();
        }
        public override void RemoveAll()
        {
            _session.Clear();
        }
    }
}
