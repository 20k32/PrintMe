using PrintMe.Server.Persistence.Entities;

namespace PrintMe.Server.Models.DTOs.PrinterDto
{
    public sealed class PrinterDto : SimplePrinterDto
    {
        public string Description { get; set; }

        public double MinModelHeight { get; set; }

        public double MinModelWidth { get; set; }

        public double MaxModelHeight { get; set; }

        public double MaxModelWidth { get; set; }

        public double LocationX { get; set; }

        public double LocationY { get; set; }

        public int UserId { get; set; }
    }
}