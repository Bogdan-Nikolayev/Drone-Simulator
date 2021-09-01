using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Drone_Simulator
{
    public static class Log
    {
        public static void Debug(params object[] messageParts)
        {
            MethodBase invokingMethod = new StackFrame(1).GetMethod();

            Android.Util.Log.Debug(Constants.Tag.DroneSimulator, string.Join(" ",
                new[] {invokingMethod.ReflectedType.Name, invokingMethod.Name}.Concat(messageParts)));
        }
    }
}