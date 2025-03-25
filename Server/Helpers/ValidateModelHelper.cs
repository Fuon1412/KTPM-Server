using System.ComponentModel.DataAnnotations;

namespace Server.Helpers
{
    public class ValidateModelHelper
    {
        public static void ValidateModel<T>(T model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model), "Model cannot be null.");
            }

            var context = new ValidationContext(model, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(model, context, results, true);

            if (!isValid)
            {
                var error = results.First();
                throw new Exception(error.ErrorMessage);
            }
        }
    }
}