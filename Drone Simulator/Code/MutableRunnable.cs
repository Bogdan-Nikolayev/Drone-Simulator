using System;
using Java.Lang;

namespace Drone_Simulator
{
    public class MutableRunnable : Java.Lang.Object, IRunnable
    {
        public MutableRunnable(Action action)
        {
            Action = action;
        }

        public Action Action { get; set; }

        public void Run()
        {
            Action.Invoke();
        }
    }
}