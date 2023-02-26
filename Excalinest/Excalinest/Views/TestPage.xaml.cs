// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Excalinest.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using System.Diagnostics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Excalinest.Views;
/// <summary>
/// A page to test execution func
/// </summary>
public sealed partial class TestPage : Page
{
    public TestViewModel ViewModel
    {
        get;
    }

    private readonly string Videojuego;
    public TestPage()
    {
        ViewModel = App.GetService<TestViewModel>();
        InitializeComponent();
        Videojuego = "RiotClientServices";
    }

    public void EjecutarVideojuego(object sender, RoutedEventArgs e)
    {
        Process.Start("Excalinest\\Assets\\Videojuegos\\", Videojuego);
    }
}
