using QuestPDF.Companion;
using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestPdfDemo
{
    internal class PdfGenerator
    {
        private MemoryStream _stream;
        private readonly string Font = "Arial Regular";
        private readonly string BoldFont = "Arial Bold";
        private readonly string ItalicFont = "Arial Italic";
        private float PageFontSize { get; set; } = 8;
        private float TableFontSize { get; set; } = 8;
        private float TableMarginLeft { get; set; }
        private float TableMarginRight { get; set; }



        public PdfGenerator()
        {
            _stream = new MemoryStream();
            QuestPDF.Settings.UseEnvironmentFonts = false;
            QuestPDF.Settings.License = LicenseType.Community;

            RegisterFont(Font, @"Pdf/arial/ARIAL.TTF");
            RegisterFont(BoldFont, @"Pdf/arial/ARIALBD.TTF");
            RegisterFont(ItalicFont, @"Pdf/arial/ARIALI.TTF");

        }

        private void RegisterFont(string fontName, string path)
        {
            var fullPath = Path.Combine(AppContext.BaseDirectory, path);
            using (var fontStream = File.OpenRead(fullPath))
            {
                FontManager.RegisterFontWithCustomName(fontName, fontStream);
            }
        }

        protected void PdfHeader(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeItem(3f)
                .AlignLeft()
                .Column(col =>
                {
                    col.Item()
                    .Text("Invoice from 12-09-2025 to 12-10-2025").FontFamily(BoldFont).FontSize(15);
                    col.Item().PaddingTop(10);
                    col.Item()
                    .Text("One Stop Kirana Store");
                    col.Item()
                    .Text("A-228, Madhuban Vihar, Pusta Road, Kulesra");
                    col.Item()
                   .Text("Greater Noida");
                    col.Item()
                   .Text("Uttar Pradesh");
                    col.Item()
                   .Text("201306 (India)");

                    col.Item().Row(row =>
                    {
                        row.AutoItem().Text("Phone: ");
                        row.RelativeItem().Text("8505996726");
                    });
                });
                row.RelativeItem(1.5f)
                .AlignLeft()
                .Column(col =>
                {
                    string formatedDate = DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    col.Spacing(1);
                    col.Item().Row(row =>
                    {
                        row.AutoItem().Text("Date: ");
                        row.RelativeItem().Text(formatedDate);
                    });

                    col.Item()
                    .Text("Deepak Keshri");
                    col.Item().Row(row =>
                    {
                        row.AutoItem().Text("Phone: ");
                        row.RelativeItem().Text("8505996726");
                    });
                });
            });
        }

        void AddLogo(IContainer container)
        {
            var path = Path.Combine(AppContext.BaseDirectory, @"Pdf/logo.jpg");
            using (var imagestream = File.OpenRead(path))
            {
                container.AlignRight()
                    .Width(35)
                    .Height(35)
                    .Image(imagestream)
                    .FitArea();
            }
        }
        public void GenerateFile()
        {
            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(42);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontFamily(Font));
                    page.DefaultTextStyle(x => x.FontSize(PageFontSize));

                    page.Background()
                    .Padding(30)
                    .Element(container =>
                    {
                        container.Border(.5f).BorderColor(Colors.Grey.Lighten1);
                    });

                    // Document Header
                    page.Header()
                        .ShowOnce()
                        .Column(column =>
                        {
                            AddLogo(column.Item());
                            PdfHeader(column.Item());
                        });


                    // Main content with regular font
                    page.Content().Column(column =>
                    {
                        // here we add Table

                        GenerateTable(column.Item());

                        column.Item().PaddingTop(30);
                        column.Item().Text("This is a normal paragraph using Arial Regular.")
                            .FontFamily(BoldFont)
                            .FontSize(14);
                        column.Item().Text("This text is important and uses Arial Bold Italic.")
                            .FontFamily(ItalicFont)
                            .FontSize(14);
                    });

                    // Footer
                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Page ");
                            x.CurrentPageNumber();
                            x.Span(" of ");
                            x.TotalPages();
                        });
                });
            })
            //.GeneratePdf("output.pdf");
            .ShowInCompanion();

        }

        protected virtual (string Name, float Width)[] GetTableHeaders()
        {
            return new (string, float)[]
            {
                ("Col1",1f),
                ("col2",2f)
            };
        }

        protected virtual IContainer CellStyle(IContainer container)
        {
            float borderSize = .5f;
            return container
                    .BorderLeft(borderSize)
                    .BorderBottom(borderSize)
                    .BorderRight(borderSize)
                    .PaddingVertical(4f);
        }

        protected virtual void GenerateTable(IContainer container)
        {
            var tableHeaders = GetTableHeaders();
            container.PaddingTop(10)
                .PaddingLeft(TableMarginLeft)
                .PaddingRight(TableMarginRight)
                .DefaultTextStyle(x => x.FontSize(TableFontSize))
                .Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        foreach (var tableHeader in tableHeaders)
                        {
                            columns.RelativeColumn(tableHeader.Width);
                        }
                    });

                    table.Header(header =>
                    {
                        foreach (var tableHeader in tableHeaders)
                        {
                            header.Cell().Border(0.5f).Background(Colors.Grey.Lighten2)
                            .Padding(4f)
                            .Text(tableHeader.Name).FontFamily(BoldFont).AlignCenter();
                        }
                    });

                    List<int> rows = new List<int>() { 2, 4 };
                    AddTableRows(table, rows);
                });
        }

        protected virtual void AddTableRows(TableDescriptor table, List<int> items)
        {
            foreach (int item in items)
            {
                table.Cell()
                    .Element(CellStyle)
                    .Text(item.ToString()).AlignCenter();
                table.Cell()
                    .Element(CellStyle)
                    .Text(item.ToString()).AlignCenter();
                
            }
            
        }



    }
}
