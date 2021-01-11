using System;
            
namespace CoreLibrary.Extensions 
{
    public static class CoreLibraryExtensions {
        public static String SafeToString(this object obj) 
        {
            if (ReferenceEquals(obj, null)) return String.Empty;
            else return obj.ToString();
        }
    }
}