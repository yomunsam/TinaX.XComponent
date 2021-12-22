using TinaX.XComponent.Warpper;

namespace TinaX.XComponent
{
    public static class XComponentExtensions
    {
        public static XComponent AddBehaviour(this XComponent xscript, XBehaviour behaviuor, bool injectBinding = true)
        {
            if(xscript.Behaviour == null)
            {
                if (injectBinding)
                {
                    if(behaviuor is XBehaviourWarpper)
                    {
                        //这是包装器
                        var behaviuorWarpper = (XBehaviourWarpper)behaviuor;
                        behaviuorWarpper.InjectBindings(xscript);
                    }
                    else
                    {
                        XComponents.InjectBindings(xscript, behaviuor);
                    }
                }
                xscript.SetBehaviour(behaviuor);
            }
            return xscript;
        }
    }
}
