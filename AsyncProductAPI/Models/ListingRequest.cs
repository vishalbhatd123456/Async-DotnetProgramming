
namespace AsyncProductAPI.Models
{
    public class ListingRequest
    {
        public int Id{get;set;}

        //this is an inbound path
        //[ToDo] - Vishal - Create a dto for this inbound path
        public string? RequestBody{get;set;}
        //pass this to the client after the request completed
        //[ToDo] - Vishal - Create a dto for this outbound path
        public string? EstimatedCompletionTime{get;set;}

        public string? RequestStatus{get;set;}
        
        // Check if this can be equal to the Id (primary key of the dB)
        public string? RequestId{get;set;} = Guid.NewGuid().ToString();
    }
}