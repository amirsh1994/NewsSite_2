namespace Framework.Implementation;

public static class ValidateFile
{
    public static bool CheckFileName(this string?  fileName)
    {
        if (fileName == null)
            return false;
        fileName = fileName.ToLower().Trim();

        return !fileName.Contains(".php") && !fileName.Contains(".py") && !fileName.Contains(".bat") && !fileName.Contains(".cpp");
    }

    public static string ToUniqueFileName(this string fileName)
    {
        return Guid.NewGuid().ToString().Replace("-", "_") + fileName;
    }
}