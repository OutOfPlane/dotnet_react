using System.Text.Json;
using Microsoft.EntityFrameworkCore;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {

        Type appCtxType = typeof(AppDbContext);
        var methodInfo = typeof(JsonSerializer)
            .GetMethods()
            .FirstOrDefault(m =>
                m.Name == "Deserialize" &&
                m.IsGenericMethodDefinition &&
                m.GetGenericArguments().Length == 1 &&
                m.GetParameters()[0].ParameterType == typeof(string));

        string[] files = Directory.GetFiles("DbSeed", "*.json");
        foreach (var ctxProperty in appCtxType.GetProperties())
        {
            if (ctxProperty.PropertyType.IsConstructedGenericType)
            {
                if (ctxProperty.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
                {
                    var path = "DbSeed/" + ctxProperty.Name + ".json";
                    if (File.Exists(path))
                    {
                        Console.WriteLine("Loading " + path + " into db");

                        Type fieldType = ctxProperty.PropertyType.GetGenericArguments()[0];
                        var constructedListType = typeof(List<>).MakeGenericType(fieldType);
                        var json = await File.ReadAllTextAsync(path);
                        var result = methodInfo?.MakeGenericMethod(constructedListType).Invoke(null, [json, null]);

                        var addRange = ctxProperty.PropertyType.GetMethods().FirstOrDefault(m =>
                            m.Name == "AddRange" &&
                            m.GetParameters()[0].ParameterType == typeof(IEnumerable<>).MakeGenericType(fieldType));

                        try
                        {
                            addRange?.Invoke(ctxProperty.GetValue(context), [result]);
                            await context.SaveChangesAsync();
                        }
                        catch (System.Exception)
                        {
                        }
                        
                    }

                }
            }
        }
        Console.WriteLine();
    }
}
