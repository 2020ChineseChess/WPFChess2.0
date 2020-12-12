using System.Windows;
using XIANG_QI_TRANSFER.Displayers;

namespace XIANG_QI_TRANSFER
{


    //这个是main函数
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindow mWindow = new MainWindow();
            mWindow.ShowDialog();
        }

    }
}
