using System.ComponentModel.DataAnnotations;
namespace  NewsAppClasses.Validators
{
    public class PubDateValidator: ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if(value == null)
                return false;

            DateTime PublicationDate = (DateTime) value; 
            
            if ((DateTime.Now-PublicationDate).TotalDays >= 7)
            {
                return false;
            }
            
            return true;
        }
    }
}
