using Gtk;
//using Sonata.Views;
using Sonata.Data;
using System;
using Microsoft.EntityFrameworkCore;
using Sonata.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ManagedBass;
namespace Sonata.Views;
public class MainScreen
{
    public static void HomeScreen()
    {
        
        string _selectedItem = " ";
        string _windowTitle = "Sonata App";
        // creating the window
        Application.Init();
        var window = new Window(_windowTitle);
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
        var screenMusicContainer = new Box(Orientation.Vertical, 0);
        var screenMusicContainerLabel = new Label(_selectedItem);
        screenMusicContainer.PackStart(
            screenMusicContainerLabel, 
            false,
            false, 
            0
        );

        var listContainer = new Box(Orientation.Vertical, 0)
        {
            Halign = Align.Center
        };

        var controlsContainer = new Box(Orientation.Horizontal, 10)
        {
            Halign = Align.Center,
            Valign = Align.End
        };

        var playMusicButton = new Button("Play Music");
        playMusicButton.Clicked += (sender, e) =>
        {
            if(!Bass.Init())
            {
                Console.Beep();
            }
            else
            {
                using var context = new MusicContext();
                string musicPath = context.Musics
                .Where(m => m.Title.Contains(_selectedItem))
                .FirstOrDefault()
                .ToString();

                var streamHandle = Bass.CreateStream(musicPath);

                if (streamHandle == 0)
                {

                }
                else
                {
                    Bass.ChannelPlay(streamHandle);
                }
            }
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
            // Bass.StreamFree(streamHandle);
            // Bass.Free();
            // base.OnDestroyed();
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

        screenMusicContainer.PackEnd(
            controlsContainer, 
            false, 
            false, 
            20
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
        treeView.CursorChanged += (sender, e) =>
        {
            TreeView? treeView = sender as TreeView;
            TreeSelection selection = treeView.Selection;

            if (selection.GetSelected(out TreeIter iter))
            {
                _selectedItem = (string)treeView.Model.GetValue(iter, 0);
                window.Title = (string)treeView.Model.GetValue(iter, 0);
            }
        };
        listContainer.PackStart(treeView, true, true, 0);

        screenMusicContainer.PackEnd(
            listContainer, 
            false, 
            false, 
            0
        );
        stack.AddTitled(
            screenMusicContainer, "screenmusiclist", "Music Screen"
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