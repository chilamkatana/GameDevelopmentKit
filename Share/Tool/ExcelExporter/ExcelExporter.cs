using System.IO;

namespace ET
{
    public static partial class ExcelExporter
    {
        private static readonly string s_LocalizationExcelFile = Path.GetFullPath($"{Define.WorkDir}/../Design/Excel/Localization.xlsx");

        public static void Export()
        {
            ExcelExporter_Luban.DoExport();
            ExcelExporter_Localization.DoExport();
        }
    }
}