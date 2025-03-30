using System.IO;

namespace UserControl_Sample
{
    class IniManagedData
    {

        public string iniPath { get; set; } = Directory.GetCurrentDirectory() + "\\setting.ini";
        public string usedOriginalArgument { get; internal set; } =string.Empty;

        public static class ControlField
        {
            public static string OriginalUserControl { get; } = nameof(OriginalUserControl);
        }
    }
}
