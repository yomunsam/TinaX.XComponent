namespace TinaX.XComponent
{
    public static class XComponentExtensions
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
