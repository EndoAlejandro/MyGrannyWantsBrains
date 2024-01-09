using UnityEngine;

namespace DarkHavoc.CustomUtils.DebugEventButtonComponents
{
    public class ButtonAttribute : PropertyAttribute
    {
        public readonly string methodName;
        public ButtonAttribute(string methodName) => this.methodName = methodName;
    }
}