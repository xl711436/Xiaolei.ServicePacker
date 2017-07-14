using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Xiaolei.ServicePacker
{
    /// <summary>读取枚举信息上自定义的attribute的帮助类
    /// </summary>
    public static class AttributeHelper
    {
        public static string GetEnumDescribeString<T>(T I_EnumValue) 
        {  
            string R_Result = string.Empty;

            FieldInfo tempFieldInfo =  typeof(T).GetField(I_EnumValue.ToString()); 
            object[] tempObjArray = tempFieldInfo.GetCustomAttributes(typeof(DescribeAttribute), false); 
            DescribeAttribute curAttribute = (DescribeAttribute)tempObjArray[0];

            R_Result = curAttribute.Describe; 
 
            return R_Result;  
        }
    }

    public class DescribeAttribute : Attribute
    { 
        public DescribeAttribute(string I_Describe)
        {
            Describe = I_Describe;
        }
        public string Describe { get; set; }
    }

 
}
