using AsyncProductAPI.Data;
using AsyncProductAPI.Models;
using AsyncProductAPI.Dtos;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlite("Data Source=RequestDB.db"));

var app = builder.Build();


app.UseHttpsRedirection();

//Start endpoint

app.MapPost("api/v1/products", async(AppDbContext context, ListingRequest listingRequest) => {
    //check for validations
    if(listingRequest == null){
        return Results.BadRequest();
    }
    listingRequest.RequestStatus = "Acknowledged";
    listingRequest.EstimatedCompletionTime = "2023-02-11 9:45PM IST";

    await context.ListingRequest.AddAsync(listingRequest);
    await context.SaveChangesAsync();

    return Results.Accepted($"api/v1/productstatus/{listingRequest.RequestId}", listingRequest);
});

//status endpoint

app.MapGet("api/v1/productstatus/{requestId}",(AppDbContext context, string requestId ) => {
    var listingRequest = context.ListingRequest.FirstOrDefault(lr => lr.RequestId == requestId);

    if(listingRequest == null){
        return Results.NotFound();
    }
    ListingStatus listingStatus = new ListingStatus{
        RequestStatus = listingRequest.RequestStatus,
        ResourceURL = string.Empty
    };

    if(listingRequest.RequestStatus!.ToUpper() == "COMPLETE"){
        listingStatus.ResourceURL = $"api/v1/products/{Guid.NewGuid().ToString()}";
        return Results.Ok(listingStatus);
    }

    listingStatus.EstimatedCompletionTime = "2023-02-11 10:45PM IST";
    return Results.Ok(listingStatus);
});

//Final Endpoint

app.MapGet("api/v1/products/{requestId}", (string requestId) => {
    return Results.Ok("This is where you would pass back the final result!");
});


app.Run();