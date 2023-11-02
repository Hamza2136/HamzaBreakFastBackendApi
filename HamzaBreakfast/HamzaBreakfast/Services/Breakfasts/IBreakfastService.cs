using ErrorOr;
using HamzaBreakfast.Models;

namespace HamzaBreakfast.Services.Breakfast;
public interface IBreakfastService{
    ErrorOr<Created> CreateBreakfast(BreakfastModel breakfast);
    ErrorOr<Deleted> DeleteBreakfast(Guid id);
    ErrorOr<BreakfastModel> GetBreakfast(Guid id);
    ErrorOr<UpsertedBreakfast> UpsertBreakfast(BreakfastModel breakfast);
}