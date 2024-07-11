using System.ComponentModel.DataAnnotations;

namespace BeyondAcademy.Custom
{
    public class PastDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime dob = DateTime.Parse(value.ToString());
            if (dob is DateTime dateTime)
            {
                return dob < DateTime.Now;
            }
            return false;
        }
    }
}
