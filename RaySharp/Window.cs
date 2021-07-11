﻿using System;
using System.Numerics;
using System.Runtime.InteropServices;

namespace RaySharp
{
    public static class Window
    {
        [DllImport(Constants.dllName, CharSet = CharSet.Ansi)]
        private static extern void InitWindow(int width, int height, string title);
        [DllImport(Constants.dllName)]
        private static extern void CloseWindow();

        [DllImport(Constants.dllName)]
        private static extern bool WindowShouldClose();

        [DllImport(Constants.dllName)]
        private static extern bool IsWindowReady();
        [DllImport(Constants.dllName)]
        private static extern bool IsWindowFullscreen();
        [DllImport(Constants.dllName)]
        private static extern bool IsWindowHidden();
        [DllImport(Constants.dllName)]
        private static extern bool IsWindowMinimized();
        [DllImport(Constants.dllName)]
        private static extern bool IsWindowMaximized();
        [DllImport(Constants.dllName)]
        private static extern bool IsWindowFocused();
        [DllImport(Constants.dllName)]
        private static extern bool IsWindowResized();

        [DllImport(Constants.dllName)]
        private static extern int GetScreenWidth();
        [DllImport(Constants.dllName)]
        private static extern int GetScreenHeight();
        [DllImport(Constants.dllName)]
        private static extern void SetWindowSize(int width, int height);
        [DllImport(Constants.dllName)]
        private static extern void SetWindowMinSize(int width, int height);                           // Set window minimum dimensions (for FLAG_WINDOW_RESIZABLE)

        [DllImport(Constants.dllName)]
        private static extern bool IsWindowState(uint flag);
        [DllImport(Constants.dllName)]
        private static extern void SetWindowState(uint flags);
        [DllImport(Constants.dllName)]
        private static extern void ClearWindowState(uint flags);

        [DllImport(Constants.dllName)]
        private static extern void ToggleFullscreen();
        [DllImport(Constants.dllName)]
        private static extern void MaximizeWindow();
        [DllImport(Constants.dllName)]
        private static extern void MinimizeWindow();
        [DllImport(Constants.dllName)]
        private static extern void RestoreWindow();

        [DllImport(Constants.dllName, CharSet = CharSet.Ansi)]
        private static extern void SetWindowTitle(string title);

        [DllImport(Constants.dllName)]
        private static extern void SetWindowPosition(int x, int y);
        [DllImport(Constants.dllName)]
        private static extern Vector2 GetWindowPosition();

        [DllImport(Constants.dllName, CharSet = CharSet.Ansi)]
        private static extern void SetClipboardText(string text);
        [DllImport(Constants.dllName, CharSet = CharSet.Ansi)]
        private static extern string GetClipboardText();

        [DllImport(Constants.dllName)]
        private static extern void SetWindowMonitor(int monitor);
        [DllImport(Constants.dllName)]
        private static extern int GetCurrentMonitor();

        [DllImport(Constants.dllName)]
        private static extern IntPtr GetWindowHandle();

        [DllImport(Constants.dllName)]
        private static extern Vector2 GetWindowScaleDPI();

        /*[DllImport(Constants.dllName)]
        private static extern void SetWindowIcon(Image image);                                        // Set icon for window (only PLATFORM_DESKTOP)*/


        public enum Flags : uint
        {
            /// <summary>
            /// Set to try enabling V-Sync on GPU
            /// </summary>
            VSYNC_HINT = 0x00000040,
            /// <summary>
            /// Set to run program in fullscreen
            /// </summary>
            FULLSCREEN_MODE = 0x00000002,
            /// <summary>
            /// Set to allow resizable window
            /// </summary>
            WINDOW_RESIZABLE = 0x00000004,
            /// <summary>
            /// Set to disable window decoration (frame and buttons)
            /// </summary>
            WINDOW_UNDECORATED = 0x00000008,
            /// <summary>
            /// Set to hide window
            /// </summary>
            WINDOW_HIDDEN = 0x00000080,
            /// <summary>
            /// Set to minimize window (iconify)
            /// </summary>
            WINDOW_MINIMIZED = 0x00000200,
            /// <summary>
            /// Set to maximize window (expanded to monitor)
            /// </summary>
            WINDOW_MAXIMIZED = 0x00000400,
            /// <summary>
            /// Set to window non focused
            /// </summary>
            WINDOW_UNFOCUSED = 0x00000800,
            /// <summary>
            /// Set to window always on top
            /// </summary>
            WINDOW_TOPMOST = 0x00001000,
            /// <summary>
            /// Set to allow windows running while minimized
            /// </summary>
            WINDOW_ALWAYS_RUN = 0x00000100,
            /// <summary>
            /// Set to allow transparent framebuffer
            /// </summary>
            WINDOW_TRANSPARENT = 0x00000010,
            /// <summary>
            /// Set to support HighDPI
            /// </summary>
            WINDOW_HIGHDPI = 0x00002000,
            /// <summary>
            /// Set to try enabling MSAA 4X
            /// </summary>
            MSAA_4X_HINT = 0x00000020,
            /// <summary>
            /// Set to try enabling interlaced video format (for V3D)
            /// </summary>
            INTERLACED_HINT = 0x00010000
        }

        private static string _title;
        private static bool _initialized;
        private static Vector2 _minSize;

        /// <summary>
        /// Check if KEY_ESCAPE pressed or Close icon pressed
        /// </summary>
        public static bool ShouldClose => WindowShouldClose();

        /// <summary>
        /// Check if window has been initialized successfully
        /// </summary>
        public static bool Ready => IsWindowReady();

        /// <summary>
        /// Get window scale DPI factor
        /// </summary>
        public static Vector2 ScaleDPI => GetWindowScaleDPI();

        /// <summary>
        /// Check if window is currently hidden
        /// </summary>
        public static bool Hidden => IsWindowHidden();

        /// <summary>
        /// Check if window is currently focused
        /// </summary>
        public static bool Focused => IsWindowFocused();

        /// <summary>
        /// Check if window has been resized last frame
        /// </summary>
        public static bool Resized => IsWindowResized();

        /// <summary>
        /// Check and set if window is in fullscreen
        /// </summary>
        public static bool Fullscreen
        {
            get => IsWindowFullscreen();
            set
            {
                if (Fullscreen != value)
                    ToggleFullscreen();
            }
        }

        public static Vector2 Size
        {
            get => new Vector2(GetScreenWidth(), GetScreenHeight());
            set => SetWindowSize((int)value.X, (int)value.Y);
        }

        /// <summary>
        /// Check and set if window is minimized
        /// </summary>
        public static bool Minimized
        {
            get => IsWindowMinimized();
            set
            {
                if (value)
                    MinimizeWindow();
                else if (Minimized)
                    RestoreWindow();
            }
        }

        /// <summary>
        /// Check and set if window is maximized
        /// </summary>
        public static bool Maximized
        {
            get => IsWindowMaximized();
            set
            {
                if (value)
                    MaximizeWindow();
                else if (Maximized)
                    RestoreWindow();
            }
        }

        /// <summary>
        /// Get/Set current window title
        /// </summary>
        public static string Title
        {
            get => _title;
            set => SetWindowTitle(value);
        }

        /// <summary>
        /// Get/Set window position on screen 
        /// </summary>
        public static Vector2 Position
        {
            get => GetWindowPosition();
            set => SetWindowPosition((int)value.X, (int)value.Y);
        }

        /// <summary>
        /// Get/Set clipboard text content
        /// </summary>
        public static string Clipboard
        {
            get => GetClipboardText();
            set => SetClipboardText(value);
        }

        /// <summary>
        /// Get/Set Monitor on which the window is displayed
        /// </summary>
        public static int Monitor
        {
            get => GetCurrentMonitor();
            set => SetWindowMonitor(value);
        }

        /// <summary>
        /// Initialize window and OpenGL context
        /// </summary>
        /// <param name="width">Width of the window</param>
        /// <param name="height">Height of the window</param>
        /// <param name="title">title of the window</param>
        public static void Init(int width, int height, string title)
        {
            if (_initialized)
                return;
            InitWindow(width, height, title);
            _title = title;
            _initialized = true;
        }

        /// <summary>
        /// Close window and unload OpenGL context
        /// </summary>
        public static void Close()
        {
            if (_initialized)
                CloseWindow();
            _initialized = false;
        }

        /// <summary>
        /// Check if one specific window flag is enabled
        /// </summary>
        /// <param name="flag">Flag to check</param>
        /// <returns></returns>
        public static bool IsState(Flags flag) => IsWindowState((uint)flag);
        /// <summary>
        /// Set window configuration state using flags
        /// </summary>
        /// <param name="flags">Flags to set</param>
        public static void SetState(Flags flags) => SetWindowState((uint)flags);
        /// <summary>
        /// Clear window configuration state flags
        /// </summary>
        /// <param name="flags">Flags to clear the window with</param>
        public static void ClearState(Flags flags) => ClearWindowState((uint)flags);

        public static Vector2 MinSize
        {
            get => _minSize;
            set
            {
                SetWindowMinSize((int)value.X, (int)value.Y);
                _minSize = value;
            }
        }
    }
}
