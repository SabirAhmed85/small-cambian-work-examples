using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace CambianKPI.DSL
{
    public interface IUtils
    {
        DateTime GetLastSunday();
        string[] CheckAuxOfFile(HttpRequestBase Request);
        string GetExtensionOfFile(string[] aux);
    }
}
