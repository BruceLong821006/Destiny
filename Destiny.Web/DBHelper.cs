using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace Destiny.Web.DB
{
    public class DBHelper
    {
        private string DBPath = ConfigurationManager.AppSettings["DBPath"];
    }
}