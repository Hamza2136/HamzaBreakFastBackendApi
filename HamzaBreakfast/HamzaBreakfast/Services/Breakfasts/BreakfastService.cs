using ErrorOr;
using HamzaBreakfast.Models;
using HamzaBreakfast.ServiceErrors;

namespace HamzaBreakfast.Services.Breakfast;
public class BreakfastService : IBreakfastService
{
    private static readonly Dictionary<Guid, BreakfastModel> _breakfast = new();
    public ErrorOr<Created> CreateBreakfast(BreakfastModel breakfast)
    {
        _breakfast.Add(breakfast.Id, breakfast);
        return Result.Created;
    }

    public ErrorOr<Deleted> DeleteBreakfast(Guid id)
    {
        _breakfast.Remove(id);
        return Result.Deleted;
    }

    public ErrorOr<BreakfastModel> GetBreakfast(Guid id)
    {
        if(_breakfast.TryGetValue(id, out var breakfast)){
            return breakfast;
        }
        else{
            return Errors.Breakfast.NotFound;
        }
    }

    public ErrorOr<UpsertedBreakfast> UpsertBreakfast(BreakfastModel breakfast)
    {
        var isNewlyCreated = !_breakfast.ContainsKey(breakfast.Id); 
        _breakfast[breakfast.Id] = breakfast;
        return new UpsertedBreakfast(isNewlyCreated);
    }
}