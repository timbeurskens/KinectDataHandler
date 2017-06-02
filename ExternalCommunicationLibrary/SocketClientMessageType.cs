namespace ExternalCommunicationLibrary
{
    public enum SocketClientMessageType
    {
        Ping, //ping message
        Body, //raw body data
        SessionStarted,
        SessionStopped,
        ExerciseProgress, //progress value (float 0-1) of current exercise
        ExerciseStatus, //status message (success, failed)
        ExerciseMessage, //message to be displayed to patient (hints)
        StartBreak, //break started
        BreakProgress, //progress value of break (float 0-1)
        EndBreak, //break end
    }
}