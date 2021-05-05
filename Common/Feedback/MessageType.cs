using System.ComponentModel.DataAnnotations;

namespace Common.Feedback
{
    public enum MessageType
    {
        [Display(Name = "Info")]
        Info = 0,
        /// <summary>
        /// logical errors like not found data or password is not match
        /// </summary>
        [Display(Name = "LogicalError")]
        LogicalError = 1,
        /// <summary>
        /// RunTimeError = unhandle Exception or expception about I/O or ....
        /// </summary>
        [Display(Name = "RunTimeError")]
        RunTimeError = 2,
    }
}