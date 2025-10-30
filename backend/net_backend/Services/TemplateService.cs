using RazorLight;

public class TemplateService
{
    private readonly RazorLightEngine _engine;

    public TemplateService()
    {
        string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates");

        _engine = new RazorLightEngineBuilder()
            .UseFileSystemProject(templatePath)
            .UseMemoryCachingProvider()
            .Build();
    }

    public async Task<string> RenderTemplateAsync<T>(string viewname, T model)
    {
        return await _engine.CompileRenderAsync<T>(viewname + ".cshtml", model);
    }
}