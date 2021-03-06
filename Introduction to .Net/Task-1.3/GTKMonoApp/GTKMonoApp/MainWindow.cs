﻿using System;
using Gtk;
using NetStandardClassLibrary;

public partial class MainWindow : Gtk.Window
{
    public MainWindow() : base(Gtk.WindowType.Toplevel)
    {
        Build();
    }

    protected void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        Application.Quit();
        a.RetVal = true;
    }

    protected void OnWriteHelloButtonPressed(object sender, EventArgs e)
    {
        var dialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok, ClassWriter.WriteHello(this.textBoxForName.Text));

        dialog.Run();
        dialog.Destroy();
    }
}
