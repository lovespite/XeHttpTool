using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace XeHttpTool.Native;

internal static partial class Win32
{
    public const int WH_KEYBOARD_LL = 13;
    public const int WH_MOUSE_LL = 14;

    public const uint WM_KEYDOWN = 0x0100;
    public const uint WM_KEYUP = 0x0101;
    public const uint WM_SYSKEYDOWN = 0x0104;
    public const uint WM_SYSKEYUP = 0x0105;

    public const uint WM_MOUSEMOVE = 0x0200;
    public const uint WM_LBUTTONDOWN = 0x0201;
    public const uint WM_LBUTTONUP = 0x0202;
    public const uint WM_RBUTTONDOWN = 0x0204;
    public const uint WM_RBUTTONUP = 0x0205;
    public const uint WM_MBUTTONDOWN = 0x0207;
    public const uint WM_MBUTTONUP = 0x0208;
    public const uint WM_MOUSEWHEEL = 0x020A;

    public static nint FindWindow(string name)
    {
        // "CASCADIA_HOSTING_WINDOW_CLASS"
        return FindWindowA(null, name);
    }

    [LibraryImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool ShowWindow(IntPtr hWnd, uint nCmdShow);
    public const uint SW_HIDE = 0;
    public const uint SW_SHOW = 5;

    [LibraryImport("user32.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf8)]
    public static partial nint FindWindowA(string? lpClassName, string lpWindowName);

    [LibraryImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool OpenClipboard(IntPtr hWndNewOwner);

    [LibraryImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool CloseClipboard();

    [LibraryImport("user32.dll", SetLastError = true)]
    public static partial IntPtr SetClipboardData(uint uFormat, IntPtr hMem);

    [LibraryImport("user32.dll", SetLastError = true)]
    public static partial IntPtr GetClipboardData(uint uFormat);

    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool EmptyClipboard();

    // CF_UNICODETEXT
    public const uint CF_UNICODETEXT = 13;

    public class Clipboard : Pixi2D.Components.IClipboardProvider
    {
        public bool SetText(string text)
        {
            // 尝试打开剪贴板。在 .NET 6+ 中，[SupportedOSPlatform("windows")] 是个好主意
            if (!OpenClipboard(IntPtr.Zero))
            {
                // 打开失败
                return false;
            }

            try
            {
                // 将C# string转换为非托管内存
                IntPtr hGlobal = Marshal.StringToHGlobalUni(text);
                if (hGlobal == IntPtr.Zero) return false;

                EmptyClipboard();

                // 设置剪贴板数据
                var handle = SetClipboardData(CF_UNICODETEXT, hGlobal);
                if (handle == IntPtr.Zero)
                {
                    // 设置失败，释放内存
                    Marshal.FreeHGlobal(hGlobal);
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                // 必须关闭剪贴板
                CloseClipboard();
                // 注意：SetClipboardData 成功后，系统拥有 hGlobal，
                // 你不应该释放它(Marshal.FreeHGlobal(hGlobal))。
                // 如果 SetClipboardData 失败，你需要释放它。
            }
        }

        public string GetText()
        {
            if (!OpenClipboard(IntPtr.Zero))
            {
                return string.Empty;
            }
            try
            {
                IntPtr handle = GetClipboardData(CF_UNICODETEXT);
                if (handle == IntPtr.Zero)
                {
                    return string.Empty;
                }
                // 将非托管内存转换为C# string
                string? text = Marshal.PtrToStringUni(handle);
                return text ?? string.Empty;
            }
            catch
            {
                return string.Empty;
            }
            finally
            {
                CloseClipboard();
            }
        }
    }

    internal static class LocalMachine
    {
        private static DirectoryInfo? _appLocalDataDir;
        public static DirectoryInfo GetOrCreateAppLocalDataDir()
        {
            _appLocalDataDir?.Refresh();
            if (_appLocalDataDir is not null) return _appLocalDataDir;

            var localDataDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var dir = Path.Combine(localDataDir, nameof(XeHttpTool));
            return _appLocalDataDir = Directory.CreateDirectory(dir);
        }
    }

}

