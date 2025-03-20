using Avalonia.Controls;
using Avalonia.Platform;
using FFmpeg.AutoGen.Abstractions;
using System;
using Silk.NET.SDL;
using Silk.NET.Maths;
using FFmpeg.AutoGen.Bindings.DynamicallyLoaded;
using System.IO;
namespace SDL2_TargerTexture
{
    public unsafe partial class MainWindow : Avalonia.Controls.Window
    {
        Sdl sdl;
        public MainWindow()
        {
            InitializeComponent();
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                string current = Environment.CurrentDirectory;
                string probe = Path.Combine("FFmpeg", "bin", Environment.Is64BitProcess ? "x64" : "x86");

                while (current != null)
                {
                    var ffmpegBinaryPath = Path.Combine(current, probe);

                    if (Directory.Exists(ffmpegBinaryPath))
                    {
                        Console.WriteLine($"FFmpeg binaries found in: {ffmpegBinaryPath}");
                        DynamicallyLoadedBindings.LibrariesPath = ffmpegBinaryPath;
                        break;
                    }

                    current = Directory.GetParent(current)?.FullName;
                }
            }
            else
            {

                DynamicallyLoadedBindings.LibrariesPath = "/data/ffmpeg/lib";
            }


            DynamicallyLoadedBindings.Initialize();
            this.Loaded += MainWindow_Loaded;
            sdl = Sdl.GetApi();
        }
        Renderer* render;
        Texture* targetTexture;
        private void MainWindow_Loaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            sdl.Init(Sdl.InitVideo);
            var window = sdl.CreateWindowFrom((void*)dis.Handle);
            render = sdl.CreateRenderer(window, -1, (uint)RendererFlags.Accelerated);
            targetTexture = sdl.CreateTexture(render, Sdl.PixelformatBgra8888, (int)TextureAccess.Target, (int)dis.Bounds.Width, (int)dis.Bounds.Height);
            sdl.SetRenderTarget(render, targetTexture);
            InitImage();
        }

        private void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Console.WriteLine("Set BackgroundColor");
            sdl.SetRenderDrawColor(render, 0, 0, 0, 255);
            sdl.RenderClear(render);
            sdl.SetRenderTarget(render, null);
            sdl.RenderCopy(render, targetTexture, null, null);
            sdl.SetRenderTarget(render, targetTexture);
            sdl.RenderPresent(render);
            Console.WriteLine("Draw 1 Img");
            var m_pTexture = sdl.CreateTexture(render, Sdl.PixelformatBgrx8888, (int)TextureAccess.Streaming, ImgFrame->width, ImgFrame->height);
            sdl.UpdateTexture(m_pTexture, null, (void*)ImgFrame->data[0], ImgFrame->linesize[0]);
            sdl.RenderCopy(render, m_pTexture, null, new Rectangle<int>(0,0, (int)dis.Bounds.Width / 2, (int)dis.Bounds.Height / 2));
           sdl.SetRenderTarget(render, null);
           sdl.RenderCopy(render, targetTexture, null, null);
           sdl.SetRenderTarget(render, targetTexture);
           sdl.RenderPresent(render);
           sdl.DestroyTexture(m_pTexture);

            Console.WriteLine("Draw 2 Img");
            m_pTexture = sdl.CreateTexture(render, Sdl.PixelformatBgrx8888, (int)TextureAccess.Streaming, ImgFrame->width, ImgFrame->height);
            sdl.UpdateTexture(m_pTexture, null, (void*)ImgFrame->data[0], ImgFrame->linesize[0]);
            sdl.RenderCopy(render, m_pTexture, null, new Rectangle<int>((int)dis.Bounds.Width / 2, 0, (int)dis.Bounds.Width / 2, (int)dis.Bounds.Height / 2));
            sdl.SetRenderTarget(render, null);
            sdl.RenderCopy(render, targetTexture, null, null);
            sdl.SetRenderTarget(render, targetTexture);
            sdl.RenderPresent(render);
            sdl.DestroyTexture(m_pTexture);

            Console.WriteLine("Draw 3 Img");
            m_pTexture = sdl.CreateTexture(render, Sdl.PixelformatBgrx8888, (int)TextureAccess.Streaming, ImgFrame->width, ImgFrame->height);
            sdl.UpdateTexture(m_pTexture, null, (void*)ImgFrame->data[0], ImgFrame->linesize[0]);
            sdl.RenderCopy(render, m_pTexture, null, new Rectangle<int>(0, (int)dis.Bounds.Height / 2, (int)dis.Bounds.Width / 2, (int)dis.Bounds.Height / 2));
           sdl.SetRenderTarget(render, null);
           sdl.RenderCopy(render, targetTexture, null, null);
           sdl.SetRenderTarget(render, targetTexture);
           sdl.RenderPresent(render);
           sdl.DestroyTexture(m_pTexture);

            Console.WriteLine("Draw 4 Img");
            m_pTexture = sdl.CreateTexture(render, Sdl.PixelformatBgrx8888, (int)TextureAccess.Streaming, ImgFrame->width, ImgFrame->height);
            sdl.UpdateTexture(m_pTexture, null, (void*)ImgFrame->data[0], ImgFrame->linesize[0]);
            sdl.RenderCopy(render, m_pTexture, null, new Rectangle<int>((int)dis.Bounds.Width / 2, (int)dis.Bounds.Height / 2, (int)dis.Bounds.Width / 2, (int)dis.Bounds.Height / 2));
            sdl.SetRenderTarget(render, null);
            sdl.RenderCopy(render, targetTexture, null, null);
            sdl.SetRenderTarget(render, targetTexture);
            sdl.RenderPresent(render);
            sdl.DestroyTexture(m_pTexture);

        }

        AVFrame* ImgFrame;
        public void InitImage()
        {
            string filename = "Background.png";
            AVFormatContext* formatCtx = null;

            // 打开输入文件
            ffmpeg.avformat_open_input(&formatCtx, filename, null, null);


            AVPacket packet;
            // 获取解码器上下文
            var codecCtx = ffmpeg.avcodec_alloc_context3(null);

            ImgFrame = ffmpeg.av_frame_alloc();

            ffmpeg.avcodec_parameters_to_context(codecCtx, formatCtx->streams[0]->codecpar);
            // 获取视频解码器参数
            AVCodecParameters* codecParameters = formatCtx->streams[0]->codecpar;
            // 查找视频解码器
            AVCodec* codec = ffmpeg.avcodec_find_decoder(codecParameters->codec_id);
            Console.WriteLine(codecParameters->codec_id);
            ffmpeg.avcodec_open2(codecCtx, codec, null);
            // 循环读取每一帧
            ffmpeg.av_read_frame(formatCtx, &packet);
            ffmpeg.avcodec_send_packet(codecCtx, &packet);
            ffmpeg.avcodec_receive_frame(codecCtx, ImgFrame);
            AVPixelFormat aVPixelFormat = (AVPixelFormat)ImgFrame->format;

            ffmpeg.av_packet_unref(&packet); // 释放当前帧的资源
            // 关闭输入文件并释放资源
            ffmpeg.avformat_close_input(&formatCtx);
            ffmpeg.avcodec_free_context(&codecCtx);
        }
    }
    public class NativeEmbeddingControl : NativeControlHost
    {
        public IntPtr Handle { get; private set; }

        protected override IPlatformHandle CreateNativeControlCore(IPlatformHandle parent)
        {
            var handle = base.CreateNativeControlCore(parent);
            Handle = handle.Handle;
            Console.WriteLine($"Handle : {Handle}");
            return handle;
        }
    }
}