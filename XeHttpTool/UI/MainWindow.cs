using D2DWindow;
using Pixi2D.Core;
using Pixi2D.Extensions;
using SharpDX.Direct2D1;
using System.Drawing;
using XeHttpTool.Model;

namespace XeHttpTool.UI;

/*
 
Lifecycle of the window:

OnDeviceReady -> OnLoad -> OnPaint (repeatedly) -> OnClosing -> OnUnload -> Dispose
 
*/

internal class MainWindow : Direct2D1Window
{
    private static readonly string s_WindowClassName = "XeHttpToolMainWindow_{3F54C0F2-5B4A-402F-B791-31EAB471DE7F}";
    public override string WindowClassName => s_WindowClassName;

    private readonly HttpClient m_HttpClient;

    public MainWindow(HttpClient client) : base(title: "XeHttpTool", width: 800, height: 600)
    {
        m_HttpClient = client;
        BackgroundColor = Color.White.ToRawColor4();
    }

    #region Window lifecycle methods

    protected sealed override void OnDeviceReady(RenderTarget target)
    {
        m_Stage.SetCachedRenderTarget(target);
    }

    protected sealed override void OnLoad()
    {
        InitializePixi2DComponents();
    }

    protected sealed override void OnPaint(RenderTarget target, float deltaTimeInSeconds)
    {
        m_Stage.Update(deltaTimeInSeconds);
        m_Stage.Render(target);
    }

    protected sealed override void OnClosing(ref bool cancel)
    {
        cancel = false;
    }

    protected sealed override void OnUnload()
    {
        base.OnUnload();
    }

    #endregion

    #region HID Event Dispatching

    protected override void OnMouseMove(Point p)
    {
        m_Stage.DispatchMouseMove(p);
        base.OnMouseMove(p);
    }

    protected override void OnMouseDown(Point p, MouseButton button)
    {
        m_Stage.DispatchMouseDown(p, (int)button);
        base.OnMouseDown(p, button);
    }

    protected override void OnMouseUp(Point p, MouseButton button)
    {
        m_Stage.DispatchMouseUp(p, (int)button);
        base.OnMouseUp(p, button);
    }

    protected override void OnMouseWheel(int delta)
    {
        m_Stage.DispatchMouseWheel(MousePosition, delta);
        base.OnMouseWheel(delta);
    }

    protected override void OnKeyDown(int key, KeyModifiers modifiers)
    {
        m_Stage.DispatchKeyDown(key, modifiers.Control(), modifiers.Alt(), modifiers.Shift());
        base.OnKeyDown(key, modifiers);
    }

    protected override void OnKeyUp(int key, KeyModifiers modifiers)
    {
        m_Stage.DispatchKeyUp(key, modifiers.Control(), modifiers.Alt(), modifiers.Shift());
        base.OnKeyUp(key, modifiers);
    }

    protected override void OnKeyPress(char keyChar, KeyModifiers modifiers)
    {
        m_Stage.DispatchKeyPress(keyChar);
        base.OnKeyPress(keyChar, modifiers);
    }

    #endregion

    public override void Dispose()
    {
        base.Dispose();
    }

    #region Pixi2D Components

    private readonly Stage m_Stage = new();
    private readonly Text.Factory m_TextFactory = new()
    {
        FontFamily = "Segoe UI",
        FontSize = 14,
        FillColor = Color.Black.ApplyAlpha(0.93f),
    };
    private Pixi2D.Controls.TextBox m_TextBox1 = null!;

    private void InitializePixi2DComponents()
    {
        m_Stage.ClipboardProvider = new Native.Win32.Clipboard();
        Pixi2D.Components.MessageBox.TextFactory = m_TextFactory;
        Pixi2D.Components.MessageBox.DefaultStage = m_Stage;

        var t = m_TextFactory.Create(" XeHttpTool v1.0(build-20260101.1)", fontSize: 11f, fillColor: Color.Gray);
        m_Stage.AddChild(t);

        var btn1 = new Pixi2D.Controls.Button(m_TextFactory.Create("测试"))
        {
            X = 10,
            Y = 18,
        };
        btn1.OnButtonClick += Btn1_OnButtonClick;

        m_TextBox1 = new Pixi2D.Controls.TextBox(m_TextFactory, 400, 400)
        {
            X = 10,
            Y = 58,
            Text = "Hello, XeHttpTool!",
            Multiline = true,
        };

        m_Stage.AddChildren(btn1, m_TextBox1);
        m_Stage.Resize(Width, Height);
    }

    private async void Btn1_OnButtonClick(Pixi2D.Controls.Button obj)
    {
        //var ws = XeWorkspace.Create("test", "D:\\teset.xeworkspace.json");
        //ws.Collections = [
        //    new XeCollection {
        //        Name = "c1",
        //        Requests = [
        //            new XeRequest {
        //                Name = "req1",
        //                Method = XeRequest.Methods.Get,
        //                Url = "https://httpbin.org/get",
        //            },
        //            new XeRequest {
        //                Name = "req2",
        //                Method = XeRequest.Methods.Get,
        //                Url = "https://httpbin.org/get",
        //            },
        //        ],
        //    },
        //    new XeCollection {
        //        Name = "c2",
        //        Requests = [
        //            new XeRequest {
        //                Name = "req1",
        //                Method = XeRequest.Methods.Get,
        //                Url = "https://httpbin.org/get",
        //            },
        //            new XeRequest {
        //                Name = "req2",
        //                Method = XeRequest.Methods.Get,
        //                Url = "https://httpbin.org/get",
        //            },
        //        ],
        //    },
        //];

        //ws.Save();
        var ws = new FileInfo("D:\\teset.xeworkspace.json").ReadAsXeWorkspace();
        m_TextBox1.Text = ws.Name;
    }

    protected override void OnResize(int width, int height)
    {
        m_Stage.Resize(width, height);
        base.OnResize(width, height);
    }

    #endregion
}
