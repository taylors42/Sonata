using ManagedBass;
using System.Threading;

namespace Sonata.Controllers;

public class Player
{
    private int stream;
    // private bool repeat;

    public void Play(string path)
    {
        Bass.Init();
        stream = Bass.CreateStream(path, 0);
        Bass.ChannelPlay(stream);
    }

    public void Pause()
    {
        Bass.ChannelPause(stream);
    }

}