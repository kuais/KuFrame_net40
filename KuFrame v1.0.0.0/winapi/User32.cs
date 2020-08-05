﻿using System;
using System.Runtime.InteropServices;

namespace Ku.winapi
{
    public class User32
    {
        public const int GWL_STYLE = (-16);
        public const int GWL_EXSTYLE = (-20);

        public const int WS_CAPTION = 0xC00000;         //带标题栏的窗口
        public const int WS_MAXIMIZEBOX = 0x10000;      //带最大化按钮的窗口
        public const int WS_MINIMIZEBOX = 0x20000;      //带最小化按钮的窗口
        public const int WS_SYSMENU = 0x80000;          //带系统菜单的窗口
        public const int WS_CLIPSIBLINGS = 0x4000000;   //不重绘层叠子窗口
        public const int WS_CLIPCHILDREN = 0x2000000;   //绘图时排子窗口区域
        public const int WS_OVERLAPPED = 0x0;          //具有标题栏和边框的层叠窗口
        public const int WS_THICKFRAME = 0x40000;       //具有可调边框
        //WS_OVERLAPPEDWINDOW具有标题栏、窗口菜单、可调边框和最大化、最小化按钮的窗口
        public const int WS_OVERLAPPEDWINDOW = (WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX);
        public const int WS_GROUP = 0x20000;               //指定一组控制的第一个控制
        public const uint WS_POPUP = 0x80000000;         //弹出式窗口
        public const int WS_BORDER = 0x800000;          //单边框窗口
        public const uint WS_POPUPWINDOW = (WS_POPUP | WS_BORDER | WS_SYSMENU); //具有单边框、标题栏菜单的弹出式窗口
        public const int WS_MINIMIZE = 0x20000000;      //窗口最小化
        public const int WS_VISIBLE = 0x10000000;       //窗口可见
        public const int WS_DISABLED = 0x8000000;       //窗口被禁用
        public const int WS_MAXIMIZE = 0x1000000;       //窗口最大化
        public const int WS_DLGFRAME = 0x400000;        //对话框边框风格
        public const int WS_VSCROLL = 0x200000;         //具有垂直滚动条
        public const int WS_HSCROLL = 0x100000;         //具有水平滚动条
        public const int WS_TABSTOP = 0x10000;         //具有TAB键控制
        public const int WS_CHILD = 0x40000000;
        public const int WS_CHILDWINDOW = WS_CHILD;        //具有子窗口

        //扩展风格
        public const int WS_EX_WINDOWEDGE = 0x100;         //窗口具有凸起的3D边框
        public const int WS_EX_CLIENTEDGE = 0x200;         //窗口具有阴影边界
        public const int WS_EX_TOOLWINDOW = 0x80;          //小标题工具窗口
        public const int WS_EX_TOPMOST = 0x8;              //窗口总在顶层
        public const int WS_EX_OVERLAPPEDWINDOW = (WS_EX_WINDOWEDGE | WS_EX_CLIENTEDGE); //WS_EX-CLIENTEDGE和WS_EX_WINDOWEDGE的组合
        public const int WS_EX_PALETTEWINDOW = (WS_EX_WINDOWEDGE | WS_EX_TOOLWINDOW | WS_EX_TOPMOST); //WS_EX_WINDOWEDGE和WS_EX_TOOLWINDOW和WS_EX_TOPMOST的组合
        public const int WS_EX_DLGMODALFRAME = 0x1;        //带双边的窗口
        public const int WS_EX_NOPARENTNOTIFY = 0x4;       //窗口在创建和销毁时不向父窗口发送WM_PARENTNOTIFY消息
        public const int WS_EX_TRANSPARENT = 0x20;         //窗口透眀
        public const int WS_EX_MDICHILD = 0x40;            //MDI子窗口
        public const int WS_EX_CONTEXTHELP = 0x400;        //标题栏包含问号联机帮助按钮
        public const int WS_EX_RIGHT = 0x1000;             //窗口具有右对齐属性
        public const int WS_EX_RTLREADING = 0x2000;        //窗口文本自右向左显示
        public const int WS_EX_LEFTSCROLLBAR = 0x4000;     //标题栏在客户区的左边
        public const int WS_EX_CONTROLPARENT = 0x10000;     //允许用户使用Tab键在窗口的子窗口间搜索
        public const int WS_EX_STATICEDGE = 0x20000;        //为不接受用户输入的项创建一个三维边界风格
        public const int WS_EX_APPWINDOW = 0x40000;         //在任务栏上显示顶层窗口的标题按钮
        public const int WS_EX_LAYERED = 0x80000;           //窗口具有透眀属性(Win2000)以上
        public const int WS_EX_NOINHERITLAYOUT = 0x100000; //窗口布局不传递给子窗口(Win2000)以上
        public const int WS_EX_LAYOUTRTL = 0x400000;        //水平起点在右边的窗口
        public const int WS_EX_NOACTIVATE = 0x8000000;      //窗口不会变成前台窗口(Win2000)以上
        public const int WS_EX_LEFT = 0x0;                 //窗口具有左对齐属性
        public const int WS_EX_LTRREADING = 0x0;           //窗口文本自左向右显示
        public const int WS_EX_RIGHTSCROLLBAR = 0x0;       //垂直滚动条在窗口的右边界
        public const int WS_EX_ACCEPTFILES = 0x10;         //接受文件拖曳
        public const int WS_EX_COMPOSITED = 0x2000000;      //窗体所有子窗口使用双缓冲从低到高绘制(XP)

        [DllImport("user32.dll", SetLastError = true)]
        public static extern long SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
        [DllImport("user32.dll", EntryPoint = "GetWindowLong", CharSet = CharSet.Auto)]
        public static extern IntPtr GetWindowLongPtr32(HandleRef hWnd, int nlndex);
        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr", CharSet = CharSet.Auto)]
        public static extern IntPtr GetWindowLongPtr64(HandleRef hWnd, int nlndex);
        [DllImport("user32.dll", EntryPoint = "SetWindowLong", CharSet = CharSet.Auto)]
        public static extern IntPtr SetWindowLongPtr32(HandleRef hWnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", CharSet = CharSet.Auto)]
        public static extern IntPtr SetWindowLongPtr64(HandleRef hWnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool MoveWindow(IntPtr hwnd, int x, int y, int cx, int cy, bool repaint);

        public static IntPtr GetWindowLong(HandleRef hWnd, int nIndex)
        {
            return IntPtr.Size == 4 ?
                GetWindowLongPtr32(hWnd, nIndex) :
                GetWindowLongPtr64(hWnd, nIndex);
        }
        public static IntPtr SetWindowLong(HandleRef hWnd, int nIndex, int dwNewLong)
        {
            return IntPtr.Size == 4 ?
                SetWindowLongPtr32(hWnd, nIndex, dwNewLong) :
                SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
        }
    }
}
