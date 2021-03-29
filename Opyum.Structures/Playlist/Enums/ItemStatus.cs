namespace Opyum.Structures.Playlist
{
    public enum ItemStatus
    {
        None = 0,
        Empty = 0,
        InUse = 1,
        Playing = 2,
        PlayingAndInUse = 3,
        Stopped = 4,
        StoppedAndInUse = 5,
        Paused = 8,
        PausedAndInUse = 9,
        PrePlay = 16,
        PrePlayAndInUse = 17,
        Updating = 32,
        UpdatingAndInUse = 33,
        Loading = 64,
        LoadingAndInUse = 65,
        Waiting = 128,
        WaitingAndInUse = 129,
        Done = 256,
        UnsuccessfulLoading = 512,
    }
}
