using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProfileApp.Common
{
    public static class EnumExtentions
    {
        public static string DisplayName(this Enum item)
        {
            var type = item.GetType(); var member = type.GetMember(item.ToString()); DisplayAttribute displayName = (DisplayAttribute)member[0].GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault();
            if (displayName != null) { return displayName.Name; }
            return item.ToString();
        }





    }
}
