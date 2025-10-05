using Microsoft.Extensions.Logging;
using Microsoft.Maui.Handlers;

#if ANDROID
using Android.Graphics.Drawables;
using Android.Widget;
#endif

namespace kanbarugym
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");

                    fonts.AddFont("Ubuntu-Regular.ttf", "UbuntuRegular");
                    fonts.AddFont("Ubuntu-Bold.ttf", "UbuntuBold");
                });

#if ANDROID
            
            EntryHandler.Mapper.AppendToMapping("NoUnderline", (handler, view) =>
            {
                if (handler.PlatformView is EditText editText)
                {
                    editText.Background = null;
                }
            });

            
            PickerHandler.Mapper.AppendToMapping("NoUnderline", (handler, view) =>
            {
                handler.PlatformView.Background = new ColorDrawable(Android.Graphics.Color.Transparent);
            });
#endif

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
