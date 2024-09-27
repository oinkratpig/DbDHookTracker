using Godot;
using System;
using System.Collections.Generic;
using GDCollections = Godot.Collections;

namespace DbDHookTracker;
//
/// <summary>
/// Controls the program.
/// </summary>
public partial class Controller : Node
{
    [Export] private HookStages[] _hookStages = new HookStages[4];

    private GlobalInputCSharp _globalInput;


    // Constructor
    public Controller()
    {


    } // end constructor

    public override void _Ready()
    {
        _globalInput = GetNode<GlobalInputCSharp>("/root/GlobalInput/GlobalInputCSharp");

    } // end _Ready

    public override void _Process(double delta)
    {
        // 1-4 keys cycle the player's hook stages
        for(int i = 0; i < 4; i++)
            if (_globalInput.IsActionJustPressed($"CyclePlayer{i+1}"))
                _hookStages[i].NextStage();

        // Reset stages
        if (_globalInput.IsActionJustPressed("Reset"))
            foreach (HookStages hookStages in _hookStages)
                hookStages.Reset();

    } // end _Process

} // end class Controller