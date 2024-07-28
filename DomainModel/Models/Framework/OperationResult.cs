namespace DomainModel.Models.Framework;

public class OperationResult
{

    public string Message { get;private set; }

    public bool Success { get;private set; }



    public OperationResult ToSuccess(string message)
    {
        this.Message = message;
        this.Success = true;
        return this;
    }


    public OperationResult ToError(string message)
    {
        Message = message;
        Success = false;
        return this;
    }
}