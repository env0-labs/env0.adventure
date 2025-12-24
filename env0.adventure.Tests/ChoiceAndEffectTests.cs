using System;
using System.Collections.Generic;
using env0.adventure.Engine;
using env0.adventure.Model;
using env0.adventure.Runtime;
using Xunit;

namespace env0.adventure.Tests;

public class ChoiceAndEffectTests
{
    private readonly ChoiceEvaluator _evaluator = new();
    private readonly EffectExecutor _executor = new();

    [Fact]
    public void Choice_disabled_when_required_flag_missing()
    {
        var state = new GameState("start");
        var choice = new ChoiceDefinition
        {
            Number = 1,
            Text = "Open door",
            RequiresAll = ["hasKey"],
            DisabledReason = "You need the key first.",
            Effects = [new EffectDefinition { Type = EffectType.GotoScene, Value = "next" }]
        };

        var enabled = _evaluator.IsEnabled(choice, state, out var reason);

        Assert.False(enabled);
        Assert.Equal("You need the key first.", reason);
    }

    [Fact]
    public void Choice_enabled_when_requirements_met()
    {
        var state = new GameState("start");
        state.SetFlag("hasKey");

        var choice = new ChoiceDefinition
        {
            Number = 1,
            Text = "Open door",
            RequiresAll = ["hasKey"],
            Effects = [new EffectDefinition { Type = EffectType.GotoScene, Value = "next" }]
        };

        var enabled = _evaluator.IsEnabled(choice, state, out var reason);

        Assert.True(enabled);
        Assert.Null(reason);
    }

    [Fact]
    public void Choice_disabled_when_requires_none_flag_present()
    {
        var state = new GameState("start");
        state.SetFlag("alarmTripped");

        var choice = new ChoiceDefinition
        {
            Number = 1,
            Text = "Enter",
            RequiresNone = ["alarmTripped"],
            DisabledReason = "The alarm is still on.",
            Effects = [new EffectDefinition { Type = EffectType.GotoScene, Value = "next" }]
        };

        var enabled = _evaluator.IsEnabled(choice, state, out var reason);

        Assert.False(enabled);
        Assert.Equal("The alarm is still on.", reason);
    }

    [Fact]
    public void Choice_disabled_when_both_requirements_fail()
    {
        var state = new GameState("start");
        state.SetFlag("blocked");

        var choice = new ChoiceDefinition
        {
            Number = 1,
            Text = "Proceed",
            RequiresAll = ["hasKey"],
            RequiresNone = ["blocked"],
            Effects = [new EffectDefinition { Type = EffectType.GotoScene, Value = "next" }]
        };

        var enabled = _evaluator.IsEnabled(choice, state, out var reason);

        Assert.False(enabled);
        Assert.Null(reason);
    }

    [Fact]
    public void Effect_executor_updates_state_and_scene()
    {
        var state = new GameState("start");
        var effects = new List<EffectDefinition>
        {
            new() { Type = EffectType.SetFlag, Value = "foundKey" },
            new() { Type = EffectType.GotoScene, Value = "hallway" }
        };

        _executor.Execute(effects, state);

        Assert.True(state.HasFlag("foundKey"));
        Assert.Equal("hallway", state.CurrentSceneId);
    }

    [Fact]
    public void Effect_executor_throws_on_unknown_type()
    {
        var state = new GameState("start");
        var effects = new List<EffectDefinition>
        {
            new() { Type = (EffectType)999, Value = "ignored" }
        };

        Assert.Throws<InvalidOperationException>(() => _executor.Execute(effects, state));
    }

    [Fact]
    public void Effect_executor_requires_value_for_scene_change()
    {
        var state = new GameState("start");
        var effects = new List<EffectDefinition>
        {
            new() { Type = EffectType.GotoScene, Value = " " }
        };

        Assert.Throws<InvalidOperationException>(() => _executor.Execute(effects, state));
    }

    [Fact]
    public void Effect_executor_requires_value_for_flags()
    {
        var state = new GameState("start");
        var effects = new List<EffectDefinition>
        {
            new() { Type = EffectType.SetFlag, Value = " " }
        };

        Assert.Throws<InvalidOperationException>(() => _executor.Execute(effects, state));
    }

    [Fact]
    public void Clear_flag_sets_flag_to_false()
    {
        var state = new GameState("start");
        state.SetFlag("doorOpen");

        var effects = new List<EffectDefinition>
        {
            new() { Type = EffectType.ClearFlag, Value = "doorOpen" }
        };

        _executor.Execute(effects, state);

        Assert.False(state.HasFlag("doorOpen"));
    }
}
