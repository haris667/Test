namespace Mirra.Application.Configs
{
    public interface ITimeDisplayConfig
    {
        string DisplayFormat { get; }
        string[] ParseFormats { get; }
    }
}
