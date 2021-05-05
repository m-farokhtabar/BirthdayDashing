using System.ComponentModel.DataAnnotations;

namespace Common.Feedback
{
    public enum FeedbackType
    {
        [Display(Name = "Server failure")]
        CouldNotConnectToServer,
        [Display(Name = "Db failure")]
        CouldNotConnectToDataBase,
        [Display(Name = "The operation was successful")]
        DoneSuccessfully,
        [Display(Name = "{0} can not find")]
        DataIsNotFound,
        [Display(Name = "{0}" + " " + "مورد نظر معتبر نمی باشد")]
        DataIsNotValid,
        [Display(Name = "This {0} is already exist" )]
        ValueIsRepititive,
        [Display(Name = "Data is Invalid" + "{0}")]
        InvalidDataFormat,
        OperationFailed = 100
    }
}