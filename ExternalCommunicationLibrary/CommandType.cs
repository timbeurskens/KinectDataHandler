namespace ExternalCommunicationLibrary
{
    public enum CommandType
    {
        ExerciseSelect, //select type of exercise session
        ExerciseStart, //start selected exercise session
        ExerciseStop, //stop current exercise session
        ExerciseTimeLimit, //set time limit of exercise session (seconds)
        ExerciseSessionLength, //set number of exercises per session
        ExerciseSeriesLength, //set number of continuous exercises
        ExerciseBreakLength, //set duration of break after n continuous exercises (seconds)

        //status return messages
        SessionStarted,
        SessionStopped,
        ExerciseProgress, //progress value (float 0-1) of current exercise
        ExerciseStatus, //status message (success, failed)
        ExerciseMessage, //message to be displayed to patient (hints)
        StartBreak, //break started
        BreakProgress, //progress value of break (float 0-1)
        EndBreak, //break end

        Null
    }
}