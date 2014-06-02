using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragDropPhoneApp.Helpers
{
    using System.Windows.Controls;
    using System.Windows.Data;

    using Microsoft.Phone.Shell;

    public static  class Indicator
  {
      public static void setLoadingIndicator(Page page, string text)
      {
          var progressIndicator = SystemTray.ProgressIndicator;
          if (progressIndicator != null)
          {
              return;
          }

          progressIndicator = new ProgressIndicator();

          SystemTray.SetProgressIndicator(page, progressIndicator);

          Binding binding = new Binding("IsLoading") { Source = page.DataContext };
          BindingOperations.SetBinding(progressIndicator, ProgressIndicator.IsVisibleProperty, binding);

          binding = new Binding("IsLoading") { Source = page.DataContext };
          BindingOperations.SetBinding(progressIndicator, ProgressIndicator.IsIndeterminateProperty, binding);

          progressIndicator.Text = text;
      }
    }
}
