using env0.adventure.Engine;
using env0.adventure.Model;
using env0.adventure.Runtime;

Console.WriteLine("env0.adventure booting");
Console.WriteLine();

// ------------------------------------------------------------------
// Temporary hardcoded scenes (JSON comes later)
// ------------------------------------------------------------------
var scenes = new List<SceneDefinition>
{
    new()
    {
        Id = "hallway",
        Text = "You are standing in a narrow hallway. A door leads to the kitchen.",
        IsEnd = false,
        Choices = new()
        {
            new ChoiceDefinition
            {
                Number = 1,
                Text = "Try the kitchen door",
                RequiresAll = new() { "doorUnlocked" },
                DisabledReason = "The door is locked.",
                Effects = new()
                {
                    new EffectDefinition { Type = EffectType.GotoScene, Value = "kitchen" }
                }
            },
            new ChoiceDefinition
            {
                Number = 2,
                Text = "Check the coat pocket for a key",
                RequiresNone = new() { "doorUnlocked" },
                DisabledReason = "You already have the key.",
                Effects = new()
                {
                    new EffectDefinition { Type = EffectType.SetFlag, Value = "doorUnlocked" }
                }
            }
        }
    },
    new()
    {
        Id = "kitchen",
        Text = "You are in a small kitchen. You made it in. The world remains stubbornly mundane.",
        IsEnd = true,
        Choices = new()
    }
};

// ------------------------------------------------------------------
// Engine setup
// ------------------------------------------------------------------
var repo = new SceneRepository(scenes);
var state = new GameState("hallway");
var evaluator = new ChoiceEvaluator();
var executor = new EffectExecutor();

// ------------------------------------------------------------------
// Main loop
// ------------------------------------------------------------------
while (true)
{
    var scene = repo.Get(state.CurrentSceneId);

    Console.WriteLine(scene.Text);
    Console.WriteLine();

    // End scene: render text only, no choices
    if (scene.IsEnd)
        break;

    // Render choices (always visible)
    foreach (var choice in scene.Choices.OrderBy(c => c.Number))
    {
        var enabled = evaluator.IsEnabled(choice, state, out var reason);

        Console.WriteLine(
            enabled
                ? $"{choice.Number}. {choice.Text}"
                : $"{choice.Number}. {choice.Text} ({reason})"
        );
    }

    Console.WriteLine();
    Console.Write("> ");

    var input = Console.ReadLine();
    Console.WriteLine();

    if (!int.TryParse(input, out var selectedNumber))
    {
        Console.WriteLine("Invalid input. Enter a number.");
        Console.WriteLine();
        continue;
    }

    var selectedChoice = scene.Choices.FirstOrDefault(c => c.Number == selectedNumber);
    if (selectedChoice is null)
    {
        Console.WriteLine("No such option.");
        Console.WriteLine();
        continue;
    }

    var isEnabled = evaluator.IsEnabled(selectedChoice, state, out var disabledReason);
    if (!isEnabled)
    {
        Console.WriteLine(disabledReason ?? "That option is not available.");
        Console.WriteLine();
        continue;
    }

    // Execute effects (mutates state and may change scene)
    executor.Execute(selectedChoice.Effects, state);

    // Spacer between turns
    Console.WriteLine();
}

// ------------------------------------------------------------------
// End
// ------------------------------------------------------------------
Console.WriteLine("Game ended.");
