namespace Hawk;

public interface IHost
{
    /// <summary>
    /// Writes output o the host without a newline.
    /// </summary>
    /// <param name="s">The <c>string</c> to output.</param>
    void Write(string s);

    /// <summary>
    /// Writes output to the host with a newline appended.
    /// </summary>
    /// <param name="s">The <c>string</c> to output.</param>
    void WriteLine(string s);
}