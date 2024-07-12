using OfficeOpenXml;
using StockCounterBackOffice.Models;

namespace StockCounterBackOffice.Helpers
{
    public static class ExportHelper
    {
        public static void ExportToExcel(List<ExportedItem> exportedItems, string fileName)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Inventory");

                var headers = new string[]
                {
                    "Item No", "Item User Define", "Barcode", "Description",
                    "BUOM", "Stocks(Pcs)", "Lot #", "Expiration",
                    "Variance", "Rack", "CFactor", "Cntr"
                };

                for (int i = 0; i < headers.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = headers[i];
                }

                for (int i = 0; i < exportedItems.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = exportedItems[i].ItemNo;
                    worksheet.Cells[i + 2, 2].Value = exportedItems[i].ItemUserDefine;
                    worksheet.Cells[i + 2, 3].Value = exportedItems[i].Barcode;
                    worksheet.Cells[i + 2, 4].Value = exportedItems[i].Description;
                    worksheet.Cells[i + 2, 5].Value = exportedItems[i].BUOM;
                    worksheet.Cells[i + 2, 6].Value = exportedItems[i].Stocks;
                    worksheet.Cells[i + 2, 7].Value = exportedItems[i].LotNo;
                    worksheet.Cells[i + 2, 8].Value = exportedItems[i].Expiration;
                    worksheet.Cells[i + 2, 9].Value = exportedItems[i].Variance;
                    worksheet.Cells[i + 2, 10].Value = exportedItems[i].Rack;
                    worksheet.Cells[i + 2, 11].Value = exportedItems[i].CFactor;
                    worksheet.Cells[i + 2, 12].Value = exportedItems[i].Counter;
                }

                using (var range = worksheet.Cells["A1:L1"])
                {
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                }

                package.SaveAs(new FileInfo(fileName));
            }
        }
    }
}
