using Gtk;
//using Sonata.Views;
using Sonata.Data;
using System;
using Microsoft.EntityFrameworkCore;
using Sonata.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
namespace Sonata.Views;
public class MainScreen
{
    public static void HomeScreen()
    {
        
        // creating the window
        Application.Init();
        var window = new Window("Sonata App");
        window.SetDefaultSize(250, 400);
        window.BorderWidth = 10;
        window.DeleteEvent += delegate { Application.Quit(); };

        // creating the stack
        var stack = new Stack();
        var stackSwitcher = new StackSwitcher
        {
            Stack = stack,
            Halign = Align.Center
        };
        // ! start of homescreen component
        var screenHome = new Box(Orientation.Vertical, 0);

        var addMusicButton = new Button("Add Music");
        addMusicButton.Clicked += (sender, e) => 
        {
            Console.Beep();
            var fileChooser = new FileChooserDialog(
                "Select a music file",
                null,
                FileChooserAction.Open,
                "Cancelar", ResponseType.Cancel,
                "Abrir", ResponseType.Accept
            );
    
            if (fileChooser.Run() == (int)ResponseType.Accept)
            {
                Console.Beep();
                using var context = new MusicContext();
                var music = new Music
                {
                    Title = System.IO.Path.GetFileNameWithoutExtension(fileChooser.Filename),
                    FilePath = fileChooser.Filename
                };
                context.Musics.Add(music);
                context.SaveChanges();
            }
            fileChooser.Destroy();
        };

        screenHome.PackStart(
            addMusicButton, 
            false, 
            false, 
            0
        );

        stack.AddTitled(
            screenHome, "screenhome", "Home Screen"
        );

        // ! start of music list component
        var screenMusicList = new Box(Orientation.Vertical, 0);
        screenMusicList.PackStart(
            new Label("you are in the music list stack"), 
            false, 
            false, 
            0
        );

        var controlsContainer = new Box(Orientation.Horizontal, 10)
        {
            Halign = Align.Center
        };

        var playMusicButton = new Button("Play Music");
        playMusicButton.Clicked += (sender, e) =>
        {
            Console.Beep();
        };

        controlsContainer.PackStart(
            playMusicButton,
            false,
            false,
            0
        );


        var pauseMusicButton = new Button("Pause Music");
        pauseMusicButton.Clicked += (sender, e) =>
        {
            Console.Beep();
        };

        controlsContainer.PackStart(
            pauseMusicButton,
            false,
            false,
            0
        );

        var deleteMusicButton = new Button("Delete Music");

        deleteMusicButton.Clicked += (sender, e) =>
        {
            Console.Beep();
        };

        controlsContainer.PackStart(
            deleteMusicButton, 
            false, 
            false, 
            0
        );

        screenMusicList.PackEnd(
            controlsContainer, 
            false, 
            false, 
            0
        );

        var musicList = new ListStore(typeof(string));
        using (var context = new MusicContext())
        {
            foreach (var music in context.Musics)
            {
                musicList.AppendValues(music.Title);
            }
        }

        var treeView = new TreeView(musicList);
        var column = new TreeViewColumn { Title = "Musics" };
        var cell = new CellRendererText();
        column.PackStart(cell, true);
        column.AddAttribute(cell, "text", 0);
        treeView.AppendColumn(column);
        screenMusicList.PackStart(treeView, true, true, 0);

        stack.AddTitled(
            screenMusicList, "screenmusiclist", "Music Screen"
        );
        // ! end of music list component

        var rootContainer = new Box(Orientation.Vertical, 0);
        rootContainer.PackStart(stack, false, false, 0);
        rootContainer.PackEnd(stackSwitcher, false, false, 0);

        // adding the root container to the window
        window.Add(rootContainer);
        window.ShowAll();

        Application.Run();
    }
}