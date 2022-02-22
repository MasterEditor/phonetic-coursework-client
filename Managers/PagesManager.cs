using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Coursework_Client.Managers
{
    static class PagesManager
    {
        private static Frame frame;
        private static Frame innerFrame;

        public static void RegisterFrame(Frame frame)
        {
            PagesManager.frame = frame;
        }

        public static void RegisterInnerFrame(Frame frame)
        {
            PagesManager.innerFrame = frame;
        }

        public static void OpenPage(Page page)
        {
            if (page is null) throw new NullReferenceException();
            frame.Navigate(page);
        }

        public static void OpenInnerPage(Page page)
        {
            if (page is null) throw new NullReferenceException();
            innerFrame.Navigate(page);
        }

        public static void GoBack()
        {
            frame.GoBack();
        }
    }
}
