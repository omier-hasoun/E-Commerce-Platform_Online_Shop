
namespace Application;

// for copy paste

/*
 public static readonly Error rename1 = Error.Validation($"{BaseErrorCode}.{nameof(rename1)}");

 public static readonly Error rename2 = Error.Validation($"{BaseErrorCode}.{nameof(rename2)}");

 public static readonly Error rename3 = Error.Validation($"{BaseErrorCode}.{nameof(rename3)}");

 public static readonly Error rename4 = Error.Validation($"{BaseErrorCode}.{nameof(rename4)}");

 public static readonly Error rename5 = Error.Validation($"{BaseErrorCode}.{nameof(rename5)}");

 public static readonly Error rename6 = Error.Validation($"{BaseErrorCode}.{nameof(rename6)}");

 public static readonly Error rename7 = Error.Validation($"{BaseErrorCode}.{nameof(rename7)}");

 public static readonly Error rename8 = Error.Validation($"{BaseErrorCode}.{nameof(rename8)}");

 public static readonly Error rename9 = Error.Validation($"{BaseErrorCode}.{nameof(rename9)}");

--
        public static readonly Error rename1 = Error.Validation(GenerateErrorCode(nameof(rename1)));

        public static readonly Error rename2 = Error.Validation(GenerateErrorCode(nameof(rename2)));

        public static readonly Error rename3 = Error.Validation(GenerateErrorCode(nameof(rename3)));

        public static readonly Error rename4 = Error.Validation(GenerateErrorCode(nameof(rename4)));

        public static readonly Error rename5 = Error.Validation(GenerateErrorCode(nameof(rename5)));

        public static readonly Error rename6 = Error.Validation(GenerateErrorCode(nameof(rename6)));

        public static readonly Error rename7 = Error.Validation(GenerateErrorCode(nameof(rename7)));

        public static readonly Error rename8 = Error.Validation(GenerateErrorCode(nameof(rename8)));
*/
public static class ApplicationErrors
{
    public static readonly Error OperationWasCanceled = Error.Validation($"{nameof(ApplicationErrors)}.{nameof(OperationWasCanceled)}");

    public static readonly Error DeleteOperationFailed = Error.Validation($"{nameof(ApplicationErrors)}.{nameof(DeleteOperationFailed)}");

    public static readonly Error rename4 = Error.Validation($"{nameof(ApplicationErrors)}.{nameof(rename4)}");
    public static class Validation
    {
        public const string BaseErrorCode = $"ApplicationErrors.Validation";
        public static string GenerateErrorCode(string error)
        {
            return $"{BaseErrorCode}.{error}";
        }

        public static readonly Error MissingInput = Error.Validation("Application.Validation.MissingInput");

        public static readonly Error PageSizeTooBig = Error.Validation("Application.Validation.PageSizeTooBig");

        public static readonly Error ProductStatusInvalid = Error.Validation(GenerateErrorCode(nameof(ProductStatusInvalid)));

        public static readonly Error InvalidEmail = Error.Validation($"{BaseErrorCode}.{nameof(InvalidEmail)}");

        public static readonly Error CartIsEmpty = Error.Validation($"{BaseErrorCode}.{nameof(CartIsEmpty)}");

        public static readonly Error RoleInvalid = Error.Validation($"{BaseErrorCode}.{nameof(RoleInvalid)}");

        public static readonly Error SearchTermRequired = Error.Validation($"{BaseErrorCode}.{nameof(SearchTermRequired)}");

        public static readonly Error YouCannotHaveMoreThanOneWarehouse = Error.Validation($"{BaseErrorCode}.{nameof(YouCannotHaveMoreThanOneWarehouse)}");

        public static readonly Error StatusInvalid = Error.Validation($"{BaseErrorCode}.{nameof(StatusInvalid)}");

        public static readonly Error rename7 = Error.Validation($"{BaseErrorCode}.{nameof(rename7)}");

        public static readonly Error rename8 = Error.Validation($"{BaseErrorCode}.{nameof(rename8)}");

        public static readonly Error rename9 = Error.Validation($"{BaseErrorCode}.{nameof(rename9)}");


    }

    public static class Authentication
    {
        public const string BaseErrorCode = $"ApplicationErrors.Auth";
        public static string GenerateErrorCode(string error)
        {
            return $"{BaseErrorCode}.{error}";
        }

        public static readonly Error Unauthorized = Error.Unauthorized("Application.Auth.Unauthorized", "You do not have permission to perform this action.");
        public static readonly Error Unauthenticated = Error.Unauthorized("Application.Auth.Unauthenticated", "You need to log in.");
        public static readonly Error ConfirmYourEmail = Error.ActionRequired("Application.ActionRequired.ConfirmYourEmail");
        public static readonly Error ChangeYourPassword = Error.ActionRequired("Application.ActionRequired.ChangeYourPassword");

        public static readonly Error rename1 = Error.Unauthorized(GenerateErrorCode(nameof(rename1)));
        public static readonly Error AccessDenied = Error.Unauthorized(GenerateErrorCode(nameof(AccessDenied)));
        public static readonly Error rename3 = Error.Unauthorized(GenerateErrorCode(nameof(rename3)));

    }

    public static class NotFound
    {
        public const string BaseErrorCode = $"ApplicationErrors";
        public static string GenerateErrorCode(string error)
        {
            return $"{BaseErrorCode}.{error}.NotFound";
        }

        public static readonly Error ProductGroup = Error.NotFound(GenerateErrorCode(nameof(ProductGroup)));

        public static readonly Error Product = Error.NotFound(GenerateErrorCode(nameof(Product)));

        public static readonly Error Order = Error.NotFound(GenerateErrorCode(nameof(Order)));

        public static readonly Error OrderItem = Error.NotFound(GenerateErrorCode(nameof(OrderItem)));

        public static readonly Error ProductReview = Error.NotFound(GenerateErrorCode(nameof(ProductReview)));

        public static readonly Error User = Error.NotFound(GenerateErrorCode(nameof(User)));

        public static readonly Error Cart = Error.NotFound(GenerateErrorCode(nameof(Cart)));

        public static readonly Error Warehouse = Error.NotFound(GenerateErrorCode(nameof(Warehouse)));

        public static readonly Error Category = Error.Validation(GenerateErrorCode(nameof(Category)));

        public static readonly Error Brand = Error.Validation(GenerateErrorCode(nameof(Brand)));

        public static readonly Error rename3 = Error.Validation(GenerateErrorCode(nameof(rename3)));

        public static readonly Error rename4 = Error.Validation(GenerateErrorCode(nameof(rename4)));

        public static readonly Error rename5 = Error.Validation(GenerateErrorCode(nameof(rename5)));

        public static readonly Error rename6 = Error.Validation(GenerateErrorCode(nameof(rename6)));

        public static readonly Error rename7 = Error.Validation(GenerateErrorCode(nameof(rename7)));

        public static readonly Error rename8 = Error.Validation(GenerateErrorCode(nameof(rename8)));
    }


    public static class Conflict
    {
        public const string BaseErrorCode = $"ApplicationErrors.Conflict";
        public static string GenerateErrorCode(string error)
        {
            return $"{BaseErrorCode}.{error}";
        }

        public static readonly Error ProductTitleMustBeUnique = Error.Conflict("Application.Conflict.ProductTitleMustBeUnique");
    }

    public static class Unexpected
    {
        public const string BaseErrorCode = $"ApplicationErrors.Unexpected";
        public static string GenerateErrorCode(string error)
        {
            return $"{BaseErrorCode}.{error}";
        }

        public static readonly Error CouldntSaveImage = Error.Unexpected(GenerateErrorCode(nameof(CouldntSaveImage)));
        public static readonly Error UnableToAddThisItem = Error.Unexpected(GenerateErrorCode(nameof(UnableToAddThisItem)));
        public static readonly Error OperationFailed = Error.Validation($"{BaseErrorCode}.{nameof(OperationFailed)}");

        public static readonly Error CheckoutFailed = Error.Unexpected($"{BaseErrorCode}.{nameof(CheckoutFailed)}");

        public static readonly Error rename3 = Error.Unexpected($"{BaseErrorCode}.{nameof(rename3)}");

        public static readonly Error rename4 = Error.Unexpected($"{BaseErrorCode}.{nameof(rename4)}");

        public static readonly Error rename5 = Error.Unexpected($"{BaseErrorCode}.{nameof(rename5)}");

        public static readonly Error rename6 = Error.Unexpected($"{BaseErrorCode}.{nameof(rename6)}");

        public static readonly Error rename7 = Error.Unexpected($"{BaseErrorCode}.{nameof(rename7)}");

        public static readonly Error rename8 = Error.Unexpected($"{BaseErrorCode}.{nameof(rename8)}");

        public static readonly Error rename9 = Error.Unexpected($"{BaseErrorCode}.{nameof(rename9)}");

    }
}
