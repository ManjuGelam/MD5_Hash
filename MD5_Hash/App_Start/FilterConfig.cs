﻿using System.Web;
using System.Web.Mvc;

namespace MD5_Hash
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
