using System;

namespace _Project.Code.Runtime.Utils
{
    public class AddressableViewAttribute : Attribute
    {
        public string KeyName { get; }

        public AddressableViewAttribute(string keyName)
        {
            KeyName = keyName;
        }
    }
}