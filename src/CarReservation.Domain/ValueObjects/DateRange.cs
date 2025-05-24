namespace CarReservation.Domain.ValueObjects;

using CarReservation.Domain.Common;

public class DateRange : ValueObject
{
    public DateTime StartDate { get; }
    public DateTime EndDate { get; }

    public DateRange(DateTime startDate, DateTime endDate)
    {
        if (startDate > endDate)
            throw new ArgumentException("Start date must be before or equal to end date");

        if (startDate < DateTime.Today)
            throw new ArgumentException("Start date cannot be in the past");

        StartDate = startDate.Date;
        EndDate = endDate.Date;
    }

    // Static factory methods for better readability
    public static DateRange Create(DateTime startDate, DateTime endDate)
    {
        return new DateRange(startDate, endDate);
    }

    public static DateRange CreateForToday(int durationDays)
    {
        var start = DateTime.Today;
        var end = start.AddDays(durationDays - 1);
        return new DateRange(start, end);
    }

    public static DateRange CreateFromTomorrow(int durationDays)
    {
        var start = DateTime.Today.AddDays(1);
        var end = start.AddDays(durationDays - 1);
        return new DateRange(start, end);
    }

    public int Days => (EndDate - StartDate).Days + 1;

    public bool OverlapsWith(DateRange other)
    {
        if (other == null) return false;
        return StartDate <= other.EndDate && EndDate >= other.StartDate;
    }

    public bool Contains(DateTime date)
    {
        return date.Date >= StartDate && date.Date <= EndDate;
    }

    public bool IsInFuture()
    {
        return StartDate > DateTime.Today;
    }

    public bool IsActive()
    {
        var today = DateTime.Today;
        return StartDate <= today && EndDate >= today;
    }

    public bool HasPassed()
    {
        return EndDate < DateTime.Today;
    }

    public DateRange ExtendBy(int days)
    {
        if (days < 0)
            throw new ArgumentException("Cannot extend by negative days");
        
        return new DateRange(StartDate, EndDate.AddDays(days));
    }

    public DateRange ShortenBy(int days)
    {
        if (days < 0)
            throw new ArgumentException("Cannot shorten by negative days");
        
        var newEndDate = EndDate.AddDays(-days);
        if (newEndDate < StartDate)
            throw new ArgumentException("Cannot shorten range to less than one day");
        
        return new DateRange(StartDate, newEndDate);
    }

    public IEnumerable<DateTime> GetDatesInRange()
    {
        for (var date = StartDate; date <= EndDate; date = date.AddDays(1))
        {
            yield return date;
        }
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return StartDate;
        yield return EndDate;
    }

    public override string ToString() => $"{StartDate:yyyy-MM-dd} to {EndDate:yyyy-MM-dd} ({Days} days)";
}
