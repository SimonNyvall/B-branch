namespace Tests.Extensions;

public class Assert
{
    public static void DoesNotThrow(Action action)
    {
        try
        {
            action();
        }
        catch (Exception e)
        {
            throw new Exception($"Expected no exception, but got: {e.Message}");
        }
    }

    
}


