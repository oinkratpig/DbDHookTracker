using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GDCollections = Godot.Collections;

namespace DbDHookTracker;
//
/// <summary>
/// Controls the displayed hooks stages of a player.
/// </summary>
public partial class HookStages : Node
{
    [Export(PropertyHint.Range, "0,1.0")] private float _alpha = 0.66f;
    [Export] private float _tunnelTimerMaxSeconds = 60f;
    [Export] private Sprite2D _firstStage;
    [Export] private Sprite2D _secondStage;
    [Export] private Sprite2D _sacrificed;
    [Export] private TextureProgressBar _tunnelTimerProgress;

    private Color _inactiveColor;
    private Color _activeColor;
    private Color _tunnelTimerColor;
    private int _stage = -1;
    private float _tunnelTime;
    private bool _nextCycleIsTunnelTimer = false;

    public static int MinStage => 0;
    public static int MaxStage => 3;

    public float TunnelTime
    {
        get => _tunnelTime;
        private set
        {
            _tunnelTime = value;
            _tunnelTimerProgress.Value = _tunnelTime / _tunnelTimerMaxSeconds * _tunnelTimerProgress.MaxValue;
        }
    }

    public int Stage
    {
        get => _stage;
        private set
        {
            if (_stage == value) return;

            _stage = value;
            while (_stage > MaxStage) _stage -= MaxStage + 1;

            // Unhooked
            if(_stage == 0)
            {
                _firstStage.Modulate = _inactiveColor;
                _secondStage.Modulate = _inactiveColor;
                _sacrificed.Visible = false;
                TunnelTime = 0;
            }
            // First hook
            else if(_stage == 1)
            {
                _firstStage.Modulate = _activeColor;
                _secondStage.Modulate = _inactiveColor;
                _sacrificed.Visible = false;
            }
            // Second hook
            else if (_stage == 2)
            {
                _firstStage.Modulate = _activeColor;
                _secondStage.Modulate = _activeColor;
                _sacrificed.Visible = false;
            }
            // Sacrificed
            else if (_stage == 3)
            {
                _firstStage.Modulate = _activeColor;
                _secondStage.Modulate = _activeColor;
                _sacrificed.Visible = true;
                TunnelTime = 0;
            }
        }
    }

    // Constructor
    public HookStages()
    {
        _inactiveColor = Colors.White;
        _inactiveColor.A = _alpha;
        _activeColor = Colors.Red;
        _activeColor.A = _alpha;
        _tunnelTimerColor = Colors.White;
        _tunnelTimerColor.A = _alpha;

    } // end constructor

    public override void _Ready()
    {
        _sacrificed.Modulate = _activeColor;
        _tunnelTimerProgress.Modulate = _tunnelTimerColor;
        Reset();

    } // end _Ready

    public override void _Process(double delta)
    {
        TunnelTime = (float)Mathf.Max(TunnelTime - delta, 0f);

    } // end _Process

    /// <summary>
    /// Reset hook stage.
    /// </summary>
    public void Reset()
    {
        _tunnelTime = -1;
        Stage = 0;
        _nextCycleIsTunnelTimer = false;

    } // end Reset

    /// <summary>
    /// Next hook stage.
    /// </summary>
    public void NextStage()
    {
        // Tunnel timer
        if(_nextCycleIsTunnelTimer)
        {
            _nextCycleIsTunnelTimer = false;
            TunnelTime = _tunnelTimerMaxSeconds;
        }
        // Hooked
        else
        {
            Stage++;
            if (Stage == 1 || Stage == 2)
                _nextCycleIsTunnelTimer = true;
        }

    } // end NextStage

} // end class HookStages