
using Microsoft.AspNetCore.Http;

namespace NewsAppClasses.Dtos
{ 
    public record NewsWriteDto(string Title, string NewsArticle,byte[]? Image, DateTime PublicationDate, int AuthorID);
    public record NewsUpdateDto(int Id,string Title, string NewsArticle, byte[]? Image, DateTime PublicationDate, int AuthorID);

}
