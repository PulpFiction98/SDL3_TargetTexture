
using Avalonia.Controls;


using Avalonia.Platform;

using System;
using SDL3;
using FFmpeg.AutoGen.Bindings.DynamicallyLoaded;
using System.IO;
using FFmpeg.AutoGen.Abstractions;
using static SDL3.SDL;
using System.Threading;
using Avalonia.Rendering;

namespace Demo
{
    public unsafe partial class MainWindow : Avalonia.Controls.Window
    {
        private void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Console.WriteLine("Set BackgroundColor");
            SDL.SetRenderDrawColor(render, 0, 0, 0, 255);
            SDL.RenderClear(render);
            SDL.SetRenderTarget(render, nint.Zero);
            SDL.RenderTexture(render, targetTexture, nint.Zero, nint.Zero);
            SDL.SetRenderTarget(render, targetTexture);
            SDL.RenderPresent(render);
       

            Console.WriteLine("Draw 1 Img");
           // Thread.Sleep(5);

            var m_pTexture = SDL.CreateTexture(render, SDL.PixelFormat.BGRX8888, SDL.TextureAccess.Streaming, ImgFrame->width, ImgFrame->height);
            SDL.UpdateTexture(m_pTexture, nint.Zero, (nint)ImgFrame->data[0], ImgFrame->linesize[0]);
            SDL.RenderTexture(render, m_pTexture, nint.Zero, new SDL.FRect() { X = 0, Y = 0, W = (int)dis.Bounds.Width / 2, H = (int)dis.Bounds.Height / 2 });
            SDL.SetRenderTarget(render, nint.Zero);
            SDL.RenderTexture(render, targetTexture, nint.Zero, nint.Zero);
            SDL.SetRenderTarget(render, targetTexture);
            SDL.RenderPresent(render);
            //SDL.SetRenderDrawColor(render, 0,255 , 0, 255);
            //SDL.RenderClear(render);
           
            SDL.DestroyTexture(m_pTexture);

            Console.WriteLine("Draw 2 Img");
       //     Thread.Sleep(5);
            m_pTexture = SDL.CreateTexture(render, SDL.PixelFormat.BGRX8888, SDL.TextureAccess.Streaming, ImgFrame->width, ImgFrame->height);
            SDL.UpdateTexture(m_pTexture, nint.Zero, (nint)ImgFrame->data[0], ImgFrame->linesize[0]);
            SDL.RenderTexture(render, m_pTexture, nint.Zero, new SDL.FRect() { X = (int)dis.Bounds.Width / 2, Y = 0, W = (int)dis.Bounds.Width / 2, H = (int)dis.Bounds.Height / 2 });
            SDL.SetRenderTarget(render, nint.Zero);
            SDL.RenderTexture(render, targetTexture, nint.Zero, nint.Zero);
            SDL.SetRenderTarget(render, targetTexture);
            SDL.RenderPresent(render);
            //SDL.SetRenderDrawColor(render, 0, 255, 0, 255);

            //SDL.RenderClear(render);
   
            SDL.DestroyTexture(m_pTexture);

            Console.WriteLine("Draw 3 Img");
        //    Thread.Sleep(5);
            m_pTexture = SDL.CreateTexture(render, SDL.PixelFormat.BGRX8888, SDL.TextureAccess.Streaming, ImgFrame->width, ImgFrame->height);
            SDL.UpdateTexture(m_pTexture, nint.Zero, (nint)ImgFrame->data[0], ImgFrame->linesize[0]);
            SDL.RenderTexture(render, m_pTexture, nint.Zero, new SDL.FRect() { X = 0, Y = (int)dis.Bounds.Height / 2, W = (int)dis.Bounds.Width / 2, H = (int)dis.Bounds.Height / 2 });
            SDL.SetRenderTarget(render, nint.Zero);
            SDL.RenderTexture(render, targetTexture, nint.Zero, nint.Zero);
            SDL.SetRenderTarget(render, targetTexture);
            SDL.RenderPresent(render);
            //SDL.SetRenderDrawColor(render, 0, 255, 0, 255);
            //SDL.RenderClear(render);
      
            SDL.DestroyTexture(m_pTexture);

            Console.WriteLine("Draw 4 Img");
        //    Thread.Sleep(5);
            m_pTexture = SDL.CreateTexture(render, SDL.PixelFormat.BGRX8888, SDL.TextureAccess.Streaming, ImgFrame->width, ImgFrame->height);
            SDL.UpdateTexture(m_pTexture, nint.Zero, (nint)ImgFrame->data[0], ImgFrame->linesize[0]);
            SDL.RenderTexture(render, m_pTexture, nint.Zero, new SDL.FRect() { X = (int)dis.Bounds.Width / 2, Y = (int)dis.Bounds.Height / 2, W = (int)dis.Bounds.Width / 2, H = (int)dis.Bounds.Height / 2 });
            SDL.SetRenderTarget(render, nint.Zero);
            SDL.RenderTexture(render, targetTexture, nint.Zero, nint.Zero);
            SDL.SetRenderTarget(render, targetTexture);
            SDL.RenderPresent(render);
            //SDL.SetRenderDrawColor(render, 0, 255, 0, 255);
            //SDL.RenderClear(render);
     
            SDL.DestroyTexture(m_pTexture);

            

        }
        public void Init()
        {
            SDL.Init(SDL.InitFlags.Video);
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
            ThreadQueue.Start();
        }
        public MainWindow()
        {
            InitializeComponent();
            Init();
            this.Loaded += MainWindow_Loaded;



        }
        nint window;
        nint render;
        nint targetTexture;
        private void MainWindow_Loaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            
          
            uint prop = SDL.CreateProperties();
            SDL.SetPointerProperty(prop, SDL.Props.WindowCreateWin32HWNDPointer, dis.Handle);
            window = SDL.CreateWindowWithProperties(prop);
            render = SDL.CreateRenderer(window, null);
            SDL.SetRenderVSync(render, -1);
            targetTexture = SDL.CreateTexture(render, SDL.PixelFormat.BGRA8888, TextureAccess.Target, (int)dis.Bounds.Width, (int)dis.Bounds.Height);
            SDL.SetRenderTarget(render, targetTexture);
            InitImage();

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