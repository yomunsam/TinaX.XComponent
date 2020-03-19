using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinaX.XComponent
{
    public static class XComponentExtend
    {
        public static XComponent AddBehaviour(this XComponent xscript, XBehaviour behaviuor, bool inject_bindings = true)
        {
            if(xscript.Behaviour == null)
            {
                if (inject_bindings)
                {
                    XComponents.InjectBindings(xscript, behaviuor);
                }
                xscript.SetBehaviour(behaviuor);
            }
            return xscript;
        }
    }
}
