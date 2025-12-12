using env0.adventure.Model;

namespace env0.adventure.Engine;

public sealed class SceneRepository
{
    private readonly Dictionary<string, SceneDefinition> _scenes;

    public SceneRepository(IEnumerable<SceneDefinition> scenes)
    {
        _scenes = scenes.ToDictionary(
            s => s.Id,
            s => s
        );
    }

    public SceneDefinition Get(string sceneId)
    {
        if (!_scenes.TryGetValue(sceneId, out var scene))
            throw new InvalidOperationException($"Scene not found: {sceneId}");

        return scene;
    }
}