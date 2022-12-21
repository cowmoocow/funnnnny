using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace funnnnny
{
    public static class ButtonExtensions
    {
        public static T GetTag<T>(this Button button)
        {
            if (button.Tag is T)
            {
                return (T)button.Tag;
            }

            return default;
        }
    }
}
