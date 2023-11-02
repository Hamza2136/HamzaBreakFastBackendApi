using ErrorOr;
using HamzaBreakfast.Contracts.Breakfast;
using HamzaBreakfast.Models;
using HamzaBreakfast.ServiceErrors;
using HamzaBreakfast.Services.Breakfast;
using Microsoft.AspNetCore.Mvc;

namespace HamzaBreakfast.Controllers;

public class BreakfastsController : ApiController
{
    private readonly IBreakfastService _breakfastservice;

    public BreakfastsController(IBreakfastService breakfastservice)
    {
        _breakfastservice = breakfastservice;
    }

    [HttpPost()]
    public IActionResult CreateBreakfast(CreateBreakfastRequest request)
    {
        ErrorOr<BreakfastModel> requestToBreakfastResult = BreakfastModel.Create(
            request.Name,
            request.Description,
            request.StartDateTime,
            request.EndDateTime,
            request.Savory,
            request.Sweet);
        if(requestToBreakfastResult.IsError){
            return Problem(requestToBreakfastResult.Errors);
        }
        var breakfast = requestToBreakfastResult.Value;

        ErrorOr<Created> CreateBreakfastResult = _breakfastservice.CreateBreakfast(breakfast);

        return CreateBreakfastResult.Match(
            Created => CreatedAtGetBreakfast(breakfast),
            errors => Problem(errors));
    }


    [HttpGet("{id:guid}")]
    public IActionResult GetBreakfast(Guid id)
    {
        ErrorOr<BreakfastModel> getBreakfastResult = _breakfastservice.GetBreakfast(id);
        return getBreakfastResult.Match(
            breakfast => Ok(MapBreakfastResponse(breakfast)),
            errors => Problem(errors)
        );
    }

    [HttpPut("{id:guid}")]
    public IActionResult UpsertBreakfastRequest(Guid id, UpsertBreakfastRequest request){
        ErrorOr<BreakfastModel> requestToBreakfastResult = BreakfastModel.Create(
            request.Name,
            request.Description,
            request.StartDateTime,
            request.EndDateTime,
            request.Savory,
            request.Sweet
        );
        if(requestToBreakfastResult.IsError){
            return Problem(requestToBreakfastResult.Errors);
        }

        var breakfast = requestToBreakfastResult.Value;
        ErrorOr<UpsertedBreakfast> UpsertBreakfastResult = _breakfastservice.UpsertBreakfast(breakfast);
        return UpsertBreakfastResult.Match(
            upserted => upserted.isNewlyCreated ? CreatedAtGetBreakfast(breakfast) : NoContent(),
            errors => Problem(errors)  
        );
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteBreakfast(Guid id){
        ErrorOr<Deleted> deleteresult = _breakfastservice.DeleteBreakfast(id);
        return deleteresult.Match(
            deleted => NoContent(),
            errors => Problem(errors)
        );
    }  
    private static BreakfastResponse MapBreakfastResponse(BreakfastModel breakfast)
    {
        return new BreakfastResponse(
                    breakfast.Id,
                    breakfast.Name,
                    breakfast.Description,
                    breakfast.StartDateTime,
                    breakfast.EndDateTime,
                    breakfast.LastModifiedDate,
                    breakfast.Savory,
                    breakfast.Sweets);
    }

    private IActionResult CreatedAtGetBreakfast(BreakfastModel breakfast)
    {
        return CreatedAtAction(
                    actionName: nameof(GetBreakfast),
                    routeValues: new { id = breakfast.Id },
                    value: MapBreakfastResponse(breakfast));
    }
}