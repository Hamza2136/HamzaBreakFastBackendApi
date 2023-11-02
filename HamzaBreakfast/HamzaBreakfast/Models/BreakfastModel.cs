using ErrorOr;
using HamzaBreakfast.ServiceErrors;

namespace HamzaBreakfast.Models;
public class BreakfastModel
{
    public const int MinNameLength = 3;
    public const int MaxNameLength = 30;
    public const int MinDescriptionLength = 10;
    public const int MaxDescriptionLength = 70;
    public Guid Id { get; }
    public string Name { get; }
    public string Description { get; }
    public DateTime StartDateTime { get; }
    public DateTime EndDateTime { get; }
    public DateTime LastModifiedDate { get; }
    public List<string> Savory { get; }
    public List<string> Sweets { get; }

    private BreakfastModel(
        Guid id,
        string name,
        string description,
        DateTime startdatetime,
        DateTime enddatetime,
        DateTime lastModifiedDate,
        List<string> savory,
        List<string> sweets)
    {
        Id = id;
        Name = name;
        Description = description;
        StartDateTime = startdatetime;
        EndDateTime = enddatetime;
        LastModifiedDate = lastModifiedDate;
        Savory = savory;
        Sweets = sweets;
    }

    public static ErrorOr<BreakfastModel> Create(
        string name,
        string description,
        DateTime startdatetime,
        DateTime enddatetme,
        List<string> savory,
        List<string> sweets,
        Guid? id = null)
    {
        List<Error> errors = new();
        if (name.Length is < MinNameLength or > MaxNameLength)
        {
            errors.Add(Errors.Breakfast.InvalidName);
        }

        if (description.Length is < MinDescriptionLength or > MaxDescriptionLength)
        {
            errors.Add(Errors.Breakfast.InvalidDescription);
        }
        if(errors.Count > 0){
            return errors;
        }
        return new BreakfastModel(
            id?? Guid.NewGuid(),
            name,
            description,
            startdatetime,
            enddatetme,
            DateTime.UtcNow,
            savory,
            sweets
        );
    }
}