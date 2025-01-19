namespace Booking.Domain.UsefulModels;

public class SalesReport
{
    public Dictionary<DateTime,SalesReportRow> SalesReportDictionary { get; set; } = new Dictionary<DateTime,SalesReportRow>();
    public Decimal TotalAmountSum { get; set; }
    public int TotalNumberOfSoldTickets { get; set; }
}
public class SalesReportRow
{   
    public Decimal DailyAmountsSum { get; set; }
    public int DailyNumberOfSoldTickets { get; set; }
}