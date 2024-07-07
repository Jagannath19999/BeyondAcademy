﻿using System.ComponentModel.DataAnnotations;

namespace BeyondAcademy.Custom
{
    public class PastDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is DateTime dateTime)
            {
                return dateTime < DateTime.Now;
            }
            return false;
        }
    }
}