using System.IO;
using System.Xml;


namespace WpfXamlSettingsFile
{
    public static class SettingsHelper
    {
        private static Dictionary<eXmlSettingsKeys, string> dicXmlSettings;
        private static string initialDirectoryPath = @"D:\TestingProjects\XmlSetting";
        private static string filePath= @"D:\TestingProjects\XmlSetting\XmlSetiings.xml";
        private static readonly object lockObject = new object();
        private static XmlDocument xmlGlobalDocInstance = null;
        public static void LoadSettings()
        {
            xmlGlobalDocInstance = new XmlDocument();
           
            if (!File.Exists(filePath))
            {
                CreateAndSaveXmlDocument();
            }
            if (File.Exists(filePath))
            {
                foreach (eXmlSettingsKeys key in Enum.GetValues(typeof(eXmlSettingsKeys)))
                {
                    bool result = IsKeyExistInXml(key);
                    if (!result)
                    {
                        AddNewKeyToXml(key.ToString());
                    }
                }
                LoadXmlSettingDictionary();
            }

        }
        private static void LoadXmlSettingDictionary()
        {
            var settingResult =XmlSettingsFromFile(filePath);
            if (settingResult != null)
            {
                dicXmlSettings = settingResult;
            }
        }
        // Check if a key exists in the XML file based on enum
        private static bool IsKeyExistInXml(eXmlSettingsKeys keyToCheck)
        {
            if (System.IO.File.Exists(filePath))
            {
                //XmlDocument xmlDoc = new XmlDocument();
                xmlGlobalDocInstance.Load(filePath);
                foreach (XmlNode node in xmlGlobalDocInstance.SelectNodes("//KeyPairValueSetting"))
                {
                    if (Enum.TryParse(node.Attributes["key"].Value, out eXmlSettingsKeys keyInXml))
                    {
                        if (keyInXml == keyToCheck)
                        {
                            return true; // Key exists in the XML file
                        }
                    }
                }
            }
            return false; // Key does not exist in the XML file or file doesn't exist
        }

        // Add a new key-value pair to the XML file
        private static void AddNewKeyToXml(string newKey)
        {
            // XmlDocument xmlDoc = new XmlDocument();

            if (System.IO.File.Exists(filePath))
            {
                xmlGlobalDocInstance.Load(filePath);
            }
            else
            {
                //Create a new XML document with the root element
               XmlElement rootElement = xmlGlobalDocInstance.CreateElement("KeyPairValueSettings");
                xmlGlobalDocInstance.AppendChild(rootElement);
            }

            // Create a new element for the new key-value pair
            XmlElement newSettingElement = xmlGlobalDocInstance.CreateElement("KeyPairValueSetting");
            newSettingElement.SetAttribute("key", newKey);
            newSettingElement.SetAttribute("value", "");

            // Append the new element to the XML document
            xmlGlobalDocInstance.DocumentElement?.AppendChild(newSettingElement);

            // Save the updated XML document to the file
            xmlGlobalDocInstance.Save(filePath);
        }

        private static void CreateAndSaveXmlDocument()
        {

            // Create the XML declaration
            if (!Directory.Exists(initialDirectoryPath))
            {
                Directory.CreateDirectory(initialDirectoryPath);
            }
            XmlDeclaration xmlDeclaration = xmlGlobalDocInstance.CreateXmlDeclaration("1.0", "utf-8", null);
            xmlGlobalDocInstance.AppendChild(xmlDeclaration);

            // Create the root element
            XmlElement rootElement = xmlGlobalDocInstance.CreateElement("KeyPairValueSettings");
            xmlGlobalDocInstance.AppendChild(rootElement);

            // Add xmlns attributes
            //rootElement.SetAttribute("xmlns:xsd", "http://www.w3.org/2001/XMLSchema");
            //rootElement.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");

            // Save the XML document to the specified file path
            xmlGlobalDocInstance.Save(filePath);
        }
        private static Dictionary<eXmlSettingsKeys, string> XmlSettingsFromFile(string filePath)
        {
            Dictionary<eXmlSettingsKeys, string> keyValuePairs = new Dictionary<eXmlSettingsKeys, string>();
            if (File.Exists(filePath))
            {
                xmlGlobalDocInstance.Load(filePath);
                foreach (XmlNode node in xmlGlobalDocInstance.SelectNodes("//KeyPairValueSetting"))
                {
                    string key = node.Attributes["key"].Value;
                    string value = node.Attributes["value"].Value;
                    keyValuePairs[(eXmlSettingsKeys)Enum.Parse(typeof(eXmlSettingsKeys), key)] = value;
                }
            }
            return keyValuePairs;
        }
        public static string GetValue(eXmlSettingsKeys key)
        {
            string retValue = "";
            // Use lock to make access to the dictionary thread-safe
            lock (lockObject)
            {
                if (dicXmlSettings.ContainsKey(key))
                {
                    retValue = dicXmlSettings[key];
                }
            }
            return retValue;
        }
        public static bool SetValue(eXmlSettingsKeys keyToUpdate, string newValue)
        {
            bool retValue = false;
            
            dicXmlSettings[keyToUpdate] = newValue;
            lock (lockObject)
            {
                XmlElement rootElement;

                if (System.IO.File.Exists(filePath))
                {
                    xmlGlobalDocInstance.Load(filePath);
                    rootElement = xmlGlobalDocInstance.DocumentElement;
                    rootElement.RemoveAll(); // Clear existing settings
                }
                else
                {
                    rootElement = xmlGlobalDocInstance.CreateElement("KeyPairValueSettings");
                    xmlGlobalDocInstance.AppendChild(rootElement);
                }
                //// Add xmlns attributes
                //rootElement.SetAttribute("xmlns:xsd", "http://www.w3.org/2001/XMLSchema");
                //rootElement.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
                foreach (var entry in dicXmlSettings)
                {
                    XmlElement settingElement = xmlGlobalDocInstance.CreateElement("KeyPairValueSetting");
                    settingElement.SetAttribute("key", entry.Key.ToString());
                    settingElement.SetAttribute("value", entry.Value);
                    rootElement.AppendChild(settingElement);
                }
                xmlGlobalDocInstance.AppendChild(rootElement);
                xmlGlobalDocInstance.Save(filePath);
            }
            return retValue;
        }
    }
    public enum eXmlSettingsKeys
    {
        Key1,
        Key2,
        Key3,
        Key4,
        Key5,
    }
}
