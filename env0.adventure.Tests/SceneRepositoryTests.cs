using System;
using System.Collections.Generic;
using System.Linq;
using env0.adventure.Engine;
using env0.adventure.Model;
using Xunit;

namespace env0.adventure.Tests;

public class SceneRepositoryTests
{
    [Fact]
    public void Initializes_with_valid_story()
    {
        var story = BuildStory(
            startSceneId: "start",
            CreateScene("start", choiceText: "Go", gotoScene: "end"),
            CreateScene("end", isEnd: true)
        );

        var repo = new SceneRepository(story);

        Assert.Equal("start", repo.StartSceneId);
        Assert.Equal("end", repo.Get("end").Id);
    }

    [Fact]
    public void Throws_when_goto_target_missing()
    {
        var story = BuildStory(
            startSceneId: "start",
            CreateScene("start", choiceText: "Go", gotoScene: "missing")
        );

        Assert.Throws<InvalidOperationException>(() => new SceneRepository(story));
    }

    [Fact]
    public void Throws_on_duplicate_choice_numbers()
    {
        var scene = new SceneDefinition
        {
            Id = "start",
            Text = "start",
            Choices =
            [
                new ChoiceDefinition
                {
                    Number = 1,
                    Text = "One",
                    Effects = [new EffectDefinition { Type = EffectType.GotoScene, Value = "end" }]
                },
                new ChoiceDefinition
                {
                    Number = 1,
                    Text = "Duplicate",
                    Effects = [new EffectDefinition { Type = EffectType.GotoScene, Value = "end" }]
                }
            ]
        };

        var story = BuildStory("start", scene);

        Assert.Throws<InvalidOperationException>(() => new SceneRepository(story));
    }

    private static StoryDefinition BuildStory(string startSceneId, params SceneDefinition[] scenes) =>
        new()
        {
            StartSceneId = startSceneId,
            Scenes = scenes.ToList()
        };

    private static SceneDefinition CreateScene(
        string id,
        bool isEnd = false,
        string? choiceText = null,
        string? gotoScene = null
    )
    {
        var choices = new List<ChoiceDefinition>();

        if (!isEnd)
        {
            choices.Add(
                new ChoiceDefinition
                {
                    Number = 1,
                    Text = choiceText ?? "Continue",
                    Effects =
                    [
                        new EffectDefinition { Type = EffectType.GotoScene, Value = gotoScene ?? id }
                    ]
                }
            );
        }

        return new SceneDefinition
        {
            Id = id,
            Text = id,
            IsEnd = isEnd,
            Choices = choices
        };
    }
}
