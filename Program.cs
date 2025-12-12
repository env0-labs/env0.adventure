using System.Text.Json;
using System.Text.Json.Serialization;
using env0.adventure.Engine;
using env0.adventure.Model;
using env0.adventure.Runtime;

Console.WriteLine("env0.adventure booting");
Console.WriteLine();

// ------------------------------------------------------------------
// Load story from JSON
// ------------------------------------------------------------------
var storyPath = Path.Combine(AppContext.BaseDirectory, "story.json");

if (!File.Exists(storyPath))
    throw new InvalidOperationException($"Story file not found: {storyPath}");

var json = File.ReadAllText(storyPath);

var story = JsonSerializer.Deserialize<StoryDefinition>(
    json,
    new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true,
        ReadCommentHandling = JsonCommentHandling.Skip,
        AllowTrailingCommas = true,
        Converters = { new JsonStringEnumConverter() }
    }
) ?? throw new InvalidOperationException("Story file could not be parsed.");



// ------------------------------------------------------------------
// Engine setup
// ------------------------------------------------------------------
var repo = new SceneRepository(story);
var state = new GameState(repo.StartSceneId);
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
