using env0.adventure.Engine;
using env0.adventure.Model;
using env0.adventure.Runtime;

Console.WriteLine("env0.adventure booting");

// ------------------------------------------------------------------
// Temporary hardcoded scenes (JSON comes later)
// ------------------------------------------------------------------
var scenes = new List<SceneDefinition>
{
    new()
    {
        Id = "hallway",
        Text = "You are standing in a narrow hallway.",
        IsEnd = false,
        Choices = new()
        {
            new ChoiceDefinition
            {
                Number = 1,
                Text = "Go into the kitchen",
                RequiresAll = new() { "doorUnlocked" },
                DisabledReason = "The door is locked.",
                Effects = new()
                {
                    new EffectDefinition
                    {
                        Type = EffectType.GotoScene,
                        Value = "kitchen"
                    }
                }
            }
        }
    },
    new()
    {
        Id = "kitchen",
        Text = "You are in a small kitchen. There is a cupboard.",
        IsEnd = true,
        Choices = new()
    }
};

// ------------------------------------------------------------------
// Engine setup
// ------------------------------------------------------------------
var repository = new SceneRepository(scenes);
var state = new GameState("hallway");
var evaluator = new ChoiceEvaluator();
var executor = new EffectExecutor();

// ------------------------------------------------------------------
// Render current scene
// ------------------------------------------------------------------
var scene = repository.Get(state.CurrentSceneId);

Console.WriteLine(scene.Text);

// Render choices (initial state: door locked)
foreach (var choice in scene.Choices)
{
    var enabled = evaluator.IsEnabled(choice, state, out var reason);

    Console.WriteLine(
        enabled
            ? $"{choice.Number}. {choice.Text}"
            : $"{choice.Number}. {choice.Text} ({reason})"
    );
}

// ------------------------------------------------------------------
// Simulate an effect (unlock the door)
// ------------------------------------------------------------------
executor.Execute(
    new[]
    {
        new EffectDefinition
        {
            Type = EffectType.SetFlag,
            Value = "doorUnlocked"
        }
    },
    state
);

Console.WriteLine();
Console.WriteLine("After unlocking door:");
Console.WriteLine();

// Re-render choices after mutation
foreach (var choice in scene.Choices)
{
    var enabled = evaluator.IsEnabled(choice, state, out var reason);

    Console.WriteLine(
        enabled
            ? $"{choice.Number}. {choice.Text}"
            : $"{choice.Number}. {choice.Text} ({reason})"
    );
}
