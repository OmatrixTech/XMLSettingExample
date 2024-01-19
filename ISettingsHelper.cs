using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfXamlSettingsFile
{
    public interface ISettingsHelper
    {
        bool SetValue(eXmlSettingsKeys keyToUpdate, string newValue);
        string GetValue(eXmlSettingsKeys key);
    }
}
