namespace ExternalCommunicationLibrary
{
    public enum SocketServerMessageType
    {
        ExerciseSelect, //select type of exercise session
        ExerciseStart, //start selected exercise session
        ExerciseStop, //stop current exercise session
        ExerciseTimeLimit, //set time limit of exercise session (seconds)
        ExerciseSessionLength, //set number of exercises per session
        ExerciseSeriesLength, //set number of continuous exercises
        ExerciseBreakLength, //set duration of break after n continuous exercises (seconds)
    }
}