using System;

[Serializable]
public class TunerNotAvailableException : Exception 
{

    public TunerNotAvailableException()
    {

    }

    public TunerNotAvailableException(string message) : base(message)
    {

    }

    public TunerNotAvailableException(string message, Exception inner) : base(message, inner)
    {

    }
}