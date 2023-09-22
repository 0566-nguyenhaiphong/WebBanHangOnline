using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBanHangOnline.Models.Common
{
    public class FormatNumber
    {
        public static string FormatNumber1(int number)
        {
            return $"{number:N0}";
        }
    }
}