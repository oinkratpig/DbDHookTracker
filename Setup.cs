using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using GDCollections = Godot.Collections;

// Ty https://github.com/KitzuGG/Godot-Clickthrough <3

namespace DbDHookTracker;
//
/// <summary>
/// Sets up the window.
/// </summary>
public partial class Setup : Node
{
    [DllImport("user32.dll")]
    public static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

    private const int GwlExStyle = -20;

    private const uint WsExLayered = 0x00080000;
    private const uint WsExTransparent = 0x00000020;
    private IntPtr _hWnd;


    public override void _Ready()
    {
        GetTree().Root.TransparentBg = true;
        GetWindow().MousePassthrough = true;

        _hWnd = GetActiveWindow();
        SetWindowLong(_hWnd, GwlExStyle, WsExLayered);
        SetClickThrough(true);

    } // end _Ready

    /// <summary>
    /// Sets the property of being clickable or not
    /// </summary>
    public void SetClickThrough(bool clickthrough)
    {
        if (clickthrough)
            SetWindowLong(_hWnd, GwlExStyle, WsExLayered | WsExTransparent);
        else
            SetWindowLong(_hWnd, GwlExStyle, WsExLayered);

    } // end SetClickThrough

} // end class Setup