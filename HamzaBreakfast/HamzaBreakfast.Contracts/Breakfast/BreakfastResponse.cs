namespace HamzaBreakfast.Contracts.Breakfast;
    public record BreakfastResponse(
        Guid id,
        string Name,
        string Description,
        DateTime StartDateTime,
        DateTime EndDateTime,
        DateTime LastModifiedDate,
        List<string> Savory,
        List<string> Sweet
    );