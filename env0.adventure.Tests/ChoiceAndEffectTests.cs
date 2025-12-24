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
