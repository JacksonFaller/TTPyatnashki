using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http.ModelBinding;

namespace Pyatnashki.API
{
    /// <summary>
    /// 
    /// </summary>
    public static class ModelStateExtension
    {
        /// <summary>
        /// GetErrors model
        /// </summary>
        /// <param name="modelState">model to validate</param>
        /// <returns>new string with erros if there are, otherwise null.</returns>
        public static string GetErrors(this ModelStateDictionary modelState)
        {
            StringBuilder stringBuilder = new StringBuilder("Not all required fields has been set:");

            foreach (var value in modelState.Values)
            {
                foreach (var error in value.Errors)
                {
                    stringBuilder.Append(error.ErrorMessage);
                }
            }
            return stringBuilder.ToString();
        }
    }
}